using Backbone.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Graphics
{
    public class ScrollableOptionGroupPanel : IGUI3D
    {
        private OptionGroup optionGroup;
        private List<MenuGraphic> visibleOptions = new List<MenuGraphic>();
        private int maxVisibleItems;
        private int firstVisibleIndex = 0;
        private float scrollbarPosition; // A percentage (0 to 1) indicating scrollbar's position
        private Movable3D movable;
        private Rectangle scrollbarRect; // Adjust the size and position based on your needs

        public ScrollableOptionGroupPanel(OptionGroupSettings settings, int maxVisibleItems = 20)
        {
            movable = Movable3D.Empty();
            settings.ParentMovable = movable;

            this.optionGroup = new OptionGroup(settings);
            this.maxVisibleItems = maxVisibleItems;
            RefreshVisibleOptions();
            // Initialize scrollbar rectangle here if necessary
        }

        private void RefreshVisibleOptions()
        {
            visibleOptions.Clear();
            for (int i = 0; i < maxVisibleItems && i + firstVisibleIndex < optionGroup.Count; i++)
            {
                visibleOptions.Add(optionGroup[i + firstVisibleIndex]);
            }
        }

        public void ScrollUp()
        {
            if (firstVisibleIndex > 0)
            {
                firstVisibleIndex--;
                RefreshVisibleOptions();
                UpdateScrollPosition();
            }
        }

        public void ScrollDown()
        {
            if (firstVisibleIndex + maxVisibleItems < optionGroup.Count)
            {
                firstVisibleIndex++;
                RefreshVisibleOptions();
                UpdateScrollPosition();
            }
        }

        public void Scroll(int amount)
        {

        }

        private void UpdateScrollPosition()
        {
            scrollbarPosition = (float)firstVisibleIndex / (optionGroup.Count - maxVisibleItems);
            // Update scrollbarRect position here based on scrollbarPosition
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            // Note: HandleMouse logic for visibleOptions instead of the whole optionGroup
            foreach (var option in visibleOptions)
            {
                // Handle mouse interaction for each visible option.
                // If you need to extend the logic to the visible options, do it here.
                optionGroup.HandleMouseForOption(command, option);
            }

            // Handle scrollbar dragging logic here
            // Update firstVisibleIndex and scrollbarPosition as necessary
        }

        public void Update(GameTime gameTime)
        {
            foreach (var option in visibleOptions)
            {
                option.Text.Update(gameTime);
            }
            // Additional updates for scrollbar or other UI elements if necessary
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (var option in visibleOptions)
            {
                optionGroup.DrawOption(option, view, projection);
            }
            // Draw scrollbar here
        }

        public void TransitionIn()
        {
        }

        public void TransitionOut()
        {
        }
    }
}
