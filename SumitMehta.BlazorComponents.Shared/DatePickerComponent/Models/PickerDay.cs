using System;

namespace SumitMehta.BlazorComponents.Shared.DatePickerComponent.Models
{
    /// <summary>
    /// Represents a picker day (day of the week) for the picker
    /// We use a custom class and don't use the <see cref="DayOfWeek"/> so that these day names can be customised by hand for more flexibility
    /// </summary>
    public class PickerDay
    {
        /// <summary>
        /// The enum representing the day of the week
        /// This is internally used to map custom day names with the right days
        /// </summary>
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// The day name to be displayed on the UI
        /// This can be customised
        /// </summary>
        public string DayName { get; set; }
    }
}
