using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_2
{
    internal class DB
    {
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "two.db");

        private static readonly string ConnectionString = new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();
        public void Add(string name, string text)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO zadachi (Name, Text) VALUES (@name, @text)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@text", text);
            command.ExecuteNonQuery();
        }
        public void PrintAll()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Name, Text FROM zadachi";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                string text = reader.GetString(1);
                Console.WriteLine($"Name: {name}, Text: {text}");
            }
        }
        public void Remove(string name)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM zadachi WHERE Name = @name";
            command.Parameters.AddWithValue("@name", name);
            int rows = command.ExecuteNonQuery();
            Console.WriteLine($"Удалено строк: {rows}");
        }
        public void Update(string name, string newText)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE zadachi SET Text = @newText WHERE Name = @name";
            command.Parameters.AddWithValue("@newText", newText);
            command.Parameters.AddWithValue("@name", name);
            int rows = command.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine($"Обновлено строк: {rows}");
            else
                Console.WriteLine($"Задача с названием \"{name}\" не найдена.");
        }
    }
}
