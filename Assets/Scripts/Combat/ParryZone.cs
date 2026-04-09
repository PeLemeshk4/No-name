using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ParryZone : MonoBehaviour
{
    private List<GameObject> attacks = new List<GameObject>();

    public List<GameObject> Attacks
    {
        get
        {
            return attacks;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            GameObject attack = other.gameObject;

            if (attack != null)
            {
                attacks.Add(attack);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            GameObject attack = other.gameObject;
            if (attack != null)
            {
                attacks.Remove(attack);
            }
        }
    }
}
