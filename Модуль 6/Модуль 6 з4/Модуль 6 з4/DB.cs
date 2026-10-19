using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_4
{
    internal class Transaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public override string ToString()
        {
            string typeRus = Type == "income" ? "Доход" : "Расход";
            return $"[{Id}] {Date} | {typeRus,-6} | {Category,-15} | {Amount,10:F2} | {Comment}";
        }
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
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "four.db");
        private static readonly string ConnectionString = new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();
        // Добавление записи
        public OperationResult Add(string type, string category, string amountText, string date, string comment)
        {
            if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(date))
                return OperationResult.Fail("Заполните категорию и дату.");
            if (!double.TryParse(amountText, out double amount))
                return OperationResult.Fail("Сумма должна быть числом.");
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO transactions (type, category, amount, date, comment) 
                                                VALUES (@type, @category, @amount, @date, @comment)";
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@amount", amount);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@comment", comment ?? "");
                        command.ExecuteNonQuery();
                    }
                }
                return OperationResult.Ok("Запись добавлена.");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Вывод всех записей
        public OperationResult GetAll()
        {
            return ExecuteSelect("SELECT id, type, category, amount, date, comment FROM transactions ORDER BY date DESC");
        }
        // Редактирование записи по id
        public OperationResult Update(string idText, string type, string category, string amountText, string date, string comment)
        {
            if (!int.TryParse(idText, out int id))
                return OperationResult.Fail("ID должен быть целым числом.");
            if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(date))
                return OperationResult.Fail("Заполните категорию и дату.");
            if (!double.TryParse(amountText, out double amount))
                return OperationResult.Fail("Сумма должна быть числом.");
            try
            {
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"UPDATE transactions 
                                                SET type = @type, category = @category, amount = @amount, 
                                                    date = @date, comment = @comment
                                                WHERE id = @id";
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@amount", amount);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@comment", comment ?? "");
                        command.Parameters.AddWithValue("@id", id);
                        rows = command.ExecuteNonQuery();
                    }
                }
                return rows > 0 ? OperationResult.Ok($"Запись #{id} обновлена.")
                                : OperationResult.Fail($"Запись #{id} не найдена.");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Удаление записи по id
        public OperationResult Remove(string idText)
        {
            if (!int.TryParse(idText, out int id))
                return OperationResult.Fail("ID должен быть целым числом.");
            try
            {
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM transactions WHERE id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        rows = command.ExecuteNonQuery();
                    }
                }
                return rows > 0 ? OperationResult.Ok($"Запись #{id} удалена.")
                                : OperationResult.Fail($"Запись #{id} не найдена.");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Итоги: суммарный доход, расход, баланс
        public OperationResult GetSummary()
        {
            try
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            SELECT 
                                IFNULL(SUM(CASE WHEN type = 'income'  THEN amount ELSE 0 END), 0),
                                IFNULL(SUM(CASE WHEN type = 'expense' THEN amount ELSE 0 END), 0)
                            FROM transactions";
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                double income = reader.GetDouble(0);
                                double expense = reader.GetDouble(1);
                                double balance = income - expense;
                                string msg = $"Доходы: {income:F2} | Расходы: {expense:F2} | Баланс: {balance:F2}";
                                return OperationResult.Ok(msg);
                            }
                        }
                    }
                }
                return OperationResult.Fail("Не удалось получить итоги.");
            }
            catch (SqliteException ex)
            {
                return OperationResult.Fail($"Ошибка БД: {ex.Message}");
            }
        }
        // Фильтры
        public OperationResult FilterByType(string type) =>
            ExecuteSelect("SELECT id, type, category, amount, date, comment FROM transactions WHERE type = @val ORDER BY date DESC", ("@val", type));
        public OperationResult FilterByCategory(string category) =>
            ExecuteSelect("SELECT id, type, category, amount, date, comment FROM transactions WHERE category = @val ORDER BY date DESC", ("@val", category));
        public OperationResult FilterByDate(string date) =>
            ExecuteSelect("SELECT id, type, category, amount, date, comment FROM transactions WHERE date = @val ORDER BY date DESC", ("@val", date));
        // Вспомогательный метод
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
                                var t = new Transaction
                                {
                                    Id = reader.GetInt32(0),
                                    Type = reader.GetString(1),
                                    Category = reader.GetString(2),
                                    Amount = reader.GetDouble(3),
                                    Date = reader.GetString(4),
                                    Comment = reader.IsDBNull(5) ? "" : reader.GetString(5)
                                };
                                result.Items.Add(t.ToString());
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