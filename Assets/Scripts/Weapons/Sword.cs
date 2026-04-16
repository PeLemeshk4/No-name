using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Sword : Weapon
{

    protected float rotateValue = 0.0f;
    protected float rotateSpeed = 0.0f;

    private void FixedUpdate()
    {
        if (!isAttacking) return;

        float currentZ = transform.eulerAngles.z;
        float step = rotateSpeed * Time.fixedDeltaTime;

        // Mathf.LerpAngle корректно обходит 360/0
        float newZ = Mathf.MoveTowardsAngle(currentZ, rotateValue, step);
        transform.eulerAngles = new Vector3(0, 0, newZ);

        // Довёрнуто?
        if (Mathf.Abs(Mathf.DeltaAngle(newZ, rotateValue)) < 0.05f)
        {
            transform.eulerAngles = new Vector3(0, 0, rotateValue);

            attack.enabled = false;
            isAttacking = false;
        }
    }

    public void Attacks(Vector2 attackDirection)
    {
        if (isAttacking) return;

        transform.eulerAngles = Vector3.zero;

        rotateValue = attackDirection.x >= 0 ? -90.0f : 90.0f;
        rotateSpeed = Mathf.Abs(rotateValue) / weaponData.AttackDuration;

        isAttacking = true;
        attack.enabled = true;

        Debug.Log(weaponData.Damage);
    }
}
