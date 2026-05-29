import { useApi } from '../../hooks/useApi';

export default function HistoryPage() {
  const [checkouts, loading, error] = useApi('/checkouts/my-history');

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p style={{ color: 'var(--accent-hover)' }}>Ошибка: {error}</p>;

  return (
    <div data-testid="history-page">
      <h1>История выдач</h1>
      {checkouts.length === 0 ? <p>История пуста</p> : (
        <div className="grid" style={{ gridTemplateColumns: '1fr' }}>
          {checkouts.map(c => (
            <div key={c.id} className="card">
              <h3>{c.bookTitle}</h3>
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