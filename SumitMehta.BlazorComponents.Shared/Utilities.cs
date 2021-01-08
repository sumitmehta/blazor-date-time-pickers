namespace SumitMehta.BlazorComponents.Shared
{
    /// <summary>
    /// This class has several utility extension methods
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Rounds up a number to the nearest number of the given multiple 
        /// For e.g. - If we are rounding 34 to its nearest multiple of 7, the answer would be 35
        /// </summary>
        /// <param name="numberToRound">The number that we want to round</param>
        /// <param name="multiple">The multiple for which we want to round the number</param>
        /// <returns>The nearest rounded number divisible by the multiple</returns>
        /// <remarks>This function works only on the positive numbers</remarks>
        public static int RoundTo(this int numberToRound, int multiple)
        {
            if (multiple == 0) //Multiple is 0, return numToRound as it is
                return numberToRound;

            int remainder = numberToRound % multiple; //Calculate the remainder now

            if (remainder == 0) //if remainder is 0, the number is already divisible by the given multiple, hence return the number as it is
                return numberToRound;

            return numberToRound + (multiple - remainder); //Calculate and return the rounded number
        }

        #region "Utility methods for time picker"

        /// <summary>
        /// Multiplies the input with the angle specified
        /// This is useful to round off clock angles to the nearest valid angle
        /// </summary>
        /// <param name="input">An input number</param>
        /// <param name="angle">The angle to multiple</param>
        /// <returns>Returns input after multiplying it by the specified angle</returns>
        private static double ToExactDegrees(this int input, double angle)
        {
            return input * angle;
        }

        /// <summary>
        /// Rounds the hour angle to multiple of 30 degrees so that hour hand is snapped to a valid number from 1-12
        /// It also accounts for the minute hand's angle and adjusts the hour hand's position accordingly
        /// </summary>
        /// <param name="hour">The hour - 1 to 12</param>
        /// <param name="minuteAngle">The current angle of the minute hand</param>
        /// <returns>Hour angle to the multiple of 30</returns>
        public static double ToExactHourDegrees(this int hour, double minuteAngle)
        {
            return hour.ToExactDegrees(30) + (30 / (360 / minuteAngle));
        }

        /// <summary>
        /// Rounds the minutes/seconds angle to multiple of 6 degrees so that minutes/seconds hand is snapped to a valid number from 0-59
        /// </summary>
        /// <param name="hour">The hour - 1 to 12</param>
        /// <returns>Minutes/Seconds angle to the multiple of 6</returns>
        public static double ToExactMinsSecsDegrees(this int minSec)
        {
            return minSec.ToExactDegrees(6);
        }

        #endregion
    }
}
