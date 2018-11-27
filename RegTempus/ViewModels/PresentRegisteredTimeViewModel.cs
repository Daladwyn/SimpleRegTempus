using RegTempus.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegTempus.ViewModels
{
    public struct PresentRegisteredTimeViewModel
    {

        //[MaxLength(10)]
        //public string Month { get; set; }

        [Range(1, 31)]
        public int Day { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeStop { get; set; }

        public TimeSpan TimeBreak { get; set; }

        public string TimeBreakString { get; set; }

        internal static List<PresentRegisteredTimeViewModel> CalculateTime(List<TimeMeasurement> MonthMesurement)
        {
            List<PresentRegisteredTimeViewModel> calculatedTimeList = new List<PresentRegisteredTimeViewModel>();
            PresentRegisteredTimeViewModel aDay = new PresentRegisteredTimeViewModel
            {
                TimeStart = MonthMesurement[0].TimeStart,
                TimeStop = MonthMesurement[0].TimeStop,
                Day = MonthMesurement[0].DayOfMonth,
                TimeBreak = TimeSpan.Zero,
                
            };

            for (int i = 1; i < MonthMesurement.Count(); i++)
            {
                if ((aDay.Day == MonthMesurement[i].DayOfMonth)&&( i<MonthMesurement.Count()+1) )
                {
                    if (aDay.TimeStop < MonthMesurement[i].TimeStart)
                    {
                        aDay.TimeBreak = aDay.TimeBreak + (MonthMesurement[i].TimeStart - aDay.TimeStop);
                        aDay.TimeBreakString = aDay.TimeBreak.ToString(@"hh\:mm\:ss");
                        aDay.TimeStop = MonthMesurement[i].TimeStop;
                    }
                }
                else
                {

                    calculatedTimeList.Add(aDay);
                    aDay.TimeStart = MonthMesurement[i].TimeStart;
                    aDay.TimeStop = MonthMesurement[i].TimeStop;
                    aDay.Day = MonthMesurement[i].DayOfMonth;
                    aDay.TimeBreak = TimeSpan.Zero;
                    aDay.TimeBreakString = aDay.TimeBreak.ToString(@"hh\:mm\:ss");
                    i--;
                }



            }
            calculatedTimeList.Add(aDay);
            return calculatedTimeList;
        }
    }
}
