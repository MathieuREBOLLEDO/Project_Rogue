using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;

    [Header("Trail")]
    [SerializeField] private GameObject trail;

    #region Events
    public Vector2Event OnTouchScreen;
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



<<<<<<< TODO: Unmerged change from project 'Assembly-CSharp.Player', Avant�:

<<<<<<< TODO: Unmerged change from project 'Assembly-CSharp.Player', Avant�:
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = true;
=======
<<<<<<< TODO: Unmerged change from project 'Assembly-CSharp.Player', Avant�:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = true;
>>>>>>> Apr�s
<<<<<<< TODO: Unmerged change from project 'Assembly-CSharp.Player', Avant�:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = true;
=======
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = true;
>>>>>>> Apr�s
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = true;

=======
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = true;
>>>>>>> Apr�s
<<<<<<< TODO: Unmerged change from project 'Assembly-CSharp.Player', Avant�:
#endif
    }
=======
#endif
    }
>>>>>>> Apr�s
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
#endif
}

private void OnEnable() => playerControls.Enable();
private void OnDisable() => playerControls.Disable();

#endregion

#region Touch
private void TouchScreen(InputAction.CallbackContext context)
{
    if (context.started)
    {
        // if (GameManager.Instance.CurrentTurnState != TurnState.PlayerTurn)
        //return;

        Vector2 screenPosition = playerControls.Touch.Position.ReadValue<Vector2>();
        screenPosition = ScreenUtils.ConvertScreenToWorld(screenPosition);
        OnTouchScreen.Invoke(screenPosition);
        Debug.Log("Input Pressed");
    }
}
    #endregion

}