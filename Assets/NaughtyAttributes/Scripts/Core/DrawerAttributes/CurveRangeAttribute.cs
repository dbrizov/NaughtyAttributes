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

        /// <summary>
        /// Draw a curve within specified bounds
        /// </summary>
        /// <param name="width">the width of the curve graph</param>
        /// <param name="height">the height of the curve graph</param>
        public CurveRangeAttribute(float width, float height) : 
            this(width, height, EColor.Clear)
        {
            // EColor.Clear is used to mark default AnimationCurve color
        }

        /// <summary>
        /// Draw a curve within specified bounds and color
        /// </summary>
        /// <param name="width">the width of the curve graph</param>
        /// <param name="height">the height of the curve graph</param>
        /// <param name="color">the color of the curve</param>
        public CurveRangeAttribute(float width, float height, EColor color) :
            this(0, 0, width, height, color)
        {
        }

        /// <summary>
        /// Draw a curve within specified bounds and color
        /// </summary>
        /// <param name="x">the x value the curve graph is measured from</param>
        /// <param name="y">the y value the curve graph is measured from</param>
        /// <param name="width">the width of the curve graph</param>
        /// <param name="height">the height of the curve graph</param>
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
