import { useNavigate } from 'react-router-dom';
import './Header.css';

function Header() {
  const navigate = useNavigate();
  const fullName = localStorage.getItem('fullName') || 'Admin';
  const role = localStorage.getItem('role') || 'Admin';

  const handleLogout = () => {
    localStorage.clear();
    navigate('/login');
  };

  return (
    <header className="header">
      <div className="header-title">
        <h1>Dashboard</h1>
      </div>
      <div className="header-user">
        <div className="user-info">
          <span className="user-name">{fullName}</span>
          <span className="user-role">{role}</span>
        </div>
        <button onClick={handleLogout} className="logout-btn">
          Çıkış Yap
        </button>
      </div>
    </header>
  );
}

export default Header;
