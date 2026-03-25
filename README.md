QuizMaster – Full Stack Quiz Platform
Overview

QuizMaster is a modern full-stack web application for creating and taking quizzes. It provides educators with tools to build assessments and students with a simple interface to complete quizzes and get instant feedback.

Features
- Authentication using JWT
- Create, edit, and delete quizzes (CRUD)
- Multiple question types (Single, Multiple, Text)
- Instant scoring (client-side)
- Responsive UI (mobile-friendly)

Tech Stack
- Backend: ASP.NET Core 8 (C#)
- Frontend: React + TypeScript (Vite)
- Database: SQLite (EF Core)
- Styling: Bootstrap

How to Run
- cd quiz-client
- npm install
- npm run dev

Then open:
http://localhost:5173

Default Login
- Email: admin@quiz.com
- Password: Admin123!

Architecture
- Frontend: React SPA
- Backend: REST API
- Database: SQLite
- Uses Repository Pattern for clean data access

Security
- JWT authentication
- Protection against SQL Injection (EF Core)
- XSS protection via React

Testing
- 9 unit tests using xUnit + Moq
- Covers CRUD operations and error handling

Future Improvements
- Role-based access (Teacher vs Student)
- Rich text editor for questions
- Migration to SQL Server
