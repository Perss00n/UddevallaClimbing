using Microsoft.Data.SqlClient;
using System.Data;


namespace UaclimbingAPI.Produkter;

/// <summary>
/// Hanterar operationer relaterade till produkter i databasen.
/// Inkluderar metoder för att hämta, lägga till, uppdatera och radera produkter.
/// </summary>
public class ProduktHantering
{
    string _connectionString;

    /// <summary>
    /// Skapar en instans av ProduktHantering med den angivna anslutningssträngen.
    /// </summary>
    /// <param name="connectionString">Anslutningssträng för databasen.</param>
    public ProduktHantering(string connectionString)
    {
        _connectionString = connectionString;
    }


    /// <summary>
    /// Hämtar en lista med alla produkter från databasen.
    /// </summary>
    /// <returns>En lista av produkter som representerar alla produkter i databasen.</returns>
    public List<Produkt> HämtaAllaProdukter()
    {
        List<Produkt> produkter = new List<Produkt>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandText = "SELECT * FROM Produkter";

        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            produkter.Add(new Produkt
            {
                ProduktId = reader.GetInt32(0),
                ProduktNamn = reader.GetString(1),
                Pris = reader.GetDecimal(2),
            });
        }

        return produkter;
    }


    /// <summary>
    /// Hämtar en specifik produkt från databasen baserat på det angivna produktID:t.
    /// </summary>
    /// <param name="id">ID för produkten som ska hämtas.</param>
    /// <returns>En produkt om den hittas annars null.</returns>
    public Produkt? HämtaProdukt(int id)
    {
        Produkt? produkt = null;

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT * FROM Produkter WHERE ProduktId = @ProduktId";
        command.Parameters.Add("@ProduktId", SqlDbType.Int).Value = id;

        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            produkt = new Produkt
            {
                ProduktId = reader.GetInt32(0),
                ProduktNamn = reader.GetString(1),
                Pris = reader.GetDecimal(2),
            };
        }

        return produkt;
    }



    /// <summary>
    /// Lägger till en ny produkt i databasen.
    /// </summary>
    /// <param name="produkt">Produktobjektet som ska läggas till i databasen.</param>
    public void NyProdukt(Produkt produkt)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Nyprodukt";

        command.Parameters.Add("@ProduktNamn", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(produkt.ProduktNamn) ? DBNull.Value : produkt.ProduktNamn;
        command.Parameters.Add("@Pris", SqlDbType.Decimal).Value = produkt.Pris.HasValue ? produkt.Pris.Value : DBNull.Value;
        command.Parameters.Add("@ProduktId", SqlDbType.Int).Direction = ParameterDirection.Output;

        connection.Open();
        command.ExecuteNonQuery();

        produkt.ProduktId = Convert.ToInt32(command.Parameters["@ProduktId"].Value);
    }



    /// <summary>
    /// Tar bort en produkt från databasen baserat på det angivna produktIDt.
    /// </summary>
    /// <param name="id">IDt för produkten som ska raderas.</param>
    public void RaderaProdukt(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Raderaprodukt";

        command.Parameters.Add("@ProduktId", SqlDbType.Int).Value = id;

        connection.Open();
        command.ExecuteNonQuery();
    }


    /// <summary>
    /// Tar bort en produkt från databasen baserat på det angivna produktIDt och
    /// raderar även relaterad data som ordrar kopplade till produkten.
    /// </summary>
    /// <param name="id">Idt för produkten som ska raderas.</param>
    public void RaderaProduktCascade(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Raderaproduktcascade";

        command.Parameters.Add("@ProduktId", SqlDbType.Int).Value = id;

        connection.Open();
        command.ExecuteNonQuery();
    }


    /// <summary>
    /// Uppdaterar en produkt i databasen baserat på den angivna produktens information.
    /// </summary>
    /// <param name="produkt">Den produkt som ska uppdateras.</param>
    public void UppdateraProdukt(Produkt produkt)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand command = connection.CreateCommand();

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "Uppdateraprodukt";

        command.Parameters.Add("@ProduktId", SqlDbType.Int).Value = produkt.ProduktId;
        command.Parameters.Add("@ProduktNamn", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(produkt.ProduktNamn) ? DBNull.Value : produkt.ProduktNamn;
        command.Parameters.Add("@Pris", SqlDbType.Decimal).Value = produkt.Pris.HasValue ? produkt.Pris.Value : DBNull.Value;

        connection.Open();
        command.ExecuteNonQuery();
    }


}