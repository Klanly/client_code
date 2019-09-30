using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
    class StatePick : StateBase
    {
        public static StatePick Instance = new StatePick();
        //private readonly float dropDruation = 3.0f;
        //private float dropTimer = 0.0f;
        private Vector3 calc_ori, calc_tar, calc_cur;
        private DropItem pickTarget, nearest = null;
        private bool passed;
        //private uint pickiid = 0;

        //private float nextpick = 0;
        private int priority;
        public override void Enter()
        {
            passed = false;
            nearest = pickTarget = null;
            priority = SelfRole._inst.m_moveAgent.avoidancePriority;
            //debug.Log("========> Enter AI Pick");
            //if (nextpick > Time.time)
            //{
            //    SelfRole.fsm.RevertToPreviousState();
            //    return;
            //}
            //nextpick = Time.time + StateInit.Instance.PickInterval;      
        }
        public override void Execute(float delta_time)
        {
            //if (dropTimer > dropDruation)
            //{//限定拾取不超时
            //    SelfRole.fsm.ChangeState(StateIdle.Instance);
            //    return;
            //}
            //dropTimer += delta_time;       
            SelfRole._inst.m_moveAgent.avoidancePriority = 1;
            if (passed)
                nearest = 
                pickTarget = null;
            if (nearest == null || !nearest.gameObject)
            {//选取一个掉落目标,移动过去                
                DropItem drpitm;
                float min = float.MaxValue;
                Dictionary<uint, DropItem>.Enumerator etor = BaseRoomItem.instance.dDropItem_own.GetEnumerator();
                while (etor.MoveNext())
                {
                    drpitm = etor.Current.Value;
                    if (drpitm != null && drpitm.gameObject)
                    {
                        bool hasOwner = drpitm.itemdta.ownerId != 0,
                             notMine = drpitm.itemdta.ownerId != PlayerModel.getInstance().cid
                             || (TeamProxy.getInstance().MyTeamData != null && drpitm.itemdta.ownerId == TeamProxy.getInstance().MyTeamData.teamId);
                        if (hasOwner && notMine)
                            continue;
                        //!--距离超过拾取范围
                        if (Vector3.Distance(drpitm.transform.position.ConvertToGamePosition(), SelfRole._inst.m_curModel.transform.position.ConvertToGamePosition()) > StateInit.Instance.PickDistance)
                            continue;
                        //!--如果背包满且不是金币，不捡
                        if (a3_BagModel.getInstance().curi <= a3_BagModel.getInstance().getItems().Count && (
                            a3_BagModel.getInstance().getItemNumByTpid((uint)drpitm.itemdta.tpid) == 0 ||
                            a3_BagModel.getInstance().getItemNumByTpid((uint)drpitm.itemdta.tpid) >= a3_BagModel.getInstance().getItemDataById((uint)drpitm.itemdta.tpid).maxnum)
                            && drpitm.itemdta.tpid != 0)
                            continue;

                        if( A3_RollModel.getInstance().rollMapping.ContainsKey(drpitm.itemdta.dpid) && !A3_RollModel.getInstance().rollMapping[drpitm.itemdta.dpid].isCanPick) continue;

                        else if (drpitm.canPick && AutoPlayModel.getInstance().WillPick((uint)drpitm.itemdta.tpid))
                        {
                            pickTarget = drpitm;
                            if (nearest == null) {
                                nearest = pickTarget;
                            }
                            else
                            {
                                float dis = Vector3.Distance(pickTarget.transform.position.ConvertToGamePosition(), SelfRole._inst.m_curModel.transform.position.ConvertToGamePosition());
                                if (dis < min)
                                {
                                    min = dis;
                                    nearest = pickTarget;
                                }
                            }
                        }
                        else
                            pickTarget = null;
                    }
                }

                calc_ori = SelfRole._inst.m_curModel.transform.position;
                if (nearest != null)
                {
                    //if (A3_RollModel.getInstance().rollMapping.ContainsKey(nearest.itemdta.dpid))  // 该物品是roll点物品
                    //{
                    //    bool isCanPick = A3_RollModel.getInstance().rollMapping[nearest.itemdta.dpid].isCanPick;

                    //    if (!isCanPick)
                    //    {
                    //        nearest = null;
                    //        calc_tar = Vector3.zero;
                    //    }
                    //    else
                    //    {
                    //        SelfRole._inst.SetDestPos(nearest.transform.position);
                    //        calc_tar = nearest.transform.position;
                    //        return;
                    //    }
                    //}
                    //else {

                        SelfRole._inst.SetDestPos(nearest.transform.position);
                        calc_tar = nearest.transform.position;

                        return;
                    //}
                }
                else
                    calc_tar = Vector3.zero;
            }
            else
            {
                //if (nearest != null)
                //{
                if (nearest.gameObject != null && Vector3.Distance(SelfRole._inst.m_curModel.position.ConvertToGamePosition(), nearest.transform.position.ConvertToGamePosition()) < 0.5f)
                {
                    nearest.PickUpItem();
                    SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                    SelfRole.ChangeRideAniState(false);
                    nearest = null;
                }

                if (nearest != null)
                {
                    bool isOutOfTouch = Vector3.Distance(nearest.transform.position.ConvertToGamePosition(), SelfRole._inst.m_curModel.transform.position.ConvertToGamePosition()) > StateInit.Instance.PickDistance;
                    if (!passed && !isOutOfTouch)
                    {
                        SelfRole._inst.TurnToPos(nearest.transform.position);
                        SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, true);
                        SelfRole.ChangeRideAniState( true );
                    }
                    else
                    {
                        nearest = null;
                        SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                        SelfRole.ChangeRideAniState( false );
                    }
                }
                //}
            }


            calc_cur = SelfRole._inst.m_curModel.transform.position;            
            passed = CheckPass(calc_ori,calc_cur,calc_tar);
            if (nearest == null || nearest.gameObject == null)
            {               
                SelfRole._inst.m_moveAgent.Stop();
                SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
                SelfRole.ChangeRideAniState( false );
                if (SelfRole.fsm.Autofighting)
                    SelfRole.fsm.ChangeState(StateAttack.Instance);
                else
                    SelfRole.fsm.ChangeState(StateIdle.Instance);
                return;
            }            
        }

        public override void Exit()
        {
            SelfRole._inst.m_moveAgent.Stop();
            SelfRole._inst.m_curAni.SetBool(EnumAni.ANI_RUN, false);
            SelfRole.ChangeRideAniState( false );
            SelfRole._inst.m_moveAgent.avoidancePriority = priority;
            //dropTimer = 0.0f;
        }

        public void AutoEquipProcess(a3_BagItemData itmdata)
        {
            AutoPlayModel apmodel = AutoPlayModel.getInstance();

            //!--非自动挂机不做处理
            if (!SelfRole.fsm.Autofighting)
                return;

            //!--不处理装备,或vip等级3以下
            if (apmodel.EqpType == 0 || A3_VipModel.getInstance().Level < 3)
                return;

            int eqpproc = apmodel.EqpProc;
            int quality = itmdata.confdata.quality;
            int bitqual = (1 << (quality - 1));
            if ((eqpproc & bitqual) == 0)
                return;

            if (apmodel.EqpType == 1)
            {//!--自动出售
                BagProxy.getInstance().sendSellItems(itmdata.id, itmdata.num);


                flytxt.instance.fly(ContMgr.getCont("StatePick0") + Globle.getColorStrByQuality(itmdata.confdata.item_name, itmdata.confdata.quality));
            }
            else if (apmodel.EqpType == 2)
            {//!--自动分解

                List<uint> eqps = new List<uint>();
                eqps.Add(itmdata.id);
                EquipProxy.getInstance().sendsell(eqps);

                flytxt.instance.fly(ContMgr.getCont("StatePick1") + Globle.getColorStrByQuality(itmdata.confdata.item_name, itmdata.confdata.quality));

            }
        }

        /// <summary>
        /// 检测角色是否已经越过了要拾取物品的位置
        /// </summary>
        /// <param name="origin">锁定拾取物品时角色的位置</param>
        /// <param name="current">当前角色位置</param>
        /// <param name="target">被锁定的物品位置</param>
        /// <returns></returns>
        private bool CheckPass(Vector3 origin, Vector3 current, Vector3 target)
        {
            Vector3
            ori = origin.ConvertToGamePosition(),
            cur = current.ConvertToGamePosition(),
            tar = target.ConvertToGamePosition();

            if (tar == Vector3.zero) return true;

            float
            ori_x = ori.x,
            ori_y = ori.z,
            cur_x = cur.x,
            cur_y = cur.z,
            tar_x = tar.x,
            tar_y = tar.z;

            bool
            x_pass,
            y_pass;

            if (ori_x < tar_x)
                x_pass = cur_x > tar_x;
            else if (ori_x == tar_x)
                x_pass = cur_x != tar_x;
            else // ori_x < tar_x
                x_pass = cur_x < tar_x;

            if (ori_y > tar_y)
                y_pass = cur_y < tar_y;
            else if (ori_y == tar_y)
                y_pass = cur_y != tar_y;
            else // ori_y < tar_y
                y_pass = cur_y > tar_y;

            return x_pass || y_pass;
        }
    }
}
