using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private WeaponData weaponData;
    private BoxCollider2D attackCollider;
    private Attack attack;
    private Vector3 attackStages;
    private bool isAttacking = false;
    private float attackTime = 0.0f;
    private bool hasAttacked = false;

    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
    }

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

        animator = GetComponent<Animator>();

        attackCollider = gameObject.AddComponent<BoxCollider2D>();
        attackCollider.size = Vector2.zero;
        attackCollider.isTrigger = true;

        attack = gameObject.AddComponent<Attack>();
        attack.enabled = false;
        attack.AttackHit += OnAttackHit;
    }

    private void Update()
    {
        if (isAttacking)
        {
            attackTime += Time.deltaTime;

            if (hasAttacked || attackTime >= WeaponData.AttackDuration)
            {
                attack.enabled = false;
                
                if (attackTime >= WeaponData.AttackDuration)
                {
                    isAttacking = false;
                    attackTime = 0.0f;

                    animator.SetBool("IsAttacking", isAttacking);
                }

                return;
            }

            if (attackTime >= attackStages.x + attackStages.y)
            {
                attack.enabled = false;
            }
            else if (attackTime >= attackStages.x)
            {
                attack.enabled = true; 
            }
        }
    }

    private void UpdateWeapon()
    {
        attack.Damage = weaponData.Damage;
        attackCollider.size = new Vector2(weaponData.Width, weaponData.Length);
        attackCollider.offset = new Vector2(0, weaponData.Length / 2);
        attackStages = weaponData.AttackStages;
    }

    private void OnAttackHit(object sender, AttackHitEventsArgs e)
    {
        attack.enabled = false;
        if (e.Attacks != null)
        {
            List<Attack> attacks = e.Attacks;
            for(int i = attacks.Count - 1; i >= 0; i--)
            {
                if (attacks[i] == null) continue;
                attacks[i].gameObject.SetActive(false);
            }
            Debug.Log("Parry");
        }
        else
        {
            Debug.Log("Attack");
        }
        hasAttacked = true;
    }

    public void Attack()
    {
        if (weaponData == null) return;
        if (isAttacking) return;

        hasAttacked = false;
        isAttacking = true;

        animator.SetBool("IsAttacking", isAttacking);
    }
}
