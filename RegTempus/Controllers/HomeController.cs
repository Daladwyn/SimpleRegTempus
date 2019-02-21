using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegTempus.Models;
using RegTempus.Services;
using RegTempus.ViewModels;

namespace RegTempus.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private IRegTempus _iRegTempus;

        //public Registrator() { }

        public HomeController(IRegTempus iRegTempus)
        {
            _iRegTempus = iRegTempus;
        }

        public IActionResult Index()
        {
            ClaimsPrincipal user = new ClaimsPrincipal();
            try
            {
                user = User;
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "No logged in user could be found.";
                return View();
            }
            Registrator registrator = Registrator.GetRegistratorData(user);
            registrator = _iRegTempus.GetRegistratorBasedOnUserId(registrator);
            bool result = ((registrator == null) ? false : true);
            if (result == false)
            {
                registrator = Registrator.GetRegistratorData(user);
                registrator.UserHaveStartedTimeMeasure = false;
                registrator.StartedTimeMeasurement = 0;
                registrator = _iRegTempus.CreateRegistrator(registrator);
            }
            UserTimeRegistrationViewModel konvertedRegistrator = UserTimeRegistrationViewModel.RestructureTheRegistratorData(registrator);
            return View(konvertedRegistrator);
        }



        [HttpPost]
        public IActionResult StartTime(int registratorId)
        {
            Registrator registrator = new Registrator
            {
                RegistratorId = registratorId
            };
            try
            {
                registrator = _iRegTempus.GetRegistratorBasedOnRegistratorId(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Did not succed in fetching userdata";
                return View("Index");
            }
            registrator.UserHaveStartedTimeMeasure = true;
            registrator.StartedTimeMeasurement = 0;
            TimeMeasurement timeRegistration = TimeMeasurement.startClock(registrator);
            try
            {
                timeRegistration = _iRegTempus.CreateNewMeasurement(timeRegistration);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Saving the new time registration did not succed";
                return View("Index");
            }
            registrator.StartedTimeMeasurement = timeRegistration.TimeMeasurementId;
            try
            {
                registrator = _iRegTempus.UpdateRegistrator(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Updating your data did not succed";
                return View("Index");
            }
            if (registrator.StartedTimeMeasurement != timeRegistration.TimeMeasurementId)
            {
                ViewBag.ErrorMessage = "Error: changing your details did not succed";
            }
            ViewBag.SuccessMessage = "Your start time is registered.";
            UserTimeRegistrationViewModel konvertedRegistrator = UserTimeRegistrationViewModel.RestructureTheRegistratorData(registrator);
            return View("Index", konvertedRegistrator);
        }

        [HttpPost]
        public IActionResult StopTime(int registratorId)
        {
            Registrator registrator = new Registrator
            {
                RegistratorId = registratorId
            };
            TimeMeasurement measuredTime = new TimeMeasurement();
            try
            {
                registrator = _iRegTempus.GetRegistratorBasedOnRegistratorId(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Fetching your data did not succed. Please make a manual note of present time.";
                return View("Index");
            }
            registrator.UserHaveStartedTimeMeasure = false;
            try
            {
                measuredTime = _iRegTempus.GetTimeMeasurement(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Fetching your start time did not succed. Please make a manual note of present time.";
                return View("Index");
            }
            DateTime stopTime = DateTime.Now;
            if (measuredTime.TimeStart.DayOfYear == stopTime.DayOfYear)
            {
                measuredTime = TimeMeasurement.stopClock(measuredTime, stopTime);
            }
            else
            {
                measuredTime = TimeMeasurement.complexStopClock(measuredTime);

                TimeMeasurement newMeasuredTime = TimeMeasurement.complexStartAndStopClock(registrator, stopTime);
                try
                {
                    newMeasuredTime = _iRegTempus.CompleteTimeMeasurement(newMeasuredTime);
                }
                catch (NullReferenceException)
                {
                    ViewBag.ErrorMessage = "Error: Updating your stop time did not succed. Please make a manual note of present time.";
                    return View("Index");
                }
            }
            try
            {
                measuredTime = _iRegTempus.CompleteTimeMeasurement(measuredTime);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Updating your stop time did not succed. Please make a manual note of present time.";
                return View("Index");
            }
            registrator.StartedTimeMeasurement = 0;
            try
            {
                registrator = _iRegTempus.UpdateRegistrator(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Updating your data did not succed. Please make a manual note of present time.";
                return View("Index");
            }
            ViewBag.SuccessMessage = "Your stop time is registered.";
            UserTimeRegistrationViewModel konvertedRegistrator = UserTimeRegistrationViewModel.RestructureTheRegistratorData(registrator);
            return View("Index", konvertedRegistrator);
        }

        [HttpPost]
        public IActionResult PresentRegistrations(int registratorId, int month)
        {
            List<TimeMeasurement> presentMonthTimeMesurements = new List<TimeMeasurement>();
            int currentMonth = (month > 0) ? month : DateTime.Now.Month;
            int previousMonth = (currentMonth == 1) ? 12 : currentMonth - 1;
            DateTime cM = DateTime.Now;
            string currentMonthAsString = cM.ToString("yyyy MMMM");

            ViewBag.Month = currentMonthAsString;
            ViewBag.PreviousMonth = previousMonth;

            Registrator registrator = new Registrator
            {
                RegistratorId = registratorId
            };
            try
            {
                registrator = _iRegTempus.GetRegistratorBasedOnRegistratorId(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Fetching your data did not succed. Please try again.";
                return View("Index");
            }

            try
            {
                presentMonthTimeMesurements = _iRegTempus.GetMonthlyTimeMeasurement(currentMonth, registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: No registrations was found for the present month.";
                return View();
            }
            List<PresentRegisteredTimeViewModel> currentMonthRegistrations = PresentRegisteredTimeViewModel.CalculateTime(presentMonthTimeMesurements);
            return View(currentMonthRegistrations);
        }
    }
}