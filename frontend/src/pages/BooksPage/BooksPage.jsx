import { useState, useEffect } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import api from '../../api/api';
import { useApi } from '../../hooks/useApi';

export default function BooksPage() {
  const { user } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();

  // Извлекаем параметры из URL при первом рендере
  const initialSearch = searchParams.get('search') || '';
  const initialLibraryId = searchParams.get('libraryId') || '';

  const [filters, setFilters] = useState({
    libraryId: initialLibraryId,
    search: initialSearch,
  });

  const [libraries] = useApi('/libraries');
  const [books, bookLoading, bookError, fetchBooks] = useApi('/books', { immediate: false });

  // Загрузка книг при изменении фильтров
  useEffect(() => {
    const params = {};
    if (filters.libraryId) params.libraryId = filters.libraryId;
    if (filters.search) params.search = filters.search;
    fetchBooks(params);
  }, [filters, fetchBooks]);

  // Загрузка книг только той библиотеки, к которой относится библиотекарь
  useEffect(() => {
    if (user?.role === 'Librarian' && user.libraryId) {
      setFilters(prev => ({ ...prev, libraryId: user.libraryId.toString() }));
    }
  }, [user]);

  // Синхронизируем фильтры с URL
  useEffect(() => {
    const params = {};
    if (filters.search) params.search = filters.search;
    if (filters.libraryId) params.libraryId = filters.libraryId;
    setSearchParams(params);
  }, [filters, setSearchParams]);

  const borrowBook = async (bookId) => {
    try {
      await api.post(`/checkouts/borrow/${bookId}`);
      alert('Книга отложена в ваш уголок!');
      // обновить список
      const params = {};
      if (filters.libraryId) params.libraryId = filters.libraryId;
      if (filters.search) params.search = filters.search;
      fetchBooks(params);
    } catch (err) {
      alert(err.response?.data?.error || 'Ошибка');
    }
  };

  const userLibraryName = 
    user?.role === 'Librarian' && user?.libraryId && Array.isArray(libraries)
      ? libraries.find(lib => lib.id === parseInt(user.libraryId))?.name
      : null;

  return (
    <div style={{ padding: '2rem' }}>
      <h1>Книжные углы</h1>
      <div style={{ display: 'flex', gap: '1rem', marginBottom: '2rem', flexWrap: 'wrap' }}>
        {user?.role === 'Librarian' ? (
          <input
            type="text"
            value={userLibraryName ? `Ваш уголок: ${userLibraryName}` : `Ваш уголок (ID: ${user.libraryId})`}
            disabled
            style={{ flex: 1, minWidth: '200px' }}
          />
        ) : (
          <select
            value={filters.libraryId}
            onChange={e => setFilters({ ...filters, libraryId: e.target.value })}
          >
            <option value="">Все уголки</option>
            {Array.isArray(libraries) && libraries.map(l => (
              <option key={l.id} value={l.id}>{l.name}</option>
            ))}
          </select>
        )}
        <input
          type="text"
          placeholder="Название или автор..."
          value={filters.search}
          onChange={e => setFilters({ ...filters, search: e.target.value })}
          style={{ flex: 1, minWidth: '200px' }}
        />
      </div>

      {bookLoading && <p>Ищем книги на полках...</p>}
      {bookError && <p style={{ color: 'var(--color-accent-hover)' }}>Ошибка загрузки каталога</p>}

      <div style={{
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
        gap: '1.5rem',
      }}>
        {Array.isArray(books) && books.map(book => (
          <div key={book.id} className="card" style={{ padding: '1.5rem', display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
            <Link to={`/books/${book.id}`} style={{ textDecoration: 'none', color: 'inherit' }}>
              <h3 style={{ fontFamily: 'var(--font-heading)' }}>{book.title}</h3>
            </Link>
            <p style={{ fontStyle: 'italic', color: 'var(--color-accent-primary)' }}>{book.author}</p>
            <p style={{ fontSize: '0.9rem' }}>
              В наличии: {book.availableCopies} / {book.totalCopies}
            </p>
            <p style={{ fontSize: '0.8rem', color: 'var(--color-accent-secondary)' }}>{book.libraryName}</p>
            {user?.role === 'Reader' && book.availableCopies > 0 && (
              <button
                onClick={() => borrowBook(book.id)}
                style={{ marginTop: 'auto', alignSelf: 'flex-start' }}
              >
                Отложить в мой уголок
              </button>
            )}
            {book.availableCopies === 0 && (
              <span className="badge-warning" style={{ marginTop: 'auto', alignSelf: 'flex-start' }}>
                Занята
              </span>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}