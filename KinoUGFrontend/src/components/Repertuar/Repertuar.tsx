import React, { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import "./Repertuar.css";

interface MinSchedule {
  id: number;
  date: string;
  movieTitle: string;
  image: string;
  description: string;
}

const fetchScheduleList = async (): Promise<MinSchedule[]> => {
  try {
    const response = await axios.get<MinSchedule[]>(
      "https://localhost:7205/api/Schedule/all",
    );
    return response.data;
  } catch (error) {
    console.error("Error fetching schedule list:", error);
    return [];
  }
};

const Repertuar: React.FC = () => {
  const [scheduleList, setScheduleList] = useState<MinSchedule[]>([]);

  useEffect(() => {
    const fetchSchedule = async () => {
      const scheduleListFromBackend = await fetchScheduleList();
      setScheduleList(scheduleListFromBackend);
    };
    fetchSchedule();
  }, []);

  return (
    <div className="repertuar">
      {scheduleList.map((schedule) => (
        <div key={schedule.id} className="movie">
          <img
            src={schedule.image}
            alt={schedule.movieTitle}
            className="movie-image"
          />
          <div className="movie-details">
            <h2>{schedule.movieTitle}</h2>
            <p>{new Date(schedule.date).toLocaleString()}</p>
            <p>{schedule.description}</p>
            <ul>
              <li className="buy-ticket">
                <Link to={`/salakinowa/${schedule.id}`}>Kup bilet</Link>
              </li>
            </ul>
          </div>
        </div>
      ))}
    </div>
  );
};

export default Repertuar;
