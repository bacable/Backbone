namespace Backbone.Menus
{
    public interface IMenuItem
    {
        int ID { get; set; }
        MenuItemType Type { get; set; }
        string Name { get; set; }
        string DisplayText { get; set; }
        bool IsSelected { get; set; }
        bool CanPrev { get; }
        bool CanNext { get; }
        void Next();
        void Prev();
        void Click();
        // TODO: Change IMenuItem to a Template so instead of object we can use T
        IMenuItem GetByName(string name);
        public object GetValue();
        void SetValue(object value);
    }
}
