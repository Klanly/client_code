using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;


namespace MuGame
{
    class M10003_Stl_Event : Monster_Base_Event
    {
        public M10003 m_StlRole;
        public void onBullet(int id)
        {
            if (id == 1)
            {
                GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("bullet_10003_bt1_b" + id.ToString());

                Vector3 pos = transform.position;
                GameObject bult = GameObject.Instantiate(obj_prefab, pos, transform.rotation) as GameObject;
                bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);

                Transform real_bt0 = bult.transform.FindChild("t/b/bt0");
                if (real_bt0 != null)
                {
                    HitData hd = real_bt0.gameObject.AddComponent<HitData>();
                    hd.m_CastRole = m_StlRole;
                    hd.m_vBornerPos = m_StlRole.m_curModel.position;
                    hd.m_ePK_Type = PK_TYPE.PK_LEGION;
                    hd.m_unPK_Param = m_StlRole.m_unLegionID;
                    hd.m_nDamage = 1888;
                    hd.m_nHitType = id;
                    hd.m_bOnlyHit = false;

                    real_bt0.gameObject.layer = EnumLayer.LM_BT_FIGHT;
                }

                //还有6小火焰的攻击
                for (int i = 1; i < 7; i++)
                {
                    Transform bt_follow = bult.transform.FindChild("t/b/bt" + i.ToString());
                    if (bt_follow != null)
                    {
                        HitData hd = bt_follow.gameObject.AddComponent<HitData>();
                        hd.m_CastRole = m_StlRole;
                        hd.m_vBornerPos = m_StlRole.m_curModel.position;
                        hd.m_ePK_Type = PK_TYPE.PK_LEGION;
                        hd.m_unPK_Param = m_monRole.m_unLegionID;
                        hd.m_nDamage = 488;
                        hd.m_nHitType = id;
                        hd.m_bOnlyHit = false;

                        bt_follow.gameObject.layer = EnumLayer.LM_BT_FIGHT;
                    }
                }


                GameObject.Destroy(bult, 4f);
            }
        }

        //自身特效
        private void onSFX(int id)
        {
            if (1 == id)
            {
                m_StlRole.PlayFire();
            }
        }

    }
}
