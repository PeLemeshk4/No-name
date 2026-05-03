using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static GenerateMap;
using static UnityEngine.Audio.GeneratorInstance;

public class GenerateMap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile tile;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;

    private List<Vector2Int> points = new List<Vector2Int>() { new Vector2Int(0, 0) };
    private int safePlatformLength = 20;
    private float yMax = 5;


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

    private void Awake()
    {
        parameters.g = player.GetComponent<Rigidbody2D>().gravityScale * Physics2D.gravity.y;
        parameters.m = player.GetComponent<Rigidbody2D>().mass;
        parameters.jumpPower = player.GetComponent<JumpAbility>().Power;
        parameters.jumpTime = Mathf.Abs(parameters.jumpPower / (parameters.m * parameters.g));
        parameters.jumpHeight = ((parameters.jumpPower / parameters.m) * parameters.jumpTime) 
            + (parameters.g * (parameters.jumpTime * parameters.jumpTime)) / 2;
        parameters.speed = player.GetComponent<MoveAbility>().Speed;
        parameters.dashLength = player.GetComponent<DashAbility>().Length;
        yMax = cam.orthographicSize * 2 - 1;

        AddSafePlatform();
        for (int i = 0; i < 10; i++)
        {
            AddRandomPlatform();
        }

        BuildMap();
    }

    private void Generate()
    {

    }

    private void AddRandomPlatform()
    {
        int last = points.Count - 1;
        Vector2Int point = GetRandomPossibilityPoint(points[last]);

        points.Add(new Vector2Int(point.x, point.y));
        points.Add(new Vector2Int(point.x + 10, point.y));
    }

    private Vector2Int GetRandomPossibilityPoint(Vector2Int previosPoint)
    {
        float yPossibility = parameters.jumpHeight + parameters.dashLength;
        float maxDY = Mathf.Min(yPossibility, yMax - previosPoint.y);
        int dy = (int)Random.Range(-previosPoint.y, maxDY);
        float maxDX = Mathf.Sqrt((dy - parameters.jumpHeight - parameters.dashLength) * 
            2 * (parameters.speed * parameters.speed) / parameters.g) + (parameters.speed * parameters.jumpTime);
        int dx = (int)Random.Range(0, maxDX);

        return new Vector2Int(previosPoint.x + dx, previosPoint.y + dy);
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
                for (int y1 = y; y1 >= 0; y1--)
                {
                    Vector3Int position = new Vector3Int(x, y1, 0);
                    
                    tilemap.SetTile(position, tile);
                }      
            }
        }
    }

}
