import { useApi } from '../../hooks/useApi';

export default function HistoryPage() {
  const [checkouts, loading, error] = useApi('/checkouts/my-history');

  if (loading) return <p style={{ padding: '2rem' }}>Загрузка архива...</p>;
  if (error) return <p style={{ padding: '2rem', color: 'var(--color-accent-hover)' }}>Ошибка</p>;

  return (
    <div style={{ padding: '2rem' }}>
      <h1>Архив историй</h1>
      {checkouts.length === 0 ? (
        <p style={{ fontStyle: 'italic' }}>Архив пока пуст</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          {checkouts.map(c => (
            <div key={c.id} className="card" style={{ padding: '1.5rem' }}>
              <h3 style={{ fontFamily: 'var(--font-heading)' }}>{c.bookTitle}</h3>
              <p>Взята: {new Date(c.checkoutDate).toLocaleDateString()}</p>
              <p>Возвращена: {c.returnDate ? new Date(c.returnDate).toLocaleDateString() : '—'}</p>
              {c.fineAmount > 0 && <p>Штраф: {c.fineAmount} руб.</p>}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}