using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    private CircleCollider2D col;

    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;

        enabled = true;
    }

    public void MakeSound(float length)
    {
        if (length == 0)
        {
            col.enabled = false;
        }
        else
        {
            col.radius = length;
            col.enabled = true;
        }
    }
}
