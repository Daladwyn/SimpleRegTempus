using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RegTempus.ViewModels
{
    public class SettingsViewModel
    {
        [Required]
        public int SettingsId { get; set; }

        [Required]
        public int RegistratorId { get; set; }

        public TimeZoneInfo userTimeZone { get; set; }

        public DaylightTime daylight { get; set; } // = TimeZoneInfo.localZone.GetDaylightChanges(currentYear);
        //TimeZoneInfo localZone = TimeZoneInfo.;


    }
}
