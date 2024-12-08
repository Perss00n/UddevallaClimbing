using Microsoft.Data.SqlClient;
using UaclimbingAPI.Kunder;
using UaclimbingAPI.Ordrar;
using UaclimbingAPI.Produkter;

namespace UaclimbingAPP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbConnectionString = @"Ange connection stringen här";
            
            KundHantering kundHantering = new KundHantering(dbConnectionString);
            ProduktHantering produktHantering = new ProduktHantering(dbConnectionString);
            OrderHantering orderHantering = new OrderHantering(dbConnectionString);

            Meny(kundHantering, produktHantering, orderHantering);
        }

        /// <summary>
        /// Lägger till en ny kund i databasen genom att samla in användarinmatningar.
        /// </summary>
        /// <param name="kundHantering">Instans av KundHantering.</param>
        /// <remarks>
        /// Ber användaren att mata in förnamn, efternamn och e-postadress för en ny kund.
        /// Om någon av inmatningarna är tomma så tvingas användaren att ange ett giltigt värde.
        /// </remarks>
        static void SkapaNyKund(KundHantering kundHantering)
        {
            try
            {
                Console.WriteLine("\n##### Lägg till en ny användare i databasen #####");

                Kund nyKund = new Kund();

                do
                {
                    Console.Write("Ange Förnamn: ");
                    nyKund.FörNamn = Console.ReadLine()!;
                }
                while (String.IsNullOrEmpty(nyKund.FörNamn));

                do
                {
                    Console.Write("Ange Efternamn: ");
                    nyKund.EfterNamn = Console.ReadLine()!;
                }
                while (String.IsNullOrEmpty(nyKund.EfterNamn));

                do
                {
                    Console.Write("Ange Epost: ");
                    nyKund.Epost = Console.ReadLine()!;
                }
                while (String.IsNullOrEmpty(nyKund.Epost));

                kundHantering.NyKund(nyKund);

                Console.WriteLine($"Lyckat! Kund skapad med ID '{nyKund.KundId}'");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }



        /// <summary>
        /// Uppdaterar en kunds information i databasen baserat på användarinmatning.
        /// </summary>
        /// <param name="produktHantering">
        /// Instans av KundHantering som används för att hantera kunddata i databasen.
        /// </param>
        /// <remarks>
        /// Ber användaren att ange ID på den kund som ska uppdateras.
        /// Användaren får också ange nya värden för kundens förnamn, efternamn och e-postadress.
        /// Lämnas fälten tomma så behålls den befintliga informationen.
        /// </remarks>
        static void UppdateraKund(KundHantering produktHantering)
        {
            try
            {
                Console.WriteLine("\n##### Uppdatera kundinformation #####");

                List<Kund> kunder = produktHantering.HämtaAllaKunder();
                foreach (var kund in kunder)
                {
                    Console.WriteLine($"ID: {kund.KundId}, Namn: {kund.FörNamn} {kund.EfterNamn}, Epost: {kund.Epost}");
                }

                Kund uppdateradKund = new Kund();

                bool isInvalidIdInput;
                int kundID;

                do
                {
                    Console.Write("\nAnge Kund ID: ");
                    isInvalidIdInput = !Int32.TryParse(Console.ReadLine(), out kundID);

                    if (isInvalidIdInput)
                        Console.WriteLine("Ogiltigt Kund ID! Vänligen ange ett giltigt heltal");

                } while (isInvalidIdInput);

                uppdateradKund.KundId = kundID;

                Console.Write("Ange nytt Förnamn (lämna tomt för att behålla nuvarande): ");
                string? förNamn = Console.ReadLine();
                uppdateradKund.FörNamn = string.IsNullOrEmpty(förNamn) ? null! : förNamn;

                Console.Write("Ange nytt Efternamn (lämna tomt för att behålla nuvarande): ");
                string? efterNamn = Console.ReadLine();
                uppdateradKund.EfterNamn = string.IsNullOrEmpty(efterNamn) ? null! : efterNamn;

                Console.Write("Ange ny Epost (lämna tomt för att behålla nuvarande): ");
                string? epost = Console.ReadLine();
                uppdateradKund.Epost = string.IsNullOrEmpty(epost) ? null! : epost;

                produktHantering.UppdateraKund(uppdateradKund);

                Console.WriteLine("Lyckat! Kundinformationen har uppdaterats");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Visar en lista över alla kunder som finns i databasen.
        /// </summary>
        /// <param name="kundHantering">Instans av KundHantering.</param>
        static void VisaAllaKunder(KundHantering kundHantering)
        {
            try
            {
                Console.WriteLine("\n##### Befintliga kunder i databasen #####");

                List<Kund> kunder = kundHantering.HämtaAllaKunder();
                foreach (var kund in kunder)
                {
                    Console.WriteLine($"ID: {kund.KundId}, Namn: {kund.FörNamn} {kund.EfterNamn}, Epost: {kund.Epost}");
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Tar bort en kund från databasen, med valmöjlighet att även radera relaterade ordrar.
        /// </summary>
        /// <param name="kundHantering">Instans av KundHantering.</param>
        /// <remarks>
        /// Användaren kan välja mellan att endast ta bort kunden (förutsatt att kunden inte har några pågående ordrar) 
        /// eller att radera både kunden och alla relaterade ordrar ifrån databasen.
        /// </remarks>

        static void RaderaKund(KundHantering kundHantering)
        {
            try
            {
                Console.WriteLine("\n##### Befintliga kunder i databasen #####");

                List<Kund> kunder = kundHantering.HämtaAllaKunder();
                foreach (var kund in kunder)
                {
                    Console.WriteLine($"ID: {kund.KundId}, Namn: {kund.FörNamn} {kund.EfterNamn}, Epost: {kund.Epost}");
                }

                bool isInvalidIdInput;
                int kundID;

                do
                {
                    Console.Write("\nAnge ID på den kund du vill radera ifrån databasen: ");
                    isInvalidIdInput = !Int32.TryParse(Console.ReadLine(), out kundID);

                    if (isInvalidIdInput)
                        Console.WriteLine("Ogiltigt Kund ID! Vänligen ange ett giltigt heltal");

                } while (isInvalidIdInput);

                int input = 0;
                do
                {
                    Console.WriteLine("\nVälj om du endast vill ta bort kunden eller kund OCH tillhörande order/ordrar som är kopplade till kunden");
                    Console.WriteLine("Observera att om du väljer att ta bort endast kunden så kan kunden inte ha några pågående ordrar");
                    Console.WriteLine("1. Ta bort kund");
                    Console.WriteLine("2. Ta bort kund och tillhörande ordrar");
                    Console.Write("Ditt val: ");

                    bool isValidInput = Int32.TryParse(Console.ReadLine(), out input);

                    if (!isValidInput || (input != 1 && input != 2))
                        Console.WriteLine("Ogiltigt val! Vänligen välj 1 eller 2");

                } while (input != 1 && input != 2);

                if (input == 1)
                {
                    kundHantering.RaderaKund(kundID);
                    Console.WriteLine($"Lyckat! Kund med ID '{kundID}' togs bort ifrån databasen!");
                }
                else if (input == 2)
                {
                    kundHantering.RaderaKundCascade(kundID);
                    Console.WriteLine($"Lyckat! Kund med ID '{kundID}' och alla tillhörande ordrar togs bort ifrån databasen!");
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Hämtar information om en specifik kund från databasen baserat på det angivna kundID:t.
        /// </summary>
        /// <param name="kundHantering">Instans av KundHantering.</param>
        /// <remarks>
        /// Ber användaren att ange ett giltigt kundID och försöker sedan hämta information om kunden.
        /// Om ingen kund hittas med det angivna IDt får användaren försöka igen.</remarks>
        static void HämtaKund(KundHantering kundHantering)
        {
            try
            {
                while (true)
                {
                    Console.Write("\nAnge ID på den kund du vill visa: ");

                    if (!int.TryParse(Console.ReadLine(), out int kundID))
                    {
                        Console.WriteLine("Ogiltigt Kund ID! Vänligen ange ett giltigt heltal");
                        continue;
                    }

                    Kund? kund = kundHantering.HämtaKund(kundID);

                    if (kund == null)
                    {
                        Console.WriteLine("Ingen kund hittades med det angivna IDt! Försök igen...");
                        continue;
                    }

                    Console.WriteLine($"ID: {kund.KundId}, Namn: {kund.FörNamn} {kund.EfterNamn}, Epost: {kund.Epost}");
                    break;
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Visar en lista över alla produkter som finns i databasen.
        /// </summary>
        /// <param name="produktHantering">Instans av ProduktHantering.</param>
        /// <remarks>
        /// Hämtar alla produkter ifrån databasen och skriver ut deras ID, namn och pris i konsolen.</remarks>
        static void VisaAllaProdukter(ProduktHantering produktHantering)
        {
            try
            {
                Console.WriteLine("\n##### Befintliga produkter i databasen #####");

                List<Produkt> produkter = produktHantering.HämtaAllaProdukter();
                foreach (var produkt in produkter)
                {
                    Console.WriteLine($"ID: {produkt.ProduktId}, Namn: {produkt.ProduktNamn}, Pris: {produkt.Pris:C}");
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }



        /// <summary>
        /// Hämtar information om en specifik produkt ifrån databasen baserat på produktens ID.
        /// </summary>
        /// <param name="produktHantering">Instans av ProduktHantering</param>
        /// <remarks>
        /// Ber användaren att ange ett giltigt produktID. Om IDt är ogiltigt får användaren försöka igen. 
        /// Om ingen produkt hittas med det angivna IDt visas ett meddelande och användaren får en ny chans att ange ett ID.
        /// </remarks>

        static void HämtaProdukt(ProduktHantering produktHantering)
        {
            try
            {
                while (true)
                {
                    Console.Write("\nAnge ID på den produkt du vill visa: ");

                    if (!int.TryParse(Console.ReadLine(), out int produktID))
                    {
                        Console.WriteLine("Ogiltigt Produkt ID! Vänligen ange ett giltigt heltal");
                        continue;
                    }

                    Produkt? produkt = produktHantering.HämtaProdukt(produktID);

                    if (produkt == null)
                    {
                        Console.WriteLine("Ingen produkt hittades med det angivna IDt!. Försök igen...");
                        continue;
                    }

                    Console.WriteLine($"ID: {produkt.ProduktId}, Namn: {produkt.ProduktNamn}, Pris: {produkt.Pris:C}");
                    break;
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }



        /// <summary>
        /// Lägger till en ny produkt i databasen baserat på användarinmatning.
        /// </summary>
        /// <param name="produktHantering">Instans av ProduktHantering</param>
        /// <remarks>
        /// Ber användaren att ange produktens namn och pris och kollar att
        /// användaren fyller i giltiga värden för både produktens namn och pris.
        /// Om inmatningen är ogiltig upprepas frågorna tills giltig information anges.
        /// </remarks>
        static void LäggTillNyProdukt(ProduktHantering produktHantering)
        {
            try
            {
                Console.WriteLine("\n##### Lägg till en ny produkt i databasen #####");

                Produkt nyProdukt = new Produkt();

                do
                {
                    Console.Write("Ange namn på produkten: ");
                    nyProdukt.ProduktNamn = Console.ReadLine()!;
                }
                while (String.IsNullOrEmpty(nyProdukt.ProduktNamn));

                bool isInvalidPrisInput;
                decimal produktPris;

                do
                {
                    Console.Write("\nAnge pris på produkten: ");
                    isInvalidPrisInput = !Decimal.TryParse(Console.ReadLine(), out produktPris);

                    if (isInvalidPrisInput)
                        Console.WriteLine("Ogiltligt pris! Försök igen...");

                } while (isInvalidPrisInput);

                nyProdukt.Pris = produktPris;


                produktHantering.NyProdukt(nyProdukt);

                Console.WriteLine($"Produkten '{nyProdukt.ProduktNamn}' las till i databasen");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }




        /// <summary>
        /// Tar bort en produkt från databasen baserat på användarinmatning.
        /// </summary>
        /// <param name="produktHantering">Instans av ProduktHantering.</param>
        /// <remarks>
        /// Ber användaren att ange ID på den produkt som ska tas bort. Användaren får även välja om endast
        /// produkten ska tas bort eller om både produkten och alla tillhörande ordrar ska tas bort.
        /// Vid val av att ta bort både produkt och ordrar görs en kaskad borttagning där alla ordrar som är
        /// kopplade till produkten tas bort.
        /// </remarks>

        static void RaderaProdukt(ProduktHantering produktHantering)
        {
            try
            {
                Console.WriteLine("\n##### Befintliga produkter i databasen #####");

                List<Produkt> produkter = produktHantering.HämtaAllaProdukter();
                foreach (var produkt in produkter)
                {
                    Console.WriteLine($"ID: {produkt.ProduktId}, Namn: {produkt.ProduktNamn}, Pris: {produkt.Pris:C}");
                }

                bool isInvalidIdInput;
                int produktID;

                do
                {
                    Console.Write("\nAnge ID på den produkt du vill radera ifrån databasen: ");
                    isInvalidIdInput = !Int32.TryParse(Console.ReadLine(), out produktID);

                    if (isInvalidIdInput)
                        Console.WriteLine("Ogiltigt produkt ID! Vänligen ange ett giltigt heltal");

                } while (isInvalidIdInput);

                int input = 0;
                do
                {
                    Console.WriteLine("\nVälj om du endast vill ta bort produkten eller produkt OCH tillhörande order/ordrar som är kopplade till produkten");
                    Console.WriteLine("Observera att om du väljer att ta bort endast produkten så kan inga ordrar kopplade till den finnas.");
                    Console.WriteLine("1. Ta bort produkt");
                    Console.WriteLine("2. Ta bort produkt och tillhörande ordrar");
                    Console.Write("Ditt val: ");

                    bool isValidInput = Int32.TryParse(Console.ReadLine(), out input);

                    if (!isValidInput || (input != 1 && input != 2))
                        Console.WriteLine("Ogiltigt val! Vänligen välj 1 eller 2");

                } while (input != 1 && input != 2);

                if (input == 1)
                {
                    produktHantering.RaderaProdukt(produktID);
                    Console.WriteLine($"Lyckat! Produkten med ID '{produktID}' togs bort ifrån databasen!");
                }
                else if (input == 2)
                {
                    produktHantering.RaderaProduktCascade(produktID);
                    Console.WriteLine($"Lyckat! Produkten med ID '{produktID}' och alla tillhörande ordrar togs bort ifrån databasen!");
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Uppdaterar informationen för en produkt i databasen baserat på användarinmatning.
        /// </summary>
        /// <param name="produktHantering">Instans av ProduktHantering.</param>
        /// <remarks>
        /// Ber användaren att ange ID på den produkt som ska uppdateras. Användaren får möjlighet att ändra
        /// produktens namn och pris. Om användaren lämnar fälten tomma behålls de nuvarande värdena för produkten. 
        /// Om användaren anger ett ogiltigt pris kastas ett FormatException-fel med ett felmeddelande.
        /// </remarks>

        static void UppdateraProdukt(ProduktHantering produktHantering)
        {
            try
            {
                Console.WriteLine("\n##### Uppdatera produkt #####");

                List<Produkt> produkter = produktHantering.HämtaAllaProdukter();
                foreach (var produkt in produkter)
                {
                    Console.WriteLine($"ID: {produkt.ProduktId}, Namn: {produkt.ProduktNamn}, Pris: {produkt.Pris:C}");
                }

                Produkt uppdateradProdukt = new Produkt();

                bool isInvalidIdInput;
                int produktID;

                do
                {
                    Console.Write("\nAnge ID på den produkt du vill uppdatera: ");
                    isInvalidIdInput = !Int32.TryParse(Console.ReadLine(), out produktID);

                    if (isInvalidIdInput)
                        Console.WriteLine("Ogiltigt produkt ID! Vänligen ange ett giltigt heltal");

                } while (isInvalidIdInput);

                uppdateradProdukt.ProduktId = produktID;

                Console.Write("Ange nytt produktnamn (lämna tomt för att behålla nuvarande): ");
                string? produktNamn = Console.ReadLine();
                uppdateradProdukt.ProduktNamn = string.IsNullOrEmpty(produktNamn) ? null! : produktNamn;

                Console.Write("Ange nytt pris på produkten (lämna tomt för att behålla nuvarande): ");
                string? prisInput = Console.ReadLine();
                uppdateradProdukt.Pris = string.IsNullOrEmpty(prisInput) ? null
                : Decimal.TryParse(prisInput, out decimal parsedPrice) ? parsedPrice
                : throw new FormatException("Ogiltigt pris! Vänligen ange ett giltigt tal");

                produktHantering.UppdateraProdukt(uppdateradProdukt);

                Console.WriteLine("Lyckat! Produkten har uppdaterats");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Visar en lista över alla ordrar som finns i databasen.
        /// </summary>
        /// <param name="orderHantering">Instans av OrderHantering.</param>
        static void VisaAllaOrdrar(OrderHantering orderHantering)
        {
            try
            {
                Console.WriteLine("\n##### Alla ordrar #####");
                List<Order> ordrar = orderHantering.HämtaAllaOrdrar();

                foreach (var order in ordrar)
                {
                    Console.WriteLine($"OrderId: {order.OrderId}, KundId: {order.KundId}, ProduktId: {order.ProduktId}, Antal: {order.Antal}, Datum: {order.OrderDatum}");
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Hämtar information om en specifik order från databasen baserat på det angivna orderID:t.
        /// </summary>
        /// <param name="orderHantering">Instans av OrderHantering.</param>
        /// <remarks>
        /// Ber användaren att ange ett giltigt orderID och försöker sedan hämta information om ordern.
        /// Om ingen order hittas med det angivna IDt får användaren försöka igen.</remarks>
        static void HämtaOrder(OrderHantering orderHantering)
        {
            try
            {
                while (true)
                {
                    Console.Write("\nAnge ID på den order du vill visa: ");

                    if (!int.TryParse(Console.ReadLine(), out int orderID))
                    {
                        Console.WriteLine("Ogiltigt order ID! Vänligen ange ett giltigt heltal");
                        continue;
                    }

                    Order? order = orderHantering.HämtaOrder(orderID);

                    if (order == null)
                    {
                        Console.WriteLine("Ingen order hittades med det angivna IDt! Försök igen...");
                        continue;
                    }

                    Console.WriteLine($"OrderId: {order.OrderId}, KundId: {order.KundId}, ProduktId: {order.ProduktId}, Antal: {order.Antal}, Datum: {order.OrderDatum}");
                    break;
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Lägg till en ny order i databasen baserat på användarinmatning.
        /// </summary>
        /// <param name="orderHantering">Instans av OrderHantering.</param>
        /// <remarks>
        /// Ber användaren att ange detaljer för en ny order såsom KundId, ProduktId, antal produkter och eventuellt orderdatum.
        /// Om användaren inte anger ett specifikt orderdatum sätts dagens datum automatiskt.
        /// </remarks>

        static void LäggTillNyOrder(OrderHantering orderHantering)
        {
            try
            {
                Console.WriteLine("\n##### Lägg till en ny order i databasen #####");

                Order nyOrder = new Order();

                bool isInvalidKundIdInput;
                int kundId;

                do
                {
                    Console.Write("Ange KundId: ");
                    isInvalidKundIdInput = !Int32.TryParse(Console.ReadLine(), out kundId);

                    if (isInvalidKundIdInput)
                        Console.WriteLine("Ogiltigt KundID! Försök igen...");

                } while (isInvalidKundIdInput);

                nyOrder.KundId = kundId;

                bool isInvalidProduktIdInput;
                int produktId;

                do
                {
                    Console.Write("Ange ProduktId: ");
                    isInvalidProduktIdInput = !Int32.TryParse(Console.ReadLine(), out produktId);

                    if (isInvalidProduktIdInput)
                        Console.WriteLine("Ogiltigt ProduktID! Försök igen...");

                } while (isInvalidProduktIdInput);

                nyOrder.ProduktId = produktId;

                bool isInvalidAntalInput;
                int antal;

                do
                {
                    Console.Write("Ange antal produkter: ");
                    isInvalidAntalInput = !Int32.TryParse(Console.ReadLine(), out antal);

                    if (isInvalidAntalInput || antal <= 0)
                    {
                        isInvalidAntalInput = true;
                        Console.WriteLine("Ogiltigt antal! Ange ett giltigt heltal större än 0");
                    }

                } while (isInvalidAntalInput);

                nyOrder.Antal = antal;

                Console.WriteLine("Vill du ange ett specifikt orderdatum? (J/N): ");
                Console.Write("Om du väljer att inte ange ett specifikt datum så kommer dagens datum att sättas: ");
                string? svar = Console.ReadLine()?.Trim().ToUpper();

                if (svar == "J")
                {
                    bool isInvalidDatumInput;
                    DateTime orderDatum;

                    do
                    {
                        Console.Write("Ange orderdatum (YYYY-MM-DD): ");
                        isInvalidDatumInput = !DateTime.TryParse(Console.ReadLine(), out orderDatum);

                        if (isInvalidDatumInput)
                            Console.WriteLine("Ogiltigt datumformat! Försök igen...");

                    } while (isInvalidDatumInput);

                    nyOrder.OrderDatum = orderDatum;
                }

                orderHantering.NyOrder(nyOrder);

                Console.WriteLine($"Lyckat! Order nr '{nyOrder.OrderId}' har lagts till i databasen.");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }



        /// <summary>
        /// Raderar en specifik order från databasen baserat på användarinmatning.
        /// </summary>
        /// <param name="orderHantering">Instans av OrderHantering.</param>
        /// <remarks>
        /// Ber användaren om ett OrderId för den order som ska raderas.
        /// Om ett ogiltigt OrderId anges upprepas frågan tills ett giltigt OrderId anges.
        /// </remarks>

        static void RaderaOrder(OrderHantering orderHantering)
        {
            try
            {
                Console.WriteLine("\n##### Befintliga ordrar i databasen #####");

                List<Order> ordrar = orderHantering.HämtaAllaOrdrar();
                foreach (var order in ordrar)
                {
                    Console.WriteLine($"OrderId: {order.OrderId}, KundId: {order.KundId}, ProduktId: {order.ProduktId}, Antal: {order.Antal}, Datum: {order.OrderDatum}");
                }

                bool isInvalidIdInput;
                int orderID;

                do
                {
                    Console.Write("\nAnge ID på den order du vill radera ifrån databasen: ");
                    isInvalidIdInput = !Int32.TryParse(Console.ReadLine(), out orderID);

                    if (isInvalidIdInput)
                        Console.WriteLine("Ogiltigt Order ID! Vänligen ange ett giltigt heltal");

                } while (isInvalidIdInput);

                orderHantering.RaderaOrder(orderID);
                Console.WriteLine($"Lyckat! Ordern med ID '{orderID}' togs bort ifrån databasen!");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }



        /// <summary>
        /// Uppdaterar en befintlig order i databasen baserat på användarinmatning.
        /// </summary>
        /// <param name="orderHantering">Instans av OrderHantering.</param>
        /// <remarks>
        /// Ber användaren ange nya värden för KundId, ProduktId, Antal och 
        /// OrderDatum. Om användaren lämnar något av fälten tomma kommer det aktuella värdet för 
        /// fältet att behållas. Vid ogiltig inmatning upprepas frågan tills giltiga värden matas in.
        /// </remarks>

        static void UppdateraOrder(OrderHantering orderHantering)
        {
            try
            {
                Console.WriteLine("\n##### Uppdatera order #####");

                List<Order> ordrar = orderHantering.HämtaAllaOrdrar();
                foreach (var order in ordrar)
                {
                    Console.WriteLine($"OrderId: {order.OrderId}, KundId: {order.KundId}, ProduktId: {order.ProduktId}, Antal: {order.Antal}, Datum: {order.OrderDatum}");
                }

                Order uppdateradOrder = new Order();

                bool isInvalidIdInput;
                int orderID;

                do
                {
                    Console.Write("\nAnge ID på den order du vill uppdatera: ");
                    isInvalidIdInput = !Int32.TryParse(Console.ReadLine(), out orderID);

                    if (isInvalidIdInput)
                        Console.WriteLine("Ogiltigt order ID! Vänligen ange ett giltigt heltal");

                } while (isInvalidIdInput);

                uppdateradOrder.OrderId = orderID;

                Console.Write("Ange nytt KundId (lämna tomt för att behålla nuvarande): ");
                string? kundIdInput = Console.ReadLine();
                uppdateradOrder.KundId = string.IsNullOrEmpty(kundIdInput) ? null : Convert.ToInt32(kundIdInput);

                Console.Write("Ange nytt ProduktId (lämna tomt för att behålla nuvarande): ");
                string? produktIdInput = Console.ReadLine();
                uppdateradOrder.ProduktId = string.IsNullOrEmpty(produktIdInput) ? null : Convert.ToInt32(produktIdInput);

                Console.Write("Ange nytt antal (lämna tomt för att behålla nuvarande): ");
                string? antalInput = Console.ReadLine();
                uppdateradOrder.Antal = string.IsNullOrEmpty(antalInput) ? null : Convert.ToInt32(antalInput);

                Console.Write("Ange nytt orderdatum (ÅÅÅÅ-MM-DD, lämna tomt för att behålla nuvarande): ");
                string? orderDatumInput = Console.ReadLine();
                uppdateradOrder.OrderDatum = string.IsNullOrEmpty(orderDatumInput)
                ? null : DateTime.TryParse(orderDatumInput, out DateTime parsedDate) ? parsedDate
                : throw new FormatException("Ogiltigt datumformat! Ange datumet i formatet ÅÅÅÅ-MM-DD.");

                orderHantering.UppdateraOrder(uppdateradOrder);

                Console.WriteLine("Lyckat! Ordern har uppdaterats");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Ett fel inträffade vid databasoperationen:");
                Console.WriteLine(sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett oväntat fel inträffade: {ex.Message}");
            }
        }


        /// <summary>
        /// Visar huvudmenyn där användaren kan välja vad de vill hantera: kunder, produkter eller ordrar.
        /// </summary>
        /// <param name="kundHantering">
        /// Instans av KundHantering som hanterar allt kundrelaterat.
        /// </param>
        /// <param name="produktHantering">
        /// Instans av ProduktHantering som hanterar allt produktrelaterat.
        /// </param>
        /// <param name="orderHantering">
        /// Instans av OrderHantering som hanterar allt orderrelaterat.
        /// </param>
        /// <remarks>
        /// Skapar en huvudmeny där användaren kan välja ett alternativ för att hantera kunder, 
        /// produkter eller ordrar. Beroende på användarens val, anropas motsvarande meny för kunder, produkter 
        /// eller ordrar. Om användaren väljer att avsluta programmet, stängs programmet ner. Felaktig inmatning hanteras 
        /// genom att användaren får en varning och kan försöka igen.
        /// Programmet fortsätter att köras i en loop tills användaren väljer att avsluta.
        /// </remarks>

        static void Meny(KundHantering kundHantering, ProduktHantering produktHantering, OrderHantering orderHantering)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("##### Välkommen till Uddevalla Klätterbutik API #####");
                Console.WriteLine("Välj vad du vill hantera:");
                Console.WriteLine("1. Kunder");
                Console.WriteLine("2. Produkter");
                Console.WriteLine("3. Ordrar");
                Console.WriteLine("4. Avsluta");
                Console.Write("Ditt val: ");

                if (Int32.TryParse(Console.ReadLine(), out int huvudVal))
                {
                    switch (huvudVal)
                    {
                        case 1:
                            KundMeny(kundHantering);
                            break;
                        case 2:
                            ProduktMeny(produktHantering);
                            break;
                        case 3:
                            OrderMeny(orderHantering);
                            break;
                        case 4:
                            Console.WriteLine("Avslutar programmet...");
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val! Vänligen ange ett nummer mellan 1 och 4.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning! Vänligen ange ett nummer.");
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Visar kundmenyn där användaren kan välja olika åtgärder för att hantera kunder.
        /// </summary>
        /// <param name="kundHantering">
        /// Instans av KundHantering som hanterar allt kundrelaterat.
        /// </param>
        /// <remarks>
        /// Skapar en meny där användaren kan välja att skapa, uppdatera, visa, radera eller hämta kundinformation.
        /// Om användaren väljer att gå tillbaka till huvudmenyn, avslutas 
        /// metoden. Felaktig inmatning hanteras genom att användaren får en varning och kan försöka igen.
        /// Programmet fortsätter att köras i en loop tills användaren väljer att gå tillbaka till huvudmenyn.
        /// </remarks>

        static void KundMeny(KundHantering kundHantering)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("##### Kundhantering #####");
                Console.WriteLine("1. Skapa ny kund");
                Console.WriteLine("2. Uppdatera kund");
                Console.WriteLine("3. Visa alla kunder");
                Console.WriteLine("4. Radera kund");
                Console.WriteLine("5. Hämta kund");
                Console.WriteLine("6. Tillbaka till huvudmenyn");
                Console.Write("Ditt val: ");

                if (Int32.TryParse(Console.ReadLine(), out int kundVal))
                {
                    switch (kundVal)
                    {
                        case 1:
                            SkapaNyKund(kundHantering);
                            break;
                        case 2:
                            UppdateraKund(kundHantering);
                            break;
                        case 3:
                            VisaAllaKunder(kundHantering);
                            break;
                        case 4:
                            RaderaKund(kundHantering);
                            break;
                        case 5:
                            HämtaKund(kundHantering);
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val! Vänligen ange ett nummer mellan 1 och 6.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning! Vänligen ange ett nummer.");
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Visar produktmenyn där användaren kan välja olika åtgärder för att hantera produkter.
        /// </summary>
        /// <param name="produktHantering">
        /// Instans av ProduktHantering som hanterar allt produktrelaterat.
        /// </param>
        /// <remarks>
        /// Skapar en meny där användaren kan välja att lägga till, uppdatera, visa, radera eller hämta produktinformation.
        /// Om användaren väljer att gå tillbaka till huvudmenyn, avslutas metoden. Felaktig inmatning hanteras genom att 
        /// användaren får en varning och kan försöka igen.
        /// Programmet fortsätter att köras i en loop tills användaren väljer att gå tillbaka till huvudmenyn.
        /// </remarks>

        static void ProduktMeny(ProduktHantering produktHantering)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("##### Produkthantering #####");
                Console.WriteLine("1. Lägg till ny produkt");
                Console.WriteLine("2. Uppdatera produkt");
                Console.WriteLine("3. Visa alla produkter");
                Console.WriteLine("4. Radera produkt");
                Console.WriteLine("5. Hämta produkt");
                Console.WriteLine("6. Tillbaka till huvudmenyn");
                Console.Write("Ditt val: ");

                if (Int32.TryParse(Console.ReadLine(), out int produktVal))
                {
                    switch (produktVal)
                    {
                        case 1:
                            LäggTillNyProdukt(produktHantering);
                            break;
                        case 2:
                            UppdateraProdukt(produktHantering);
                            break;
                        case 3:
                            VisaAllaProdukter(produktHantering);
                            break;
                        case 4:
                            RaderaProdukt(produktHantering);
                            break;
                        case 5:
                            HämtaProdukt(produktHantering);
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val! Vänligen ange ett nummer mellan 1 och 6.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning! Vänligen ange ett nummer.");
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }


        /// <summary>
        /// Visar ordermenyn där användaren kan välja olika åtgärder för att hantera ordrar.
        /// </summary>
        /// <param name="orderHantering">
        /// Instans av OrderHantering som hanterar allt orderrelaterat.
        /// </param>
        /// <remarks>
        /// Skapar en meny där användaren kan välja att lägga till, uppdatera, visa, radera eller hämta orderinformation.
        /// Om användaren väljer att gå tillbaka till huvudmenyn, avslutas metoden. Felaktig inmatning hanteras genom att 
        /// användaren får en varning och kan försöka igen.
        /// Programmet fortsätter att köras i en loop tills användaren väljer att gå tillbaka till huvudmenyn.
        /// </remarks>

        static void OrderMeny(OrderHantering orderHantering)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("##### Orderhantering #####");
                Console.WriteLine("1. Lägg till ny order");
                Console.WriteLine("2. Uppdatera order");
                Console.WriteLine("3. Visa alla ordrar");
                Console.WriteLine("4. Radera order");
                Console.WriteLine("5. Hämta order");
                Console.WriteLine("6. Tillbaka till huvudmenyn");
                Console.Write("Ditt val: ");

                if (Int32.TryParse(Console.ReadLine(), out int orderVal))
                {
                    switch (orderVal)
                    {
                        case 1:
                            LäggTillNyOrder(orderHantering);
                            break;
                        case 2:
                            UppdateraOrder(orderHantering);
                            break;
                        case 3:
                            VisaAllaOrdrar(orderHantering);
                            break;
                        case 4:
                            RaderaOrder(orderHantering);
                            break;
                        case 5:
                            HämtaOrder(orderHantering);
                            break;
                        case 6:
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val! Vänligen ange ett nummer mellan 1 och 6.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning! Vänligen ange ett nummer.");
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }
    }
}