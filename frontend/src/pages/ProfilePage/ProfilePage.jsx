import { useState } from 'react';
import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
import styles from './ProfilePage.module.css';
import { useAuth } from '../../contexts/AuthContext';

export default function ProfilePage() {
  const { user } = useAuth(); // для получения текущей роли
  const [profile, loading, error, refetch] = useApi('/readers/profile');
  const [libraries] = useApi('/libraries');
  const [editMode, setEditMode] = useState(false);
  const [form, setForm] = useState({ fullName: '', email: '' });

  const startEdit = () => {
    if (profile) {
      setForm({ fullName: profile.fullName, email: profile.email });
      setEditMode(true);
    }
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      await api.put('/readers/profile', form);
      alert('Профиль обновлён');
      setEditMode(false);
      refetch();
    } catch (err) {
      alert(err.response?.data?.error || 'Ошибка');
    }
  };

  if (loading) return <p className={styles.loading}>Загрузка профиля...</p>;
  if (error) return <p className={styles.error}>Ошибка загрузки</p>;
  if (!profile) return <p className={styles.loading}>Профиль не найден</p>;

  const userLibrary = Array.isArray(libraries)
    ? libraries.find((l) => l.id === profile.libraryId)
    : null;

  // Инициалы для аватара
  const initials = profile.fullName
    .split(' ')
    .map((word) => word[0])
    .slice(0, 2)
    .join('')
    .toUpperCase();

  return (
    <div className={styles.container}>
      <h1 className={styles.heading}>Мой уголок</h1>

      {editMode ? (
        <form onSubmit={handleSave} className={styles.editForm}>
          <label className={styles.label}>Имя</label>
          <input
            value={form.fullName}
            onChange={(e) => setForm({ ...form, fullName: e.target.value })}
            required
          />
          <label className={styles.label}>Email</label>
          <input
            type="email"
            value={form.email}
            onChange={(e) => setForm({ ...form, email: e.target.value })}
            required
          />
          <div className={styles.editActions}>
            <button type="submit" className={styles.saveBtn}>
              Сохранить
            </button>
            <button
              type="button"
              onClick={() => setEditMode(false)}
              className={styles.cancelBtn}
            >
              Отмена
            </button>
          </div>
        </form>
      ) : (
        <div className={styles.layout}>
          {/* Левая колонка — аватар и основная информация */}
          <div className={styles.leftColumn}>
            <div className={styles.avatar}>{initials}</div>
            <h2 className={styles.fullName}>{profile.fullName}</h2>
            <span className={styles.roleBadge}>
              {profile.role === 'Librarian' ? '📚 Библиотекарь' : '✒️ Читатель'}
            </span>
            <p className={styles.email}>
              <span className={styles.label}>Email: </span>
              {profile.email}
            </p>
            <button onClick={startEdit} className={styles.editBtn}>
              Редактировать профиль
            </button>
          </div>

          {/* Правая колонка — информация о библиотеке */}
          <div className={styles.rightColumn}>
            <div className={styles.libraryCard}>
              <h3 className={styles.libraryTitle}>Ваш книжный уголок</h3>
              {userLibrary ? (
                <>
                  <p className={styles.libraryName}>{userLibrary.name}</p>
                  <p className={styles.libraryAddress}>{userLibrary.address}</p>
                </>
              ) : (
                <p className={styles.libraryEmpty}>
                  Вы пока не привязаны ни к одному уголку.
                </p>
              )}
              <div className={styles.decorativeLine} />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}