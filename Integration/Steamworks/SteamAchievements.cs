using System;
using System.Collections.Generic;
using Backbone.Achievements;
using Steamworks;

namespace Backbone.Integration.Steamworks
{
    public class SteamAchievements : IAchievementManager
    {
        public void GainAchievement(IAchievement achievement)
        {
            SteamUserStats.GetAchievement(achievement.Id, out bool completed);
            if(!completed)
            {
                SteamUserStats.SetAchievement(achievement.Id);
                SteamUserStats.StoreStats();
            }

        }

        public int ReportStat(string statId, int amountChanged)
        {
            SteamUserStats.GetStat(statId, out int statValue);
            
            statValue += amountChanged;
            
            SteamUserStats.SetStat(statId, statValue);
            SteamUserStats.StoreStats();

            return statValue;
        }

        public void ResetAllStats(bool resetAchievementsAlso)
        {
            SteamUserStats.ResetAllStats(resetAchievementsAlso);
        }
    }
}
