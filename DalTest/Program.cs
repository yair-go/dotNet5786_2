using System;
using System.Collections.Generic;
using DO;

namespace DalTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var couriers = CreateFixedCourierCollection();
            Console.WriteLine($"Created {couriers.Count} couriers:");
            foreach (var courier in couriers)
            {
                Console.WriteLine($"{courier.Id:D9} | {courier.FullName} | {courier.Phone} | {courier.Email} | {courier.DeliveryType} | Active:{courier.IsActive} | MaxKm:{(courier.MaxDeliveryDistance?.ToString("F1") ?? "no-limit")} | Start:{courier.StartWork:yyyy-MM-dd}");
            }
        }

        static List<Courier> CreateFixedCourierCollection()
        {
            var rand = new Random(42); // deterministic for repeatability
            const double companyMaxKm = 50.0; // company-wide maximum delivery distance (km)

            // fixed human-readable names to ensure a reproducible, inspectable dataset
            var names = new[]
            {
                "Avi Cohen","Maya Levi","Eitan Shapiro","Noga Kaplan","Omer Azulay","Lior Rubin",
                "Yael Feldman","Gal Itzkovitz","Rinat Barak","Noam Stern","Hadar Amir","Shai Peretz",
                "Yonatan Mor","Tamar Navon","Sarit Golan","Uri Harari","Michal Klein","Assaf Ziv",
                "Roni Ezra","Dafna Shalev","Efrat Naveh","Boaz Dayan","Leah Kadosh","Yossi Ben-Ami"
            };

            var couriers = new List<Courier>(names.Length);

            // choose a few indices to be inactive (ex-employees with history)
            var inactiveIndices = new HashSet<int>();
            while (inactiveIndices.Count < 3) // "few" inactive
                inactiveIndices.Add(rand.Next(names.Length));

            for (int i = 0; i < names.Length; i++)
            {
                int id = 100000000 + i + 1; // simple unique id (assume valid check-digit handled in logic layer)
                string fullName = names[i];

                // phone: 10 digits, starts with '0' (format: 05Xxxxxxxx)
                string phone = RandomPhone(rand);

                // email: deterministic from name to keep dataset fixed
                string email = $"{SanitizeForEmail(fullName)}@example.com";

                // delivery type: uniform probability for all four types
                DeliveryType dtype = (DeliveryType)rand.Next(0, 4);

                // random decide whether courier has a personal max distance (60% chance to have one)
                bool hasPersonalMax = rand.NextDouble() < 0.6;

                double? maxKm = null;
                if (hasPersonalMax)
                {
                    // reasonable ranges by transport mode (km)
                    (double min, double max) = dtype switch
                    {
                        DeliveryType.Vehicle => (10.0, 50.0),
                        DeliveryType.Motorcycle => (8.0, 30.0),
                        DeliveryType.Bicycle => (2.0, 15.0),
                        DeliveryType.Pedestrian => (0.5, 5.0),
                        _ => (1.0, companyMaxKm)
                    };

                    // generate and clamp to company max
                    var raw = min + rand.NextDouble() * (max - min);
                    maxKm = Math.Min(raw, companyMaxKm);
                }

                // StartWork: a past time before now; choose between 1 day and 10 years ago,
                // inactive couriers tend to have older StartWork
                DateTime now = DateTime.Now;
                int maxDaysAgo = inactiveIndices.Contains(i) ? 3650 : 2000; // older for inactive
                int daysAgo = 1 + rand.Next(0, maxDaysAgo);
                var startWork = now.AddDays(-daysAgo)
                                   .AddHours(-rand.Next(0, 24))
                                   .AddMinutes(-rand.Next(0, 60));

                // active status
                bool isActive = !inactiveIndices.Contains(i);

                // password: leave null for most (DAL will store encrypted value when provided)
                string? password = null;
                if (rand.NextDouble() < 0.25) // some couriers already had initial password set
                    password = "encrypted-placeholder"; // DAL assumes encryption done earlier

                var courier = new Courier(
                    Id: id,
                    FullName: fullName,
                    Phone: phone,
                    Email: email,
                    StartWork: startWork,
                    DeliveryType: dtype,
                    Password: password,
                    IsActive: isActive,
                    MaxDeliveryDistance: maxKm
                );

                couriers.Add(courier);
            }

            return couriers;
        }

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
    }
}
