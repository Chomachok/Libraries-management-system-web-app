import { useState } from 'react';
import { useApi } from '../../hooks/useApi';
import Modal from '../../components/Modal/Modal';
import Pagination from '../../components/Pagination/Pagination';
import api from '../../api/api';

const ITEMS_PER_PAGE = 8;

export default function ManageCheckoutsPage() {
  const [checkouts, chLoading, chError, refetchCheckouts] = useApi('/checkouts/library');
  const [books] = useApi('/books');
  const [readers] = useApi('/readers');
  const [modalOpen, setModalOpen] = useState(false);
  const [issueForm, setIssueForm] = useState({ userId: '', bookId: '', durationDays: 14 });
  const [currentPage, setCurrentPage] = useState(1);

  const handleReturn = async (checkoutId) => {
    try {
      await api.post(`/checkouts/return-by-librarian/${checkoutId}`);
      refetchCheckouts();
    } catch (err) { alert(err.response?.data?.error || 'Ошибка'); }
  };

  const handleIssue = async (e) => {
    e.preventDefault();
    try {
      await api.post('/checkouts/issue', issueForm);
      setModalOpen(false);
      refetchCheckouts();
    } catch (err) { alert(err.response?.data?.error || 'Ошибка'); }
  };

  if (chLoading) return <p style={{ padding: '2rem' }}>Загрузка...</p>;
  if (chError) return <p style={{ padding: '2rem', color: 'var(--color-accent-hover)' }}>Ошибка загрузки выдач</p>;

  const totalPages = Math.ceil((checkouts?.length || 0) / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const paginatedCheckouts = Array.isArray(checkouts) ? checkouts.slice(startIndex, startIndex + ITEMS_PER_PAGE) : [];

  return (
    <div style={{ padding: '2rem' }}>
      <h1>Учёт выдач</h1>
      <button onClick={() => setModalOpen(true)} style={{ marginBottom: '1rem' }}>+ Новая выдача</button>

      <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
        {paginatedCheckouts.map(c => (
          <div key={c.id} className="card" style={{ padding: '1.5rem' }}>
            <p><strong>Книга:</strong> {c.bookTitle}</p>
            <p><strong>Читатель:</strong> {c.userName}</p>
            <p>Выдана: {new Date(c.checkoutDate).toLocaleDateString()}</p>
            <p>Срок: {new Date(c.dueDate).toLocaleDateString()}</p>
            {c.returnDate ? (
              <p>Возвращена: {new Date(c.returnDate).toLocaleDateString()}</p>
            ) : (
              <button onClick={() => handleReturn(c.id)}>Принять возврат</button>
            )}
            {c.fineAmount > 0 && <p>Штраф: {c.fineAmount} руб.</p>}
          </div>
        ))}
      </div>

      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />

      {modalOpen && (
        <Modal title="Новая выдача" onClose={() => setModalOpen(false)}>
          <form onSubmit={handleIssue} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
            <select value={issueForm.userId} onChange={e => setIssueForm({...issueForm, userId: e.target.value})} required>
              <option value="">Выберите читателя</option>
              {Array.isArray(readers) && readers.map(r => <option key={r.id} value={r.id}>{r.fullName}</option>)}
            </select>
            <select value={issueForm.bookId} onChange={e => setIssueForm({...issueForm, bookId: e.target.value})} required>
              <option value="">Выберите книгу</option>
              {Array.isArray(books) && books.filter(b => b.availableCopies > 0).map(b => <option key={b.id} value={b.id}>{b.title} (доступно {b.availableCopies})</option>)}
            </select>
            <input type="number" value={issueForm.durationDays} onChange={e => setIssueForm({...issueForm, durationDays: e.target.value})} min="1" required placeholder="Дней" />
            <button type="submit">Выдать</button>
          </form>
        </Modal>
      )}
    </div>
  );
}