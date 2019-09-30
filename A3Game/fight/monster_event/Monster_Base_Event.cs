using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
using DG.Tweening;

public class Monster_Base_Event : MonoBehaviour
{
    public const int desTime = 10;
    public MonsterRole m_monRole;
    //public static Dictionary<string, GameObject> bultList = new Dictionary<string, GameObject>();
    public void onSAI(float time)
    {
        m_monRole.SleepAI(time);
    }
    public void onSK(string param)
    {//技能蓄力时间
        string[] arr = param.Split(',');
        if (arr.Length < 5)
            return;
        m_monRole.FreezeAni(float.Parse(arr[0]), float.Parse(arr[1]));
        int type = int.Parse(arr[2]);

        GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("FX_monsterSFX_com_yujing_" + type.ToString());
        if (obj_prefab != null)
        {
            GameObject fx_inst = GameObject.Instantiate(obj_prefab, transform.position, transform.rotation) as GameObject;
            float scale = float.Parse(arr[4]);
            fx_inst.transform.localScale = new Vector3(scale, scale, scale);
            fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
            GameObject.Destroy(fx_inst, float.Parse(arr[0]));
        }

        int fx_id = int.Parse(arr[3]);
        if (fx_id > 0)
        {
            GameObject fx_prefab = GAMEAPI.ABFight_LoadPrefab("FX_monsterSFX_com_FX_monster_" + fx_id.ToString());
            GameObject fx_inst = GameObject.Instantiate(fx_prefab, transform.position, transform.rotation) as GameObject;
            fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
            GameObject.Destroy(fx_inst, float.Parse(arr[0]));
        }
    }

    //加入地形特效
    private void onTFX(int id)
    {
        if (id < 100) //小于100的常规地形特效，100以上是怪物自身的地形特效需要特殊制作
        {
            Vector3 born_pos;
            if (id >= 11 && id <= 20) //左脚
            {
                born_pos = m_monRole.m_LeftFoot.position;
            }
            else if (id >= 21 && id <= 30) //右脚
            {
                born_pos = m_monRole.m_RightFoot.position;
            }
            else if (id >= 31 && id <= 40) //左手
            {
                born_pos = m_monRole.m_LeftHand.position;
            }
            else if (id >= 41 && id <= 50) //右手
            {
                born_pos = m_monRole.m_RightHand.position;
            }
            else
            {
                born_pos = this.transform.position;
            }

            GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_TFX_Prefabs[id % 10], born_pos, transform.rotation) as GameObject;
            fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
            GameObject.Destroy(fx_inst, 1f);
        }
    }

    private void onNewbie(string str)
    {
        NewbieTeachMgr.getInstance().add(str, -1);
    }

    //加入特效
    private void onFX(int id)
    {
        SceneFXMgr.Instantiate("FX_FX_" + id.ToString(), transform.position, transform.rotation, 2f);
    }


    public void onSFX_EFF(string id)
    {
        if (!MonsterMgr._inst.dMonEff.ContainsKey(id))
        {
            return;
        }
        MonEffData one = MonsterMgr._inst.dMonEff[id];

        Quaternion rotation = transform.rotation;

        Quaternion add_rotation = Quaternion.Euler(0, one.rotation, 0);

        Vector3 pos = transform.position + add_rotation * transform.forward * one.f;
        pos.y += one.y;

        if (one.romote)
        {//子弹类型
         //技能eff展示子弹类型 （定点，必中） 
            if (m_monRole.m_LockRole != null && m_monRole.m_LockRole.m_curModel != null)
            {
                m_monRole.TurnToRole(m_monRole.m_LockRole, false);
                GameObject bult_go = GAMEAPI.ABFight_LoadPrefab(one.file);

                GameObject bult = GameObject.Instantiate(bult_go, pos, rotation) as GameObject;
                Destroy(bult, desTime);
                bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

                Transform real_track = bult.transform.FindChild("t");
                if (real_track != null)
                {
                    if (real_track.GetComponent<Animator>() != null)
                        real_track.GetComponent<Animator>().enabled = false;
                    real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

                    float dis = Vector3.Distance(transform.position, m_monRole.m_LockRole.m_curModel.position);
                    Vector3 end_pos = m_monRole.m_LockRole.m_curModel.position;
                    end_pos.y += m_monRole.m_LockRole.headOffset.y * 3 / 4;

                    Tweener tween1 = bult.transform.DOLocalMove(end_pos, dis * 0.03f / one.speed);
                    tween1.SetUpdate(true);
                    int rang = UnityEngine.Random.Range(0, 4);
                    switch (rang)
                    {
                        case 1:
                            tween1.SetEase(Ease.InQuad);
                            break;
                        case 2:
                            tween1.SetEase(Ease.InCirc);
                            break;
                        case 3:
                            tween1.SetEase(Ease.InCubic);
                            break;
                        case 4:
                            tween1.SetEase(Ease.InExpo);
                            break;
                    }
                    tween1.OnComplete(delegate ()
                    {
                        if (bult.transform != null && bult.transform.FindChild("t") != null)
                        {
                            Transform real_fx = real_track.FindChild("f");
                            if (real_fx != null)
                            {
                                real_fx.GetComponent<Animator>().SetTrigger(EnumAni.ANI_T_FXDEAD);
                            }
                            GameObject.Destroy(bult, 2f);
                        }

                    });
                }
            }
        }
        else if (one.Lockpos)
        {//目标位置特效

            if (m_monRole.ismapfx && m_monRole.fxvec != Vector3 .zero)
            {
                GameObject pre = GAMEAPI.ABFight_LoadPrefab(one.file);
                GameObject bult = GameObject.Instantiate(pre, m_monRole.fxvec, rotation) as GameObject;
                bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);
                int ss = 0;
                Tweener tween1 = DOTween.To(() => ss, (float s) => {
                }, (float)0, 10f);
                tween1.OnComplete(delegate () {
                    if (bult.GetComponent<Animator>() != null)
                    {
                        bult.GetComponent<Animator>().enabled = false;
                        //bult.transform.FindChild("loop").gameObject.SetActive(false);
                        //bult.transform.FindChild("end").gameObject.SetActive(true);
                    }
                    GameObject.Destroy(bult, 4f);
                });
               // SceneFXMgr.Instantiate(one.file, m_monRole.fxvec, rotation, 4f);
                m_monRole.ismapfx = false;
            }
            else 
                SceneFXMgr.Instantiate(one.file, m_monRole.m_LockRole.m_curModel.position, rotation, 4f);
        }
        else
        {
            SceneFXMgr.Instantiate(one.file, pos, rotation, 4f);
        }

        if (one.sound != "null")
        {
            MediaClient.instance.PlaySoundUrl("audio_eff_" + one.sound, false, null);
        }
    }

    public void onSound(string path)
    {
        if (m_monRole != null)
            MediaClient.instance.PlaySoundUrl("audio_monster_" + path, false, null);
    }

    public void onJump(float x, float y)
    {
        float to_x = x / GameConstant.PIXEL_TRANS_UNITYPOS;
        float to_y = y / GameConstant.PIXEL_TRANS_UNITYPOS;

        NavMeshHit hit;
        Vector3 endpos = new Vector3(to_x, transform.position.y, to_y);
        if (GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3342])
        {//解决多层阻挡点寻路的问题

        }
        else
        {
            NavMesh.SamplePosition(endpos, out hit, 100f, m_monRole.m_layer);
            endpos = hit.position;
        }
        float dis = Vector3.Distance(transform.position, endpos);
        m_monRole.SetDestPos(endpos);
        transform.DOJump(endpos, 0.2f * dis + endpos.y, 1, 0.01f);

    }

    public void onShake(string param)
    {
        if (m_monRole == null)
            return;
        //if (!m_monRole.isBoos)
        //    return;

        string[] arr = param.Split(',');
        if (arr.Length < 3)
            return;

        SceneCamera.cameraShake(float.Parse(arr[0]), int.Parse(arr[1]), float.Parse(arr[2]));
    }

    public void onBorned()
    {
        m_monRole.onBorned();
    }

    public void onDeadEnd()
    {
        m_monRole.onDeadEnd();
    }
    public void onShow(int id)
    {
    }
    public void onMSSFX_EFF(string param)
    {
    }
}
