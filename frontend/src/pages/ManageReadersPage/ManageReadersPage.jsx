import { useState } from 'react';
import { useApi } from '../../hooks/useApi';
import Modal from '../../components/Modal/Modal';
import Pagination from '../../components/Pagination/Pagination';
import api from '../../api/api';

const ITEMS_PER_PAGE = 6;

export default function ManageReadersPage() {
  const [readers, loading, error, refetch] = useApi('/readers');
  const [search, setSearch] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingReader, setEditingReader] = useState(null);
  const [form, setForm] = useState({ fullName: '', email: '', password: '' });
  const [currentPage, setCurrentPage] = useState(1);

  const filtered = Array.isArray(readers) ? readers.filter(r =>
    r.fullName.toLowerCase().includes(search.toLowerCase()) ||
    r.email.toLowerCase().includes(search.toLowerCase())
  ) : [];

  const totalPages = Math.ceil(filtered.length / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const paginatedReaders = filtered.slice(startIndex, startIndex + ITEMS_PER_PAGE);

  const handleDelete = async (id) => {
    if (window.confirm('Удалить читателя?')) {
      try {
        await api.delete(`/readers/${id}`);
        refetch();
      } catch (err) { alert(err.response?.data?.error || 'Ошибка'); }
    }
  };

  const openCreateModal = () => {
    setEditingReader(null);
    setForm({ fullName: '', email: '', password: '' });
    setModalOpen(true);
  };

  const openEditModal = (reader) => {
    setEditingReader(reader);
    setForm({ fullName: reader.fullName, email: reader.email, password: '' });
    setModalOpen(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      if (editingReader) {
        const payload = { fullName: form.fullName, email: form.email };
        if (form.password) payload.password = form.password;
        await api.put(`/readers/${editingReader.id}`, payload);
      } else {
        await api.post('/readers', form);
      }
      setModalOpen(false);
      refetch();
    } catch (err) { alert(err.response?.data?.error || 'Ошибка'); }
  };

  return (
    <div>
      <h1>Управление читателями</h1>
      <div className="filters">
        <input placeholder="Поиск..." value={search} onChange={e => { setSearch(e.target.value); setCurrentPage(1); }} />
        <button className="btn-accent" onClick={openCreateModal}>+ Добавить читателя</button>
      </div>
      {loading && <p>Загрузка...</p>}
      {error && <p style={{ color: 'var(--accent-hover)' }}>Ошибка: {error}</p>}
      <div className="grid">
        {paginatedReaders.map(reader => (
          <div key={reader.id} className="card">
            <h3>{reader.fullName}</h3>
            <p>{reader.email}</p>
            <div style={{ display: 'flex', gap: '0.5rem', marginTop: 'auto' }}>
              <button className="btn-accent" onClick={() => openEditModal(reader)}>✏️</button>
              <button className="btn-accent" onClick={() => handleDelete(reader.id)}>🗑️</button>
            </div>
          </div>
        ))}
      </div>
      <Pagination currentPage={currentPage} totalPages={totalPages} onPageChange={setCurrentPage} />
      {modalOpen && (
        <Modal title={editingReader ? 'Редактировать читателя' : 'Добавить читателя'} onClose={() => setModalOpen(false)}>
          <form style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }} onSubmit={handleSave}>
            <input value={form.fullName} onChange={e => setForm({...form, fullName: e.target.value})} placeholder="ФИО" required />
            <input value={form.email} onChange={e => setForm({...form, email: e.target.value})} type="email" placeholder="Email" required />
            <input value={form.password} onChange={e => setForm({...form, password: e.target.value})} type="password"
              placeholder={editingReader ? 'Новый пароль (если нужно)' : 'Пароль'} required={!editingReader} />
            <button className="btn-accent" type="submit">Сохранить</button>
          </form>
        </Modal>
      )}
    </div>
  );
}