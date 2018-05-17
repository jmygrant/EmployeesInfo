using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace EmployeesInfo
{
    class Program
    {
        static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			//Read In File
			string fileName = "\\Content\\Employees.txt";
			string filePath = Directory.GetCurrentDirectory();
			int index = filePath.IndexOf("\\bin");
			int strLength = filePath.Substring(index).Length;
			string filePathSubStr = filePath.Substring(0, filePath.Length - strLength);
			string path = filePathSubStr + fileName;
			Dictionary<string, Employee> employeeDict = new Dictionary<string, Employee>();

			employeeDict = ReadInFile(path, employeeDict);
			WriteStepOneToFile(filePathSubStr, employeeDict);

			Console.ReadLine();
		}

		public static void WriteStepOneToFile(string filePathSubStr, Dictionary<string, Employee> employeeDict)
		{
			var list = employeeDict.ToList();
			list.Sort((pair1, pair2) => pair1.Value.GrossPayAmount.CompareTo(pair2.Value.GrossPayAmount));

			using (var sw = new StreamWriter(filePathSubStr + "\\Step_01_Output.txt"))
			{
				for (int count = list.Count-1; count > -1; count--)
				{
					var currEmployee = list[count].Value;
					sw.WriteLine(string.Format("{0}, {1}, {2}, {3:N}, {4:N}, {5:N}, {6:N}",
						currEmployee.EmployeeId, currEmployee.FirstName,
						currEmployee.LastName, currEmployee.GrossPayAmount,
						currEmployee.FederalTaxAmount, currEmployee.StateTaxAmount,
						currEmployee.NetPayAmount));
				}
			}
		}

		public static Dictionary<string, Employee> ReadInFile(string fileName, Dictionary<string, Employee> employeeInfoDict)
		{
			employeeInfoDict.Clear();
			using (var sr = new StreamReader(fileName))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					var employeeInfo = line.Split(',');
					Employee employee = new Employee();
					employee.EmployeeId = employeeInfo[0];
					employee.FirstName = employeeInfo[1];
					employee.LastName = employeeInfo[2];
					PayType payType;
					Enum.TryParse(employeeInfo[3], out payType);
					employee.EmployeePayType = payType;
					employee.PayRate = Convert.ToDouble(employeeInfo[4]);
					var startDate = employeeInfo[5].Split('/');
					employee.StartDate = employee.ConvertSeperatedDateToDateTime(startDate);
					States state;
					Enum.TryParse(employeeInfo[6], out state);
					employee.EmployeeState = state;
					employee.HoursWorked = Convert.ToDouble(employeeInfo[7]);
					employee.CalculatePay();

					if (!employeeInfoDict.ContainsKey(employee.EmployeeId))
					{
						employeeInfoDict.Add(employee.EmployeeId, employee);
					}

				}
			}

			return employeeInfoDict;
		}
    }
}
