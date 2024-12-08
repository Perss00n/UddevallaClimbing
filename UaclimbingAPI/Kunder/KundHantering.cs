using Microsoft.Data.SqlClient;
using System.Data;

namespace UaclimbingAPI.Kunder
{
    /// <summary>
    /// Denna klass hanterar kunder i systemet, inklusive att lägga till, uppdatera, ta bort och hämta kundinformation.
    /// </summary>
    public class KundHantering
    {
        string _connectionString;

        /// <summary>
        /// Skapar en instans av KundHantering med den angivna anslutningssträngen.
        /// </summary>
        /// <param name="connectionString">Anslutningssträng för databasen.</param>
        public KundHantering(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Lägger till en ny kund i databasen genom att anropa den lagrade proceduren "Nykund".
        /// </summary>
        /// <param name="kund">Ett kund objekt som innehåller kundens information som ska sparas i databasen. </param>
        public void NyKund(Kund kund)
        {
            // Skapa en anslutning till databasen och hämta anslutningssträngen
            using SqlConnection connection = new SqlConnection(_connectionString);

            // Skapa ett command objekt kopplat till anslutningen som gör att det går att köra sql queries
            using SqlCommand command = connection.CreateCommand();

            // Ange att det är en lagrad procedur som ska köras och ange vad denna procedur heter
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "Nykund";

            // Sätt parametrarna och med hjälp utav en ternary operator så sätter vi värdet till null om värdet skulle vara tomt
            command.Parameters.Add("@FörNamn", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(kund.FörNamn) ? DBNull.Value : kund.FörNamn;
            command.Parameters.Add("@EfterNamn", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(kund.EfterNamn) ? DBNull.Value : kund.EfterNamn;
            command.Parameters.Add("@Epost", SqlDbType.NVarChar, 80).Value = string.IsNullOrEmpty(kund.Epost) ? DBNull.Value : kund.Epost;

            // Output parameter för att få tillbaka det genererade idt 
            command.Parameters.Add("@KundId", SqlDbType.Int).Direction = ParameterDirection.Output;

            // Öppna anslutningen till databasen och kör den lagrade proceduren
            connection.Open();
            command.ExecuteNonQuery();

            // Hämta det genererade idt från output parametern
            kund.KundId = Convert.ToInt32(command.Parameters["@KundId"].Value);
        }

        /// <summary>
        /// Uppdaterar en befintlig kund i databasen genom att anropa den lagrade proceduren "UppdateraKund".
        /// </summary>
        /// <param name="kund">Ett kund objekt som innehåller de nya värdena för kunden.</param>
        public void UppdateraKund(Kund kund)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UppdateraKund";

            command.Parameters.Add("@KundId", SqlDbType.Int).Value = kund.KundId;
            command.Parameters.Add("@FörNamn", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(kund.FörNamn) ? DBNull.Value : kund.FörNamn;
            command.Parameters.Add("@EfterNamn", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(kund.EfterNamn) ? DBNull.Value : kund.EfterNamn;
            command.Parameters.Add("@Epost", SqlDbType.NVarChar, 80).Value = string.IsNullOrEmpty(kund.Epost) ? DBNull.Value : kund.Epost;

            connection.Open();
            command.ExecuteNonQuery();

        }


        /// <summary>
        /// Raderar en kund från databasen baserat på det angivna kundID:t genom att anropa den lagrade proceduren "Raderakund".
        /// </summary>
        /// <param name="id">Kundens ID som används för att hitta och ta bort kunden från databasen.</param>
        public void RaderaKund(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "Raderakund";

            command.Parameters.Add("@KundId", SqlDbType.Int).Value = id;

            connection.Open();
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// Raderar en kund från databasen samt alla relaterade ordrar för den kunden genom att anropa den lagrade proceduren "Raderakundcascade".
        /// </summary>
        /// <param name="id">Kundens ID som används för att hitta och ta bort kunden samt alla dess ordrar från databasen.</param>
        public void RaderaKundCascade(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "Raderakundcascade";

            command.Parameters.Add("@KundId", SqlDbType.Int).Value = id;

            connection.Open();
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// Hämtar alla kunder från databasen.
        /// </summary>
        /// <returns>En lista av alla kunder som finns i databasen.</returns>
        public List<Kund> HämtaAllaKunder()
        {
            List<Kund> kunder = new List<Kund>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM Kunder";

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                kunder.Add(new Kund
                {
                    KundId = reader.GetInt32(0),
                    FörNamn = reader.GetString(1),
                    EfterNamn = reader.GetString(2),
                    Epost = reader.GetString(3)
                });
            }

            return kunder;
        }



        /// <summary>
        /// Hämtar en kund från databasen baserat på det angivna kundID:t.
        /// </summary>
        /// <param name="id">Det unika IDt för den kund som ska hämtas.</param>
        /// <returns>Ett kund objekt om en kund med det angivna ID:t finns i databasen annars null.</returns>
        public Kund? HämtaKund(int id)
        {
            Kund? kund = null;

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();

            command.CommandText = @"SELECT * FROM Kunder WHERE KundID = @KundId";
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@KundId", SqlDbType.Int).Value = id;

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                kund = new Kund
                {
                    KundId = reader.GetInt32(0),
                    FörNamn = reader.GetString(1),
                    EfterNamn = reader.GetString(2),
                    Epost = reader.GetString(3)
                };
            }

            return kund;
        }



    }
}