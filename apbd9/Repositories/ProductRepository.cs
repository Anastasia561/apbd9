using Microsoft.Data.SqlClient;

namespace apbd9.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> CheckIfProductExistsAsync(int id, CancellationToken cancellationToken)
    {
        var con = new SqlConnection(_connectionString);
        var com = new SqlCommand();
        com.Connection = con;
        com.CommandText = "select count(*) from Product where IdProduct = @id";
        com.Parameters.AddWithValue("@id", id);

        await con.OpenAsync(cancellationToken);
        var result = (int)await com.ExecuteScalarAsync(cancellationToken);
        await con.DisposeAsync();
        return result > 0;
    }

    public async Task<float> GetProductPriceByIdAsync(int id, CancellationToken cancellationToken)
    {
        var con = new SqlConnection(_connectionString);
        var com = new SqlCommand();
        com.Connection = con;
        com.CommandText = "select Price from Product where IdProduct = @id";
        com.Parameters.AddWithValue("@id", id);

        await con.OpenAsync(cancellationToken);
        var result = Convert.ToSingle(await com.ExecuteScalarAsync(cancellationToken));
        await con.DisposeAsync();
        return result;
    }
}