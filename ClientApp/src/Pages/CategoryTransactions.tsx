import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import type { Transaction } from "../Types/Types";
import { TransactionTypeLabel } from "../Types/Types";
import type { TransactionResponse } from "../Types/Types";

export default function CategoryTransactions() {
  const { id } = useParams<{ id: string }>();

  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [message, setMessage] = useState<string>("");

  const navigate = useNavigate();

  useEffect(() => {
    loadTransactions();
  }, [id]);

  async function loadTransactions() {
    try {
      if (!id) {
        throw new Error("Categoria inválida");
      }

      const res = await fetch(`/api/transaction/category/${id}`);

      if (!res.ok) {
        throw new Error("Erro ao carregar transações");
      }

      const data: TransactionResponse | Transaction[] = await res.json();

      const list = Array.isArray(data)
        ? data
        : (data.transactions ?? data.data ?? []);

      setTransactions(list);
    } catch (error) {
      if (error instanceof Error) {
        setMessage(error.message);
      } else {
        setMessage("Erro inesperado");
      }
    }
  }

  function goToCategories() {
    navigate("/categories");
  }

  function createTransaction() {
    navigate(`/create-transaction/${id}`);
  }

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2>Transações da Categoria</h2>

        <div>
          <button className="btn btn-secondary me-2" onClick={goToCategories}>
            Voltar para Categorias
          </button>

          <button className="btn btn-success" onClick={createTransaction}>
            Nova Transação
          </button>
        </div>
      </div>

      {message && <div className="alert alert-danger">{message}</div>}

      <div className="row mt-3">
        {transactions.map((t) => (
          <div key={t.id} className="col-md-4 mb-3">
            <div className="card shadow">
              <div className="card-body">
                <h5>{t.description}</h5>
                <p>Valor: R$ {t.value}</p>
                <p>Tipo: {TransactionTypeLabel[t.type]}</p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
