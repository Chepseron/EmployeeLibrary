using System;
using System.Collections;

namespace EmployeeLibrary
{
    public class Employees
    {
        ArrayList empList;
        public Employees(string csv)
        {
            try
            {
                empList = filterCSVToArray(csv);
                ValidateSalaries(empList);
                ValidateEmployReportTo(empList);
            }

            catch (Exception e)
            {
                throw e;
            }
        }


        /* This method returns csv array list converted from string
         */

        public ArrayList filterCSVToArray(string csv)
        {
            ArrayList cleanedData = new ArrayList();
            if (string.IsNullOrEmpty(csv) || !(csv is string))
            {
                throw new Exception("csv cannot be null");

            }

            /**
             *In CSV (Comma Separated Values), each line corresponds to a row, and columns are separated by a comma.
             * We first divide data into rows so that we may and save the individual row as an array.
             */

            string[] datarows = csv.Split(
            new[] { Environment.NewLine },
            StringSplitOptions.None
            );


            /**
             * After we have the rows in an array as words separated by commas, we split the words rows into arrays.
             * The purpose of doing this is to be able to validate each data separately.
             **/

            foreach (string row in datarows)
            {
                string[] data = row.Split(',');
                ArrayList filteredData = new ArrayList();

                foreach (string cell in data)
                {
                    filteredData.Add(cell);
                }
                if (filteredData.Count != 3)
                {
                    throw new Exception("CSV value must have 3 values in each row");
                }
                cleanedData.Add(filteredData);
            }

            return cleanedData;
        }

        /*
         *Validates if employees salaries are valid
         */

        public void ValidateSalaries(ArrayList employeesList)
        {
            foreach (ArrayList employee in employeesList)
            {
                string employeeSalary = Convert.ToString(employee[2]);
                int number;
                if (!(Int32.TryParse(employeeSalary, out number)))
                {
                    throw new Exception("Employees salaries must be a valid integer");
                }
            }
        }

        /*
         * Check if each employees reports to one manager;
         */

        public void ValidateEmployReportTo(ArrayList employees)
        {
            ArrayList savedEmployees = new ArrayList();
            ArrayList managers = new ArrayList(); //List of managers
            ArrayList ceos = new ArrayList();
            ArrayList juniorEmployess = new ArrayList();

            foreach (ArrayList employee in employees)
            {
                string employeename = employee[0] as string;
                string managername = employee[1] as string;

                if (savedEmployees.Contains(employeename.Trim()))
                {
                    throw new Exception("Employee value is duplicated this may be as a result of an employee reporting to more than one manager");
                }

                savedEmployees.Add(employeename.Trim());

                if (!string.IsNullOrEmpty(managername.Trim()))
                {
                    managers.Add(managername.Trim());
                }
                else
                {
                    ceos.Add(employeename.Trim());
                }

            }

            // check if we only have one CEO
            int managersDiff = employees.Count - managers.Count;
            if (managersDiff != 1)
            {
                throw new Exception("We can't determine who is the CEO kindly look through your data");
            }

            // check if all managers are employess
            foreach (string manager in managers)
            {
                if (!savedEmployees.Contains(manager.Trim()))
                {
                    throw new Exception("The list is incomplete it seems there are some managers who aren't listed in employess cell");
                }
            }


            // Add a junior employees
            foreach (string employee in savedEmployees)
            {
                if (!managers.Contains(employee) && !ceos.Contains(employee))
                {
                    juniorEmployess.Add(employee.Trim());
                }
            }

            ////// check for circular reference
            for (var i = 0; i < employees.Count; i++)
            {
                var employeeData = employees[i] as ArrayList;
                var employeeManager = employeeData[1] as string;
                int index = savedEmployees.IndexOf(employeeManager);

                if (index != -1)
                {
                    var managerData = employees[index] as ArrayList;
                    var topManager = managerData[1] as string;

                    if ((managers.Contains(topManager.Trim()) && !ceos.Contains(topManager.Trim()))
                        || juniorEmployess.Contains(topManager.Trim()))
                    {
                        throw new Exception("Circular reference error");
                    }
                }
            }


        }

        /**
  *
  *Return salary budgets of a specified manager
  * **/

        public long managerSalaryBudget(string manageName)
        {
            long  totalManagerSalary = 0;
            foreach (ArrayList employee in empList)
            {
                var name = employee[1] as string;
                var employeeSalary = employee[2] as string;
                var employeName = employee[0] as string;
                if (name.Trim() == manageName.Trim() || employeName.Trim() == manageName.Trim())
                {
                    totalManagerSalary += Convert.ToInt32(employeeSalary);
                }
            }
            return totalManagerSalary;
        }

    }
}
