using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    [SerializeField] private float maxPercentOffset = 70;

    private List<Vector2Int> downFloor = new List<Vector2Int>() { new Vector2Int(0, 0) };

    private int safePlatformLength = 30;
    private int maxOffset;

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

    public List<PlatformCoordinate> GeneratePlatform(int count, int minLength, int maxLength, int totalLength)
    {
        int remainCount = count;
        int remainLength = totalLength;

        List<PlatformCoordinate> newPlatform = new List<PlatformCoordinate>();

        newPlatform.Add(GetPlatformCoordinate(0, safePlatformLength - 1, 1));
        remainCount--;
        remainLength -= safePlatformLength;

        for (int i = 0; i < count - 1; i++)
        {
            //Vector2Int nextPoint = GetRandomPossibilityPoint(newPlatform[i].X2, newPlatform[i].Y);
            Vector2Int nextPoint = new Vector2Int(newPlatform[i].X2 + 1, Random.Range(0, maxOffset));

            (int, int) newLength = GetNewLength(minLength, maxLength, remainCount, remainLength);
            int platformLength = Random.Range(newLength.Item1, newLength.Item2);
            newPlatform.Add(GetPlatformCoordinate(nextPoint.x, nextPoint.x + platformLength - 1, nextPoint.y));
            remainCount--;
            remainLength -= platformLength;
        }
        return newPlatform;
    }

    private (int, int) GetNewLength(int minLength, int maxLength, int count, int totalLength)
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
