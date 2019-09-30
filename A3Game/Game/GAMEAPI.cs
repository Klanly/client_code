using UnityEngine;

public static class GAMEAPI
{
    public delegate void objDeleget1(string abname, string assetname);
    public delegate bool objDeleget2();
    public delegate void objDeleget3(string abname);
    public delegate Object objDeleget4(string abname, string assetname);
    public delegate void objDeleget5(string abname, string assetname, System.Action<Object, System.Object> call_back, System.Object back_data);
    public delegate T objDeleget6<T>(string abname, string assetname);
    public delegate void objDeleget7();

    public static objDeleget1 LoadAsset_Async;
    public static objDeleget2 Res_Async_Loaded;
    public static objDeleget3 Unload_Asset;
    public static objDeleget4 LoadAsset_Obj;
    public static objDeleget5 LoadOneAsset_Async;
    public static objDeleget6<GameObject> LoadNow_GameObject_OneAsset;
    public static objDeleget6<Sprite> LoadNow_Sprite_OneAsset;
    public static objDeleget7 ClearAllOneAsset;
    public static objDeleget3 KeepOneAsset;
    public static objDeleget3 KillOneAsset;

    public static GameObject ABFight_LoadPrefab(string asset_name)
    {
        GameObject obj_prefab = MuGame.InterfaceMgr.doGetAssert("ab_fight.assetbundle", asset_name);
        if (obj_prefab == null)
        {
            return U3DAPI.DEF_GAMEOBJ;
        }

        return obj_prefab;
    }

    public static void ABModel_LoadTexture2D(string asset_name, System.Action<Object, System.Object> call_back, System.Object back_data)
    {
        LoadOneAsset_Async(asset_name + ".assetbundle", asset_name, call_back, back_data);
    }

    public static void ABModel_LoadGameObject(string asset_name, System.Action<Object, System.Object> call_back, System.Object back_data)
    {
        LoadOneAsset_Async(asset_name + ".assetbundle", asset_name, call_back, back_data);
    }

    public static GameObject ABModel_LoadNow_GameObject(string asset_name)
    {
        GameObject loaded_one = LoadNow_GameObject_OneAsset(asset_name + ".assetbundle", asset_name);
        if( loaded_one == null )
        {
            return U3DAPI.DEF_GAMEOBJ;
        }

        return loaded_one;
    }

    public static GameObject ABUI_LoadPrefab(string asset_name)
    {
        GameObject obj_prefab = MuGame.InterfaceMgr.doGetAssert("ab_ui.assetbundle", asset_name);
        if (obj_prefab == null)
        {
            return U3DAPI.DEF_GAMEOBJ;
        }

        return obj_prefab;
    }

    public static GameObject ABLayer_LoadNow_GameObject(string asset_name)
    {
        GameObject loaded_one = LoadNow_GameObject_OneAsset(asset_name + ".assetbundle", asset_name);
        return loaded_one;
    }

    public static void ABLayer_LoadGameObject(string asset_name, System.Action<Object, System.Object> call_back, System.Object back_data)
    {
        LoadOneAsset_Async(asset_name + ".assetbundle", asset_name, call_back, back_data);
    }

    public static Sprite ABUI_LoadSprite(string asset_name)
    {
        Sprite loaded_one = LoadNow_Sprite_OneAsset(asset_name + ".assetbundle", asset_name);
        if (loaded_one == null)
        {
            return U3DAPI.DEF_SPRITE;
        }

        return loaded_one;
    }

    public static void ABAUDIO_LoadAudioClip(string asset_name, System.Action<Object, System.Object> call_back, System.Object back_data)
    {
        LoadOneAsset_Async(asset_name + ".assetbundle", asset_name, call_back, back_data);
    }
}

