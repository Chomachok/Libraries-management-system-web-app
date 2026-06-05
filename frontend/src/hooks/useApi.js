import { useState, useEffect, useCallback } from 'react';
import api from '../api/api';

export function useApi(url, { params = {}, immediate = true } = {}) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(immediate);
  const [error, setError] = useState(null);

  const fetchData = useCallback(async (customParams) => {
    setLoading(true);
    try {
      const res = await api.get(url, { params: customParams || params });
      if (Array.isArray(res.data)) {
        setData(res.data);
      } else if (typeof res.data === 'object' && res.data !== null) {
        setData(res.data);
      } else {
        setData([]);
      }
      setError(null);
    } catch (err) {
      console.error(`Ошибка загрузки ${url}:`, err);
      setData([]);
      setError(err.response?.data?.error || err.message);
    } finally {
      setLoading(false);
    }
  }, [url, JSON.stringify(params)]);

  useEffect(() => {
    if (immediate) fetchData();
  }, [fetchData, immediate]);

  return [data, loading, error, fetchData, setData]; // добавили setData
}