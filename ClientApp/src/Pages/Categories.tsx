import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { CategoryPurposeLabel } from "../Types/Types";
import type { Category } from "../Types/Types";
import type { CategoryResponse } from "../Types/Types";

export default function Categories() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [message, setMessage] = useState<string>("");

  const navigate = useNavigate();

  useEffect(() => {
    loadCategories();
  }, []);

  async function loadCategories() {
    try {
      const userStr = localStorage.getItem("user");

      if (!userStr) {
        navigate("/");
        return;
      }

      const user = JSON.parse(userStr);

      const res = await fetch(`/api/category/user/${user.id}`);

      if (!res.ok) {
        throw new Error();
      }

      const data: CategoryResponse | Category[] = await res.json();

      const list = Array.isArray(data)
        ? data
        : (data.categories ?? data.data ?? []);

      setCategories(list);
    } catch {
      setMessage("Erro ao carregar categorias");
    }
  }

  function goToCreateCategory() {
    navigate("/create-category");
  }

  function openCategoryTransactions(id: number) {
    navigate(`/category/${id}/transactions`);
  }

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between mb-4">
        <h2>Categorias</h2>

        <button className="btn btn-success" onClick={goToCreateCategory}>
          Nova Categoria
        </button>
      </div>

      {message && <div className="alert alert-danger">{message}</div>}

      <div className="row">
        {categories.map((cat) => (
          <div key={cat.id} className="col-md-4 mb-3">
            <div
              className="card shadow"
              style={{ cursor: "pointer" }}
              onClick={() => openCategoryTransactions(cat.id)}
            >
              <div className="card-body">
                <h5>{cat.description}</h5>

                <p>
                  Propósito:{" "}
                  {CategoryPurposeLabel[cat.purpose] ?? "Desconhecido"}
                </p>

                <p>UserId: {cat.userId}</p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
