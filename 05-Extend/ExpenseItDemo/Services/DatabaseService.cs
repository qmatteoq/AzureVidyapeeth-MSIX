using ExpenseItDemo.Data.Model;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ExpenseItDemo.Data.Services
{
    public class DatabaseService
    {
        private SqlConnection connection;
        public DatabaseService()
        {          
            connection = new SqlConnection();
            connection.ConnectionString = $"Server=tcp:wpc2017.database.windows.net,1433;Initial Catalog=wpc2017;Persist Security Info=False;User ID=qmatteoq;Password=wpc2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public List<Employee> GetEmployees()
        {
            SqlCommand command = new SqlCommand();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            List<Employee> employees = new List<Employee>();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM Employees";
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        EmployeeId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Type = reader.GetString(3),
                        Email = reader.GetString(4)
                    };

                    employees.Add(employee);
                }
            }

            return employees;
        }

        public Employee GetEmployeeById(int employeeId)
        {
            SqlCommand command = new SqlCommand();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            Employee employee = null;

            command.Connection = connection;
            command.CommandText = "SELECT * FROM Employees WHERE Id=@EmployeeId";
            command.Parameters.AddWithValue("EmployeeId", employeeId);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employee = new Employee
                    {
                        EmployeeId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Type = reader.GetString(3),
                        Email = reader.GetString(4),
                        CostCenterId = reader.GetInt32(5)
                    };
                }
            }

            return employee;
        }

        public List<Employee> GetEmployeesByType(string type)
        {
            SqlCommand command = new SqlCommand();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            
            List<Employee> employees = new List<Employee>();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM Employees WHERE Type=@Type";
            command.Parameters.AddWithValue("Type", type);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        EmployeeId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Type = reader.GetString(3),
                        Email = reader.GetString(4),
                        CostCenterId = reader.GetInt32(5)
                    };

                    employees.Add(employee);
                }
            }

            return employees;
        }

        public List<Expense> GetExpensesByEmployee(int employeeId)
        {
            SqlCommand command = new SqlCommand();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            List<Expense> expenses = new List<Expense>();

            command.Connection = connection;
            command.CommandText = "SELECT * FROM Expenses WHERE EmployeeId=@EmployeeId";
            command.Parameters.AddWithValue("EmployeeId", employeeId);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Expense expense = new Expense();
                    expense.ExpenseId = reader.GetInt32(0);
                    expense.Type = reader.GetString(1);
                    expense.Description = reader.GetString(2);
                    expense.Cost = (double)reader.GetDecimal(3);
                    expense.EmployeeId = employeeId;

                    expenses.Add(expense);
                }
            }

            return expenses;
        }
    }
}
