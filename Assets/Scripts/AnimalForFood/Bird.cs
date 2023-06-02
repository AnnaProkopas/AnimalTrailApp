public class Bird : Mouse, ISavable
{
    private readonly TriggeredObjectType type = TriggeredObjectType.Bird;
    public new TriggeredObjectType Type { get => type; }
}
