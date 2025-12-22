import { useState, useEffect } from 'react';
import api from '../services/api';
import './SystemLogs.css';
import './Common.css';

function SystemLogs() {
  const [logs, setLogs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [levelFilter, setLevelFilter] = useState('');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    fetchLogs();
  }, []);

  const fetchLogs = async () => {
    try {
      setLoading(true);
      const response = await api.get('/SystemLog');
      setLogs(response.data);
      setError('');
    } catch (err) {
      setError('Log kayıtları yüklenirken hata oluştu');
      console.error('Error fetching logs:', err);
    } finally {
      setLoading(false);
    }
  };

  const filteredLogs = logs.filter(log => {
    const matchesLevel = levelFilter === '' || log.level === levelFilter;
    const matchesSearch = log.action.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         log.description.toLowerCase().includes(searchTerm.toLowerCase());
    return matchesLevel && matchesSearch;
  });

  if (loading) {
    return (
      <div className="page-container">
        <div className="loading">Yükleniyor...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="page-container">
        <div className="error">{error}</div>
        <button onClick={fetchLogs} className="retry-btn">Tekrar Dene</button>
      </div>
    );
  }

  return (
    <div className="page-container">
      <div className="page-header">
        <h2>Log Kayıtları</h2>
      </div>

      <div className="filters">
        <input
          type="text"
          placeholder="Log ara..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="search-input"
        />
        <select value={levelFilter} onChange={(e) => setLevelFilter(e.target.value)} className="filter-select">
          <option value="">Tüm Seviyeler</option>
          <option value="Info">Info</option>
          <option value="Warning">Warning</option>
          <option value="Error">Error</option>
        </select>
      </div>

      <div className="table-container">
        <table className="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Aksiyon</th>
              <th>Açıklama</th>
              <th>Seviye</th>
              <th>Tarih</th>
            </tr>
          </thead>
          <tbody>
            {filteredLogs.map(log => (
              <tr key={log.id}>
                <td>{log.id}</td>
                <td><strong>{log.action}</strong></td>
                <td className="log-description">{log.description}</td>
                <td>
                  <span className={`log-level ${log.level.toLowerCase()}`}>
                    {log.level}
                  </span>
                </td>
                <td>{new Date(log.createdAt).toLocaleString('tr-TR')}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default SystemLogs;
