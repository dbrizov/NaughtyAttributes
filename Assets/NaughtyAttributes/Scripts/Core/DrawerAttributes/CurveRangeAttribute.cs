using System;
using UnityEngine;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CurveRangeAttribute : DrawerAttribute
    {
        public float x { get; private set; }
        public float y { get; private set; }
        public float width { get; private set; }
        public float height { get; private set; }
        public EColor color { get; private set; }

        public CurveRangeAttribute(float width, float height)
        {
            this.x = 0;
            this.y = 0;
            this.width = width;
            this.height = height;
            this.color = EColor.Green;
        }

        /// <summary>
        /// Draw a curve within specified width and height bounds
        /// </summary>
        /// <param name="x">the x value the rect is measured from</param>
        /// <param name="y">the y value the rect is measured from</param>
        /// <param name="width">the width of the rectangle</param>
        /// <param name="height">the height of the rectangle</param>
        /// <param name="color">the color of the curve</param>
        public CurveRangeAttribute(float x, float y, float width, float height, EColor color)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
        }
    }
}
