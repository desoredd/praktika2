using Microsoft.Data.Sqlite;
namespace Модуль_6_Задание_3
{
    internal class DB
    {
        private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "tree.db");

        private static readonly string ConnectionString = new SqliteConnectionStringBuilder { DataSource = DbPath }.ToString();
        // Добавление новой книги
        public void Add(string name, string author, string genre, int vp)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO library (name, author, genre, vp) VALUES (@name, @author, @genre, @vp)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@author", author);
            command.Parameters.AddWithValue("@genre", genre);
            command.Parameters.AddWithValue("@vp", vp);
            command.ExecuteNonQuery();
        }
        // Вывод всех книг
        public void PrintAll()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name, author, genre, vp FROM library";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                string author = reader.GetString(1);
                string genre = reader.GetString(2);
                int vp = reader.GetInt32(3);
                Console.WriteLine($"Название: {name}, Автор: {author}, Жанр: {genre}, Год выпуска: {vp}");
            }
        }
        // Удаление книги по названию
        public void Remove(string name)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM library WHERE name = @name";
            command.Parameters.AddWithValue("@name", name);
            int rows = command.ExecuteNonQuery();
            Console.WriteLine($"Удалено строк: {rows}");
        }
        // Обновление данных книги по названию (все поля кроме названия)
        public void Update(string name, string newAuthor, string newGenre, int newVp)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE library SET author = @author, genre = @genre, vp = @vp WHERE name = @name";
            command.Parameters.AddWithValue("@author", newAuthor);
            command.Parameters.AddWithValue("@genre", newGenre);
            command.Parameters.AddWithValue("@vp", newVp);
            command.Parameters.AddWithValue("@name", name);
            int rows = command.ExecuteNonQuery();
            if (rows > 0)
                Console.WriteLine($"Обновлено строк: {rows}");
            else
                Console.WriteLine($"Книга с названием \"{name}\" не найдена.");
        }
        // Фильтр по названию
        public void FilterByName(string name)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name, author, genre, vp FROM library WHERE name = @name";
            command.Parameters.AddWithValue("@name", name);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Название: {reader.GetString(0)}, Автор: {reader.GetString(1)}, Жанр: {reader.GetString(2)}, Год выпуска: {reader.GetInt32(3)}");
            }
        }
        // Фильтр по автору
        public void FilterByAuthor(string author)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name, author, genre, vp FROM library WHERE author = @author";
            command.Parameters.AddWithValue("@author", author);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Название: {reader.GetString(0)}, Автор: {reader.GetString(1)}, Жанр: {reader.GetString(2)}, Год выпуска: {reader.GetInt32(3)}");
            }
        }
        // Фильтр по жанру
        public void FilterByGenre(string genre)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name, author, genre, vp FROM library WHERE genre = @genre";
            command.Parameters.AddWithValue("@genre", genre);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Название: {reader.GetString(0)}, Автор: {reader.GetString(1)}, Жанр: {reader.GetString(2)}, Год выпуска: {reader.GetInt32(3)}");
            }
        }
        // Фильтр по году выпуска (vp)
        public void FilterByVp(int vp)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name, author, genre, vp FROM library WHERE vp = @vp";
            command.Parameters.AddWithValue("@vp", vp);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Название: {reader.GetString(0)}, Автор: {reader.GetString(1)}, Жанр: {reader.GetString(2)}, Год выпуска: {reader.GetInt32(3)}");
            }
        }
    }
}
