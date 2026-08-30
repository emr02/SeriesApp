# SeriesApp

SeriesApp is a C# web application (with HTML/CSS front-end files) for managing and browsing TV/series information. This README provides an overview, development setup, and contribution guidelines. Update the sections below to match your project's specific architecture, configuration, and requirements.

## Features

- View and manage a list of series
- Add, edit, and remove series entries
- Responsive HTML/CSS front-end integrated with a C# backend

> Note: This README is intentionally generic. Please update the Usage and Configuration sections below to match the exact structure of your repository (web project path, database, environment variables, ports, etc.).

## Technology stack

- Backend: C# (.NET)
- Frontend: HTML, CSS

## Prerequisites

- .NET SDK 6.0 or later (https://dotnet.microsoft.com/)
- A code editor (Visual Studio, Visual Studio Code, Rider, etc.)

## Getting started (local development)

1. Clone the repository

   git clone https://github.com/emr02/SeriesApp.git
   cd SeriesApp

2. Restore dependencies

   dotnet restore

3. Build the solution

   dotnet build

4. Run the app

   - If your solution contains a web project at the repository root, you can run:

     dotnet run

   - If the web project is in a subfolder, run from that folder instead, for example:

     cd src/SeriesApp
     dotnet run

5. Open a browser and navigate to the address shown in the console (commonly https://localhost:5001 or http://localhost:5000).

## Configuration

- If your app uses a database, update the connection string in appsettings.json or environment variables.
- Add any required secrets or keys to your user secrets or environment variables for local development.

## Testing

- If this repository contains unit or integration tests, run them with:

  dotnet test

## Common tasks

- Format code (C#): dotnet format
- Run database migrations (if using EF Core): dotnet ef database update

## Contributing

Contributions are welcome. To contribute:

1. Fork the repository
2. Create a feature branch: `git checkout -b feat/your-feature`
3. Commit your changes and open a pull request

Please follow the existing coding style and add tests for new features or bug fixes.

## License

This project does not yet have a license file. Add a LICENSE file (for example MIT) if you want to open-source it.

## Contact

For questions, open an issue or contact the repository owner.
