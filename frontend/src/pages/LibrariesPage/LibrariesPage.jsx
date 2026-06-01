import { useApi } from '../../hooks/useApi';

export default function LibrariesPage() {
  const [libraries, loading, error] = useApi('/libraries');

  if (loading) return <p style={{ padding: '2rem' }}>Загрузка...</p>;
  if (error) return <p style={{ padding: '2rem', color: 'var(--color-accent-hover)' }}>Ошибка загрузки</p>;

  return (
    <div style={{ padding: '2rem' }}>
      <h1>Наши уголки</h1>
      <div>
        {libraries.map(lib => (
          <div key={lib.id} className="card">
            <h3>{lib.name}</h3>
            <p>{lib.address}</p>
          </div>
        ))}
      </div>
    </div>
  );
}