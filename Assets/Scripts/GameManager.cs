using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static DefaultInputActions;

public class GameManager : MonoBehaviour, IPlayerActions
{
    [SerializeField] private LevelGenerator levelGenerator = null;

    private LevelConfig currentLevel;

    void Start()
    {
        var controlls = new DefaultInputActions();
        controlls.Player.SetCallbacks(this);
        controlls.Enable();

        levelGenerator.GenerateLevel(new LevelConfig()
        {
            Collision = new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(0, 3),
                new Vector2Int(1, 3),
                new Vector2Int(2, 3),
                new Vector2Int(3, 3),
                new Vector2Int(3, 2),
                new Vector2Int(3, 1),
                new Vector2Int(3, 0),
                new Vector2Int(2, 0),
                new Vector2Int(1, 0),
            },
            Walkable = new Vector2Int[]
            {
                new Vector2Int(1, 1),
                new Vector2Int(1, 2),
                new Vector2Int(2, 2),
                new Vector2Int(2, 1),
            },
            BoxHolders = new Vector2Int[]
            {
                new Vector2Int(2,2)
            },
            Boxes = new Vector2Int[]
            {
                new Vector2Int(2,1)
            },
            StartPosition = new Vector2Int(1, 1)
        });
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
