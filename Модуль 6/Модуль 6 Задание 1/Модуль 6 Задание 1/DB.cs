using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_1
{
    internal class DB
    {
        private const string ConnectionString = @"Data Source=C:\Users\Артем\Source\Repos\Модуль 6 Задание 1\Модуль 6 Задание 1\bin\Debug\net8.0\one.db";
        // Добавление сотрудника
        public void Add(string name, string dolwnost)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Sotrydniki (Name, Dolwnost) VALUES (@name, @dolwnost)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@dolwnost", dolwnost);
            command.ExecuteNonQuery();
        }
        public void Remove(string name)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Sotrydniki WHERE Name = @name";
            command.Parameters.AddWithValue("@name", name);
            int rows = command.ExecuteNonQuery();
            Console.WriteLine($"Удалено строк: {rows}");
        }
        // Вывод всех сотрудников в консоль
        public void PrintAll()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Name, Dolwnost FROM Sotrydniki";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                string dolwnost = reader.GetString(1);
                Console.WriteLine($"Name: {name}, Dolwnost: {dolwnost}");
            }
        }
    }
}