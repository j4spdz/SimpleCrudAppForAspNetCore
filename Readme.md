# SimpleCrudApp

## Steps

- Edit connection string in appsettings
- cd API
- dotnet watch run
- Check database is running: localhost:5000/health
- Check endpoints: localhost:5000
- If got issue, drop database and rerun: dotnet ef database drop -p Infrastructure/ -s API/
- Add migration from root: dotnet ef migrations add InitialCreate -p Infrastructure/ -s API/

## References

- [Building ASP.NET Core Web APIs with Clean Architecture](https://fullstackmark.com/post/18/building-aspnet-core-web-apis-with-clean-architecture)
- [Complete guide to building an app with .Net Core and React](https://www.udemy.com/course/complete-guide-to-building-an-app-with-net-core-and-react)
