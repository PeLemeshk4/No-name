using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillerWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
