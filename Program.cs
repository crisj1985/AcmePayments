using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayWork
{
    class Program
    {
        public static string PathFile
        {
            get
            {
                return @"C:\Users\CRISTIAN\source\repos\PayWork\WorkEmployee.txt";
            }
        }
        public static int InitialHourWork { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["InitialHour"]);
        public static int FinalHourWork { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["FinalHour"]);
        public static int MountBetweenWeek1 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountBetweenWeek1"]);
        public static int MountBetweenWeek2 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountBetweenWeek2"]);
        public static int MountBetweenWeek3 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountBetweenWeek3"]);
        public static int MountWeekend1 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountWeekend1"]);
        public static int MountWeekend2 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountWeekend2"]);
        public static int MountWeekend3 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountWeekend3"]);
        static void Main(string[] args)
        {
            ReadFileWorkEmp(PathFile).ForEach(x => 
                                                {
                                                    int intMount = 0;
                                                    x.DaysWork.ForEach(y => 
                                                                         {
                                                                             intMount += Mount(y);
                                                                         });
                                                    Console.WriteLine($"The amount to pay {x.Name} is: {intMount} ");
                                                });
        }
        public static List<EmployeeWork> ReadFileWorkEmp(string strPathFile)
        {
            List<EmployeeWork> lsResult = new List<EmployeeWork>();
            EmployeeWork objEmpWork;
            DayWork objDayWork;
            List<DayWork> lsDayWork;

            try
            {
                IEnumerable<string> lines = File.ReadLines(strPathFile);
                lines.ToList().ForEach(x =>
                                       {
                                           x = x.Trim().Replace(" ", "");
                                           objEmpWork = new EmployeeWork();
                                           lsDayWork = new List<DayWork>();
                                           objEmpWork.Name = x.Substring(0, x.IndexOf('='));
                                           x.Substring(x.IndexOf('=') + 1)
                                           .Split(',')
                                           .ToList()
                                           .ForEach(y =>
                                                        {
                                                            y = y.Trim();
                                                            objDayWork = new DayWork();
                                                            objDayWork.Day = y.Substring(0, 2);
                                                            objDayWork.Hour = y.Substring(2);
                                                            lsDayWork.Add(objDayWork);
                                                    });
                                           objEmpWork.DaysWork = lsDayWork;
                                           lsResult.Add(objEmpWork);
                                       });

            }
            catch (Exception)
            {

                throw;
            }
            
            return lsResult;
        }
        public static int Mount(DayWork item)
        {
            int intResult = 0;
            int intInitialHour = Convert.ToInt32(item.Hour.Split('-')[0].Replace(":", ""))/100;
            int intFinalHour = Convert.ToInt32(item.Hour.Split('-')[1].Replace(":", ""))/100;
            int intHours = Math.Abs(intFinalHour - intInitialHour);
            int intLevelInitialHour = 0;
            int intLevelFinalHour = 0;


            if (intInitialHour <= InitialHourWork)
                intLevelInitialHour = 1;
            else if (intInitialHour > InitialHourWork && intInitialHour <= FinalHourWork)
                intLevelInitialHour = 2;
            else if (intInitialHour > FinalHourWork)
                intLevelInitialHour = 3;
            
            if (intFinalHour <= InitialHourWork)
                intLevelFinalHour = 1;
            else if (intFinalHour > InitialHourWork && intFinalHour <= FinalHourWork)
                intLevelFinalHour = 2;
            else if (intFinalHour > FinalHourWork)
                intLevelFinalHour = 3;

            switch (item.Day)
            {
                case "MO":
                case "TU":
                case "WE":
                case "TH":
                case "FR":
                    if (intLevelInitialHour == intLevelFinalHour)
                    {
                        if (intInitialHour <= InitialHourWork)
                            intResult = intHours * MountBetweenWeek1;
                        else if (intInitialHour > InitialHourWork && intInitialHour <= FinalHourWork)
                            intResult = intHours * MountBetweenWeek2;
                        else if (intInitialHour > FinalHourWork)
                            intResult = intHours * MountBetweenWeek3;
                    }
                    else 
                    { 
                        if (intLevelInitialHour == 1 && intLevelFinalHour == 2)
                        {
                            intResult = (InitialHourWork - intInitialHour) * MountBetweenWeek1 + (intFinalHour - InitialHourWork) * MountBetweenWeek2;
                        }
                        else if (intLevelInitialHour == 2 && intLevelFinalHour == 3)
                        {
                            intResult = (FinalHourWork - intInitialHour) * MountBetweenWeek2 + (intFinalHour - FinalHourWork) * MountBetweenWeek3;
                        }
                    }
                    break;
                case "SA":
                case "SU":
                    if (intLevelInitialHour == intLevelFinalHour)
                    {
                        if (intInitialHour <= InitialHourWork)
                            intResult = intHours * MountWeekend1;
                        else if (intInitialHour > InitialHourWork && intInitialHour <= FinalHourWork)
                            intResult = intHours * MountWeekend2;
                        else if (intInitialHour > FinalHourWork)
                            intResult = intHours * MountWeekend3;
                    }
                    else
                    {
                        if (intLevelInitialHour == 1 && intLevelFinalHour == 2)
                        {
                            intResult = (InitialHourWork - intInitialHour) * MountWeekend1 + (intFinalHour - InitialHourWork) * MountWeekend2;
                        }
                        else if (intLevelInitialHour == 2 && intLevelFinalHour == 3)
                        {
                            intResult = (FinalHourWork - intInitialHour) * MountWeekend2 + (intFinalHour - FinalHourWork) * MountWeekend3;
                        }
                    }
                    break;

            }

            return intResult;
        }

    }
    enum Days
    {
        MO = 1,
        TU,
        WE,
        TH,
        FR,
        SA,
        SU
    }
}
