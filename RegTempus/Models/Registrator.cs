using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using RegTempus.Services;

namespace RegTempus.Models
{
    public class Registrator
    {
        public int RegistratorId { get; set; }

        [MaxLength(36)]
        public string UserId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public bool UserHaveStartedTimeMeasure { get; set; }

        public int StartedTimeMeasurement { get; set; }


        /// <summary>
        /// This function extracts 3 properties(ObjectIdentifier, Givenname and surname)
        /// of the User token from Azure AD.
        /// </summary>
        /// <returns>Output from this function is a Registrator object</returns>
        internal static Registrator GetRegistratorData(ClaimsPrincipal User)
        {
            Registrator registrator = new Registrator();
            if (User.Identity.IsAuthenticated)
            {
                foreach (var Identity in User.Identities)
                {
                    foreach (var claim in Identity.Claims)
                    {
                        if (claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                        {
                            registrator.UserId = claim.Value;
                        }
                        if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")
                        {
                            registrator.FirstName = claim.Value;
                        }
                        if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")
                        {
                            registrator.LastName = claim.Value;
                        }
                    }
                }
            }
            return registrator;
        }


        //public static Registrator GetUserBasedOnEmail(string userEmail)
        //{
        //     private readonly UserManager<IdentityUser> _userManager;

        //public GetUserBasedOnEmail(
        //    UserManager<IdentityUser> userManager, IRegTempus iRegTempus)
        //{
        //    _iRegTempus = iRegTempus;
        //    _signInManager = signInManager;
        //    _userManager = userManager;
        //}
        ////Registrator registrator = ;
        //var user = _userManager.FindByNameAsync(userEmail);
        //    return registrator;
        //}
    }
}