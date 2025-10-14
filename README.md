# Webcore Messenger

Webcore Messenger is a working name for a prototype communication app that allows users to exchange messages and files bidirectionally, join servers, and chat in real time.
The project focuses on two main goals that will distinguish it from other messengers:

### Highly customizable frontend

Users should have the ability to modify every visible element — similar to how skins work in Winamp — as well as customize the layout of their profiles and servers. They will also be able to create and share their own themes publicly.

### Reducing maintenance and resource overhead

Over time, as the user base grows, disk space and bandwidth usage can become a burden. The app aims to encourage peer-to-peer file transfers and temporary uploads for large files that are automatically removed after a set period.
Inactive servers will be deleted after prior notification to their owners, who will have the option to archive all their files locally.

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