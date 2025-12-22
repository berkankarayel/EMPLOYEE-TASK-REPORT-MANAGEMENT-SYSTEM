import { useState, useEffect } from 'react';
import api from '../services/api';
import './Tasks.css';
import './Common.css';

function Tasks() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [statusFilter, setStatusFilter] = useState('');

  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = async () => {
    try {
      setLoading(true);
      const response = await api.get('/Task');
      setTasks(response.data);
      setError('');
    } catch (err) {
      setError('Görevler yüklenirken hata oluştu');
      console.error('Error fetching tasks:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Bu görevi silmek istediğinizden emin misiniz?')) {
      return;
    }

    try {
      await api.delete(`/Task/${id}`);
      setTasks(tasks.filter(task => task.id !== id));
    } catch (err) {
      alert('Silme işlemi başarısız: ' + (err.response?.data?.message || 'Bir hata oluştu'));
      console.error('Error deleting task:', err);
    }
  };

  const filteredTasks = tasks.filter(task => statusFilter === '' || task.status === statusFilter);

  const getStatusText = (status) => {
    const statusMap = {
      'Pending': 'Bekliyor',
      'Started': 'Devam Ediyor',
      'Completed': 'Tamamlandı'
    };
    return statusMap[status] || status;
  };

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
        <button onClick={fetchTasks} className="retry-btn">Tekrar Dene</button>
      </div>
    );
  }

  return (
    <div className="page-container">
      <div className="page-header">
        <h2>Görev Yönetimi</h2>
        <button className="add-btn">+ Görev Ekle</button>
      </div>

      <div className="filters">
        <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)} className="filter-select">
          <option value="">Tüm Durumlar</option>
          <option value="Pending">Bekliyor</option>
          <option value="Started">Devam Ediyor</option>
          <option value="Completed">Tamamlandı</option>
        </select>
      </div>

      <div className="table-container">
        <table className="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Başlık</th>
              <th>Açıklama</th>
              <th>Durum</th>
              <th>Bitiş Tarihi</th>
              <th>Atanan Kişi</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {filteredTasks.map(task => (
              <tr key={task.id}>
                <td>{task.id}</td>
                <td><strong>{task.title}</strong></td>
                <td className="description">{task.description}</td>
                <td>
                  <span className={`status-badge ${task.status.toLowerCase()}`}>
                    {getStatusText(task.status)}
                  </span>
                </td>
                <td>{new Date(task.dueDate).toLocaleDateString('tr-TR')}</td>
                <td>{task.assignedUserFullName || `User ID: ${task.assignedUserId}`}</td>
                <td>
                  <div className="action-buttons">
                    <button className="edit-btn" title="Düzenle">✏️</button>
                    <button className="delete-btn" title="Sil" onClick={() => handleDelete(task.id)}>🗑️</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Tasks;
