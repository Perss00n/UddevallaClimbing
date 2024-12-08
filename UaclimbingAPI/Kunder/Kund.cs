namespace UaclimbingAPI.Kunder
{
    /// <summary>
    /// Representerar en kund i systemet.
    /// </summary>
    public class Kund
    {
        /// <summary>
        /// Hämtar eller sätter kundens unika ID.
        /// </summary>
        public int KundId { get; set; }

        /// <summary>
        /// Hämtar eller sätter kundens förnamn.
        /// Om inget förnamn anges sätts det till en tom sträng.
        /// </summary>
        public string FörNamn { get; set; } = string.Empty;

        /// <summary>
        /// Hämtar eller sätter kundens efternamn.
        /// Om inget efternamn anges sätts det till en tom sträng.
        /// </summary>
        public string EfterNamn { get; set; } = string.Empty;

        /// <summary>
        /// Hämtar eller sätter kundens e-postadress.
        /// Om ingen e-postadress anges sätts det till en tom sträng.
        /// </summary>
        public string Epost { get; set; } = string.Empty;
    }
}
