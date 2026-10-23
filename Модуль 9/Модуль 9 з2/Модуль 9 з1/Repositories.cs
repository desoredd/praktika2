using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
namespace ProjectManagerWpf
{
    public class EmployeeRepository
    {
        private readonly Database _db;
        public EmployeeRepository(Database db)
        {
            _db = db;
        }
        public List<Employee> GetAll()
        {
            var result = new List<Employee>();
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, FullName, Position, Email FROM Employees ORDER BY Id";
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        result.Add(new Employee
                        {
                            Id = r.GetInt32(0),
                            FullName = r.GetString(1),
                            Position = r.GetString(2),
                            Email = r.GetString(3)
                        });
                    }
                }
            }
            return result;
        }
        public int Add(Employee e)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Employees (FullName, Position, Email)
                    VALUES ($n, $p, $e);
                    SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("$n", e.FullName);
                cmd.Parameters.AddWithValue("$p", e.Position);
                cmd.Parameters.AddWithValue("$e", e.Email);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public void Update(Employee e)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Employees
                                    SET FullName=$n, Position=$p, Email=$e
                                    WHERE Id=$id";
                cmd.Parameters.AddWithValue("$n", e.FullName);
                cmd.Parameters.AddWithValue("$p", e.Position);
                cmd.Parameters.AddWithValue("$e", e.Email);
                cmd.Parameters.AddWithValue("$id", e.Id);
                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(int id)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Employees WHERE Id=$id";
                cmd.Parameters.AddWithValue("$id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
    public class ProjectRepository
    {
        private readonly Database _db;
        public ProjectRepository(Database db)
        {
            _db = db;
        }
        public List<Project> GetAll()
        {
            var result = new List<Project>();
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name, Description, StartDate, EndDate FROM Projects ORDER BY Id";
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        result.Add(new Project
                        {
                            Id = r.GetInt32(0),
                            Name = r.GetString(1),
                            Description = r.IsDBNull(2) ? "" : r.GetString(2),
                            StartDate = DateTime.Parse(r.GetString(3)),
                            EndDate = r.IsDBNull(4) ? (DateTime?)null : DateTime.Parse(r.GetString(4))
                        });
                    }
                }
            }
            return result;
        }
        public int Add(Project p)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Projects (Name, Description, StartDate, EndDate)
                    VALUES ($n, $d, $s, $e);
                    SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("$n", p.Name);
                cmd.Parameters.AddWithValue("$d", p.Description);
                cmd.Parameters.AddWithValue("$s", p.StartDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("$e",
                    p.EndDate.HasValue ? (object)p.EndDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public void Update(Project p)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Projects
                                    SET Name=$n, Description=$d, StartDate=$s, EndDate=$e
                                    WHERE Id=$id";
                cmd.Parameters.AddWithValue("$n", p.Name);
                cmd.Parameters.AddWithValue("$d", p.Description);
                cmd.Parameters.AddWithValue("$s", p.StartDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("$e",
                    p.EndDate.HasValue ? (object)p.EndDate.Value.ToString("yyyy-MM-dd") : DBNull.Value);
                cmd.Parameters.AddWithValue("$id", p.Id);
                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(int id)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Projects WHERE Id=$id";
                cmd.Parameters.AddWithValue("$id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
    public class TaskRepository
    {
        private readonly Database _db;
        public TaskRepository(Database db)
        {
            _db = db;
        }
        private const string SelectSql = @"
            SELECT t.Id, t.ProjectId, t.EmployeeId, t.Title, t.Description,
                   t.Status, t.Progress, t.CreatedAt,
                   IFNULL(p.Name, ''), IFNULL(e.FullName, '')
            FROM Tasks t
            LEFT JOIN Projects  p ON p.Id = t.ProjectId
            LEFT JOIN Employees e ON e.Id = t.EmployeeId";
        public List<TaskItem> GetAll()
        {
            var result = new List<TaskItem>();
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = SelectSql + " ORDER BY t.Id";
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read()) result.Add(Map(r));
                }
            }
            return result;
        }
        public int Add(TaskItem t)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Tasks (ProjectId, EmployeeId, Title, Description, Status, Progress, CreatedAt)
                    VALUES ($pid, $eid, $title, $desc, $st, $pr, $cr);
                    SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("$pid", t.ProjectId);
                cmd.Parameters.AddWithValue("$eid",
                    t.EmployeeId.HasValue ? (object)t.EmployeeId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("$title", t.Title);
                cmd.Parameters.AddWithValue("$desc", t.Description);
                cmd.Parameters.AddWithValue("$st", (int)t.Status);
                cmd.Parameters.AddWithValue("$pr", t.Progress);
                cmd.Parameters.AddWithValue("$cr", t.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public void Update(TaskItem t)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"UPDATE Tasks
                                    SET ProjectId=$pid, EmployeeId=$eid, Title=$title,
                                        Description=$desc, Status=$st, Progress=$pr
                                    WHERE Id=$id";
                cmd.Parameters.AddWithValue("$pid", t.ProjectId);
                cmd.Parameters.AddWithValue("$eid",
                    t.EmployeeId.HasValue ? (object)t.EmployeeId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("$title", t.Title);
                cmd.Parameters.AddWithValue("$desc", t.Description);
                cmd.Parameters.AddWithValue("$st", (int)t.Status);
                cmd.Parameters.AddWithValue("$pr", t.Progress);
                cmd.Parameters.AddWithValue("$id", t.Id);
                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(int id)
        {
            using (var conn = _db.Open())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Tasks WHERE Id=$id";
                cmd.Parameters.AddWithValue("$id", id);
                cmd.ExecuteNonQuery();
            }
        }
        private static TaskItem Map(SqliteDataReader r)
        {
            return new TaskItem
            {
                Id = r.GetInt32(0),
                ProjectId = r.GetInt32(1),
                EmployeeId = r.IsDBNull(2) ? (int?)null : r.GetInt32(2),
                Title = r.GetString(3),
                Description = r.IsDBNull(4) ? "" : r.GetString(4),
                Status = (TaskStatus)r.GetInt32(5),
                Progress = r.GetInt32(6),
                CreatedAt = DateTime.Parse(r.GetString(7)),
                ProjectName = r.GetString(8),
                EmployeeName = r.GetString(9)
            };
        }
    }
}