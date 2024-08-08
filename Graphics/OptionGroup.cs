using Backbone.Input;
using Backbone.Menus;
using Backbone.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Graphics
{
    public class OptionGroup : IGUI3D, IMenuUI, ITooltipProvider
    {
        List<MenuGraphic> Options { get; set; } = new List<MenuGraphic>();

        public int Count
        {
            get { return Options.Count; }
        }

        public MenuGraphic this[int index]
        {
            // get and set accessors
            get
            {
                return Options[index];
            }
        }

        public int SelectedIndex {
            get
            {
                return Options.FindIndex(x => x.Item.IsSelected);
            }
            set
            {
                for (int i = 0; i < Options.Count; i++)
                {
                    var option = Options[i];
                    option.Item.IsSelected = (i == value);
                }
            }
        }

        string selectedColor = "#FFFFFF";
        string unselectedColor = "#AAAAAA";

        // Kind of hacky, but we'll update this item without drawing it so the options have a parent to animate off of
        Movable3D parent;

        private OptionGroupSettings settings;

        private bool shouldStopUpdating = false;

        public MenuGraphic SelectedOption
        {
            get
            {
                return Options.Where(x => x.Item.IsSelected).FirstOrDefault();
            }
        }

        public IMenuItem SelectedItem
        {
            get
            {
                return Options.Where(x => x.Item.IsSelected).FirstOrDefault()?.Item;
            }
        }

        public OptionGroup(OptionGroupSettings settings)
        {
            // Determine parent node. Empty if nothing provided
            parent = (settings.ParentMovable != null) ? settings.ParentMovable : Movable3D.Empty();

            settings.Menu.Observer = this;

            this.settings = settings;

            UpdateOptions();
            UpdateTexts();

            UpdateColors(settings.SelectedColor, settings.UnselectedColor);

        }

        private void UpdateOptions()
        {
            Options.Clear();

            var menuTab = (settings.Menu.SelectedItem as MenuTab);
            var optionsToDisplay = menuTab != null ? menuTab.getItems() : settings.Menu.Items;

            var index = 0;
            foreach (var item in optionsToDisplay)
            {
                var option = new MenuGraphic();

                var transitionInAnim = settings.TransitionInAnims != null && settings.TransitionInAnims.Count > 0 ?
                    settings.TransitionInAnims[index % settings.TransitionInAnims.Count] : null;
                var transitionOutAnim = settings.TransitionOutAnims != null && settings.TransitionOutAnims.Count > 0 ?
                    settings.TransitionOutAnims[index % settings.TransitionOutAnims.Count] : null;

                option.Text = new TextGroup(new TextGroupSettings()
                {
                    Color = settings.UnselectedColor,
                    Id = index,
                    Parent = parent,
                    Position = new Vector3(settings.Position.X, settings.Position.Y - 90f * index, settings.Position.Z),
                    Scale = 80f,
                    Text = string.Empty,
                    TransitionInAnim = transitionInAnim,
                    TransitionOutAnim = transitionOutAnim,
                });

                option.Item = item;
                option.Item.IsSelected = index == 0;
                index += 1;
                Options.Add(option);
            }
        }


        public float Left
        {
            get
            {
                return SelectedOption.Text.Left;
            }
        }

        public float Right
        {
            get
            {
                return SelectedOption.Text.Right;
            }
        }

        public void UpdateSelectedOption()
        {
            UpdateTexts();
        }


        public void UpdateSelected(IMenuItem item)
        {
            Options.ForEach(option =>
            {
                option.Item.IsSelected = (option.Item == item);
            });
        }

        public void NextTab()
        {
            var menuTab = (settings.Menu.SelectedItem as MenuTab);

            if (menuTab != null)
            {
                settings.Menu.Next();
                UpdateOptions();
                UpdateTexts();
            }
        }

        public void PrevTab()
        {
            var menuTab = (settings.Menu.SelectedItem as MenuTab);

            if (menuTab != null)
            {
                settings.Menu.Prev();
                UpdateOptions();
                UpdateTexts();
            }
        }

        private void UpdateTexts()
        {
            Options.ForEach(x =>
            {
                if(!x.Text.Text.Equals(GetTextForOption(x)))
                {
                    x.Text.SetText(GetTextForOption(x));
                }
            });
        }

        public string GetTextForOption(MenuGraphic option)
        {
            switch (option.Item.Type)
            {
                case MenuItemType.Button:
                    return option.Item.DisplayText;
                case MenuItemType.OptionChooser:
                    return (!string.IsNullOrWhiteSpace(option.Item.DisplayText) ? option.Item.DisplayText + ": " :
                        string.Empty) + (option.Item as MenuOptionChooser).SelectedOption.Name;
                case MenuItemType.OptionSlider:
                    return option.Item.DisplayText + ": " + (option.Item as MenuOptionSlider).Value;
                default:
                    return string.Empty;
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateTexts();
            Options.ForEach(x => x.Text.Update(gameTime));
        }

        public void UpdateColors (string selected, string unselected)
        {
            selectedColor = selected;
            unselectedColor = unselected;
        }

        /// <summary>
        /// Mouse Handler for a single option.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="option"></param>
        /// <returns>Returns true if collision occurred, otherwise no</returns>
        public bool HandleMouseForOption(HandleMouseCommand command, MenuGraphic option)
        {
            Rectangle modifiedBoundingBox = new Rectangle((int)(option.Text.Position.X * command.Ratio.X),
                        (int)(option.Text.Position.Y * command.Ratio.Y),
                        (int)(option.Text.BoundingBox.Width * command.Ratio.X),
                        (int)(option.Text.BoundingBox.Height * command.Ratio.Y));

            if (Collision2D.IntersectRect(command.Viewport, command.WorldPosition, command.Ratio, modifiedBoundingBox))
            {
                UpdateSelected(option.Item);
                if (command.State == settings.ClickType)
                {
                    if (option.Item.Type == MenuItemType.Button)
                    {
                        shouldStopUpdating = !settings.UpdateAfterClick;
                        option.Item.Click();
                    }
                    else if (option.Item.Type == MenuItemType.OptionChooser || option.Item.Type == MenuItemType.OptionSlider)
                    {
                        shouldStopUpdating = !settings.UpdateAfterClick;
                        option.Item.Next();
                    }
                }
                UpdateSelectedOption();
                UpdateTexts();

                for (var i = 0; i < Options.Count; i++)
                {
                    Options[i].Item.IsSelected = (Options[i] == option);
                }

                return true;
            }
            return false;
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            if (!shouldStopUpdating && (command.HasMoved && command.State == MouseEvent.None || command.State == settings.ClickType))
            {
                int newSelection = -1;

                for (var i = 0; i < Options.Count; i++)
                {
                    var option = Options[i];
                    var collision = HandleMouseForOption(command, option);
                    if(collision)
                    {
                        break;
                    }
                }
            }
        }

        public void DrawOption(MenuGraphic option, Matrix view, Matrix projection)
        {
            if (option.Item.IsSelected)
            {
                option.Text.SetColor(selectedColor);
            }
            else
            {
                option.Text.SetColor(unselectedColor);
            }

            option.Text.Draw(view, projection);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Options.ForEach(x =>
            {
                DrawOption(x, view, projection);
            });
        }

        public void TransitionOut()
        {
            Options.ForEach(option => option.Text.TransitionOut());
        }

        public void TransitionIn()
        {
            Options.ForEach(option => option.Text.TransitionIn());
        }

        public string GetTooltipText(Vector2 position)
        {
            if(SelectedOption != null)
            {
                return SelectedOption.Text.Text;
            }

            return string.Empty;
        }
    }
}
