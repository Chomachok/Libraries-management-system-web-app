import { createContext, useState, useContext, useEffect, useCallback } from 'react';
import api, { setAccessToken, setOnTokenRefreshed } from '../api/api';

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);        // данные пользователя (роль, имя...)
  const [loading, setLoading] = useState(true);

  // Попытка восстановить сессию через refresh-токен при старте
  useEffect(() => {
    (async () => {
      try {
        const res = await api.post('/auth/refresh');
        const accessToken = res.data.accessToken;
        // Сохраняем access-токен в памяти (через api.setAccessToken)
        setAccessToken(accessToken);

        // Загружаем профиль пользователя
        const profileRes = await api.get('/readers/profile');
        setUser(profileRes.data);
      } catch (err) {
        // Refresh не удался – пользователь не авторизован
        setUser(null);
        setAccessToken(null);
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  // Колбэк для api.interceptor – обновляет user при принудительном разлогине
  useEffect(() => {
    setOnTokenRefreshed(() => {
      // Если обновление токена не удалось, сбрасываем пользователя
      setUser(null);
      setAccessToken(null);
    });
  }, []);

  const login = async (email, password) => {
    const res = await api.post('/auth/login', { email, password });
    setAccessToken(res.data.accessToken);
    const profileRes = await api.get('/readers/profile');
    setUser(profileRes.data);
  };

  const register = async (form) => {
    const res = await api.post('/auth/register', form);
    setAccessToken(res.data.accessToken);
    const profileRes = await api.get('/readers/profile');
    setUser(profileRes.data);
  };

  const logout = async () => {
    try {
      // Сообщаем серверу удалить refresh-куку
      await api.post('/auth/logout');
    } catch (e) {
      // игнорируем ошибку
    }
    setAccessToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, register, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);