import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
import BookCover from '../../components/BookCover/BookCover';
import styles from './HomePage.module.css';
import { interfaceTexts } from '../../locale/interfaceTexts';

export default function HomePage() {
  const navigate = useNavigate();
  const { user } = useAuth();
  const [search, setSearch] = useState('');
  const [books, loadingBooks] = useApi('/books');
  const [featuredBook, setFeaturedBook] = useState(null);

  useEffect(() => {
    if (Array.isArray(books) && books.length > 0) {
      const availableBooks = books.filter(b => b.availableCopies > 0);
      const pool = availableBooks.length > 0 ? availableBooks : books;
      const randomIndex = Math.floor(Math.random() * pool.length);
      setFeaturedBook(pool[randomIndex]);
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

  const handleBorrow = async (bookId) => {
    try {
      await api.post(`/checkouts/borrow/${bookId}`);
      showToast('Книга отложена!', 'success');
      // Обновляем данные книги вечера (если она одна)
      const updatedBooks = await api.get('/books');
      if (Array.isArray(updatedBooks.data)) {
        const stillAvailable = updatedBooks.data.find(b => b.id === bookId);
        if (stillAvailable && stillAvailable.availableCopies > 0) {
          setFeaturedBook(stillAvailable);
        } else {
          // Книга закончилась – выбрать новую
          const availableBooks = updatedBooks.data.filter(b => b.availableCopies > 0);
          if (availableBooks.length > 0) {
            setFeaturedBook(availableBooks[Math.floor(Math.random() * availableBooks.length)]);
          } else {
            setFeaturedBook(null);
          }
        }
      }
    } catch (err) {
      showToast(err.response?.data?.error || 'Ошибка', 'error');
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
              <BookCover coverUrl={featuredBook.coverImageUrl} title={featuredBook.title} />
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