﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesInfo
{
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

		private void CalculateGrossPay()
		{
			int payAmount = 0;
			if(EmployeePayType == PayType.H)
			{
				payAmount = Convert.ToInt32(PayRate * 100) * Convert.ToInt32(HoursWorked * 100);
				_grossPayAmount = payAmount / 100;
			}
			else
			{
				payAmount = Convert.ToInt32((PayRate / 26.0) * 100);
				_grossPayAmount = payAmount / 100;
			}
			CalculateFederalTaxAmount();
			CalculateStateTaxAmount();
			CalculateNetPayAmount();
		}

		private void CalculateFederalTaxAmount()
		{
			int taxAmount = 0;
			taxAmount = Convert.ToInt32(GrossPayAmount * _federalTaxRate);
			_federalTaxAmount = Convert.ToDouble(taxAmount / 100);
		}

		private void CalculateStateTaxAmount()
		{
			int stateTaxAmount = 0;
			double stateTaxRate = 0.0;

			switch(EmployeeState)
			{
				case States.NV:
				case States.UT:
				case States.WY:
					{
						StateTaxRate = 0.05;
						break;
					}
				case States.AR:
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
			_stateTaxAmount = Convert.ToDouble(stateTaxAmount / 100);

		}

		private void CalculateNetPayAmount()
		{
			int netPayAmount = Convert.ToInt32(GrossPayAmount * 100) - Convert.ToInt32(FederalTaxAmount * 100)
				- Convert.ToInt32(StateTaxAmount * 100);

			_netPayAmount = Convert.ToDouble(netPayAmount / 100.00);
		}

		public Employee(string employeeId, string firstName, string lastName, PayType payType,
			double payRate, string startDate, States state, double timeWorked)
		{
			EmployeeId = employeeId;
			FirstName = firstName;
			LastName = lastName;
			EmployeePayType = payType;
			PayRate = payRate;

			var startDateArr = startDate.Split('/');
			int day = Convert.ToInt32(startDateArr[0]);
			int month = Convert.ToInt32(startDateArr[1]);
			int year = Convert.ToInt32(startDateArr[2]);
			if(year > 70 && year <=99)
			{
				year += 1900;
			}
			else if(year>= 00 && year <19)
			{
				year += 2000;
			}
			StartDate = new DateTime(year, month, day);
			EmployeeState = state;
			HoursWorked = timeWorked;
		}
    }
}