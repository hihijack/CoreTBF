using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameResLoader
{
    public static void Load<T>(string assetName, Action<T> onComplete) where T : UnityEngine.Object
    {
         Addressables.LoadAssetAsync<T>(assetName).Completed += (a) => { onComplete(a.Result); };
    }
}
