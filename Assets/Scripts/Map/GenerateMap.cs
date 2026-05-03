using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;

    private List<Vector2Int> points = new List<Vector2Int>() { new Vector2Int(0, 0) };
    private int length = 500;
    private Vector2Int minMaxLengthPlatform = new Vector2Int(3, 20);
    private int safePlatformLength = 20;
    private int dYMax = 5;

    private struct PlayerMovementCapabilities
    {
        
    }

    private void Awake()
    {
        AddSafePlatform();
        AddRandomPlatform();

        BuildMap();
    }

    private void Generate()
    {

    }

    private void AddRandomPlatform()
    {
        int last = points.Count - 1;
        int dX = Random.Range(0, 5);
        int y = Random.Range(0, points[last].y + dYMax);
        
        points.Add(new Vector2Int(points[last].x + dX, y));
        points.Add(new Vector2Int(points[last].x + dX + 10, y));
    }

    private void AddSafePlatform()
    {
        int last = points.Count - 1;
        Vector2Int nextPoint = new Vector2Int(points[last].x + safePlatformLength, points[last].y);
        points.Add(nextPoint);
    }

    private void BuildMap()
    {
        Vector2Int previosPoint = points[0];

        for (int i = 1; i < points.Count; i++)
        {
            if (previosPoint.y != points[i].y)
            {
                previosPoint = points[i];
                continue;
            }

            int y = points[i].y;
            for (int x = previosPoint.x; x < points[i].x; x++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                //Метод для нормализации позиции тайла
                tilemap.SetTile(position, tile);
            }
        }
    }

}
