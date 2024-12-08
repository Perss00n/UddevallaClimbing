namespace UaclimbingAPI.Produkter
{
    /// <summary>
    /// Representerar en produkt i systemet.
    /// </summary>
    public class Produkt
    {
        /// <summary>
        /// Hämtar eller sätter produktens unika ID.
        /// </summary>
        public int ProduktId { get; set; }

        /// <summary>
        /// Hämtar eller sätter namnet på produkten.
        /// Om inget namn anges sätts det till en tom sträng.
        /// </summary>
        public string ProduktNamn { get; set; } = string.Empty;

        /// <summary>
        /// Hämtar eller sätter priset på produkten.
        /// Om inget pris anges kan det vara null.
        /// </summary>
        public decimal? Pris { get; set; }
    }
}