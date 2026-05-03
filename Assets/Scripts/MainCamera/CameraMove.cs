using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Transform player;

    private void Update()
    {
        if (player.position.x > transform.position.x + 3 || player.position.x < transform.position.x - 3)
        {
            transform.position += new Vector3(player.position.x - transform.position.x, 0, 0) * speed * Time.deltaTime;
        }
    }
}
