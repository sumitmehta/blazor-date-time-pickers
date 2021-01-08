namespace SumitMehta.BlazorComponents.Shared.TimePickerComponent.Models
{
    /// <summary>
    /// Represents the bounding rectangle of an element in the DOM
    /// </summary>
    public class BoundingClientRect
    {
        /// <summary>
        /// The X element of the element
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// The Y element of the element
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The width of the element
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The height of the element
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// The absolute top metric for the element
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// The absolute right metric for the element
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// The absolute bottom metric for the element
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// The absolute left metric for the element
        /// </summary>
        public double Left { get; set; }
    }
}