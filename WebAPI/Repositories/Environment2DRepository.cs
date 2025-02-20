using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

public class Environment2DRepository
{
    private readonly string _connectionString;

    public Environment2DRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Voeg een nieuwe Environment2D toe
    public async Task<bool> CreateEnvironment2DAsync(Environment2D environment)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        environment.id = Guid.NewGuid().ToString(); // Genereer een unieke ID

        var sql = @"
            INSERT INTO environment2ds (id, name, maxLength, maxHeight)
            VALUES (@id, @name, @maxLength, @maxHeight)";

        var result = await connection.ExecuteAsync(sql, environment);
        return result > 0;
    }
}