# Hexagonal Refactoring
This repository was created to refactor a project to use hexagonal architecture and clean code concepts.  
The inital project was based on an MBA Java project in this [link](https://github.com/devfullcycle/MBA-hexagonal-architecture), and converted to a C# language version.  
Change to the git branch `refactoring` to see each step of the refactoring process.
From the directory of the main project, run the database with `docker compose up` before initalizing the project or running the integration tests.
You can enter the database from the docker desktop or with the exec command `docker exec -it <container_name> bash` and then run the command `mysql -proot` to enter the database and make queries directly.

# TODO
- Remove ValidationException from domain
- Refactor Controller Unit tests using this [tutorial](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-8.0).
