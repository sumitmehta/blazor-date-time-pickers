namespace SumitMehta.BlazorComponents.Shared.DatePickerComponent.Models
{
    /// <summary>
    /// Represents a picker month for the picker
    /// We use a custom class so that month names can be customised by hand
    /// </summary>
    public class PickerMonth
    {
        /// <summary>
        /// The index of the month - ranging from 1 to 12
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// The text to display for the month - Typically it will be a month name
        /// </summary>
        public string MonthName { get; set; }

        /// <summary>
        /// True if the selected month is the current month; false otherwise
        /// </summary>
        public bool IsActive { get; set; }
    }
}
