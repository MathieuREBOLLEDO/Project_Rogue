using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;

    [Header("Trail / Aiming")]
    [SerializeField] private GameObject trail;       // Cible visuelle (GC node)
    [SerializeField] private GameObject cancelZone;  // UI ou collider pour annuler

    private bool isAiming = false;
    private bool isCanceled = false;

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
        playerControls.Touch.Press.performed += TouchScreen;
        playerControls.Touch.Press.canceled += TouchScreen;
    }

    private void Start()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
    }

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();
    #endregion

    #region Touch

    private void TouchScreen(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = playerControls.Touch.Position.ReadValue<Vector2>();
        Vector2 worldPosition = ScreenUtils.ConvertScreenToWorld(screenPosition);

        if (context.started)
        {
            isAiming = true;
            isCanceled = false;

            //trail.SetActive(true);
            //trail.transform.position = worldPosition;

            OnTouchScreen?.Invoke(worldPosition);
            Debug.Log("Input Started");
        }

        if (context.performed && isAiming)
        {
            UpdateTouchPosition(worldPosition);
        }

        if (context.canceled)
        {
            if (!isCanceled)
            {
                OnShoot?.Invoke(worldPosition);
                Debug.Log("Shot fired!");
            }
            else
            {
                OnCancel?.Invoke();
                Debug.Log("Shot canceled.");
            }

            isAiming = false;
            //trail.SetActive(false);
        }
    }

    private void UpdateTouchPosition(Vector2 worldPos)
    {
        //trail.transform.position = worldPos;

        // Si tu as un cancel zone, tu peux vérifier si le doigt est dedans
        //if (cancelZone != null)
        //{
        //    Vector2 cancelZoneScreenPos = Camera.main.WorldToScreenPoint(cancelZone.transform.position);
        //    Rect cancelRect = new Rect(cancelZoneScreenPos - new Vector2(50, 50), new Vector2(100, 100)); // zone de 100x100px
        //
        //    Vector2 currentScreenPos = playerControls.Touch.Position.ReadValue<Vector2>();
        //    isCanceled = cancelRect.Contains(currentScreenPos);
        //}
    }

    #endregion
}
