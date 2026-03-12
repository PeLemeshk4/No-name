using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(speed * Time.fixedDeltaTime, 0, 0);
        transform.position += direction;
    }
}
