using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Weapon : MonoBehaviour
{
    protected WeaponData weaponData;
    protected BoxCollider2D attackCollider;
    protected Attack attack;
    private ParryZone parryZone;
    protected bool isAttacking = false;
    protected float attackTime = 0.0f;

    public WeaponData WeaponData
    {
        get
        {
            return weaponData;
        }
        set
        {
            weaponData = value;
            if (weaponData == null)
            {
                gameObject.SetActive(false);
                return;
            }
            UpdateWeapon();
            gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        gameObject.SetActive(false);

        attackCollider = gameObject.AddComponent<BoxCollider2D>();
        attackCollider.size = Vector2.zero;
        attackCollider.isTrigger = true;

        attack = gameObject.AddComponent<Attack>();
        attack.enabled = false;
        attack.AttackHit += OnAttackHit;

        parryZone = gameObject.AddComponent<ParryZone>();
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            attackTime += Time.fixedDeltaTime;
            if (attackTime >= weaponData.AttackDuration)
            {
                isAttacking = false;
                attack.enabled = false;
                attackTime = 0.0f;
            }
        }
    }

    private void UpdateWeapon()
    {
        attack.Damage = weaponData.Damage;
        attackCollider.size = new Vector2(weaponData.Width, weaponData.Length);
        attackCollider.offset = new Vector2(0, weaponData.Length / 2);
    }

    private void OnAttackHit(object sender, AttackHitEventsArgs e)
    {
        attack.enabled = false;
    }

    public void Attack()
    {
        if (weaponData == null) return;
        if (isAttacking) return;

        isAttacking = true;
        attack.enabled = true;

        Debug.Log(weaponData.Damage);
    }

    public void Parry()
    {
        if (weaponData == null) return;
        if (isAttacking) return;

        List<GameObject> attacks = parryZone.Attacks;

        for (int i = attacks.Count - 1; i >= 0; i--)
        {
            GameObject attack = attacks[i];
            if (attack != null)
            {
                attacks.RemoveAt(i);
                Destroy(attack);
            }
        }
    }
}
