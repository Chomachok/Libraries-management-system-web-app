import { Routes, Route, useLocation } from 'react-router-dom';
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

function PageWrapper({ children }) {
  return <div className="page-enter">{children}</div>;
}

export default function App() {
  const location = useLocation();

  return (
    <>
      <Header />
      <main style={{ padding: '1rem 2rem' }}>
        <Routes location={location}>
          <Route
            path="/"
            element={
              <PageWrapper key={location.pathname}>
                <HomePage />
              </PageWrapper>
            }
          />
          <Route
            path="/login"
            element={
              <PageWrapper key={location.pathname}>
                <LoginPage />
              </PageWrapper>
            }
          />
          <Route
            path="/register"
            element={
              <PageWrapper key={location.pathname}>
                <RegisterPage />
              </PageWrapper>
            }
          />
          <Route
            path="/libraries"
            element={
              <PageWrapper key={location.pathname}>
                <LibrariesPage />
              </PageWrapper>
            }
          />
          <Route
            path="/books"
            element={
              <PageWrapper key={location.pathname}>
                <BooksPage />
              </PageWrapper>
            }
          />

          {/* Читатель */}
          <Route
            path="/my-books"
            element={
              <ProtectedRoute roles={['Reader']}>
                <PageWrapper key={location.pathname}>
                  <MyBooksPage />
                </PageWrapper>
              </ProtectedRoute>
            }
          />
          <Route
            path="/history"
            element={
              <ProtectedRoute roles={['Reader']}>
                <PageWrapper key={location.pathname}>
                  <HistoryPage />
                </PageWrapper>
              </ProtectedRoute>
            }
          />
          <Route
            path="/profile"
            element={
              <ProtectedRoute roles={['Reader', 'Librarian']}>
                <PageWrapper key={location.pathname}>
                  <ProfilePage />
                </PageWrapper>
              </ProtectedRoute>
            }
          />

          {/* Библиотекарь */}
          <Route
            path="/dashboard"
            element={
              <ProtectedRoute roles={['Librarian']}>
                <PageWrapper key={location.pathname}>
                  <DashboardPage />
                </PageWrapper>
              </ProtectedRoute>
            }
          />
          <Route
            path="/manage-books"
            element={
              <ProtectedRoute roles={['Librarian']}>
                <PageWrapper key={location.pathname}>
                  <ManageBooksPage />
                </PageWrapper>
              </ProtectedRoute>
            }
          />
          <Route
            path="/manage-readers"
            element={
              <ProtectedRoute roles={['Librarian']}>
                <PageWrapper key={location.pathname}>
                  <ManageReadersPage />
                </PageWrapper>
              </ProtectedRoute>
            }
          />
          <Route
            path="/manage-checkouts"
            element={
              <ProtectedRoute roles={['Librarian']}>
                <PageWrapper key={location.pathname}>
                  <ManageCheckoutsPage />
                </PageWrapper>
              </ProtectedRoute>
            }
          />
        </Routes>
      </main>
    </>
  );
}