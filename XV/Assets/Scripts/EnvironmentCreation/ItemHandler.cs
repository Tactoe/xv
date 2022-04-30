using UnityEngine;

public enum EditorState { normal, editItem, placingItem };

public class ItemHandler : MonoBehaviour
{
    public static ItemHandler Instance;

    EditorState m_CurrentState;
    GameObject m_CurrentObjectToPlace;
    Camera m_Cam;
    RaycastHit m_HitInfo;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }

     void Start()
    {
        m_Cam = GetComponent<Camera>();
    }

    public bool CheckIfState(EditorState i_State)
    {
        return i_State == m_CurrentState;
    }

    public void NormalMode()
    {
        m_CurrentState = EditorState.normal;
    }

    public void EditMode(GameObject i_ItemToEdit)
    {
        i_ItemToEdit.GetComponent<ItemData>().DisplayWindow();
        m_CurrentState = EditorState.editItem;
    }

    public void PlaceMode(GameObject i_ItemToPlace)
    {
        if (m_CurrentState != EditorState.placingItem)
            m_CurrentState = EditorState.placingItem;
        if (m_CurrentObjectToPlace != null)
            Destroy(m_CurrentObjectToPlace);
        m_CurrentObjectToPlace = Instantiate(i_ItemToPlace, Vector3.one * 1000, Quaternion.identity);
        m_CurrentObjectToPlace.name = i_ItemToPlace.name;
        m_CurrentObjectToPlace.transform.Find("Hitbox").gameObject.SetActive(false);
        m_CurrentObjectToPlace.transform.Find("PlacementBubble").gameObject.SetActive(true);
    }

    public bool TryPlaceObject()
    {
        PlacementBubble pb = m_CurrentObjectToPlace.transform.Find("PlacementBubble").GetComponent<PlacementBubble>();
        if (pb.CanBePlaced)
        {
            m_CurrentObjectToPlace.transform.Find("Hitbox").gameObject.SetActive(true);
            pb.gameObject.SetActive(false);
            m_CurrentObjectToPlace = null;
            return true;
        }
        return false;
    }

    private void Update()
    {
        Ray ray = m_Cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out m_HitInfo))
        {
            switch (m_CurrentState)
            {
                case EditorState.normal:
                    if (Input.GetMouseButtonDown(0) && m_HitInfo.collider.CompareTag("Object"))
                    {
                        EditMode(m_HitInfo.collider.gameObject);
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
