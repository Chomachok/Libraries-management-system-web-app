import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { useApi } from '../../hooks/useApi';
import styles from './RegisterPage.module.css';

export default function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [libraries, libLoading] = useApi('/libraries');
  const [form, setForm] = useState({ fullName: '', email: '', password: '', libraryId: '' });
  const [error, setError] = useState('');

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      await register({ ...form, libraryId: parseInt(form.libraryId) });
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.error || 'Ошибка регистрации');
    }
  };

  return (
    <div className="form-container" data-testid="register-page">
      <form className="form-card" onSubmit={handleSubmit} data-testid="register-form">
        <h2>Регистрация читателя</h2>
        {error && <p className={styles.error}>{error}</p>}
        <input placeholder="ФИО" value={form.fullName} onChange={e => setForm({...form, fullName: e.target.value})} required />
        <input type="email" placeholder="Email" value={form.email} onChange={e => setForm({...form, email: e.target.value})} required />
        <input type="password" placeholder="Пароль" value={form.password} onChange={e => setForm({...form, password: e.target.value})} required />
        <select value={form.libraryId} onChange={e => setForm({...form, libraryId: e.target.value})} required>
          <option value="">Выберите библиотеку</option>
          {Array.isArray(libraries) && libraries.map(l => <option key={l.id} value={l.id}>{l.name}</option>)}
        </select>
        <button type="submit" className="btn-accent">Зарегистрироваться</button>
        <p>Уже есть аккаунт? <Link to="/login">Войти</Link></p>
      </form>
    </div>
  );
}