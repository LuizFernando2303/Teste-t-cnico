import { BrowserRouter, Routes, Route } from "react-router-dom";

import Login from "./Pages/Login";
import Register from "./Pages/Register";

import Categories from "./Pages/Categories";
import CreateCategory from "./Pages/CreateCategory";
import CategoryTransactions from "./Pages/CategoryTransactions";
import CreateTransaction from "./Pages/CreateTransaction";

import ProtectedRoute from "./Pages/ProtectedRoute";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route element={<ProtectedRoute />}>
          <Route path="/categories" element={<Categories />} />

          <Route path="/create-category" element={<CreateCategory />} />

          <Route
            path="/category/:id/transactions"
            element={<CategoryTransactions />}
          />

          <Route
            path="/create-transaction/:categoryId"
            element={<CreateTransaction />}
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
