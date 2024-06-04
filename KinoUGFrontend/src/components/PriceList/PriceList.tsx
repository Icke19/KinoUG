import "./PriceList.css";
import { useEffect } from "react";

interface PriceListProps {
  isOpen: boolean;
  onClose: () => void;
}

function PriceList({ isOpen, onClose }: PriceListProps) {
  useEffect(() => {
    const handleOutsideClick = (event: MouseEvent) => {
      const modal = document.getElementById("modal-content");
      if (modal && !modal.contains(event.target as Node)) {
        onClose();
      }
    };

    if (isOpen) {
      document.addEventListener("mousedown", handleOutsideClick);
    } else {
      document.removeEventListener("mousedown", handleOutsideClick);
    }

    return () => {
      document.removeEventListener("mousedown", handleOutsideClick);
    };
  }, [isOpen, onClose]);

  return (
    <>
      {isOpen && (
        <div className="modal-overlay">
          <div id="modal-content" className="modal-content">
            <button className="close-button" onClick={onClose}>
              &times;
            </button>
            <h2>Drodzy użytkownicy</h2>
            <p>
              Ceny biletów w naszym kinie wynoszą dla wszystkich klientów 50zł.
            </p>
          </div>
        </div>
      )}
    </>
  );
}

export default PriceList;
