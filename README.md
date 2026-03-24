# TecTreasure

Web-based game developed for Sorteos Tec to attract new customers and enhance engagement with existing ones.

## Setup Instructions

### 1. Clone the repository

### 2. Database Setup
The `dataBase.sql` file contains the database structure and initial data. Make sure you have MySQL installed and running, then create a new database and execute the script (either via the command line or a database management tool)

### 3. REST API Service
- In the `ServiceREST` directory, open the `UsuariosController.cs` file (in Controllers directory), add your MySQL password to the connection string
- Run the API using `dotnet run`
- Verify it's working by accessing an endpoint at https://localhost:7222/swagger

### 4. Web Application
- Download the zip files from the google drive link and decompress them. Store both the `images` and `Build` folder in `WebAppTecTreasure/wwwroot`
https://drive.google.com/drive/folders/1XM-jXpiABkaTo2rk64XCLJYkoMudgnCy?usp=sharing
- In the `WebAppTecTreasure` directory, run `dotnet run`
- Access the app at https://localhost:7123
- You can create your own account with the `Registrarse` option
- Or you can log in as an user with these credentials:
    - A00834438@tec.mx
    - sergio05_
- Or you can log in as an admin with these:
    - A00834438
    - equipoIOT
