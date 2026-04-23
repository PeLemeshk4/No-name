using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    private List<string> paths = new List<string>() { PrefabPaths.WeaponPrefab };

    private void Awake()
    {
        foreach (string path in paths)
        {
            StartCoroutine(AssetLoader.LoadAsync<GameObject>(path,
                onSuccess: name => Debug.Log(name),
                onError: message => Debug.Log(message)));
        }

    }
}
