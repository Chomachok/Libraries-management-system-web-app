import styles from './Modal.module.css';

export default function Modal({ title, onClose, children }) {
  return (
    <div className={styles.overlay} onClick={onClose} data-testid="modal-overlay">
      <div className={styles.modal} onClick={e => e.stopPropagation()} data-testid="modal">
        <div className={styles.header}>
          <h2 data-testid="modal-title">{title}</h2>
          <button className={styles.closeBtn} onClick={onClose} data-testid="modal-close-button">×</button>
        </div>
        <div className={styles.body} data-testid="modal-body">
          {children}
        </div>
      </div>
    </div>
  );
}