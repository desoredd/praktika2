using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_3
{
    internal class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Vp { get; set; }
        public override string ToString() =>
            $"Название: {Name}, Автор: {Author}, Жанр: {Genre}, Год выпуска: {Vp}";
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
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "tree.db");
        private static readonly string ConnectionString = new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();
        // Добавление новой книги
        public OperationResult Add(string name, string author, string genre, string vpText)
        {
            name = name?.Trim();
            author = author?.Trim();
            genre = genre?.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre))
                return OperationResult.Fail("Заполните все текстовые поля.");
            if (!int.TryParse(vpText, out int vp))
                return OperationResult.Fail("Год выпуска должен быть числом.");
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO library (name, author, genre, vp) VALUES (@name, @author, @genre, @vp)";
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@author", author);
                        command.Parameters.AddWithValue("@genre", genre);
                        command.Parameters.AddWithValue("@vp", vp);
                        command.ExecuteNonQuery();
                    }
                }
                return OperationResult.Ok($"Книга \"{name}\" добавлена.");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Получение всех книг
        public OperationResult GetAll()
        {
            string query = "SELECT name, author, genre, vp FROM library";
            return ExecuteSelect(query);
        }
        // Удаление книги по названию
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
                        command.CommandText = "DELETE FROM library WHERE name = @name";
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
        // Обновление данных книги по названию
        public OperationResult Update(string name, string newAuthor, string newGenre, string newVpText)
        {
            name = name?.Trim();
            newAuthor = newAuthor?.Trim();
            newGenre = newGenre?.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(newAuthor) || string.IsNullOrEmpty(newGenre))
                return OperationResult.Fail("Заполните все текстовые поля.");
            if (!int.TryParse(newVpText, out int newVp))
                return OperationResult.Fail("Год выпуска должен быть числом.");
            try
            {
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE library SET author = @author, genre = @genre, vp = @vp WHERE name = @name";
                        command.Parameters.AddWithValue("@author", newAuthor);
                        command.Parameters.AddWithValue("@genre", newGenre);
                        command.Parameters.AddWithValue("@vp", newVp);
                        command.Parameters.AddWithValue("@name", name);
                        rows = command.ExecuteNonQuery();
                    }
                }
                if (rows > 0)
                    return OperationResult.Ok($"Обновлено строк: {rows}");
                else
                    return OperationResult.Fail($"Книга с названием \"{name}\" не найдена.");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Фильтры
        public OperationResult FilterByName(string name)
        {
            return ExecuteSelect("SELECT name, author, genre, vp FROM library WHERE name = @val", ("@val", name?.Trim()));
        }
        public OperationResult FilterByAuthor(string author)
        {
            return ExecuteSelect("SELECT name, author, genre, vp FROM library WHERE author = @val", ("@val", author?.Trim()));
        }
        public OperationResult FilterByGenre(string genre)
        {
            return ExecuteSelect("SELECT name, author, genre, vp FROM library WHERE genre = @val", ("@val", genre?.Trim()));
        }
        public OperationResult FilterByVp(string vpText)
        {
            if (!int.TryParse(vpText, out int vp))
                return OperationResult.Fail("Год выпуска должен быть числом.");
            return ExecuteSelect("SELECT name, author, genre, vp FROM library WHERE vp = @val", ("@val", vp));
        }
        // Вспомогательный метод для SELECT-запросов
        private OperationResult ExecuteSelect(string query, params (string Name, object Value)[] parameters)
        {
            try
            {
                var result = OperationResult.Ok();
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        foreach (var p in parameters)
                            command.Parameters.AddWithValue(p.Name, p.Value);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var book = new Book
                                {
                                    Name = reader.GetString(0),
                                    Author = reader.GetString(1),
                                    Genre = reader.GetString(2),
                                    Vp = reader.GetInt32(3)
                                };
                                result.Items.Add(book.ToString());
                            }
                        }
                    }
                }
                if (result.Items.Count == 0 && parameters.Length > 0)
                    result.Message = "Ничего не найдено.";
                return result;
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
    }
}