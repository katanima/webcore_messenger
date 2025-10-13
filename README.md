# Webcore Backend

This is the backend service for Webcore, a communicator app.

## Installation

1. Clone the repository:
    ```
    git clone https://github.com/katanima/webcore_backend.git
    ```  
2. Change to the project repository:
    ```
    cd webcore_backend
    ```
3. Configure environment variables:
    - Rename `.env.example` to `.env`.
    - Rename `appsettings.json.example` to `appsettings.json`.
    - Change values as needed.

4. Run the application:
    ```
    dotnet run
    ```
    or with docker:
    ```
    docker compose up --build
    ```

## API Documentation

Connecting to the same URL as the backend will provide access to Swagger UI for API documentation and testing.
    - Type: `http://localhost:5000`