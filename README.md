```markdown
# DB Workload Distribution

This project implements a system for horizontally scaling workers and redistributing workload based on database changes using .NET Core and PostgreSQL with Docker Compose.

## Folder Structure

- `src/Worker/` - Contains the .NET Core Worker application.
- `docker-compose.yml` - Docker Compose configuration for deploying the worker service and PostgreSQL.

## Setup and Running

### Prerequisites

- Docker and Docker Compose installed on your machine.

### Build and Run the Application

1. **Build and Start Services**:
   ```bash
   docker-compose up --build
   ```

2. **Verify Services**:
   - **PostgreSQL**: Accessible on `localhost:5432`.
   - **Worker**: Accessible on `localhost:5000`.

### Database Initialization and Migrations

1. **Initialize the Database Schema**:
   - Open a terminal and navigate to the `src/Worker` directory.
   - Run the following commands to create and apply the initial migration:

     ```bash
     # Add a migration to create the initial schema
     dotnet ef migrations add InitialCreate

     # Update the database to apply the migration
     dotnet ef database update
     ```

   - **Note**: Ensure that your `DATABASE_URL` in `docker-compose.yml` matches the PostgreSQL configuration.

2. **Verify Database Schema**:
   - You can connect to PostgreSQL using a tool like pgAdmin or any PostgreSQL client to check the `mydatabase` schema.

### Configuration

- **Worker Service**:
  - The worker service uses the `WORKER_NAME` environment variable to identify itself.
  - The `DATABASE_URL` environment variable is used to connect to the PostgreSQL database.

- **Docker Compose**:
  - The `docker-compose.yml` file includes configurations for PostgreSQL and the worker service. Adjust environment variables as needed.

### Scaling Workers

1. **Update the `docker-compose.yml`**:
   - Modify the `replicas` value in the `worker` service section to change the number of worker instances.

   ```yaml
   deploy:
     replicas: 3 # Update this value to scale up or down
   ```

2. **Apply Changes**:
   ```bash
   docker-compose up --build
   ```

### Monitoring and Logs

- **View Logs**:
  - To view the logs of the worker service, use the following command:

    ```bash
    docker-compose logs -f worker
    ```

- **Check for Redistributed Workload**:
  - Worker logs will show how items are being processed and redistributed when worker nodes are added or removed.

## Notes

- **Consistent Hashing**:
  - The system uses a simplified consistent hashing algorithm to distribute items among workers. For production use, consider using a more robust solution.

- **Database Configuration**:
  - Ensure that PostgreSQL is properly configured and accessible to the worker service. Adjust `DATABASE_URL` in `docker-compose.yml` as needed.

- **Development and Testing**:
  - To run the application outside of Docker, ensure you have PostgreSQL installed and configured locally. Update the connection string in `AppDbContext` accordingly.

## Troubleshooting

- **Database Connection Issues**:
  - Ensure PostgreSQL is running and accessible.
  - Verify `DATABASE_URL` in the `docker-compose.yml` matches PostgreSQL credentials.

- **Migration Issues**:
  - Check the console output for errors during `dotnet ef` commands. Ensure that your database schema is up-to-date.

Feel free to reach out if you encounter any issues or need further assistance!
```