using UnityEngine;

public class Zoom : MonoBehaviour
{
    Camera m_Cam;
    public bool CanZoom;
    public float MinFOV = 15;
    public float MaxZoomFOV = 60;
    [Range(0, 1)]
    public float CurrentZoom;
    public float Sensitivity = 1;


    void Awake()
    {
        // Get the camera on this gameObject and the defaultZoom.
        m_Cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (CanZoom)
        {
            // Update the currentZoom and the camera's fieldOfView.
            CurrentZoom += Input.mouseScrollDelta.y * Sensitivity * .05f;
            CurrentZoom = Mathf.Clamp01(CurrentZoom);
            m_Cam.fieldOfView = Mathf.Lerp(MaxZoomFOV, MinFOV, CurrentZoom);
        }
    }
}
