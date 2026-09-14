using Dapper;
using Microsoft.Data.SqlClient;
using dz3baza.Models;

string connectionString = "Server=.\\SQLEXPRESS;Database=EFCore1;Trusted_Connection=True;TrustServerCertificate=True;";

using var context = new SqlConnection(connectionString);


//1
//var sql = $@"
//    SELECT COUNT(*) FROM Employees";

//int employeeCount = await context.ExecuteScalarAsync<int>(sql);

//Console.WriteLine($"Employee count: {employeeCount}");


//2
//void GetEmployeeById(int id)
//{
//    var sql = $@"
//        SELECT * FROM Employees WHERE Id = @Id";

//    var parameters = new { Id = id };
//    var employee = context.QueryFirstOrDefault<Employee>(sql, parameters);
//    if (employee != null)
//    {
//        Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}, Salary: {employee.Salary}, Age: {employee.Age}");
//    }
//    else
//    {
//        Console.WriteLine($"Employee with ID {id} not found.");
//    }
//}


//3
//void GetEmployeesByDepartment(int departmentId)
//{
//    var sql = $@"
//        SELECT * FROM Employees WHERE DepartmentId = @DepartmentId";

//    var parameters = new { DepartmentId = departmentId };
//    var employees = context.Query<Employee>(sql, parameters);
//    foreach (var employee in employees)
//    {
//        Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}, Salary: {employee.Salary}, Age: {employee.Age}");
//    }
//}
//GetEmployeesByDepartment(1);


//4

//void GetEmployeesWithDepartments()
//{
//    var sql = $@"SELECT * FROM EMPLOYEES E INNER JOIN DEPARTMENTS D ON E.DEPARTMENTID = D.ID";

//    var result =  context.Query<Employee,Department,Employee>(sql, (employee, department) =>
//    {
//        employee.DepartmentId = department.Id;
//        return employee;
//    }, splitOn: "Id");

//    foreach (var employee in result)
//    {
//        Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}, Salary: {employee.Salary}, Age: {employee.Age}, DepartmentId: {employee.DepartmentId}");
//    }
//}

//GetEmployeesWithDepartments();

//5


var sql = @"
SELECT TOP 4 * FROM Employees ORDER BY Id DESC;
SELECT* FROM Departments;
SELECT COUNT(*) FROM Employees;
SELECT AVG(Salary) FROM Employees; ";

using var multi = await context.QueryMultipleAsync(sql);

var lastEmployees = (await multi.ReadAsync<Employee>()).ToList();
var departments = (await multi.ReadAsync<Department>()).ToList();
var employeeCount = await multi.ReadFirstAsync<int>();
var averageSalary = await multi.ReadFirstAsync<decimal>();

Console.WriteLine("Последние сотрудники:");
foreach (var emp in lastEmployees)
{
    Console.WriteLine($"- {emp.FirstName} {emp.LastName}");
}

Console.WriteLine("\nОтделы:");
foreach (var dept in departments)
{
    Console.WriteLine($"- {dept.Name}");
}

Console.WriteLine($"Количество сотрудников: {employeeCount}");
Console.WriteLine($"Средняя зарплата: {averageSalary}");