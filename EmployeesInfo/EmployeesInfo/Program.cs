using System;

namespace EmployeesInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

			Employee employee = new Employee();
			employee.FirstName = "Jared";
			employee.LastName = "Mygrant";
			employee.EmployeeId = "AA";
			int Month = 1;
			int Day = 27;
			int year = 87;
			employee.StartDate = new DateTime();
        }
    }
}
