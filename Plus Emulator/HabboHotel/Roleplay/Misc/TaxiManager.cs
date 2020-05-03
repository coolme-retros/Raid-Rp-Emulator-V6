using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plus.HabboHotel.Roleplay.Misc
{
    public static class TaxiManager
    {
        /// <summary>
        /// Gets the taxi wait time based on time of day.
        /// </summary>
        /// <returns></returns>
        public static int GetTaxiTime()
        {
            DateTime TimeNow = DateTime.Now;
            TimeSpan TimeOfDay = TimeNow.TimeOfDay;
            int TaxiTime = 0;

            #region Hours

           /* if (!TaxiManager.GetTaxiTime)
                TaxiTime = 0;*/

            if (TimeOfDay.Hours == 00)
                TaxiTime = 3;
            else if (TimeOfDay.Hours == 01)
                TaxiTime = 4;
            else if (TimeOfDay.Hours == 02)
                TaxiTime = 5;
            else if (TimeOfDay.Hours == 03)
                TaxiTime = 5;
            else if (TimeOfDay.Hours == 04)
                TaxiTime = 4;
            else if (TimeOfDay.Hours == 05)
                TaxiTime = 3;
            else if (TimeOfDay.Hours == 06)
                TaxiTime = 2;
            else if (TimeOfDay.Hours == 07)
                TaxiTime = 1;
            else if (TimeOfDay.Hours == 08)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 09)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 10)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 11)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 12)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 13)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 14)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 15)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 16)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 17)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 18)
                TaxiTime = 0;
            else if (TimeOfDay.Hours == 19)
                TaxiTime = 1;
            else if (TimeOfDay.Hours == 20)
                TaxiTime = 1;
            else if (TimeOfDay.Hours == 21)
                TaxiTime = 2;
            else if (TimeOfDay.Hours == 22)
                TaxiTime = 3;
            else if (TimeOfDay.Hours == 23)
                TaxiTime = 3;

            #endregion

            return TaxiTime;
        }
        public static int GetTaxiPrice()
        {
            DateTime TimeNow = DateTime.Now;
            TimeSpan TimeOfDay = TimeNow.TimeOfDay;
            int TaxiPrice = 0;

            #region Hours

            /* if (!TaxiManager.GetTaxiTime)
                 TaxiTime = 0;*/

            if (TimeOfDay.Hours == 00)
                TaxiPrice = 5;
            else if (TimeOfDay.Hours == 01)
                TaxiPrice = 5;
            else if (TimeOfDay.Hours == 02)
                TaxiPrice = 5;
            else if (TimeOfDay.Hours == 03)
                TaxiPrice = 5;
            else if (TimeOfDay.Hours == 04)
                TaxiPrice = 5;
            else if (TimeOfDay.Hours == 05)
                TaxiPrice = 4;
            else if (TimeOfDay.Hours == 06)
                TaxiPrice = 4;
            else if (TimeOfDay.Hours == 07)
                TaxiPrice = 4;
            else if (TimeOfDay.Hours == 08)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 09)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 10)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 11)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 12)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 13)
                TaxiPrice = 0;
            else if (TimeOfDay.Hours == 14)
                TaxiPrice = 0;
            else if (TimeOfDay.Hours == 15)
                TaxiPrice = 0;
            else if (TimeOfDay.Hours == 16)
                TaxiPrice = 1;
            else if (TimeOfDay.Hours == 17)
                TaxiPrice = 1;
            else if (TimeOfDay.Hours == 18)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 19)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 20)
                TaxiPrice = 3;
            else if (TimeOfDay.Hours == 21)
                TaxiPrice = 4;
            else if (TimeOfDay.Hours == 22)
                TaxiPrice = 5;
            else if (TimeOfDay.Hours == 23)
                TaxiPrice = 5;

            #endregion

            return TaxiPrice;
        }
    }
}
