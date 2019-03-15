using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RegTempus.ViewModels;

namespace RegTempus.Models
{
    public class TimeMeasurement
    {
        public int TimeMeasurementId { get; set; }

        [Required]
        public int RegistratorId { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeStop { get; set; }

        public TimeSpan TimeRegistered { get; set; }

        public string TimeType { get; set; }

        //[Range(1,7)]
        //public int DayInWeek { get; set; }

        [Range(1, 31)]
        public int DayOfMonth { get; set; }

        [Range(1, 12)]
        public int MonthOfYear { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        /// <summary>
        /// The basic function of starting the clock.
        /// </summary>
        /// <param name="registrator"></param>
        /// <returns></returns>
        public static TimeMeasurement startClock(Registrator registrator)
        {
            TimeMeasurement measuredTime = new TimeMeasurement
            {
                TimeMeasurementId = 0,
                RegistratorId = registrator.RegistratorId,
                TimeStart = DateTime.Now.AddHours(1),
                TimeStop = DateTime.Now.AddHours(1),
               // DayInWeek= DateTime.Today.DayOfWeek,
                DayOfMonth = DateTime.Today.Day,
                MonthOfYear = DateTime.Today.Month,
                Year = DateTime.Today.Year,
                TimeType = "Work"
            };
            return measuredTime;
        }

        /// <summary>
        /// this function sets start time to the date and at 0 hours, 0 minutes and 0 seconds.
        /// stop time is present time when the function invokes.
        /// </summary>
        /// <param name="registrator"></param>
        /// <param name="stopTime"></param>
        /// <returns></returns>
        public static TimeMeasurement complexStartAndStopClock(Registrator registrator, DateTime stopTime)
        {
            int year = stopTime.Year;
            int month = stopTime.Month;
            int day = stopTime.Day;
            DateTime timestart = stopTime;
            timestart.Subtract(stopTime);
            timestart.AddYears(year);
            timestart.AddMonths(month);
            timestart.AddDays(day);
            TimeMeasurement measuredTime = new TimeMeasurement
            {
                TimeMeasurementId = 0,
                RegistratorId = registrator.RegistratorId,
                TimeStart = timestart,
                TimeStop = stopTime,
                DayOfMonth = DateTime.Today.Day,
                MonthOfYear = DateTime.Today.Month,
                Year = DateTime.Today.Year,
                TimeType = "Work"
            };
            return measuredTime;
        }
        /// <summary>
        /// The basic function to stop the clock.
        /// This function checks the dates so its the same date on start and stop.
        /// If not same dates, calls another funktion that creates the new registration.
        /// </summary>
        /// <param name="measuredTime"></param>
        /// <returns></returns>
        public static TimeMeasurement stopClock(TimeMeasurement measuredTime, DateTime stopTime)
        {
            measuredTime.TimeStop = stopTime.AddHours(1);
            measuredTime.TimeRegistered = measuredTime.TimeStop - measuredTime.TimeStart;
            return measuredTime;
        }

        /// <summary>
        /// this function sets stoptime to 23:59:59.
        /// </summary>
        /// <param name="measuredTime"></param>
        /// <returns></returns>
        public static TimeMeasurement complexStopClock(TimeMeasurement measuredTime)
        {
            double hour = measuredTime.TimeStop.Hour;
            double minutes = measuredTime.TimeStop.Minute;
            double seconds = measuredTime.TimeStop.Second;
            hour = 23 - hour;
            minutes = 59 - minutes;
            seconds = 59 - seconds;
            measuredTime.TimeStop.AddHours(hour);
            measuredTime.TimeStop.AddMinutes(minutes);
            measuredTime.TimeStop.AddSeconds(seconds);
            return measuredTime;
        }
    }
}