using Microsoft.Data.SqlClient;
using System.Data;

namespace UaclimbingAPI.Ordrar;
/// <summary>
/// Hanterar operationer relaterade till ordrar i systemet inklusive att skapa, uppdatera, hämta och ta bort ordrar.
/// </summary>
public class OrderHantering
{
    string _connectionString;

    /// <summary>
    /// Skapar en instans av OrderHantering med den angivna anslutningssträngen.
    /// </summary>
    /// <param name="connectionString">Anslutningssträng för databasen.</param>
    public OrderHantering(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Hämtar en lista med alla ordrar från databasen
    /// </summary>
    /// <returns>En lista av Order objekt där varje objekt representerar en order i databasen.</returns>
    public List<Order> HämtaAllaOrdrar()
    {
        List<Order> ordrar = new List<Order>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandText = "SELECT * FROM Ordrar";

        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            ordrar.Add(new Order
            {
                OrderId = reader.GetInt32(0),
                KundId = reader.GetInt32(1),
                ProduktId = reader.GetInt32(2),
                Antal = reader.GetInt32(3),
                OrderDatum = reader.GetDateTime(4)
            });
        }

        return ordrar;
    }

    /// <summary>
    /// Hämtar en specifik order från databasen baserat på det angivna orderID:t.
    /// </summary>
    /// <param name="id">OrderID som används för att identifiera ordern.</param>
    /// <returns>Ett Order objekt om det hittas annars null.</returns>
    public Order? HämtaOrder(int id)
    {
        Order? order = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT * FROM Ordrar WHERE OrderId = @OrderId";
        command.Parameters.Add("@OrderId", SqlDbType.Int).Value = id;

        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            order = new Order
            {
                OrderId = reader.GetInt32(0),
                KundId = reader.GetInt32(1),
                ProduktId = reader.GetInt32(2),
                Antal = reader.GetInt32(3),
                OrderDatum = reader.GetDateTime(4)
            };
        }

        return order;
    }

    /// <summary>
    /// Skapar en ny order i databasen med hjälp av en lagrad procedur.
    /// </summary>
    /// <param name="order">Order objekt som innehåller information om den nya ordern.</param>
    public void NyOrder(Order order)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Nyorder";

        command.Parameters.Add("@KundId", SqlDbType.Int).Value = order.KundId.HasValue ? order.KundId.Value : DBNull.Value;
        command.Parameters.Add("@ProduktId", SqlDbType.Int).Value = order.ProduktId.HasValue ? order.ProduktId.Value : DBNull.Value;
        command.Parameters.Add("@Antal", SqlDbType.Int).Value = order.Antal.HasValue ? order.Antal.Value : DBNull.Value;
        command.Parameters.Add("@OrderDatum", SqlDbType.DateTime).Value = order.OrderDatum.HasValue ? order.OrderDatum.Value : DBNull.Value;

        command.Parameters.Add("@OrderId", SqlDbType.Int).Direction = ParameterDirection.Output;

        connection.Open();
        command.ExecuteNonQuery();

        order.OrderId = Convert.ToInt32(command.Parameters["@OrderId"].Value);

    }

    /// <summary>
    /// Uppdaterar en befintlig order i databasen med hjälp av en lagrad procedur.
    /// </summary>
    /// <param name="order">Order objekt som innehåller de nya värdena för ordern som ska uppdateras.</param>
    public void UppdateraOrder(Order order)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "UppdateraOrder";

        command.Parameters.Add("@OrderId", SqlDbType.Int).Value = order.OrderId;
        command.Parameters.Add("@KundId", SqlDbType.Int).Value = order.KundId.HasValue ? order.KundId.Value : DBNull.Value;
        command.Parameters.Add("@ProduktId", SqlDbType.Int).Value = order.ProduktId.HasValue ? order.ProduktId.Value : DBNull.Value;
        command.Parameters.Add("@Antal", SqlDbType.Int).Value = order.Antal.HasValue ? order.Antal.Value : DBNull.Value;
        command.Parameters.Add("@OrderDatum", SqlDbType.DateTime).Value = order.OrderDatum.HasValue ? order.OrderDatum.Value : DBNull.Value;

        connection.Open();
        command.ExecuteNonQuery();
    }


    /// <summary>
    /// Tar bort en order från databasen baserat på det angivna orderID:t.
    /// </summary>
    /// <param name="id">ID för den order som ska raderas.</param>
    public void RaderaOrder(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Raderaorder";

        command.Parameters.Add("@OrderId", SqlDbType.Int).Value = id;

        connection.Open();
        command.ExecuteNonQuery();
    }
}
