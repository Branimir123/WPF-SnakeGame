namespace SnakeGame.GameObjects.Contracts
{
    public interface IDestroyable
    {
        void Destroy();

        bool IsDestroyed { get; set; }
    }
}
