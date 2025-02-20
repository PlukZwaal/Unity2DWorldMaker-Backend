using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace WebAPI.Repositories
{
    public class EnvironmentRepository
    {
        private readonly string _sqlConnectionString;

        public EnvironmentRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        // Create new environment
        public async Task<bool> CreateEnvironmentAsync(Environment environment)
        {
            using var sqlConnection = new SqlConnection(_sqlConnectionString);
            await sqlConnection.OpenAsync();

            environment.id = environment.id == Guid.Empty ? Guid.NewGuid() : environment.id;

            var sql = @"INSERT INTO Environments (id, name, maxLength, maxHeight, username)
                VALUES (@id, @name, @maxLength, @maxHeight, @username)"; 

            var result = await sqlConnection.ExecuteAsync(sql, environment);
            return result > 0;
        }
    }
}
