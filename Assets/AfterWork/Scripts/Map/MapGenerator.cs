using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemy
{
    Melee,
    Range
}

public class MapFloor
{
    private int length = 0;
    private int maxY = 0;
    private int platformCount = 0;
    private List<PlatformCoordinate> platforms;
    private List<List<(TypeEnemy, int)>> enemies;
    private HashSet<int> enemiesNumbers;
    private List<List<Vector2Int>> puddles;

    public int Length { get { return length; } }
    public int MaxY { get { return maxY;  } }
    public int PlatformCount { get { return platformCount; } }
    public List<PlatformCoordinate> Platforms { get { return platforms; } }
    public List<List<(TypeEnemy, int)>> Enemies { get { return enemies; } }
    public HashSet<int> EnemiesNumbers {  get { return enemiesNumbers; } }
    public List<List<Vector2Int>> Puddles { get { return puddles; } }

    public MapFloor(int maxY)
    {
        platforms = new List<PlatformCoordinate>();
        enemies = new List<List<(TypeEnemy, int)>>();
        enemiesNumbers = new HashSet<int>();
        puddles = new List<List<Vector2Int>>();
        this.maxY = maxY;
    }

    public int GetIndexPlatform(int coordinate)
    {
        for (int i = 0; i < platformCount; i++)
        {
            if (coordinate >= platforms[i].X1 && coordinate < platforms[i].X2)
            {
                return i;
            }
        }
        return -1;
    }

    public bool AddPlatform(int lengthPlatform, int y)
    {
        if (y > maxY) return false;
        if (lengthPlatform < 1) return false;

        int x1 = length;
        int x2 = x1 + lengthPlatform;
        PlatformCoordinate coordinate = new PlatformCoordinate(x1, x2, y);
        platforms.Add(coordinate);
        enemies.Add(new List<(TypeEnemy, int)>());
        puddles.Add(new List<Vector2Int>());
        platformCount++;
        length = coordinate.X2;
        return true;
    }

    public bool AddEnemy(TypeEnemy type, int platformNumber, int coordinate)
    {
        if (platformNumber >= platformCount) return false;
        if (coordinate < platforms[platformNumber].X1
            && coordinate > platforms[platformNumber].X2) return false;

        enemies[platformNumber].Add((type, coordinate));
        enemiesNumbers.Add(platformNumber);
        return true;
    }
}

public class MapGenerator
{
    private int maxOffset;

    public MapGenerator(CMSEntity playerModel, Camera cam, float maxPercentOffset)
    {
        maxOffset = (int)(cam.orthographicSize * maxPercentOffset);
    }

    public MapFloor GenerateMapFloor(CMSEntity mapModel, bool platforms = true, bool enemies = true)
    {
        MapFloor floor = new MapFloor(maxOffset);

        if (platforms)
        {
            TagMapPlatforms tagMP = mapModel.Get<TagMapPlatforms>();
            GeneratePlatforms(floor, tagMP.Count, tagMP.MinLength, tagMP.MaxLength, tagMP.TotalLength);
        }
        if (enemies)
        {
            TagMapEnemies tagME = mapModel.Get<TagMapEnemies>();
            GenerateEnemies(floor, tagME.PlacesCount, tagME.P);
        }

        return floor;
    }

    public void GeneratePlatforms(MapFloor floor, int count, int minLength, int maxLength, int totalLength)
    {
        List<PlatformCoordinate> platforms = floor.Platforms;
        int platformCount = floor.PlatformCount;

        int remainCount = count;
        int remainLength = totalLength;

        for (int i = platformCount - 1; i < platformCount + count; i++)
        {
            (int, int) minMaxLength = GetMinMaxLength(minLength, maxLength, remainCount, remainLength);
            int platformLength = Random.Range(minMaxLength.Item1, minMaxLength.Item2);
            int y = Random.Range(0, floor.MaxY);
            floor.AddPlatform(platformLength, y);
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

    public void GenerateEnemies(MapFloor floor, int placesCount, float p)
    {
        int mapLength = floor.Length;
        List<int> enemySpotNumbers = DistributeSpots(placesCount, (int)(placesCount * p));
        int spotLength = mapLength / placesCount;

        foreach (int i in enemySpotNumbers)
        {
            int minX = i * spotLength;
            int maxX = minX + spotLength;
            int x = Random.Range(minX, maxX);
            floor.AddEnemy(TypeEnemy.Melee, floor.GetIndexPlatform(x), x);
        }
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
