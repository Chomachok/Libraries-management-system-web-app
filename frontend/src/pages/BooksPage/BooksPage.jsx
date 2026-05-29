import { useState, useEffect } from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
import styles from './BooksPage.module.css';

export default function BooksPage() {
  const { user } = useAuth();
  const [filters, setFilters] = useState({ libraryId: '', search: '' });
  const [libraries] = useApi('/libraries');
  const [books, bookLoading, bookError, fetchBooks] = useApi('/books', { immediate: false });

  useEffect(() => {
    const params = {};
    if (filters.libraryId) params.libraryId = filters.libraryId;
    if (filters.search) params.search = filters.search;
    fetchBooks(params);
  }, [filters, fetchBooks]);

  const borrowBook = async (bookId) => {
    try {
      await api.post(`/checkouts/borrow/${bookId}`);
      alert('Книга успешно взята!');
      fetchBooks(filters);
    } catch (err) {
      alert(err.response?.data?.error || 'Ошибка');
    }
  };

  return (
    <div data-testid="books-page">
      <h1 data-testid="books-title">Каталог книг</h1>
      <div className="filters" data-testid="books-filters">
        <select value={filters.libraryId} onChange={e => setFilters({...filters, libraryId: e.target.value})}>
          <option value="">Все библиотеки</option>
          {Array.isArray(libraries) && libraries.map(l => <option key={l.id} value={l.id}>{l.name}</option>)}
        </select>
        <input
          type="text"
          placeholder="Поиск по названию или автору"
          value={filters.search}
          onChange={e => setFilters({...filters, search: e.target.value})}
        />
      </div>
      {bookLoading && <p>Загрузка...</p>}
      {bookError && <p style={{ color: 'var(--accent-hover)' }}>Ошибка: {bookError}</p>}
      <div className="grid" data-testid="books-grid">
        {Array.isArray(books) && books.map(book => (
          <div key={book.id} className={`card ${styles.bookCard}`} data-testid={`book-card-${book.id}`}>
            <h3>{book.title}</h3>
            <p className={styles.author}>{book.author}</p>
            <p>В наличии: {book.availableCopies}/{book.totalCopies}</p>
            <p className={styles.library}>{book.libraryName}</p>
            {user?.role === 'Reader' && book.availableCopies > 0 && (
              <button className={`btn-accent ${styles.borrowBtn}`} onClick={() => borrowBook(book.id)}>
                Взять книгу
              </button>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}