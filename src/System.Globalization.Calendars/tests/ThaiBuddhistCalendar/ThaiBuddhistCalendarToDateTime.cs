// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Globalization.Tests;
using Xunit;

namespace System.Globalization.CalendarsTests
{
    // System.Globalization.ThaiBuddhistCalendar.ToDateTime(Int32,Int32,Int32,Int32,Int32,Int32,Int32,Int32)
    public class ThaiBuddhistCalendarToDateTime
    {
        private readonly int[] _DAYS_PER_MONTHS_IN_LEAP_YEAR = new int[13]
        {
            0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
        };
        private readonly int[] _DAYS_PER_MONTHS_IN_NO_LEAP_YEAR = new int[13]
        {
            0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
        };

        #region Positive Tests
        // PosTest1: Verify the year is a random year
        [Fact]
        public void PosTest1()
        {
            System.Globalization.Calendar tbc = new ThaiBuddhistCalendar();
            Random rand = new Random(-55);
            int year = rand.Next(tbc.MinSupportedDateTime.Year + 543, tbc.MaxSupportedDateTime.Year + 543);
            int month = rand.Next(1, 13);
            int day;
            if (IsLeapYear(year))
            {
                day = rand.Next(1, _DAYS_PER_MONTHS_IN_LEAP_YEAR[month] + 1);
            }
            else
            {
                day = rand.Next(1, _DAYS_PER_MONTHS_IN_NO_LEAP_YEAR[month] + 1);
            }

            int hour = rand.Next(0, 24);
            int minute = rand.Next(0, 60);
            int second = rand.Next(0, 60);
            int milliSecond = rand.Next(0, 1000);
            int era = 0;
            for (int i = 0; i < tbc.Eras.Length; i++)
            {
                era = tbc.Eras[i];
                DateTime dt = tbc.ToDateTime(year, month, day, hour, minute, second, milliSecond, era);
                DateTime desiredDT = tbc.ToDateTime(year, month, day, hour, minute, second, milliSecond);
                Assert.Equal(dt, desiredDT);
            }
        }

        // PosTest2: Verify the DateTime is 9999-12-31 23:59:29:999
        [Fact]
        public void PosTest2()
        {
            System.Globalization.Calendar tbc = new ThaiBuddhistCalendar();
            int year = 9999 + 543;
            int month = 12;
            int day = 31;
            int hour = 23;
            int minute = 59;
            int second = 59;
            int milliSecond = 999;
            int era;
            for (int i = 0; i < tbc.Eras.Length; i++)
            {
                era = tbc.Eras[i];
                DateTime dt = tbc.ToDateTime(year, month, day, hour, minute, second, milliSecond, era);
                DateTime desireDT = tbc.ToDateTime(year, month, day, hour, minute, second, milliSecond);
                Assert.Equal(dt, desireDT);
            }
        }

        // PosTest3: Verify the DateTime is ThaiBuddhistCalendar MinSupportedDateTime
        [Fact]
        public void PosTest3()
        {
            System.Globalization.Calendar tbc = new ThaiBuddhistCalendar();
            DateTime minDT = tbc.MinSupportedDateTime;
            int year = 1 + 543;
            int month = 1;
            int day = 1;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int milliSecond = 0;
            int era;
            for (int i = 0; i < tbc.Eras.Length; i++)
            {
                era = tbc.Eras[i];
                DateTime dt = tbc.ToDateTime(year, month, day, hour, minute, second, milliSecond, era);
                Assert.Equal(dt, minDT);
            }
        }
        #endregion
        
        #region Helper Methods        
        private bool IsLeapYear(int year)
        {
            year -= 543;
            return (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        }
        #endregion
    }
}
