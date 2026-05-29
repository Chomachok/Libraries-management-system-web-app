import { useState } from 'react';
import { useApi } from '../../hooks/useApi';
import Modal from '../../components/Modal/Modal';
import Pagination from '../../components/Pagination/Pagination';
import api from '../../api/api';

const ITEMS_PER_PAGE = 6;

export default function ManageBooksPage() {
  const [search, setSearch] = useState('');
  const [books, loading, error, fetchBooks] = useApi('/books', { params: { search }, immediate: false });
  const [modalOpen, setModalOpen] = useState(false);
  const [editingBook, setEditingBook] = useState(null);
  const [form, setForm] = useState({ title: '', author: '', isbn: '', genre: '', year: 2024, totalCopies: 1 });
  const [currentPage, setCurrentPage] = useState(1);

  useState(() => {
    fetchBooks({ search });
  }, [search, fetchBooks]);

  const handleDelete = async (id) => {
    if (window.confirm('Удалить книгу?')) {
      try {
        await api.delete(`/books/${id}`);
        fetchBooks({ search });
      } catch (err) { alert(err.response?.data?.error || 'Ошибка'); }
    }
  };

  const openCreateModal = () => {
    setEditingBook(null);
    setForm({ title: '', author: '', isbn: '', genre: '', year: 2024, totalCopies: 1 });
    setModalOpen(true);
  };

  const openEditModal = (book) => {
    setEditingBook(book);
    setForm({ title: book.title, author: book.author, isbn: book.isbn, genre: book.genre, year: book.year, totalCopies: book.totalCopies });
    setModalOpen(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      if (editingBook) {
        await api.put(`/books/${editingBook.id}`, form);
      } else {
        await api.post('/books', form);
      }
      setModalOpen(false);
      fetchBooks({ search });
    } catch (err) { alert(err.response?.data?.error || 'Ошибка'); }
  };

  const totalPages = Math.ceil((books?.length || 0) / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const paginatedBooks = Array.isArray(books) ? books.slice(startIndex, startIndex + ITEMS_PER_PAGE) : [];

  return (
    <div>
      <h1>Управление книгами</h1>
      <div className="filters">
        <input placeholder="Поиск..." value={search} onChange={e => { setSearch(e.target.value); setCurrentPage(1); }} />
        <button className="btn-accent" onClick={openCreateModal}>+ Добавить книгу</button>
      </div>
      {loading && <p>Загрузка...</p>}
      {error && <p style={{ color: 'var(--accent-hover)' }}>Ошибка: {error}</p>}
      <div className="grid">
        {paginatedBooks.map(book => (
          <div key={book.id} className="card">
            <h3>{book.title}</h3>
            <p>{book.author}</p>
            <p>Копий: {book.totalCopies} (доступно {book.availableCopies})</p>
            <div style={{ display: 'flex', gap: '0.5rem', marginTop: 'auto' }}>
              <button className="btn-accent" onClick={() => openEditModal(book)}>✏️</button>
              <button className="btn-accent" onClick={() => handleDelete(book.id)}>🗑️</button>
            </div>
          </div>
        ))}
      </div>
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {modalOpen && (
        <Modal title={editingBook ? 'Редактировать книгу' : 'Добавить книгу'} onClose={() => setModalOpen(false)}>
          <form style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }} onSubmit={handleSave}>
            <input value={form.title} onChange={e => setForm({...form, title: e.target.value})} placeholder="Название" required />
            <input value={form.author} onChange={e => setForm({...form, author: e.target.value})} placeholder="Автор" required />
            <input value={form.isbn} onChange={e => setForm({...form, isbn: e.target.value})} placeholder="ISBN" />
            <input value={form.genre} onChange={e => setForm({...form, genre: e.target.value})} placeholder="Жанр" />
            <input type="number" value={form.year} onChange={e => setForm({...form, year: e.target.value})} placeholder="Год" />
            <input type="number" value={form.totalCopies} onChange={e => setForm({...form, totalCopies: e.target.value})} min="1" required />
            <button className="btn-accent" type="submit">Сохранить</button>
          </form>
        </Modal>
      )}
    </div>
  );
}