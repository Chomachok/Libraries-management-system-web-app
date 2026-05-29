import { useApi } from '../../hooks/useApi';
// Можно не импортировать стили, но чтобы не было ошибок, оставим пустой импорт
import './LibrariesPage.module.css'; // файл содержит только /* пусто */

export default function LibrariesPage() {
  const [libraries, loading, error] = useApi('/libraries');

  if (loading) return <p data-testid="loading">Загрузка...</p>;
  if (error) return <p data-testid="error" style={{ color: 'var(--accent-hover)' }}>Ошибка: {error}</p>;

  return (
    <div data-testid="libraries-page">
      <h1 data-testid="libraries-title">Библиотеки</h1>
      <div className="grid" data-testid="libraries-grid">
        {libraries.map(l => (
          <div key={l.id} className="card" data-testid={`library-card-${l.id}`}>
            <h3 data-testid={`library-name-${l.id}`}>{l.name}</h3>
            <p data-testid={`library-address-${l.id}`}>{l.address}</p>
          </div>
        ))}
      </div>
    </div>
  );
}