namespace Backbone.Menus
{
    /// <summary>
    /// Added to OptionGroup and anything else that might want to know and update based on what changes have happened to the menu data when it happens
    /// </summary>
    public interface IMenuUI
    {
        void UpdateSelected(IMenuItem item);
        void UpdateSelectedOption();
    }
}
