using Backbone.Graphics;
using Backbone.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Backbone.UI
{
    public struct TooltipColors
    {
        public Color Background;
        public Color Border;
        public Color Text;
    }

    public class TooltipManager
    {
        private IScreen screen; // Currently active screen
        private string tooltipText; // Tooltip text associated with the element
        private float hoverTime; // Time the mouse has hovered over the element
        private bool isTooltipVisible; // Flag indicating if the tooltip is currently visible
        private TooltipColors colors;
        private Vector2 position;
        private SpriteFont font;

        private const float TooltipDelay = 2f; // Delay in seconds before showing the tooltip

        public TooltipManager(IScreen screen, TooltipColors colors, SpriteFont font)
        {
            this.screen = screen;
            this.colors = colors;
            tooltipText = string.Empty;
            hoverTime = 0f;
            isTooltipVisible = false;
            this.font = font;
        }

        public void UpdateColors(TooltipColors newColors)
        {
            colors = newColors;
        }

        public void Update(GameTime gameTime)
        {
            if(!string.IsNullOrWhiteSpace(tooltipText))
            {
                hoverTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                var hoverT = hoverTime;
                VariableMonitor.Report(x => hoverT);

                // Check if the hover time exceeds the delay to show the tooltip
                if (hoverTime >= TooltipDelay)
                {
                    // Show the tooltip
                    ShowTooltip();
                }
            }
            else
            {
                // Reset the hover time and hide the tooltip
                hoverTime = 0f;
                HideTooltip();
            }
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            string newTooltipText = GetTooltipAtMousePosition(command.MousePosition);
            if (string.IsNullOrWhiteSpace(newTooltipText))
            {
                tooltipText = string.Empty;
                HideTooltip();
            }
            else
            {
                if(!tooltipText.Equals(newTooltipText))
                {
                    hoverTime = 0f;
                    isTooltipVisible = false;
                    tooltipText = newTooltipText;
                }
                position = command.MousePosition;
            }

        }

        private void ShowTooltip()
        {
            // Create and show the tooltip UI element
            // This will depend on your UI framework and rendering system

            isTooltipVisible = true;
        }

        private void HideTooltip()
        {
            // Hide and destroy the tooltip UI element
            // This will depend on your UI framework and rendering system

            isTooltipVisible = false;
        }

        private string GetTooltipAtMousePosition(Vector2 mousePosition)
        {
            // Iterate through the GUI elements in the active screen
            // and check if the mouse position intersects with any of them
            foreach (IGUI3D element in screen.GuiElements)
            {
                ITooltipProvider provider = element as ITooltipProvider;

                if(provider != null)
                {
                    var text = provider.GetTooltipText(mousePosition);
                    if(!string.IsNullOrWhiteSpace(text))
                    {
                        return text;
                    }
                }
            }

            return string.Empty;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                if (isTooltipVisible)
                {
                    spriteBatch.DrawString(font, tooltipText, position, colors.Text, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

}
