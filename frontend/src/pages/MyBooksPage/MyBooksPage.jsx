import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
// модульные стили не нужны, но файл существует

export default function MyBooksPage() {
  const [checkouts, loading, error, refetch] = useApi('/checkouts/my-active');

  const returnBook = async (checkoutId) => {
    try {
      await api.post(`/checkouts/return/${checkoutId}`);
      alert('Книга возвращена');
      refetch();
    } catch (err) {
      alert(err.response?.data?.error || 'Ошибка');
    }
  };

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p style={{ color: 'var(--accent-hover)' }}>Ошибка: {error}</p>;

  return (
    <div data-testid="my-books-page">
      <h1>Мои текущие книги</h1>
      {checkouts.length === 0 ? (
        <p>Нет активных выдач</p>
      ) : (
        <div className="grid" style={{ gridTemplateColumns: '1fr' }}>
          {checkouts.map(c => (
            <div key={c.id} className="card" data-testid={`checkout-card-${c.id}`}>
              <h3>{c.bookTitle}</h3>
              <p>Взята: {new Date(c.checkoutDate).toLocaleDateString()}</p>
              <p>Вернуть до: {new Date(c.dueDate).toLocaleDateString()}</p>
              <button className="btn-accent" onClick={() => returnBook(c.id)}>
                Вернуть книгу
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}