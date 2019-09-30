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
    //3:群团
    //4:红名
    class PkmodelAdmin:MonoBehaviour
    {
        //判断一下几转..
        //攻击伤害的刷新
       static  public void  RefreshList(List<uint> oldList, List<uint> isProfession)
        {
            switch (PlayerModel.getInstance().now_pkState)
            {

                case 0:
                    for (int i = 0; i < isProfession.Count; i++)
                    {
                       if(oldList.Contains(isProfession[i]))
                       {
                           oldList.Remove(isProfession[i]);
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
                    break;
                    
            };


        }
        //鼠标点中lock图像颜色的刷新
       static public void RefreshShow(BaseRole LockRole)
       {         
           switch (PlayerModel.getInstance().now_pkState)
           {
               case 0:
                   if (LockRole is ProfessionRole)
                       SelfRole.s_LockFX.gameObject.transform.FindChild("fx").GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f);                  
                   break;
               case 1:
                   SelfRole.s_LockFX.gameObject.transform.FindChild("fx").GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
                   break;
               case 2:
                   break;
               case 3:
                   break;
               case 4:
                   break;
           };
           if (LockRole is ProfessionRole)
           {
               if (OtherPlayerMgr._inst.m_mapOtherPlayer[LockRole.m_unIID].zhuan < 1)
                  SelfRole.s_LockFX.gameObject.transform.FindChild("fx").GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f);                  
           }
           else if (LockRole is MonsterRole)
               SelfRole.s_LockFX.gameObject.transform.FindChild("fx").GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f);
          
       }
       //播放技能朝向的刷新
       static public BaseRole RefreshLockRoleTransform(BaseRole LockRole)
       {
           if (LockRole is MonsterRole)
               return LockRole;
           switch (PlayerModel.getInstance().now_pkState)
           {
               case 0:                
                   return null;
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
                   break;
               case 3:
                   break;
               case 4:
                   break;
           };

           return LockRole;
       }
        //锁定技能的处理(能否攻击)
       static public bool   RefreshLockSkill(BaseRole LockRole)
       {
           if (LockRole is MonsterRole)
               return true;
           else
           {
               switch (PlayerModel.getInstance().now_pkState)
               {
                   case 0:
                       return false;
                   case 1:
                       if (OtherPlayerMgr._inst.m_mapOtherPlayer[LockRole.m_unIID].zhuan < 1)
                           return false;
                       else                       
                           return true;

                   case 2:
                       break;
                   case 3:
                       break;
                   case 4:
                       break;
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
                   return null;
               case 1:
                   return OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.position);
               case 2:
                   break;
               case 3:
                   break;
               case 4:
                   break;
           };
           return null;
       }

     }
}
