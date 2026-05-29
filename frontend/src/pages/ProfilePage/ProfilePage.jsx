import { useState } from 'react';
import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
import styles from './ProfilePage.module.css';

export default function ProfilePage() {
  const [profile, loading, error, refetch] = useApi('/readers/profile');
  const [editMode, setEditMode] = useState(false);
  const [form, setForm] = useState({ fullName: '', email: '' });

  const startEdit = () => {
    if (profile) setForm({ fullName: profile.fullName, email: profile.email });
    setEditMode(true);
  };

  const handleSave = async e => {
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

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p style={{ color: 'var(--accent-hover)' }}>Ошибка: {error}</p>;
  if (!profile) return <p>Профиль не найден</p>;

  return (
    <div className="form-container">
      <div className="form-card">
        <h2>Профиль</h2>
        {editMode ? (
          <form onSubmit={handleSave} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <input value={form.fullName} onChange={e => setForm({...form, fullName: e.target.value})} placeholder="ФИО" required />
            <input value={form.email} onChange={e => setForm({...form, email: e.target.value})} placeholder="Email" required />
            <button type="submit" className="btn-accent">Сохранить</button>
            <button type="button" onClick={() => setEditMode(false)}>Отмена</button>
          </form>
        ) : (
          <div className={styles.info}>
            <p><strong>ФИО:</strong> {profile.fullName}</p>
            <p><strong>Email:</strong> {profile.email}</p>
            <p><strong>Роль:</strong> {profile.role}</p>
            <button className="btn-accent" onClick={startEdit}>Редактировать</button>
          </div>
        )}
      </div>
    </div>
  );
}