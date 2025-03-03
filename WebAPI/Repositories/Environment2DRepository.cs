using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces;

public class Environment2DRepository : IEnvironment2DRepository
{
    private readonly string _connectionString;

    public Environment2DRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> CreateEnvironment2DAsync(Environment2D environment, string userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        environment.id = Guid.NewGuid().ToString();

        var sql = @"
        INSERT INTO environment2ds (id, name, maxLength, maxHeight, userId)
        VALUES (@id, @name, @maxLength, @maxHeight, @userId)";

        var result = await connection.ExecuteAsync(sql, new
        {
            environment.id,
            environment.name,
            environment.maxLength,
            environment.maxHeight,
            userId
        });

        return result > 0;
    }

    public async Task<IEnumerable<Environment2D>> GetEnvironment2DsByUserIdAsync(string userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = "SELECT * FROM environment2ds WHERE userId = @userId";
        return await connection.QueryAsync<Environment2D>(sql, new { userId });
    }

    public async Task<bool> DeleteEnvironment2DAsync(string id, string userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var sqlObjects = "DELETE FROM object2ds WHERE environmentId = @id";
        await connection.ExecuteAsync(sqlObjects, new { id });

        var sql = "DELETE FROM environment2ds WHERE id = @id AND userId = @userId";
        var result = await connection.ExecuteAsync(sql, new { id, userId });

        return result > 0;
    }
}