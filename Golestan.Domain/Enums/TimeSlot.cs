namespace Golestan.Domain.Enums;

using System.Reflection;
using Microsoft.Extensions.Primitives;


public enum TimeSlot {

    First = 1,

    Second = 2,

    Third = 3,

    Fourth = 4,

    Fifth = 5,

    Sixth = 6,

}


public static class StatusExtensions {

    private static readonly Dictionary<TimeSlot, string> StatusDescriptions = new Dictionary<TimeSlot, string>
    {
        { TimeSlot.First, "7:30 - 9:00" },
        { TimeSlot.Second, "9:00 - 10:30" },
        { TimeSlot.Third, "10:30 - 12:00" },
        { TimeSlot.Fourth, "13:00 - 14:30" },
        { TimeSlot.Fifth, "14:30 - 16:00" },
        { TimeSlot.Sixth, "16:00 - 17:00" },
    };

    public static string GetDescription(this TimeSlot timeSlot)
    {
        return StatusDescriptions[timeSlot];
    }

}
