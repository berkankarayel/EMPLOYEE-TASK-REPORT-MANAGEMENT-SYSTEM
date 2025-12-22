import { NavLink } from 'react-router-dom';
import './Sidebar.css';

function Sidebar() {
  return (
    <div className="sidebar">
      <div className="sidebar-header">
        <h2>Task Management</h2>
      </div>
      <nav className="sidebar-nav">
        <NavLink to="/dashboard/users" className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}>
          <span className="nav-icon">👥</span>
          Kullanıcılar
        </NavLink>
        <NavLink to="/dashboard/tasks" className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}>
          <span className="nav-icon">📋</span>
          Görevler
        </NavLink>
        <NavLink to="/dashboard/leave-requests" className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}>
          <span className="nav-icon">🏖️</span>
          İzin İstekleri
        </NavLink>
        <NavLink to="/dashboard/logs" className={({ isActive }) => isActive ? 'nav-item active' : 'nav-item'}>
          <span className="nav-icon">📝</span>
          Log Kayıtları
        </NavLink>
      </nav>
    </div>
  );
}

export default Sidebar;
