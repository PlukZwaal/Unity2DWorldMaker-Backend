using Dapper;
using Microsoft.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class UserRepository
    {
        private readonly string _sqlConnectionString;

        public UserRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        // Haalt een gebruiker op via de gebruikersnaam
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryFirstOrDefaultAsync<User>(
                    "SELECT Username, Password FROM Users WHERE Username = @Username",
                    new { Username = username });
            }
        }

        // Voegt een nieuwe gebruiker toe
        public async Task AddUserAsync(User user)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();

                var sql = @"INSERT INTO Users (Username, Password)
                            VALUES (@Username, @Password)";
                await sqlConnection.ExecuteAsync(sql, user);
            }
        }

        // Controleert of een gebruiker al bestaat op basis van de gebruikersnaam
        public async Task<bool> UserExistsAsync(string username)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var result = await sqlConnection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM Users WHERE Username = @Username",
                    new { Username = username });
                return result > 0;
            }
        }
    }
}
