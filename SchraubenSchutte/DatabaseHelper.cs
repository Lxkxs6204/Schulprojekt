using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SchraubenSchuette
{
    public static class DatabaseHelper
    {
        private static readonly string _connectionString = "Data Source=produkte.db;Version=3;";

        public static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Produkte (
                                Artikelnummer TEXT PRIMARY KEY,
                                Bezeichnung TEXT,
                                Anzahl INTEGER,
                                Material TEXT,
                                Maße TEXT,
                                Einkaufspreis REAL,
                                Verkaufspreis REAL,
                                Lieferant TEXT,
                                BildPfad TEXT
                            );";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

        }


        public static void ProduktSpeichern(Product produkt)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = @"INSERT OR REPLACE INTO Produkte
                               (Artikelnummer, Bezeichnung, Anzahl, Material, Maße, Einkaufspreis, Verkaufspreis, Lieferant, BildPfad)
                               VALUES (@Artikelnummer, @Bezeichnung, @Anzahl, @Material, @Maße, @Einkaufspreis, @Verkaufspreis, @Lieferant, @BildPfad);";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Artikelnummer", produkt.Artikelnummer);
                    command.Parameters.AddWithValue("@Bezeichnung", produkt.Bezeichnung);
                    command.Parameters.AddWithValue("@Anzahl", produkt.Anzahl);
                    command.Parameters.AddWithValue("@Material", produkt.Material);
                    command.Parameters.AddWithValue("@Maße", produkt.Maße);
                    command.Parameters.AddWithValue("@Einkaufspreis", produkt.Einkaufspreis);
                    command.Parameters.AddWithValue("@Verkaufspreis", produkt.Verkaufspreis);
                    command.Parameters.AddWithValue("@Lieferant", produkt.Lieferant);
                    command.Parameters.AddWithValue("@BildPfad", produkt.BildPfad);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Product> ProdukteLaden()
        {
            var produkte = new List<Product>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Produkte;";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        produkte.Add(new Product
                        {
                            Artikelnummer = reader["Artikelnummer"].ToString(),
                            Bezeichnung = reader["Bezeichnung"].ToString(),
                            Anzahl = Convert.ToInt32(reader["Anzahl"]),
                            Material = reader["Material"].ToString(),
                            Maße = reader["Maße"].ToString(),
                            Einkaufspreis = Convert.ToDecimal(reader["Einkaufspreis"]),
                            Verkaufspreis = Convert.ToDecimal(reader["Verkaufspreis"]),
                            Lieferant = reader["Lieferant"].ToString(),
                            BildPfad = reader["BildPfad"].ToString()
                        });
                    }
                }
            }

            return produkte;
        }
    }
}
