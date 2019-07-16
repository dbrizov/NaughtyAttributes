using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class InlineButtonAttribute : DrawerAttribute
    {
        public readonly string MethodName;
        public readonly string Label;
        public readonly bool ExpandButton;

        /// <summary>
		/// Draws a button to the right of the Field/Property.
		/// </summary>
		/// <param name="methodName">Name of member method to call when the button is clicked.</param>
		/// <param name="label">Optional, label of the button.</param>
		/// <param name="expandButton">Optional, makes the button width to fit the size of the text or expand horizontally evenly with the field/property.</param>
		public InlineButtonAttribute(string methodName, string label = null, bool expandButton = false)
        {
            MethodName = methodName;
            Label = label;
            ExpandButton = expandButton;
        }
    }
}
