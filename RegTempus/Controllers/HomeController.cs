using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegTempus.Models;
using RegTempus.Services;
using RegTempus.ViewModels;


namespace RegTempus.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IRegTempus _iRegTempus;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(IRegTempus iRegTempus, SignInManager<IdentityUser> signInManager)
        {
            _iRegTempus = iRegTempus;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                string userEmail = User.Identity.Name;
                Registrator registrator = _iRegTempus.GetRegistratorBasedOnEmail(userEmail);
                return RedirectToAction("RegisterTime", "Home", registrator);
            }
        }

        [HttpGet]
        public IActionResult RegisterTime(Registrator registrator)
        {
            bool result = ((registrator == null) ? false : true);
            if (result == true)
            {
                UserTimeRegistrationViewModel konvertedRegistrator = UserTimeRegistrationViewModel.RestructureTheRegistratorData(registrator);
                ViewBag.CurrentMonth = DateTime.Now;
                return View(konvertedRegistrator);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
                return View("RegisterTime");
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
                return View("RegisterTime");
            }
            registrator.StartedTimeMeasurement = timeRegistration.TimeMeasurementId;
            try
            {
                registrator = _iRegTempus.UpdateRegistrator(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Updating your data did not succed";
                return View("RegisterTime");
            }
            if (registrator.StartedTimeMeasurement != timeRegistration.TimeMeasurementId)
            {
                ViewBag.ErrorMessage = "Error: changing your details did not succed";
            }
            ViewBag.SuccessMessage = "Your start time is registered.";
            UserTimeRegistrationViewModel konvertedRegistrator = UserTimeRegistrationViewModel.RestructureTheRegistratorData(registrator);
            return View("RegisterTime", konvertedRegistrator);
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
                return View("RegisterTime");
            }
            registrator.UserHaveStartedTimeMeasure = false;
            try
            {
                measuredTime = _iRegTempus.GetTimeMeasurement(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Fetching your start time did not succed. Please make a manual note of present time.";
                return View("RegisterTime");
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
                    return View("RegisterTime");
                }
            }
            try
            {
                measuredTime = _iRegTempus.CompleteTimeMeasurement(measuredTime);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Updating your stop time did not succed. Please make a manual note of present time.";
                return View("RegisterTime");
            }
            registrator.StartedTimeMeasurement = 0;
            try
            {
                registrator = _iRegTempus.UpdateRegistrator(registrator);
            }
            catch (NullReferenceException)
            {
                ViewBag.ErrorMessage = "Error: Updating your data did not succed. Please make a manual note of present time.";
                return View("RegisterTime");
            }
            ViewBag.SuccessMessage = "Your stop time is registered.";
            UserTimeRegistrationViewModel konvertedRegistrator = UserTimeRegistrationViewModel.RestructureTheRegistratorData(registrator);
            return View("RegisterTime", konvertedRegistrator);
        }

        [HttpPost]
        public IActionResult PresentRegistrations(int registratorId, DateTime currentMonth)
        {
            List<TimeMeasurement> presentMonthTimeMesurements = new List<TimeMeasurement>();
            //DateTime currentMonth = DateTime.Now;
            int currentMonthAsInt = currentMonth.Month;
            ViewBag.Month = currentMonth.ToString("yyyy MMMM");
            ViewBag.NextMonth = currentMonth.AddMonths(1);
            TimeSpan oneMonth = currentMonth.AddMonths(1) - currentMonth;
            DateTime PrevMonth = currentMonth.Subtract(oneMonth);
            ViewBag.PrevMonth = PrevMonth;

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
                return View("RegisterTime");
            }

            try
            {
                presentMonthTimeMesurements = _iRegTempus.GetMonthlyTimeMeasurement(currentMonthAsInt, registrator);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Error: No registrations was found for the present month.";
                return View("RegisterTime");
            }
            if (presentMonthTimeMesurements.Count == 0)
            {
                List<PresentRegisteredTimeViewModel> NoRegisteredTime = new List<PresentRegisteredTimeViewModel>();
                return View(NoRegisteredTime);
            }
            List<PresentRegisteredTimeViewModel> currentMonthRegistrations = PresentRegisteredTimeViewModel.CalculateTime(presentMonthTimeMesurements);
            return View(currentMonthRegistrations);
        }
    }
}