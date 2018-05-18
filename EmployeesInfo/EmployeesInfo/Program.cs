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
			Console.WriteLine("Starting Program.");
			//Creating a file path to open the file.
			// I am setting the file path to be in the
			// Content folder.
			string fileName = "\\Content\\Employees.txt";
			string filePath = Directory.GetCurrentDirectory();
			int index = filePath.IndexOf("\\bin");
			int strLength = filePath.Substring(index).Length;
			string filePathSubStr = filePath.Substring(0, filePath.Length - strLength);
			string path = filePathSubStr + fileName;
			Dictionary<string, Employee> employeeDict = new Dictionary<string, Employee>();

			employeeDict = ReadInFile(path, employeeDict);
			
			//Creating a list from the dictionary and sorting it in descending order
			// so that we can use it in our functions.
			var list = employeeDict.ToList();
			list.Sort((pair1, pair2) => -1* pair1.Value.GrossPayAmount.CompareTo(pair2.Value.GrossPayAmount));

			//Running the three different Steps listed in the email.
			WriteStepOneToFile(filePathSubStr, list);
			WriteStepTwoToFile(filePathSubStr, list);
			WriteStepThreeToFile(filePathSubStr, list);

			Console.WriteLine("Finished Program.");
			//Keeping the console open to se the latest messages.
			Console.ReadLine();
		}

		public static void WriteStepOneToFile(string filePathSubStr, List<KeyValuePair<string, Employee>> list)
		{
			Console.WriteLine("Beginning Processing for Step One.");

			//Since we already sorted the passed in file we just create or
			// open the file to output the wanted information.
			using (var sw = new StreamWriter(filePathSubStr + "\\Step_01_Output.txt"))
			{
				for (int count = 0; count <list.Count; count++)
				{
					var currEmployee = list[count].Value;
					sw.WriteLine(string.Format("{0}, {1}, {2}, {3:N}, {4:N}, {5:N}, {6:N}",
						currEmployee.EmployeeId, currEmployee.FirstName,
						currEmployee.LastName, currEmployee.GrossPayAmount,
						currEmployee.FederalTaxAmount, currEmployee.StateTaxAmount,
						currEmployee.NetPayAmount));
				}
			}

			Console.WriteLine("Finished Processing for Step One.");
		}

		public static void WriteStepTwoToFile(string filePathSubStr, List<KeyValuePair<string, Employee>> list)
		{
			Console.WriteLine("Beginning Processing for Step Two.");
			//Getting 15% of the list.
			int count = Convert.ToInt32(list.Count * 0.15);

			//The list has already been ordered so we are getting
			// first 15% of the list.
			var subList = list.GetRange(0, count);

			//My attempt at ordering the top 15% of earners by 
			// years worked and then LastName.
			var newList = subList.OrderByDescending(pair1 => pair1.Value.YearsWorked())
				.ThenBy(pair1 => pair1.Value.LastName)
				.Select(pair1 => pair1.Value).ToList();

			using (var sw = new StreamWriter(filePathSubStr + "\\Step_02_Output.txt"))
			{
				for (int index = 0; index < newList.Count; index++)
				{
					var currEmployee = newList[index];
					sw.WriteLine(string.Format("{0}, {1}, {2}, {3:N}",
						currEmployee.FirstName,
						currEmployee.LastName,
						currEmployee.YearsWorked(),
						currEmployee.GrossPayAmount));
				}
			}
			Console.WriteLine("Finished Processing for Step Two.");
		}

		public static void WriteStepThreeToFile(string filePath, List<KeyValuePair<string, Employee>> list)
		{
			Console.WriteLine("Beginning Processing for Step Three.");
			//Sorts the list according to state.
			list.Sort((pair1, pair2) => pair1.Value.EmployeeState.CompareTo(pair2.Value.EmployeeState));

			List<string> resultList = new List<string>();
			for (int i = 0; i < 51; i++)
			{
				double sumHoursWorked = 0;
				double sumNetPay = 0;
				double sumStateTax = 0;

				//Creates a variable representing the current state.
				var currState = (States)i;

				//Parsing the list and returning only the values
				// that coorespond to my current state.
				var currStateList = list.Where(pair => pair.Value.EmployeeState == currState);
				foreach (var item in currStateList)
				{
					sumHoursWorked += item.Value.HoursWorked;
					sumNetPay += item.Value.NetPayAmount;
					sumStateTax += item.Value.StateTaxAmount;
				}

				//Calculate the averages.
				int stateCount = currStateList.Count();
				double averageHoursWorked = stateCount > 0 ? sumHoursWorked / stateCount : 0.0d;
				double averageNetPay = stateCount > 0 ? sumNetPay / stateCount : 0.0d;
				double averageStateTax = stateCount > 0 ? sumStateTax / stateCount : 0.0d;

				//Since I need to store the results of my averages somewhere
				// I decided ro just make them strings.
				resultList.Add(string.Format("{0}, {1:N}, {2:N}, {3:N}", currState,
					averageHoursWorked,
					averageNetPay,
					averageStateTax));
			}
			//Output the results to a file.
			using (var sw = new StreamWriter(filePath + "\\Step_03_Output.txt"))
			{
				for (int index = 0; index < resultList.Count; index++)
				{
					sw.WriteLine(resultList[index]);
				}
			}

			Console.WriteLine("Finished Processing for Step Three.");
		}

		public static Dictionary<string, Employee> ReadInFile(string fileName, Dictionary<string, Employee> employeeInfoDict)
		{
			Console.WriteLine("Staring to read in the file.");
			//Making sure that we start with a fresh Dictionary.
			employeeInfoDict.Clear();

			//Open the file for reading.
			using (var sr = new StreamReader(fileName))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					// Creates a new employeeInfo and fills it.
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

					//Adds the information to the Dictionary.
					//I assume that the EmployeeId is always unique.
					if (!employeeInfoDict.ContainsKey(employee.EmployeeId))
					{
						employeeInfoDict.Add(employee.EmployeeId, employee);
					}

				}
			}
			Console.WriteLine("Finished reading in file.");
			return employeeInfoDict;
		}
    }
}
