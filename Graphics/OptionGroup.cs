using Backbone.Input;
using Backbone.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Graphics
{
    public class OptionGroup : IGUI3D, IMenuUI
    {
        List<MenuGraphic> Options { get; set; } = new List<MenuGraphic>();

        // Kind of hacky, but we'll update this item without drawing it so the options have a parent to animate off of
        Movable3D parent;

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

        public OptionGroup(Vector3 position, MenuContainer menu, Movable3D parentMovable = null)
        {
            // Determine parent node. Empty if nothing provided
            parent = (parentMovable != null) ? parentMovable : Movable3D.Empty();

            var index = 0;
            foreach(var item in menu.Items)
            {
                var option = new MenuGraphic();
                option.Text = new TextGroup(index, parent, new Vector3(position.X, position.Y - 90f * index, position.Z), 80f);
                option.Item = item;
                index += 1;
                Options.Add(option);
            }

            menu.Observer = this;

            UpdateTexts();
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
                switch (x.Item.Type)
                {
                    case MenuItemType.Button:
                        x.Text.SetText(x.Item.DisplayText);
                        break;
                    case MenuItemType.OptionChooser:
                        x.Text.SetText(x.Item.DisplayText + ": " + (x.Item as MenuOptionChooser).SelectedOption.Name);
                        break;
                    case MenuItemType.OptionSlider:
                        x.Text.SetText(x.Item.DisplayText + ": " + (x.Item as MenuOptionSlider).Value);
                        break;
                    default:
                        break;
                }
            });
        }



        public void Update(GameTime gameTime)
        {
            Options.ForEach(x => x.Text.Update(gameTime));
        }

        public void HandleMouse(HandleMouseCommand command)
        {
            
        }

        public void Draw(Matrix view, Matrix projection)
        {

            Options.ForEach(x =>
            {
                if (x.Item.IsSelected)
                {
                    x.Text.SetColor(ColorType.LightOrange);
                }
                else
                {
                    x.Text.SetColor(ColorType.DefaultText);
                }

                x.Text.Draw(view, projection);
            });
        }

        public void TransitionOut()
        {
        }

        public void TransitionIn()
        {
        }
    }
}
