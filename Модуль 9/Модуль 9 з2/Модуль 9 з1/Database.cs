using Microsoft.Data.Sqlite;
namespace ProjectManagerWpf
{
    public class Database
    {
        private readonly string _connectionString;
        public Database(string dbPath = "project_manager.db")
        {
            _connectionString = "Data Source=" + dbPath;
        }
        public SqliteConnection Open()
        {
            var conn = new SqliteConnection(_connectionString);
            conn.Open();
            return conn;
        }
        public void Initialize()
        {
            using (var conn = Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    PRAGMA foreign_keys = ON;
                    CREATE TABLE IF NOT EXISTS Employees (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT NOT NULL,
                        Position TEXT NOT NULL,
                        Email TEXT NOT NULL UNIQUE
                    );
                    CREATE TABLE IF NOT EXISTS Projects (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        StartDate TEXT NOT NULL,
                        EndDate TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProjectId INTEGER NOT NULL,
                        EmployeeId INTEGER,
                        Title TEXT NOT NULL,
                        Description TEXT,
                        Status INTEGER NOT NULL DEFAULT 0,
                        Progress INTEGER NOT NULL DEFAULT 0,
                        CreatedAt TEXT NOT NULL,
                        FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
                        FOREIGN KEY (EmployeeId) REFERENCES Employees(Id) ON DELETE SET NULL
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}