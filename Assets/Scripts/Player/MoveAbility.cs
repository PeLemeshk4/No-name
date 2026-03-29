using UnityEngine;

public class MoveAbility : MonoBehaviour
{
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private float speed = 5.0f;

    private float direction = 0.0f;

    public float Direction 
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }

    private void Start()
    {
        movementSystem = GetComponent<MovementSystem>();
    }

    private void FixedUpdate()
    {
        movementSystem.Move = direction * speed * Time.fixedDeltaTime;
    }
}
