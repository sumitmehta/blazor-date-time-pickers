using System;

namespace SumitMehta.BlazorComponents.Shared.DatePickerComponent.Models
{
    /// <summary>
    /// Represents a picker date for the picker
    /// We use a custom class and don't use the <see cref="DateTime"/> directly so that digits of the dates can be customised by hand for more flexibility
    /// </summary>
    public class PickerDate
    {
        /// <summary>
        /// The actual date object representing the date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The text to display for the date - Typically it will be a digit representing the date
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// True if it's today's date; false otherwise
        /// </summary>
        public bool IsToday { get; set; }

        /// <summary>
        /// True if the date belongs to the current month; false otherwise (and it is grayed out in the UI)
        /// </summary>
        public bool BelongsToCurrentMonth { get; set; }

        /// <summary>
        /// True if this is the selected date; false otherwise
        /// </summary>
        public bool IsActive { get; set; }
    }
}
