using Backbone.Actions;
using Backbone.Events;
using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using ProximityND.Backbone.Graphics;
using ProximityND.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Backbone.Graphics
{
    public class OptionGroup : IGUI3D, IMenuUI
    {
        List<MenuGraphic> Options { get; set; } = new List<MenuGraphic>();

        // Kind of hacky, but we'll update this item without drawing it so the options have a parent to animate off of
        Movable3D parent;

        private OptionGroupSettings settings;

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
        }

        private void UpdateOptions()
        {
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
                    Color = ProviderHub<string, ThemeElementType>.Request(ThemeElementType.TextColor),
                    Id = index,
                    Parent = parent,
                    Position = new Vector3(settings.Position.X, settings.Position.Y - 90f * index, settings.Position.Z),
                    Scale = 80f,
                    Text = string.Empty,
                    TransitionInAnim = transitionInAnim,
                    TransitionOutAnim = transitionOutAnim,
                });

                option.Item = item;
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
        }

        public void HandleMouse(HandleMouseCommand command)
        {

            if (command.State == MouseEvent.Moved || command.State == MouseEvent.Release)
            {
                int newSelection = -1;

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
            var selectedColor = ProviderHub<string, UIElementColorType>.Request(UIElementColorType.OptionGroupSelectedTextColor);
            var unselectedColor = ProviderHub<string, UIElementColorType>.Request(UIElementColorType.OptionGroupUnselectedTextColor);

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
