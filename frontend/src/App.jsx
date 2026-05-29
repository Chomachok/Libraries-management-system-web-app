import { Routes, Route } from 'react-router-dom';
import Header from './components/Header/Header';
import { ProtectedRoute } from './components/ProtectedRoute/ProtectedRoute';
import HomePage from './pages/HomePage/HomePage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegisterPage from './pages/RegisterPage/RegisterPage';
import LibrariesPage from './pages/LibrariesPage/LibrariesPage';
import BooksPage from './pages/BooksPage/BooksPage';
import MyBooksPage from './pages/MyBooksPage/MyBooksPage';
import HistoryPage from './pages/HistoryPage/HistoryPage';
import ProfilePage from './pages/ProfilePage/ProfilePage';
import DashboardPage from './pages/DashboardPage/DashboardPage';
import ManageBooksPage from './pages/ManageBooksPage/ManageBooksPage';
import ManageReadersPage from './pages/ManageReadersPage/ManageReadersPage';
import ManageCheckoutsPage from './pages/ManageCheckoutsPage/ManageCheckoutsPage';

export default function App() {
  return (
    <>
      <Header />
      <main style={{ padding: '1rem 2rem' }}>
        <Routes>
          {/* Публичные */}
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/libraries" element={<LibrariesPage />} />
          <Route path="/books" element={<BooksPage />} />

          {/* Читатель */}
          <Route path="/my-books" element={
            <ProtectedRoute roles={['Reader']}>
              <MyBooksPage />
            </ProtectedRoute>
          } />
          <Route path="/history" element={
            <ProtectedRoute roles={['Reader']}>
              <HistoryPage />
            </ProtectedRoute>
          } />
          <Route path="/profile" element={
            <ProtectedRoute roles={['Reader', 'Librarian']}>
              <ProfilePage />
            </ProtectedRoute>
          } />

          {/* Библиотекарь */}
          <Route path="/dashboard" element={
            <ProtectedRoute roles={['Librarian']}>
              <DashboardPage />
            </ProtectedRoute>
          } />
          <Route path="/manage-books" element={
            <ProtectedRoute roles={['Librarian']}>
              <ManageBooksPage />
            </ProtectedRoute>
          } />
          <Route path="/manage-readers" element={
            <ProtectedRoute roles={['Librarian']}>
              <ManageReadersPage />
            </ProtectedRoute>
          } />
          <Route path="/manage-checkouts" element={
            <ProtectedRoute roles={['Librarian']}>
              <ManageCheckoutsPage />
            </ProtectedRoute>
          } />
        </Routes>
      </main>
    </>
  );
}