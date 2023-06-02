
public class Garbage : Cake, ISavable
{
    private readonly TriggeredObjectType type = TriggeredObjectType.Garbage;
    public new TriggeredObjectType Type { get => type; }
}
