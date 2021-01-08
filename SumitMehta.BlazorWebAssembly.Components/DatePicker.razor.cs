using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using SumitMehta.BlazorWebAssembly.Components.Base;
using SumitMehta.BlazorComponents.Shared.DatePickerComponent.Models;
using SumitMehta.BlazorComponents.Shared.Enums;
using SumitMehta.BlazorComponents.Shared.Base;

namespace SumitMehta.BlazorWebAssembly.Components
{
    /// <summary>
    /// Initializes the date picker component
    /// </summary>
    public partial class DatePickerBase : HideableComponentBase, IThemeable
    {
        #region "Private variables and methods"

        /// <summary>
        /// The culture info - Used for globalization
        /// </summary>
        private CultureInfo _culture;

        /// <summary>
        /// Private field for the currently selected date
        /// </summary>
        private DateTime? _selectedDate;

        /// <summary>
        /// The months to render
        /// </summary>
        protected PickerMonth[] _months;

        /// <summary>
        /// The days of the week
        /// </summary>
        protected PickerDay[] _days;

        #endregion

        #region "Component Parameters"

        /// <summary>
        /// The width of the component
        /// If this is NULL, the component auto-sizes
        /// </summary>
        [Parameter]
        public int? Width { get; set; }

        /// <summary>
        /// Indicates the month active for the calendar
        /// </summary>
        [Parameter]
        public int Month { get; set; }

        /// <summary>
        /// Indicates the year active for the calendar
        /// </summary>
        [Parameter]
        public int Year { get; set; }

        /// <summary>
        /// The currently selected date
        /// This is also bound to the outside of the component
        /// </summary>
        [Parameter]
        public DateTime? SelectedDate
        {
            get
            {
                return _selectedDate;
            }

            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    SelectedDateChanged.InvokeAsync(_selectedDate);
                }
            }
        }

        /// <summary>
        /// The current culture (for globalization purposes)
        /// This needs to the standard ISO code (hi-IN, pt, en, fr etc.)
        /// </summary>
        [Parameter]
        public string Culture { get; set; }

        /// <summary>
        /// The theme for the picker
        /// </summary>
        [Parameter]
        public Themes Theme { get; set; } = Themes.Blue;

        /// <summary>
        /// This must be trigerred whenever the selected date is changed
        /// </summary>
        [Parameter]
        public EventCallback<DateTime?> SelectedDateChanged { get; set; }

        #endregion

        #region "Component Events"

        /// <summary>
        /// Changes the current month
        /// It is trigerred when month name is clicked
        /// </summary>
        /// <param name="selectedMonth">The month we want to change to</param>
        protected void Change_Month(PickerMonth selectedMonth)
        {
            Month = selectedMonth.Month;

            Array.ForEach(_months, m =>
            {
                if (m.Month == selectedMonth.Month)
                { //if this is the selected month, make it active
                    m.IsActive = true;
                }
                else
                { //else mark all others as inactive
                    m.IsActive = false;
                }
            });
        }

        /// <summary>
        /// Changes the selected date and hides the calendar
        /// It is trigerred when the date in the calendar is clicked
        /// </summary>
        /// <param name="dt">The clicked date</param>
        protected void Change_Date(PickerDate dt)
        {
            SelectedDate = dt.Date;
            Hide(); //Hide calendar after the date is picked
        }

        #endregion

        #region "UI related properties and methods"

        /// <summary>
        /// The selected date text to be displayed on the top left
        /// </summary>
        protected string SelectedDateText => SelectedDate.HasValue ? SelectedDate.Value.ToString("dd MMMM", _culture) : string.Empty;

        /// <summary>
        /// The name of the day for the selected date to be displayed on the top left
        /// </summary>
        protected string SelectedDateDayText => SelectedDate.HasValue ? SelectedDate.Value.ToString("dddd", _culture) : string.Empty;

        /// <summary>
        /// Returns the weeks and dates inside each week for rendering on the UI
        /// </summary>
        protected PickerDate[][] Weeks
        {
            get
            {
                //Let's get the month and year of the current date
                int month = Month,
                    year = Year;

                //Let's set the first date of the selected month
                var firstDayOfMonth = new DateTime(year, month, 1);

                //This will be the start date of our calendar
                DateTime startDate = firstDayOfMonth;

                if (firstDayOfMonth.DayOfWeek != _days.First().Day)
                { //If the first date of month isn't on the first day of the week, we need to go back and start with the date of the previous month that falls on the first day of the week
                    //These dates will be grayed out

                    //Let's get the index of the day the first day falls in
                    var firstDayIndex = Array.IndexOf(_days, _days.First(d => d.Day == firstDayOfMonth.DayOfWeek));

                    //Let's go back and set the start date starting off the first day of the week
                    //For e.g. - If the start date is on Wednesday, we go 3 days backwards (3 is the index of Wednesday in the _days array) and start the date from Monday
                    //so that all the blocks on the calendar are filled
                    startDate = firstDayOfMonth.AddDays(-firstDayIndex);
                }

                //Let's set the last date of the selected month
                var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                //This will be the end date of our calendar
                DateTime endDate = lastDayOfMonth;

                if (lastDayOfMonth.DayOfWeek != _days.Last().Day)
                { //If the last date of month isn't on the last day of the week, we need to go forward and end with the date of the next month that falls on the last day of the week
                    //These dates will be grayed out

                    //Let's get the index of the day the last day falls in
                    var lastDayIndex = Array.IndexOf(_days, _days.First(d => d.Day == lastDayOfMonth.DayOfWeek));

                    //Let's go forward and set the end date ending off the last day of the week
                    //For e.g. - If the end date is on Wednesday, we go 4 days forwards (7 - 3 where 7 is the length of the week and 3 is the index of Wednesday in the _days array) and end the date on Sunday
                    //so that all the blocks on the calendar are filled
                    endDate = lastDayOfMonth.AddDays(_days.Length - lastDayIndex);
                }

                //These are the weeks where each element inside week is a date
                List<PickerDate[]> weeks = new List<PickerDate[]>();

                //These are the dates that are displayed on the calendar
                List<PickerDate> dates = new List<PickerDate>();

                //Last day of the week
                var lastDayOfTheWeek = _days.Last();

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                { //Now run through the start and end dates and fill the dates and weeks
                    var dateToAdd = new PickerDate
                    {
                        Date = date,
                        Text = date.Day.ToString(),
                        BelongsToCurrentMonth = date.Month == month,
                        IsToday = date.Date == DateTime.Now.Date,
                        IsActive = SelectedDate.HasValue && date.Date == SelectedDate.Value.Date
                    };

                    dates.Add(dateToAdd);

                    if (date.DayOfWeek == lastDayOfTheWeek.Day)
                    { //if the date falls on the last day of the week, append all the dates added to the weeks array and initialize a new list for the next week dates
                        weeks.Add(dates.ToArray());
                        dates = new List<PickerDate>();
                    }
                }

                //Return weeks
                return weeks.ToArray();
            }
        }

        /// <summary>
        /// Changes the theme for the picker to the theme provided in the parameter
        /// </summary>
        /// <param name="newTheme">The new theme to be applied</param>
        public void ChangeTheme(Themes newTheme)
        {
            Theme = newTheme;
        }

        #endregion

        #region "Component Lifecycle Events"

        /// <summary>
        /// This is a component life cycle event that's trigerred when the component is initialized
        /// We use it to initialize days, dates, culture and selected date
        /// </summary>
        /// <returns>An async Task</returns>
        protected override Task OnInitializedAsync()
        {
            //Initialize the culture and date time info
            if (string.IsNullOrEmpty(Culture) || string.IsNullOrWhiteSpace(Culture))
            {
                _culture = CultureInfo.CurrentCulture;

            }
            else
            {
                _culture = new CultureInfo(Culture);
            }

            //Initialize all the months
            _months = new PickerMonth[]
            {
                new PickerMonth { Month = 1, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(1) },
                new PickerMonth { Month = 2, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(2) },
                new PickerMonth { Month = 3, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(3) },
                new PickerMonth { Month = 4, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(4) },
                new PickerMonth { Month = 5, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(5) },
                new PickerMonth { Month = 6, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(6) },
                new PickerMonth { Month = 7, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(7) },
                new PickerMonth { Month = 8, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(8) },
                new PickerMonth { Month = 9, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(9) },
                new PickerMonth { Month = 10, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(10) },
                new PickerMonth { Month = 11, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(11) },
                new PickerMonth { Month = 12, MonthName = _culture.DateTimeFormat.GetAbbreviatedMonthName(12) },
            };

            //Initialize all the days
            _days = new PickerDay[]
            {
                new PickerDay { Day = DayOfWeek.Monday, DayName = _culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Monday) },
                new PickerDay { Day = DayOfWeek.Tuesday, DayName = _culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Tuesday) },
                new PickerDay { Day = DayOfWeek.Wednesday, DayName = _culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Wednesday) },
                new PickerDay { Day = DayOfWeek.Thursday, DayName = _culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Thursday) },
                new PickerDay { Day = DayOfWeek.Friday, DayName = _culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Friday) },
                new PickerDay { Day = DayOfWeek.Saturday, DayName = _culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Saturday) },
                new PickerDay { Day = DayOfWeek.Sunday, DayName = _culture.DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Sunday) }
            };

            if (SelectedDate.HasValue)
            { //if a selected date is supplied, extract month and year from the date to select the calendar active month and year (if they are also not supplied explicitly)
                if (Month == default)
                {
                    Month = SelectedDate.Value.Month;
                }

                if (Year == default)
                {
                    Year = SelectedDate.Value.Year;
                }
            }
            else
            { //if no selected date is provided, set the month and year to the current date (if they are also not supplied explicitly)
                if (Month == default)
                {
                    Month = DateTime.Now.Month;
                }

                if (Year == default)
                {
                    Year = DateTime.Now.Year;
                }
            }

            //Change to initial month
            Change_Month(_months.First(m => m.Month == Month));

            return base.OnInitializedAsync();
        }

        #endregion
    }
}