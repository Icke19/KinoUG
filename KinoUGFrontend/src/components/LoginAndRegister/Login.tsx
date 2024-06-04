import "./LoginAndRegister.css";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import axios from "axios";

interface LoginForm {
  email: string;
  password: string;
}

interface LoginErrors {
  email?: string;
  password?: string;
}

function Login() {
  const [form, setForm] = useState<LoginForm>({
    email: "",
    password: "",
  });

  const [errors, setErrors] = useState<LoginErrors>({});
  const [showPassword, setShowPassword] = useState(false);
  const navigate = useNavigate();

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm({
      ...form,
      [name]: value,
    });
    if (errors[name as keyof LoginErrors]) {
      validateInput(name, value);
    }
  };

  const validateInput = (name: string, value: string) => {
    let tempErrors = { ...errors };
    if (name === "email" && !/^\S+@\S+\.\S+$/.test(value)) {
      tempErrors.email = "Podaj poprawny adres email.";
    } else if (name === "password" && value.length < 6) {
      tempErrors.password = "Hasło musi mieć co najmniej 6 znaków.";
    } else {
      delete tempErrors[name as keyof LoginErrors];
    }
    setErrors(tempErrors);
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const newErrors = validateForm();
    setErrors(newErrors);

    if (Object.keys(newErrors).length === 0 && form.email && form.password) {
      try {
        const response = await axios.post(
          "https://localhost:7205/api/Account/login",
          form,
        );
        console.log("API response:", response.data); // Debug log

        if (response.data.token) {
          console.log("Form submitted");
          alert("Logowanie się powiodło!");
          localStorage.setItem("token", response.data.token);
          navigate("/");
        } else {
          setErrors({ password: "Nieprawidłowy email lub hasło." });
        }
      } catch (error) {
        console.error("There was an error logging in!", error);
        setErrors({ password: "Nieprawidłowy email lub hasło." });
      }
    }
  };

  const validateForm = (): LoginErrors => {
    let tempErrors: LoginErrors = {};
    if (!/^\S+@\S+\.\S+$/.test(form.email)) {
      tempErrors.email = "Podaj poprawny adres email.";
    }
    if (form.password.length < 6) {
      tempErrors.password = "Hasło musi mieć co najmniej 6 znaków.";
    }
    return tempErrors;
  };

  return (
    <>
      <form className="container" onSubmit={handleSubmit}>
        <div className="header">
          <div className="text">Logowanie</div>
          <div className="underline"></div>
        </div>
        <div className="inputs">
          <input
            className="input"
            type="text"
            name="email"
            placeholder="Email"
            value={form.email}
            onChange={handleInputChange}
          />
          {errors.email && <div className="error">{errors.email}</div>}
          <input
            className="input"
            type={showPassword ? "text" : "password"}
            name="password"
            placeholder="Hasło"
            value={form.password}
            onChange={handleInputChange}
          />
          {errors.password && <div className="error">{errors.password}</div>}
        </div>
        <div>
          <label htmlFor="check" className="check">
            Pokaż hasło
          </label>
          <input
            id="check"
            className="checkbox"
            type="checkbox"
            checked={showPassword}
            onChange={(e) => setShowPassword(e.target.checked)}
          />
        </div>
        <button className="submit">Zaloguj się</button>
        <ul>
          <li className="create-account">
            <Link to="/register">
              Nie posiadasz jeszcze konta? <span>Zarejestruj się!</span>
            </Link>
          </li>
        </ul>
      </form>
    </>
  );
}

export default Login;
