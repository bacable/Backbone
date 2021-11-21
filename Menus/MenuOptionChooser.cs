using System;
using System.Collections.Generic;

namespace Backbone.Menus
{
    public class MenuOptionChooser : IMenuItem
    {
        public int ID { get; set; }
        public int Rank { get; set; }
        public string Name { get; set; }
        public int SelectedIndex { get; set; }
        public Action OnChange { get; set; } = null;
        public MenuItemType Type { get; set; } = MenuItemType.OptionChooser;

        public bool CanNext
        {
            get
            {
                return SelectedIndex > 0 || WrapAround;
            }
        }

        public bool CanPrev
        {
            get
            {
                return SelectedIndex < (Options.Count - 2) || WrapAround;
            }
        }

        public MenuOption SelectedOption {
            get {
                if (SelectedIndex >= 0 && SelectedIndex < Options.Count) return Options[SelectedIndex];
                return null;
            }
        }
        public bool WrapAround { get; set; } = false;

        public List<MenuOption> Options { get; set; } = new List<MenuOption>();
        public bool IsSelected { get; set; } = false;

        public MenuOptionChooser(string name, bool isYesNo = false)
        {
            Name = name;

            if(isYesNo)
            {
                SetupYesNo();
            }
        }

        private void SetupYesNo()
        {
            WrapAround = true;
            Options.Add(new MenuOption() { Name = "YES" }); // TODO: Add localization to Backbone library so this can be localized
            Options.Add(new MenuOption() { Name = "NO" });
        }

        public void Click()
        {

        }

        public void Next()
        {
            var oldSelectedIndex = SelectedIndex;

            SelectedIndex = (SelectedIndex < (Options.Count - 1) || WrapAround) ? (SelectedIndex + 1) % Options.Count : SelectedIndex;

            // If the option changed and we want something to change because of it,
            // call the OnChange event and pass in the current option text
            if (SelectedIndex != oldSelectedIndex && OnChange != null)
            {
                OnChange.Invoke();
            }
        }
        public void Prev()
        {
            var oldSelectedIndex = SelectedIndex;

            SelectedIndex -= 1;
            if(SelectedIndex < 0)
            {
                SelectedIndex = (WrapAround) ? Options.Count - 1 : 0;
            }

            // If the option changed and we want something to change because of it,
            // call the OnChange event and pass in the current option text
            if (SelectedIndex != oldSelectedIndex && OnChange != null)
            {
                OnChange.Invoke();
            }
        }
    }
}
