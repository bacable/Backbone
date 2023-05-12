using SharpDX.Direct3D9;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Menus
{
    public class MenuContainer : IMenuItem
    {
        public List<IMenuItem> Items { get; set; } = new List<IMenuItem>();
        public int SelectedIndex = 0;

        public MenuContainer(string name)
        {
            Name = name;
            Type = MenuItemType.Container;
        }
        public int ID { get; set; }

        public bool WrapAround { get; set; } = true;
        public string Name { get; set; }
        public MenuItemType Type { get; set; }
        public bool IsSelected { get; set; }


        public IMenuUI Observer { get; set; }

        public void SetValue(string name, object value)
        {
            var requestedItem = GetByName(name);
            if(requestedItem != null)
            {
                requestedItem.SetValue(value);
            }
        }

        public void Add(IMenuItem item)
        {
            Items.Add(item);

            // if this is the first item added, mark it as being selected
            if (Items.Count == 1)
            {
                Items[0].IsSelected = true;
            }
        }

        public void Click()
        {
            Items[SelectedIndex].Click();
        }

        public IMenuItem SelectedItem
        {
            get
            {
                return Items[SelectedIndex];
            }
        }

        public void Next()
        {
            var pastSelected = SelectedIndex;

            SelectedIndex = (SelectedIndex < (Items.Count - 1) || WrapAround) ? (SelectedIndex + 1) % Items.Count : SelectedIndex;

            if (pastSelected != SelectedIndex)
            {
                Items[pastSelected].IsSelected = false;
                Items[SelectedIndex].IsSelected = true;
            }

            Observer?.UpdateSelected(SelectedItem);
        }

        public void Prev()
        {
            var pastSelected = SelectedIndex;

            SelectedIndex -= 1;
            if (SelectedIndex < 0)
            {
                SelectedIndex = (WrapAround) ? Items.Count - 1 : 0;
            }

            if (pastSelected != SelectedIndex)
            {
                Items[pastSelected].IsSelected = false;
                Items[SelectedIndex].IsSelected = true;
            }

            Observer?.UpdateSelected(SelectedItem);
        }



        public void NextOption()
        {
            Items[SelectedIndex].Next();
            Observer?.UpdateSelectedOption();
        }

        public void PrevOption()
        {
            Items[SelectedIndex].Prev();
            Observer?.UpdateSelectedOption();
        }

        public void SetValue(object value)
        {
            // not needed for the container, at least not yet
        }

        // find item in container with the following name and then return its value
        public object GetValue(string name)
        {
            var item = GetByName(name);
            if(item != null)
            {
                return item.GetValue();
            }
            else
            {
                #if DEBUG
                    throw new System.Exception("Check the code, shouldn't be an issue");
                #endif
            }
        }

        public object GetValue()
        {
            // shouldn't be called
            return null;
        }

        public IMenuItem GetByName(string name)
        {
            IMenuItem returnItem = null;

            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                var requestedItem = item.GetByName(name);
                if (requestedItem != null)
                {
                    returnItem = requestedItem;
                    break;
                }
            }

            return returnItem;
        }

        public bool CanNext
        {
            get
            {
                return Items[SelectedIndex].CanNext;
            }
        }

        public bool CanPrev
        {
            get
            {
                return Items[SelectedIndex].CanPrev;
            }
        }

        public string DisplayText { get; set; }
    }
}
