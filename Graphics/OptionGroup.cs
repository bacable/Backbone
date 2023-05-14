using Backbone.Events;
using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Graphics
{
    public class OptionGroup : IGUI3D, IMenuUI
    {
        List<MenuGraphic> Options { get; set; } = new List<MenuGraphic>();

        private IGUI3D LeftArrowButton = null;
        private IGUI3D RightArrowButton = null;

        string selectedColor = "#FFFFFF";
        string unselectedColor = "#AAAAAA";

        // Kind of hacky, but we'll update this item without drawing it so the options have a parent to animate off of
        Movable3D parent;

        private TextGroup tabNameText = null;

        private OptionGroupSettings settings;

        private bool hasTabs = false;

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

            hasTabs = (settings.Menu.SelectedItem as MenuTab) != null;
            if (hasTabs)
            {
                // Initialize tabNameText
                tabNameText = new TextGroup(new TextGroupSettings()
                {
                    Color = settings.TabHeaderColor,
                    Parent = parent,
                    Position = new Vector3(settings.Position.X, settings.Position.Y + 100f, settings.Position.Z), // Position it above other options
                    Scale = 80f,
                    Text = string.Empty,
                });

                LeftArrowButton = settings.LeftArrowButton;
                RightArrowButton = settings.RightArrowButton;
            }

            UpdateOptions();
            UpdateTexts();

            UpdateColors(settings.SelectedColor, settings.UnselectedColor, settings.TabHeaderColor);

        }

        private void UpdateOptions()
        {
            Options.Clear();

            var menuTab = (settings.Menu.SelectedItem as MenuTab);
            var optionsToDisplay = menuTab != null ? menuTab.getItems() : settings.Menu.Items;

            // Update tab name
            if (menuTab != null)
            {
                tabNameText.SetText(menuTab.DisplayText);
            }

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
                    Color = settings.TabHeaderColor,
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
            if(hasTabs)
            {
                settings.Menu.Next();
                UpdateOptions();
                UpdateTexts();
            }
        }

        public void PrevTab()
        {
            if(hasTabs)
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
                x.Text.SetText(GetTextForOption(x));
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
            Options.ForEach(x => x.Text.Update(gameTime));
            if(hasTabs)
            {
                LeftArrowButton.Update(gameTime);
                RightArrowButton.Update(gameTime);
                tabNameText.Update(gameTime);
            }
        }

        public void UpdateColors (string selected, string unselected, string tabHeader)
        {
            selectedColor = selected;
            unselectedColor = unselected;
            if(hasTabs)
            {
                tabNameText.SetColor(tabHeader);
            }
        }

        public void HandleMouse(HandleMouseCommand command)
        {

            if (command.State == MouseEvent.Moved || command.State == MouseEvent.Release)
            {
                int newSelection = -1;

                if(hasTabs)
                {
                    LeftArrowButton.HandleMouse(command);
                    RightArrowButton.HandleMouse(command);
                }

                for (var i = 0; i < Options.Count; i++)
                {
                    var option = Options[i];

                    Rectangle modifiedBoundingBox = new Rectangle((int)(option.Text.Position.X * command.Ratio.X),
                        (int)(option.Text.Position.Y * command.Ratio.Y),
                        (int)(option.Text.BoundingBox.Width * command.Ratio.X),
                        (int)(option.Text.BoundingBox.Height * command.Ratio.Y));

                    if (Collision2D.IntersectRect(command.Viewport, command.WorldPosition, command.Ratio, modifiedBoundingBox))
                    {
                        UpdateSelected(option.Item);
                        newSelection = i;
                        if (command.State == MouseEvent.Release)
                        {
                            if (option.Item.Type == MenuItemType.Button)
                            {
                                option.Item.Click();
                            }
                            else if (option.Item.Type == MenuItemType.OptionChooser || option.Item.Type == MenuItemType.OptionSlider)
                            {
                                option.Item.Next();
                            }
                        }
                        UpdateSelectedOption();
                        UpdateTexts();
                        break;
                    }
                }

                if (newSelection > -1)
                {
                    for (var i = 0; i < Options.Count; i++)
                    {
                        Options[i].Item.IsSelected = i == newSelection;
                    }
                }
            }
        }



        public void Draw(Matrix view, Matrix projection)
        {
            Options.ForEach(x =>
            {
                if (x.Item.IsSelected)
                {
                    x.Text.SetColor(selectedColor);
                }
                else
                {
                    x.Text.SetColor(unselectedColor);
                }

                x.Text.Draw(view, projection);
            });

            if(hasTabs)
            {
                tabNameText.Draw(view, projection);
                LeftArrowButton.Draw(view, projection);
                RightArrowButton.Draw(view, projection);
            }
        }

        public void TransitionOut()
        {
            Options.ForEach(option => option.Text.TransitionOut());
        }

        public void TransitionIn()
        {
            Options.ForEach(option => option.Text.TransitionIn());
        }
    }
}
