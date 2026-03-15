import { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { RegisterResponse } from "../Types/Types";

export default function Register() {
  const [email, setEmail] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [age, setAge] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [message, setMessage] = useState<string>("");

  const navigate = useNavigate();

  async function register() {
    setMessage("");

    try {
      const res = await fetch("/api/auth/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email,
          name,
          age: Number(age),
          password,
        }),
      });

      const data: RegisterResponse = await res.json();

      if (!res.ok) {
        throw new Error(data.message || "Erro ao registrar");
      }

      setMessage(data.message || "Registro realizado!");

      setTimeout(() => {
        navigate("/");
      }, 1000);
    } catch (err) {
      if (err instanceof Error) {
        setMessage(err.message);
      } else {
        setMessage("Erro inesperado");
      }
    }
  }

  return (
    <div className="container vh-100 d-flex justify-content-center align-items-center">
      <div style={{ width: "400px" }}>
        <div className="card p-4 shadow">
          <h2 className="text-center mb-4">Registrar</h2>

          <input
            className="form-control mb-3"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <input
            className="form-control mb-3"
            placeholder="Nome"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />

          <input
            className="form-control mb-3"
            type="number"
            placeholder="Idade"
            value={age}
            onChange={(e) => setAge(e.target.value)}
          />

          <input
            className="form-control mb-3"
            type="password"
            placeholder="Senha"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button className="btn btn-primary w-100" onClick={register}>
            Registrar
          </button>

          <button
            className="btn btn-secondary w-100 mt-2"
            onClick={() => navigate("/")}
          >
            Voltar para Login
          </button>

          {message && (
            <div className="alert alert-info mt-3 text-center">{message}</div>
          )}
        </div>
      </div>
    </div>
  );
}
