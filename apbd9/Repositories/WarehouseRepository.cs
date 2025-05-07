using Microsoft.Data.SqlClient;

namespace apbd9.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly string _connectionString;

    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> CheckIfWarehouseExistsAsync(int id, CancellationToken cancellationToken)
    {
        var con = new SqlConnection(_connectionString);
        var com = new SqlCommand();
        com.Connection = con;
        com.CommandText = "select count(*) FROM Warehouse where IdWarehouse = @id";
        com.Parameters.AddWithValue("@id", id);

        await con.OpenAsync(cancellationToken);

        var result = (int)await com.ExecuteScalarAsync(cancellationToken);
        await con.DisposeAsync();
        return result > 0;
    }
}