# Webcore Messenger

This is the backend service for Webcore, a communicator app.

## Installation

Clone the repository:
    ```
    git clone https://github.com/katanima/webcore_messenger.git
    ```  
## Run backend
  
1. Change to the project repository:
    ```
    cd {app_path}/backend/webcore_backend
    ```
	
2. Configure environment variables:
    - Rename `.env.example` to `.env`.
    - Rename `appsettings.json.example` to `appsettings.json`.
    - Change values as needed.

3. Run the application:
	locally (requires running PostgreSQL in the background):
    ```
    dotnet run
    ```
    or in the docker (requires running Docker engine in the background):
    ```
    docker compose up --build
    ```

## API Documentation

Connecting to the same URL as the backend will provide access to Swagger UI for API documentation and testing.
    - Connect to: `http://localhost:5000`