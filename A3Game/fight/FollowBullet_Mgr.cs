using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using MuGame;
using Cross;

//跟踪类子弹管理器

public class OneFollowBlt
{
    public uint id;
    public Vector3 beginpos = Vector3.zero;
    public BaseRole locker;
    public HitData blt_hd;
    public Animator aniTrack;
    public Animator aniFx;
    public float costtime;
    public float maxtime;
}

static public class FollowBullet_Mgr
{
    static public uint m_unFollowBulletMakeID = 0;
    static public Dictionary<uint, OneFollowBlt> m_mapFollowBullet = new Dictionary<uint, OneFollowBlt>();

    static public void AddBullet(BaseRole locker, HitData bullet, float time)
    {
        m_unFollowBulletMakeID++;

        OneFollowBlt blt = new OneFollowBlt();
        blt.id = m_unFollowBulletMakeID;
        blt.beginpos = bullet.transform.position;
        blt.locker = locker;
        blt.blt_hd = bullet;
        blt.costtime = 0;
        blt.maxtime = time;

        Transform real_track = bullet.transform.FindChild("t");
        if (real_track != null)
        {
            //HitData hd = m_linkProfessionRole.Link_PRBullet(2004, 2f, bult, real_track);
            //hd.m_Color_Main = Color.gray;
            //hd.m_Color_Rim = Color.white;

            blt.aniTrack = real_track.GetComponent<Animator>();
            real_track.gameObject.layer = EnumLayer.LM_BT_FIGHT;

            Transform real_fx = real_track.FindChild("f");
            if (real_fx != null)
            {
                blt.aniFx = real_fx.GetComponent<Animator>();
            }
        }



        m_mapFollowBullet.Add(m_unFollowBulletMakeID, blt);
    }

    static public void clear()
    {
        m_mapFollowBullet.Clear();
    }

    static public void FrameMove(float fdt)
    {
        List<uint> need_del = new List<uint>();
        foreach (OneFollowBlt p in m_mapFollowBullet.Values)
        {
            try
            {
                float t = p.costtime / p.maxtime;
                p.costtime += fdt;
                if (t > 1f)
                {
                    if (p.aniTrack != null) p.aniTrack.speed = 0;
                    if (p.aniFx != null) p.aniFx.SetTrigger(EnumAni.ANI_T_FXDEAD);

                    GameObject.Destroy(p.blt_hd.m_hdRootObj, 1f);

                    need_del.Add(p.id);
                    if (p.locker is MonsterRole)
                    {
                        MonsterRole mr = p.locker as MonsterRole;
                        if (mr.isfake)
                        {
                            mr.onHurt(p.blt_hd);
                        }
                        else
                        {
                            //如果是主角，请求服务器
                            if (p.blt_hd.m_CastRole == SelfRole._inst)
                            {
                                List<uint> list_hitted = new List<uint>();
                                list_hitted.Add(mr.m_unIID);

                                int lockid = -1;
                                if (SelfRole._inst.m_LockRole != null && SelfRole._inst.m_LockRole.m_unIID == mr.m_unIID)
                                {
                                    lockid = (int)SelfRole._inst.m_LockRole.m_unIID;
                                }

                                BattleProxy.getInstance().sendcast_target_skill(p.blt_hd.m_unSkillID, list_hitted, 0, lockid);
                                list_hitted = null;
                            }
                        }
                    }

                }
                else
                {
                    Vector3 t_pos = p.locker.m_curModel.position;
                    t_pos.y += 1.5f;
                    Vector3 pos = p.beginpos + (t_pos - p.beginpos) * t;
                    p.blt_hd.m_hdRootObj.transform.position = pos;
                }
            }
            catch (System.Exception ex)
            {
                need_del.Add(p.id);
                break;
            }

        }

        for (int i = 0; i < need_del.Count; i++)
        {
            m_mapFollowBullet.Remove(need_del[i]);
        }
    }

}

