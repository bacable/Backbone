namespace Backbone.Menus
{
    public interface IMenuItem
    {
        int ID { get; set; }
        MenuItemType Type { get; set; }
        string Name { get; set; }
        bool IsSelected { get; set; }
        bool CanPrev { get; }
        bool CanNext { get; }
        void Next();
        void Prev();
        void Click();
    }
}
