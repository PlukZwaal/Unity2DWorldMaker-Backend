﻿using Dapper;
using Microsoft.Data.SqlClient;
using WebAPI.Interfaces;

public class Object2DRepository : IObject2DRepository
{
    private readonly string _connectionString;

    public Object2DRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> CreateObject2DAsync(Object2D object2D, string environmentId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        object2D.id = Guid.NewGuid().ToString();
        object2D.environmentId = environmentId;

        var sql = @"
        INSERT INTO object2ds (id, environmentId, prefabId, positionX, positionY, scaleX, scaleY, rotationZ, sortingLayer)
        VALUES (@id, @environmentId, @prefabId, @positionX, @positionY, @scaleX, @scaleY, @rotationZ, @sortingLayer)";

        var result = await connection.ExecuteAsync(sql, object2D);
        return result > 0;
    }

    public async Task<bool> CheckEnvironmentOwnership(string environmentId, string userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = "SELECT COUNT(*) FROM environment2ds WHERE id = @environmentId AND userId = @userId";
        int count = await connection.ExecuteScalarAsync<int>(sql, new { environmentId, userId });

        return count > 0;
    }

    public async Task<List<Object2D>> GetObjectsByEnvironmentIdAsync(string environmentId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = "SELECT * FROM Object2Ds WHERE environmentId = @environmentId";
        var objects = await connection.QueryAsync<Object2D>(sql, new { environmentId });

        return objects.AsList();
    }
}
