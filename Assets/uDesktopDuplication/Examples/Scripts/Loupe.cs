using UnityEngine;

[RequireComponent(typeof(uDesktopDuplication.MonitorTexture))]
public class Loupe : MonoBehaviour
{
    private uDesktopDuplication.MonitorTexture uddTexture_;
    public float zoom = 3f;
    public float aspect = 1f;

    void Start()
    {
        uddTexture_ = GetComponent<uDesktopDuplication.MonitorTexture>();
        uddTexture_.useClip = true;
    }

    void LateUpdate()
    {
        CheckVariables();

        if (uDesktopDuplication.MonitorManager.cursorMonitorId < 0) return;
        uddTexture_.monitorId = uDesktopDuplication.MonitorManager.cursorMonitorId;

        // To get other monitor textures, set dirty flag.
        foreach (var target in uDesktopDuplication.MonitorManager.monitors)
        {
            target.CreateTextureIfNeeded();
            target.shouldBeUpdated = true;
        }

        var monitor = uddTexture_.monitor;
        var hotspotX = uDesktopDuplication.Lib.GetCursorHotSpotX();
        var hotspotY = uDesktopDuplication.Lib.GetCursorHotSpotY();
        var x = monitor.isCursorVisible ?
            (float)(monitor.cursorX + hotspotX) / monitor.width :
            (float)monitor.systemCursorX / monitor.width;
        var y = monitor.isCursorVisible ?
            (float)(monitor.cursorY + hotspotY) / monitor.height :
            (float)monitor.systemCursorY / monitor.height;
        var w = 1f / zoom;
        var h = w / aspect * monitor.aspect;
        x = Mathf.Clamp(x - w / 2, 0f, 1f - w);
        y = Mathf.Clamp(y - h / 2, 0f, 1f - h);
        uddTexture_.clipPos = new Vector2(x, y);
        uddTexture_.clipScale = new Vector2(w, h);
    }

    void CheckVariables()
    {
        if (zoom < 1f) zoom = 1f;
        if (aspect < 0.01f) aspect = 0.01f;
    }
}
