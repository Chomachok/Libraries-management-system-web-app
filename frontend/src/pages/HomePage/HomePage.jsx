import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
import styles from './HomePage.module.css';
import { interfaceTexts } from '../../locale/interfaceTexts';
import { useAuth } from '../../contexts/AuthContext';

export default function HomePage() {
  const navigate = useNavigate();
  const [search, setSearch] = useState('');
  const { user } = useAuth();

  // Загружаем все книги, чтобы выбрать случайную доступную
  const [books, loadingBooks] = useApi('/books');

  // Состояние для "Книги вечера"
  const [featuredBook, setFeaturedBook] = useState(null);

  useEffect(() => {
    if (Array.isArray(books) && books.length > 0) {
      // Отфильтровываем только доступные книги
      const availableBooks = books.filter(b => b.availableCopies > 0);
      if (availableBooks.length > 0) {
        const randomIndex = Math.floor(Math.random() * availableBooks.length);
        setFeaturedBook(availableBooks[randomIndex]);
      } else {
        // Если доступных нет, берём любую
        const randomIndex = Math.floor(Math.random() * books.length);
        setFeaturedBook(books[randomIndex]);
      }
    }
  }, [books]);

  const handleSearch = (e) => {
    e.preventDefault();
    if (search.trim()) {
      navigate(`/books?search=${encodeURIComponent(search.trim())}`);
    } else {
      navigate('/books');
    }
  };

  // Функция "Отложить в мой уголок" (требует авторизации)
  const handleBorrow = async (bookId) => {
    try {
      await api.post(`/checkouts/borrow/${bookId}`);
      alert(interfaceTexts.booking.success);
    } catch (err) {
      alert(err.response?.data?.error || 'Ошибка');
    }
  };

  return (
    <div className={styles.home}>
      {/* Hero */}
      <section className={styles.hero}>
        <h1>{interfaceTexts.home.welcome}</h1>
        <form onSubmit={handleSearch} className={styles.searchForm}>
          <input
            type="text"
            placeholder={interfaceTexts.home.searchPlaceholder}
            value={search}
            onChange={e => setSearch(e.target.value)}
            className={styles.searchInput}
          />
          <button type="submit" className="btn-accent">Искать</button>
        </form>
      </section>

      {/* Книга вечера */}
      <section className={styles.featured}>
        <h2>📖 Книга вечера</h2>
        {loadingBooks && <p>Выбираем особенную книгу...</p>}
        {!loadingBooks && !featuredBook && (
          <p>Сегодня все книги разобраны, загляните позже.</p>
        )}
        {featuredBook && (
          <div className={styles.featuredContent}>
            <div className={styles.coverWrapper}>
              {/* Обложка-заглушка, если нет реальной */}
              <img
                src={featuredBook.cover || '/assets/book-cover-placeholder.jpg'}
                alt={featuredBook.title}
                className={styles.cover}
              />
            </div>
            <div className={styles.info}>
              <h3>{featuredBook.title}</h3>
              <p className={styles.author}>{featuredBook.author}</p>
              <p className={styles.desc}>
                {featuredBook.description || 'Увлекательная история, которая скрасит ваш вечер.'}
              </p>
              {featuredBook.availableCopies > 0 && user?.role === 'Reader' ? (
          <button className="btn-accent" onClick={() => handleBorrow(featuredBook.id)}>
            Отложить в мой уголок
          </button>
        ) : featuredBook.availableCopies === 0 ? (
          <span className="badge-warning">
            Сейчас в гостях у другого читателя. Вернётся домой примерно {new Date(Date.now() + 14 * 86400000).toLocaleDateString()}
          </span>
        ) : null}
            </div>
          </div>
        )}
      </section>
    </div>
  );
}