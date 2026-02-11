using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using JSON_FILE_DATA.Models;
// my name is kishore 
[ApiController]
[Route("api/[controller]")]
public class Json_Data_Controller : ControllerBase
{
    private readonly IConfiguration _config;

    public Json_Data_Controller(IConfiguration config)
    {
        _config = config;
    }
    // i made changes on sprint_154preqa
    // done with code

    [HttpGet]
    public IActionResult GetEmployees()
    {
        var employees = new List<Json_Data>();
        string connStr = _config.GetConnectionString("DefaultConnection");

        using var conn = new SqlConnection(connStr);
        conn.Open();
        using var cmd = new SqlCommand("SELECT Id, Name, Department FROM Employees", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            employees.Add(new Json_Data
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Department = reader.GetString(2)
            });
        }
        return Ok(employees);
    }

    [HttpPost]
    public IActionResult AddJsonData([FromBody] Json_Data json_Data)
    {
        string connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new SqlConnection(connStr);
        conn.Open();

        using var cmd = new SqlCommand(
            "INSERT INTO Employees (Name, Department) VALUES (@Name, @Department)", conn);

        // Use the variable json_Data (lowercase), not the class name
        cmd.Parameters.AddWithValue("@Name", json_Data.Name);
        cmd.Parameters.AddWithValue("@Department", json_Data.Department);
        cmd.ExecuteNonQuery();

        return Ok(json_Data);
    }
}
