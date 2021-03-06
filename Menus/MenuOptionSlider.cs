using System;

namespace Backbone.Menus
{
    public class MenuOptionSlider : IMenuItem
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int StepAmount { get; set; }
        public int Value { get; set; }

        public Action OnChange { get; set; } = null;
        public MenuItemType Type { get; set; } = MenuItemType.OptionSlider;
        public bool IsSelected { get; set; } = false;

        public bool CanPrev
        {
            get
            {
                return Value > Minimum;
            }
        }

        public bool CanNext
        {
            get
            {
                return Value < Maximum;
            }
        }

        public MenuOptionSlider(string name, int min, int max, int step)
        {
            Name = name;
            Minimum = min;
            Maximum = max;
            StepAmount = step;
            Value = min;
        }

        public void Next()
        {
            var oldValue = Value;
            Value = Math.Min(Maximum, Value + StepAmount);

            if (Value != oldValue && OnChange != null)
            {
                OnChange.Invoke();
            }
        }

        public void Prev()
        {
            var oldValue = Value;
            Value = Math.Max(Minimum, Value - StepAmount);

            if(Value != oldValue && OnChange != null)
            {
                OnChange.Invoke();
            }
        }

        public void Click()
        {
            throw new NotImplementedException();
        }
    }
}
