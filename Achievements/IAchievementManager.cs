namespace Backbone.Achievements
{
    public interface IAchievementManager
    {
        void GainAchievement(IAchievement achievement);
        int ReportStat(string statId, int amountChanged);
        void ResetAllStats(bool resetAchievementsAlso);
    }
}
