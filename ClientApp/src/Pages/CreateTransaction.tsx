import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import type { User } from "../Types/Types";
import type { CreateTransactionResponse } from "../Types/Types";

export default function CreateTransaction() {
  const { categoryId } = useParams<{ categoryId: string }>();
  const navigate = useNavigate();

  const [description, setDescription] = useState<string>("");
  const [value, setValue] = useState<string>("");
  const [type, setType] = useState<number>(0);
  const [message, setMessage] = useState<string>("");

  async function createTransaction() {
    try {
      const userString = localStorage.getItem("user");

      if (!userString) {
        throw new Error("Usuário não autenticado");
      }

      if (!categoryId) {
        throw new Error("Categoria inválida");
      }

      const user: User = JSON.parse(userString);

      const res = await fetch("/api/transaction/create", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          description,
          value: Number(value),
          type,
          categoryId: Number(categoryId),
          userId: user.id,
        }),
      });

      const data: CreateTransactionResponse = await res.json();

      if (!res.ok) {
        throw new Error(data.message || "Erro ao criar transação");
      }

      navigate(`/category/${categoryId}/transactions`);
    } catch (error) {
      if (error instanceof Error) {
        setMessage(error.message);
      } else {
        setMessage("Erro inesperado");
      }
    }
  }

  function handleTypeChange(e: React.ChangeEvent<HTMLSelectElement>) {
    setType(Number(e.target.value));
  }

  return (
    <div className="container mt-4">
      <h2>Nova Transação</h2>

      {message && <div className="alert alert-danger">{message}</div>}

      <div className="mt-3">
        <div className="mb-3">
          <label className="form-label">Descrição</label>

          <input
            className="form-control"
            maxLength={400}
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Valor</label>

          <input
            type="number"
            step="0.01"
            className="form-control"
            value={value}
            onChange={(e) => setValue(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Tipo</label>

          <select
            className="form-control"
            value={type}
            onChange={handleTypeChange}
          >
            <option value={0}>Receita</option>
            <option value={1}>Despesa</option>
          </select>
        </div>

        <button className="btn btn-primary" onClick={createTransaction}>
          Criar
        </button>
      </div>
    </div>
  );
}
