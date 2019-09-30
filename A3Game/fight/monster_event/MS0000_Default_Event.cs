using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;
using DG.Tweening;

public class MS0000_Default_Event : Monster_Base_Event
{

    public string effect;
    public Vector3 vec;
    private void onSFX(string id)
    {
        if (SceneCamera.m_nSkillEff_Level > 1 && m_monRole.masterid != PlayerModel.getInstance().cid)
            return;

        SceneFXMgr.Instantiate("FX_zhsFX_slef_FX_" + id, transform.position, transform.rotation, 1.5f);
    }

    public void onBullet(int type)
    {
       
    }

    // 因为没有多余的动作 直接用攻击动作代替 释放技能动作  美术那边加不了动作 原文件找不到   为了不影响 怪物 因为召唤兽模型做东都是 直接拿怪物的
    public void onMSSFX_EFF() {

        onSFX_EFF();

    }

    public void onSFX_EFF()
    {
        if (SceneCamera.m_nSkillEff_Level > 1 && m_monRole.masterid != PlayerModel.getInstance().cid)
            return;

        if (effect == null || !MonsterMgr._inst.dMonEff.ContainsKey(effect))
        {
            return;
        }
        MonEffData one = MonsterMgr._inst.dMonEff[effect];

        Quaternion rotation = transform.rotation;
        Quaternion add_rotation = new Quaternion();
        add_rotation.y = one.rotation / 90;
        add_rotation.w = 1;
        rotation.y += one.rotation / 90;

        Vector3 pos = transform.position + add_rotation * transform.forward * one.f;
        pos.y += one.y;

        if (one.romote)
        {//子弹类型
         //技能eff展示子弹类型 （定点，必中） 
            if (m_monRole.m_LockRole != null)
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
                        if (bult != null)
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
           // m_monRole.TurnToPos(vec);
            GameObject pre = GAMEAPI.ABFight_LoadPrefab(one.file);
            GameObject bult = GameObject.Instantiate(pre, vec, rotation) as GameObject;
            bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);
            int ss = 0;
            Tweener tween1 = DOTween.To(() => ss, (float s) => {
            }, (float)0, 10f);
            tween1.OnComplete(delegate () {
               if( bult.GetComponent<Animator>() != null)
                {
                    bult.GetComponent<Animator>().enabled = false;
                    //bult.transform.FindChild("loop").gameObject.SetActive(false);
                    //bult.transform.FindChild("end").gameObject.SetActive(true);

                    GameObject.Destroy(bult, 4f);
                }
            });
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
}
