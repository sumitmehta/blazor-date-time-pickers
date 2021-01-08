using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SumitMehta.BlazorComponents.Shared.TimePickerComponent.Models;
using SumitMehta.BlazorComponents.Shared;
using SumitMehta.BlazorWebAssembly.Components.Base;
using SumitMehta.BlazorComponents.Shared.Base;
using SumitMehta.BlazorComponents.Shared.Enums;

namespace SumitMehta.BlazorWebAssembly.Components
{
    /// <summary>
    /// Initializes the time picker component
    /// </summary>
    public partial class TimePickerBase : HideableComponentBase, IThemeable
    {
        #region "Private Enums"

        /// <summary>
        /// The types of our drag sources/handles
        /// </summary>
        private enum DragSources
        {
            /// <summary>
            /// Represents the hours handle of the clock
            /// </summary>
            HoursHand,

            /// <summary>
            /// Represents the minutes handle of the clock
            /// </summary>
            MinutesHand,

            /// <summary>
            /// Represents the seconds handle of the clock
            /// </summary>
            SecondsHand
        }

        /// <summary>
        /// Represents the two time modes - AM/PM
        /// </summary>
        private enum TimeMode
        {
            /// <summary>
            /// AM
            /// </summary>
            AM,

            /// <summary>
            /// PM
            /// </summary>
            PM
        }

        #endregion

        #region "Dependencies"

        /// <summary>
        /// The javascript runtime service
        /// </summary>
        [Inject]
        private IJSRuntime JS { get; set; }

        #endregion

        #region "Component Parameters"

        /// <summary>
        /// The current time selected
        /// This is also bound to the outside of the component
        /// </summary>
        [Parameter]
        public TimeSpan? SelectedTime { get; set; }

        /// <summary>
        /// True if the clock is enabled to set the time; false otherwise
        /// This is toggled by the center button and when it's false, <seealso cref="SelectedTime"/> is set to NULL
        /// This allows us to have Nullable TimeSpan (if time is optional)
        /// </summary>
        [Parameter]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Represents the hour (1-12) for the selected time
        /// </summary>
        [Parameter]
        public int Hour { get; set; }

        /// <summary>
        /// Represents the minute (0-59) for the selected time
        /// </summary>
        [Parameter]
        public int Minute { get; set; }

        /// <summary>
        /// Represents the second (0-59) for the selected time
        /// </summary>
        [Parameter]
        public int Second { get; set; }

        /// <summary>
        /// The theme for the picker
        /// </summary>
        [Parameter]
        public Themes Theme { get; set; } = Themes.Blue;

        /// <summary>
        /// This must be trigerred whenever the selected time is changed
        /// </summary>
        [Parameter]
        public EventCallback<TimeSpan?> SelectedTimeChanged { get; set; }

        #endregion

        #region "Private variables and methods"

        /// <summary>
        /// This variable saves the base hour set by hour hand dragging operation
        /// This is used to adjust the position of the hour hand by the minute hand
        /// </summary>
        private int _hourBase;

        /// <summary>
        /// Represents the bounding rectangle of the time-picker component
        /// </summary>
        private BoundingClientRect _clockDimensions;

        /// <summary>
        /// The time mode - AM/PM
        /// </summary>
        private TimeMode _mode;

        /// <summary>
        /// Updates the time and notifies the subscribers outside of this component
        /// </summary>
        private void UpdateTime()
        {
            //If the time picker is disabled for picking dates, set the selected time to NULL
            TimeSpan? newTime = !IsEnabled ? null : new TimeSpan(Hour + (_mode == TimeMode.PM ? 12 : 0), Minute, Second); //Consider AM/PM
            SelectedTime = newTime;

            //Notify everyone using the time from the component
            SelectedTimeChanged.InvokeAsync(newTime);
        }

        #endregion

        #region "UI related properties and methods"

        /// <summary>
        /// Represents the DOM element for the time-picker
        /// This is used to get the bounding rectangle from javascript code
        /// </summary>
        protected ElementReference TimePickerRef { get; set; }

        /// <summary>
        /// The mode text to display: AM or PM
        /// </summary>
        protected string ModeText => _mode == TimeMode.AM ? "AM" : "PM";

        /// <summary>
        /// Represents the angle for the hours hand
        /// </summary>
        protected double HoursDegrees { get; set; }

        /// <summary>
        /// Represents the angle for the minutes hand
        /// </summary>
        protected double MinutesDegrees { get; set; }

        /// <summary>
        /// Represents the angle for the seconds hand
        /// </summary>
        protected double SecondsDegrees { get; set; }

        /// <summary>
        /// Represents the current hand being dragged: Hour, Minute or Second
        /// This is set on Drag_Start event for the respective hand 
        /// </summary>
        private DragSources DragSource { get; set; }

        /// <summary>
        /// Changes the theme for the picker to the theme provided in the parameter
        /// </summary>
        /// <param name="newTheme">The new theme to be applied</param>
        public void ChangeTheme(Themes newTheme)
        {
            Theme = newTheme;
        }

        #endregion

        #region "Component Events"

        /// <summary>
        /// Trigerred when the hours hand is started for being dragged
        /// </summary>
        /// <param name="args">The drag event arguments</param>
        protected void HoursHand_DragStart(DragEventArgs args)
        {
            //Set the current drag source to hours hand so that Clock_DragOver operates on it
            DragSource = DragSources.HoursHand;
        }

        /// <summary>
        /// Trigerred when the hours hand is being stopped dragging
        /// </summary>
        /// <param name="args">The drag event arguments</param>
        protected void HoursHand_DragEnd(DragEventArgs args)
        {
            //Round the hour angle to multiple of 30 degrees so that hour hand is snapped to a valid number from 1-12
            //(Simply said, this will prevent the hour hand to be placed somewhere between the hours 3 and 4 and will snap it to the nearest one) 
            HoursDegrees = Hour.ToExactHourDegrees(MinutesDegrees);
        }

        /// <summary>
        /// Trigerred when the minutes hand is started for being dragged
        /// </summary>
        /// <param name="args">The drag event arguments</param>
        protected void MinutesHand_DragStart(DragEventArgs args)
        {
            //Set the current drag source to minutes hand so that Clock_DragOver operates on it
            DragSource = DragSources.MinutesHand;
        }

        /// <summary>
        /// Trigerred when the minutes hand is being stopped dragging
        /// </summary>
        /// <param name="args">The drag event arguments</param>
        protected void MinutesHand_DragEnd(DragEventArgs args)
        {
            //Round the minute angle to multiple of 6 degrees so that minute hand is snapped to a valid number from 0-59
            //(Simply said, this will prevent the minute hand to be placed somewhere between the minutes 30 and 31 and will snap it to the nearest one) 
            MinutesDegrees = Minute.ToExactMinsSecsDegrees();
        }

        /// <summary>
        /// Trigerred when the seconds hand is started for being dragged
        /// </summary>
        /// <param name="args">The drag event arguments</param>
        protected void SecondsHand_DragStart(DragEventArgs args)
        {
            //Set the current drag source to seconds hand so that Clock_DragOver operates on it
            DragSource = DragSources.SecondsHand;
        }

        /// <summary>
        /// Trigerred when the seconds hand is being stopped dragging
        /// </summary>
        /// <param name="args">The drag event arguments</param>
        protected void SecondsHand_DragEnd(DragEventArgs args)
        {
            //Round the second angle to multiple of 6 degrees so that second hand is snapped to a valid number from 0-59
            //(Simply said, this will prevent the second hand to be placed somewhere between the seconds 30 and 31 and will snap it to the nearest one)
            SecondsDegrees = Second.ToExactMinsSecsDegrees();
        }

        /// <summary>
        /// This is the main event that is fired when the hands are dragged
        /// </summary>
        /// <param name="args">Drop event arguments</param>
        protected void Clock_DragOver(DragEventArgs args)
        {
            //Get the x and y points of the drag operation
            var x = args.ClientX - _clockDimensions.Left;
            var y = args.ClientY - _clockDimensions.Top;

            //Calculate the angle with respect to the center point.
            //We are restricting the clock size to 340x340 pixel so center is going to be 170x170
            int center = 170; //just make it static
            double degrees = -Math.Atan2(center - x, center - y) * 180 / Math.PI;

            //Let's keep it under 360 and not cycle it upwards
            if (degrees < 0) degrees += 360;

            if (DragSource == DragSources.HoursHand)
            { //if it's the hours hand being dragged currently, set the angle for the hours hand and also calculate and set the hour number (1-12)
                HoursDegrees = degrees;
                Hour = (int)Math.Round(12 / (360 / degrees));

                //Save a copy in a private variable so that minute hand can use it later to calculate the offset of angle for the hour hand
                //based on the current minute hand's angle
                _hourBase = Hour;
            }

            if (DragSource == DragSources.MinutesHand || DragSource == DragSources.SecondsHand)
            { //if it's the minutes or seconds hand being dragged currently, set the angle for the respective hand and also calculate and set the respective number (0-59)
                int intNumber = (int)Math.Round(60 / (360 / degrees));

                if (DragSource == DragSources.MinutesHand)
                {
                    MinutesDegrees = degrees;
                    Minute = intNumber;
                    HoursDegrees = _hourBase.ToExactHourDegrees(MinutesDegrees); //Adjust the position of the hour hand
                }

                if (DragSource == DragSources.SecondsHand)
                {
                    SecondsDegrees = degrees;
                    Second = intNumber;
                }
            }

            //Trigger update time operation so that everyone is notified and a TimeSpan can be updated globally
            UpdateTime();
        }

        /// <summary>
        /// Toggles the AP/PM mode
        /// </summary>
        /// <param name="args">Mouse event arguments</param>
        protected void Toggle_Mode(MouseEventArgs args)
        {
            if (_mode == TimeMode.AM)
            {
                _mode = TimeMode.PM;
            }
            else
            {
                _mode = TimeMode.AM;
            }

            //Update the time parameter after adjusting AM/PM
            UpdateTime();
        }

        /// <summary>
        /// Enables/Disables the time picker for picking up dates
        /// This allows us to have Nullable TimeSpan (if time is optional)
        /// </summary>
        /// <param name="args">Mouse event arguments</param>
        protected void Toggle_Picker(MouseEventArgs args)
        {
            IsEnabled = !IsEnabled;
            UpdateTime();
        }

        #endregion

        #region "Component Lifecycle Events"

        /// <summary>
        /// This is a component life cycle event that's trigerred after the component is rendered
        /// We use it to load additional javascript to get the bounding client rectangle for our component so that we can
        /// calculate the offsets correctly while doing drag and drop operation for clock arms
        /// </summary>
        /// <param name="firstRender">True if the component is rendering for the first time; false otherwise</param>
        /// <returns>An async task</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Import the js file
            //Please note that the path of the js file specified here is generated at runtime by the project that will use this component library
            IJSObjectReference jsModule = await JS.InvokeAsync<IJSObjectReference>("import", $"./_content/{Assembly.GetExecutingAssembly().GetName().Name}/TimePicker.razor.js");

            //Let's now call our js function and store the bounding rectangle dimensions
            _clockDimensions = await jsModule.InvokeAsync<BoundingClientRect>("GetBoundingClientRect", TimePickerRef);

            //Call the base OnAfterRenderAsync
            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// This is a component life cycle event that's trigerred when the component is initialized
        /// We use it to initialize selected time
        /// </summary>
        /// <returns>An async Task</returns>
        protected override async Task OnInitializedAsync()
        {
            if (SelectedTime.HasValue)
            { //if a selected time is supplied, extract hours, minutes and seconds (if they are also not supplied explicitly) from the time to select the active time 
                if (Second == default)
                {
                    Second = SelectedTime.Value.Seconds;
                }

                if (Minute == default)
                {
                    Minute = SelectedTime.Value.Minutes;
                }

                if (Hour == default)
                {
                    Hour = SelectedTime.Value.Hours;
                }
            }
            else
            { //if no selected time is provided, set the hours, minutes and seconds to the current time (if they are also not supplied explicitly)
                if (Second == default)
                {
                    Second = DateTime.Now.Second;
                }

                if (Minute == default)
                {
                    Minute = DateTime.Now.Minute;
                }

                if (Hour == default)
                {
                    Hour = DateTime.Now.Hour;
                }
            }

            if (Hour > 12)
            {
                _mode = TimeMode.PM;
                Hour -= 12;
            }
            else
            {
                _mode = TimeMode.AM;
            }

            //Set the angle for the set time
            MinutesDegrees = Minute.ToExactMinsSecsDegrees();
            HoursDegrees = Hour.ToExactHourDegrees(MinutesDegrees);
            _hourBase = Hour;
            SecondsDegrees = Second.ToExactMinsSecsDegrees();

            await base.OnInitializedAsync();
        }

        #endregion
    }
}