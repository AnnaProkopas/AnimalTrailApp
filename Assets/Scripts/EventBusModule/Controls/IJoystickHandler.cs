
namespace EventBusModule.Controls
{
    public interface IJoystickHandler : IGlobalSubscriber
    {
        void HandleDragJoystick(float horizontal, float vertical);
    }
}