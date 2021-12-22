using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.Menus
{
    public class InputBoxSettings
    {
        /// <summary>
        /// If when created it starts in edit mode. Defaults to false
        /// </summary>
        public bool StartInEditMode { get; set; } = false;

        /// <summary>
        /// What value the input starts with
        /// </summary>
        public string StartingValue { get; set; } = string.Empty;


        /// <summary>
        /// Maximum number of characters for the text box
        /// </summary>
        public int MaxCharacters { get; set; } = 80;
    }
}
