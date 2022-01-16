using System;
using System.Collections;
using System.IO;

namespace EmployeeLibrary
{
    public class Employees
    {
        ArrayList empList;
        public Employees(string csv)
        {
            try
            {
                ArrayList data = new ArrayList();
                if (string.IsNullOrEmpty(csv) || !(csv is string))
                {
                    WriteLog("csv is null");

                }
                string[] datarows = csv.Split(
            new[] { Environment.NewLine },
            StringSplitOptions.None
            );

                foreach (string row in datarows)
                {
                    string[] availableData = row.Split(',');
                    ArrayList convertedData = new ArrayList();

                    foreach (string cell in data)
                    {
                        convertedData.Add(cell);
                    }
                    if (convertedData.Count != 3)
                    {
                        WriteLog("CSV value must have 3 columns in each row");
                    }
                    data.Add(convertedData);
                }
                //Proceed to validate the data i.e check for circular reference and the managers the employees report to
                ValidateSalaries(data);
                EmployeeReportsTo(data);
            }

            catch (Exception e)
            {
                throw e;
            }
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
                    WriteLog("Employees salaries must be an integer");
                }
            }
        }

        /*
         * Check if each employees reports to one manager;
         */

        public void EmployeeReportsTo(ArrayList employees)
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

                    WriteLog("Employee value is duplicated this may be as a result of an employee reporting to more than one manager");
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

                WriteLog("No Company CEO");
            }

            // check if all managers are employess
            foreach (string manager in managers)
            {
                if (!savedEmployees.Contains(manager.Trim()))
                {
                    WriteLog("Some managers do not exist in the list");
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

                        WriteLog("Circular reference");
                    }
                }
            }


        }

        /**
  *
  *Return salary budgets of a specified manager
  * **/

        public long SalaryBudget(string manager)
        {
            long totalManagerSalary = 0;
            foreach (ArrayList employee in empList)
            {
                var name = employee[1] as string;
                var employeeSalary = employee[2] as string;
                var employeName = employee[0] as string;
                if (name.Trim() == manager.Trim() || employeName.Trim() == manager.Trim())
                {
                    totalManagerSalary += Convert.ToInt32(employeeSalary);
                }
            }
            return totalManagerSalary;
        }




        public static void WriteLog(string content)
        {
            //this logs to the execution path of the application. in this case the debug folder under the application directory
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo fullDirPath = null;
            FileInfo fileInfo;

            string filePath = appPath + "Logs\\";
            filePath = filePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            fileInfo = new FileInfo(filePath);
            fullDirPath = new DirectoryInfo(fileInfo.DirectoryName);
            if (!fullDirPath.Exists) fullDirPath.Create();
            if (!fileInfo.Exists)
            {
                fileStream = fileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(filePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(content);
            log.Close();
        }

    }
}
