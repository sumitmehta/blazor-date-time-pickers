using System;
using SumitMehta.BlazorComponents.Shared.Enums;

namespace SumitMehta.BlazorComponents.Shared.Base
{
    /// <summary>
    /// Applied on components which can be themed
    /// </summary>
    public interface IThemeable
    {
        /// <summary>
        /// The active theme for the component
        /// </summary>
        Themes Theme { get; set; }

        /// <summary>
        /// Changes the current active theme to the theme provided in the parameter
        /// </summary>
        /// <param name="newTheme">New theme to be applied</param>
        void ChangeTheme(Themes newTheme);
    }
    
}
