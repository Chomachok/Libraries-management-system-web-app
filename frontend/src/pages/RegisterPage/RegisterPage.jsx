import { useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import api from '../../api/api';
import { interfaceTexts } from '../../locale/interfaceTexts';

export default function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [libraries, setLibraries] = useState([]);
  const [form, setForm] = useState({ fullName: '', email: '', password: '', libraryId: '' });
  const [error, setError] = useState('');

  useEffect(() => {
    api.get('/libraries')
      .then(res => setLibraries(Array.isArray(res.data) ? res.data : []))
      .catch(() => setLibraries([]));
  }, []);

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      await register({...form, libraryId: parseInt(form.libraryId)});
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.error || 'Ошибка регистрации');
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
        <h2 style={{ fontFamily: 'var(--font-heading)', textAlign: 'center' }}>Регистрация читателя</h2>
        {error && <p style={{ color: 'var(--color-accent-hover)', fontSize: '0.9rem' }}>{error}</p>}
        <input placeholder="ФИО" value={form.fullName} onChange={e => setForm({...form, fullName: e.target.value})} required />
        <input type="email" placeholder="Email" value={form.email} onChange={e => setForm({...form, email: e.target.value})} required />
        <input type="password" placeholder="Пароль" value={form.password} onChange={e => setForm({...form, password: e.target.value})} required />
        <select value={form.libraryId} onChange={e => setForm({...form, libraryId: e.target.value})} required>
          <option value="">Выберите библиотеку</option>
          {libraries.map(l => <option key={l.id} value={l.id}>{l.name}</option>)}
        </select>
        <button type="submit" style={{ padding: '0.8rem', fontSize: '1rem', fontWeight: 600 }}>Зарегистрироваться</button>
        <p style={{ textAlign: 'center' }}>
          Уже есть аккаунт? <Link to="/login">Войти</Link>
        </p>
      </form>
    </div>
  );
}