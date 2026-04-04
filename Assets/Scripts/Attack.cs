using UnityEngine;

public class Attack : MonoBehaviour
{
    private float damage = 0;

    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
