using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_2
{
    internal class TaskItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public override string ToString() => $"Name: {Name}, Text: {Text}";
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
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "two.db");
        private static readonly string ConnectionString = new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();
        // Добавление задачи
        public OperationResult Add(string name, string text)
        {
            name = name?.Trim();
            text = text?.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(text))
                return OperationResult.Fail("Заполните оба поля.");
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO zadachi (Name, Text) VALUES (@name, @text)";
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@text", text);
                        command.ExecuteNonQuery();
                    }
                }
                return OperationResult.Ok($"Добавлено: {name}");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Получение всех задач
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
                        command.CommandText = "SELECT Name, Text FROM zadachi";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new TaskItem
                                {
                                    Name = reader.GetString(0),
                                    Text = reader.GetString(1)
                                };
                                result.Items.Add(item.ToString());
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
        // Удаление задачи
        public OperationResult Remove(string name)
        {
            name = name?.Trim();
            if (string.IsNullOrEmpty(name))
                return OperationResult.Fail("Введите название для удаления.");
            try
            {
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM zadachi WHERE Name = @name";
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
        // Обновление задачи
        public OperationResult Update(string name, string newText)
        {
            name = name?.Trim();
            newText = newText?.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(newText))
                return OperationResult.Fail("Заполните оба поля.");
            try
            {
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE zadachi SET Text = @newText WHERE Name = @name";
                        command.Parameters.AddWithValue("@newText", newText);
                        command.Parameters.AddWithValue("@name", name);
                        rows = command.ExecuteNonQuery();
                    }
                }
                if (rows > 0)
                    return OperationResult.Ok($"Обновлено строк: {rows}");
                else
                    return OperationResult.Fail($"Задача с названием \"{name}\" не найдена.");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
    }
}