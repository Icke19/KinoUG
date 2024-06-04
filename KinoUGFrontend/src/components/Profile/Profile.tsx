import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { jwtDecode } from "jwt-decode";
import "./Profile.css";
import { useTheme } from "../ThemeContext";

interface UserProfile {
  name: string;
  surname: string;
  email: string;
}

interface Ticket {
  id: number;
  row: number;
  seatId: number;
  hallId: number;
  date: string;
  movieTitle: string;
  userId: string;
  scheduleId: number;
}

interface TokenPayload {
  nameid: string;
  email: string;
  name: string;
  family_name: string;
  unique_name: string;
  role: string[];
}

interface Movie {
  title: string;
  image: string | null;
}

interface Schedule {
  id: number;
  movie: Movie;
}

function Profile() {
  const [user, setUser] = useState<UserProfile | null>(null);
  const [tickets, setTickets] = useState<(Ticket & { movie: Movie | null })[]>(
    [],
  );
  const [loading, setLoading] = useState(true);
  const [isAdmin, setIsAdmin] = useState(false);
  const { isDarkMode } = useTheme();

  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const token = localStorage.getItem("token");
        if (!token) {
          navigate("/login");
          return;
        }

        const response = await axios.get(
          "https://localhost:7205/api/User/GetCurrentUser",
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          },
        );
        setUser(response.data);

        const decodedToken = jwtDecode<TokenPayload>(token);
        setIsAdmin(decodedToken.role.includes("Admin"));
      } catch (error) {
        console.error("Error fetching user profile:", error);
        navigate("/login");
      }
    };

    const fetchUserTickets = async () => {
      try {
        const token = localStorage.getItem("token");
        if (!token) {
          navigate("/login");
          return;
        }

        const decodedToken = jwtDecode<TokenPayload>(token);
        const userIdFromToken = decodedToken.nameid;

        const response = await axios.get(
          "https://localhost:7205/api/Ticket/GetTicket",
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          },
        );

        const userTickets = response.data.filter(
          (ticket: Ticket) => ticket.userId === userIdFromToken,
        );

        const ticketsWithMovies = await Promise.all(
          userTickets.map(async (ticket: Ticket) => {
            const scheduleResponse = await axios.get(
              `https://localhost:7205/api/Schedule/${ticket.scheduleId}`,
              {
                headers: {
                  Authorization: `Bearer ${token}`,
                },
              },
            );
            const schedule: Schedule = scheduleResponse.data;
            return { ...ticket, movie: schedule.movie };
          }),
        );

        setTickets(ticketsWithMovies);
      } catch (error) {
        console.error("Error fetching user tickets:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
    fetchUserTickets();
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  if (!user) {
    return <div>Loading...</div>;
  }

  return (
    <div className={`profile-container ${isDarkMode ? "dark" : "light"}`}>
      <h1 style={{ color: isDarkMode ? "white" : "#1e1e1e" }}>Profile Info</h1>
      <div className="profile-info">
        <p>
          <strong>Name:</strong> {user.name}
        </p>
        <p>
          <strong>Surname:</strong> {user.surname}
        </p>
        <p>
          <strong>Email:</strong> {user.email}
        </p>
      </div>
      <h2>Tickets:</h2>
      <div className="tickets-container">
        {loading ? (
          <p>Loading...</p>
        ) : tickets.length > 0 ? (
          <ul>
            {tickets.map((ticket) => (
              <li key={ticket.id} className={isDarkMode ? "dark" : "light"}>
                {ticket.movie && (
                  <>
                    <p>
                      <strong>Movie:</strong> {ticket.movie.title}
                    </p>
                    {ticket.movie.image ? (
                      <img
                        src={ticket.movie.image}
                        alt={ticket.movie.title}
                        width="100"
                      />
                    ) : (
                      <p>No image available</p>
                    )}
                  </>
                )}
                <p>
                  <strong>Data:</strong> {ticket.date}
                </p>
                <p>
                  <strong>Seat:</strong> {ticket.seatId}
                </p>
              </li>
            ))}
          </ul>
        ) : (
          <p>No purchased tickets.</p>
        )}
      </div>
      {isAdmin && (
        <div className={`admin-section ${isDarkMode ? "dark" : "light"}`}>
          <h2>Admin Section</h2>
          <button onClick={() => navigate("/manage-movies")}>
            Manage Movies
          </button>
          <button onClick={() => navigate("/manage-users")}>
            Manage Users
          </button>
        </div>
      )}
      <button className="logout-button" onClick={handleLogout}>
        Logout
      </button>
    </div>
  );
}

export default Profile;
