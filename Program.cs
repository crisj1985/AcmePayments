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
        #region properties
        public static string PathFile { get; set; } = ConfigurationManager.AppSettings["AbsolutePathFile"];
        public static int InitialHourWork { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["InitialHour"]);
        public static int FinalHourWork { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["FinalHour"]);
        public static int MountBetweenWeek1 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountBetweenWeek1"]);
        public static int MountBetweenWeek2 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountBetweenWeek2"]);
        public static int MountBetweenWeek3 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountBetweenWeek3"]);
        public static int MountWeekend1 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountWeekend1"]);
        public static int MountWeekend2 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountWeekend2"]);
        public static int MountWeekend3 { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MountWeekend3"]);
        public static int LenghtAbbreviationDay { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["LenghtAbbreviationDay"]);
        public static char SeparatorName { get; set; } = Convert.ToChar(ConfigurationManager.AppSettings["SeparatorName"]);
        public static char SeparatorDays { get; set; } = Convert.ToChar(ConfigurationManager.AppSettings["SeparatorDays"]);
        #endregion properties

        static void Main(string[] args)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message} | {ex.StackTrace} ");
            }
        }
        public static List<EmployeeWork> ReadFileWorkEmp(string strPathFile)
        {
            List<EmployeeWork> lsResult = new List<EmployeeWork>();
            EmployeeWork objEmpWork= null;
            DayWork objDayWork = null;
            List<DayWork> lsDayWork = null;

            try
            {
                IEnumerable<string> lines = File.ReadLines(strPathFile);
                lines.ToList().ForEach(x =>
                                       {
                                           x = x.Trim().Replace(" ", string.Empty);
                                           objEmpWork = new EmployeeWork();
                                           lsDayWork = new List<DayWork>();
                                           objEmpWork.Name = x.Substring(0, x.IndexOf(SeparatorName));
                                           x.Substring(x.IndexOf(SeparatorName) + 1)
                                           .Split(SeparatorDays)
                                           .ToList()
                                           .ForEach(y =>
                                                        {
                                                            objDayWork = new DayWork();
                                                            objDayWork.Day = y.Substring(0, LenghtAbbreviationDay);
                                                            objDayWork.Hour = y.Substring(LenghtAbbreviationDay);
                                                            lsDayWork.Add(objDayWork);
                                                        });
                                           objEmpWork.DaysWork = lsDayWork;
                                           lsResult.Add(objEmpWork);
                                       });

            }
            catch (Exception)
            {
                throw new Exception("File structure error");
            }
            finally
            {
                if (objEmpWork != null)
                    objEmpWork = null;
                if (objDayWork != null)
                    objDayWork = null;
                if (lsDayWork != null)
                    lsDayWork = null;
            }
            
            return lsResult;
        }
        public static int Mount(DayWork item)
        {
            int intResult = 0;

            try
            {

                int intInitialHour = Convert.ToInt32(item.Hour.Split('-')[0].Replace(":", string.Empty))/100;
                int intFinalHour = Convert.ToInt32(item.Hour.Split('-')[1].Replace(":", string.Empty))/100;
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

                switch (item.Day.ToUpper())
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
                    default:
                        throw new Exception($"There is no day {item.Day}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Hour structure error or {ex?.Message}");
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
