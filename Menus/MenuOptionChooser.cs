using Backbone.Localization;
using System;
using System.Collections.Generic;

namespace Backbone.Menus
{
    public class MenuOptionChooser : IMenuItem
    {
        const string TRUE_VALUE = "YES";
        const string FALSE_VALUE = "NO";

        #region Properties
        public int ID { get; set; }
        public int Rank { get; set; }
        public string Name { get; set; }
        public string DisplayText { get; set; }
        public int SelectedIndex { get; set; }
        public Action<string> OnChange { get; set; } = null;
        public MenuItemType Type { get; set; } = MenuItemType.OptionChooser;

        public bool CanNext
        {
            get
            {
                return SelectedIndex < (Options.Count - 1) || WrapAround;
            }
        }

        public bool CanPrev
        {
            get
            {
                return SelectedIndex > 0 || WrapAround;
            }
        }

        public MenuOption SelectedOption {
            get {
                if (SelectedIndex >= 0 && SelectedIndex < Options.Count) return Options[SelectedIndex];
                return null;
            }
        }
        public bool WrapAround { get; set; } = false;

        private List<MenuOption> Options { get; set; } = new List<MenuOption>();
        public bool IsSelected { get; set; } = false;
        #endregion Properties

        private bool isBoolValue = false;

        public MenuOptionChooser(string name, string displayText, bool isYesNo = false)
        {
            Name = name;
            DisplayText = displayText;

            if(isYesNo)
            {
                SetupYesNo();
            }
        }

        private void SetupYesNo()
        {
            isBoolValue = true;
            WrapAround = true;
            Options.Add(new MenuOption(CommonTerms.Yes, TRUE_VALUE));
            Options.Add(new MenuOption(CommonTerms.No, FALSE_VALUE));
        }

        public void Click()
        {

        }

        public void Add(MenuOption newOption)
        {
            Options.Add(newOption);
        }

        public void Add(string name, string value)
        {
            Add(new MenuOption(name, value));
        }

        public void Add(List<Tuple<string, string>> options)
        {
            options.ForEach(option =>
            {
                Add(new MenuOption(option.Item1, option.Item2));
            });
        }

        public void Next()
        {
            var oldSelectedIndex = SelectedIndex;

            SelectedIndex = (SelectedIndex < (Options.Count - 1) || WrapAround) ? (SelectedIndex + 1) % Options.Count : SelectedIndex;

            // If the option changed and we want something to change because of it,
            // call the OnChange event and pass in the current option text
            if (SelectedIndex != oldSelectedIndex && OnChange != null)
            {
                OnChange.Invoke(Options[SelectedIndex].Value);
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
                OnChange.Invoke(Options[SelectedIndex].Value);
            }
        }

        public void SetValue(object value)
        {
            if (isBoolValue)
            {
                SelectedIndex = ((bool)value == true) ? 0 : 1;
            }
            else
            {
                string matchString = (string)value;

                int foundIndex = -1;
                for (var i = 0; i < Options.Count; i++)
                {
                    if (Options[i].Value.Equals(matchString))
                    {
                        foundIndex = i;
                        break;
                    }
                }

                if (foundIndex > -1)
                {
                    SelectedIndex = foundIndex;
                }
            }
        }

        public object GetValue()
        {
            if(isBoolValue)
            {
                return Options[SelectedIndex].Value.Equals(TRUE_VALUE);
            }
            else
            {
                return Options[SelectedIndex].Value;
            }
        }
    }
}
