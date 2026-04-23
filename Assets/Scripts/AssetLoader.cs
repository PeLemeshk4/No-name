using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AssetLoader
{
    private static readonly Dictionary<string, Object> cache = new Dictionary<string, Object>();

    public static async Task<GameObject> ClonePrefabAsync(string path, Transform parent)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        await handle.Task;
        GameObject instance = Object.Instantiate(handle.Result, parent);
        Addressables.Release(handle);
        return instance;
    }

    public static IEnumerator LoadAsync<T>(string key, System.Action<T> onSuccess, System.Action<string> onError = null) where T : Object
    {
        if (string.IsNullOrEmpty(key))
        {
            onError?.Invoke("Addressable key is null or empty.");
            yield break;
        }

        if (cache.TryGetValue(key, out Object cachedObject) && cachedObject is T typedResult)
        {
            onSuccess?.Invoke(typedResult);
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<T>(key);
        
        while (!handle.IsDone)
        {
            yield return null;
        }
        

        if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
        {
            T loadedAsset = handle.Result;
            cache[key] = loadedAsset;
            onSuccess?.Invoke(loadedAsset);
        }
        else
        {
            string errorMessage = $"Addressable resource not found or failed to load: {key}";
            onError?.Invoke(errorMessage);
            Debug.LogError(errorMessage);
        }

        Addressables.Release(handle);
    }

    public static T GetObjectFromCache<T>(string key) where T : Object
    {
        if (cache.TryGetValue(key, out Object cachedObject) && cachedObject is T tObject)
        {
            return tObject;
        }
        return null;
    }
}
