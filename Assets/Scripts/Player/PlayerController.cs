using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    public void Move(Vector3 direction)
    {
        transform.position += direction;
    }

    public void Push(Vector3 direction, Transform box)
    {
        Move(direction);
        box.position += direction;
    }
}

