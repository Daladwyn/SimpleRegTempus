using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using RegTempus.Models;

namespace RegTempus.Services
{
    public class SqlRegTempusData : IRegTempus
    {
        private RegTempusDbContext _context;

        public SqlRegTempusData()
        {
        }

        public SqlRegTempusData(RegTempusDbContext context)
        {
            _context = context;
        }

        public TimeMeasurement CompleteTimeMeasurement(TimeMeasurement timeStop)
        {
            _context.TimeMeasurements.Attach(timeStop);
            _context.SaveChanges();
            return timeStop;
        }

        public TimeMeasurement CreateNewMeasurement(TimeMeasurement timeStart)
        {
            _context.TimeMeasurements.Add(timeStart);
            _context.SaveChanges();
            return timeStart;
        }

        public Registrator CreateRegistrator(Registrator user)
        {
            _context.Registrators.Add(user);
            _context.SaveChanges();
            return user;
        }

        public List<TimeMeasurement> GetMonthlyTimeMeasurement(int monthOfYear, int year, Registrator user)
        {
            List<TimeMeasurement> MonthFulOfRegistrations = _context.TimeMeasurements.Where(m => m.MonthOfYear == monthOfYear).ToList();
            MonthFulOfRegistrations = MonthFulOfRegistrations.Where(y => y.Year == year).ToList();
            return MonthFulOfRegistrations.Where(r => r.RegistratorId == user.RegistratorId).ToList();
        }

        public Registrator GetRegistratorBasedOnEmail(string userEmail)
        {
            IdentityUser user = _context.Users.SingleOrDefault(u => u.UserName == userEmail);
            Registrator registrator = _context.Registrators.SingleOrDefault(u => u.UserId == user.Id);
            return registrator;
        }

        public Registrator GetRegistratorBasedOnRegistratorId(Registrator user)
        {
            return _context.Registrators.SingleOrDefault(r => r.RegistratorId == user.RegistratorId);
        }

        public Registrator GetRegistratorBasedOnUserId(Registrator user)
        {
            return _context.Registrators.SingleOrDefault(r => r.UserId == user.UserId);
        }

        public TimeMeasurement GetTimeMeasurement(Registrator user)
        {
            return _context.TimeMeasurements.SingleOrDefault(t => t.TimeMeasurementId == user.StartedTimeMeasurement);
        }

        public Registrator UpdateRegistrator(Registrator user)
        {
            _context.Registrators.Attach(user);
            _context.SaveChanges();
            return user;
        }
    }
}