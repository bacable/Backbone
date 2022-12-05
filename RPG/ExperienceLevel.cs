using System;
using System.Collections.Generic;
using System.Text;

namespace Backbone.RPG
{
    public class ExperienceLevel<T>
    {
        public int Id { get; set; }
        public List<T> LevelRewards { get; set; } = new List<T>();
        public int StartXP { get; set; }
        public int TargetXP { get; set; }
    }
}
