using System;
using System.Collections.Generic;
using System.Configuration;

namespace Utils
{
    public static class Config
    {
        public static int IntConfig(string key)
        { 
            return int.Parse(StringConfig(key));
        }

        public static string StringConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static bool BoolConfig(string key)
        {
            return Convert.ToBoolean(StringConfig(key));
        }
    }
}
