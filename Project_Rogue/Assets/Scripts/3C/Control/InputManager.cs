using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;

    [Header("Trail / Aiming")]
    [SerializeField] private GameObject trail;       // Cible visuelle (GC node)
    [SerializeField] private GameObject debugSphere;
    [SerializeField] private GameObject cancelZone;  // UI ou collider pour annuler

    private bool isAiming = false;
    private bool isCanceled = false;

    private GameObject objDebug;
    private Vector2 currentWorldPos;

    #region Events
    public Vector2Event OnTouchScreen;
    public Vector2Event OnShoot;
    public UnityEvent OnCancel;
    #endregion

    #region Init
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Touch.Press.started += TouchScreen;
        playerControls.Touch.Position.performed += TouchScreen;
        playerControls.Touch.Press.canceled += TouchScreen;
    }

    private void Start()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif

        objDebug = GameObject.Instantiate(debugSphere, currentWorldPos, Quaternion.identity);
        objDebug.SetActive (false);
    }

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();
    #endregion

    #region Touch

    public void SetIsCanceled(bool newValue)
    {
        isCanceled = newValue;
    }

    private void TouchScreen(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = playerControls.Touch.Position.ReadValue<Vector2>();

        //if (IsPointerOverUI())
        //    return;


        if (!IsWithinCameraView(Camera.main, screenPosition))
            return;


        currentWorldPos = ScreenUtils.ConvertScreenToWorld(screenPosition);
        

        if (context.started)
        {
            isAiming = true;
            isCanceled = false;
            Debug.Log("Input Started");
            objDebug.SetActive(true);
            OnTouchScreen?.Invoke(currentWorldPos);

        }

        if (context.performed && isAiming)
        {
            OnTouchScreen?.Invoke(currentWorldPos);
        }

        if (context.canceled)
        {
            if (!isCanceled)
            {
                OnShoot?.Invoke(currentWorldPos);
                Debug.Log("Input not canceled!");
            }
            else
            {
                OnCancel?.Invoke();
                Debug.Log("Input canceled.");
            }
            isAiming = false;
            //trail.SetActive(false);
            objDebug.SetActive(false);
        }
            


        objDebug.transform.position = currentWorldPos;
    }

    #endregion

    #region Utils
    private bool IsWithinCameraView(Camera cam, Vector2 screenPos)
    {
        if (cam == null) return true; // sécurité si la caméra n’est pas assignée

        Vector3 viewportPos = cam.ScreenToViewportPoint(screenPos);
        return viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }

    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // Souris
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
#elif UNITY_IOS || UNITY_ANDROID
        // Tactile — chaque toucher a un fingerId
        if (EventSystem.current == null) return false;
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return false;
#else
        return false;
#endif
    }
    #endregion
}
