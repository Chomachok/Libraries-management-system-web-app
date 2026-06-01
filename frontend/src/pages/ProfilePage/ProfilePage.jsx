import { useState, useEffect } from 'react';
import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
import styles from './ProfilePage.module.css';
import { useAuth } from '../../contexts/AuthContext';
import { useNotification } from '../../contexts/NotificationContext';

export default function ProfilePage() {
  const { user } = useAuth();
  const [profile, loading, error, refetch] = useApi('/readers/profile');
  const [libraries] = useApi('/libraries');
  const [editMode, setEditMode] = useState(false);
  const [form, setForm] = useState({ fullName: '', email: '' });
  const [fieldErrors, setFieldErrors] = useState({});

  // Уведомление (успех / серверная ошибка)
  const [notification, setNotification] = useState(null);
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    if (notification) {
      setIsVisible(true);
      const timer = setTimeout(() => {
        setIsVisible(false);
        setTimeout(() => setNotification(null), 400);
      }, 4000);
      return () => clearTimeout(timer);
    }
  }, [notification]);

  // Сброс ошибок при изменении полей
  const handleInputChange = (field, value) => {
    setForm((prev) => ({ ...prev, [field]: value }));
    setFieldErrors((prev) => ({ ...prev, [field]: '' }));
  };

  const validate = () => {
    const errors = {};
    if (!form.fullName.trim()) {
      errors.fullName = 'Пожалуйста, напишите, как к вам обращаться.';
    }
    if (!form.email.trim()) {
      errors.email = 'Email не должен быть пустым.';
    } else if (!/\S+@\S+\.\S+/.test(form.email)) {
      errors.email = 'Кажется, в email не хватает символа @. Проверьте, пожалуйста.';
    }
    return errors;
  };

  const startEdit = () => {
    if (profile) {
      setForm({ fullName: profile.fullName, email: profile.email });
      setFieldErrors({});
      setEditMode(true);
    }
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      await api.put('/readers/profile', form);
      showToast('Профиль бережно обновлён.', 'success');
      setEditMode(false);
      refetch();
    } catch (err) {
      showToast(err.response?.data?.error || 'Ошибка обновления', 'error');
    }
  };

  if (loading) return <p className={styles.loading}>Загрузка профиля...</p>;
  if (error) return <p className={styles.error}>Ошибка загрузки</p>;
  if (!profile) return <p className={styles.loading}>Профиль не найден</p>;

  const userLibrary = Array.isArray(libraries)
    ? libraries.find((l) => l.id === profile.libraryId)
    : null;

  const initials = profile.fullName
    .split(' ')
    .map((word) => word[0])
    .slice(0, 2)
    .join('')
    .toUpperCase();

  return (
    <div className={styles.container}>
      {/* Уведомление */}
      {notification && (
        <div
          className={`${styles.notification} ${styles[notification.type]} ${
            isVisible ? styles.visible : styles.hidden
          }`}
          onClick={() => setIsVisible(false)}
        >
          <span className={styles.notificationIcon}>
            {notification.type === 'success' ? '✓' : '✗'}
          </span>
          <span>{notification.message}</span>
        </div>
      )}

      <h1 className={styles.heading}>Мой уголок</h1>

      {editMode ? (
        <form onSubmit={handleSave} className={styles.editForm}>
          <label className={styles.label}>Имя</label>
          <input
            value={form.fullName}
            onChange={(e) => handleInputChange('fullName', e.target.value)}
            required
            className={fieldErrors.fullName ? styles.inputError : ''}
          />
          {fieldErrors.fullName && (
            <span className={styles.fieldError}>{fieldErrors.fullName}</span>
          )}

          <label className={styles.label}>Email</label>
          <input
            type="email"
            value={form.email}
            onChange={(e) => handleInputChange('email', e.target.value)}
            required
            className={fieldErrors.email ? styles.inputError : ''}
          />
          {fieldErrors.email && (
            <span className={styles.fieldError}>{fieldErrors.email}</span>
          )}

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