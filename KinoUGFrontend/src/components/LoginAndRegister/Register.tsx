import "./LoginAndRegister.css";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import axios from "axios";

interface FormState {
  name: string;
  surname: string;
  email: string;
  // userName: string;
  password: string;
  confirmPassword: string;
}

interface ErrorsState {
  email?: string;
  userName?: string;
  password?: string;
  confirmPassword?: string;
}

function Register() {
  const [form, setForm] = useState<FormState>({
    name: "",
    surname: "",
    email: "",
    // userName: "",
    password: "",
    confirmPassword: "",
  });

  const [errors, setErrors] = useState<ErrorsState>({});
  const [showPassword, setShowPassword] = useState(false);
  const [triedToSubmit, setTriedToSubmit] = useState(false);

  const navigate = useNavigate();

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prevForm) => ({
      ...prevForm,
      [name]: value,
    }));
    if (triedToSubmit) {
      validateInput(name as keyof FormState, value);
    }
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setTriedToSubmit(true);
    const newErrors = validateForm();
    setErrors(newErrors);

    if (Object.keys(newErrors).length === 0) {
      console.log("Form submitted");
      alert("Rejestracja zakończona pomyślnie!");
    }
    try {
      const response = await axios.post(
        "http://localhost:5268/api/Account/register",
        form,
      );
      if (response.status === 200) {
        alert(response.data.message);
        navigate("/login");
      }
    } catch (error) {
      alert("Rejestracja nie powiodła się");
    }
  };

  const validateInput = (name: keyof FormState, value: string) => {
    const tempErrors = { ...errors };
    switch (name) {
      case "email":
        tempErrors.email = /^\S+@\S+\.\S+$/.test(value)
          ? undefined
          : "Podaj poprawny adres email.";
        break;
      // case "userName":
      //   tempErrors.userName = value.trim()
      //     ? undefined
      //     : "Nazwa użytkownika jest wymagana.";
      //   break;
      case "password":
        tempErrors.password =
          value.length >= 6
            ? undefined
            : "Hasło musi mieć co najmniej 6 znaków.";
        break;
      case "confirmPassword":
        tempErrors.confirmPassword =
          value === form.password ? undefined : "Hasła nie są identyczne.";
        break;
      default:
        break;
    }
    setErrors(tempErrors);
  };

  const validateForm = (): ErrorsState => {
    const newErrors: ErrorsState = {};
    if (!/^\S+@\S+\.\S+$/.test(form.email)) {
      newErrors.email = "Podaj poprawny adres email.";
    }
    // if (!form.userName.trim()) {
    //   newErrors.userName = "Nazwa użytkownika jest wymagana.";
    // }
    if (form.password.length < 6) {
      newErrors.password = "Hasło musi mieć co najmniej 6 znaków.";
    }
    if (form.confirmPassword !== form.password) {
      newErrors.confirmPassword = "Hasła nie są identyczne.";
    }
    return newErrors;
  };

  return (
    <>
      <form className="container" onSubmit={handleSubmit}>
        <div className="header">
          <div className="text">Rejestracja</div>
          <div className="underline"></div>
        </div>
        <div className="inputs">
          <input
            className="input"
            type="text"
            name="name"
            placeholder="Imie"
            value={form.name}
            onChange={handleInputChange}
          />
          <input
            className="input"
            type="text"
            name="surname"
            placeholder="Nazwisko"
            value={form.surname}
            onChange={handleInputChange}
          />
          <input
            className="input"
            type="text"
            name="email"
            placeholder="Email"
            value={form.email}
            onChange={handleInputChange}
          />
          {errors.email && <div className="error">{errors.email}</div>}
          {/*<input*/}
          {/*    className="input"*/}
          {/*    type="text"*/}
          {/*    name="userName"*/}
          {/*    placeholder="Nazwa użytkownika"*/}
          {/*    value={form.userName}*/}
          {/*    onChange={handleInputChange}*/}
          {/*/>*/}
          {errors.userName && <div className="error">{errors.userName}</div>}
          <input
            className="input"
            type={showPassword ? "text" : "password"}
            name="password"
            placeholder="Hasło"
            value={form.password}
            onChange={handleInputChange}
          />
          {errors.password && <div className="error">{errors.password}</div>}
          <input
            className="input"
            type={showPassword ? "text" : "password"}
            name="confirmPassword"
            placeholder="Potwierdź hasło"
            value={form.confirmPassword}
            onChange={handleInputChange}
          />
          {errors.confirmPassword && (
            <div className="error">{errors.confirmPassword}</div>
          )}
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
        <button className="submit">Zarejestruj się.</button>
        <ul>
          <li className="have-account">
            <Link to="/login">
              Masz już konto? <span>Zaloguj się!</span>
            </Link>
          </li>
        </ul>
      </form>
    </>
  );
}

export default Register;
