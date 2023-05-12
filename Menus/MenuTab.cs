using System;

namespace Backbone.Menus
{
    public class MenuTab : IMenuItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public MenuContainer Container { get; set; }
        public MenuItemType Type { get; set; } = MenuItemType.Tab;
        public bool IsSelected { get; set; }

        public bool CanPrev => true;

        public bool CanNext => true;

        public string DisplayText { get; set; } = string.Empty;

        public void Click()
        {
        }

        public object GetValue()
        {
            return Container.GetValue();
        }

        public void Next()
        {
        }

        public void Prev()
        {
        }

        public void SetValue(object value)
        {
            throw new NotImplementedException();
        }
    }
}
