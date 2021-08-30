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

        public void Add(IMenuItem item)
        {
            Items.Add(item);

            // if this is the first item added, mark it as being selected
            if(Items.Count == 1)
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

        public IMenuItem Get(string name)
        {
            return Items.FirstOrDefault(x => x.Name == name);
        }

        public void Next()
        {
            var pastSelected = SelectedIndex;

            SelectedIndex = (SelectedIndex < (Items.Count - 1) || WrapAround) ? (SelectedIndex + 1) % Items.Count : SelectedIndex;

            if(pastSelected != SelectedIndex)
            {
                Items[pastSelected].IsSelected = false;
                Items[SelectedIndex].IsSelected = true;
            }
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
        }



        public void NextOption()
        {
            Items[SelectedIndex].Next();
        }

        public void PrevOption()
        {
            Items[SelectedIndex].Prev();
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
    }
}
