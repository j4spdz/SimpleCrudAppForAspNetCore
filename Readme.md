# SimpleCrudApp

## Steps

- Add migration from root: dotnet ef migrations add InitialCreate -p Persistence/ -s API/
- cd API
- dotnet watch run
- If got issue, drop database and rerun: dotnet ef database drop -p Persistence/ -s API/
