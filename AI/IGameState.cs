namespace Backbone.AI
{
    public interface IGameState<T>
    {
        float Score { get; set; }
        void ApplyMove(IMoveState<T> move);
    }
}
