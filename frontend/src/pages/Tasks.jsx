import { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import api from '../services/api';
import TaskModal from '../components/TaskModal';
import './Tasks.css';
import './Common.css';

function Tasks() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingTask, setEditingTask] = useState(null);
  const [viewMode, setViewMode] = useState('list'); // 'list' or 'cards'
  
  const role = localStorage.getItem('role');
  const isAdmin = role === 'Admin';

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
      toast.success('Görev başarıyla silindi!');
    } catch (err) {
      toast.error('Silme işlemi başarısız: ' + (err.response?.data?.message || 'Bir hata oluştu'));
      console.error('Error deleting task:', err);
    }
  };

  const handleAdd = () => {
    setEditingTask(null);
    setIsModalOpen(true);
  };

  const handleEdit = (task) => {
    setEditingTask(task);
    setIsModalOpen(true);
  };

  const handleModalSubmit = async (formData) => {
    try {
      if (editingTask) {
        // Update
        const updateData = {
          id: editingTask.id,
          ...formData
        };
        await api.put(`/Task/${editingTask.id}`, updateData);
        toast.success('Görev başarıyla güncellendi!');
      } else {
        // Create
        await api.post('/Task', formData);
        toast.success('Görev başarıyla oluşturuldu!');
      }
      
      // Başarılı olursa modal'ı kapat
      await fetchTasks();
      setIsModalOpen(false);
      setEditingTask(null);
    } catch (err) {
      // Hata olursa modal açık kalsın
      toast.error('İşlem başarısız: ' + (err.response?.data?.message || err.message || 'Bir hata oluştu'));
      console.error('Error saving task:', err);
      throw err; // TaskModal'da finally bloğu çalışsın
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

  const handleStatusChange = async (taskId, newStatus) => {
    try {
      const task = tasks.find(t => t.id === taskId);
      const updateData = {
        id: taskId,
        title: task.title,
        description: task.description,
        status: newStatus,
        dueDate: task.dueDate,
        assignedUserId: task.assignedUserId
      };
      await api.put(`/Task/${taskId}`, updateData);
      await fetchTasks();
      toast.success('Görev durumu güncellendi!');
    } catch (err) {
      toast.error('Durum güncelleme başarısız: ' + (err.response?.data?.message || 'Bir hata oluştu'));
      console.error('Error updating task status:', err);
    }
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
      <TaskModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleModalSubmit}
        editTask={editingTask}
      />
      
      <div className="page-header">
        <h2>Görev Yönetimi</h2>
        <div className="header-actions">
          {!isAdmin && (
            <div className="view-toggle">
              <button 
                className={viewMode === 'list' ? 'view-btn active' : 'view-btn'}
                onClick={() => setViewMode('list')}
              >
                📋 Liste
              </button>
              <button 
                className={viewMode === 'cards' ? 'view-btn active' : 'view-btn'}
                onClick={() => setViewMode('cards')}
              >
                🃏 Kartlar
              </button>
            </div>
          )}
          {isAdmin && <button className="add-btn" onClick={handleAdd}>+ Görev Ekle</button>}
        </div>
      </div>

      <div className="filters">
        <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)} className="filter-select">
          <option value="">Tüm Durumlar</option>
          <option value="Pending">Bekliyor</option>
          <option value="Started">Devam Ediyor</option>
          <option value="Completed">Tamamlandı</option>
        </select>
      </div>

      {!isAdmin && viewMode === 'cards' ? (
        <div className="task-cards-container">
          <div className="task-column">
            <h3 className="column-title pending">📋 Bekliyor</h3>
            {filteredTasks.filter(t => t.status === 'Pending').map(task => (
              <div key={task.id} className="task-card pending">
                <h4>{task.title}</h4>
                <p className="task-description">{task.description}</p>
                <div className="task-meta">
                  <span className="task-date">📅 {new Date(task.dueDate).toLocaleDateString('tr-TR')}</span>
                </div>
                <button 
                  className="status-change-btn start"
                  onClick={() => handleStatusChange(task.id, 'Started')}
                >
                  ▶️ Başla
                </button>
              </div>
            ))}
          </div>

          <div className="task-column">
            <h3 className="column-title started">⚡ Devam Ediyor</h3>
            {filteredTasks.filter(t => t.status === 'Started').map(task => (
              <div key={task.id} className="task-card started">
                <h4>{task.title}</h4>
                <p className="task-description">{task.description}</p>
                <div className="task-meta">
                  <span className="task-date">📅 {new Date(task.dueDate).toLocaleDateString('tr-TR')}</span>
                </div>
                <button 
                  className="status-change-btn complete"
                  onClick={() => handleStatusChange(task.id, 'Completed')}
                >
                  ✅ Tamamla
                </button>
              </div>
            ))}
          </div>

          <div className="task-column">
            <h3 className="column-title completed">✅ Tamamlandı</h3>
            {filteredTasks.filter(t => t.status === 'Completed').map(task => (
              <div key={task.id} className="task-card completed">
                <h4>{task.title}</h4>
                <p className="task-description">{task.description}</p>
                <div className="task-meta">
                  <span className="task-date">📅 {new Date(task.dueDate).toLocaleDateString('tr-TR')}</span>
                </div>
                <span className="completed-badge">✔️ Tamamlandı</span>
              </div>
            ))}
          </div>
        </div>
      ) : (
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
                <td>{task.assignedUserName || `User ID: ${task.assignedUserId}`}</td>
                <td>
                  <div className="action-buttons">
                    <button className="edit-btn" title="Düzenle" onClick={() => handleEdit(task)}>✏️</button>
                    <button className="delete-btn" title="Sil" onClick={() => handleDelete(task.id)}>🗑️</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      )}
    </div>
  );
}

export default Tasks;
