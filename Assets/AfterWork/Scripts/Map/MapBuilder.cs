using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct PlatformCoordinate
{
    public int X1;
    public int X2;
    public int Y;
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

    public void BuildPlatforms(List<PlatformCoordinate> platformCoordinates, bool isBottomPlatform)
    {
        foreach(PlatformCoordinate platformCoordinate in platformCoordinates)
        {
            BuildPlatform(platformCoordinate, isBottomPlatform);
        }
    }

    private void BuildPlatform(PlatformCoordinate platformCoordinate, bool isBottomPlatform)
    {
        if (isBottomPlatform)
        {
            for (int x = platformCoordinate.X1; x < platformCoordinate.X2; x++)
            {
                for (int y = 0; y < platformCoordinate.Y; y++)
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
                for (int y = 0; y < platformCoordinate.Y; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    upperPlatform.SetTile(position, platformTile);
                }
            }
        }
    }
}
