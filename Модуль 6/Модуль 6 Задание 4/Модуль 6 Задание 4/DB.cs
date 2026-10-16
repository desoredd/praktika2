using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_4
{
    internal class DB
    {
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "four.db");

        private static readonly string ConnectionString =
            new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();
        // Добавление записи
        public void Add(string type, string category, double amount, string date, string comment)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO transactions (type, category, amount, date, comment) 
                                    VALUES (@type, @category, @amount, @date, @comment)";
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@comment", comment ?? "");
            command.ExecuteNonQuery();
        }

        // Вывод всех записей
        public void PrintAll()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, type, category, amount, date, comment FROM transactions ORDER BY date DESC";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string type = reader.GetString(1);
                string category = reader.GetString(2);
                double amount = reader.GetDouble(3);
                string date = reader.GetString(4);
                string comment = reader.IsDBNull(5) ? "" : reader.GetString(5);

                string typeRus = type == "income" ? "Доход" : "Расход";
                Console.WriteLine($"[{id}] {date} | {typeRus,-6} | {category,-15} | {amount,10:F2} | {comment}");
            }
        }

        // Редактирование записи по id
        public void Update(int id, string type, string category, double amount, string date, string comment)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
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
            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? $"Запись #{id} обновлена." : $"Запись #{id} не найдена.");
        }

        // Удаление записи по id
        public void Remove(int id)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM transactions WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? $"Запись #{id} удалена." : $"Запись #{id} не найдена.");
        }

        // Итоги: суммарный доход, расход, баланс
        public void PrintSummary()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 
                    IFNULL(SUM(CASE WHEN type = 'income'  THEN amount ELSE 0 END), 0),
                    IFNULL(SUM(CASE WHEN type = 'expense' THEN amount ELSE 0 END), 0)
                FROM transactions";
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                double income = reader.GetDouble(0);
                double expense = reader.GetDouble(1);
                double balance = income - expense;
                Console.WriteLine($"Доходы:  {income,10:F2}");
                Console.WriteLine($"Расходы: {expense,10:F2}");
                Console.WriteLine($"Баланс:  {balance,10:F2}");
            }
        }

        // Фильтры
        public void FilterByType(string type)   // "income" / "expense"
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, type, category, amount, date, comment FROM transactions WHERE type = @type ORDER BY date DESC";
            command.Parameters.AddWithValue("@type", type);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string typeRus = reader.GetString(1) == "income" ? "Доход" : "Расход";
                Console.WriteLine($"[{reader.GetInt32(0)}] {reader.GetString(4)} | {typeRus,-6} | {reader.GetString(2),-15} | {reader.GetDouble(3),10:F2} | {(reader.IsDBNull(5) ? "" : reader.GetString(5))}");
            }
        }

        public void FilterByCategory(string category)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, type, category, amount, date, comment FROM transactions WHERE category = @category ORDER BY date DESC";
            command.Parameters.AddWithValue("@category", category);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string typeRus = reader.GetString(1) == "income" ? "Доход" : "Расход";
                Console.WriteLine($"[{reader.GetInt32(0)}] {reader.GetString(4)} | {typeRus,-6} | {reader.GetString(2),-15} | {reader.GetDouble(3),10:F2} | {(reader.IsDBNull(5) ? "" : reader.GetString(5))}");
            }
        }

        public void FilterByDate(string date)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, type, category, amount, date, comment FROM transactions WHERE date = @date ORDER BY date DESC";
            command.Parameters.AddWithValue("@date", date);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string typeRus = reader.GetString(1) == "income" ? "Доход" : "Расход";
                Console.WriteLine($"[{reader.GetInt32(0)}] {reader.GetString(4)} | {typeRus,-6} | {reader.GetString(2),-15} | {reader.GetDouble(3),10:F2} | {(reader.IsDBNull(5) ? "" : reader.GetString(5))}");
            }
        }
    }
}
