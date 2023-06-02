public class GrayMouse : Mouse, ISavable
{
    private readonly TriggeredObjectType type = TriggeredObjectType.GrayMouse;
    public new TriggeredObjectType Type { get => type; }
}
