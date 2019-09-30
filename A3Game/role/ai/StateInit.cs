using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
    /// <summary>
    /// 状态机的初始化状态,以及一系列辅助API
    /// </summary>
    class StateInit : StateBase
    {
        public static StateInit Instance = new StateInit();

        //!--触发挂机状态时的起始位置,中心点
        private Vector3 _origin;
        public Vector3 Origin {
            get { return _origin; }
            set { if (!LockOriPos) _origin = value; }
        }
        
        //!--圆圈的半径
        public float Distance = float.MaxValue;
        public float DistanceNormal = float.MaxValue;
        //!--以中心点，为半径的圈中所包含的挂机点
        public List<Vector3> AutoPoints;

        //!--复活次数
        public int RespawnTimes = 10;

        //!--自动PK的追击距离
        public float PKDistance;

        //!--拾取的范围与最小间隔
        public float PickDistance;
        public float PickDistanceNormal;
        //public float PickInterval;

        //!--玩家在挂机中手动释放的技能,可能延时放出
        public int PreferedSkill;
        //!--玩家手动释放技能的时间
        private long releaseTime;
        // --使用Joystick拖拽多远后停止挂机
        public int maxAllowedDistance;
        // --是否锁定挂机开始的位置(只在开启自动战斗时才能被设置为true)
        private bool lockOriPos = false;
        public bool LockOriPos
        {
            get
            {
                return lockOriPos;
            }
            set
            {
                if (SelfRole.fsm.Autofighting || !value)
                    lockOriPos = value;
            }
        }
        public StateInit()
        {
            AutoPlayModel apmodel = AutoPlayModel.getInstance();
            DistanceNormal = apmodel.AutoplayXml.GetNode("mdis").getFloat("val");
            SXML xml = AutoPlayModel.getInstance().AutoplayXml;
            PickDistanceNormal = xml.GetNode("pickdis").getFloat("val");
            maxAllowedDistance = xml.GetNode("interrupt")?.getInt("val") ?? 3;
        }
        public override void Enter()
        {
            Origin = SelfRole._inst.m_curModel.position;
            
            AutoPoints = new List<Vector3>();
            GetProperWayPoints();

            if (AutoPlayModel.getInstance().RespawnLimit > 0)
            {//开启了自动复活限制
                RespawnTimes = AutoPlayModel.getInstance().RespawnUpBound;
            }
            else
            {
                RespawnTimes = int.MaxValue;
            }

            SXML xml = AutoPlayModel.getInstance().AutoplayXml;
            PKDistance = xml.GetNode("pkdis").getFloat("val");
                      
            //PickInterval = xml.GetNode("pickinterval").getFloat("val");

            PreferedSkill = -1;
        }

        public override void Execute(float delta_time)
        {
            SelfRole.fsm.ChangeState(StateIdle.Instance);
        }

        public override void Exit()
        {
        }

        //!--寻找符合的挂机点
        private void GetProperWayPoints()
        {
            AutoPlayModel apmodel = AutoPlayModel.getInstance();
            
            //DistanceInFB = apmodel.AutoplayXml.GetNode("mdis_fb").getFloat("val");
            Dictionary<int, List<Vector3>> wpdic = apmodel.mapWayPoint;
            
            List<Vector3> wps = null;
            wpdic.TryGetValue(GRMap.instance.m_nCurMapID, out wps);
            if (wps == null || wps.Count == 0)
                return;

            /*Distance = float.MaxValue;
            if (apmodel.Scope == 0)
                Distance = apmodel.AutoplayXml.GetNode("odis").getFloat("val");
            else if (apmodel.Scope == 1)*/ // Modified

            for (int i = 0; i < wps.Count; i++)
            {
                float dis = Vector3.Distance(Origin, wps[i]);
                if (dis <= Distance)
                {
                    //!--挂机点在圈内为期望的挂机点
                    AutoPoints.Add(new Vector3(wps[i].x, wps[i].y, wps[i].z));
                }
            }
        }

        //!--寻找最近的挂机点
        public Vector3 GetNearestWayPoint()
        {
            float mindis = float.MaxValue;
            Vector3 mypos = SelfRole._inst.m_curModel.position;
            Vector3 minpt = mypos;
            foreach (Vector3 pt in AutoPoints)
            {
                float dis = Vector3.Distance(pt, minpt);
                if (dis < mindis)
                {
                    mindis = dis;
                    minpt = pt;
                }
            }
            return minpt;
        }

        //!--是否超出挂机范围
        public bool IsOutOfAutoPlayRange()
        {
            float dis = Vector3.Distance(Origin, SelfRole._inst.m_curModel.position);
            return dis > Distance;
        }

        //!--获取可以使用的技能ID
        public int GetSkillCanUse()
        {
            if (PreferedSkill != -1)
            {
                long curtime = muNetCleint.instance.CurServerTimeStampMS;
                if (curtime - releaseTime >= 2000)
                {
                    PreferedSkill = -1;
                }
                else
                {
                    skill_a3Data skdata = Skill_a3Model.getInstance().skilldic[PreferedSkill];
                    if (skdata.mp <= PlayerModel.getInstance().mp &&
                        skdata.cdTime <= 0)
                    {
                        return PreferedSkill;
                    }
                }
            }
            //List<int> skils=new List<int>();
            //foreach (skill_a3Data skillInfo in Skill_a3Model.getInstance().skilllst) {
            //    if (skillInfo.now_lv > 0 && skillInfo.skillType2 == 0)
            //        skils.Add(skillInfo.skill_id);
            //}
            List<int> skils = new List<int>();
            List<int> allSkils = AutoPlayModel.getInstance().Skills.ToList();
            for (int i = 0; i < allSkils.Count; i++)
            {
                if (Skill_a3Model.getInstance().skilldic.ContainsKey(allSkils[i]))
                    if (Skill_a3Model.getInstance().skilldic[allSkils[i]].skillType2 <= 1)
                        skils.Add(allSkils[i]);
            }
            int skid = -1;

            //优先使用buff技能，buff技能不配置在挂机技能组。(现在去掉了)
          
   //         foreach (skill_a3Data sk in Skill_a3Model.getInstance().skilldic.Values) {
			//	if (sk.carr == PlayerModel.getInstance().profession &&
			//		sk.skill_id != a1_gamejoy.NORNAL_SKILL_ID &&
			//		sk.now_lv != 0 &&
   //                 sk.skillType2 == 1 &&
   //                 sk.mp <= PlayerModel.getInstance().mp &&
			//		 sk.cdTime <= 0
   //                  ) {
			//			return sk.skill_id;
			//	}
					
			//}

			//选择的技能按cd从长到短排序
			for (int i = 0; i < skils.Count; i++)
            {
				for (int j = i; j < skils.Count; j++)
                {
                    //if (Skill_a3Model.getInstance().skilldic.ContainsKey(skils[i]) && Skill_a3Model.getInstance().skilldic.ContainsKey(skils[j]))
                    //{
                        if (Skill_a3Model.getInstance().skilldic[skils[i]].cd > Skill_a3Model.getInstance().skilldic[skils[j]].cd)
                        {
                            int temp = skils[i];
                            skils[i] = skils[j];
                            skils[j] = temp;
                        }
                    //}
				}
			}

			for (int i = skils.Count - 1; i >= 0; i--)
            {
                if (skils[i] == 0)
                    continue;

                skid = skils[i];
                skill_a3Data skdata = Skill_a3Model.getInstance().skilldic[skid];
                if (skdata.mp > PlayerModel.getInstance().mp ||
                    skdata.cdTime > 0)
                    continue;

                return skid;
            }

            return a1_gamejoy.NORNAL_SKILL_ID;
        }

        //!--挂机状态下,玩家手动释放技能
        public void PlaySkillInAutoPlay(int tpid)
        {
            PreferedSkill = tpid;
            releaseTime = muNetCleint.instance.CurServerTimeStampMS;
        }
    }
}
