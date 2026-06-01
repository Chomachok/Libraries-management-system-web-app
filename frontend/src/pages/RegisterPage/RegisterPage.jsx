import { useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import api from '../../api/api';
import styles from './RegisterPage.module.css';

export default function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [libraries, setLibraries] = useState([]);
  const [form, setForm] = useState({
    fullName: '',
    email: '',
    password: '',
    libraryId: '',
  });
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [fieldErrors, setFieldErrors] = useState({});

  // Загрузка списка библиотек
  useEffect(() => {
    api.get('/libraries')
      .then(res => setLibraries(Array.isArray(res.data) ? res.data : []))
      .catch(() => setLibraries([]));
  }, []);

  // Сброс ошибок при изменении полей
  const handleChange = (field, value) => {
    setForm(prev => ({ ...prev, [field]: value }));
    setFieldErrors(prev => ({ ...prev, [field]: '' }));
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
    if (!form.password) {
      errors.password = 'Придумайте пароль.';
    } else if (form.password.length < 6) {
      errors.password = 'Пароль должен содержать хотя бы 6 символов.';
    }
    if (!confirmPassword) {
      errors.confirmPassword = 'Пожалуйста, повторите пароль.';
    } else if (form.password !== confirmPassword) {
      errors.confirmPassword = 'Пароли не совпадают. Попробуйте ещё раз.';
    }
    if (!form.libraryId) {
      errors.libraryId = 'Выберите, в какой уголок вы хотите записаться.';
    }
    return errors;
  };

  const handleSubmit = async e => {
    e.preventDefault();
    const errors = validate();
    if (Object.keys(errors).length > 0) {
      setFieldErrors(errors);
      return;
    }

    try {
      await register({ ...form, libraryId: parseInt(form.libraryId) });
      navigate('/');
    } catch (err) {
      setError(err.response?.data?.error || 'Ошибка регистрации');
    }
  };

  return (
    <div className={styles.container} data-testid="register-page">
      <form className={styles.form} onSubmit={handleSubmit} data-testid="register-form">
        <h2 data-testid="register-title">Регистрация читателя</h2>
        {error && <p className={styles.error} data-testid="register-error">{error}</p>}

        <input
          placeholder="ФИО"
          value={form.fullName}
          onChange={e => handleChange('fullName', e.target.value)}
          className={fieldErrors.fullName ? styles.inputError : ''}
          data-testid="register-fullname-input"
        />
        {fieldErrors.fullName && <span className={styles.fieldError}>{fieldErrors.fullName}</span>}

        <input
          type="email"
          placeholder="Email"
          value={form.email}
          onChange={e => handleChange('email', e.target.value)}
          className={fieldErrors.email ? styles.inputError : ''}
          data-testid="register-email-input"
        />
        {fieldErrors.email && <span className={styles.fieldError}>{fieldErrors.email}</span>}

        <input
          type="password"
          placeholder="Пароль"
          value={form.password}
          onChange={e => handleChange('password', e.target.value)}
          className={fieldErrors.password ? styles.inputError : ''}
          data-testid="register-password-input"
        />
        {fieldErrors.password && <span className={styles.fieldError}>{fieldErrors.password}</span>}

        <input
          type="password"
          placeholder="Подтверждение пароля"
          value={confirmPassword}
          onChange={e => {
            setConfirmPassword(e.target.value);
            setFieldErrors(prev => ({ ...prev, confirmPassword: '' }));
          }}
          className={fieldErrors.confirmPassword ? styles.inputError : ''}
          data-testid="register-confirm-password-input"
        />
        {fieldErrors.confirmPassword && <span className={styles.fieldError}>{fieldErrors.confirmPassword}</span>}

        <select
          value={form.libraryId}
          onChange={e => handleChange('libraryId', e.target.value)}
          className={fieldErrors.libraryId ? styles.inputError : ''}
          data-testid="register-library-select"
        >
          <option value="">Выберите библиотеку</option>
          {libraries.map(l => (
            <option key={l.id} value={l.id}>{l.name}</option>
          ))}
        </select>
        {fieldErrors.libraryId && <span className={styles.fieldError}>{fieldErrors.libraryId}</span>}

        <button type="submit" data-testid="register-submit-button">
          Зарегистрироваться
        </button>
        <p>
          Уже есть аккаунт?{' '}
          <Link to="/login" className={styles.link}>Войти</Link>
        </p>
      </form>
    </div>
  );
}