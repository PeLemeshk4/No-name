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
    private int platformsCount = 0;
    private List<PlatformCoordinate> platforms;
    private List<List<Vector2Int>> puddles;
    private HashSet<int> puddlesNumbers;
    private List<List<(TypeEnemy, int)>> enemies;
    private HashSet<int> enemiesNumbers;

    public int Length { get { return length; } }
    public int MaxY { get { return maxY;  } }
    public int PlatformsCount { get { return platformsCount; } }
    public List<PlatformCoordinate> Platforms { get { return platforms; } }
    public List<List<Vector2Int>> Puddles { get { return puddles; } }
    public HashSet<int> PuddlesNumbers { get { return puddlesNumbers; } }
    public List<List<(TypeEnemy, int)>> Enemies { get { return enemies; } }
    public HashSet<int> EnemiesNumbers {  get { return enemiesNumbers; } }

    public MapFloor(int maxY)
    {
        platforms = new List<PlatformCoordinate>();
        enemies = new List<List<(TypeEnemy, int)>>();
        enemiesNumbers = new HashSet<int>();
        puddles = new List<List<Vector2Int>>();
        puddlesNumbers = new HashSet<int>();
        this.maxY = maxY;
    }

    public int GetIndexPlatform(int coordinate)
    {
        for (int i = 0; i < platformsCount; i++)
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
        platformsCount++;
        length = coordinate.X2;
        return true;
    }

    public bool AddPuddle(int platformNumber, int startX, int length)
    {
        if (startX < platforms[platformNumber].X1) return false;
        if (startX + length > platforms[platformNumber].X2) return false;

        puddles[platformNumber].Add(new Vector2Int(startX, startX + length));
        puddlesNumbers.Add(platformNumber);
        return true;
    }

    public bool AddEnemy(int platformNumber, TypeEnemy type, int coordinate)
    {
        if (platformNumber >= platformsCount) return false;
        if (coordinate < platforms[platformNumber].X1
            || coordinate > platforms[platformNumber].X2) return false;

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

    public MapFloor GenerateMapFloor(CMSEntity mapModel, bool platforms = true, bool puddles = true, bool enemies = true)
    {
        MapFloor floor = new MapFloor(maxOffset);

        if (platforms)
        {
            TagMapPlatforms tagMP = mapModel.Get<TagMapPlatforms>();
            GeneratePlatforms(floor, tagMP.Count, tagMP.MinLength, tagMP.MaxLength, tagMP.TotalLength);
        }
        if (puddles)
        {
            TagMapPuddles tagMPud = mapModel.Get<TagMapPuddles>();
            GeneratePuddles(floor, tagMPud.PlacesCount, tagMPud.P);
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
        int platformsCount = floor.PlatformsCount;

        int remainCount = count;
        int remainLength = totalLength;

        for (int i = platformsCount - 1; i < platformsCount + count; i++)
        {
            (int, int) minMaxLength = GetMinMaxLength(minLength, maxLength, remainCount, remainLength);
            int platformLength = Random.Range(minMaxLength.Item1, minMaxLength.Item2 + 1);
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

    public void GeneratePuddles(MapFloor floor, int placesCount, float p)
    {
        int mapLength = floor.Length;
        int spotsCount = (int)(placesCount * p);
        if (spotsCount == 0) return;
        List<int> puddleSpotNumbers = DistributeSpots(floor, spotsCount);

        foreach (int i in puddleSpotNumbers)
        {
            PlatformCoordinate coordinate = floor.Platforms[i];
            int x = Random.Range(coordinate.X1, coordinate.X2 - 2);
            floor.AddPuddle(i, x, 3);
        }
    }

    public void GenerateEnemies(MapFloor floor, int placesCount, float p)
    {
        int mapLength = floor.Length;
        int spotsCount = (int)(placesCount * p);
        if (spotsCount == 0) return;
        List<int> enemySpotNumbers = DistributeSpots(floor, spotsCount);

        foreach (int i in enemySpotNumbers)
        {
            PlatformCoordinate coordinate = floor.Platforms[i];
            int x = Random.Range(coordinate.X1, coordinate.X2);
            floor.AddEnemy(i, TypeEnemy.Melee, x);
        }
    }

    private List<int> DistributeSpots(MapFloor floor, int spotsCount)
    {
        if (spotsCount == 0) return null;

        List<int> spotsNumbers = new List<int>();

        LinkedList<(int, int)> places = new LinkedList<(int, int)>();
        for (int i = 0; i < floor.PlatformsCount; i++)
        {
            int length = floor.Platforms[i].X2 - floor.Platforms[i].X1;
            places.AddLast((i, length));
        }

        int d = floor.Length / spotsCount;

        int remainPlacesCount = floor.PlatformsCount;
        for (int i = 0; i < spotsCount; i++)
        {
            int placeNumber = Random.Range(0, remainPlacesCount);

            LinkedListNode<(int, int)> chosenPlace = places.First;
            for (int j = 0; j < placeNumber; j++)
            {
                chosenPlace = chosenPlace.Next;
            }
            spotsNumbers.Add(chosenPlace.Value.Item1);

            int remainD = d - chosenPlace.Value.Item2;
            if (remainD >= 0)
            {
                LinkedListNode<(int, int)> previosPlace = chosenPlace.Previous;
                if (previosPlace != null)
                {
                    int previosRemainD = remainD / 2 - previosPlace.Value.Item2;
                    while (previosRemainD >= 0)
                    {
                        LinkedListNode<(int, int)> newPreviosPlace = previosPlace.Previous;
                        places.Remove(previosPlace);
                        remainPlacesCount--;
                        if (newPreviosPlace == null) break;
                        previosPlace = newPreviosPlace;
                        previosRemainD = previosRemainD - previosPlace.Value.Item2;
                    }
                    if (previosRemainD < 0)
                    {
                        previosPlace.Value = (previosPlace.Value.Item1, Mathf.Abs(previosRemainD));
                    }
                }

                LinkedListNode<(int, int)> nextPlace = chosenPlace.Next;
                if (nextPlace != null)
                {
                    int nextRemainD = remainD / 2 - nextPlace.Value.Item2;
                    while (nextRemainD >= 0)
                    {
                        LinkedListNode<(int, int)> newNextPlace = nextPlace.Next;
                        places.Remove(nextPlace);
                        remainPlacesCount--;
                        if (newNextPlace == null) break; 
                        nextPlace = newNextPlace;
                        nextRemainD = nextRemainD - nextPlace.Value.Item2;
                    }
                    if (nextRemainD < 0)
                    {
                        nextPlace.Value = (nextPlace.Value.Item1, Mathf.Abs(nextRemainD));
                    }
                }

                places.Remove(chosenPlace);
                remainPlacesCount--;
            }
            else
            {
                chosenPlace.Value = (chosenPlace.Value.Item1, Mathf.Abs(remainD));
            }
        }

        return spotsNumbers;
    }
}
