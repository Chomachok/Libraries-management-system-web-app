import axios from 'axios';

let accessToken = null; // хранится только в памяти
let onTokenRefreshed = null; // колбэк при неудачном обновлении

// Устанавливается из AuthProvider
export const setAccessToken = (token) => {
  accessToken = token;
};

export const setOnTokenRefreshed = (callback) => {
  onTokenRefreshed = callback;
};

// Axios инстанс
const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials: true, // чтобы браузер отправлял httpOnly куки
});

// Добавляем access-токен в заголовок, если он есть
api.interceptors.request.use((config) => {
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }
  return config;
});

// Очередь запросов, ожидающих обновления токена
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
};

// Перехватчик ответов для обработки 401 и автоматического обновления
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // Если ошибка 401 и мы ещё не пробовали обновить токен для этого запроса
    if (error.response?.status === 401 && !originalRequest._retry) {
      // Если уже идёт процесс обновления, ставим запрос в очередь
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers.Authorization = `Bearer ${token}`;
            return api(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        // Запрашиваем новый access-токен (refresh-кука отправится автоматически)
        const res = await axios.post(
          `${import.meta.env.VITE_API_URL}/auth/refresh`,
          {},
          { withCredentials: true }
        );
        const newToken = res.data.accessToken;
        setAccessToken(newToken); // обновляем в памяти
        processQueue(null, newToken); // выполняем ожидающие запросы с новым токеном
        originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return api(originalRequest); // повторяем исходный запрос
      } catch (refreshError) {
        processQueue(refreshError, null);
        // Обновить не удалось – пользователь больше не авторизован
        if (onTokenRefreshed) onTokenRefreshed();
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export default api;