using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class G : MonoBehaviour
{
    private GameObject playerPfb;
    private GameObject enemyPfb;

    private CMSEntity playerModel;
    private CMSEntity enemyModel;

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

        GameObject player = Factory.Create(
            playerPfb,
            playerModel);
        player.transform.position = new Vector3(0, 0, 0);

        staminaPlayerSlider = GameObject.FindGameObjectWithTag("StaminaBar").GetComponent<SliderOfController>();
        staminaPlayerSlider.Init(player.GetComponent<StaminaController>());
        healthPlayerSlider = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<SliderOfController>();
        healthPlayerSlider.Init(player.GetComponent<HealthController>());
        actionData = GameObject.FindGameObjectWithTag("ActionData").GetComponent<ActionDataUI>();
        actionData.Init(player.GetComponent<PlayerInitializer>().actionTracker);


        GenerateMap map = GameObject.FindGameObjectWithTag("Map").GetComponent<GenerateMap>();
        map.Init(player);
        map.Generate();
        map.AddEnemy(enemyPfb, enemyModel, 40);

        CameraMove camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>();
        camera.Init(player.transform);

    }
}
