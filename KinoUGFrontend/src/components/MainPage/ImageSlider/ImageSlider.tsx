import { useEffect, useState } from "react";
import { ArrowBigLeft, ArrowBigRight, CircleDot, Circle } from "lucide-react";
import axios from "axios";
import "./ImageSlider.css";

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

const ImageSlider = () => {
  const [imageUrls, setImageUrls] = useState<ImageUrl[]>([]);
  const [imageIndex, setImageIndex] = useState(0);

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
      console.log("Fetched movies:", response.data);
      const images = response.data.map((movie) => ({
        src: movie.image,
        alt: movie.title,
      }));
      console.log("Mapped images:", images);
      return images;
    } catch (error) {
      console.error("Error fetching images:", error);
      return [];
    }
  };

  useEffect(() => {
    const fetchImages = async () => {
      const images = await fetchImagesByIds([8, 9, 11]);
      setImageUrls(images);
      console.log("Set imageUrls:", images);
    };
    fetchImages();
  }, []);

  const showNextImage = () => {
    setImageIndex((index) => (index === imageUrls.length - 1 ? 0 : index + 1));
  };

  const showPrevImage = () => {
    setImageIndex((index) => (index === 0 ? imageUrls.length - 1 : index - 1));
  };

  useEffect(() => {
    const intervalId = setInterval(() => {
      setImageIndex((index) =>
        index === imageUrls.length - 1 ? 0 : index + 1,
      );
    }, 5000);

    return () => clearInterval(intervalId);
  }, [imageUrls.length]);

  if (imageUrls.length === 0) {
    return <div>Loading...</div>;
  }

  console.log("Rendering images:", imageUrls);

  return (
    <div style={{ width: "100%", height: "100%", position: "relative" }}>
      <div
        style={{
          width: "100%",
          height: "100%",
          display: "flex",
          overflow: "hidden",
        }}
      >
        {imageUrls.map((url, index) => (
          <img
            className="img-slider-img"
            style={{
              translate: `${-100 * imageIndex}%`,
            }}
            src={url.src}
            alt={url.alt}
            key={index}
            onError={(e) => {
              console.error("Error loading image:", url.src);
              (e.target as HTMLImageElement).style.display = "none";
            }}
          />
        ))}
      </div>
      <button
        onClick={showPrevImage}
        className="img-slider-btn"
        style={{ left: 0 }}
      >
        <ArrowBigLeft />
      </button>
      <button
        onClick={showNextImage}
        className="img-slider-btn"
        style={{ right: 0 }}
      >
        <ArrowBigRight />
      </button>
      <div
        style={{
          position: "absolute",
          bottom: ".5rem",
          left: "50%",
          translate: "-50%",
          display: "flex",
          gap: ".25rem",
        }}
      >
        {imageUrls.map((_, index) => (
          <button
            className="img-slider-dot-btn"
            key={index}
            onClick={() => setImageIndex(index)}
          >
            {index === imageIndex ? <CircleDot /> : <Circle />}
          </button>
        ))}
      </div>
    </div>
  );
};

export default ImageSlider;
