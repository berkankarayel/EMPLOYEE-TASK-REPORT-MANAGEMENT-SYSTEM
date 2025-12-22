import { useState, useEffect } from 'react';
import './Modal.css';

function UserModal({ isOpen, onClose, onSubmit, editUser }) {
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    password: '',
    role: 'User',
    isActive: true
  });
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (editUser) {
      setFormData({
        fullName: editUser.fullName,
        email: editUser.email,
        password: '',
        role: editUser.role,
        isActive: editUser.isActive
      });
    } else {
      setFormData({
        fullName: '',
        email: '',
        password: '',
        role: 'User',
        isActive: true
      });
    }
  }, [editUser, isOpen]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
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
          <h2>{editUser ? 'Kullanıcı Güncelle' : 'Yeni Kullanıcı Ekle'}</h2>
          <button className="close-btn" onClick={onClose}>&times;</button>
        </div>
        
        <form onSubmit={handleSubmit} className="modal-form">
          <div className="form-group">
            <label>Ad Soyad *</label>
            <input
              type="text"
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
              required
              placeholder="Ad Soyad giriniz"
            />
          </div>

          <div className="form-group">
            <label>Email *</label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
              placeholder="email@example.com"
            />
          </div>

          <div className="form-group">
            <label>Şifre {!editUser && '*'}</label>
            <input
              type="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required={!editUser}
              placeholder={editUser ? 'Değiştirmek için yeni şifre giriniz' : 'Şifre giriniz'}
            />
          </div>

          <div className="form-group">
            <label>Rol *</label>
            <select name="role" value={formData.role} onChange={handleChange} required>
              <option value="User">User</option>
              <option value="Admin">Admin</option>
            </select>
          </div>

          <div className="form-group checkbox-group">
            <label>
              <input
                type="checkbox"
                name="isActive"
                checked={formData.isActive}
                onChange={handleChange}
              />
              <span>Aktif</span>
            </label>
          </div>

          <div className="modal-actions">
            <button type="button" onClick={onClose} className="cancel-btn" disabled={isSubmitting}>
              İptal
            </button>
            <button type="submit" className="submit-btn" disabled={isSubmitting}>
              {isSubmitting ? 'İşleniyor...' : (editUser ? 'Güncelle' : 'Ekle')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default UserModal;
