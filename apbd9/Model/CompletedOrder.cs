namespace apbd9.Model;

public class CompletedOrder
{
    public int IdWarehouse { get; set; }
    public int IdProduct { get; set; }
    public int IdOrder { get; set; }
    public int Amount { get; set; }
    public float Price { get; set; }
}