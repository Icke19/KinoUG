import React, { useState, useEffect } from "react";
import axios from "axios";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import "./SalaKinowa.css";

interface Seat {
  seatId: number;
  row: number;
  column: number;
  isReserved?: boolean;
}

interface Hall {
  hallId: number;
  seats: Seat[];
}

interface Ticket {
  seatId: number;
  scheduleId: number;
}

const fetchHall = async (hallId: number): Promise<Hall> => {
  try {
    const response = await axios.get<Hall>(`https://localhost:7205/api/Hall/1`);
    console.log("Fetched hall data:", response.data); // Debugging
    return response.data;
  } catch (error) {
    console.error("Error fetching hall data:", error);
    return { hallId, seats: [] };
  }
};

const fetchReservedSeats = async (scheduleId: number): Promise<number[]> => {
  try {
    const response = await axios.get<Ticket[]>(
      `https://localhost:7205/api/Ticket/GetTicket`,
    );
    console.log("Fetched tickets:", response.data);
    const reservedSeats = response.data
      .filter((ticket) => ticket.scheduleId === scheduleId)
      .map((ticket) => ticket.seatId);
    return reservedSeats;
  } catch (error) {
    console.error("Error fetching reserved seats:", error);
    return [];
  }
};

const SalaKinowa: React.FC = () => {
  const { id } = useParams<{ id: string }>(); // This is the scheduleId
  const hallId = 1; // Stałe hallId
  const [hall, setHall] = useState<Hall>({ hallId, seats: [] });
  const [selectedSeats, setSelectedSeats] = useState<number[]>([]);
  const navigate = useNavigate();
  const location = useLocation();

  const loadHallData = async () => {
    const hallData = await fetchHall(hallId);
    const reservedSeatIds = await fetchReservedSeats(Number(id));
    hallData.seats.forEach((seat) => {
      seat.isReserved = reservedSeatIds.includes(seat.seatId);
    });
    console.log("Setting hall data with reserved seats:", hallData); // Debugging
    setHall(hallData);
  };

  useEffect(() => {
    loadHallData();
  }, [id]);

  useEffect(() => {
    if (location.state && location.state.reload) {
      loadHallData();
    }
  }, [location.state]);

  const toggleSeatSelection = (seatId: number) => {
    setSelectedSeats((prevSelectedSeats) =>
      prevSelectedSeats.includes(seatId)
        ? prevSelectedSeats.filter((id) => id !== seatId)
        : [...prevSelectedSeats, seatId],
    );
  };

  const handlePurchase = () => {
    if (selectedSeats.length === 0) {
      alert("Proszę wybrać co najmniej jedno miejsce.");
      return;
    }
    navigate("/koszyk", {
      state: { selectedSeats, scheduleId: id, reload: true },
    });
  };

  return (
    <div className="hall">
      <div className="screen">Ekran</div>
      <div className="seats">
        {hall.seats.length > 0 ? (
          hall.seats.map((seat) => (
            <div
              key={seat.seatId}
              className={`seat ${seat.isReserved ? "reserved" : ""} ${
                selectedSeats.includes(seat.seatId) ? "selected" : ""
              }`}
              onClick={() =>
                !seat.isReserved && toggleSeatSelection(seat.seatId)
              }
            >
              {seat.row}-{seat.column}
            </div>
          ))
        ) : (
          <p>Brak dostępnych miejsc.</p>
        )}
      </div>
      {hall.seats.length > 0 && (
        <button onClick={handlePurchase}>Zakup bilet</button>
      )}
    </div>
  );
};

export default SalaKinowa;
