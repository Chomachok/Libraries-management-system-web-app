import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { interfaceTexts } from '../../locale/interfaceTexts';
import styles from './LoginPage.module.css';

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [form, setForm] = useState({ email: '', password: '' });
  const [error, setError] = useState('');

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      await login(form.email, form.password);
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.error || 'Неверные учётные данные');
    }
  };

  return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '80vh', padding: '1rem' }}>
      <form onSubmit={handleSubmit} style={{
        background: 'var(--color-card-bg)',
        borderRadius: '6px',
        padding: '3rem',
        boxShadow: 'var(--shadow-soft)',
        width: '100%',
        maxWidth: '420px',
        display: 'flex',
        flexDirection: 'column',
        gap: '1.2rem'
      }}>
        <h2 style={{ fontFamily: 'var(--font-heading)', textAlign: 'center' }}>{interfaceTexts.auth.title}</h2>
        {error && <p style={{ color: 'var(--color-accent-hover)', fontSize: '0.9rem' }}>{error}</p>}
        <input
          type="email"
          placeholder={interfaceTexts.auth.placeholderLogin}
          value={form.email}
          onChange={e => setForm({...form, email: e.target.value})}
          required
        />
        <input
          type="password"
          placeholder="Пароль"
          value={form.password}
          onChange={e => setForm({...form, password: e.target.value})}
          required
        />
        <button type="submit" style={{ padding: '0.8rem', fontSize: '1rem', fontWeight: 600 }}>Войти</button>
        <p style={{ textAlign: 'center', fontFamily: 'var(--font-ui)' }}>
          Нет аккаунта? <Link to="/register" className={styles.link}>Зарегистрироваться</Link>
        </p>
      </form>
    </div>
  );
}