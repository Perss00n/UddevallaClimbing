namespace UaclimbingAPI.Ordrar
{
    /// <summary>
    /// Representerar en order i systemet.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Hämtar eller sätter orderns unika ID.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Hämtar eller sätter kundens unika ID som är kopplad till ordern.
        /// Om kunden inte är specificerad kan värdet vara null.
        /// </summary>
        public int? KundId { get; set; }

        /// <summary>
        /// Hämtar eller sätter produktens unika ID som är kopplad till ordern.
        /// Om produkten inte är specificerad kan värdet vara null.
        /// </summary>
        public int? ProduktId { get; set; }

        /// <summary>
        /// Hämtar eller sätter antalet av produkten som beställts i ordern.
        /// Om antalet inte är specificerat kan värdet vara null.
        /// </summary>
        public int? Antal { get; set; }

        /// <summary>
        /// Hämtar eller sätter datumet och tiden när ordern lades.
        /// Om inget datum anges sätts det till det aktuella datumet och tiden.
        /// </summary>
        public DateTime? OrderDatum { get; set; } = DateTime.Now;
    }
}