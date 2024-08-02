namespace Backbone.AI
{
    public interface IMoveState<T>
    {
        T data { get; set; }
        float Score { get; set; }
    }
}
