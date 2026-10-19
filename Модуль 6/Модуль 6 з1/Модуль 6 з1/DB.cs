using System.Collections.Generic;
using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_1
{
    internal class Employee
    {
        public string Name { get; set; }
        public string Dolwnost { get; set; }
        public override string ToString() => $"Name: {Name}, Dolwnost: {Dolwnost}";
    }
    internal class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Items { get; set; } = new List<string>();
        public static OperationResult Ok(string message = "") =>
            new OperationResult { Success = true, Message = message };

        public static OperationResult Fail(string message) =>
            new OperationResult { Success = false, Message = message };
    }
    internal class DB
    {
        private const string ConnectionString = @"Data Source=one.db";
        // Добавление сотрудника
        public OperationResult Add(string name, string dolwnost)
        {
            name = name?.Trim();
            dolwnost = dolwnost?.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(dolwnost))
                return OperationResult.Fail("Заполните оба поля.");
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Sotrydniki (Name, Dolwnost) VALUES (@name, @dolwnost)";
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@dolwnost", dolwnost);
                        command.ExecuteNonQuery();
                    }
                }
                return OperationResult.Ok($"Добавлен: {name}");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Удаление сотрудника
        public OperationResult Remove(string name)
        {
            name = name?.Trim();
            if (string.IsNullOrEmpty(name))
                return OperationResult.Fail("Введите имя для удаления.");
            try
            {
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM Sotrydniki WHERE Name = @name";
                        command.Parameters.AddWithValue("@name", name);
                        rows = command.ExecuteNonQuery();
                    }
                }
                return OperationResult.Ok($"Удалено строк: {rows}");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Получение всех сотрудников
        public OperationResult GetAll()
        {
            try
            {
                var result = OperationResult.Ok();
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT Name, Dolwnost FROM Sotrydniki";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var emp = new Employee
                                {
                                    Name = reader.GetString(0),
                                    Dolwnost = reader.GetString(1)
                                };
                                result.Items.Add(emp.ToString());
                            }
                        }
                    }
                }
                return result;
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
    }
}