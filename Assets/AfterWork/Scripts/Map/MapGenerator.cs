using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator
{
    [SerializeField] private float maxPercentOffset = 70;

    private List<Vector2Int> downFloor = new List<Vector2Int>() { new Vector2Int(0, 0) };
    private List<Vector2Int> upperFloor = new List<Vector2Int>() { new Vector2Int(0, 0) };
    private int safePlatformLength = 30;
    private float maxOffset;

    private List<PlatformCoordinate> lowerPlatformCoordinates = new List<PlatformCoordinate>();
    private List<PlatformCoordinate> upperPlatformCoordinates = new List<PlatformCoordinate>();

    public List<PlatformCoordinate> LowerPLatformCoordinate { get { return lowerPlatformCoordinates; } }
    public List<PlatformCoordinate> UpperPlatformCoordinate { get { return upperPlatformCoordinates; } } 

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

    public MapGenerator(CMSEntity playerModel, Rigidbody2D rb, Camera cam)
    {
        parameters.g = rb.gravityScale * Physics2D.gravity.y;
        parameters.m = rb.mass;
        parameters.jumpPower = playerModel.Get<TagJump>().Power;
        parameters.jumpTime = Mathf.Abs(parameters.jumpPower / (parameters.m * parameters.g));
        parameters.jumpHeight = ((parameters.jumpPower / parameters.m) * parameters.jumpTime) 
            + (parameters.g * (parameters.jumpTime * parameters.jumpTime)) / 2;
        parameters.speed = playerModel.Get<TagSpeed>().Speed;
        parameters.dashLength = playerModel.Get<TagDash>().Length;
        maxOffset = (int)(cam.orthographicSize * maxPercentOffset / 100.0f);  
    }

    public void Generate()
    {
        lowerPlatformCoordinates.Add(GetPlatformCoordinate(0, safePlatformLength, 1));
        for (int i = 0; i < 10; i++)
        {
            Vector2Int nextPoint = GetRandomPossibilityPoint(
                lowerPlatformCoordinates[i].X2, lowerPlatformCoordinates[i].Y);
            lowerPlatformCoordinates.Add(GetPlatformCoordinate(nextPoint.x, nextPoint.x + Random.Range(5, 10), nextPoint.y));
        }

        upperPlatformCoordinates.Add(GetPlatformCoordinate(0, safePlatformLength, 1));
        for (int i = 0; i < 10; i++)
        {
            Vector2Int nextPoint = GetRandomPossibilityPoint(
                upperPlatformCoordinates[i].X2, upperPlatformCoordinates[i].Y);
            upperPlatformCoordinates.Add(GetPlatformCoordinate(nextPoint.x, nextPoint.x + Random.Range(5, 10), nextPoint.y));
        }
    }

    private PlatformCoordinate GetPlatformCoordinate(int x1, int x2, int y)
    {
        PlatformCoordinate platformCoordinate = new PlatformCoordinate();
        platformCoordinate.X1 = x1;
        platformCoordinate.X2 = x2;
        platformCoordinate.Y = y;
        return platformCoordinate;
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
                    enemy.transform.localPosition = new Vector3(Random.Range(downFloor[i][0], downFloor[i+1][0]), downFloor[i][1] + 3, 0);
                }
            }
        }
    }

    private Vector2Int GetRandomPossibilityPoint(int x, int y)
    {
        float yPossibility = parameters.jumpHeight + parameters.dashLength;
        float maxDY = Mathf.Min(yPossibility, maxOffset - y);
        int dy = (int)Random.Range(-y, maxDY);
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

        return new Vector2Int(x + dx, y + dy);
    }
}
