using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesInfo
{
	//Created an enum for the PayType just
	// to make sure that we use valid values.
	public enum PayType
	{
		H,
		S
	}

	//Enum of United States Values listed in 
	// alphabetical order by the state.
	public enum States
	{
		AL /*Alabama*/,
		AK /*Alaska*/,
		AZ /*Arizona*/,
		AR /*Arkansas*/,
		CA /*California*/,
		CO /*Colorado*/,
		CT /*Connecticut*/,
		DE /*Delaware*/,
		DC /*District of Columbia*/,
		FL /*Florida*/,
		GA /*Georgia*/,
		HI /*Hawaii*/,
		ID /*Idaho*/,
		IL /*Illinois*/,
		IN /*Indiana*/,
		IA /*Iowa*/,
		KS /*Kansas*/,
		KY /*Kentucky*/,
		LA /*Louisiana*/,
		ME /*Maine*/,
		MD /*Maryland*/,
		MA /*Massachusetts*/,
		MI /*Michigan*/,
		MN /*Minnesota*/,
		MS /*Mississippi*/,
		MO /*Missouri*/,
		MT /*Montana*/,
		NE /*Nebraska*/,
		NV /*Nevada*/,
		NH /*New Hampshire*/,
		NJ /*New Jersey*/,
		NM /*New Mexico*/,
		NY /*New York*/,
		NC /*North Carolina*/,
		ND /*North Dakota*/,
		OH /*Ohio*/,
		OK /*Oklahoma*/,
		OR /*Oregon*/,
		PA /*Pennsylvania*/,
		RI /*Rhode Island*/,
		SC /*South Carolina*/,
		SD /*South Dakota*/,
		TN /*Tennessee*/,
		TX /*Texas*/,
		UT /*Utah*/,
		VT /*Vermont*/,
		VA /*Virginia*/,
		WA /*Washington*/,
		WV /*West Virginia*/,
		WI /*Wisconsin*/,
		WY /*Wyoming*/
	}

    public class Employee
    {
		public string EmployeeId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public PayType EmployeePayType { get; set; }
		public double PayRate { get; set; }
		public DateTime StartDate { get; set; }
		public double HoursWorked { get; set; }
		public States EmployeeState { get; set; }
		public double StateTaxRate { get; set; }

		private double _stateTaxAmount;
		public double StateTaxAmount
		{
			get
			{
				return _stateTaxAmount;
			}
		}

		private double _federalTaxAmount;
		public double FederalTaxAmount
		{
			get
			{
				return _federalTaxAmount;
			}
		}

		private double _grossPayAmount;
		public double GrossPayAmount
		{
			get
			{
				return _grossPayAmount;
			}
		}

		private double _netPayAmount;
		public double NetPayAmount
		{
			get
			{
				return _netPayAmount;
			}
		}

		private double _federalTaxRate = 0.15;

		/// <summary>
		/// Used to run the calculations for
		/// GrossPayAmount, FederalPayAmount,
		/// StatePayAmount, and NetPayAmount.
		/// </summary>
		public void CalculatePay()
		{
			if(PayRate > 0 && HoursWorked > 0)
			{
				CalculateGrossPay();
			}
		}

		/// <summary>
		/// Calculate the Number of Years worked.
		/// </summary>
		/// <returns></returns>
		public int YearsWorked()
		{
			if(StartDate.Year != 0)
			{
				return DateTime.Now.Year - StartDate.Year;
			}
			return 0;
		}

		/// <summary>
		/// Calclates the GrossPayAmount and calls the 
		/// other calculate functions to set all values.
		/// </summary>
		private void CalculateGrossPay()
		{
			double payAmount = 0;
			if(EmployeePayType == PayType.H)
			{
				if (HoursWorked > 80.0)
				{
					double overtime = 80.0 - HoursWorked;

					double regularPay = PayRate * 80.0;

					//Figuring out if overtime is less than 10 hours over.
					if (overtime < 10.0)
					{
						//Calculate overtime.
						double overtimePayRate = PayRate * .5 + PayRate;
						double overtimeAmount = overtimePayRate * overtime;
						_grossPayAmount = regularPay + overtimeAmount;
					}
					else
					{
						//Calculate overtime and double overtime.
						double singleOvertimeRate = PayRate * .5 + PayRate;
						double doubleOvertimeRate = PayRate * .75 + PayRate;

						double singleOvertimeAmount = singleOvertimeRate * 10;
						double doubleOvertimeAmount = doubleOvertimeRate * (overtime - 10);
						_grossPayAmount = regularPay + singleOvertimeAmount + doubleOvertimeAmount;
					}

				}
				else
				{
					//Calculate PayRate for hourly employees
					payAmount = PayRate * HoursWorked;
					_grossPayAmount = payAmount;
				}
			}
			else
			{
				//Calculate the payRate for Salary Employees.
				payAmount = PayRate / 26.0;
				_grossPayAmount = payAmount;
			}

			//Run the other functions to calculate taxes and net pay.
			CalculateFederalTaxAmount();
			CalculateStateTaxAmount();
			CalculateNetPayAmount();
		}

		/// <summary>
		/// Caluclate the federal tax amount. This depends on
		/// GrossPayAmount being set properly.
		/// </summary>
		private void CalculateFederalTaxAmount()
		{
			int taxAmount = 0;
			taxAmount = Convert.ToInt32(GrossPayAmount * _federalTaxRate);
			_federalTaxAmount = Convert.ToDouble(taxAmount);
		}

		/// <summary>
		/// Calculate the state tax amount. This depends on
		/// GrossPayAmount being set properly.
		/// </summary>
		private void CalculateStateTaxAmount()
		{
			int stateTaxAmount = 0;

			//The only states that I have tax infomation for are listed below.
			// I have set the value to zero for any state not listed below.
			switch(EmployeeState)
			{
				case States.NV:
				case States.UT:
				case States.WY:
					{
						StateTaxRate = 0.05;
						break;
					}
				case States.AZ:
				case States.CO:
				case States.ID:
				case States.OR:
					{
						StateTaxRate = 0.065;
						break;
					}
				case States.NM:
				case States.TX:
				case States.WA:
					{
						StateTaxRate = 0.07;
						break;
					}
			}

			stateTaxAmount = Convert.ToInt32(GrossPayAmount * StateTaxRate);
			_stateTaxAmount = Convert.ToDouble(stateTaxAmount);

		}

		/// <summary>
		/// Calculate the net pay amount. This depends on
		/// GrossPayAmount being set properly.
		/// </summary>
		private void CalculateNetPayAmount()
		{
			int netPayAmount = Convert.ToInt32(GrossPayAmount * 100) - Convert.ToInt32(FederalTaxAmount * 100)
				- Convert.ToInt32(StateTaxAmount * 100);

			_netPayAmount = Convert.ToDouble(netPayAmount / 100.00);
		}

		/// <summary>
		/// Default constructor to create an employee.
		/// </summary>
		public Employee()
		{
			EmployeeId = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
			EmployeePayType = PayType.H;
			PayRate = 0.00d;
			StartDate = new DateTime();
			EmployeeState = States.AL;
			HoursWorked = 0.00d;
		}

		/// <summary>
		/// Creates an Employee from passed in information.
		/// </summary>
		/// <param name="employeeId">Employee Id</param>
		/// <param name="firstName">Employee First Name</param>
		/// <param name="lastName">Employee Last Name</param>
		/// <param name="payType">Employee Pay Type</param>
		/// <param name="payRate">Employee Pay Rate</param>
		/// <param name="startDate">Employee Start Date</param>
		/// <param name="state">Employee State</param>
		/// <param name="hoursWorked">Employee Hours Worked</param>
		public Employee(string employeeId, string firstName, string lastName, PayType payType,
			double payRate, string startDate, States state, double hoursWorked)
		{
			EmployeeId = employeeId;
			FirstName = firstName;
			LastName = lastName;
			EmployeePayType = payType;
			PayRate = payRate;

			var startDateArr = startDate.Split('/');
			DateTime dateTime = ConvertSeperatedDateToDateTime(startDateArr);
			StartDate = dateTime;
			EmployeeState = state;
			HoursWorked = hoursWorked;
		}
	
		/// <summary>
		/// Convert the input date to a form that can be used with the built in DateTime.
		/// </summary>
		/// <param name="startDateArr">Takes the date in the form MM/DD/YY.</param>
		/// <returns>DateTime set to the passed in date.</returns>
		public DateTime ConvertSeperatedDateToDateTime(string[] startDateArr)
		{
			int month = Convert.ToInt32(startDateArr[0]);
			int day = Convert.ToInt32(startDateArr[1]);
			int year = Convert.ToInt32(startDateArr[2]);
			if (year > 70 && year <= 99)
			{
				year += 1900;
			}
			else if (year >= 00 && year < 19)
			{
				year += 2000;
			}
			return new DateTime(year, month, day);
		}
	}
}
