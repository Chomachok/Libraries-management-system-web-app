import { useState } from 'react';
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
    setMobileMenuOpen(false);
  };

  const closeMenu = () => setMobileMenuOpen(false);

  return (
    <header className={styles.header} data-testid="header">
      {/* Логотип */}
      <div className={styles.logo}>
        <Link to="/" className={styles.logoLink} onClick={closeMenu}>
          📖 Книжный угол
        </Link>
      </div>

      {/* Десктопная навигация */}
      <nav className={styles.desktopNav}>
        <Link to="/manage-books">Книжный стеллаж</Link>
        {user?.role !== 'Librarian' && <Link to="/libraries">Наши уголки</Link>}
        {user?.role === 'Reader' && <Link to="/my-books">Мой уголок</Link>}
      </nav>

      {/* Десктопные действия */}
      <div className={styles.actions}>
        <ThemeToggle />
        {user ? (
          <>
            <Link to="/profile" className={styles.loginBtn} data-testid="profile-btn">
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

      {/* Бургер-кнопка */}
      <button className={styles.burger} onClick={() => setMobileMenuOpen(!mobileMenuOpen)}>
        <span />
        <span />
        <span />
      </button>

      {/* Мобильное меню */}
      {mobileMenuOpen && (
        <div className={styles.mobileMenu}>
          <button className={styles.closeBtn} onClick={closeMenu} aria-label="Закрыть меню">
            ✕
          </button>
          <nav className={styles.mobileNav}>
            {/* Переключатель темы в мобильном меню */}
            <div className={styles.mobileThemeToggle}>
              <ThemeToggle />
            </div>

            <Link to="/books" onClick={closeMenu}>Книжный стеллаж</Link>
            {user?.role !== 'Librarian' && (
              <Link to="/libraries" onClick={closeMenu}>Наши уголки</Link>
            )}
            {user?.role === 'Reader' && (
              <Link to="/my-books" onClick={closeMenu}>Мой уголок</Link>
            )}
            {user ? (
              <>
                <Link to="/profile" onClick={closeMenu}>Профиль</Link>
                <button onClick={handleLogout} className={styles.mobileLogoutBtn}>
                  Выйти
                </button>
              </>
            ) : (
              <Link to="/login" onClick={closeMenu}>Войти</Link>
            )}
          </nav>
        </div>
      )}
    </header>
  );
}