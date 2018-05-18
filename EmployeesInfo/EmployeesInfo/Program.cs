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

			//Using the EmployeeRecords to run the processing that is needed.
			var employeeRecords = new EmployeeInfoProcessing();
			employeeRecords.FileName = "\\Content\\Employees.txt";

			//Read In File.
			employeeRecords.ReadInFile();

			//Running the three different Steps listed in the email.
			employeeRecords.WriteStepOneToFile("\\Step_01_Output.txt");
			employeeRecords.WriteStepTwoToFile("\\Step_02_Output.txt");
			employeeRecords.WriteStepThreeToFile("\\Step_03_Output.txt");

			Console.WriteLine("Finished Program.");
			//Keeping the console open to see the messages.
			Console.ReadLine();
		}
    }
}
