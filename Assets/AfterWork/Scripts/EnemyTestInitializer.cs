using UnityEngine;

public class EnemyTestInitializer : Initializer
{
    [Header("AutoAdd")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private AttackHandler attackHandler;

    public override void Init(CMSEntity model)
    {
        healthController = GetComponent<HealthController>();
        attackHandler = GetComponent<AttackHandler>();

        healthController.Init(model.Get<TagHealth>());
        attackHandler.Init(healthController);
    }
}
