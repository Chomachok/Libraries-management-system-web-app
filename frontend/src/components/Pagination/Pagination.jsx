export default function Pagination({ currentPage, totalPages, onPageChange }) {
  if (totalPages <= 1) return null;

  return (
    <div className="pagination" style={{ display: 'flex', gap: '0.5rem', justifyContent: 'center', margin: '2rem 0' }}>
      {Array.from({ length: totalPages }, (_, i) => i + 1).map(page => (
        <button
          key={page}
          onClick={() => onPageChange(page)}
          style={{
            background: page === currentPage ? 'var(--color-accent-primary)' : 'var(--color-card-bg)',
            color: page === currentPage ? 'white' : 'var(--color-text-main)',
            border: `1px solid var(--color-border)`,
            borderRadius: '6px',
            padding: '0.5rem 1rem',
            cursor: 'pointer',
            transition: 'all 0.2s'
          }}
          data-testid={`pagination-page-${page}`}
        >
          {page}
        </button>
      ))}
    </div>
  );
}