using UnityEngine;
using MuGame;
using System;
using System.Collections.Generic;
using GameFramework;
using Cross;
public class RaffleHurtPoint : MonoBehaviour
{
    public GameObject m_Raffle;
    public GameObject m_RafWhole;
    public GameObject m_RafBoom;
    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == EnumLayer.LM_BT_SELF) //主角队发出的攻击
        {
            HitData hd = other.gameObject.GetComponent<HitData>();
            if (hd == null) return;

            //debug.Log("杂物被打中了");
            m_RafWhole.SetActive(false);
            m_RafBoom.SetActive(true);

            //临时处理只碰一次
            this.GetComponent<Collider>().enabled = false;
            other.enabled = false;

            //停止动画
            hd.HitAndStop();

            // BaseRoomItem.instance.showDropItem(m_Raffle.transform, ConfigUtil.getRandom(0, 2));

            GameObject.Destroy(m_Raffle, 3f);

        }
    }
}

public class BtWallHurtPoint : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == EnumLayer.LM_BT_SELF) //主角队发出的攻击
        {

            HitData hd = other.gameObject.GetComponent<HitData>();
            if (hd == null) return;

            if (hd.m_bOnlyHit) other.enabled = false;

            if (hd.m_unSkillID == 3006)
                hd.HitAndStop(-1, true);
            else
                hd.HitAndStop();
        }
    }
}


public static class SelfRole
{
    public static ProfessionRole _inst;
    public static bool UnderPlayerAttack { get; set; } = false;
    public static bool UnderTaskAutoMove { get; set; } = false;
    public static ProfessionRole LastAttackPlayer { get; set; } = null;

    //static public BaseRole s_LockRole; //锁定的目标
    public static Transform s_LockFX; //锁定的效果
    public static Transform s_LockFX_boss; //boss的锁定效果
    public static bool s_bStandaloneScene = false; //在单机副本中
    public static bool s_bInTransmit = false; //传送中
    //static private RoleStateHelper s_rshelper;
    static int lastNpcId;
    private static StateMachine _fsm = null;
    public static StateMachine fsm
    {
        get
        {
            if (_fsm == null)
            {
                //状态机
                _fsm = new StateMachine();
                _fsm.Configure(StateInit.Instance, StateGlobal.Instance, StateProxy.Instance);
            }
            return _fsm;
        }
    }

    public static void moveto(Vector3 pos, Action handle = null, bool forcefacetopos = false, float dis = 0.3f,bool forceStop = true,bool taskmove=false/*自动任务寻路*/)
    {
        if (_inst == null) return;
        if (!_inst.canMove) return;
        if(forceStop)
            fsm.Stop();
        StateAutoMoveToPos.Instance.handle = handle;
        StateAutoMoveToPos.Instance.pos = pos;
        StateAutoMoveToPos.Instance.forceFaceToPos = forcefacetopos;
        StateAutoMoveToPos.Instance.stopdistance = dis;
        fsm.ChangeState(StateAutoMoveToPos.Instance);
        debug.Log("我的寻路是自动任务寻路：" + taskmove);
        if(a3_liteMinimap.instance&&a3_liteMinimap.instance.ZidongTask&& !taskmove)
        {
            a3_liteMinimap.instance.ZidongTask = false;
        }
        if(taskmove&& a3_liteMinimap.instance && !a3_liteMinimap.instance.ZidongTask)
        {
            if (a3_liteMinimap.instance.ZidongTaskType == TaskType.MAIN)
                a3_liteMinimap.instance.TaskBtn(null,true);
        }

    }

    public static List<MapLinkInfo> line;
    public static void moveto(int id, Vector3 pos, Action handle = null, float stopDis = 0.3f,bool taskmove=false)
    {
        if (GRMap.instance == null)
        {
            return;
        }
        if (id == GRMap.instance.m_nCurMapID)
        {
            moveto(pos, handle, dis: stopDis,taskmove: taskmove);
            return;
        }


        line = new List<MapLinkInfo>();
        if (!worldmap.getMapLine(GRMap.instance.m_nCurMapID, id, line))
            return;
        fsm.StopDistance = stopDis;
        StateMoveLine.Instance.handle = handle;
        StateMoveLine.Instance.line = line;
        StateMoveLine.Instance.pos = pos;
        fsm.ChangeState(StateMoveLine.Instance);
    }

    public static void  setMaxhp( int hp ) {
        if (_inst != null) {
            _inst.maxHp = hp;
        }

    }

    public static void moveToNPc(int mapid, int npcId, List<string> listTalk = null, Action handle = null,bool taskmove=false)
    {        
        bool onlyOne = true;
        Vector3 npcPos = Vector3.zero;
        NpcRole targetNpc = NpcMgr.instance.getRole(npcId);
        if (targetNpc != null)
        {
            if (fsm.Autofighting)
                fsm.Stop();
            Vector3 targetNpcPos = targetNpc.transform.position;
            float curDis = Vector3.Distance(targetNpcPos, _inst.m_curModel.transform.position);
            if (curDis > 2 || lastNpcId != npcId || lastNpcId == 0)
                npcPos = targetNpcPos;
            lastNpcId = npcId;
        }
        moveto(mapid, npcPos, () =>
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_BOSSRANKING);
            DoAfterMgr.instacne.addAfterRender(() =>
            {

                if (onlyOne)//修改自动任务寻找npc对话垮地图时会出现对话框提前弹出的bug
                    {
                    onlyOne = false;
                    NpcRole n = NpcMgr.instance.getRole(npcId);
                    if (n == null)
                        return;
                    if (Vector3.Distance(n.transform.position, SelfRole._inst.m_curModel.transform.position) > 2)
                    {
                        SelfRole.moveto(n.transform.position, () =>
                        {
                            if (InterfaceMgr.getInstance().AnyWinOpen())
                                InterfaceMgr.getInstance().afterClose = () =>
                                {
                                    if (listTalk != null)
                                        n.newDesc = listTalk;
                                    n.handle = handle;
                                    n.onClick();
                                };
                            else
                            {
                                if (listTalk != null)
                                    n.newDesc = listTalk;
                                n.handle = handle;
                                n.onClick();
                            }
                        }, false, 1.5f,taskmove: taskmove);
                    }
                    else
                    {
                        if (InterfaceMgr.getInstance().AnyWinOpen())
                            InterfaceMgr.getInstance().afterClose = () =>
                            {
                                if (listTalk != null)
                                    n.newDesc = listTalk;
                                n.handle = handle;
                                n.onClick();
                            };
                        else
                        {
                            if (listTalk != null)
                                n.newDesc = listTalk;
                            n.handle = handle;
                            n.onClick();
                        }
                    }
                }
            });
        }, stopDis: 2f,taskmove: taskmove);
    }


    static public void FrameMove(float delta_time)
    {
        //切地图时加个保护
        if (GRMap.instance != null && GRMap.grmap_loading == true)
            return;

        //需要不停的发位置变化的消息给服务器
        if (_inst != null)
        {
            bool haveTarget = false;
            try { haveTarget = _inst.m_LockRole?.m_curModel ?? false; } catch (Exception e) { _inst.m_LockRole = null; haveTarget = false; }
            if (haveTarget)
            {
                if (s_LockFX != null && _inst.m_LockRole.m_circle_type == -1)
                {//通用类型光圈
                    PkmodelAdmin.RefreshShow(_inst.m_LockRole);                    
                    s_LockFX.position = _inst.m_LockRole.m_curModel.position;//m_curPhy                    
                    if (_inst.m_LockRole.isDead)
                    {
                        s_LockFX.position = Vector3.zero;
                    }
                }
                if (s_LockFX_boss != null && _inst.m_LockRole.m_circle_type == 1)
                {//boss类型光圈
                    s_LockFX_boss.position = _inst.m_LockRole.m_curPhy.position;
                    if (_inst.m_LockRole.isDead)
                    {
                        s_LockFX_boss.position = Vector3.zero;
                    }
                    float scale_value = _inst.m_LockRole.m_circle_scale;
                    s_LockFX_boss.localScale = new Vector3(scale_value, scale_value, scale_value);
                }
            }
            else
            {
                if (s_LockFX != null)
                    s_LockFX.position = Vector3.zero;
                if (s_LockFX_boss != null)
                    s_LockFX_boss.position = Vector3.zero;
            }

            _inst.FrameMove(delta_time);
            MoveProxy.getInstance().TrySyncPos(delta_time);
            try
            {

                long tm = muNetCleint.instance.g_netM.CurServerTimeStampMS;
                Vector3 pos = _inst.m_curModel.position;


                //Vector3 vec3 = new Vector3(x, pGameobject.transform.position.y, y);
                //LGCamera.instance.updateMainPlayerPos();
                //  MoveProxy.getInstance().sendstop((uint)x, (uint)y, 1, tm, false);

                if (_inst.moving)
                {
                    float angle = Mathf.Atan2(a1_gamejoy.inst_joystick.MovePosiNorm.z, a1_gamejoy.inst_joystick.MovePosiNorm.x) * Mathf.Rad2Deg;
                    angle = SceneCamera.m_curCamGo.transform.eulerAngles.y - angle;
                    pos = Quaternion.Euler(0, angle, 0) * new Vector3(1f, 0f, 0f) + pos;

                    NavMeshHit hit;
                    NavMesh.SamplePosition(pos, out hit, 3f, NavmeshUtils.allARE);
                    pos = hit.position;
                }

                float x = (float)(pos.x) * GameConstant.PIXEL_TRANS_UNITYPOS;
                float y = (float)(pos.z) * GameConstant.PIXEL_TRANS_UNITYPOS;
                MoveProxy.getInstance().sendstop((uint)x, (uint)y, 1, tm, false);
            }
            catch (System.Exception ex)
            {
                debug.Log(ex.ToString());
            }

            fsm.Update(delta_time);

            if (_inst.m_LockRole != null)
            {//添加怪物离开锁定范围的判定
                try
                {
                    Vector3 off_pos = _inst.m_LockRole.m_curPhy.position - _inst.m_curModel.position;
                    float off_dis = off_pos.magnitude;
                    if (off_dis > _inst.m_LockDis)
                    {
                        _inst.m_LockRole = null;
                    }
                }
                catch (Exception) { _inst.m_LockRole = null; }
            }
        }
    }

    static public void Init() //玩家使用的职业的初始化
    {
        //锁定的脚底光圈
        GameObject obj_prefab = GAMEAPI.ABFight_LoadPrefab("common_lock_fx");
        GameObject obj_inst = GameObject.Instantiate(obj_prefab) as GameObject;
        s_LockFX = obj_inst.transform;

        GameObject obj_boss_prefab = GAMEAPI.ABFight_LoadPrefab("common_lock_fx_boss");
        GameObject obj_boss_inst = GameObject.Instantiate(obj_boss_prefab) as GameObject;
        s_LockFX_boss = obj_boss_inst.transform;


        int p = PlayerModel.getInstance().profession;
        if (p == 2)
            FightText.userText = FightText.WARRIOR_TEXT;
        else if (p == 3)
            FightText.userText = FightText.MAGE_TEXT;
        else if (p == 5)
            FightText.userText = FightText.ASSI_TEXT;


        //P1Warrior warrior = new P1Warrior();
        //warrior.Init();
        //_inst = warrior;

        //debug.Log("进入游戏的角色是 ................ " + PlayerModel.getInstance().profession+" "+PlayerModel.getInstance().iid);


        GameObject mainborn = GameObject.Find("mainbronpt");
        if (mainborn != null)
        {
            PlayerModel.getInstance().mapBeginX = mainborn.transform.position.x;
            PlayerModel.getInstance().mapBeginY = mainborn.transform.position.z;
            PlayerModel.getInstance().mapBeginroatate = mainborn.transform.eulerAngles.y;
        }

        ProfessionRole me = null;
        A3_PROFESSION a3_pf = (A3_PROFESSION)PlayerModel.getInstance().profession;
        if (A3_PROFESSION.Warrior == a3_pf)
        {
            P2Warrior p2 = new P2Warrior();
            p2.m_bUserSelf = true;
            p2.Init(PlayerModel.getInstance().name, EnumLayer.LM_SELFROLE, Vector3.zero, true);
            me = p2;
        }
        else if (A3_PROFESSION.Mage == a3_pf)
        {
            P3Mage p3 = new P3Mage();
            p3.m_bUserSelf = true;
            p3.Init(PlayerModel.getInstance().name, EnumLayer.LM_SELFROLE, Vector3.zero, true);
            me = p3;
        }
        else if (A3_PROFESSION.Assassin == a3_pf)
        {
            P5Assassin p5 = new P5Assassin();
            p5.m_bUserSelf = true;
            p5.Init(PlayerModel.getInstance().name, EnumLayer.LM_SELFROLE, Vector3.zero, true);
            me = p5;
        }
        else
        {
            P3Mage p3 = new P3Mage();
            p3.m_bUserSelf = true;
            p3.Init(PlayerModel.getInstance().name, EnumLayer.LM_SELFROLE, Vector3.zero, true);
            me = p3;
        }

        //P3Mage me = new P3Mage();
        me.m_ePK_Type = PlayerModel.getInstance().up_lvl < 1 ? PK_TYPE.PK_PEACE : PK_TYPE.PK_PKALL;
        me.m_unIID = PlayerModel.getInstance().iid;
        me.m_unPK_Param = PlayerModel.getInstance().cid;
        me.refreshmapCount((int)PlayerModel.getInstance().treasure_num);
        me.refreshserialCount(PlayerModel.getInstance().serial);
        me.serial = PlayerModel.getInstance().serial;
        me.refreshVipLvl((uint)A3_VipModel.getInstance().Level);
        me.m_curPhy.gameObject.layer = EnumLayer.LM_BT_SELF;
        _inst = me;
        _inst.speed = PlayerModel.getInstance().speed;

        var rideInfo = A3_RideModel.getInstance().GetRideInfo();

        if ( GRMap.curSvrConf.ContainsKey( "maptype" ) && GRMap.curSvrConf[ "maptype" ]._int > 0 )
        {
            if ( rideInfo != null && rideInfo.mount == (uint)RIDESTATE.UP)
            {
                A3_RideModel.getInstance().GetRideInfo().mount = 0;
                A3_RideProxy.getInstance().SendC2S(4, "mount", 0, true);
            }
           
            //副本中不携带坐骑
        }
        else {

            if ( rideInfo != null &&  rideInfo.mount == ( uint ) RIDESTATE.UP ) _inst.set_Ride( ( int ) rideInfo.dress , true  );

        }


        //Transform one = U3DAPI.DEF_TRANSFORM;
        //one.position = me.m_curModel.position;
        //OtherPlayerMgr._inst.AddOtherPlayer(one, 1);
        //OtherPlayerMgr._inst.AddOtherPlayer(one, 2);
        //OtherPlayerMgr._inst.AddOtherPlayer(one, 1);
        //OtherPlayerMgr._inst.AddOtherPlayer(one, 1);
        //OtherPlayerMgr._inst.AddOtherPlayer(one, 1);
        //OtherPlayerMgr._inst.AddOtherPlayer(one, 1);


        //临时存放在这里
        //GameObject raffle_group = GameObject.Find("a3_scene/raffle");
        GameObject raffle_group = GameObject.Find("game_scene/lv3/raffle");
        if (raffle_group != null)
        {
            //debug.Log("找到了木桶组");
            for (int i = 0; i < raffle_group.transform.childCount; i++)
            {
                Transform cur_tf = raffle_group.transform.GetChild(i);

                Transform cur_whole = cur_tf.Find("whole");
                Transform cur_boom = cur_tf.Find("boom");

                if (cur_whole != null && cur_boom != null)
                {
                    RaffleHurtPoint rht = cur_tf.gameObject.AddComponent<RaffleHurtPoint>();
                    rht.m_Raffle = cur_tf.gameObject;
                    rht.m_RafWhole = cur_whole.gameObject;
                    rht.m_RafBoom = cur_boom.gameObject;

                    cur_tf.gameObject.layer = EnumLayer.LM_BT_FIGHT;
                }
                else
                {
                    GameObject.Destroy(cur_tf.gameObject);
                }
            }
        }

        //子弹墙
        //GameObject btwall_group = GameObject.Find("a3_scene/btwall");
        GameObject btwall_group = GameObject.Find("game_scene/lv3/btwall");
        if (btwall_group != null)
        {
            for (int i = 0; i < btwall_group.transform.childCount; i++)
            {
                Transform cur_tf = btwall_group.transform.GetChild(i);
                cur_tf.gameObject.AddComponent<BtWallHurtPoint>();

                cur_tf.gameObject.layer = EnumLayer.LM_BT_FIGHT;
            }
        }

        ////初始化装备模型
        //foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
        //{
        //    a3_EquipModel.getInstance().equipModel_on(equip);
        //}
        //初始化装备模型
        if (a3_EquipModel.getInstance().getEquips()!=null&& a3_EquipModel.getInstance().getEquips().Count>0)
        {
            foreach (a3_BagItemData equip in a3_EquipModel.getInstance().getEquips().Values)
            {
                a3_EquipModel.getInstance().equipModel_on(equip);
            }
        }
        else
            a3_EquipModel.getInstance().equipModel_on(null);     


        //初始化翅膀模型
        A3_WingModel.getInstance().OnEquipModelChange();

        //初始化宠物模型和注册宠物avatar改变
        if (PlayerModel.getInstance().last_time != 0)
        {
            OnPetAvatarChange();
        }

        //A3_PetModel.getInstance().OnStageChange += OnPetAvatarChange;
        MonsterMgr._inst.dispatchEvent(GameEvent.Create(MonsterMgr.EVENT_ROLE_BORN, MonsterMgr._inst, SelfRole._inst));


        //用于设置第一次创建模型时不发移动协议，解决刚进副本激活怪物ai的bug
        MoveProxy.getInstance().resetFirstMove();

        _inst.m_LockDis = XMLMgr.instance.GetSXML("comm.lockdis").getFloat("dis");
    }

    static private void OnPetAvatarChange()
    {

        A3_PetModel petmodel = A3_PetModel.getInstance();
        if (!PlayerModel.getInstance().havePet)
            return;
        _inst.ChangePetAvatar((int)petmodel.Tpid, 0);
        debug.Log(petmodel.Tpid + "::::tpid:::::::::::::::::");
    }
    static public void OnPetHaveChange(Variant data)
    {
        A3_PetModel petmodel = A3_PetModel.getInstance();
        if (!data["hava_pet"] || PlayerModel.getInstance().last_time == 0)
            return;
        PlayerModel.getInstance().havePet = data["hava_pet"];
        petmodel.Stage = 0;
        _inst.ChangePetAvatar(data["pet"]["id"], 0);
    }
    static public void showPet(Variant data)
    {

        if (data["iid"] != PlayerModel.getInstance().iid)
        {
            if (data["pet_food_last_time"] == 0)
            {
                OtherPlayerMgr._inst.PlayPetAvatarChange(data["iid"], 0, 0);
                debug.Log("饲料到期");
            }
            else
                OtherPlayerMgr._inst.PlayPetAvatarChange(data["iid"], data["pet_id"], 0);
        }
        else
        {
            PlayerModel.getInstance().last_time = data["pet_food_last_time"];//刷新playermodel里面的数据，宠物到期；
            _inst.ChangePetAvatar(0, 0);
            flytxt.instance.fly(ContMgr.getCont("pet_no_feed"));
            debug.Log("饲料到期");
        }
    }

    static public void Transmit(int toid, Action after = null, bool transmit = false, bool ontask = false, int lineid = -1,bool taskTrans=false/*任务传送*/) //传送
    {
        if (!MapModel.getInstance().dicMappoint.ContainsKey(GRMap.instance.m_nCurMapID)) return;
        if (toid != MapModel.getInstance().dicMappoint[GRMap.instance.m_nCurMapID] || lineid != -1)
        {
            if (!FindBestoModel.getInstance().Canfly) //宝图活动战斗状态禁止传送
            {
                flytxt.instance.fly(FindBestoModel.getInstance().nofly_txt);
                return;
            }
            cd.updateHandle = (cd cdt) =>
            {
                int temp = (int)(cd.secCD - cd.lastCD) / 100;
                cdt.txt.text = ContMgr.getCont("worldmap_cd", ((float)temp / 10f).ToString());
            };
            GameObject goeff = GameObject.Instantiate(worldmap.EFFECT_CHUANSONG1) as GameObject;
            goeff.transform.SetParent(SelfRole._inst.m_curModel, false);
            s_bInTransmit = true;
            cd.show(() =>
            {
                MapProxy.getInstance().sendBeginChangeMap(toid, true, false, lineid);
                DoAfterMgr.instacne.addAfterRender(() =>
                {
                    after?.Invoke();
                });
                s_bInTransmit = false;
            }, 3f, false,
            () =>
            {
                GameObject.Destroy(goeff);
                s_bInTransmit = false;
            }
            );
        }
        else
            after?.Invoke();
        debug.Log("我的传送是自动任务传送：" + taskTrans);
        if (a3_liteMinimap.instance && a3_liteMinimap.instance.ZidongTask && !taskTrans)
        {
            a3_liteMinimap.instance.ZidongTask = false;
        }
        if (a3_liteMinimap.instance && !a3_liteMinimap.instance.ZidongTask && taskTrans)
        {
            if(a3_liteMinimap.instance.ZidongTaskType == TaskType.MAIN)
                  a3_liteMinimap.instance.TaskBtn(null,true);
        }
    }

    public static void WalkToMap(int id, Vector3 vec, Action handle = null, float stopDis = 0.3f,bool taskmove=false)
    {
        if (fsm.Autofighting)
            fsm.Stop();
        if (id != GRMap.instance.m_nCurMapID)
        {
            moveto(id, vec, handle, stopDis);
            return;
        }
        NavMeshHit hit;
        NavMesh.SamplePosition(vec, out hit, 20f, NavmeshUtils.allARE);
        moveto(hit.position, handle, dis: stopDis,taskmove:taskmove);
    }

    //public static void TaskFinishMoveToNpc(int npcId)
    //{
    //    SXML npcsXml = XMLMgr.instance.GetSXML("npcs.npc", "id==" + npcId);
    //    if (npcsXml != null)
    //    {
    //        int mapId = npcsXml.getInt("map_id");
    //        SelfRole.moveToNPc(mapId, npcId);
    //    }
    //}

    public static void ChangeRideAniState( bool isRun ) {

        if (SelfRole._inst == null)
        {
            return;
        }

        var zuoji_Ani = SelfRole._inst.zuoji_Ani;

        if ( zuoji_Ani != null )
        {
            if (a1_gamejoy.inst_joystick && a1_gamejoy.inst_joystick.moveing)
            {
                zuoji_Ani.SetBool(EnumAni.ANI_T_RIDERUN, true);

                return;

            }  //自动战斗中 拖动摇杆 状态机切换的问题 a1_gamejoy.inst_joystick.moveing中时 动画状态可能会被切换为flase 

            zuoji_Ani.SetBool(EnumAni.ANI_T_RIDERUN, isRun);

        }
        else {

        }

    } // zmh

}


