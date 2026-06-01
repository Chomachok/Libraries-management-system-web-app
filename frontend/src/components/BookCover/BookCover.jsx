import { useState } from 'react';
import styles from './BookCover.module.css';

export default function BookCover({ coverUrl, title }) {
  const [imgError, setImgError] = useState(false);
  const placeholder = '/covers/placeholder.jpg';

  const src = coverUrl
    ? coverUrl.startsWith('http')
      ? coverUrl
      : `/covers/${coverUrl}`
    : placeholder;

  return (
    <div className={styles.cover}>
      <img
        src={imgError ? placeholder : src}
        alt={title || 'Обложка книги'}
        className={styles.image}
        onError={() => setImgError(true)}
      />
    </div>
  );
}