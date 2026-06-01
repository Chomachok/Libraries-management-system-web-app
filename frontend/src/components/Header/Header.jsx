import { useState, useEffect, useRef } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import styles from './Header.module.css';
import ThemeToggle from '../ThemeToggle/ThemeToggle';

export default function Header() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  const profile = () => {
    navigate('/profile');
  }

  return (
    <header className={styles.header} data-testid="header">
      <div className={styles.logo}>
        <Link to="/" className={styles.logoLink}>
          📖 Книжный угол
        </Link>
      </div>

      <nav className={styles.desktopNav}>
        <Link to="/books">Книжный стеллаж</Link>
        {user?.role != 'Librarian' && <Link to="/libraries">Наши уголки</Link>}
        {user?.role == 'Reader' && <Link to="/my-books">Мой уголок</Link>}
      </nav>

      <div className={styles.actions}>
        <ThemeToggle />
        {user ? (
          <>
            <Link to="/profile" className={styles.loginBtn} data-testid="profile-btn" style={{ marginLeft: '0.5rem' }}>
              Профиль
            </Link>
            <button onClick={handleLogout} className="btn-accent" style={{ marginLeft: '0.5rem' }}>
              Выйти
            </button>
          </>
        ) : (
          <Link to="/login" className={styles.loginBtn}>
            Войти
          </Link>
        )}
      </div>

      <button className={styles.burger} onClick={() => setMobileMenuOpen(!mobileMenuOpen)}>
        <span />
        <span />
        <span />
      </button>

      {mobileMenuOpen && (
        <div className={styles.mobileMenu}>
          {/* аналогичные ссылки */}
        </div>
      )}
    </header>
  );
}