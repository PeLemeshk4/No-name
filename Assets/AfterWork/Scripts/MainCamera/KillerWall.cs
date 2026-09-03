using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillerWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
