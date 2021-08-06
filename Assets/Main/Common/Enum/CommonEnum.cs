public enum LayerName
{
    Wall = 1 << 7,
    GrabTarget = 1 << 8,
    InteractTarget = 1 << 9,
    RaycastTarget = Wall + InteractTarget,
    EventSender = 1 << 10,
    EventReceiver = 1 << 11,
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
public enum BulletType
{
    WingmanAmmo,
}