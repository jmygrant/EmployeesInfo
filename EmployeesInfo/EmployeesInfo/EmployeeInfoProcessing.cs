﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace EmployeesInfo
{
    public class EmployeeInfoProcessing
    {
		private Dictionary<string, Employee> _employeeRecords;
		private List<KeyValuePair<string, Employee>> _employeeList;
		private string _filePath;
		public string FileName { get; set; }

		/// <summary>
		/// Constructor to make an instance of employee record.
		/// </summary>
		public EmployeeInfoProcessing()
		{
			_employeeRecords = new Dictionary<string, Employee>();
			_employeeList = new List<KeyValuePair<string, Employee>>();

			//Figuring out a path to look for files and output files.
			//I depend on files being put in Content folder that is
			// part of the solution.
			string filePath = Directory.GetCurrentDirectory();
			int index = filePath.IndexOf("\\bin");
			int strLength = filePath.Substring(index).Length;
			string filePathSubStr = filePath.Substring(0, filePath.Length - strLength);
			_filePath = filePathSubStr;

		}

		/// <summary>
		/// Used to find the information for Step One on the coding test.
		/// </summary>
		/// <param name="fileName">The name of the file where we will output the information.</param>
		public void WriteStepOneToFile(string fileName)
		{
			Console.WriteLine("Beginning Processing for Step One.");

			if (string.IsNullOrEmpty(fileName))
			{
				Console.WriteLine("Invalid file name. Please make sure the file name is correct.");
				return;
			}

			//Since we already sorted the list created from the dictionary
			// we just create or open the file to output the wanted information.
			using (var sw = new StreamWriter(_filePath + fileName))
			{
				for (int count = 0; count < _employeeList.Count; count++)
				{
					var currEmployee = _employeeList[count].Value;
					sw.WriteLine(string.Format("{0}, {1}, {2}, {3:N}, {4:N}, {5:N}, {6:N}",
						currEmployee.EmployeeId, currEmployee.FirstName,
						currEmployee.LastName, currEmployee.GrossPayAmount,
						currEmployee.FederalTaxAmount, currEmployee.StateTaxAmount,
						currEmployee.NetPayAmount));
				}
			}

			Console.WriteLine("Finished Processing for Step One.");
		}

		/// <summary>
		/// Used to find the information for Step Two on the coding test.
		/// </summary>
		/// <param name="fileName">The name of the file where we will output the information.</param>
		public void WriteStepTwoToFile(string fileName)
		{
			Console.WriteLine("Beginning Processing for Step Two.");

			if (string.IsNullOrEmpty(fileName))
			{
				Console.WriteLine("Invalid file name. Please make sure the file name is correct.");
				return;
			}

			//Getting 15% of the list.
			int count = Convert.ToInt32(_employeeList.Count * 0.15);

			//The list has already been ordered so we are getting
			// first 15% of the list.
			var subList = _employeeList.GetRange(0, count);

			//My attempt at ordering the top 15% of earners by 
			// years worked and then LastName.
			var newList = subList.OrderByDescending(pair1 => pair1.Value.YearsWorked())
				.ThenBy(pair1 => pair1.Value.LastName)
				.Select(pair1 => pair1.Value).ToList();

			using (var sw = new StreamWriter(_filePath + fileName))
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

		/// <summary>
		/// Used to find the information for Step Three on the coding test.
		/// </summary>
		/// <param name="fileName">The name of the file where we will output the information.</param>
		public void WriteStepThreeToFile(string fileName)
		{
			Console.WriteLine("Beginning Processing for Step Three.");

			if (string.IsNullOrEmpty(fileName))
			{
				Console.WriteLine("Invalid file name. Please make sure the file name is correct.");
				return;
			}

			var copyList = new List<KeyValuePair<string, Employee>>(_employeeList);
			//Sorts the list according to state.
			copyList.Sort((pair1, pair2) => pair1.Value.EmployeeState.CompareTo(pair2.Value.EmployeeState));

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
				var currStateList = copyList.Where(pair => pair.Value.EmployeeState == currState);
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
			using (var sw = new StreamWriter(_filePath + fileName))
			{
				for (int index = 0; index < resultList.Count; index++)
				{
					sw.WriteLine(resultList[index]);
				}
			}

			Console.WriteLine("Finished Processing for Step Three.");
		}

		/// <summary>
		/// Read in the information from the file. I do expect that the 
		/// property FileName has been filled. That is where the code will
		/// look for the information.
		/// </summary>
		public void ReadInFile()
		{
			Console.WriteLine("Staring to read in the file.");

			if (string.IsNullOrEmpty(FileName))
			{
				Console.WriteLine("Invalid FileName. The Property FileName must be set to run this function.");
				return;
			}

			//Making sure that we start with a fresh Dictionary.
			_employeeRecords.Clear();

			//Open the file for reading.
			using (var sr = new StreamReader(_filePath + FileName))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					// Creates a new employeeInfo and fills it.
					var employeeInfo = line.Split(',');
					if (employeeInfo.Length == 8)
					{
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
						if (!_employeeRecords.ContainsKey(employee.EmployeeId))
						{
							_employeeRecords.Add(employee.EmployeeId, employee);
						}
					}

				}
			}
			SortEmployeeList();
			Console.WriteLine("Finished reading in file.");
			
		}

		/// <summary>
		/// Sort the EmployeeList in Descending order.
		/// </summary>
		private void SortEmployeeList()
		{
			//Creating a list from the dictionary and sorting it in descending order
			// so that we can use it in our functions.
			var list = _employeeRecords.ToList();
			list.Sort((pair1, pair2) => -1 * pair1.Value.GrossPayAmount.CompareTo(pair2.Value.GrossPayAmount));
			_employeeList = list;
		}

		/// <summary>
		/// Lookup and return employee information by Id.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns>Return the employee at the passed Id. If the value is empty returns an empty employee.</returns>
		public Employee GetEmployeeById(string employeeId)
		{
			if(string.IsNullOrEmpty(employeeId))
			{
				return new Employee();
			}
			return _employeeRecords[employeeId];
		}
	}
}
