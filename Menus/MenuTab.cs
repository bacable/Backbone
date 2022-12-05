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

        public bool CanPrev => throw new NotImplementedException();

        public bool CanNext => throw new NotImplementedException();

        public string DisplayText { get; set; }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public object GetValue()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Prev()
        {
            throw new NotImplementedException();
        }

        public void SetValue(object value)
        {
            throw new NotImplementedException();
        }
    }
}
