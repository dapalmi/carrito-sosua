using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarritoSosua.ExtensionMethods
{
    public static class DayOfWeekExtensions
    {
            public static string ToCurrentCultureString(this DayOfWeek dayOfWeek)
            {
                var formatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

                return formatInfo.DayNames[(int)dayOfWeek];
            }

        public static string ToSpanishDayOfWeekName(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Lunes";
                case DayOfWeek.Tuesday:
                    return "Martes";
                case DayOfWeek.Wednesday:
                    return "Miercoles";
                case DayOfWeek.Thursday:
                    return "Jueves";
                case DayOfWeek.Friday:
                    return "Viernes";
                case DayOfWeek.Saturday:
                    return "Sabado";
                case DayOfWeek.Sunday:
                    return "Domingo";
                default:
                    return "n/a";
            }
        }
    }
}
