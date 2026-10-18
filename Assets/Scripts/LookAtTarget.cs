using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LookAtTarget : MonoBehaviour
{
    public Camera cam;                 // boş bırakırsan bu objedeki kamerayı kullanır
    public GameObject defaultTarget;
    public GameObject currentTarget;

    // Sahnedeki tüm LookAtTarget'ların ortak listesi
    static readonly List<LookAtTarget> all = new List<LookAtTarget>();

    void OnEnable()  { all.Add(this); }
    void OnDisable() { all.Remove(this); }

    void Start()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (defaultTarget == null) defaultTarget = gameObject;
        if (currentTarget == null) currentTarget = defaultTarget;
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse != null && cam != null)
        {
            Vector2 mousePos = mouse.position.ReadValue();

            bool insideMyView = cam.pixelRect.Contains(mousePos);
            bool overUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

            if (insideMyView && !overUI)
            {
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    Ray ray = cam.ScreenPointToRay(mousePos);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        // Tüm kameraların hedefini aynı objeye ayarla
                        GameObject target = hit.collider.gameObject;
                        foreach (LookAtTarget l in all)
                            l.currentTarget = target;

                        Debug.Log("Tüm kameralar şuna kilitlendi: " + target.name);
                    }
                }
                else if (mouse.rightButton.wasPressedThisFrame)
                {
                    // Tüm kameraları kendi varsayılan hedefine döndür
                    foreach (LookAtTarget l in all)
                        l.currentTarget = l.defaultTarget;
                }
            }
        }

        if (currentTarget != null)
            transform.LookAt(currentTarget.transform);
    }
}