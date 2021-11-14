using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ProximityND.Backbone.Graphics
{
    /// <summary>
    /// Whatever is passed into it is displayed as "variableName: value" in a list at a location on the screen each frame. Each reporting has an expiration associated with it, so it won't stay up there forever.
    /// </summary>
    public static class VariableMonitor
    {
        public static Dictionary<string, Tuple<object, float>> Monitored = new Dictionary<string, Tuple<object, float>>();

        // Settings for what is drawn on screen. Can set by calling Init
        private static Color color;
        private static SpriteFont font;
        private static float height;
        private static float scale;
        private static Vector2 position;

        public static void Init(VariableMonitorSettings settings)
        {
            color = settings.Color;
            font = settings.Font;
            height = settings.Height;
            scale = settings.Scale;
            position = settings.Position;
        }

        /// <summary>
        /// A very convoluted method using reflection to get both the variable name and the variable value, but it works (might be too slow if used a bunch). Use "VariableMonitor.Report(x => variable);"
        /// </summary>
        /// <param name="f"></param>
        /// <param name="timeout"></param>
        public static void Report(Expression<Func<string, string>> f, float timeout = float.MaxValue)
        {
            var memberExpression = (f.Body as MemberExpression);
            SetMonitoredValue(timeout, memberExpression);
        }

        public static void Report(Expression<Func<string, bool>> f, float timeout = float.MaxValue)
        {
            var memberExpression = (f.Body as MemberExpression);
            SetMonitoredValue(timeout, memberExpression);
        }

        public static void Report(Expression<Func<string, int>> f, float timeout = float.MaxValue)
        {
            var memberExpression = (f.Body as MemberExpression);
            SetMonitoredValue(timeout, memberExpression);
        }

        public static void Report(Expression<Func<string, float>> f, float timeout = float.MaxValue)
        {
            var memberExpression = (f.Body as MemberExpression);
            SetMonitoredValue(timeout, memberExpression);
        }

        private static void SetMonitoredValue(float timeout, MemberExpression memberExpression)
        {
            string variableName;
            object value;
            ExtractVariableNameAndValue(memberExpression, out variableName, out value);
            Monitored[variableName] = new Tuple<object, float>(value, timeout);
        }

        private static void ExtractVariableNameAndValue(MemberExpression memberExpression, out string variableName, out object value)
        {
            variableName = memberExpression.Member.Name;

            // try to cast as if a local variable were passed in first
            var ce = memberExpression.Expression as ConstantExpression;
            if (ce != null)
            {
                var fieldInfo = ce.Value.GetType().GetField(variableName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                value = fieldInfo.GetValue(ce.Value);
            }
            else // must be a property on an object, currently not supported (can't figure it out yet)
            {
                value = "???";
                /*
                var fieldInfo = memberExpression.Member.DeclaringType;
                var field = fieldInfo.GetProperty(variableName);

                value = field.GetValue(memberExpression.Expression);*/
            }
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var kvp in Monitored.OrderBy(x => x.Key))
            {
                var timeLeft = kvp.Value.Item2 - (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeLeft < 0)
                {
                    Monitored.Remove(kvp.Key);
                    continue;
                }
                else
                {
                    Monitored[kvp.Key] = new Tuple<object, float>(kvp.Value.Item1, timeLeft);
                }
            }
        }


        public static void Draw(SpriteBatch spriteBatch)
        {
            if(font == null)
            {
                throw new Exception("Please call Init first and set the SpriteFont");
            }

            var currentPosition = position;

            foreach (var kvp in Monitored.OrderBy(x => x.Key))
            {
                var textLine = $"{kvp.Key}: {kvp.Value.Item1}";
                spriteBatch.DrawString(font, textLine, currentPosition, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                currentPosition += new Vector2(0, height);
            }
        }
    }
}
