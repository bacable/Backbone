namespace Backbone.Menus
{
    public class MenuOption
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public MenuOption(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
