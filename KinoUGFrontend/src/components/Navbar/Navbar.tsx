import { Link } from "react-router-dom";
import "./Navbar.css";
import PriceList from "../PriceList/PriceList.tsx";
import { useTheme } from "../ThemeContext";
import { useState } from "react";

interface Props {
  isModalOpen: boolean;
  hideModal: () => void;
  onButtonClick: () => void;
}

function Navbar({ isModalOpen, hideModal, onButtonClick }: Props) {
  const { isDarkMode, toggleDarkMode } = useTheme();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const isLoggedIn = () => {
    return !!localStorage.getItem("token");
  };

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <div>
      <PriceList isOpen={isModalOpen} onClose={hideModal} />
      <nav className="nav">
        <Link to="/" className="site-title">
          Moje Kino
        </Link>
        <button className="menu-toggle-button" onClick={toggleMenu}>
          {isMenuOpen ? "X" : "☰"}
        </button>
        <ul className={`nav-links ${isMenuOpen ? "open" : ""}`}>
          <li>
            <button onClick={toggleDarkMode} className="mode-toggle-button">
              {isDarkMode ? "Light Mode" : "Dark Mode"}
            </button>
          </li>
          <li>
            <Link to="/repertuar">Repertuar</Link>
          </li>
          <li>
            <a onClick={onButtonClick} className="price-list-button">
              Cennik
            </a>
          </li>
          <li>
            {isLoggedIn() ? (
              <Link to="/profil">Konto</Link>
            ) : (
              <Link to="/login">Konto</Link>
            )}
          </li>
        </ul>
      </nav>
    </div>
  );
}

export default Navbar;
