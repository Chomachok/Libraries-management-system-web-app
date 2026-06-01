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
      try { await api.delete(`/books/${id}`); fetchBooks({ search }); } catch (err) { alert(err.response?.data?.error || 'Ошибка'); }
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
    <div style={{ padding: '2rem' }}>
      <h1>Управление фондом</h1>
      <div style={{ display: 'flex', gap: '1rem', marginBottom: '1.5rem' }}>
        <input
          placeholder="Поиск..."
          value={search}
          onChange={e => { setSearch(e.target.value); setCurrentPage(1); }}
          style={{ flex: 1 }}
        />
        <button onClick={openCreateModal}>+ Добавить книгу</button>
      </div>
      {loading && <p>Загрузка...</p>}
      {error && <p style={{ color: 'var(--color-accent-hover)' }}>Ошибка</p>}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))', gap: '1.5rem' }}>
        {paginatedBooks.map(book => (
          <div key={book.id} className="card" style={{ padding: '1.5rem' }}>
            <h3 style={{ fontFamily: 'var(--font-heading)' }}>{book.title}</h3>
            <p style={{ fontStyle: 'italic' }}>{book.author}</p>
            <p>Копий: {book.totalCopies} (доступно {book.availableCopies})</p>
            <div style={{ display: 'flex', gap: '0.5rem', marginTop: 'auto' }}>
              <button onClick={() => openEditModal(book)} style={{ background: 'var(--color-accent-secondary)', color: 'var(--color-text-main)' }}>✏️</button>
              <button onClick={() => handleDelete(book.id)}>🗑️</button>
            </div>
          </div>
        ))}
      </div>
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {modalOpen && (
        <Modal title={editingBook ? 'Редактировать книгу' : 'Добавить книгу'} onClose={() => setModalOpen(false)}>
          <form onSubmit={handleSave} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <input value={form.title} onChange={e => setForm({...form, title: e.target.value})} placeholder="Название" required />
            <input value={form.author} onChange={e => setForm({...form, author: e.target.value})} placeholder="Автор" required />
            <input value={form.isbn} onChange={e => setForm({...form, isbn: e.target.value})} placeholder="ISBN" />
            <input value={form.genre} onChange={e => setForm({...form, genre: e.target.value})} placeholder="Жанр" />
            <input type="number" value={form.year} onChange={e => setForm({...form, year: e.target.value})} placeholder="Год" />
            <input type="number" value={form.totalCopies} onChange={e => setForm({...form, totalCopies: e.target.value})} min="1" required />
            <button type="submit">Сохранить</button>
          </form>
        </Modal>
      )}
    </div>
  );
}