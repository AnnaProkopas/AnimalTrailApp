
public class NotSavable : Cake, ISavable
{
    private readonly TriggeredObjectType type = TriggeredObjectType.Default;
    public new TriggeredObjectType Type { get => type; }
}
