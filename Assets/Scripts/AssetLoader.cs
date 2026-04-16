using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public static class AssetLoader
{
    public static async Task<GameObject> ClonePrefabAsync(string path, Transform parent)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(path);
        await handle.Task;
        GameObject instance = Object.Instantiate(handle.Result, parent);
        Addressables.Release(handle);
        return instance;
    }
}
