using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backbone.RPG
{
    public static class ExperienceManager<T>
    {
        public static List<ExperienceLevel<T>> Levels = new List<ExperienceLevel<T>>();

        public static void AddLevel(ExperienceLevel<T> newLevel)
        {
            Levels.Add(newLevel);
        }

        public static (ExperienceLevel<T> newLevel, int newXP, List<T> rewards) GainXP(ExperienceLevel<T> currentLevel, int currentXP, int amount)
        {
            var rewards = new List<T>();
            var newXP = currentXP += amount;
            while(currentXP > currentLevel.TargetXP)
            {
                var nextLevelId = currentLevel.Id + 1;
                var nextLevel = Levels.Where(x => x.Id == nextLevelId).FirstOrDefault();
                if(nextLevel != null)
                {
                    rewards.AddRange(nextLevel.LevelRewards);
                    currentLevel = nextLevel;
                }
                else
                {
                    break;
                }
            }
            return (currentLevel, newXP, rewards);
        }
    }
}
