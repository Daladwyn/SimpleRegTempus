using RegTempus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegTempus.Services
{
    public interface IRegTempus
    {
        TimeMeasurement CreateNewMeasurement(TimeMeasurement timeStart);
        TimeMeasurement CompleteTimeMeasurement(TimeMeasurement timeStop);
        TimeMeasurement GetTimeMeasurement(Registrator user);
        List<TimeMeasurement> GetMonthlyTimeMeasurement(int monthOfYear,Registrator registrator);
        Registrator GetRegistratorBasedOnRegistratorId(Registrator user);
        Registrator GetRegistratorBasedOnUserId(Registrator user);
        Registrator GetRegistratorBasedOnEmail(string userEmail);
        Registrator UpdateRegistrator(Registrator user);
        Registrator CreateRegistrator(Registrator user);
    }
}
