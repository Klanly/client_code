using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AssetResInfo
{
    public GameObject m_AB_Asset;
    public Vector3 m_position;
    public Quaternion m_rotation;
    public float m_time;
    public void Instantiate(string path, Vector3 position, Quaternion rotation, float time)
    {
        if (m_AB_Asset != null)
        {
            GameObject fx_inst = GameObject.Instantiate(m_AB_Asset, position, rotation) as GameObject;

            if (SceneCamera.m_nSkillEff_Level > 1)
            {//隐藏部分特效
                Transform hide = fx_inst.transform.FindChild("hide");
                if (hide != null)
                    hide.gameObject.SetActive(false);
            }

            fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
            GameObject.Destroy(fx_inst, time);
        }
        else
        {
            m_position = position;
            m_rotation = rotation;
            m_time = time;
        }
    }
}


public class SceneFXMgr : MonoBehaviour
{
    static private SceneFXMgr _inst = null;

    static private Dictionary<string, AssetResInfo> m_mapAssetsRes = new Dictionary<string, AssetResInfo>();
    //static private Dictionary<string, AssetResInfo> m_mapLoadingRes = new Dictionary<string, AssetResInfo>();
    //static string m_bAsync_Key = null;
    static public void Instantiate(string path, Vector3 position, Quaternion rotation, float time)
    {
        if (false == m_mapAssetsRes.ContainsKey(path))
        {
            AssetResInfo arinfo = new AssetResInfo();
            arinfo.m_AB_Asset = GAMEAPI.ABFight_LoadPrefab(path);
            m_mapAssetsRes[path] = arinfo;
            //m_mapLoadingRes[path] = arinfo;
        }

        m_mapAssetsRes[path].Instantiate(path, position, rotation, time);
    }

    void Awake()
    {
        _inst = this;
        Application.DontDestroyOnLoad(this.gameObject);
    }

    public static void StartLoadScene(string name)
    {
        if( _inst != null )
            _inst.StartCoroutine(_inst.LoadGameScene(name));
    }

    private IEnumerator LoadGameScene(string name)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        yield return async;

        GAMEAPI.Unload_Asset(name + ".assetbundle");
    }

    
    //IEnumerator Async_Load()
    //{
    //    ////mAsyn = Application.LoadLevelAsync(m_strLoadSceneName);
    //    //while (true)
    //    //{
    //    //    debug.Log("异步加载........");
    //    //    yield return null;
    //    //}
    //    ////yield return false;

    //    m_mapAssetsRes[m_bAsync_Key].LoadAsync(m_bAsync_Key);
    //    yield return m_mapAssetsRes[m_bAsync_Key].m_ResReq;

    //    m_mapAssetsRes[m_bAsync_Key].CheckFirstLoad();

    //    m_mapLoadingRes.Remove(m_bAsync_Key);
    //    m_bAsync_Key = null;
    //    yield return null;
    //}

    //void Update()
    //{
    //    //debug.Log("m_mapLoadingRes   " + m_mapLoadingRes.Count);
    //    if (m_bAsync_Key == null)
    //    {
    //        foreach (string strkey in m_mapLoadingRes.Keys)
    //        {
    //            //debug.Log("加载模型数据");
    //            m_bAsync_Key = strkey;
    //            StartCoroutine("Async_Load");
    //            return;
    //        }
    //    }
    //}
}

