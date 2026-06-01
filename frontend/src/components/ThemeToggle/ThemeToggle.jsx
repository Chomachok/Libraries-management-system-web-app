import { useState, useEffect } from 'react';
import styles from './ThemeToggle.module.css';

export default function ThemeToggle() {
  const [isAmber, setIsAmber] = useState(() => {
    // Сначала проверяем localStorage, затем класс
    const saved = localStorage.getItem('theme');
    if (saved === 'amber') return true;
    if (saved === 'light') return false;
    return document.body.classList.contains('amber-theme');
  });

  // При монтировании синхронизируем класс с состоянием
  useEffect(() => {
    document.body.classList.toggle('amber-theme', isAmber);
    localStorage.setItem('theme', isAmber ? 'amber' : 'light');
  }, [isAmber]);

  const toggle = () => setIsAmber(prev => !prev);

  return (
    <button
      onClick={toggle}
      className={styles.toggle}
      title={isAmber ? 'Включить свет' : 'Включить настольную лампу'}
      aria-label={isAmber ? 'Переключить на светлую тему' : 'Переключить на тёмную тему'}
    >
      <span className={styles.icon}>{isAmber ? '💡' : '🕯️'}</span>
      <span className={styles.label}>{isAmber ? 'Свет' : 'Уют'}</span>
    </button>
  );
}