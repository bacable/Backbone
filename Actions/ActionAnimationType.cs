namespace Backbone.Actions
{
    /// <summary>
    /// Used for determining how the percentage is calculated to show easing or linear. Parametric is the same as EaseInAndOut for most, i.e.
    /// it slows down at the beginning and the end. Linear is the same speed throughout
    /// 
    /// Note: need to update ActionMath.cs when new ones are added.
    /// </summary>
    public enum ActionAnimationType
    {
        Linear = 0,
        Parametric = 1,
    }
}
