using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct PlatformCoordinate
{
    private int x1;
    private int x2;
    private int y;

    public int X1 { get { return x1; } }
    public int X2 { get { return x2; } }
    public int Y { get { return y; } }

    public PlatformCoordinate(int x1, int x2, int y)
    {
        this.x1 = x1;
        this.x2 = x2;
        this.y = y;
    }
}

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private RuleTile platformTile;
    [SerializeField] private Tilemap lowerPlatform;
    [SerializeField] private Tilemap upperPlatform;

    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        //this.platformTile = platformTile;
        //this.lowerPlatform = bottomPlatform;  
        //this.upperPlatform = upperPlatform;

        float width = Screen.width;
        float height = Screen.height;
        float scale = width / height;
        transform.localPosition = new Vector3(-scale * cam.orthographicSize, -0.5f, 0);
        upperPlatform.transform.localPosition = new Vector3(0, cam.orthographicSize, 0);
        lowerPlatform.transform.localPosition = new Vector3(0, -cam.orthographicSize, 0);

        enabled = true;
    }

    public void BuildFloor(MapFloor floor, bool isBottom, GameObject enemyPfb)
    {
        BuildPlatforms(floor, isBottom);
        BuildEnemies(floor, enemyPfb);
    }

    public void BuildPlatforms(MapFloor floor, bool isBottom)
    {
        foreach(PlatformCoordinate platformCoordinate in floor.Platforms)
        {
            BuildPlatform(platformCoordinate, isBottom);
        }
    }

    private void BuildPlatform(PlatformCoordinate platformCoordinate, bool isBottomPlatform)
    {
        if (isBottomPlatform)
        {
            for (int x = platformCoordinate.X1; x < platformCoordinate.X2; x++)
            {
                for (int y = 0; y <= platformCoordinate.Y; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    lowerPlatform.SetTile(position, platformTile);
                }
            }
        }
        else
        {
            for (int x = platformCoordinate.X1; x < platformCoordinate.X2; x++)
            {
                for (int y = 0; y <= platformCoordinate.Y; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    upperPlatform.SetTile(position, platformTile);
                }
            }
        }
    }

    public void BuildEnemies(MapFloor floor, GameObject enemyPfb)
    {
        foreach (int i in floor.EnemiesNumbers)
        {
            foreach ((TypeEnemy, int) enemyData in floor.Enemies[i])
            {
                GameObject enemyObject = Instantiate(enemyPfb);
                enemyObject.transform.parent = lowerPlatform.transform;
                enemyObject.transform.localPosition = new Vector3(enemyData.Item2, floor.Platforms[i].Y + 2, 0);
            }
        }
    }
}
