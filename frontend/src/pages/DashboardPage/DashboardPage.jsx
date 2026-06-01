import { useApi } from '../../hooks/useApi';

export default function DashboardPage() {
  const [stats, loading, error] = useApi('/dashboard/stats');

  if (loading) return <p style={{ padding: '2rem' }}>Загрузка статистики...</p>;
  if (error) return <p style={{ padding: '2rem', color: 'var(--color-accent-hover)' }}>Ошибка</p>;
  if (!stats) return <p style={{ padding: '2rem' }}>Нет данных</p>;

  return (
    <div style={{ padding: '2rem' }}>
      <h1>Рабочий угол</h1>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(180px, 1fr))', gap: '1.5rem' }}>
        <div className="card" style={{ textAlign: 'center', padding: '2rem' }}>
          <h2 style={{ color: 'var(--color-accent-primary)' }}>{stats.booksCount}</h2>
          <p>Книг в фонде</p>
        </div>
        <div className="card" style={{ textAlign: 'center', padding: '2rem' }}>
          <h2 style={{ color: 'var(--color-accent-primary)' }}>{stats.readersCount}</h2>
          <p>Читателей</p>
        </div>
        <div className="card" style={{ textAlign: 'center', padding: '2rem' }}>
          <h2 style={{ color: 'var(--color-accent-primary)' }}>{stats.activeCheckouts}</h2>
          <p>Активных выдач</p>
        </div>
        <div className="card" style={{ textAlign: 'center', padding: '2rem' }}>
          <h2 style={{ color: 'var(--color-status-warning)' }}>{stats.overdueCheckouts}</h2>
          <p>Просроченных</p>
        </div>
      </div>
    </div>
  );
}