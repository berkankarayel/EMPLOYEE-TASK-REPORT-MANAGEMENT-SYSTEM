import { useState, useEffect } from 'react';
import api from '../services/api';
import './Modal.css';

function TaskModal({ isOpen, onClose, onSubmit, editTask }) {
  const [users, setUsers] = useState([]);
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    status: 'Pending',
    dueDate: '',
    assignedUserId: ''
  });
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (isOpen) {
      fetchUsers();
    }
  }, [isOpen]);

  useEffect(() => {
    if (editTask) {
      setFormData({
        title: editTask.title,
        description: editTask.description,
        status: editTask.status,
        dueDate: editTask.dueDate ? editTask.dueDate.split('T')[0] : '',
        assignedUserId: editTask.assignedUserId
      });
    } else {
      setFormData({
        title: '',
        description: '',
        status: 'Pending',
        dueDate: '',
        assignedUserId: ''
      });
    }
  }, [editTask, isOpen]);

  const fetchUsers = async () => {
    try {
      const response = await api.get('/User');
      setUsers(response.data);
    } catch (err) {
      console.error('Error fetching users:', err);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      await onSubmit(formData);
    } catch (err) {
      console.error('Submit error:', err);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{editTask ? 'Görev Güncelle' : 'Yeni Görev Ekle'}</h2>
          <button className="close-btn" onClick={onClose}>&times;</button>
        </div>
        
        <form onSubmit={handleSubmit} className="modal-form">
          <div className="form-group">
            <label>Başlık *</label>
            <input
              type="text"
              name="title"
              value={formData.title}
              onChange={handleChange}
              required
              placeholder="Görev başlığı giriniz"
            />
          </div>

          <div className="form-group">
            <label>Açıklama *</label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              required
              rows="4"
              placeholder="Görev açıklaması giriniz"
            />
          </div>

          <div className="form-group">
            <label>Durum *</label>
            <select name="status" value={formData.status} onChange={handleChange} required>
              <option value="Pending">Bekliyor</option>
              <option value="Started">Devam Ediyor</option>
              <option value="Completed">Tamamlandı</option>
            </select>
          </div>

          <div className="form-group">
            <label>Bitiş Tarihi *</label>
            <input
              type="date"
              name="dueDate"
              value={formData.dueDate}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label>Atanan Kullanıcı *</label>
            <select name="assignedUserId" value={formData.assignedUserId} onChange={handleChange} required>
              <option value="">Kullanıcı Seçiniz</option>
              {users.map(user => (
                <option key={user.id} value={user.id}>
                  {user.fullName} ({user.email})
                </option>
              ))}
            </select>
          </div>

          <div className="modal-actions">
            <button type="button" onClick={onClose} className="cancel-btn" disabled={isSubmitting}>
              İptal
            </button>
            <button type="submit" className="submit-btn" disabled={isSubmitting}>
              {isSubmitting ? 'İşleniyor...' : (editTask ? 'Güncelle' : 'Ekle')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default TaskModal;
