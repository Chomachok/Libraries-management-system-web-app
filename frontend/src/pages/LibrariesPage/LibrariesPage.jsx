import { useApi } from '../../hooks/useApi';

export default function LibrariesPage() {
  const [libraries, loading, error] = useApi('/libraries');

  if (loading) return <p style={{ padding: '2rem' }}>Загрузка...</p>;
  if (error) return <p style={{ padding: '2rem', color: 'var(--color-accent-hover)' }}>Ошибка загрузки</p>;

  return (
    <div style={{ padding: '2rem' }}>
      <h1>Наши уголки</h1>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '1.5rem' }}>
        {libraries.map(lib => (
          <div key={lib.id} className="card" style={{ padding: '2rem' }}>
            <h3 style={{ fontFamily: 'var(--font-heading)' }}>{lib.name}</h3>
            <p style={{ color: 'var(--color-accent-secondary)' }}>{lib.address}</p>
          </div>
        ))}
      </div>
    </div>
  );
}