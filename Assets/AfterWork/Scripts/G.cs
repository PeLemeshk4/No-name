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
    private List<PlatformCoordinate> lowerPlatform;
    private List<PlatformCoordinate> upperPlatform;
    private List<int> enemySpotsCoordinates;

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

        mapGenerator = new MapGenerator(playerModel, player.GetComponent<Rigidbody2D>(), cam, 0.7f);
        MapFloor floor = new MapFloor();
        lowerPlatform = new List<PlatformCoordinate>();
        upperPlatform = new List<PlatformCoordinate>();
        enemySpotsCoordinates = mapGenerator.GenerateEnemies(18, 0.7f, 450);

        mapBuilder.BuildPlatforms(lowerPlatform, true);
        mapBuilder.BuildPlatforms(upperPlatform, false);
        mapBuilder.BuildEnemies(enemySpotsCoordinates, lowerPlatform, enemyPfb);
    }

    // так добавл€етс€ тег (словарь не поддерживаетс€ в CMS) 
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
