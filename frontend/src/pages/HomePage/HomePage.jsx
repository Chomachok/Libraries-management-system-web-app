import { Link } from 'react-router-dom';
import styles from './HomePage.module.css';

export default function HomePage() {
  return (
    <div className={styles.hero} data-testid="home-page">
      <div className={styles.content}>
        <h1 className={styles.title}>📚 Книжная полка</h1>
        <p className={styles.subtitle}>
          Современная система для управления библиотеками.  
          Просматривайте каталог, берите книги, отслеживайте историю выдач  
          и автоматически рассчитывайте штрафы за просрочку.
        </p>
        <div className={styles.features}>
          <div className={styles.feature}>
            <span className={styles.icon}>📖</span>
            <h3>Единый каталог</h3>
            <p>Поиск по названию, автору или библиотеке</p>
          </div>
          <div className={styles.feature}>
            <span className={styles.icon}>👥</span>
            <h3>Роли и доступ</h3>
            <p>Читатели и библиотекари – каждый со своим функционалом</p>
          </div>
          <div className={styles.feature}>
            <span className={styles.icon}>⚡</span>
            <h3>Автоматизация</h3>
            <p>Контроль экземпляров, сроки возврата, расчёт штрафов</p>
          </div>
        </div>
        <Link to="/books" className={`btn-accent ${styles.cta}`}>
          Перейти в каталог
        </Link>
      </div>
    </div>
  );
}