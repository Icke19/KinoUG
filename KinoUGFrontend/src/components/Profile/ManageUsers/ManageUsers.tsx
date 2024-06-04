import { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./ManageUsers.css";
import { useTheme } from "../../ThemeContext";

interface Ticket {
  id: string;
  price: number;
  seatId: number;
  scheduleId: number;
  movieTitle: string;
  movieImage: string;
}

interface User {
  id: string;
  name: string;
  surname: string;
  email: string;
  userTickets?: Ticket[];
}

const ManageUsers = () => {
  const [users, setUsers] = useState<User[]>([]);
  const navigate = useNavigate();
  const { isDarkMode } = useTheme();

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await axios.get(
        "https://localhost:7205/api/User/GetUsers",
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        },
      );
      setUsers(response.data);
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const deleteUser = async (email: string) => {
    try {
      await axios.delete(`https://localhost:7205/api/User/${email}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      fetchUsers();
    } catch (error) {
      console.error("Error deleting user:", error);
    }
  };

  return (
    <div className={`manage-users-container ${isDarkMode ? "dark" : "light"}`}>
      <h1>Manage Users</h1>
      <ul className="manage-users-list">
        {users.map((user) => (
          <li key={user.id} className={isDarkMode ? "dark" : "light"}>
            <h3>
              {user.name} {user.surname}
            </h3>
            <p>{user.email}</p>
            <button onClick={() => deleteUser(user.email)}>Delete</button>
            <h4>Tickets:</h4>
            <div className="ticket-container">
              {(user.userTickets ?? []).map((ticket) => (
                <div
                  key={ticket.id}
                  className={`ticket-item ${isDarkMode ? "dark" : "light"}`}
                >
                  <img
                    src={ticket.movieImage}
                    alt={ticket.movieTitle}
                    width="100"
                  />
                  <div className="ticket-info">
                    <p>
                      <strong>Film:</strong> {ticket.movieTitle}
                    </p>
                    <p>
                      <strong>Data:</strong> {ticket.scheduleId}
                    </p>
                    <p>
                      <strong>Miejsce:</strong> {ticket.seatId}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </li>
        ))}
      </ul>
      <button className="back-button" onClick={() => navigate("/profil")}>
        Back to Profile
      </button>
    </div>
  );
};

export default ManageUsers;
