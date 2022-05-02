using UnityEngine;

public enum EditorState { normal, editItem, placingItem };

public class ItemHandler : MonoBehaviour
{
    public static ItemHandler Instance;

    [SerializeField]
    float m_RotationSpeed;
    EditorState m_CurrentState;
    GameObject m_CurrentObjectToPlace;
    Camera m_Cam;
    RaycastHit m_HitInfo;

    public delegate void ChangeMode(EditorState i_NewState);
    public static event ChangeMode ModeChanged;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }

     void Start()
    {
        m_Cam = GetComponent<Camera>();
        NormalMode();
    }

    public bool CheckIfState(EditorState i_State)
    {
        return i_State == m_CurrentState;
    }

    public void NormalMode()
    {
        m_CurrentState = EditorState.normal;
        if (ModeChanged != null)
            ModeChanged(m_CurrentState);
    }

    public void EditMode(GameObject i_ItemToEdit)
    {
        EditWindow.Instance.EnableWindow(i_ItemToEdit.transform.parent.gameObject);
        m_CurrentState = EditorState.editItem;
        if (ModeChanged != null)
            ModeChanged(m_CurrentState);
    }

    public void PlaceMode(GameObject i_ItemToPlace, bool i_InstantiateItem = true)
    {
        if (m_CurrentState != EditorState.placingItem)
            m_CurrentState = EditorState.placingItem;
        if (m_CurrentObjectToPlace != null)
            Destroy(m_CurrentObjectToPlace);
        if (i_InstantiateItem)
            m_CurrentObjectToPlace = Instantiate(i_ItemToPlace, Vector3.one * 1000, Quaternion.identity);
        else
            m_CurrentObjectToPlace = i_ItemToPlace;
        ItemData itemData = m_CurrentObjectToPlace.GetComponentInChildren<Item>().Data;
        itemData.PrefabName = i_ItemToPlace.name;
        itemData.ItemName = i_ItemToPlace.name;
        m_CurrentObjectToPlace.transform.parent = GameObject.FindGameObjectWithTag("Scene").transform;
        m_CurrentObjectToPlace.transform.Find("Hitbox").gameObject.SetActive(false);
        m_CurrentObjectToPlace.transform.Find("PlacementBubble").gameObject.SetActive(true);
        if (ModeChanged != null)
            ModeChanged(m_CurrentState);
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
        if ((Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.RightArrow))
            && CheckIfState(EditorState.placingItem))
        {
            float direction = Input.GetKey(KeyCode.LeftArrow) ? 1 : -1;
            m_CurrentObjectToPlace.transform.Rotate(Vector3.up * Time.deltaTime * m_RotationSpeed * direction);

        }
    }
}
