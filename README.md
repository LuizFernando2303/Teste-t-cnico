# Rodar o projeto

### 1. Instalar dependências do frontend

```bash
cd ClientApp
npm install
cd ..

2. Criar banco de dados
dotnet ef migrations add InitialCreate
dotnet ef database update

3. Iniciar aplicação
dotnet run
