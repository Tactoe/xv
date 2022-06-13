using UnityEngine;

public enum EditorState { normal, editItem, placingItem, placingOrder, exploring };

public class ItemHandler : MonoBehaviour
{
    public static ItemHandler Instance;

    [SerializeField]
    Transform m_SceneItemListTF;
    [SerializeField]
    GameObject m_SceneItemSelector;
    [SerializeField]
    GameObject m_FPSController;
    [SerializeField]
    GameObject m_PauseMenu;
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
        GetComponent<PlayerMovement>().enabled = true;
        m_FPSController.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        ModeChanged?.Invoke(m_CurrentState);
    }
    
    public void ExploreMode()
    {
        m_CurrentState = EditorState.exploring;
        Cursor.lockState = CursorLockMode.Locked;
        m_FPSController.SetActive(true);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<PlayerMovement>().enabled = false;
        ModeChanged?.Invoke(m_CurrentState);
    }

    public void EditMode(GameObject i_ItemToEdit = null)
    {
        m_CurrentState = EditorState.editItem;
        if (i_ItemToEdit != null)
			EditWindow.Instance.EnableWindow(i_ItemToEdit.transform.parent.gameObject);
        ModeChanged?.Invoke(m_CurrentState);
    }

    public void PlaceMode(GameObject i_ItemToPlace, bool i_InstantiateItem = true)
    {
        m_CurrentState = EditorState.placingItem;
        if (m_CurrentObjectToPlace != null)
            Destroy(m_CurrentObjectToPlace);
        if (i_InstantiateItem)
        {
            m_CurrentObjectToPlace = Instantiate(i_ItemToPlace, Vector3.one * 1000, Quaternion.identity);
            GameObject tmp = Instantiate(m_SceneItemSelector, m_SceneItemListTF);
            tmp.GetComponent<SceneItemSelector>().Init(m_CurrentObjectToPlace, i_ItemToPlace.name);
        }
        else
            m_CurrentObjectToPlace = i_ItemToPlace;
        SetupItem(i_ItemToPlace);
        ModeChanged?.Invoke(m_CurrentState);
    }

	public void PlaceOrderMode()
	{
		m_CurrentState = EditorState.placingOrder;
		ModeChanged?.Invoke(m_CurrentState);
	}

    public void SetupItem(GameObject i_ItemToPlace)
    {
        ItemData itemData = m_CurrentObjectToPlace.GetComponentInChildren<Item>().Data;
        if (itemData.PrefabName == "")
            itemData.PrefabName = i_ItemToPlace.name;
        if (itemData.ItemName == "")
            itemData.ItemName = i_ItemToPlace.name;
        m_CurrentObjectToPlace.name = itemData.ItemName;
		Transform scene = GameObject.FindGameObjectWithTag("Scene").transform;
        m_CurrentObjectToPlace.transform.parent = scene;
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
                case EditorState.editItem:
                    if (Input.GetMouseButtonDown(0) && m_HitInfo.collider.CompareTag("Object"))
                    {
                        EditWindow.Instance.CloseWindow();
                        EditWindow.Instance.EnableWindow(m_HitInfo.collider.gameObject);
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
        HandleKeyboardInputs();
    }

    void HandleKeyboardInputs()
    {
        if (CheckIfState(EditorState.exploring) && Input.GetKeyDown(KeyCode.Escape))
        {
            NormalMode();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            m_PauseMenu.SetActive(!m_PauseMenu.activeInHierarchy);
            Time.timeScale = m_PauseMenu.activeInHierarchy ? 0 : 1;
            
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
