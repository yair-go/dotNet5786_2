using DO;

namespace DalList
{
    public class InitDO
    {
        static string RandomPhone(Random rand)
        {
            // Israeli-like 10-digit mobile starting with '05' (0XXXXXXXXX)
            int secondDigit = 5;
            int third = rand.Next(0, 10);
            int remaining = rand.Next(0, 1_000_000_000); // 9 digits remaining combined; we'll build string
            string tail = remaining.ToString("D7"); // ensure length; compose to 10 digits total
            // Compose as 0 5 X #######
            return $"0{secondDigit}{third}{rand.Next(0, 10):D1}{rand.Next(0, 10):D1}{rand.Next(0, 10):D1}{rand.Next(0, 10):D1}{rand.Next(0, 10):D1}{rand.Next(0, 10):D1}{rand.Next(0, 10):D1}";
        }

        static string SanitizeForEmail(string name)
        {
            var s = name.ToLowerInvariant().Replace(" ", ".").Replace("-", "");
            // remove any non-letter/digit/dot characters (simple)
            var arr = new List<char>(s.Length);
            foreach (var ch in s)
                if (char.IsLetterOrDigit(ch) || ch == '.')
                    arr.Add(ch);
            return new string(arr.ToArray());
        }

        public static List<Order> CreateSampleOrders(int count = 20)
        {
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));

            var rnd = new Random(42); // deterministic seed for reproducibility
            const double centerLat = 32.0853;   // Tel Aviv (approx) as central point
            const double centerLon = 34.7818;
            const double maxRadiusKm = 50.0;    // reasonable coverage for delivery area

            var customerNames = new[]
            {
                "Avi Cohen","Maya Levi","Eitan Shapiro","Noga Kaplan","Omer Azulay","Lior Rubin",
                "Yael Feldman","Gal Itzkovitz","Rinat Barak","Noam Stern","Hadar Amir","Shai Peretz",
                "Yonatan Mor","Tamar Navon","Sarit Golan","Uri Harari","Michal Klein","Assaf Ziv",
                "Roni Ezra","Dafna Shalev","Efrat Naveh","Boaz Dayan","Leah Kadosh","Yossi Ben-Ami"
            };

            var streets = new[]
            {
                "HaNevi'im 7, Jerusalem, Israel",
                "Jabotinsky 12, Tel Aviv-Yafo, Israel",
                "HaHashmonaim 45, Petah Tikva, Israel",
                "Derech Ben-Gurion 3, Haifa, Israel",
                "Herzl 20, Rishon LeZion, Israel",
                "Yarkon 15, Tel Aviv-Yafo, Israel",
                "Hertzel 1, Holon, Israel",
                "HaPalmach 9, Kfar Saba, Israel",
                "Haneviim 11, Be'er Sheva, Israel",
                "HaTaasiya 4, Netanya, Israel"
            };

            var orderTypes = (OrderType[])Enum.GetValues(typeof(OrderType));
            var list = new List<Order>(count);
            int baseId = 300_000_000;

            for (int i = 0; i < count; i++)
            {
                int id = baseId + i + 1;
                var type = orderTypes[rnd.Next(orderTypes.Length)]; // uniform distribution
                var customer = customerNames[i % customerNames.Length];
                var address = streets[rnd.Next(streets.Length)];

                // random coordinates near center within maxRadiusKm
                (double lat, double lon) = RandomCoordinateNear(centerLat, centerLon, rnd.NextDouble() * maxRadiusKm, rnd);

                // phone: 10 digits starting with '0' (format: 05Xxxxxxxx)
                string phone = RandomPhone(rnd);

                // opened time: within last 30 days, random hour/minute
                DateTime openedAt = DateTime.Now
                    .AddDays(-rnd.Next(0, 30))
                    .AddHours(-rnd.Next(0, 24))
                    .AddMinutes(-rnd.Next(0, 60));

                // optional fields
                string? description = rnd.NextDouble() < 0.6 ? $"Order for {customer} - items #{i + 1}" : null;
                double? weight = rnd.NextDouble() < 0.7 ? Math.Round(0.1 + rnd.NextDouble() * 20.0, 2) : null; // kg
                double? volume = rnd.NextDouble() < 0.5 ? Math.Round(0.001 + rnd.NextDouble() * 0.5, 3) : null; // m^3
                bool isFragile = rnd.NextDouble() < 0.15;

                var order = new Order(
                    Id: id,
                    Type: type,
                    Address: address,
                    Latitude: lat,
                    Longitude: lon,
                    CustomerName: customer,
                    CustomerPhone: phone,
                    OpenedAt: openedAt,
                    Description: description,
                    Weight: weight,
                    Volume: volume,
                    IsFragile: isFragile
                );

                list.Add(order);
            }

            return list;
        }



        // Returns approximate coordinates (lat, lon) a given distance (km) away from center at random bearing.
        static (double lat, double lon) RandomCoordinateNear(double centerLat, double centerLon, double distanceKm, Random rnd)
        {
            // Convert to radians
            double R = 6371.0; // Earth radius km
            double bearing = rnd.NextDouble() * 2.0 * Math.PI;
            double dR = distanceKm / R;

            double lat1 = DegreesToRadians(centerLat);
            double lon1 = DegreesToRadians(centerLon);

            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(dR) + Math.Cos(lat1) * Math.Sin(dR) * Math.Cos(bearing));
            double lon2 = lon1 + Math.Atan2(Math.Sin(bearing) * Math.Sin(dR) * Math.Cos(lat1), Math.Cos(dR) - Math.Sin(lat1) * Math.Sin(lat2));

            return (RadiansToDegrees(lat2), RadiansToDegrees(lon2));
        }

        static double DegreesToRadians(double deg) => deg * Math.PI / 180.0;
        static double RadiansToDegrees(double rad) => rad * 180.0 / Math.PI;
    }
}
