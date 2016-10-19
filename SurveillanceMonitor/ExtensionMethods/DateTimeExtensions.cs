using System;

namespace SurveillanceMonitor.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static string ToFileDateTime(this DateTime dateTime)
        {            
            return dateTime.ToString("yyyy-MM-dd_hh_mm_ss");
        }

    }
}
