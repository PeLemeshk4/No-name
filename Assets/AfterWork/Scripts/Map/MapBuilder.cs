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
    [SerializeField] private RuleTile puddleTile;

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
        Tilemap tileMap = isBottom ? lowerPlatform : upperPlatform;
        BuildPlatforms(floor, tileMap);
        BuildEnemies(floor, tileMap, enemyPfb);
        BuildPuddles(floor, tileMap, puddleTile);
    }

    public void BuildPlatforms(MapFloor floor, Tilemap tileMap)
    {
        foreach(PlatformCoordinate platformCoordinate in floor.Platforms)
        {
            BuildPlatform(platformCoordinate, tileMap);
        }
    }

    private void BuildPlatform(PlatformCoordinate platformCoordinate, Tilemap tileMap)
    {
        for (int x = platformCoordinate.X1; x < platformCoordinate.X2; x++)
        {
            for (int y = 0; y <= platformCoordinate.Y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                tileMap.SetTile(position, platformTile);
            }
        }
    }

    public void BuildPuddles(MapFloor floor, Tilemap tileMap, RuleTile puddleTile)
    {
        foreach (int i in floor.PuddlesNumbers)
        {
            foreach (Vector2Int coordinate in floor.Puddles[i])
            {
                for (int j = coordinate.x; j < coordinate.y; j++)
                {
                    Vector3Int position = new Vector3Int(j, floor.Platforms[i].Y, 0);
                    tileMap.SetTile(position, puddleTile);
                }
            }
        }

        
    }

    public void BuildEnemies(MapFloor floor, Tilemap tileMap, GameObject enemyPfb)
    {
        foreach (int i in floor.EnemiesNumbers)
        {
            foreach ((TypeEnemy, int) enemyData in floor.Enemies[i])
            {
                GameObject enemyObject = Instantiate(enemyPfb);
                enemyObject.transform.parent = tileMap.transform;
                enemyObject.transform.localPosition = new Vector3(enemyData.Item2, floor.Platforms[i].Y + 2, 0);
            }
        }
    }
}
