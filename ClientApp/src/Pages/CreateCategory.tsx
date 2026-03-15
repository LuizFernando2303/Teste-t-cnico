import { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { User } from "../Types/Types";
import { CategoryPurpose } from "../Types/Types";
import type { CreateCategoryResponse } from "../Types/Types";

export default function CreateCategory() {
  const [description, setDescription] = useState<string>("");
  const [purpose, setPurpose] = useState<CategoryPurpose>(
    CategoryPurpose.Income
  );
  const [message, setMessage] = useState<string>("");

  const navigate = useNavigate();

  async function createCategory() {
    try {
      setMessage("");

      const userString = localStorage.getItem("user");

      if (!userString) {
        throw new Error("Usuário não encontrado");
      }

      const user: User = JSON.parse(userString);

      const res = await fetch("/api/category/create", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          userId: user.id,
          description,
          purpose,
        }),
      });

      const data: CreateCategoryResponse = await res.json();

      if (!res.ok) {
        throw new Error(data.message || "Erro ao criar categoria");
      }

      navigate("/categories");
    } catch (error) {
      if (error instanceof Error) {
        setMessage(error.message);
      } else {
        setMessage("Erro inesperado");
      }
    }
  }

  function goBack() {
    navigate("/categories");
  }

  function handlePurposeChange(e: React.ChangeEvent<HTMLSelectElement>) {
    setPurpose(Number(e.target.value) as CategoryPurpose);
  }

  return (
    <div className="container vh-100 d-flex justify-content-center align-items-center">
      <div style={{ width: "400px" }}>
        <div className="card p-4 shadow">
          <h2 className="text-center mb-4">Criar Categoria</h2>

          <input
            className="form-control mb-3"
            placeholder="Descrição"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />

          <select
            className="form-control mb-3"
            value={purpose}
            onChange={handlePurposeChange}
          >
            <option value={CategoryPurpose.Income}>Receita</option>
            <option value={CategoryPurpose.Expense}>Despesa</option>
            <option value={CategoryPurpose.Both}>Ambos</option>
          </select>

          <button className="btn btn-success w-100" onClick={createCategory}>
            Criar Categoria
          </button>

          <button className="btn btn-secondary w-100 mt-2" onClick={goBack}>
            Voltar para Categorias
          </button>

          {message && (
            <div className="alert alert-info mt-3 text-center">{message}</div>
          )}
        </div>
      </div>
    </div>
  );
}
