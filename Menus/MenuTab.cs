﻿using System;
using System.Collections.Generic;

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

        public IMenuItem GetByName(string name)
        {
            return Container.GetByName(name);
        }

        public object GetValue()
        {
            return Container.GetValue();
        }

        public void Next()
        {
            Container.Next();
        }

        public void Prev()
        {
            Container.Prev();
        }

        public void NextOption()
        {
            Container.NextOption();
        }

        public void PrevOption()
        {
            Container.PrevOption();
        }

        public void SetValue(object value)
        {
            throw new NotImplementedException();
        }

        internal List<IMenuItem> getItems()
        {
            return Container.Items;
        }
    }
}
