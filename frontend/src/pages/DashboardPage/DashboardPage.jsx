import { useApi } from '../../hooks/useApi';

export default function DashboardPage() {
  const [stats, loading, error] = useApi('/dashboard/stats');

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p style={{ color: 'var(--accent-hover)' }}>Ошибка: {error}</p>;
  if (!stats) return <p>Нет данных</p>;

  return (
    <div>
      <h1>Статистика библиотеки</h1>
      <div className="grid" style={{ gridTemplateColumns: 'repeat(auto-fill, minmax(160px, 1fr))' }}>
        <div className="stat-card">
          <h3>{stats.booksCount}</h3>
          <p>Книг</p>
        </div>
        <div className="stat-card">
          <h3>{stats.readersCount}</h3>
          <p>Читателей</p>
        </div>
        <div className="stat-card">
          <h3>{stats.activeCheckouts}</h3>
          <p>Активных выдач</p>
        </div>
        <div className="stat-card">
          <h3>{stats.overdueCheckouts}</h3>
          <p>Просроченных</p>
        </div>
      </div>
    </div>
  );
}