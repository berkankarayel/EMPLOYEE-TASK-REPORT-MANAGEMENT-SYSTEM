import { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import api from '../services/api';
import LeaveRequestModal from '../components/LeaveRequestModal';
import './LeaveRequests.css';
import './Common.css';

function LeaveRequests() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  
  const role = localStorage.getItem('role');
  const isAdmin = role === 'Admin';

  useEffect(() => {
    fetchRequests();
  }, []);

  const fetchRequests = async () => {
    try {
      setLoading(true);
      const response = await api.get('/LeaveRequest');
      setRequests(response.data);
      setError('');
    } catch (err) {
      setError('İzin istekleri yüklenirken hata oluştu');
      console.error('Error fetching leave requests:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (id) => {
    try {
      await api.patch(`/LeaveRequest/${id}/status`, { id, status: 'Approved' });
      await fetchRequests();
      toast.success('İzin talebi onaylandı!');
    } catch (err) {
      toast.error('Onaylama işlemi başarısız: ' + (err.response?.data?.message || 'Bir hata oluştu'));
      console.error('Error approving request:', err);
    }
  };

  const handleReject = async (id) => {
    try {
      await api.patch(`/LeaveRequest/${id}/status`, { id, status: 'Rejected' });
      await fetchRequests();
      toast.success('İzin talebi reddedildi.');
    } catch (err) {
      toast.error('Reddetme işlemi başarısız: ' + (err.response?.data?.message || 'Bir hata oluştu'));
      console.error('Error rejecting request:', err);
    }
  };

  const handleCreateRequest = async (formData) => {
    try {
      await api.post('/LeaveRequest', formData);
      await fetchRequests();
      setIsModalOpen(false);
      toast.success('İzin isteği başarıyla oluşturuldu!');
    } catch (err) {
      toast.error('İşlem başarısız: ' + (err.response?.data?.message || err.message || 'Bir hata oluştu'));
      console.error('Error creating leave request:', err);
      throw err;
    }
  };

  const filteredRequests = requests.filter(req => statusFilter === '' || req.status === statusFilter);

  const getStatusText = (status) => {
    const statusMap = {
      'Pending': 'Bekliyor',
      'Approved': 'Onaylandı',
      'Rejected': 'Reddedildi'
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
        <button onClick={fetchRequests} className="retry-btn">Tekrar Dene</button>
      </div>
    );
  }

  return (
    <div className="page-container">
      <LeaveRequestModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleCreateRequest}
      />
      
      <div className="page-header">
        <h2>İzin İstekleri</h2>
        {!isAdmin && (
          <button className="add-btn" onClick={() => setIsModalOpen(true)}>+ İzin İsteği Oluştur</button>
        )}
      </div>

      <div className="filters">
        <select value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)} className="filter-select">
          <option value="">Tüm Durumlar</option>
          <option value="Pending">Bekliyor</option>
          <option value="Approved">Onaylandı</option>
          <option value="Rejected">Reddedildi</option>
        </select>
      </div>

      <div className="table-container">
        <table className="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Kullanıcı</th>
              <th>Başlangıç</th>
              <th>Bitiş</th>
              <th>Sebep</th>
              <th>Durum</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {filteredRequests.map(req => (
              <tr key={req.id}>
                <td>{req.id}</td>
                <td><strong>{req.userFullName || `User ID: ${req.userId}`}</strong></td>
                <td>{new Date(req.startDate).toLocaleDateString('tr-TR')}</td>
                <td>{new Date(req.endDate).toLocaleDateString('tr-TR')}</td>
                <td className="reason-cell">{req.reason}</td>
                <td>
                  <span className={`leave-status ${req.status.toLowerCase()}`}>
                    {getStatusText(req.status)}
                  </span>
                </td>
                <td>
                  <div className="action-buttons">
                    {isAdmin && req.status === 'Pending' && (
                      <>
                        <button className="approve-btn" title="Onayla" onClick={() => handleApprove(req.id)}>✔️</button>
                        <button className="reject-btn" title="Reddet" onClick={() => handleReject(req.id)}>❌</button>
                      </>
                    )}
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

export default LeaveRequests;
