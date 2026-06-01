import { useApi } from '../../hooks/useApi';
import api from '../../api/api';
import { interfaceTexts } from '../../locale/interfaceTexts';

export default function MyBooksPage() {
  const [checkouts, loading, error, refetch] = useApi('/checkouts/my-active');

  const returnBook = async (checkoutId) => {
    try {
      await api.post(`/checkouts/return/${checkoutId}`);
      refetch();
    } catch (err) {
      alert(err.response?.data?.error || 'Ошибка');
    }
  };

  if (loading) return <p style={{ padding: '2rem' }}>Загрузка вашего уголка...</p>;
  if (error) return <p style={{ padding: '2rem', color: 'var(--color-accent-hover)' }}>Ошибка</p>;

  return (
    <div style={{ padding: '2rem' }}>
      <h1>Мой уголок</h1>
      {checkouts.length === 0 ? (
        <p style={{ fontStyle: 'italic' }}>{interfaceTexts.booking.emptyShelf}</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          {checkouts.map(c => {
            const isOverdue = new Date(c.dueDate) < new Date() && !c.returnDate;
            return (
              <div key={c.id} className="card" style={{
                padding: '1.5rem',
                border: isOverdue ? '1px solid var(--color-status-warning)' : 'none'
              }}>
                <h3 style={{ fontFamily: 'var(--font-heading)' }}>{c.bookTitle}</h3>
                <p>Взята: {new Date(c.checkoutDate).toLocaleDateString()}</p>
                <p>Вернуть до: {new Date(c.dueDate).toLocaleDateString()}</p>
                {isOverdue && <p style={{ color: 'var(--color-accent-hover)' }}>Книга соскучилась по полке!</p>}
                <button onClick={() => returnBook(c.id)} style={{ marginTop: '0.5rem', alignSelf: 'flex-start' }}>
                  Вернуть в уголок
                </button>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}