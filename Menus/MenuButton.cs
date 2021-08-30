using System;

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
    }
}
