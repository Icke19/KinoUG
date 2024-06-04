import "./Film.css";
import axios from "axios";
import { useEffect, useState } from "react";
import { useTheme } from "../../ThemeContext";

interface Film {
  id: number;
  name: string;
  src: string;
  alt: string;
}

interface Movie {
  id: number;
  title: string;
  description: string;
  image: string;
}

interface ImageUrl {
  src: string;
  alt: string;
}

export default function Film() {
  const [hits, setHits] = useState<ImageUrl[]>([]);
  const [upcoming, setUpcoming] = useState<ImageUrl[]>([]);
  const { isDarkMode } = useTheme();

  useEffect(() => {
    const fetchImagesByIds = async (ids: number[]) => {
      try {
        const response = await axios.get<Movie[]>(
          `https://localhost:7205/api/Movie/GetMoviesByIds`,
          {
            params: { ids },
            paramsSerializer: (params) => {
              return Object.keys(params)
                .map((key) =>
                  params[key].map((val: number) => `${key}=${val}`).join("&"),
                )
                .join("&");
            },
          },
        );
        const images = response.data.map((movie) => ({
          src: movie.image,
          alt: movie.title,
        }));
        return images;
      } catch (error) {
        console.error("Error fetching images:", error);
        return [];
      }
    };

    const fetchImages = async () => {
      const hitsImages = await fetchImagesByIds([12, 13, 14]);
      setHits(hitsImages);
      const upcomingImages = await fetchImagesByIds([19, 1004, 1005]);
      setUpcoming(upcomingImages);
    };

    fetchImages();
  }, []);

  return (
    <div className="FilmMenu">
      <h1 className={`TextHit ${isDarkMode ? "dark" : "light"}`}>
        NAJWIĘKSZE HITY!
      </h1>
      <ul className={`films ${isDarkMode ? "dark" : "light"}`}>
        {hits.map((film, idx) => (
          <img
            src={film.src}
            alt={film.alt}
            key={idx}
            className={isDarkMode ? "dark" : "light"}
          />
        ))}
      </ul>
      <h1 className={`TextHit ${isDarkMode ? "dark" : "light"}`}>WKRÓTCE!</h1>
      <ul className={`films ${isDarkMode ? "dark" : "light"}`}>
        {upcoming.map((film, idx) => (
          <img
            src={film.src}
            alt={film.alt}
            key={idx}
            className={isDarkMode ? "dark" : "light"}
          />
        ))}
      </ul>
    </div>
  );
}
