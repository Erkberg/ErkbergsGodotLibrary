using Godot;
using System;

namespace ErkbergsGodotLibrary
{
    public partial class TimeFormatUtil
    {
        public static string GetFormattedTimeStringMinutesSeconds(float seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"mm\:ss");
        }

        public static string GetFormattedTimeStringHoursMinutesSeconds(float seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}