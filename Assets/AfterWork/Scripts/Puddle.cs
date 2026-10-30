using UnityEngine;

public class Puddle : MonoBehaviour
{
    private Effect effect;

    private void Awake()
    {
        enabled = false;
    }
    private void Init(Effect effect, float size)
    {
        this.effect = effect;
        GetComponent<BoxCollider2D>().size = new Vector2(1, size);

        enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(1);
        if (collision.gameObject.TryGetComponent<EffectHandler>(out EffectHandler eH))
        {

        }
    }
}
