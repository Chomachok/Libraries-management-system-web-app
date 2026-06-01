import { useState, useEffect } from 'react';

export default function ThemeToggle() {
  const [isAmber, setIsAmber] = useState(() => document.body.classList.contains('amber-theme'));

  const toggle = () => {
    setIsAmber(prev => {
      const newVal = !prev;
      document.body.classList.toggle('amber-theme', newVal);
      return newVal;
    });
  };

  return (
    <button
      onClick={toggle}
      title={isAmber ? 'Выключить настольную лампу' : 'Включить настольную лампу'}
      style={{ background: 'none', color: 'var(--color-text-main)', fontSize: '1.2rem', padding: '0.2rem' }}
    >
      {isAmber ? '💡' : '🕯️'}
    </button>
  );
}