using Dapper;
using Microsoft.Data.SqlClient; 

namespace WebAPI.Repositories
{
    public class Environment2DRepository
    {
        private readonly string _sqlConnectionString;

        public Environment2DRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        // Haalt alle omgevingen op, inclusief Username
        public async Task<IEnumerable<Environment2D>> GetAllEnvironmentsAsync()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<Environment2D>(
                    "SELECT EnvironmentId, Name, MaxLength, MaxHeight, Username FROM Environments");
            }
        }

        // Haalt één omgeving op via EnvironmentId
        public async Task<Environment2D> GetEnvironmentByIdAsync(Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryFirstOrDefaultAsync<Environment2D>(
                    "SELECT EnvironmentId, Name, MaxLength, MaxHeight, Username FROM Environments WHERE EnvironmentId = @Id",
                    new { Id = environmentId });
            }
        }

        // Voegt een nieuwe omgeving toe
        public async Task AddEnvironmentAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();

                // Genereer een nieuwe GUID als er geen is meegegeven
                if (environment.EnvironmentId == Guid.Empty)
                {
                    environment.EnvironmentId = Guid.NewGuid();
                }

                var sql = @"INSERT INTO Environments (EnvironmentId, Name, MaxLength, MaxHeight, Username)
                            VALUES (@EnvironmentId, @Name, @MaxLength, @MaxHeight, @Username)";
                await sqlConnection.ExecuteAsync(sql, environment);
            }
        }

        // Wijzigt een bestaande omgeving
        public async Task UpdateEnvironmentAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var sql = @"UPDATE Environments 
                            SET Name = @Name, MaxLength = @MaxLength, MaxHeight = @MaxHeight, Username = @Username
                            WHERE EnvironmentId = @EnvironmentId";
                await sqlConnection.ExecuteAsync(sql, environment);
            }
        }

        // Verwijdert een omgeving
        public async Task DeleteEnvironmentAsync(Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var sql = "DELETE FROM Environments WHERE EnvironmentId = @Id";
                await sqlConnection.ExecuteAsync(sql, new { Id = environmentId });
            }
        }
    }
}
