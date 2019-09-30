using System;
using UnityEngine;
using System.Collections;

static public class SceneTFX
{
    static public GameObject[] m_TFX_Prefabs = { null, null, null, null, null, null, null, null, null, null };// = new GameObject[10];
    static public GameObject[] m_Bullet_Prefabs = { null, null, null, null, null, null, null, null, null, null };// = new GameObject[10];
    static public GameObject[] m_HFX_Prefabs = { null, null, null, null, null, null, null, null, null, null };// = new GameObject[10];

    static public void InitScene()
    {
        for (int i = 0; i < 10; i++)
        {
            //重载了场景之后，都Destroy了。所以这里是不用Destroy的
            //GameObject.Destroy(m_TFX_Prefabs[i]);

            m_TFX_Prefabs[i] = null;
            GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("FX_TFX_" + i.ToString());
            if (obj_prefab != null)
            {
                m_TFX_Prefabs[i] = obj_prefab;
            }
            else
            {
                m_TFX_Prefabs[i] = U3DAPI.DEF_GAMEOBJ;// new GameObject();
            }

            m_Bullet_Prefabs[i] = null;
            GameObject blt_prefab = GAMEAPI.ABFight_LoadPrefab("bullet_b" + i.ToString());
            if (blt_prefab != null)
            {
                m_Bullet_Prefabs[i] = blt_prefab;
            }
            else
            {
                m_Bullet_Prefabs[i] = U3DAPI.DEF_GAMEOBJ;
            }

            m_HFX_Prefabs[i] = null;
            GameObject hfx_prefab = GAMEAPI.ABFight_LoadPrefab("FX_HFX_HFX_" + i.ToString());
            if (hfx_prefab != null)
            {
                m_HFX_Prefabs[i] = hfx_prefab;
            }
            else
            {
                m_HFX_Prefabs[i] = U3DAPI.DEF_GAMEOBJ;
            }

        }
    }


}
