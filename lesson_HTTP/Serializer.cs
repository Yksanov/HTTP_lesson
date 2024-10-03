using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace lesson_HTTP;

public class Serializer : IEnumerable<Employee>
{

    public static List<Employee> _Employees = new List<Employee>();
    private static string path = "../../../Employees.json";

    public static JsonSerializerOptions op = new JsonSerializerOptions()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    public static void SaveEmployee() => File.WriteAllText(path, JsonSerializer.Serialize(_Employees, op));

    public static List<Employee> GetEmployees()
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "[]");
            Console.WriteLine("------------- В файле ничего нету -----------");
            SaveEmployee();
        }
        else
        {
            _Employees = JsonSerializer.Deserialize<List<Employee>>(File.ReadAllText(path));
            if (_Employees.Count == 0)
            {
                Console.WriteLine("---------- В файле ничего нету ---------");
                SaveEmployee();
            }
        }
        return _Employees;
    }
    
    public IEnumerator<Employee> GetEnumerator()
    {
        for (int i = 0; i < _Employees.Count; i++)
        {
            yield return _Employees[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}