# Book Store API

A RESTful API built with ASP.NET Core, Entity Framework Core, and SQL Server, managing Books, Authors, and Categories with full CRUD operations, search, and pagination.

## Tech Stack

- ASP.NET Core Web API (.NET 10)
- Entity Framework Core (Code-First, migrations)
- SQL Server (LocalDB)
- Swagger / OpenAPI for interactive documentation

## Features

- Full CRUD (Create, Read, Update, Delete) for Books, Authors, and Categories
- One-to-many relationships: an Author has many Books, a Category has many Books
- Search books by title or author name
- Pagination support (`page`, `pageSize` query parameters)
- Request/response DTOs to prevent circular reference issues and control exactly what data is exposed
- Foreign key validation — creating/updating a Book checks that the given AuthorId and CategoryId actually exist before saving
- Referential integrity enforced at the database level (Restrict delete behavior — an Author or Category with existing Books cannot be deleted)

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server LocalDB (comes with Visual Studio)
- Visual Studio 2022 (or later)

### Setup

1. Clone the repository:
2. Open `BookStoreApi.slnx` in Visual Studio.
3. Update the connection string in `appsettings.json` if needed (defaults to LocalDB).
4. Open Package Manager Console and run:
5.    This creates the database and applies all migrations.
5. Run the project (F5). Swagger UI will open automatically at `/swagger`.

## API Endpoints

### Categories
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | /api/Categories | Get all categories |
| GET | /api/Categories/{id} | Get a category by Id |
| POST | /api/Categories | Create a new category |
| PUT | /api/Categories/{id} | Update a category |
| DELETE | /api/Categories/{id} | Delete a category |

### Authors
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | /api/Authors | Get all authors |
| GET | /api/Authors/{id} | Get an author by Id |
| POST | /api/Authors | Create a new author |
| PUT | /api/Authors/{id} | Update an author |
| DELETE | /api/Authors/{id} | Delete an author |

### Books
| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | /api/Books?search=&page=&pageSize= | Get books with optional search and pagination |
| GET | /api/Books/{id} | Get a book by Id |
| POST | /api/Books | Create a new book |
| PUT | /api/Books/{id} | Update a book |
| DELETE | /api/Books/{id} | Delete a book |

## Database Design

- **Category** (1) → (many) **Book**
- **Author** (1) → (many) **Book**
- Delete behavior: Restrict — an Author/Category cannot be deleted while Books still reference it.

## Known Limitations

- No authentication/authorization yet (planned for the next phase)
- No automated tests yet

## Future Improvements

- Add JWT authentication and role-based authorization
- Add unit tests
- Deploy to a cloud environment
