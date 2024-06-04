import { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./ManageMovies.css";
import { useTheme } from "../../ThemeContext";

interface Movie {
  id: number;
  title: string;
  description: string;
  image: string | null;
}

const ManageMovies = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [image, setImage] = useState("");
  const navigate = useNavigate();
  const { isDarkMode } = useTheme();

  useEffect(() => {
    fetchMovies();
  }, []);

  const fetchMovies = async () => {
    try {
      const response = await axios.get("https://localhost:7205/api/Movie", {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      setMovies(response.data);
    } catch (error) {
      console.error("Error fetching movies:", error);
    }
  };

  const addMovie = async () => {
    try {
      const newMovie = { title, description, image };
      await axios.post("https://localhost:7205/api/Movie", newMovie, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      fetchMovies();
      setTitle("");
      setDescription("");
      setImage("");
    } catch (error) {
      console.error("Error adding movie:", error);
    }
  };

  const deleteMovie = async (id: number) => {
    try {
      await axios.delete(`https://localhost:7205/api/Movie/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      fetchMovies();
    } catch (error) {
      console.error("Error deleting movie:", error);
    }
  };

  return (
    <div className={`manage-movies-container ${isDarkMode ? "dark" : "light"}`}>
      <h1>Manage Movies</h1>
      <div className="manage-movies-form">
        <input
          type="text"
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <input
          type="text"
          placeholder="Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
        <input
          type="text"
          placeholder="Image URL"
          value={image}
          onChange={(e) => setImage(e.target.value)}
        />
        <button onClick={addMovie}>Add Movie</button>
      </div>
      <ul className="movies-list">
        {movies.map((movie) => (
          <li key={movie.id} className={isDarkMode ? "dark" : "light"}>
            <h3>{movie.title}</h3>
            <p>{movie.description}</p>
            {movie.image && <img src={movie.image} alt={movie.title} />}
            <button onClick={() => deleteMovie(movie.id)}>Delete</button>
          </li>
        ))}
      </ul>
      <button onClick={() => navigate("/profil")}>Back to Profile</button>
    </div>
  );
};

export default ManageMovies;
