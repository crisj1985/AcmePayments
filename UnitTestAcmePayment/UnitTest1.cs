using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayWork;

namespace UnitTestAcmePayment
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadFileWorkEmp_FileExist()
        {
            string Path = @"C:\Users\CRISTIAN\source\repos\PayWork\WorkEmployee.txt";

            List<EmployeeWork> expected = new List<EmployeeWork>();
            List<DayWork> DwExpected = new List<DayWork>();
            DwExpected.Add(new DayWork { Day = "MO", Hour = "10:00-12:00" });

            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "RENE" });
            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "ASTRID" });
            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "CRISTIAN" });

            List<EmployeeWork> actual = Program.ReadFileWorkEmp(Path);

            Assert.AreEqual(expected.Count, actual.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ReadFileWorkEmp_FileNotFound()
        {
            string Path = @"C:\Users\CRISTIAN\source\repos\PayWork\WorkEmploye.txt";

            List<EmployeeWork> expected = new List<EmployeeWork>();
            List<DayWork> DwExpected = new List<DayWork>();
            DwExpected.Add(new DayWork { Day = "MO", Hour = "10:00-12:00" });

            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "RENE" });
            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "ASTRID" });
            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "CRISTIAN" });

            List<EmployeeWork> actual = Program.ReadFileWorkEmp(Path);

            Assert.AreEqual(expected.Count, actual.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFileWorkEmp_SeparatorNameDifferentInFile()
        {
            string Path = @"C:\Users\CRISTIAN\source\repos\PayWork\WorkEmployeeError.txt";

            List<EmployeeWork> expected = new List<EmployeeWork>();
            List<DayWork> DwExpected = new List<DayWork>();
            DwExpected.Add(new DayWork { Day = "MO", Hour = "10:00-12:00" });

            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "RENE" });
            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "ASTRID" });
            expected.Add(new EmployeeWork { DaysWork = DwExpected, Name = "CRISTIAN" });

            List<EmployeeWork> actual = Program.ReadFileWorkEmp(Path);

            Assert.AreEqual(expected.Count, actual.Count);
        }
        [TestMethod]
        public void Amount_Successful()
        {
            DayWork objTest = new DayWork() { Day = "MO", Hour = "10:00-12:00" };
            int a = Program.AmountBetweenWeek1;
            int expected = 30;
            int actual = Program.Amount(objTest);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        [ExpectedException(typeof(DayNoExistException))]
        public void Amount_DayNoExist()
        {
            DayWork objTest = new DayWork() { Day = "MP", Hour = "10:00-12:00" };
            int a = Program.AmountBetweenWeek1;
            int expected = 30;
            int actual = Program.Amount(objTest);

            Assert.AreEqual(expected, actual);
        }
    }
}
