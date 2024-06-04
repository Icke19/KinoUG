import { useState } from "react";
import Navbar from "./components/Navbar/Navbar.tsx";
import MainPage from "./components/MainPage/MainPage.tsx";
import Repertuar from "./components/Repertuar/Repertuar.tsx";
import Login from "./components/LoginAndRegister/Login.tsx";
import Register from "./components/LoginAndRegister/Register.tsx";
import SalaKinowa from "./components/SalaKinowa/SalaKinowa.tsx";
import Cart from "./components/Cart/Cart.tsx";
import { Routes, Route, Navigate } from "react-router-dom";
import Profile from "./components/Profile/Profile.tsx";
import ManageMovies from "./components/Profile/ManageMovies/ManageMovies.tsx";
import ManageUsers from "./components/Profile/ManageUsers/ManageUsers.tsx";
import { ThemeProvider } from "./components/ThemeContext.tsx";

const PrivateRoute: React.FC<{ component: React.FC }> = ({
  component: Component,
}) => {
  const isAuthenticated = !!localStorage.getItem("token");
  return isAuthenticated ? <Component /> : <Navigate to="/login" />;
};

function App() {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

  function showModal() {
    setIsModalOpen(true);
  }

  function hideModal() {
    setIsModalOpen(false);
  }

  return (
    <>
      <ThemeProvider>
        <Navbar
          onButtonClick={showModal}
          isModalOpen={isModalOpen}
          hideModal={hideModal}
        />
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/repertuar" element={<Repertuar />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
            path="/salakinowa/:id"
            element={<PrivateRoute component={SalaKinowa} />}
          />
          <Route path="/koszyk" element={<PrivateRoute component={Cart} />} />
          <Route path="/profil" element={<Profile />} />
          <Route path="/manage-movies" element={<ManageMovies />} />
          <Route path="/manage-users" element={<ManageUsers />} />
        </Routes>
      </ThemeProvider>
    </>
  );
}

export default App;
