import styles from './ConfirmDialog.module.css';

export default function ConfirmDialog({ title, message, onConfirm, onCancel }) {
  return (
    <div className={styles.overlay} onClick={onCancel}>
      <div className={styles.dialog} onClick={e => e.stopPropagation()}>
        <h3>{title}</h3>
        <p>{message}</p>
        <div className={styles.actions}>
          <button className={`btn-accent`} onClick={onConfirm}>Да</button>
          <button className={styles.cancelBtn} onClick={onCancel}>Нет</button>
        </div>
      </div>
    </div>
  );
}