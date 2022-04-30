using UnityEngine;

public enum EditorState { normal, placingItem };

public class ItemHandler : MonoBehaviour
{
    public static ItemHandler Instance;
    EditorState m_CurrentState;
    GameObject m_CurrentObjectToPlace;
    Camera m_Cam;
    RaycastHit m_HitInfo;

    void Awake()
    {
        Instance = this;
    }

     void Start()
    {
        m_Cam = GetComponent<Camera>();
    }

    public void NormalMode()
    {
        m_CurrentState = EditorState.normal;
    }

    public void PlaceMode(GameObject i_ItemToPlace)
    {
        if (m_CurrentState != EditorState.placingItem)
            m_CurrentState = EditorState.placingItem;
        m_CurrentObjectToPlace = Instantiate(i_ItemToPlace, Vector3.one * 1000, Quaternion.identity);
    }

    public bool TryPlaceObject()
    {
        m_CurrentObjectToPlace = null;
        return true;
    }

    private void Update()
    {
        Ray ray = m_Cam.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray, out m_HitInfo))
        {
            switch (m_CurrentState)
            {
                case EditorState.normal:
                    if (Input.GetMouseButtonDown(0) && m_HitInfo.collider.CompareTag("Object"))
                    {
                        print(m_HitInfo.collider.GetComponent<ItemData>().Weight);
                    }
                break;
                case EditorState.placingItem:
                    if (m_HitInfo.collider.CompareTag("Ground"))
                    {
                        m_CurrentObjectToPlace.transform.position = m_HitInfo.point;
                    }
                    if (Input.GetMouseButtonDown(0) && m_HitInfo.collider.CompareTag("Ground"))
                    {
                        if (TryPlaceObject())
                            NormalMode();
                    }
                break;
            }
        }
    }
}
