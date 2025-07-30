namespace Game.Scripts.UI
{
    public interface IMessageActionEvents : IGlobalSubscriber
    {
        void Show(MessageAction action);
    }
}