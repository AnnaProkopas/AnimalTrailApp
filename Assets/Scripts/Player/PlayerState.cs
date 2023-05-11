public enum PlayerState
{
    Idle = 0,
    LookAround,
    Walk,
    Attack = 3,
    Wounded = Attack,
    Dying,
    Dead = 5,
    Sleep,
    Hidden
}