using apbd9.Model;
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

    public async Task<int> FulfillOrderAsync(CompletedOrder completedOrder, CancellationToken cancellationToken)
    {
        var con = new SqlConnection();
        var com = new SqlCommand();
        com.Connection = con;
        await con.OpenAsync(cancellationToken);

        var transaction = await con.BeginTransactionAsync(cancellationToken);
        com.Transaction = transaction as SqlTransaction;

        try
        {
            com.CommandText = "update Order set FulfilledAt=@currDateTime where IdOrder=@id";
            var currDateTime = DateTime.Now;
            com.Parameters.AddWithValue("@id", completedOrder.IdOrder);
            com.Parameters.AddWithValue("@currDateTime", currDateTime);
            await com.ExecuteNonQueryAsync(cancellationToken);

            com.Parameters.Clear();

            com.CommandText =
                "insert into Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)" +
                "values(@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt); select cast(scope_identity() as int);";
            com.Connection = con;
            com.Parameters.AddWithValue("@idWarehouse", completedOrder.IdWarehouse);
            com.Parameters.AddWithValue("@idProduct", completedOrder.IdProduct);
            com.Parameters.AddWithValue("@idOrder", completedOrder.IdOrder);
            com.Parameters.AddWithValue("@amount", completedOrder.Amount);
            com.Parameters.AddWithValue("@price", completedOrder.Price);
            com.Parameters.AddWithValue("@createdAt", completedOrder.CreatedAt);

            await con.OpenAsync(cancellationToken);
            var result = (int)await com.ExecuteScalarAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new ApplicationException(ex.Message);
        }
    }
}