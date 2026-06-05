import { useEffect, useState } from 'react';
import { useApi } from '../../hooks/useApi';
import Modal from '../../components/Modal/Modal';
import Pagination from '../../components/Pagination/Pagination';
import BookCover from '../../components/BookCover/BookCover';
import api from '../../api/api';
import styles from './ManageBooksPage.module.css';
import { useNotification } from '../../contexts/NotificationContext';

const ITEMS_PER_PAGE = 6;

export default function ManageBooksPage() {
  const [search, setSearch] = useState('');
  const [books, loading, error, fetchBooks] = useApi('/books', { params: { search }, immediate: false });
  const [modalOpen, setModalOpen] = useState(false);
  const [editingBook, setEditingBook] = useState(null);
  const [form, setForm] = useState({
    title: '',
    author: '',
    isbn: '',
    genre: '',
    year: 2024,
    totalCopies: 1,
    coverImageUrl: '',
  });
  const [currentPage, setCurrentPage] = useState(1);
  const { showToast, showConfirm } = useNotification();

  // Загрузка книг при изменении поиска
  useEffect(() => {
    fetchBooks({ search });
  }, [search, fetchBooks]);

  const handleDelete = async (id) => {
    const confirmed = await showConfirm('Вы действительно хотите удалить книгу?', 'Удаление');
    if (!confirmed) return;
    try {
      await api.delete(`/books/${id}`);
      showToast('Книга удалена.', 'success');
      fetchBooks({ search });
    } catch (err) {
      showToast(err.response?.data?.error || 'Ошибка удаления', 'error');
    }
  };

  const openCreateModal = () => {
    setEditingBook(null);
    setForm({
      title: '',
      author: '',
      isbn: '',
      genre: '',
      year: 2024,
      totalCopies: 1,
      coverImageUrl: '',
    });
    setModalOpen(true);
  };

  const openEditModal = (book) => {
    setEditingBook(book);
    setForm({
      title: book.title,
      author: book.author,
      isbn: book.isbn,
      genre: book.genre,
      year: book.year,
      totalCopies: book.totalCopies,
      coverImageUrl: book.coverImageUrl || '',
    });
    setModalOpen(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      if (editingBook) {
        await api.put(`/books/${editingBook.id}`, form);
        showToast('Книга обновлена.', 'success');
      } else {
        await api.post('/books', form);
        showToast('Книга добавлена в фонд.', 'success');
      }
      setModalOpen(false);
      fetchBooks({ search });
    } catch (err) {
      showToast(err.response?.data?.error || 'Ошибка сохранения', 'error');
    }
  };

  const totalPages = Math.ceil((books?.length || 0) / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const paginatedBooks = Array.isArray(books)
    ? books.slice(startIndex, startIndex + ITEMS_PER_PAGE)
    : [];

  return (
    <div className={styles.page}>
      <h1>Управление фондом</h1>
      <div className={styles.controls}>
        <input
          placeholder="Поиск..."
          value={search}
          onChange={(e) => {
            setSearch(e.target.value);
            setCurrentPage(1);
          }}
        />
        <button onClick={openCreateModal} className="btn-accent">
          + Добавить книгу
        </button>
      </div>

      {loading && <p>Загрузка...</p>}
      {error && <p className={styles.error}>Ошибка</p>}

      <div className={styles.grid}>
        {paginatedBooks.map((book) => (
          <div key={book.id} className="card">
            <BookCover coverUrl={book.coverImageUrl} title={book.title} />
            <h3>{book.title}</h3>
            <p className={styles.author}>{book.author}</p>
            <p>Копий: {book.totalCopies} (доступно {book.availableCopies})</p>
            <div className={styles.actions}>
              <button onClick={() => openEditModal(book)} className="btn-accent">
                ✏️
              </button>
              <button onClick={() => handleDelete(book.id)} className="btn-accent">
                🗑️
              </button>
            </div>
          </div>
        ))}
      </div>

      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />

      {modalOpen && (
        <Modal
          title={editingBook ? 'Редактировать книгу' : 'Добавить книгу'}
          onClose={() => setModalOpen(false)}
        >
          <form className={styles.form} onSubmit={handleSave} data-testid="book-form">
            <input
              value={form.title}
              onChange={(e) => setForm({ ...form, title: e.target.value })}
              placeholder="Название"
              required
            />
            <input
              value={form.author}
              onChange={(e) => setForm({ ...form, author: e.target.value })}
              placeholder="Автор"
              required
            />
            <input
              value={form.isbn}
              onChange={(e) => setForm({ ...form, isbn: e.target.value })}
              placeholder="ISBN"
            />
            <input
              value={form.genre}
              onChange={(e) => setForm({ ...form, genre: e.target.value })}
              placeholder="Жанр"
            />
            <input
              type="number"
              value={form.year}
              onChange={(e) => setForm({ ...form, year: e.target.value })}
              placeholder="Год"
            />
            <input
              type="number"
              value={form.totalCopies}
              onChange={(e) => setForm({ ...form, totalCopies: e.target.value })}
              min="1"
              required
            />
            <input
              value={form.coverImageUrl}
              onChange={(e) => setForm({ ...form, coverImageUrl: e.target.value })}
              placeholder="URL обложки (необязательно)"
            />
            <button type="submit" className="btn-accent">
              Сохранить
            </button>
          </form>
        </Modal>
      )}
    </div>
  );
}