using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] private Tilemap lowerPlatform;
    [SerializeField] private Tilemap upperPlatform;
    [SerializeField] private RuleTile platform;
    [SerializeField] private Tilemap lowerSpikes;
    [SerializeField] private Tilemap upperSpikes;
    [SerializeField] private Tile spike;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float maxPercentOffset = 50;

    private List<Vector2Int> downFloor = new List<Vector2Int>() { new Vector2Int(0, 0) };
    private List<Vector2Int> upperFloor = new List<Vector2Int>() { new Vector2Int(0, 0) };
    private int safePlatformLength = 20;
    private float maxOffset;

    struct PlayerParameters
    {
        public float g;
        public float m;
        public float jumpPower;
        public float jumpTime;
        public float jumpHeight;
        public float speed;
        public float dashLength;      
    }
    PlayerParameters parameters = new PlayerParameters();

    public void Init(GameObject player)
    {
        transform.position = new Vector3(-cam.orthographicSize * Screen.width / Screen.height, -cam.orthographicSize - 1, 0);

        upperPlatform.transform.localPosition = new Vector3(0, 2 * cam.orthographicSize + 1, 0);
        upperSpikes.transform.localPosition = new Vector3(0, 2 * cam.orthographicSize + 1, 0);

        parameters.g = player.GetComponent<Rigidbody2D>().gravityScale * Physics2D.gravity.y;
        parameters.m = player.GetComponent<Rigidbody2D>().mass;
        parameters.jumpPower = player.GetComponent<JumpAbility>().Power;
        parameters.jumpTime = Mathf.Abs(parameters.jumpPower / (parameters.m * parameters.g));
        parameters.jumpHeight = ((parameters.jumpPower / parameters.m) * parameters.jumpTime) 
            + (parameters.g * (parameters.jumpTime * parameters.jumpTime)) / 2;
        parameters.speed = player.GetComponent<MoveAbility>().Speed;
        parameters.dashLength = player.GetComponent<DashAbility>().Length;
        maxOffset = (int)(cam.orthographicSize * maxPercentOffset / 100.0f);  
    }

    public void Generate()
    {
        AddSafePlatform(downFloor);
        for (int i = 0; i < 10; i++)
        {
            AddRandomPlatform(downFloor);
        }
        BuildPlatforms(downFloor);

        AddSafePlatform(upperFloor);
        for (int i = 0; i < 10; i++)
        {
            AddRandomPlatform(upperFloor);
        }
        BuildPlatforms(upperFloor, true);
    }

    public void AddEnemy(GameObject enemyPfb, CMSEntity enemyModel, float chanceOnPlatform)
    {
        for (int i = 0; i < downFloor.Count - 1; i++)
        {
            if (downFloor[i][1] == downFloor[i + 1][1])
            {
                float r = Random.Range(0, 100);
                if (r <= chanceOnPlatform)
                {
                    GameObject enemy = Factory.Create(enemyPfb, enemyModel);
                    enemy.transform.parent = gameObject.transform;
                    enemy.transform.localPosition = new Vector3(Random.Range(downFloor[i][0], downFloor[i+1][0]), downFloor[i][1] + 3, 0);
                }
            }
        }
    }

    private void AddRandomPlatform(List<Vector2Int> points)
    {
        int last = points.Count - 1;
        Vector2Int point = GetRandomPossibilityPoint(points[last]);

        points.Add(new Vector2Int(point.x, point.y));
        points.Add(new Vector2Int(point.x + 10, point.y));
    }

    private Vector2Int GetRandomPossibilityPoint(Vector2Int previosPoint)
    {
        float yPossibility = parameters.jumpHeight + parameters.dashLength;
        float maxDY = Mathf.Min(yPossibility, maxOffset - previosPoint.y);
        int dy = (int)Random.Range(-previosPoint.y, maxDY);
        float maxDX;
        if (dy > parameters.jumpHeight)
        {
            maxDX = Mathf.Sqrt((dy - parameters.jumpHeight - parameters.dashLength) *
            2 * (parameters.speed * parameters.speed) / parameters.g) + (parameters.speed * parameters.jumpTime);
        }
        else
        {
            maxDX = Mathf.Sqrt((dy - parameters.jumpHeight) *
            2 * (parameters.speed * parameters.speed) / parameters.g) + (parameters.speed * parameters.jumpTime) + parameters.dashLength;
        }

        int dx = (int)Random.Range(0, maxDX);

        return new Vector2Int(previosPoint.x + dx, previosPoint.y + dy);
    }

    private void AddSafePlatform(List<Vector2Int> points)
    {
        int last = points.Count - 1;
        Vector2Int nextPoint = new Vector2Int(points[last].x + safePlatformLength, points[last].y);
        points.Add(nextPoint);
    }

    private void BuildPlatforms(List<Vector2Int> points, bool reverse = false)
    {
        int i = 0;
        Vector2Int previosPoint = points[i];
 
        for (i = 1; i < points.Count; i++)
        {
            if (previosPoint.y != points[i].y)
            {
                BuildSpikes(previosPoint, points[i], reverse);
                previosPoint = points[i];
                continue;
            }

            int y = points[i].y;
            if (!reverse)
            {
                for (int x = previosPoint.x; x <= points[i].x; x++)
                {
                    for (int y1 = y; y1 >= 0; y1--)
                    {
                        Vector3Int position = new Vector3Int(x, y1, 0);

                        lowerPlatform.SetTile(position, platform);
                    }
                }
            }
            else
            {
                for (int x = previosPoint.x; x <= points[i].x; x++)
                {
                    for (int y1 = y; y1 >= 0; y1--)
                    {
                        Vector3Int position = new Vector3Int(x, y1, 0);

                        upperPlatform.SetTile(position, platform);
                    }
                }
            }
            previosPoint = points[i];
        }
    }

    private void BuildSpikes(Vector2Int firstPoint, Vector2Int secondPoint, bool reverse = false)
    {
        if (!reverse)
        {
            for (int x = firstPoint.x + 1; x <= secondPoint.x - 1; x++)
            {
                Vector3Int position = new Vector3Int(x, 0, 0);
                lowerPlatform.SetTile(position, platform);
                position.y += 1;
                lowerSpikes.SetTile(position, spike);
            }
        }
        else
        {
            for (int x = firstPoint.x + 1; x <= secondPoint.x - 1; x++)
            {
                Vector3Int position = new Vector3Int(x, 0, 0);
                upperPlatform.SetTile(position, platform);
                position.y += 1;
                upperSpikes.SetTile(position, spike);
            }
        }        
    }
}
