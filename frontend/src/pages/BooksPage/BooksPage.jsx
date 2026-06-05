import { useState, useEffect } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import api from '../../api/api';
import { useApi } from '../../hooks/useApi';
import BookCover from '../../components/BookCover/BookCover';
import styles from './BooksPage.module.css';
import { useNotification } from '../../contexts/NotificationContext';

export default function BooksPage() {
  const { user } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();
  const initialSearch = searchParams.get('search') || '';
  const initialLibraryId = searchParams.get('libraryId') || '';

  const [filters, setFilters] = useState({ libraryId: initialLibraryId, search: initialSearch });
  const [libraries] = useApi('/libraries');
  const [books, bookLoading, bookError, fetchBooks] = useApi('/books', { immediate: false });

  // Если библиотекарь — сразу показываем только его библиотеку
  useEffect(() => {
    if (user?.role === 'Librarian' && user.libraryId) {
      setFilters(prev => ({ ...prev, libraryId: user.libraryId.toString() }));
    }
  }, [user]);

  useEffect(() => {
    const params = {};
    if (filters.libraryId) params.libraryId = filters.libraryId;
    if (filters.search) params.search = filters.search;
    fetchBooks(params);
  }, [filters, fetchBooks]);

  useEffect(() => {
    const params = {};
    if (filters.search) params.search = filters.search;
    if (filters.libraryId) params.libraryId = filters.libraryId;
    setSearchParams(params);
  }, [filters, setSearchParams]);

  const borrowBook = async (bookId) => {
    try {
      await api.post(`/checkouts/borrow/${bookId}`);
      showToast('Книга отложена в ваш уголок!', 'success');
      fetchBooks(filters);
    } catch (err) {
      showToast(err.response?.data?.error || 'Не удалось взять книгу', 'error');
    }
  };

  // Вычисляем название библиотеки библиотекаря
  const userLibraryName =
    user?.role === 'Librarian' && user?.libraryId && Array.isArray(libraries)
      ? libraries.find(l => l.id === parseInt(user.libraryId))?.name
      : null;

  return (
    <div className={styles.page}>
      <h1>Книжные углы</h1>
      <div className={styles.filters}>
        {user?.role === 'Librarian' ? (
          <input
            type="text"
            value={userLibraryName ? `Ваш уголок: ${userLibraryName}` : `Ваш уголок`}
            disabled
          />
        ) : (
          <select
            value={filters.libraryId}
            onChange={e => setFilters({ ...filters, libraryId: e.target.value })}
          >
            <option value="">Все уголки</option>
            {Array.isArray(libraries) &&
              libraries.map(l => (
                <option key={l.id} value={l.id}>
                  {l.name}
                </option>
              ))}
          </select>
        )}
        <input
          type="text"
          placeholder="Название, автор или ваше настроение..."
          value={filters.search}
          onChange={e => setFilters({ ...filters, search: e.target.value })}
        />
      </div>
      {bookLoading && <p>Ищем книги на полках...</p>}
      {bookError && <p className={styles.error}>Ошибка загрузки каталога</p>}
      <div className={styles.grid}>
        {Array.isArray(books) &&
          books.map(book => (
            <div key={book.id} className={`${styles.card} card`}>
              <Link to={`/books/${book.id}`} style={{ textDecoration: 'none', color: 'inherit' }}>
                <BookCover coverUrl={book.coverImageUrl} title={book.title} />
                <h3 className={styles.title}>{book.title}</h3>
              </Link>
              <p className={styles.author}>{book.author}</p>
              <p className={styles.available}>
                В наличии: {book.availableCopies} / {book.totalCopies}
              </p>
              <p className={styles.library}>{book.libraryName}</p>
              {user?.role === 'Reader' && book.availableCopies > 0 && (
                <button onClick={() => borrowBook(book.id)} className={`btn-accent ${styles.borrowBtn}`}>
                  Отложить в мой уголок
                </button>
              )}
            </div>
          ))}
      </div>
    </div>
  );
}