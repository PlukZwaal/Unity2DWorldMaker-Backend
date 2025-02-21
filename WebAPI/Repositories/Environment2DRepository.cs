using Microsoft.Data.SqlClient;
using Dapper; 

public class Environment2DRepository
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
}
