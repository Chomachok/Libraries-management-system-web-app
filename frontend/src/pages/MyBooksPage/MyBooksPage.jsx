import { useApi } from '../../hooks/useApi';
import { useNotification } from '../../contexts/NotificationContext';
import api from '../../api/api';
import styles from './MyBooksPage.module.css';

export default function MyBooksPage() {
  const { showToast } = useNotification();
  const [checkouts, loading, error, refetch, setCheckouts] = useApi('/checkouts/my-active');

  const returnBook = async (checkoutId) => {
    // Оптимистично убираем выдачу из списка
    setCheckouts(prev => Array.isArray(prev) ? prev.filter(c => c.id !== checkoutId) : prev);

    try {
      await api.post(`/checkouts/return/${checkoutId}`);
      showToast('Книга возвращена в уголок!', 'success');
      // Актуализируем список
      refetch();
    } catch (err) {
      // В случае ошибки возвращаем выдачу обратно в список
      const failedCheckout = Array.isArray(checkouts) 
        ? checkouts.find(c => c.id === checkoutId) 
        : null;
      if (failedCheckout) {
        setCheckouts(prev => Array.isArray(prev) 
          ? [...prev, failedCheckout] 
          : prev
        );
      } else {
        // Если не нашли, просто перезагрузим весь список
        refetch();
      }
      showToast(err.response?.data?.error || 'Не удалось вернуть книгу', 'error');
    }
  };

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p style={{ color: 'var(--color-accent-hover)' }}>Ошибка: {error}</p>;

  return (
    <div className={styles.page}>
      <h1>Мои текущие книги</h1>
      {checkouts.length === 0 ? (
        <p className={styles.empty}>Ваш уголок пока пустует.</p>
      ) : (
        <div className={styles.list}>
          {checkouts.map(c => {
            const isOverdue = new Date(c.dueDate) < new Date();
            return (
              <div 
                key={c.id} 
                className={`${styles.card} ${isOverdue ? styles.overdue : ''}`}
                data-testid={`checkout-card-${c.id}`}
              >
                <h3>{c.bookTitle}</h3>
                <p>Взята: {new Date(c.checkoutDate).toLocaleDateString()}</p>
                <p>Вернуть до: {new Date(c.dueDate).toLocaleDateString()}</p>
                {isOverdue && (
                  <p className={styles.overdueText}>
                    Книга соскучилась по полке! Пожалуйста, верните её.
                  </p>
                )}
                <button 
                  onClick={() => returnBook(c.id)} 
                  className={`btn-accent ${styles.returnBtn}`}
                >
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