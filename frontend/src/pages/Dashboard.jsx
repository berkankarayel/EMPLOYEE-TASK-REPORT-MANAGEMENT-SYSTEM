import { Routes, Route, Navigate } from 'react-router-dom';
import Sidebar from '../components/Sidebar';
import Header from '../components/Header';
import Users from './Users';
import Tasks from './Tasks';
import LeaveRequests from './LeaveRequests';
import SystemLogs from './SystemLogs';
import './Dashboard.css';

function Dashboard() {
  return (
    <div className="dashboard-layout">
      <Sidebar />
      <div className="main-content">
        <Header />
        <div className="content-area">
          <Routes>
            <Route path="/" element={<Navigate to="/dashboard/users" replace />} />
            <Route path="/users" element={<Users />} />
            <Route path="/tasks" element={<Tasks />} />
            <Route path="/leave-requests" element={<LeaveRequests />} />
            <Route path="/logs" element={<SystemLogs />} />
          </Routes>
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
