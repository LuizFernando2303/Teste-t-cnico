# Rodar o projeto

```bash
git clone https://github.com/LuizFernando2303/Teste-t-cnico.git
cd Teste-t-cnico/

dotnet restore

cd ClientApp
npm install
cd ..

dotnet ef migrations add InitialCreate
dotnet ef database update

dotnet run
