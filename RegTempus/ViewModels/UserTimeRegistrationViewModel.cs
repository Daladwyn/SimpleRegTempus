using RegTempus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegTempus.ViewModels
{
    public class UserTimeRegistrationViewModel
    {
        public int RegistratorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool UserHaveStartedTimeMeasure { get; set; }

        public static UserTimeRegistrationViewModel RestructureTheRegistratorData(Registrator registator)
        {
            UserTimeRegistrationViewModel modifiedRegistrator = new UserTimeRegistrationViewModel
            {
                RegistratorId = registator.RegistratorId,
                FirstName = registator.FirstName,
                LastName = registator.LastName,
                UserHaveStartedTimeMeasure = registator.UserHaveStartedTimeMeasure
            };
            return modifiedRegistrator;
        }
    }
}
