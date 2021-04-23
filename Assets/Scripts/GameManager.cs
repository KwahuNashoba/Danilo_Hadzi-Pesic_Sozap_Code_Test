using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static DefaultInputActions;

public class GameManager : MonoBehaviour, IPlayerActions
{
    public Tilemap collisionMap;
    public Tilemap walkableMap;

    private LevelData currentLevel;

    void Start()
    {
        var controlls = new DefaultInputActions();
        controlls.Player.SetCallbacks(this);
        controlls.Enable();
    }

    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.action.phase == InputActionPhase.Performed)
        {
            Debug.Log(context.action);
        }    
    }
}
