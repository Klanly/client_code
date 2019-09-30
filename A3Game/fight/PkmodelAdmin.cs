using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MuGame.role;
using MuGame;
using Cross;

namespace MuGame
{
    //0:和平
    //1:全部
    //2:组队
    //3:军团
    //4:红名
    class PkmodelAdmin
    {
        //判断一下几转..
        //攻击伤害的刷新
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldList"></param>被攻击的所有
        /// <param name="isProfession"></param>被攻击的人
        static public void RefreshList(List<uint> oldList, List<uint> isProfession)
        {
            switch (PlayerModel.getInstance().now_pkState)
            {

                case 0:
                    for (int i = 0; i < isProfession.Count; i++)
                    {
                        if (oldList.Contains(isProfession[i]))
                        {
                            if(OtherPlayerMgr._inst.m_mapOtherPlayer.ContainsKey(isProfession[i]))
                            {
                                if(!OtherPlayerMgr._inst.m_mapOtherPlayer[isProfession[i]].havefanjibuff)
                                    oldList.Remove(isProfession[i]);
                            }
                            
                        }
                    }
                    break;
                case 1:
                    return;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    for (int i = 0; i < isProfession.Count; i++)
                    {
                        if (OtherPlayerMgr._inst != null && OtherPlayerMgr._inst.m_mapOtherPlayer.Count > 0)
                        {
                            if (OtherPlayerMgr._inst.m_mapOtherPlayer.ContainsKey(isProfession[i]))
                            {
                                if (OtherPlayerMgr._inst.m_mapOtherPlayer[isProfession[i]].rednm <= 0)
                                    oldList.Remove(isProfession[i]);
                            }
                        }
                    }
                    break;

            };


        }
        //鼠标点中lock图像颜色的刷新
        static int pkstat = -1;
        static MeshRenderer skin = null;
        static BaseRole br = null;
        static  bool haveLockpeople = false;
        static public void RefreshShow(BaseRole LockRole, bool havepeopleLv = false, bool havpeoplerednam = false)
        {
            if (br == LockRole && PlayerModel.getInstance().now_pkState == pkstat && skin != null && havepeopleLv == false && havpeoplerednam == false)
                return;
            br = LockRole;
            skin = SelfRole.s_LockFX.gameObject.GetComponent<MeshRenderer>();
            if (skin == null) return;

            if (LockRole is ProfessionRole)
            {
                if (OtherPlayerMgr._inst.m_mapOtherPlayer.ContainsKey(LockRole.m_unIID) && OtherPlayerMgr._inst.m_mapOtherPlayer[LockRole.m_unIID].zhuan < 1)
                {
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(0f, 1f, 0f, 1f));
                    pkstat = PlayerModel.getInstance().now_pkState;
                    return;
                }
                if (!OtherPlayerMgr._inst.m_mapOtherPlayer.ContainsKey(LockRole.m_unIID))
                {//这里加下保护
                    SelfRole._inst.m_LockRole = null;
                }
            }
            else if (LockRole is MS0000)
            {
                if (((MS0000)LockRole).owner_cid == PlayerModel.getInstance().cid)
                {
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(0f, 1f, 0f, 1f));
                }
                else
                {
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(1f, 0f, 0f, 1f));
                }
                return;
            }
            else if (LockRole is MDC000)//镖车
            {

                if (((MDC000)LockRole).escort_name == A3_LegionModel.getInstance().myLegion.clname)
                {
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(0f, 1f, 0f, 1f));
                    
                }
                else
                {
                    if (((float)LockRole.curhp/ (float)LockRole.maxHp*100)<=20)
                    {
                        skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(0f, 1f, 0f, 1f));
                        return;
                    }
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(1f, 0f, 0f, 1f));
                }

                return;
            }
            else if (LockRole is MonsterRole)
            {
                skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(1f, 0f, 0f, 1f));
                pkstat = PlayerModel.getInstance().now_pkState;
                return;
            }
            pkstat = PlayerModel.getInstance().now_pkState;
            bool canattack = false;
            switch (PlayerModel.getInstance().now_pkState)
            {
                case 0:
                    if (LockRole is ProfessionRole)
                    {
                        if(!LockRole.havefanjibuff)
                           skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(0f, 1f, 0f, 1f));
                        else
                           skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(1f, 0f, 0f, 1f));
                    }
                    else if (LockRole == null)
                        skin = null;
                    break;
                case 1:
                    // cricle.startColor = new Color(1f, 0f, 0f, 1f);
                    skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(1f, 0f, 0f, 1f));
                    break;
                case 2:
                    // debug.Log("我的军团id：" + PlayerModel.getInstance().clanid+"他的军团id:"+ LockRole.m_unLegionID);
                    //debug.Log("是否同一队伍：" + TeamProxy.getInstance().MyTeamData.IsInMyTeam(LockRole.roleName)+"他的队伍id："+ LockRole.m_unTeamID);
                    if (LockRole == null)
                    {
                        skin = null;
                        return;
                    }
                    else
                    {

                        if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam(LockRole.roleName))
                            canattack = false;
                        else
                        {
                            if (PlayerModel.getInstance().clanid == 0)
                                canattack = true;
                            else
                                canattack = PlayerModel.getInstance().clanid == LockRole.m_unLegionID ? false : true;

                        }
                    }
                    if (canattack)
                    {
                        haveLockpeople = true;
                        skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(1f, 0f, 0f, 1f));
                    }
                    else
                    {
                        haveLockpeople = false;
                        skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(0f, 1f, 0f, 1f));
                    }
                    
                    break;
                case 3:
                   
                    break;
                case 4:
                    if (LockRole == null)
                        skin = null;
                    else
                    {
                        if (LockRole.rednm > 0)
                            skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(1f, 0f, 0f, 1f));
                        else
                            skin.material.SetColor(EnumShader.SPI_TINT_COLOR, new Color(0f, 1f, 0f, 1f));
                    }

                    break;
            };

           


        }
        //播放技能朝向的刷新
        static public BaseRole RefreshLockRoleTransform(BaseRole LockRole)
        {
            if (LockRole is MonsterRole)
                return LockRole;
            if (LockRole.isDead || LockRole == null)
                return null;
            switch (PlayerModel.getInstance().now_pkState)
            {
                case 0:
                    if (!LockRole.havefanjibuff)
                        return null;
                    else
                        return LockRole;

                case 1:
                    if (LockRole is ProfessionRole)
                    {
                        if (OtherPlayerMgr._inst.m_mapOtherPlayer[LockRole.m_unIID].zhuan < 1)
                            return null;
                    }
                    else
                        return LockRole;
                    break;
                case 2:
                    bool canattack = false;
                    if (LockRole is ProfessionRole)
                    {

                        if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam(LockRole.roleName))
                            canattack = false;
                        else
                        {
                            if (PlayerModel.getInstance().clanid == 0)
                                canattack = true;
                            else
                                canattack = PlayerModel.getInstance().clanid == LockRole.m_unLegionID ? false : true;

                        }
                    }
                    if (canattack)
                        return LockRole;
                    else
                        return null;
                case 3:
                    break;
                case 4:
                    if (LockRole is ProfessionRole && LockRole.rednm > 0)
                        return LockRole;
                    return null;
            };

            return LockRole;
        }
        //锁定技能的处理(能否攻击)
        static public bool RefreshLockSkill(BaseRole LockRole)
        {

            if (LockRole.isDead || LockRole == null)
                return false;
            if (LockRole is MonsterRole)
                return true;
            else
            {
                if (OtherPlayerMgr._inst.m_mapOtherPlayer[LockRole.m_unIID].zhuan < 1)
                    return false;
                switch (PlayerModel.getInstance().pk_state)
                {
                    case PK_TYPE.PK_PEACE:
                        if (!LockRole.havefanjibuff)
                            return false;
                        else
                            return true;
                        
                    case PK_TYPE.PK_PKALL:
                        return true;
                    case PK_TYPE.PK_TEAM:
                        bool canattack = false;
                        if (LockRole is ProfessionRole)
                        {

                            if (TeamProxy.getInstance().MyTeamData != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam(LockRole.roleName))
                                canattack = false;
                            else
                            {
                                if (PlayerModel.getInstance().clanid == 0)
                                    canattack = true;
                                else
                                    canattack = PlayerModel.getInstance().clanid == LockRole.m_unLegionID ? false : true;

                            }
                        }
                        if (canattack)
                            return true;
                        else
                            return false;
                    //return (LockRole is ProfessionRole && (TeamProxy.getInstance().MyTeamData?.itemTeamDataList.Exists((m) => m.cid == LockRole.m_unCID) ?? true));
                    case PK_TYPE.PK_LEGION:
                        break;
                    case PK_TYPE.PK_HERO:
                        if (LockRole.rednm > 0)
                            return true;
                        return false;
                    case PK_TYPE.Pk_SPOET:
                        bool canattack1 = false;
                        if (LockRole is ProfessionRole)
                        {
                            if (PlayerModel.getInstance().lvlsideid == LockRole.lvlsideid)
                            {
                                canattack1 = false;
                            }
                            else {
                                canattack1 = true;
                            }
                        }
                        return canattack1;
                };
            }
            return false;
        }
        //没有目标自动锁定最近的刷新(OtherPlayerMgr的FindNearestEnemyOne里面筛选)
        static public BaseRole RefreshLockRole()
        {
            switch (PlayerModel.getInstance().now_pkState)
            {
                case 0:
                    return OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.position, false, null, false, PK_TYPE.PK_PEACE);
                case 1:
                    return OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.position);
                case 2:
                    return OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.position,false,null,false, PK_TYPE.PK_TEAM);
                  //  break;
                case 3:
                    break;
                case 4:
                    return OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.position, true);
                case 5:
                    return OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.position, false, null, false, PK_TYPE.Pk_SPOET);
                    break;
            };
            return null;
        }

    }
}
