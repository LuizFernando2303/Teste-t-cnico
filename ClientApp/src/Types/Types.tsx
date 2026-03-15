export type Category = {
  id: number;
  userId: number;
  description: string;
  purpose: CategoryPurpose;
};

export type Transaction = {
  id: number;
  description: string;
  value: number;
  type: TransactionType;
};

export type LoginResponse = {
  user: {
    id: number;
    name: string;
    email: string;
  };
};

export type CategoryResponse = {
  categories?: Category[];
  data?: Category[];
};

export type CreateCategoryResponse = {
  message: string;
};

export type TransactionResponse = {
  transactions?: Transaction[];
  data?: Transaction[];
};

export type CreateTransactionResponse = {
  message?: string;
};

export type User = {
  id: number;
};

export type RegisterResponse = {
  message: string;
};

export const TransactionTypeLabel: Record<TransactionType, string> = {
  0: "Receita",
  1: "Despesa",
};

export const CategoryPurposeLabel: Record<CategoryPurpose, string> = {
  0: "Receita",
  1: "Despesa",
  2: "Ambos",
};

export const CategoryPurpose = {
  Income: 0,
  Expense: 1,
  Both: 2,
} as const;
export type CategoryPurpose =
  (typeof CategoryPurpose)[keyof typeof CategoryPurpose];

export const TransactionType = {
  Income: 0,
  Expense: 1,
} as const;
export type TransactionType =
  (typeof TransactionType)[keyof typeof TransactionType];
