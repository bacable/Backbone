using System;
using System.Diagnostics;

namespace Backbone.Menus
{
    public class MenuButton : IMenuItem
    {
        public string Name { get; set; }
        public MenuItemType Type { get; set; } = MenuItemType.Button;
        public bool IsSelected { get; set; }
        public int ID { get; set; }

        public Action OnClick;

        public MenuButton(string name, Action onClickAction)
        {
            Name = name;
            OnClick = onClickAction;
        }

        public void Next()
        {
        }

        public void Prev()
        {
        }

        public void Click()
        {
            OnClick?.Invoke();
        }

        public void SetValue(object value)
        {
            // not needed right now, maybe change text of button later?
        }

        public object GetValue()
        {
            throw new NotImplementedException();
        }

        public bool CanNext
        {
            get
            {
                return false;
            }
        }

        public bool CanPrev
        {
            get
            {
                return false;
            }
        }

        // TODO: make them separate
        public string DisplayText { get { return Name; } set { } }
    }
}
