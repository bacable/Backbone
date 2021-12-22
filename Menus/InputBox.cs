using Backbone.Events;
using Backbone.Input;
using System;

namespace Backbone.Menus
{
    /// <summary>
    /// Data backend for InputBox (needs GUI element attached to render it). 
    /// Events Subscribed:
    /// BackboneEvent.InputBoxAdd - modifies value by appending to it
    /// BackboneEvent.InputBoxRemoveLast - removes the final character from the end
    ///                                    (like if Backspace key is hit).
    /// </summary>
    public class InputBox : IMenuItem, ISubscriber<BackboneEvent>
    {
        public InputBox(InputBoxSettings settings)
        {
            IsEditing = settings.StartInEditMode;
            Value = settings.StartingValue;
            MaxCharacters = settings.MaxCharacters;

            InputHelper.IsTextTyping = IsEditing;

            //PubHub<BackboneEvent>.Sub(BackboneEvent.InputBoxAdd, this);
            //PubHub<BackboneEvent>.Sub(BackboneEvent.InputBoxRemoveLast, this);
        }

        #region Properties
        public int ID { get; set; }
        public MenuItemType Type { get; set; } = MenuItemType.InputBox;
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool CanPrev { get { return false; } }
        public bool CanNext { get { return false; } }
        public Action<string> OnChange { get; set; } = null;

        private string _value;
        public string Value { get { return _value; }
            set
            {
                var newValue = value;
                if(MaxCharacters > 0 && value.Length > MaxCharacters)
                {
                    newValue = value.Substring(0, MaxCharacters);
                }
                _value = newValue;
            }
        }
        public bool IsEditing { get; set; } = false;
        public int MaxCharacters { get; set; }
        #endregion


        public void Click()
        {
            IsEditing = !IsEditing;

            InputHelper.IsTextTyping = IsEditing;
        }

        public void Next()
        {
            // Does nothing for input boxes, at least not yet
        }

        public void Prev()
        {
            // Does nothing for input boxes, at least not yet
        }

        private void addToValue(string valueToAppend)
        {
            if(!string.IsNullOrEmpty(valueToAppend))
            {
                Value += valueToAppend;
            }
        }

        private void removeLastChar()
        {
            if(Value.Length > 0)
            {
                Value = Value.Substring(0, Value.Length - 1);
            }
        }

        public void HandleEvent(BackboneEvent eventType, object payload)
        {
            switch (eventType)
            {
                case BackboneEvent.InputBoxAdd:
                    addToValue((string)payload);
                    break;
                case BackboneEvent.InputBoxRemoveLast:
                    removeLastChar();
                    break;
                default:
                    break;
            }
        }
    }
}
