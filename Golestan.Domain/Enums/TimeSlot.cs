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
        { TimeSlot.First, "7-9" },
        { TimeSlot.Second, "9-10" },
        { TimeSlot.Third, "10-11" }
    };

    public static string GetDescription(this TimeSlot timeSlot)
    {
        return StatusDescriptions[timeSlot];
    }

}
