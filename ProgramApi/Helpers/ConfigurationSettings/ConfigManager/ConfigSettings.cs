﻿using ProgramApi.Helpers.ConfigurationSettings.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramApi.Helpers.ConfigurationSettings.ConfigManager
{
    public static class ConfigSettings
    {
        public static ConnectionStrings ConnectionString => ConfigurationSettingsHelper.GetConfigurationSectionObject<ConnectionStrings>("ConnectionStrings");
        public static ApplicationSettings ApplicationSetting => ConfigurationSettingsHelper.GetConfigurationSectionObject<ApplicationSettings>("ApplicationSettings");
    }
}
