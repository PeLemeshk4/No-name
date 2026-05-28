using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Transform player;

    private void Awake()
    {
        enabled = false;
    }
    public void Init(Transform player)
    {
        this.player = player;

        transform.position = new Vector3(0, -0.5f, -10);

        enabled = true;
    }

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
