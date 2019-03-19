using RegTempus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegTempus.Repositories
{
    public class RegTempusRepository
    {
        private readonly RegTempusDbContext _appDbContext;

        public RegTempusRepository(RegTempusDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void CompleteTimeMeasurement(DateTime TimeStop, int TimeMeasurementId)
        {
            throw new NotImplementedException();
        }

        public TimeMeasurement CompleteTimeMeasurement(TimeMeasurement timeStop)
        {
            throw new NotImplementedException();
        }

        public void CreateNewMeasurement(DateTime TimeStart)
        {
            throw new NotImplementedException();
        }

        public TimeMeasurement CreateNewMeasurement(TimeMeasurement timeStart)
        {
            throw new NotImplementedException();
        }

        public Registrator CreateRegistrator(Registrator user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TimeMeasurement> GetMonthlyTimeMeasurement(int monthOfYear)
        {
            throw new NotImplementedException();
        }

        public Registrator GetRegistrator(Registrator user)
        {
            throw new NotImplementedException();
        }

        public Registrator GetRegistratorBasedOnRegistratorId(Registrator user)
        {
            throw new NotImplementedException();
        }

        public Registrator GetRegistratorBasedOnUserId(Registrator user)
        {
            throw new NotImplementedException();
        }

        public Registrator UpdateRegistrator(Registrator user)
        {
            throw new NotImplementedException();
        }
    }
}
