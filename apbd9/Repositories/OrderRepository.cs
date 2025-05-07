using Microsoft.Data.SqlClient;

namespace apbd9.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> CheckIfOrderExistsAsync(int productId, int amount, CancellationToken cancellationToken)
    {
        var con = new SqlConnection();
        var com = new SqlCommand();
        com.CommandText = "select CreatedAt from Order where IdProduct = @productId and Amount = @amount";
        com.Connection = con;
        com.Parameters.AddWithValue("@productId", productId);
        com.Parameters.AddWithValue("@amount", amount);

        await con.OpenAsync(cancellationToken);
        var result = (DateTime)await com.ExecuteScalarAsync(cancellationToken);
        await con.DisposeAsync();

        if (!result.Equals(null))
        {
            return result < DateTime.Now;
        }

        return false;
    }

    public async Task<int> GetOrderIdByProductAndAmountAsync(int productId, int amount,
        CancellationToken cancellationToken)
    {
        var con = new SqlConnection();
        var com = new SqlCommand();
        com.CommandText = "select IdOrder from Order where IdProduct = @productId and Amount = @amount";
        com.Connection = con;
        com.Parameters.AddWithValue("@productId", productId);
        com.Parameters.AddWithValue("@amount", amount);

        await con.OpenAsync(cancellationToken);
        var result = (int)await com.ExecuteScalarAsync(cancellationToken);
        await con.DisposeAsync();
        return result;
    }

    public async Task<bool> CheckIfOrderCompletedAsync(int id, CancellationToken cancellationToken)
    {
        var con = new SqlConnection();
        var com = new SqlCommand();
        com.CommandText = "select count(*) from Product_Warehouse where IdOrder=@id";
        com.Connection = con;
        com.Parameters.AddWithValue("@id", id);

        await con.OpenAsync(cancellationToken);
        var result = (int)await com.ExecuteScalarAsync(cancellationToken);
        await con.DisposeAsync();
        return result > 0;
    }
}