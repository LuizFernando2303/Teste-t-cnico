import { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { LoginResponse } from "../Types/Types";

export default function Login() {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [message, setMessage] = useState<string>("");

  const navigate = useNavigate();

  async function handleLogin() {
    try {
      setMessage("");

      const res = await fetch("/api/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ email, password }),
      });

      if (!res.ok) {
        throw new Error("Email ou senha inválidos");
      }

      const data: LoginResponse = await res.json();

      localStorage.setItem("user", JSON.stringify(data.user));

      navigate("/categories");
    } catch (error) {
      if (error instanceof Error) {
        setMessage(error.message);
      } else {
        setMessage("Erro inesperado");
      }
    }
  }

  function goToRegister() {
    navigate("/register");
  }

  return (
    <div className="container-fluid min-vh-100 d-flex justify-content-center align-items-center">
      <div className="card p-4 shadow" style={{ width: "400px" }}>
        <h2 className="text-center mb-4">Login</h2>

        <input
          className="form-control mb-3"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />

        <input
          className="form-control mb-3"
          type="password"
          placeholder="Senha"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <button className="btn btn-secondary w-100 mb-2" onClick={goToRegister}>
          Criar conta
        </button>

        <button className="btn btn-primary w-100" onClick={handleLogin}>
          Entrar
        </button>

        {message && (
          <div className="alert alert-info mt-3 text-center">{message}</div>
        )}
      </div>
    </div>
  );
}
