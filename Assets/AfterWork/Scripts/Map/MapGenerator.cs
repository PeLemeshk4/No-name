using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemy
{
    Melee,
    Range
}

public class MapFloor
{
    private int platformCount = 0;
    private List<PlatformCoordinate> platforms;
    private List<List<(TypeEnemy, Vector2Int)>> enemies;
    private List<List<Vector2Int>> puddles;

    public int PlatformCount { get { return platformCount; } }
    public List<PlatformCoordinate> Platforms { get { return platforms; } }
    public List<List<(TypeEnemy, Vector2Int)>> Enemies { get { return enemies; } }
    public List<List<Vector2Int>> Puddles { get { return puddles; } }

    public MapFloor()
    {
        platforms = new List<PlatformCoordinate>();
        enemies = new List<List<(TypeEnemy, Vector2Int)>>();
        puddles = new List<List<Vector2Int>>();
    }

    public bool AddPlatform(PlatformCoordinate coordinate)
    {
        if (coordinate.X2 < coordinate.X1) return false;
        if (coordinate.X1 != platforms[platforms.Count - 1].X2 + 1) return false;

        platforms.Add(coordinate);
        enemies.Add(new List<(TypeEnemy, Vector2Int)>());
        puddles.Add(new List<Vector2Int>());
        platformCount++;
        return true;
    }
}

public class MapGenerator
{
    private int safePlatformLength = 30;
    private int maxOffset;

    public MapGenerator(CMSEntity playerModel, Rigidbody2D rb, Camera cam, float maxPercentOffset)
    {
        maxOffset = (int)(cam.orthographicSize * maxPercentOffset / 100.0f);  
    }

    public MapFloor GenerateMapFloor(CMSEntity mapModel)
    {
        MapFloor floor = new MapFloor();

        TagMapPlatforms tagMP = mapModel.Get<TagMapPlatforms>();
        GeneratePlatforms(floor, tagMP.Count, tagMP.MinLength, tagMP.MaxLength, tagMP.TotalLength);


        return floor;
    }

    public void GeneratePlatforms(MapFloor floor, int count, int minLength, int maxLength, int totalLength)
    {
        List<PlatformCoordinate> platforms = floor.Platforms;
        int platformCount = floor.PlatformCount;

        int remainCount = count;
        int remainLength = totalLength;

        if (platformCount == 0)
        {
            (int, int) minMaxLength = GetMinMaxLength(minLength, maxLength, remainCount, remainLength);
            int platformLength = Random.Range(minMaxLength.Item1, minMaxLength.Item2);
            int x1 = 0;
            int x2 = platformLength - 1;
            int y = Random.Range(0, maxOffset);
            floor.AddPlatform(GetPlatformCoordinate(x1, x2, y));
            remainCount--;
            remainLength -= platformLength;
        }

        for (int i = platformCount - 1; i < platformCount + count - 1; i++)
        {
            (int, int) minMaxLength = GetMinMaxLength(minLength, maxLength, remainCount, remainLength);
            int platformLength = Random.Range(minMaxLength.Item1, minMaxLength.Item2);
            int x1 = platforms[i].X2 + 1;
            int x2 = platforms[i].X2 + platformLength;
            int y = Random.Range(0, maxOffset);
            floor.AddPlatform(GetPlatformCoordinate(x1, x2, y));
            remainCount--;
            remainLength -= platformLength;
        }
    }

    private (int, int) GetMinMaxLength(int minLength, int maxLength, int count, int totalLength)
    {
        if (count == 1)
        {
            return (totalLength, totalLength);
        }

        int newMinLength;
        int newMaxLength;

        float a1 = totalLength - (minLength * count);
        float b1 = Mathf.Abs(a1 / ((maxLength - minLength) * count));

        float a2 = totalLength - (maxLength * count);
        float b2 = Mathf.Abs(a2 / ((maxLength - minLength) * count));

        if (b1 > b2)
        {
            newMinLength = (int)(minLength + Mathf.Abs((maxLength - minLength) * b1));
            newMaxLength = maxLength;
        }
        else
        {
            newMinLength = minLength;
            newMaxLength = (int)(maxLength - Mathf.Abs((maxLength - minLength) * b2));
        }

        return (newMinLength, newMaxLength);
    }

    private PlatformCoordinate GetPlatformCoordinate(int x1, int x2, int y)
    {
        PlatformCoordinate platformCoordinate = new PlatformCoordinate();
        platformCoordinate.X1 = x1;
        platformCoordinate.X2 = x2;
        platformCoordinate.Y = y;
        return platformCoordinate;
    }

    public List<int> GenerateEnemies(int placesCount, float p, int mapLength)
    {
        List<int> enemySpotsCoordinates = new List<int>();

        List<int> enemySpotNumbers = DistributeSpots(placesCount, (int)(placesCount * p));
        int spotLength = mapLength / placesCount;

        foreach (int i in enemySpotNumbers)
        {
            int minX = i * spotLength;
            int maxX = minX + spotLength;
            int x = Random.Range(minX, maxX);
            enemySpotsCoordinates.Add(x);
        }

        return enemySpotsCoordinates;
    }

    private List<int> DistributeSpots(int placesCount, int spotsCount)
    {
        List<int> enemySpotsNumbers = new List<int>();

        LinkedList<int> placesNumbers = new LinkedList<int>();
        for (int i = 0; i < placesCount; i++)
        {
            placesNumbers.AddLast(i);
        }

        int d = (placesCount - spotsCount) / (2 * spotsCount - 2);

        int remainPlacesCount = placesCount;
        for (int i = 0; i < spotsCount; i++)
        {
            int spotNumber = Random.Range(0, remainPlacesCount);

            LinkedListNode<int> nodeWillDelete = placesNumbers.First;
            for (int j = 0; j < spotNumber; j++)
            {
                nodeWillDelete = nodeWillDelete.Next;
            }
            enemySpotsNumbers.Add(nodeWillDelete.Value);

            LinkedListNode<int> previosNode = nodeWillDelete.Previous;
            LinkedListNode<int> nextNode = nodeWillDelete.Next;
            placesNumbers.Remove(nodeWillDelete);
            int deletedPlaces = 1;
            for (int j = 0; j < d; j++)
            {
                if (previosNode != null)
                {
                    nodeWillDelete = previosNode;
                    previosNode = previosNode.Previous;
                    placesNumbers.Remove(nodeWillDelete);
                    deletedPlaces++;
                }

                if (nextNode != null)
                {
                    nodeWillDelete = nextNode;
                    nextNode = nextNode.Next;
                    placesNumbers.Remove(nodeWillDelete);
                    deletedPlaces++;
                }
            }
            remainPlacesCount -= deletedPlaces;
        }

        return enemySpotsNumbers;
    }
}
