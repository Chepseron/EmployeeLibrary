using System;
using System.Linq;
using Xunit;

namespace EmployeeLibrary
{
    public class EmployeeTest
    {
        [Fact]
        public void InvalidCSVListIsInput()
        {
            Assert.Throws<Exception>(() => new Employees(""));

        }

        [Fact]
        public void OneEmployessReportsToMoreThanoneManger()
        {
            Assert.Throws<Exception>(() => new Employees("John,manager1,90000" +
                "\n" +
                "Amon,manager1," + //error
                "150000\nKilali,manager2,90000" +
                "\n" +
                "Amon,manager3,150000" + //error
                "\nCEO,,100000 \n " +
                "manager1,CEO,150000" +
                "\n" +
                "manager3,CEO,150000\nsalasia,manager1,150000\nmanager2,CEO,150000"));

        }

        [Fact]
        public void TestExceptionisThrownWhenWehaveMoreThanOneCEO()
        {
            Assert.Throws<Exception>(() => new Employees("John,manager1,90000" +
                "\n" +
                "Amon,manager1," +
                "150000\nKilali,manager2,90000" +
                "\n" +
                "Faith,,150000" + // error
                "\nCEO,,100000 \n " + // error
                "manager1,CEO,150000" +
                "\n" +
                "manager3,CEO,150000\nsalasia,manager1,150000\nmanager2,CEO,150000"));

        }

        [Fact]
        public void CircularReference()
        {
            Assert.Throws<Exception>(() => new Employees("John,manager1,90000" +
                "\n" +
                "Amon,manager1," +
                "150000\nKilali,manager2,90000" +
                "\n" +
                "Faith,manager1,150000" +
                "\nCEO,,100000 \n " +
                "manager1,CEO,150000" +
                "\n" +
                "manager2,manager1,150000\n" + // error
    "salasia,manager1,150000\nmanager2,CEO,150000"));

        }

        [Fact]
        public void AllManagersNotListedInEmployessCell()
        {
            Assert.Throws<Exception>(() => new Employees("John,manager1,90000" +
                "\n" +
                "Amon,manager1," +
                "150000\nKilali,manager2,90000" +
                "\n" +
                "Faith,manager5,150000" + //error
                "\nCEO,,100000 \n " +
                "manager1,CEO,150000" +
                "\n" +
                "employess,manager1,150000\nsalasia,manager1,150000\nmanager2,CEO,150000"));

        }

        [Fact]
        public void ManagerBurgets()
        {

            Employees testEmployee = new Employees("John,manager1,90000" +
                "\n" +
                "Amon,manager1," +
                "150000\nKilali,manager2,90000" +
                "\n" +
                "Faith,manager1,150000" +
                "\nCEO,,100000 \n " +
                "manager1,CEO,150000" +
                "\n" +
                "employess,manager1,150000\nsalasia,manager1,150000\nmanager2,CEO,150000");

            Assert.Equal(300000, testEmployee.SalaryBudget("CEO"));

        }
    }
}
