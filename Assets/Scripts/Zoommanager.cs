using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public Camera[] cameras;          // Inspector'dan iki kamerayı ekle
    public float zoomSpeed = 40f;     // basılı tutunca saniyede değişecek FOV miktarı
    public float minFov = 20f;        // en fazla yakınlaşma
    public float maxFov = 90f;        // en fazla uzaklaşma
    public float smooth = 10f;        // yumuşaklık (büyüdükçe hızlanır)

    float[] targetFov;
    bool zoomingIn;
    bool zoomingOut;

    void Start()
    {
        targetFov = new float[cameras.Length];
        for (int i = 0; i < cameras.Length; i++)
            targetFov[i] = cameras[i].fieldOfView;
    }

    // UI butonları için (Pointer Down / Pointer Up ile bağlanacak)
    public void StartZoomIn()  { zoomingIn = true; }
    public void StopZoomIn()   { zoomingIn = false; }
    public void StartZoomOut() { zoomingOut = true; }
    public void StopZoomOut()  { zoomingOut = false; }

    void Update()
    {
        bool inHeld = zoomingIn;
        bool outHeld = zoomingOut;

        // Klavye: E yakınlaştır, Q uzaklaştır (basılı tut)
        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.eKey.isPressed) inHeld = true;
            if (kb.qKey.isPressed) outHeld = true;
        }

        float delta = 0f;
        if (inHeld)  delta -= zoomSpeed * Time.deltaTime;
        if (outHeld) delta += zoomSpeed * Time.deltaTime;

        for (int i = 0; i < cameras.Length; i++)
        {
            targetFov[i] = Mathf.Clamp(targetFov[i] + delta, minFov, maxFov);
            cameras[i].fieldOfView = Mathf.Lerp(cameras[i].fieldOfView, targetFov[i], Time.deltaTime * smooth);
        }
    }
}