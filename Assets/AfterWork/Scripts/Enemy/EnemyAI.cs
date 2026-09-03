using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private MoveAbility moveAbility;
    [SerializeField] private DetectorPlayerInCircle detector;
    [SerializeField] private LookAbility lookAbility;
    [SerializeField] private ActiveWeapon activeWeapon;
    [SerializeField] private AttackAbility attackAbility;
    [SerializeField] private HealthController healthController;
    [SerializeField] private AttackHandler attackHandler;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Node tree;

    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        moveAbility = GetComponent<MoveAbility>();
        attackAbility = GetComponent<AttackAbility>();
        activeWeapon = GetComponent<ActiveWeapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthController = GetComponent<HealthController>();

        attackHandler = GetComponent<AttackHandler>();
        animator = GetComponent<Animator>();

        healthController.HealthIsNull += Dead;

        CreateBehaviourTree();

        enabled = true;
    }

    private void CreateBehaviourTree()
    {
        tree = new Selector(
            // Атакует
            new Sequence(
                new IsAttacking(activeWeapon)
                ),
            // Игрок в зоне видимости
            new Sequence(
                new IsPlayerVisible(detector),
                new Selector(
                    // Игрок в зоне досягаемости, начало атаки
                    new Sequence(
                        new IsPlayerInAttackRange(transform, activeWeapon, detector),
                        new StartAttack(transform, activeWeapon, attackAbility, moveAbility, lookAbility, detector, animator)
                    ),
                    // Преследование игрока
                    new Pursue(transform, moveAbility, lookAbility, detector, animator)
                    )
                ),
            // Бездействие
            new Idle(moveAbility, animator)
        );
    }

    void Update()
    {
        if (tree != null)
        {
            tree.Evaluate();
        }
    }

    private void Dead(object s, EventArgs e)
    {
        StartCoroutine(Dying());
    }

    private IEnumerator Dying()
    {
        enabled = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    } 
}
