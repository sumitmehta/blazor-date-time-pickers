using System;
using Microsoft.AspNetCore.Components;

namespace SumitMehta.BlazorWebAssembly.Components.Base
{
    /// <summary>
    /// Extend this for any component that supports showing/hiding functionality for the component
    /// </summary>
    public class HideableComponentBase : ComponentBase
    {
        /// <summary>
        /// True if the component is visible; false otherwise
        /// This is used by Toggle method to show/hide component from outside
        /// </summary>
        [Parameter]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Shows the component
        /// </summary>
        public void Show() => IsVisible = true;

        /// <summary>
        /// Hides the component
        /// </summary>
        public void Hide() => IsVisible = false;
    }
}
