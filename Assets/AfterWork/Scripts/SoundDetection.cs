using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(transform.parent.localPosition);
    }
}
