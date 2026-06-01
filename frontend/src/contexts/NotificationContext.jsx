import { createContext, useContext, useState, useCallback, useRef } from 'react';
import Toast from '../components/Toast/Toast';
import ConfirmDialog from '../components/ConfirmDialog/ConfirmDialog';

const NotificationContext = createContext();

export function NotificationProvider({ children }) {
  const [toast, setToast] = useState(null);
  const [confirm, setConfirm] = useState(null);
  const resolveRef = useRef(null);

  const showToast = useCallback((message, type = 'success', duration = 4000) => {
    setToast({ message, type });
    if (duration) {
      setTimeout(() => setToast(null), duration);
    }
  }, []);

  const showConfirm = useCallback((message, title = 'Подтверждение') => {
    return new Promise((resolve) => {
      setConfirm({ message, title });
      resolveRef.current = resolve;
    });
  }, []);

  const handleConfirmClose = (result) => {
    setConfirm(null);
    if (resolveRef.current) {
      resolveRef.current(result);
      resolveRef.current = null;
    }
  };

  return (
    <NotificationContext.Provider value={{ showToast, showConfirm }}>
      {children}
      {toast && <Toast message={toast.message} type={toast.type} onClose={() => setToast(null)} />}
      {confirm && (
        <ConfirmDialog
          title={confirm.title}
          message={confirm.message}
          onConfirm={() => handleConfirmClose(true)}
          onCancel={() => handleConfirmClose(false)}
        />
      )}
    </NotificationContext.Provider>
  );
}

export const useNotification = () => useContext(NotificationContext);