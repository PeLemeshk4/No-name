using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Transform player;

    private void Update()
    {
        if (player.position.x > transform.position.x + 3 || player.position.x < transform.position.x - 3)
        {
            Vector3 offset = new Vector3(player.position.x - transform.position.x, 0, 0) * speed * Time.deltaTime;

            if ((transform.position + offset).x <= 0) return;

            transform.position += offset;
        }
    }
}
