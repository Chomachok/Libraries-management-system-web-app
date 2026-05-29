import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import styles from './Header.module.css';

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
      <Link to="/" className={styles.logo} data-testid="header-logo">
        <span className={styles.logoIcon}>📚</span>
        <span className={styles.logoText}>Книжная полка</span>
      </Link>

      <nav className={styles.desktopNav} data-testid="nav">
        {!user && (
          <>
            <Link to="/" datatest-id="nav-home">Главная</Link>
            <Link to="/books" data-testid="nav-books">Каталог</Link>
          </>
        )}
        {user?.role === 'Reader' && (
          <>
            <Link to="/books" data-testid="nav-catalog">Каталог</Link>
            <Link to="/my-books" data-testid="nav-my-books">Мои книги</Link>
            <Link to="/history" data-testid="nav-history">История</Link>
          </>
        )}
        {user?.role === 'Librarian' && (
          <>
            <Link to="/dashboard" data-testid="nav-dashboard">Дашборд</Link>
            <Link to="/manage-books" data-testid="nav-manage-books">Книги</Link>
            <Link to="/manage-readers" data-testid="nav-manage-readers">Читатели</Link>
            <Link to="/manage-checkouts" data-testid="nav-manage-checkouts">Выдачи</Link>
          </>
        )}
      </nav>

      <div className={styles.desktopAuth} data-testid="auth-section">
        {user ? (
          <>
            <Link to="/profile" data-testid="auth-profile-link">{user.fullName}</Link>
            <button className={`btn-accent`} onClick={handleLogout} data-testid="logout-button">
              Выйти
            </button>
          </>
        ) : (
          <>
            <Link to="/login" data-testid="login-link">Войти</Link>
            <Link to="/register" data-testid="register-link">Регистрация</Link>
          </>
        )}
      </div>

      <button
        className={styles.burger}
        onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
        data-testid="burger-button"
        aria-label="Меню"
      >
        <span />
        <span />
        <span />
      </button>

      {mobileMenuOpen && (
        <div className={styles.mobileMenu} data-testid="mobile-menu">
          <nav className={styles.mobileNav}>
            {!user && (
              <>
                <Link to="/" onClick={closeMenu} data-testid="mobile-nav-home">Главная</Link>
                <Link to="/books" onClick={closeMenu} data-testid="mobile-nav-books">Каталог</Link>
                <Link to="/login" onClick={closeMenu} data-testid="mobile-login-link">Войти</Link>
                <Link to="/register" onClick={closeMenu} data-testid="mobile-register-link">Регистрация</Link>
              </>
            )}
            {user?.role === 'Reader' && (
              <>
                <Link to="/books" onClick={closeMenu} data-testid="mobile-nav-catalog">Каталог</Link>
                <Link to="/my-books" onClick={closeMenu} data-testid="mobile-nav-my-books">Мои книги</Link>
                <Link to="/history" onClick={closeMenu} data-testid="mobile-nav-history">История</Link>
                <Link to="/profile" onClick={closeMenu} data-testid="mobile-profile-link">Профиль</Link>
              </>
            )}
            {user?.role === 'Librarian' && (
              <>
                <Link to="/dashboard" onClick={closeMenu} data-testid="mobile-nav-dashboard">Дашборд</Link>
                <Link to="/manage-books" onClick={closeMenu} data-testid="mobile-nav-manage-books">Книги</Link>
                <Link to="/manage-readers" onClick={closeMenu} data-testid="mobile-nav-manage-readers">Читатели</Link>
                <Link to="/manage-checkouts" onClick={closeMenu} data-testid="mobile-nav-manage-checkouts">Выдачи</Link>
                <Link to="/profile" onClick={closeMenu} data-testid="mobile-profile-link">Профиль</Link>
              </>
            )}
            {user && (
              <button className={`btn-accent`} onClick={handleLogout} data-testid="mobile-logout-button">
                Выйти
              </button>
            )}
          </nav>
        </div>
      )}
    </header>
  );
}