using System;
using CargoCarrier.Mvc.Models;
using Microsoft.Data.Sqlite;

namespace CargoCarrier.Mvc.Database
{
    public class TripDatabase
    {
        private readonly string _connectionString;

        public TripDatabase()
        {
            _connectionString = "Data Source=Memory;Mode=Memory;Cache=Shared";
            InitializeDatabase();
        }

        // seeding the database with some sample data
        // dirty way to do it, but it's just for the sake of the demo
        // do not change this method
        private void InitializeDatabase()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"
                CREATE TABLE Trips (
                    Id INTEGER PRIMARY KEY,
                    ParcelSize INTEGER NOT NULL,
                    LicensePlate TEXT NOT NULL,
                    Start TEXT NOT NULL,
                    End TEXT NOT NULL
                )";
            createTableCommand.ExecuteNonQuery();

            // Seed data
            var seedTripsCommand = connection.CreateCommand();
            seedTripsCommand.CommandText = @"
                INSERT INTO Trips (ParcelSize, LicensePlate, Start, End) VALUES
                (02, 'ABC123', '2024-03-25 08:00:00', '2024-03-25 10:00:00'),
                (20, 'XYZ789', '2024-03-26 09:00:00', '2024-03-26 11:00:00'),
                (10, 'DEF456', '2024-03-27 10:00:00', '2024-03-27 12:00:00')";
            seedTripsCommand.ExecuteNonQuery();
        }


        // This method must be fixed
        public async Task<List<Trip>> GetTripsWithParcelSizeLessThanAsync(string maxParcelSize)
        {
            var trips = new List<Trip>();

            var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Trips WHERE ParcelSize < " + maxParcelSize;

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var trip = new Trip
                {
                    Id = reader.GetInt32(0),
                    ParcelSize = reader.GetInt32(1),
                    LicensePlate = reader.GetString(2),
                    Start = DateTime.Parse(reader.GetString(3)),
                    End = DateTime.Parse(reader.GetString(4))
                };
                trips.Add(trip);
            }

            return trips;
        }
    }
}
