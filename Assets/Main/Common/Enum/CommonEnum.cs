public enum LayerName
{
    Wall = 1 << 7,
    GrabTarget = 1 << 8,
    InteractTarget = 1 << 9,
    RaycastTarget = Wall + InteractTarget,
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    Forward,
    Backward,
}