using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour
{
    private CMSEntity playerModel;
    private CMSEntity enemyModel;
    private CMSEntity mapModel;

    private GameObject playerPfb;
    [SerializeField] private GameObject enemyPfb;

    private MapGenerator mapGenerator;
    private MapBuilder mapBuilder;
    private MapFloor lowerFloor;
    private MapFloor upperFloor;

    private GameObject player;
    private Camera cam;

    private PlayerActionTracker playerActionTracker;

    private SliderOfController healthPlayerSlider;
    private SliderOfController staminaPlayerSlider;
    private ActionDataUI actionData;

    private void Start()
    {
        StartMainScene();
    }

    public void StartMainScene()
    {
        CMS.Init();

        playerPfb = Resources.Load<GameObject>("CMS/Prefabs/GameObjects/Player");
        playerModel = CMS.Get<CMSEntity>("CMS/Prefabs/Models/Entities/PlayerModel");
        //enemyPfb = Resources.Load<GameObject>("CMS/Prefabs/GameObjects/Enemy");
        enemyModel = CMS.Get<CMSEntity>("CMS/Prefabs/Models/Entities/EnemyModel");
        mapModel = CMS.Get<CMSEntity>("CMS/Prefabs/Models/MapModel");

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        AddStatesAndVolumes();
        player = Factory.Create(
            playerPfb,
            playerModel);
        player.transform.position = new Vector3(0, 0, 0);

        playerActionTracker = gameObject.AddComponent<PlayerActionTracker>();
        playerActionTracker.Init(player.GetComponent<DashAbility>(),
            player.GetComponent<PlayerInitializer>().AttackAbility,
            player.GetComponent<PlayerInitializer>().StateManager);
        playerActionTracker.StartTracking();

        staminaPlayerSlider = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<SliderOfController>();
        staminaPlayerSlider.Init(player.GetComponent<StaminaController>());
        healthPlayerSlider = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<SliderOfController>();
        healthPlayerSlider.Init(player.GetComponent<HealthController>());
        actionData = GameObject.FindGameObjectWithTag("ActionData").GetComponent<ActionDataUI>();
        actionData.Init(playerActionTracker);

        CameraMove camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        camera.Init(player.transform);

        CreateMap(mapModel);

    }

    private void CreateMap(CMSEntity mapModel)
    {
        mapBuilder = GameObject.FindGameObjectWithTag("Map").GetComponent<MapBuilder>();
        mapBuilder.Init();

        mapGenerator = new MapGenerator(playerModel, cam, 0.7f);
        lowerFloor = mapGenerator.GenerateMapFloor(mapModel);
        upperFloor = mapGenerator.GenerateMapFloor(mapModel, enemies:false);

        mapBuilder.BuildFloor(lowerFloor, true, enemyPfb);
        mapBuilder.BuildFloor(upperFloor, false, enemyPfb);
    }

    // Добавление тэга (словарь не поддерживается в этом CMS) 
    private void AddStatesAndVolumes()
    {
        playerModel.Get<TagStatesAndVolumes>().StatesAndVolumes = new Dictionary<States, float>
        {
            { States.Idle, 0 },
            { States.Run, 5 },
            { States.Attack, 7 },
            { States.Jump, 5 },
            { States.Fall, 5 }
        };
    }
}
