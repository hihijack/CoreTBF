using UnityEngine;


public class GameResLoader
{
    //public static void Load<T>(string assetName, Action<T> onComplete) where T : UnityEngine.Object
    //{
    //     Addressables.LoadAssetAsync<T>(assetName).Completed += (a) => { onComplete(a.Result); };
    //}

    public static T Load<T>(string assetName) where T : UnityEngine.Object
    {
        return Resources.Load<T>(assetName);
    }
}
