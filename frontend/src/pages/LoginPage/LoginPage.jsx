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
  const [fieldErrors, setFieldErrors] = useState({});

  // Сброс ошибок поля при вводе
  const handleChange = (field, value) => {
    setForm(prev => ({ ...prev, [field]: value }));
    setFieldErrors(prev => ({ ...prev, [field]: '' }));
  };

  // Локальная валидация
  const validate = () => {
    const errors = {};
    if (!form.email.trim()) {
      errors.email = 'Email обязателен.';
    } else if (!/\S+@\S+\.\S+/.test(form.email)) {
      errors.email = 'Некорректный формат email.';
    }
    if (!form.password) {
      errors.password = 'Пароль обязателен.';
    } else if (form.password.length < 6) {
      errors.password = 'Пароль должен содержать не менее 6 символов.';
    }
    return errors;
  };

  const handleSubmit = async e => {
    e.preventDefault();

    const localErrors = validate();
    if (Object.keys(localErrors).length > 0) {
      setFieldErrors(localErrors);
      return;
    }

    try {
      await login(form.email, form.password);
      navigate('/');
    } catch (err) {
      const data = err.response?.data;
      if (data) {
        if (typeof data === 'string') {
          setError(data);
        } else if (data.error) {
          setError(data.error);
        } else if (data.errors) {
          // FluentValidation возвращает { field: [messages] }
          const messages = Object.values(data.errors).flat();
          setError(messages.join('. '));
        } else {
          setError('Неверные учётные данные.');
        }
      } else {
        setError('Не удалось связаться с сервером.');
      }
    }
  };

  return (
    <div className={styles.container}>
      <form className={styles.form} onSubmit={handleSubmit} data-testid="login-form">
        <h2>{interfaceTexts.auth.title}</h2>

        {error && <p className={styles.error} data-testid="login-error">{error}</p>}

        <input
          type="email"
          placeholder={interfaceTexts.auth.placeholderLogin}
          value={form.email}
          onChange={e => handleChange('email', e.target.value)}
          className={fieldErrors.email ? styles.inputError : ''}
          data-testid="login-email-input"
        />
        {fieldErrors.email && <span className={styles.fieldError}>{fieldErrors.email}</span>}

        <input
          type="password"
          placeholder="Пароль"
          value={form.password}
          onChange={e => handleChange('password', e.target.value)}
          className={fieldErrors.password ? styles.inputError : ''}
          data-testid="login-password-input"
        />
        {fieldErrors.password && <span className={styles.fieldError}>{fieldErrors.password}</span>}

        <button type="submit" data-testid="login-submit-button">
          Войти
        </button>

        <p>
          Нет аккаунта?{' '}
          <Link to="/register" className={styles.link}>Зарегистрироваться</Link>
        </p>
      </form>
    </div>
  );
}