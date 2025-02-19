using Dapper;
using Microsoft.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class Object2DRepository
    {
        private readonly string _sqlConnectionString;

        public Object2DRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        // Haal alle objecten op
        public async Task<IEnumerable<Object2D>> GetAllObjectsAsync()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var objects = await sqlConnection.QueryAsync<Object2D>(
                    "SELECT ObjectId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentId FROM Objects");

                return objects;
            }
        }

        // Haal object op op basis van ObjectId
        public async Task<Object2D> GetObjectByIdAsync(Guid objectId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var objectData = await sqlConnection.QuerySingleOrDefaultAsync<Object2D>(
                    "SELECT ObjectId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentId FROM Objects WHERE ObjectId = @ObjectId",
                    new { ObjectId = objectId });

                return objectData;
            }
        }

        // Voeg een object toe
        public async Task CreateObjectAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var query = "INSERT INTO Objects (ObjectId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentId) " +
                            "VALUES (@ObjectId, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @EnvironmentId)";

                await sqlConnection.ExecuteAsync(query, object2D);
            }
        }

        // Update een object
        public async Task UpdateObjectAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var query = "UPDATE Objects SET PrefabId = @PrefabId, PositionX = @PositionX, PositionY = @PositionY, " +
                            "ScaleX = @ScaleX, ScaleY = @ScaleY, RotationZ = @RotationZ, SortingLayer = @SortingLayer, " +
                            "EnvironmentId = @EnvironmentId WHERE ObjectId = @ObjectId";

                await sqlConnection.ExecuteAsync(query, object2D);
            }
        }

        // Verwijder een object
        public async Task DeleteObjectAsync(Guid objectId)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();

                // Loggen voor debugdoeleinden
                Console.WriteLine($"Verwijderen van object met ObjectId: {objectId}");

                var query = "DELETE FROM Objects WHERE ObjectId = @ObjectId";

                // Aantal rijen die zijn verwijderd controleren
                var rowsAffected = await sqlConnection.ExecuteAsync(query, new { ObjectId = objectId });

                if (rowsAffected == 0)
                {
                    Console.WriteLine("Geen object gevonden om te verwijderen.");
                }
            }
        }
    }
}
