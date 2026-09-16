using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class G : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private MapBuilder mapBuilder;

    private GameObject playerPfb;
    private GameObject enemyPfb;

    private GameObject player;
    private Camera cam;

    private CMSEntity playerModel;
    private CMSEntity enemyModel;

    private PlayerActionTracker playerActionTracker;

    private SliderOfController healthPlayerSlider;
    private SliderOfController staminaPlayerSlider;
    private ActionDataUI actionData;

    private void Awake()
    {
        StartMainScene();
    }

    public void StartMainScene()
    {
        CMS.Init();

        playerPfb = Resources.Load<GameObject>("CMS/Prefabs/GameObjects/Player");
        playerModel = CMS.Get<CMSEntity>("CMS/Prefabs/Models/Entities/PlayerModel");
        enemyPfb = Resources.Load<GameObject>("CMS/Prefabs/GameObjects/Enemy");
        enemyModel = CMS.Get<CMSEntity>("CMS/Prefabs/Models/Entities/EnemyModel");

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        player = Factory.Create(
            playerPfb,
            playerModel);
        player.transform.position = new Vector3(0, 0, 0);

        playerActionTracker = gameObject.AddComponent<PlayerActionTracker>();
        playerActionTracker.Init(player.GetComponent<DashAbility>(),
            player.GetComponent<AttackAbility>(), player.GetComponent<PlayerInitializer>().StateManager);
        playerActionTracker.StartTracking();

        staminaPlayerSlider = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<SliderOfController>();
        staminaPlayerSlider.Init(player.GetComponent<StaminaController>());
        healthPlayerSlider = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<SliderOfController>();
        healthPlayerSlider.Init(player.GetComponent<HealthController>());
        actionData = GameObject.FindGameObjectWithTag("ActionData").GetComponent<ActionDataUI>();
        actionData.Init(playerActionTracker);

        CameraMove camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        camera.Init(player.transform);

        CreateMap();

    }

    private void CreateMap()
    {
        mapBuilder = GameObject.FindGameObjectWithTag("Map").GetComponent<MapBuilder>();
        mapBuilder.Init();

        mapGenerator = new MapGenerator(playerModel, player.GetComponent<Rigidbody2D>(), cam);
        mapGenerator.Generate();

        mapBuilder.BuildPlatforms(mapGenerator.LowerPLatformCoordinate, true);
        mapBuilder.BuildPlatforms(mapGenerator.UpperPlatformCoordinate, false);
    }
}
