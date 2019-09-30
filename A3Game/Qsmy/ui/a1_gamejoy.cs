using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using MuGame.Qsmy.model;

namespace MuGame
{
    class a1_gamejoy : GameJoyUI
    {

        //自动战斗技能
        AutoPlayModel apModel;

        static public a1_gamejoy inst_joystick;

        RectTransform cv_canvasRT;
        Animator m_joystick_Ani;

        private Vector3 Origin;
        private Vector3 touchOrigin;
        private Vector3 _deltaPos;
        private bool _drag = false;
        float dis;
        private float MoveMaxDistance = 80;            //最大拖动距离  
        public Vector3 MovePosiNorm;  //标准化移动的距离  
        private float ActiveMoveDistance = 20;              //激活移动的最低距离  

        float true_delta_x = 0.0f;
        float true_delta_y = 0.0f;

        public bool moveing = false;
        GameObject stick;
        public GameObject Stick => stick;
        Transform stickTrans;
        Transform stickBgTrans;

        Image stick_ig;
        Image stick_igbg;


        private List<SKillCdBt> vecSkillBt;

        static public a1_gamejoy inst_skillbar;

        public static int NORNAL_SKILL_ID = 0;
        public static long lastNormalSkillTimer = 0;
        public int skillsetIdx = 1;

        public BaseButton m_skillbar_hp_Add_btn;
        //private Text m_skillbar_hpNum_text;
        private const int SKILLBAR_HPBTN_NUM = 0;

        private Image m_skillbar_cd_image;
        Transform m_skillbar_tfCombat;
        //BaseButton m_skillbar_btnBuff;
        Transform m_skillbar_transChangeLock;
        //Text m_skillbar_txtBuff;
        BaseButton m_skillbar_btLeaveNewbie;
        BaseButton m_skillbar_btChange;
        BaseButton btn_ride;

        Animator m_skillbar_ani;

        Animator m_skillbar_on_5008_1;
        //Animator m_skillbar_on_5008_2;
        //GameObject m_skillbar_on_5008_3;

        public PicText m_pictext_GJ = new PicText();

        public static bool skill_wait = false;
        public override void init()
        {
            inst_joystick = this;
            alain();
            stick = this.getGameObjectByPath("joystick/stick");
            stickTrans = stick.transform;
            stickBgTrans = this.getGameObjectByPath("joystick/Image").transform;
            stick_ig = stickTrans.GetComponent<Image>();
            stick_igbg = transform.FindChild("joystick/Image").GetComponent<Image>();
            Origin = stickTrans.localPosition;//设置原点  
            touchOrigin = Origin;
            if (cemaraRectTran == null)
                cemaraRectTran = GameObject.Find("canvas_main").GetComponent<RectTransform>();

            cv_canvasRT = cemaraRectTran;

            m_joystick_Ani = transform.FindChild("joystick").GetComponent<Animator>();

            EventTriggerListener.Get(stick).onDrag = OnDrag;
            EventTriggerListener.Get(stick).onUp = (v) => { moveing = false; };
            EventTriggerListener.Get(stick).onDragOut = OnDragOut;
            if (EventTriggerListener.Get(stick).onDown != null)
                EventTriggerListener.Get(stick).onDown += OnMoveStart;
            else
                EventTriggerListener.Get(stick).onDown = OnMoveStart;

            MoveProxy.getInstance();
            BattleProxy.getInstance();

            EventTriggerListener.Get(stick).onDown += (GameObject go) =>
            {
                SelfRole.fsm.manBeginPos = SelfRole._inst.m_curModel.transform.position;
            };



            //skillbar
            //制作图片文字
            m_pictext_GJ.Init(getGameObjectByPath("num_pool"), 5, 15, 12f); //有5个位置显示数字，数字最大都是999--》5*3=15

            //alain();
            inst_skillbar = this;
            vecSkillBt = new List<SKillCdBt>();
            //m_skillbar_btnBuff = new BaseButton(getTransformByPath("skillbar/btnBuff"));
            //m_skillbar_txtBuff = getTransformByPath("skillbar/btnBuff/Text").GetComponent<Text>();
            //m_skillbar_btnBuff.onClick = onBtnBuffClick;
            m_skillbar_tfCombat = getTransformByPath("skillbar/combat");
            m_skillbar_btChange = new BaseButton(getTransformByPath("skillbar/combat/btn_change"));
            m_skillbar_btChange.onClick = onChange;

            m_skillbar_btLeaveNewbie = new BaseButton(getTransformByPath("skillbar/leavenewbie"));
            m_skillbar_btLeaveNewbie.onClick = onLeaveNewbie;

            for (int i = 0; i < 5; i++)
            {
                vecSkillBt.Add(new SKillCdBt(getTransformByPath("skillbar/combat/bt" + i), i));
            }
            refreshAllSkills(SelfRole.s_bStandaloneScene ? 0 : -1);

            getComponentByPath<Button>("skillbar/combat/apbtn").onClick.AddListener(OnSwitchAutoPlay);
            SelfRole.fsm.OnFSMStartStop += OnFSMStartStop;

            m_skillbar_hp_Add_btn = new BaseButton(this.getTransformByPath("skillbar/combat/hpbtn"));
            m_skillbar_hp_Add_btn.onClick = Add_Hp;

            m_pictext_GJ.SetNodeID(SKILLBAR_HPBTN_NUM, getTransformByPath("skillbar/combat/hpbtn/num"));
            //m_skillbar_hpNum_text = this.getComponentByPath<Text>("skillbar/combat/hpbtn/num");
            updata_hpNum();

            m_skillbar_cd_image = this.getComponentByPath<Image>("skillbar/combat/hpbtn/icon/cd");
            m_skillbar_on_5008_1 = this.transform.FindChild("skillbar/combat").GetComponent<Animator>();
            // on_5008_2 = this.transform.FindChild("ani_dsp").GetComponent<Animator>();
            //on_5008_3 = this.transform.FindChild("ani_hold").gameObject;

            m_skillbar_ani = transform.FindChild ("skillbar").GetComponent<Animator>();
            //this.transform.FindChild("ani_dsp").SetParent(transform.FindChild("combat"), false);
            //this.transform.FindChild("ani_hold").SetParent(transform.FindChild("combat"), false);

            m_skillbar_transChangeLock = m_skillbar_tfCombat.FindChild("bt_changeLock");
            new BaseButton(m_skillbar_transChangeLock).onClick = OnChangeLock;

            transform.FindChild("skillbar/combat/mark1").gameObject.SetActive(true);
            transform.FindChild("skillbar/combat/mark2").gameObject.SetActive(false);
            playselfSkills();
            if (GRMap.curSvrConf != null && (GRMap.curSvrConf["id"] == 10 ||GRMap.curSvrConf["id"] == 24))
                ShowCombatUI(false);
            else
                ShowCombatUI(true);


            if (GRMap.instance.m_nCurMapID == 3333 || SceneCamera.m_isFirstLogin)
            {
                getGameObjectByPath("skillbar/combat/apbtn")?.SetActive(false);
                getGameObjectByPath("skillbar/combat/bt_changeLock")?.SetActive(false);
            }

            //else
            //getGameObjectByPath("combat/apbtn")?.SetActive(true);


            btn_ride = new BaseButton( transform.FindChild( "joystick/btn_ride/IsDown" ) );

            btn_ride.onClick = ( go ) =>
            {
                var rideInfo = A3_RideModel.getInstance().GetRideInfo();

                if ( rideInfo == null  || rideInfo.ridedressMapiping.Count <= 0 )
                {
                    flytxt.instance.fly( ContMgr.getCont( "a3_not_haveride" ) );

                    return;
                }

                A3_RideProxy.getInstance().SendC2S( 4 , "mount" , ( uint ) ( ( int ) A3_RideModel.getInstance().GetRideInfo().mount == 0 ? 1 : 0 ) );
            };

            CheckBtnRideIsOpen();
        }

        public override void onShowed()
        {
            if (MapProxy.isyinsh)
                forSkill_5008(MapProxy.isyinsh);
        }

        public void CheckBtnRideIsOpen() {
            btn_ride.gameObject.SetActive( FunctionOpenMgr.instance.Check( FunctionOpenMgr.MOUNT ) );
        }

        public void OpenRide() {
            btn_ride.gameObject.SetActive(true);
        }
        public void onoffAni(bool onoff)
        {
            m_joystick_Ani.SetBool("onoff", onoff);
        }


        public int curTick = 0;
        public static bool Cd_lose = true;
        public static bool canClick = false;
        void skillbar_Upddate()
        {
            if (SelfRole._inst == null || muNetCleint.instance == null)
                return;
            updata_hpNum();
            if (a3_BagModel.getInstance().getItemCds().Count > 0 && a3_BagModel.getInstance() != null)
            {
                foreach (int type in a3_BagModel.getInstance().getItemCds().Keys)
                {
                    if (type == 4)
                    {
                        if (a3_BagModel.getInstance().getItemCds()[type] <= 0)
                        {
                            m_skillbar_cd_image.fillAmount = 0;
                        }
                        else
                        {
                            m_skillbar_cd_image.fillAmount = a3_BagModel.getInstance().getItemCds()[type] / a3_BagModel.getInstance().getItemDataById(1533).cd_time;
                        }
                    }
                }
            }
            //处理鼠标选中攻击目标
            bool inWin = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer;

            bool mouseTouch = Input.GetMouseButtonDown(0);
            bool click_one = false;
            Vector3 click_pos = Vector3.zero;
            if (mouseTouch || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                if ((inWin && EventSystem.current.IsPointerOverGameObject())) return;
                if (!inWin && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;

                if (mouseTouch)
                {
                    click_one = true;
                    click_pos = Input.mousePosition;
                }
                else
                {
                    //for (int i = 0; i < Input.touchCount; i++)
                    //{
                    //    Touch t = Input.GetTouch(i);
                    //    FightText.play(FightText.MOUSE_POINT, t.position, 0);
                    //}
                }
            }

            if (click_one)
            {
                Ray cam_ray = SceneCamera.m_curCamera.ScreenPointToRay(click_pos);
                RaycastHit hit;

                if (Physics.Raycast(cam_ray, out hit, 65535f, (1 << EnumLayer.LM_BT_FIGHT) + (1 << EnumLayer.LM_NPC)))
                {
                    //debug.Log("射线命中" + hit.collider.name);

                    //需要遍历找一下是谁的
                    SelfRole._inst.m_LockRole = OtherPlayerMgr._inst.FindWhoPhy(hit.collider.transform);

                    if (SelfRole._inst.m_LockRole == null)
                    {
                        SelfRole._inst.m_LockRole = MonsterMgr._inst.FindWhoPhy(hit.collider.transform, true);
                        if (SelfRole._inst.m_LockRole != null)
                        {
                            MonsterRole monster = SelfRole._inst.m_LockRole as MonsterRole;
                            monster.onClick();
                        }
                    }
                    if (SelfRole._inst.m_LockRole == null)
                    {
                        if (!canClick)
                        {
                            canClick = true;
                            NpcRole npc = hit.collider.transform.GetComponent<NpcRole>();
                            if (npc != null)
                            {
                                if (Vector3.Distance(npc.transform.position, SelfRole._inst.m_curModel.transform.position) > 2)
                                {
                                    SelfRole.moveto(npc.transform.position, () =>
                                    {
                                        npc.onClick();
                                    }, true, 2f);
                                }
                                else
                                {
                                    npc.onClick();
                                }
                            }
                        }
                    }


                }
            }
            long timestp = muNetCleint.instance.CurServerTimeStampMS;
            vecSkillBt[0].update(timestp); vecSkillBt[1].update(timestp); vecSkillBt[2].update(timestp); vecSkillBt[3].update(timestp); vecSkillBt[4].update(timestp);

            int skillid = SelfRole._inst.m_curSkillId;

            if (vecSkillBt[0].m_nCurDownID != 0 && skillid == NORNAL_SKILL_ID)
            {
                SelfRole._inst.m_fAttackCount = 0.5f;

                if (lastNormalSkillTimer == 0)
                    lastNormalSkillTimer = timestp + 500;
                else if (lastNormalSkillTimer < timestp)
                {
                    vecSkillBt[0].keepSkill();
                    lastNormalSkillTimer = timestp + 500;
                }
            }

            //法师按压类技能，需要特殊处理
            //if (3007 == skillid && SelfRole._inst.m_fAttackCount < 0.1f)
            //{
            //    for (int i = 1; i < vecSkillBt.Count; i++)
            //    {
            //        if (vecSkillBt[i].m_nCurDownID == 3007)
            //        {
            //            vecSkillBt[i].keepSkill();
            //        }
            //    }
            //}
            //if (3002 == skillid && SelfRole._inst.m_fAttackCount < 0.1f)
            //{
            //    for (int i = 1; i < vecSkillBt.Count; i++)
            //    {
            //        if (vecSkillBt[i].m_nCurDownID == 3002)
            //        {
            //            vecSkillBt[i].keepSkill();
            //        }
            //    }
            //}

            if (3010 == skillid && Cd_lose)
            {
                long time = Skill_a3Model.getInstance().skilldic[skillid].xml.GetNode("skill_att", "skill_lv==" + Skill_a3Model.getInstance().skilldic[skillid].now_lv.ToString()).getInt("time"); ;
                if (Cd_lose == true)
                {
                    foreach (int it in Skill_a3Model.getInstance().skilldic.Keys)
                    {
                        if (Skill_a3Model.getInstance().skilldic[it].endCD > 0 && it != 3010)
                        {
                            Skill_a3Model.getInstance().skilldic[it].endCD -= time;
                        }
                    }
                    Cd_lose = false;
                }
            }
            if (Skill_a3Model.getInstance().skilldic.ContainsKey(3010) && Skill_a3Model.getInstance().skilldic[3010].endCD <= 0)
            {
                Cd_lose = true;
            }
        }


        public void Not_show()
        {
            this.transform.FindChild("skillbar/leavenewbie").gameObject.SetActive(false);
        }
        public void show_btnIcon(bool show)
        {
            transform.FindChild("skillbar/combat/hpbtn").gameObject.SetActive(show);
        }

        void Update()
        {
            skillbar_Upddate();

            if (MouseClickMgr.instance == null) return;

            //if (Input.touchCount >= 2)
            //{
            //    if (false == EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && false == EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
            //    {
            //        moveing = false;
            //    }
            //}

            if (false == Globle.A3_DEMO)
            {
                //if (muNetCleint.instance.curServerPing < 500)
                //{
                //    //normal.SetActive(true);
                //   // slow.SetActive(false);
                //   // ping.text = Globle.getColorStrByQuality("ping:" + muNetCleint.instance.curServerPing.ToString(), 2);
                //}
                //else
                //{
                //    //normal.SetActive(false);
                //   // slow.SetActive(true);
                //  //  ping.text = Globle.getColorStrByQuality("ping:" + muNetCleint.instance.curServerPing.ToString(), 6);
                //}
                if (MouseClickMgr.instance.m_bTowTouchZoom)
                {
                    moveing = false;
                }


                if (moveing)
                {
                    dis = Vector3.Distance(new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0), touchOrigin);//拖动距离，这不是最大的拖动距离，是根据触摸位置算出来的  
                    if (dis >= MoveMaxDistance)       //如果大于可拖动的最大距离  
                    {
                        Vector3 vec = touchOrigin + (new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0) - touchOrigin) * MoveMaxDistance / dis;  //求圆上的一点：(目标点-原点) * 半径/原点到目标点的距离  
                        stickTrans.localPosition = vec;
                    }

                    if (Vector3.Distance(stickTrans.localPosition, touchOrigin) > ActiveMoveDistance) //距离大于激活移动的距离  
                    {
                        MovePosiNorm = (stickTrans.localPosition - touchOrigin).normalized;
                        MovePosiNorm = new Vector3(MovePosiNorm.x, 0, MovePosiNorm.y);
                    }
                    else
                        MovePosiNorm = Vector3.zero;

                    Color color = stick_ig.color;
                    color.a = 0.5f + (dis / MoveMaxDistance) / 2;
                    stick_ig.color = color;
                    stick_igbg.color = color;

                    //float angle = Mathf.Atan2(MovePosiNorm.x, MovePosiNorm.z) * Mathf.Rad2Deg;
                    // float angle = Mathf.Atan2(MovePosiNorm.z, MovePosiNorm.x) * Mathf.Rad2Deg;

                    // UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_PLAY_MOV, this, angle-90));

                    //debug.Log("摇杆的角度" + angle);

                    //lgSelfPlayer.instance.onJoystickMove(angle - angleOffset);

                    //  lgSelfPlayer.instance.onJoystickMove(angle);
                }
                else
                {
                    Color color = stick_ig.color;
                    color.a = 0.5f;
                    stick_ig.color = color;
                    stick_igbg.color = color;
                }
            }
            else
            {
                //移动将重新处理
                if (moveing)
                {
                    dis = Vector3.Distance(new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0), touchOrigin);//拖动距离，这不是最大的拖动距离，是根据触摸位置算出来的  
                    if (dis >= MoveMaxDistance)       //如果大于可拖动的最大距离  
                    {
                        Vector3 vec = touchOrigin + (new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0) - touchOrigin) * MoveMaxDistance / dis;  //求圆上的一点：(目标点-原点) * 半径/原点到目标点的距离  
                        stickTrans.localPosition = vec;
                    }

                    if (Vector3.Distance(stickTrans.localPosition, touchOrigin) > ActiveMoveDistance) //距离大于激活移动的距离  
                    {
                        MovePosiNorm = (stickTrans.localPosition - touchOrigin).normalized;
                        MovePosiNorm = new Vector3(MovePosiNorm.x, 0, MovePosiNorm.y);
                    }
                    else
                        MovePosiNorm = Vector3.zero;

                    Color color = stick_ig.color;
                    color.a = 0.5f + (dis / MoveMaxDistance) / 2;
                    stick_ig.color = color;
                    stick_igbg.color = color;


                    //处理角色的移动(加入主角的移动)
                    if (MovePosiNorm != Vector3.zero)
                    {
                        if (!SelfRole._inst.isDead)
                            SelfRole._inst.StartMove(MovePosiNorm.x, MovePosiNorm.z);

                        //if (tempGo == null)
                        //{
                        //    tempGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //    tempGo.layer = EnumLayer.LM_FX;
                        //}

                        //float angle = Mathf.Atan2(MovePosiNorm.z, MovePosiNorm.x) * Mathf.Rad2Deg;
                        //angle = SceneCamera.m_curCamGo.transform.eulerAngles.y - angle;
                        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
                        //Vector3 newPos = rotation * new Vector3(2f, 0f, 0f);
                        //tempGo.transform.position = newPos + SelfRole._inst.m_curModel.position;
                    }
                }
                else
                {
                    Color color = stick_ig.color;
                    color.a = 0.5f;
                    stick_ig.color = color;
                    stick_igbg.color = color;
                }
            }
        }

        void MiouseDown()
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {

            }
            else
            {
                touchOrigin = Origin;
                stickTrans.localPosition = Origin;
            }
        }

        Vector3 joystk_result;

        private Vector3 _checkPosition(Vector3 movePos, Vector3 _offsetPos)
        {
            joystk_result = movePos + _offsetPos;
            return joystk_result;
        }

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (!moveing)
            {
                OnDragOut(null);
                return;
            }

            if (!_drag)
            {
                _drag = true;
            }
            _deltaPos = delta;

            true_delta_x = true_delta_x + _deltaPos.x;
            true_delta_y = true_delta_y + _deltaPos.y;

            stickTrans.localPosition += new Vector3(_deltaPos.x, _deltaPos.y, 0);

            if (SelfRole.fsm.Autofighting)
                SelfRole.fsm.Pause();
            else
            {
                //if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
                //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
                //if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
                SelfRole.fsm.Stop();
            }
        }

        [SerializeField]
        float MaxAllowedDistance = 3f;
        public void OnDragOut(GameObject go = null)
        {
            stop();

            if (SelfRole.fsm.Autofighting)
            {
                if (SelfRole.fsm.CheckJoystickMoveDis(SelfRole._inst.m_curModel.position, StateInit.Instance?.maxAllowedDistance ?? MaxAllowedDistance))
                {
                    //if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
                    //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
                    //if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
                    SelfRole.fsm.Stop();
                }
                else
                {
                    StateInit.Instance.Origin = SelfRole._inst.m_curModel.position;
                    SelfRole.fsm.Resume();
                }
            }
            else
            {
                //if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
                //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
                //if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
                SelfRole.fsm.Stop();
            }
        }

        public void OnDragOut_wait(float t = 0.2f)
        {
            CancelInvoke("timeGo");
            Invoke("timeGo", t);
        }
        void timeGo()
        {
            OnDragOut(null);
        }

        public void stop()
        {
            _drag = false;
            stickTrans.localPosition = Origin;
            stickBgTrans.localPosition = Origin;
            touchOrigin = Origin;
            //  if (PlayerMoveControl.moveEnd != null) PlayerMoveControl.moveEnd();
            //  UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_PLAY_STOP, this, null));

            if (false == Globle.A3_DEMO)
            {
                lgSelfPlayer.instance.onJoystickEnd();
            }
            else
            {
                SelfRole._inst.StopMove();
            }


            moveing = false;

            true_delta_x = 0.0f;
            true_delta_y = 0.0f;
        }

        void OnMoveStart(GameObject go)
        {
            moveing = true;
            //TaskModel.getInstance().isSubTask = false;
            worldmap.Desmapimg();//消除地图移动路径
            debug.Log("I'm moving");
            //在自动任务时手动摇杆自动不取消
            if (a3_liteMinimap.instance && a3_liteMinimap.instance.ZidongTask&& SelfRole.fsm.currentState!=StateAttack.Instance)
                a3_liteMinimap.instance.ZidongTask = false;
            a1_gamejoy.canClick = false;
            if (a3_dartproxy.getInstance().dartHave)
            {
                a3_dartproxy.getInstance().gotoDart = false;
                a3_liteMinimap.instance.getGameObjectByPath("goonDart").SetActive(true);
            }
            if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
            //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
            // if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);

            //新手副本指引移动
            UiEventCenter.getInstance().onStartMove();
        }
        void OnDragOut1(GameObject go)
        {
            stopDrag();
        }

        public void stopDrag()
        {
            _drag = false;
            stickTrans.localPosition = Origin;
            stickBgTrans.localPosition = Origin;
            touchOrigin = Origin;

            if (false == Globle.A3_DEMO)
            {
                lgSelfPlayer.instance.onJoystickEnd();
            }
            else
            {
                SelfRole._inst?.StopMove();
            }


            moveing = false;

            true_delta_x = 0.0f;
            true_delta_y = 0.0f;
        }


        void OnDrag1(GameObject go, Vector2 delta)
        {
            if (!moveing)
            {
                OnDragOut1(null);
                return;
            }

            if (!_drag)
            {
                _drag = true;
            }
            _deltaPos = delta;

            true_delta_x = true_delta_x + _deltaPos.x;
            true_delta_y = true_delta_y + _deltaPos.y;

            stickTrans.localPosition += new Vector3(_deltaPos.x, _deltaPos.y, 0);
        }
        void OnMoveStart1(GameObject go)
        {
            //debug.Log("设置摇杆点");
            Vector3 touch_pos = Input.mousePosition;
            if (Input.touchCount > 1)
            {
                touch_pos.x = 99999.9f;
                //找一个离摇杆最近的触点来处理位置，以x轴向做对比
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.GetTouch(i); //0,0的坐标为左下角
                    if (touch_pos.x > t.position.x)
                    {
                        touch_pos.x = t.position.x;
                        touch_pos.y = t.position.y;
                    }
                }
            }

            moveing = true;
            dis = Vector3.Distance(stickTrans.position, touch_pos);
            Vector3 pos = new Vector3(touch_pos.x / Screen.width * cv_canvasRT.rect.width - cv_canvasRT.rect.width / 2 - transform.localPosition.x,
                touch_pos.y / Screen.height * cv_canvasRT.rect.height - cv_canvasRT.rect.height / 2 - transform.localPosition.y);
            stickBgTrans.localPosition = pos;
            stickTrans.localPosition = pos;
            touchOrigin = pos;
            //Vector3 vec = Origin + (Input.mousePosition - stickTrans.position) * MoveMaxDistance / dis;  //求圆上的一点：(目标点-原点) * 半径/原点到目标点的距离  
            //stickTrans.localPosition = vec;

            SelfRole.fsm.Pause();
        }





        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////skill bar/////////////////////////////////////////////
        /// </summary>
        /// <param name="go"></param>
        //void onBtnBuffClick(GameObject go) => welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.onLineTimeAward);

        void onLeaveNewbie(GameObject go)
        {
            m_skillbar_btLeaveNewbie.interactable = false;
            PlayerModel.getInstance().LeaveStandalone_CreateChar();
        }

        public void onoff_skillbarAni(bool onoff) => m_skillbar_ani.SetBool("onoff", onoff);


        public int NowSkillIndex = 1;
        public void onChange(GameObject go)
        {
            //主线任务626之前，不开放2
            if (A3_TaskModel.getInstance().main_task_id <626)
            {
                flytxt.instance.fly(ContMgr.getCont("skillbar1"));
                return;
            }

            if (skillsetIdx == 1)
            {
                transform.FindChild("skillbar/combat/mark1").gameObject.SetActive(false);
                transform.FindChild("skillbar/combat/mark2").gameObject.SetActive(true);
                skillsetIdx = 2;
                playselfSkills(2);
                NowSkillIndex = 2;
            }
            else
            {
                transform.FindChild("skillbar/combat/mark1").gameObject.SetActive(true);
                transform.FindChild("skillbar/combat/mark2").gameObject.SetActive(false);
                skillsetIdx = 1;
                playselfSkills(1);
                NowSkillIndex = 1;
            }
            refreshAllSkills();
        }
        //自动战斗技能
        public void playselfSkills(int skillbar=1)
        {
            apModel = AutoPlayModel.getInstance();
            if (skillbar==1)
            {
                for (int i = 0; i < Skill_a3Model.getInstance().idsgroupone.Length; i++)
                {
                    if (Skill_a3Model.getInstance().idsgroupone[i] != 0)
                    {
                        if (/*Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgroupone[i]].skillType2 != 1 ||*/
                            Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgroupone[i]].skillType2 != 2)
                            apModel.Skills[i] = Skill_a3Model.getInstance().idsgroupone[i];
                        else
                            apModel.Skills[i] = 0;
                    }
                    else
                    {
                        apModel.Skills[i] = 0;
                    }

                }
            }
            else
            {
                for (int i = 0; i < Skill_a3Model.getInstance().idsgrouptwo.Length; i++)
                {
                    if (Skill_a3Model.getInstance().idsgrouptwo[i] != 0)
                    {
                        if (/*Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgrouptwo[i]].skillType2 != 1 ||*/
                            Skill_a3Model.getInstance().skilldic[Skill_a3Model.getInstance().idsgrouptwo[i]].skillType2 != 2)
                            apModel.Skills[i] = Skill_a3Model.getInstance().idsgrouptwo[i];
                        else
                            apModel.Skills[i] = 0;
                    }
                    else
                        apModel.Skills[i] = 0;

                }
            }
        }

        public void refreshAllSkills(int fakeType = -1)
        {
            NORNAL_SKILL_ID = PlayerModel.getInstance().profession * 1000 + 1;
            refreSkill(0, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID]);

            if (fakeType >= 0)
            {
                m_skillbar_btChange.transform.GetComponent<Button>().interactable = false;
                m_skillbar_btLeaveNewbie.gameObject.SetActive(true);

                for (int i = 1; i < 5; i++)
                {
                    refreSkill(i, null);
                }

                if (fakeType > 3)
                {
                    if (2 == PlayerModel.getInstance().profession)
                    {
                        refreSkill(4, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID + 5]);
                    }
                    else
                    {
                        refreSkill(4, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID + 4]);
                    }
                }
                if (fakeType > 2)
                {
                    refreSkill(3, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID + 3]);
                }
                if (fakeType > 1)
                {
                    refreSkill(2, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID + 2]);
                }
                if (fakeType > 0)
                {
                    if (3 == PlayerModel.getInstance().profession)
                    {
                        refreSkill(1, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID + 6]);
                    }
                    else
                    {
                        refreSkill(1, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID + 1]);
                    }
                }

            }
            else
            {
                m_skillbar_btChange.transform.GetComponent<Button>().interactable = true;
                m_skillbar_btLeaveNewbie.gameObject.SetActive(false);
                int[] ids = skillsetIdx == 1 ? Skill_a3Model.getInstance().idsgroupone : Skill_a3Model.getInstance().idsgrouptwo;
                int idx = 1;
                foreach (int id in ids)
                {
                    if (id > 0)
                        refreSkill(idx, Skill_a3Model.getInstance().skilldic[id]);
                    else
                        refreSkill(idx, null);
                    idx++;

                }
            }
            if (MapProxy.isyinsh)
            {
                forSkill_5008(MapProxy.isyinsh);
                SelfRole._inst.invisible = MapProxy.isyinsh;
            }
        }

        public void forSkill_5008(bool b)
        {
            // for (int i = 1; i <= 4; i++)
            //{
            //transform.FindChild("combat/bt" + i).gameObject.SetActive(!b);
            //}
            //if (b)
            //{
            //    SXML xml = XMLMgr.instance.GetSXML("skill.skill", "id==" + 5008);
            //    int skill_id = xml.GetNode("skill_att", "skill_lv==" + Skill_a3Model.getInstance().skilldic[5008].now_lv).GetNode("cast").getInt("skill_id");
            //    refreSkill(0, Skill_a3Model.getInstance().skilldic[skill_id]);
            //    on_5008_1.enabled = true;
            //    on_5008_1.SetBool("onoff", true);
            //    on_5008_2.gameObject.SetActive(true);
            //    on_5008_2.Play("ass_skill_hide2", -1, 0f);
            //    on_5008_3.SetActive(true);
            //}
            //else
            //{
            //    refreSkill(0, Skill_a3Model.getInstance().skilldic[NORNAL_SKILL_ID]);
            //    on_5008_3.SetActive(false);
            //    on_5008_1.SetBool("onoff", false);
            //}
        }
        public void refreSkill(int idx, skill_a3Data d) => vecSkillBt[idx].skl_data = d;

        public int getEmptyfreSkill()
        {
            int datid = -1;
            for (int i = 1; i < vecSkillBt.Count; i++)
            {
                if (vecSkillBt[i].skl_data == null && datid <= 0)
                {
                    datid = i;
                    break;
                }
            }
            return datid;
        }

        private void Add_Hp(GameObject go) => a3_BagModel.getInstance().useItemByTpid(1533, 1);

        public void updata_hpNum()
        {
            int count = a3_BagModel.getInstance().getItemNumByTpid(1533);
            if (count > 999)
            {
                m_pictext_GJ.SetNodeNum(SKILLBAR_HPBTN_NUM, 999);
            }
            else
            {
                m_pictext_GJ.SetNodeNum(SKILLBAR_HPBTN_NUM, count);
            }
        }
        public void refreSkill(int idx, int skillid)
        {
            vecSkillBt[idx].skl_data = Skill_a3Model.getInstance().skilldic[skillid];
            List<int> lst = new List<int>();
            for (int ii = 0; ii < 4; ii++)
            {
                lst.Add(Skill_a3Model.getInstance().idsgroupone[ii]);
            }
            lst.AddRange(Skill_a3Model.getInstance().idsgrouptwo);
            Skill_a3Proxy.getInstance().sendProxy(0, lst);
        }


        public void clearCD()
        {
            foreach (int it in Skill_a3Model.getInstance().skilldic.Keys)
            {
                if (Skill_a3Model.getInstance().skilldic[it].endCD > 0)
                {
                    Skill_a3Model.getInstance().skilldic[it].endCD = 0;
                }
            }
        }

        public bool playSkillById(int skillid, bool pkmode = false)
        {
            if (SelfRole._inst == null || muNetCleint.instance == null)
                return false;

            skill_a3Data d = Skill_a3Model.getInstance().skilldic[skillid];

            if (d.cdTime > 0)
                return false;

            if (SelfRole._inst.m_LockRole == null || SelfRole._inst.m_LockRole.isDead)
            {
                Vector3 vec = SelfRole._inst.m_curModel.position;
                if (pkmode)
                    SelfRole._inst.m_LockRole = OtherPlayerMgr._inst.FindNearestEnemyOne(vec)/*PkmodelAdmin.RefreshLockRole()*/;
                if (SelfRole._inst.m_LockRole == null)
                    SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestMonster(vec);

                if (SelfRole._inst.m_LockRole == null)
                    return false;
            }

            if (SelfRole._inst.m_fAttackCount > 0)
                return false;

            SelfRole._inst.LeaveHide();

            if (SelfRole._inst.m_curSkillId == skillid && skillid == NORNAL_SKILL_ID)
            {
                long timestp = muNetCleint.instance.CurServerTimeStampMS;
                if (lastNormalSkillTimer == 0)
                    lastNormalSkillTimer = timestp + 500;
                else if (lastNormalSkillTimer < timestp)
                {
                    vecSkillBt[0].keepSkill();
                    lastNormalSkillTimer = timestp + 500;
                }
            }
            else
            {
                SelfRole._inst.TurnToRole(SelfRole._inst.m_LockRole, true);
                SelfRole._inst.PlaySkill(skillid);
                if (!SelfRole._inst.m_LockRole.isfake)
                {
                    BattleProxy.getInstance().send_cast_self_skill(SelfRole._inst.m_LockRole.m_unIID, skillid);
                }
                else
                {

                    switch (skillid)
                    {
                        //case 3002:
                        case 5003:
                        case 2003:
                            MediaClient.instance.PlaySoundUrl("audio_skill_" + skillid, false, null);
                            break;
                    }
                }



                d.doCD();

            }
            if (d.skillType == 0)
                BattleProxy.getInstance().sendUseSelfSkill((uint)d.skill_id);

            return true;
        }

        public int getSkillCanUse()
        {
            for (int i = vecSkillBt.Count - 1; i > -1; i--)
            {
                SKillCdBt bt = vecSkillBt[i];
                if (bt.canUse())
                    return bt.skl_data.skill_id;
            }
            return -1;
        }

        static public LGAvatarGameInst getAttackTarget(int range = 1000)
        {
            LGAvatarGameInst temp = null;
            if (PlayerModel.getInstance().checkPK())
            {
                temp = LGOthers.instance.getNearPlayer(range);
            }

            if (temp == null)
                temp = LGMonsters.instacne.getNearMon(range);
            return temp;
        }

        //!--切换自动战斗

        private void OnSwitchAutoPlay()
        {
            //      PlayerModel.getInstance().  LeaveStandalone_CreateChar();

            if (SelfRole.s_bStandaloneScene)
                return;

            bool isOn = SelfRole.fsm.Autofighting;
            if (isOn)
            {
               // //自动任务刷怪玩家点击取消自动战斗再点击自动战斗会继续自动任务
               //if (a3_liteMinimap.instance && a3_liteMinimap.instance.ZidongTask)
               //{
               //     //1秒后变成自动
               //     CancelInvoke("OnSwitchAutoPlay");
               //     Invoke("OnSwitchAutoPlay", 1);
               //}
                SelfRole.fsm.Stop();
                flytxt.flyUseContId("autoplay_stop");
            

            }
            else
            {
             
               if (a3_liteMinimap.instance && a3_liteMinimap.instance.ZidongTask&& SelfRole.fsm.currentState == StateAutoMoveToPos.Instance)
                {
                    a3_liteMinimap.instance.ZidongTask = false;
                }
                SelfRole.fsm.StartAutofight();
                SelfRole.fsm.ClearAutoConfig();
                flytxt.flyUseContId("autoplay_start");

            }
        }

        private void OnFSMStartStop(bool running)
        {
            getGameObjectByPath("skillbar/combat/apbtn/on").SetActive(!running);
            getGameObjectByPath("skillbar/combat/apbtn/off").SetActive(running);
            getGameObjectByPath("skillbar/combat/apbtn/fire").SetActive(running);

            if (running)
            {
                ArrayList data = new System.Collections.ArrayList();
                Action act = () => InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_AUTOPLAY2);
                data.Add(act);
                data.Add(ContMgr.getCont("skillbar"));
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_DOING, data);
            }
            else
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_DOING);
            }
            A3_BeStronger.Instance.ContentShown.gameObject.SetActive(false);
            A3_BeStronger.Instance.ClickPanel.gameObject.SetActive(false);
        }
        public void ShowCombatUI(bool show)
        {
            //Vector3 v3 = show ? Vector3.one : Vector3.zero;
            //m_skillbar_tfCombat.localScale = v3;
            m_skillbar_tfCombat.gameObject.SetActive(show);
        }
        private void OnChangeLock(GameObject go)
        {
            Vector3 playerPos = SelfRole._inst.m_curModel.position;
            switch (PlayerModel.getInstance().pk_state)
            {
                case PK_TYPE.PK_PKALL:
                    SelfRole._inst.m_LockRole = OtherPlayerMgr._inst.FindNearestEnemyOne(playerPos, false, (otherPlayer) => otherPlayer.m_isMarked, true, PK_TYPE.PK_PKALL);
                    if (SelfRole._inst.m_LockRole == null)
                        SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestSummon(playerPos);
                    if (SelfRole._inst.m_LockRole == null)
                        goto case PK_TYPE.PK_PEACE;
                    break;
                case PK_TYPE.PK_TEAM:
                    if (TeamProxy.getInstance().MyTeamData == null)
                        goto case PK_TYPE.PK_PKALL;
                    SelfRole._inst.m_LockRole = OtherPlayerMgr._inst.FindNearestEnemyOne(playerPos, false, (otherPlayer) => otherPlayer.m_isMarked || (TeamProxy.getInstance().MyTeamData.itemTeamDataList?.Exists((member) => member.cid == otherPlayer.m_unCID) ?? false), true, PK_TYPE.PK_TEAM);
                    if (SelfRole._inst.m_LockRole == null)
                        SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestSummon(playerPos);
                    if (SelfRole._inst.m_LockRole == null)
                        goto case PK_TYPE.PK_PEACE;
                    break;
                default:
                case PK_TYPE.PK_PEACE:
                    SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestMonster(playerPos, (mon) => mon.m_isMarked, true);
                    break;
                case PK_TYPE.Pk_SPOET:
                    SelfRole._inst.m_LockRole = OtherPlayerMgr._inst.FindNearestEnemyOne(playerPos, false, (otherPlayer) => otherPlayer.m_isMarked || PlayerModel .getInstance ().lvlsideid  == otherPlayer.lvlsideid , true, PK_TYPE.Pk_SPOET );
                    if (SelfRole._inst.m_LockRole == null)
                        SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestSummon(playerPos);
                    if (SelfRole._inst.m_LockRole == null)
                        goto case PK_TYPE.PK_PEACE;
                    break;
            }
        }


    }







    class SKillCdBt : Skin
    {
        public int idx;

        private GameObject goIcon;
        private GameObject goLock;
        private Image _cdmask;
        private Image skillIcon;
        private skill_a3Data _data;

        public uint maxCD = 0;
        // public long endCD = 0;

        private float _turns_Interval = 1000;
        private float _cur_truns_tm = 0;

        //private Text txtCd;

        public bool autofighting = false;

        private Button m_click_skill_btn;

        public int m_nCurDownID = 0;

        public SKillCdBt(Transform trans, int i)
            : base(trans)
        {

            idx = i;
            init();
        }
        float beginScale;
        float endScale;
        void init()
        {
            m_click_skill_btn = this.__mainTrans.gameObject.AddComponent<Button>();
            m_click_skill_btn.transition = Selectable.Transition.None;
            beginScale = recTransform.localScale.x;
            endScale = beginScale * 0.92f;
            //if (idx == 0)
            //{
            EventTriggerListener.Get(m_click_skill_btn.gameObject).onDown = onDrag;
            EventTriggerListener.Get(m_click_skill_btn.gameObject).onExit = onDragout;
            EventTriggerListener.Get(m_click_skill_btn.gameObject).onUp = onDragout;
            //}
            //else
            //{
            //    bt.onClick.AddListener(onBtClick);
            //}
            goIcon = getGameObjectByPath("icon");
            _cdmask = getComponentByPath<Image>("icon/cd");
            skillIcon = getComponentByPath<Image>("icon/icon");
            _cdmask.fillAmount = 0;

            //if (this.transform.FindChild("txtcd") != null)
            //{
            //    txtCd = getComponentByPath<Text>("txtcd");
            //    txtCd.text = "";
            //}
            goLock = this.getGameObjectByPath("lock");

            if( idx > 0 )
            {
                a1_gamejoy.inst_skillbar.m_pictext_GJ.SetNodeID(idx, this.transform.FindChild("txtcd"));
            }
        }

        public bool canUse()
        {
            if (_data == null)
                return false;

            if (skl_data.skill_id == a1_gamejoy.NORNAL_SKILL_ID)
                return true;


            if (PlayerModel.getInstance().mp < skl_data.mp)
            {
                flytxt.instance.fly(ContMgr.getCont("skill_outofmana"));
                return false;
            }

            if (skl_data.endCD > muNetCleint.instance.CurServerTimeStampMS)
            {
                //flytxt.instance.fly(ContMgr.getCont("skill_cd"));
                return false;
            }
            //if (_data.skillType == 0)
            //    BattleProxy.getInstance().sendUseSelfSkill((uint)_data.skill_id);
            return true;
        }

        public skill_a3Data skl_data
        {
            get
            {
                return _data;
            }
            set
            {
                if (GRMap.instance.m_nCurMapID == 3333 || value != null && value.now_lv != 0)
                {
                    _data = value;

                    if( idx > 0 )
                    {
                        a1_gamejoy.inst_skillbar.m_pictext_GJ.ClearNodeNum(idx);
                        _cdmask.fillAmount = 0;
                    }

                    if (_data != null)
                    {
                        Sprite sp = GAMEAPI.ABUI_LoadSprite("icon_skill_" + _data.skill_id);

                        if (sp != null)
                            skillIcon.sprite = sp;
                        goLock.SetActive(false);
                        goIcon.SetActive(true);
                    }
                    else
                    {
                        goIcon.SetActive(false);
                        goLock.SetActive(true);
                    }
                }
                else if (value == null)
                {
                    _data = null;
                    goIcon.SetActive(false);
                    goLock.SetActive(true);
                }

            }
        }

        override public bool visiable
        {
            get { return __visiable; }
            set
            {
                if (__visiable == value) return; __visiable = value;
                if (goLock != null)
                    goLock.SetActive(!__visiable);
                m_click_skill_btn.interactable = __visiable;
            }
        }

        public void doCD()
        {
            skl_data.doCD();
        }

        public void keepSkill()
        {
            //debug.Log("keepSkill" + _data.skill_id);
            if (SelfRole._inst.m_LockRole != null && !SelfRole._inst.m_LockRole.isDead)
            {
                SelfRole._inst.m_fAttackCount = 0.5f;

                if (!SelfRole._inst.m_LockRole.isfake)
                {
                    BattleProxy.getInstance().send_cast_self_skill(SelfRole._inst.m_LockRole.m_unIID, _data.skill_id);
                }

            }
            else
            {
                SelfRole._inst.m_fAttackCount = 0.5f;

                //if (_data.skill_id == 3002)
                //{//3002重新自动锁定一个目标
                //    setSelf_LockRole();
                //    if (SelfRole._inst.m_LockRole != null && !SelfRole._inst.m_LockRole.isfake)
                //    {
                //        SelfRole._inst.TurnToRole(SelfRole._inst.m_LockRole, true);
                //        BattleProxy.getInstance().send_cast_self_skill(SelfRole._inst.m_LockRole.m_unIID, _data.skill_id);
                //    }
                //}
                //else
                BattleProxy.getInstance().send_cast_self_skill(0, _data.skill_id);
            }

        }

        public void setSelf_LockRole()
        {
            if (SelfRole._inst.m_LockRole != null && SelfRole._inst.m_LockRole.isDead)
                SelfRole._inst.m_LockRole = null;

            if (PlayerModel.getInstance().now_pkState == 0)
            {
                //和平模式选中角色或召唤兽以及镖车时重新选择攻击目标(更改了，和平模式可以攻击有反击buff的玩家)
                if ((SelfRole._inst.m_LockRole is ProfessionRole && !SelfRole._inst.m_LockRole.havefanjibuff) || SelfRole._inst.m_LockRole is MS0000 || SelfRole._inst.m_LockRole is MDC000)
                {
                    SelfRole._inst.m_LockRole = null;

                }
                if (SelfRole._inst.m_LockRole is MDC000 && ((MDC000)SelfRole._inst.m_LockRole).escort_name == A3_LegionModel.getInstance().myLegion.clname)
                {
                    SelfRole._inst.m_LockRole = null;
                }
                if (SelfRole._inst.m_LockRole is MDC000)
                {
                    float curhp = (float)((float)((MDC000)SelfRole._inst.m_LockRole).curhp / (float)((MDC000)SelfRole._inst.m_LockRole).maxHp) * 100;
                    if (curhp <= 20)
                    {
                        SelfRole._inst.m_LockRole = null;
                    }
                }
            }
            else if (PlayerModel.getInstance().now_pkState == 1)
            {
                //全部模式选中自己召唤兽时重新选择攻击目标
                if (SelfRole._inst.m_LockRole is MS0000 && (SelfRole._inst.m_LockRole as MS0000).masterid == PlayerModel.getInstance().cid)
                {
                    SelfRole._inst.m_LockRole = null;
                }
                //选中自家镖车攻击时
                if (SelfRole._inst.m_LockRole is MDC000 && ((MDC000)SelfRole._inst.m_LockRole).escort_name == A3_LegionModel.getInstance().myLegion.clname)
                {
                    SelfRole._inst.m_LockRole = null;
                }
                if (SelfRole._inst.m_LockRole is MDC000)
                {
                    float curhp = (float)((float)((MDC000)SelfRole._inst.m_LockRole).curhp / (float)((MDC000)SelfRole._inst.m_LockRole).maxHp) * 100;
                    if (curhp <= 20)
                    {
                        SelfRole._inst.m_LockRole = null;
                    }
                }
            }
            else if (PlayerModel.getInstance().now_pkState == 2)
            {
                //军团模式选中己方召唤兽时重新选择攻击目标
                if (SelfRole._inst.m_LockRole is MS0000)
                {// TeamProxy.getInstance ().MyTeamData.itemTeamDataList
                    bool isTeamper = false;
                    if (TeamProxy.getInstance().MyTeamData != null)
                    {
                        foreach (ItemTeamData it in TeamProxy.getInstance().MyTeamData.itemTeamDataList)
                        {
                            if (it.cid == (SelfRole._inst.m_LockRole as MS0000).masterid)
                            {
                                isTeamper = true;
                                break;
                            }
                        }
                    }

                    if (!isTeamper && A3_LegionModel.getInstance().members != null)
                    {
                        foreach (A3_LegionMember it in A3_LegionModel.getInstance().members.Values)
                        {
                            if (it.cid == (SelfRole._inst.m_LockRole as MS0000).masterid)
                            {
                                isTeamper = true;
                                break;
                            }
                        }
                    }
                    if (isTeamper || (SelfRole._inst.m_LockRole as MS0000).masterid == PlayerModel.getInstance().cid)
                        SelfRole._inst.m_LockRole = null;
                }
                else if (SelfRole._inst.m_LockRole is MDC000)
                {
                    float curhp = (float)(((float)((MDC000)SelfRole._inst.m_LockRole).curhp / (float)((MDC000)SelfRole._inst.m_LockRole).maxHp) * 100);
                    if (((MDC000)SelfRole._inst.m_LockRole).escort_name == A3_LegionModel.getInstance().myLegion.clname)
                    {
                        SelfRole._inst.m_LockRole = null;
                    }
                    if (curhp <= 20)
                    {
                        SelfRole._inst.m_LockRole = null;
                    }
                }
                else if (SelfRole._inst.m_LockRole is ProfessionRole)
                {
                    BaseRole LockRole = SelfRole._inst.m_LockRole;
                    if (TeamProxy.getInstance().MyTeamData == null)
                        SelfRole._inst.m_LockRole = null;
                    else
                    {
                        if (!TeamProxy.getInstance().MyTeamData.IsInMyTeam(LockRole.roleName))
                            SelfRole._inst.m_LockRole = null;
                    }
                    if (PlayerModel.getInstance().clanid == 0)
                        SelfRole._inst.m_LockRole = null;
                    else
                    {
                        if (PlayerModel.getInstance().clanid != LockRole.m_unLegionID)
                            SelfRole._inst.m_LockRole = null;
                    }
                    //bool isTeamper1 = false;
                    //if (TeamProxy.getInstance().MyTeamData != null)
                    //{
                    //    foreach (A3_LegionMember it in A3_LegionModel.getInstance().members.Values)
                    //    {
                    //        if (it.cid == SelfRole._inst.m_LockRole.m_unCID)
                    //        {
                    //            isTeamper1 = true;
                    //            break;
                    //        }
                    //    }
                    //}
                    //if (!isTeamper1 && A3_LegionModel.getInstance().members != null)
                    //{
                    //    foreach (A3_LegionMember it in A3_LegionModel.getInstance().members.Values)
                    //    {
                    //        if (it.cid == SelfRole._inst.m_LockRole.m_unCID)
                    //        {
                    //            isTeamper1 = true;
                    //            break;
                    //        }
                    //    }
                    //}
                    //if (isTeamper1)
                    //    SelfRole._inst.m_LockRole = null;
                }
            }
            else if (PlayerModel.getInstance().now_pkState == 5)
            {
                if (SelfRole._inst.m_LockRole is ProfessionRole)
                {
                    BaseRole LockRole = SelfRole._inst.m_LockRole;
                    if (LockRole.lvlsideid == PlayerModel .getInstance ().lvlsideid ) SelfRole._inst.m_LockRole = null;
                }
            }
            //寻找一个可以锁定的目标, 获取其iid，进行攻击
            if (SelfRole._inst.m_LockRole == null)
                SelfRole._inst.m_LockRole = PkmodelAdmin.RefreshLockRole();

            if (SelfRole._inst.m_LockRole == null)
                SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestMonster(SelfRole._inst.m_curModel.position);

            if (SelfRole._inst.m_LockRole == null)
                SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestFakeMonster(SelfRole._inst.m_curModel.position);


        }


        public void onDrag(GameObject go)
        {
            recTransform.DOScale(endScale, 0.15f);

            //this.transform.FindChild("click").gameObject.SetActive(true);
            //this.transform.FindChild("click").GetComponent<Animator>().Play("skill_click", -1, 0f);
            //this.transform.FindChild("click").GetComponent<Animator>().SetBool("isLoop", true);

            if (a1_gamejoy.skill_wait)
            {
                flytxt.instance.fly(ContMgr.getCont("a3_insideui_fb_wait"));
                return;
            }
            if (SelfRole.fsm.Autofighting)
            {
                StateInit.Instance.PlaySkillInAutoPlay(_data.skill_id);
                return;
            }
            if (skl_data == null)
                return;

            if (!canUse())
                return;


            if (SelfRole._inst.isPlayingSkill && skl_data.skill_id != a1_gamejoy.NORNAL_SKILL_ID)
                return;

            //if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
            //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
            // if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);


            m_nCurDownID = _data.skill_id;

            setSelf_LockRole();
            //锁定技能特殊处理
            if (SelfRole._inst.m_LockRole == null)
            {

                if (skl_data.skill_id == 3006 || skl_data.skill_id == 5004 || skl_data.skill_id == 5009 || skl_data.skill_id == 3004)
                {
                    if (flytxt.instance != null)
                        flytxt.instance.fly("无锁定的目标!");
                    //SelfRole._inst.m_curModel.position =
                    //SelfRole._inst.m_roleDta.pos = MoveProxy.getInstance().GetLastSendXY() / 53.3f + new Vector3(0, SelfRole._inst.m_roleDta.pos.y,0);
                    return;
                }
            }
            else
            {
                if (skl_data.skill_id == 3006 || skl_data.skill_id == 5004 || skl_data.skill_id == 5009 || skl_data.skill_id == 3004)
                {
                    if (PkmodelAdmin.RefreshLockSkill(SelfRole._inst.m_LockRole) == false)
                    {
                        if (flytxt.instance != null)
                            flytxt.instance.fly("无锁定的目标!");
                        //SelfRole._inst.m_curModel.position =
                        //SelfRole._inst.m_roleDta.pos = MoveProxy.getInstance().GetLastSendXY()/53.3f + new Vector3(0, SelfRole._inst.m_roleDta.pos.y, 0);
                        return;
                    }
                }
            }

            if (SelfRole._inst.m_LockRole != null)
            {
                SelfRole._inst.LeaveHide();
                if (skl_data.skill_id != 3010) //法师的闪避技能维持朝向
                    SelfRole._inst.TurnToRole(SelfRole._inst.m_LockRole, true);
                SelfRole._inst.PlaySkill(_data.skill_id);
                if (!SelfRole._inst.m_LockRole.isfake)
                    BattleProxy.getInstance().send_cast_self_skill(SelfRole._inst.m_LockRole.m_unIID, _data.skill_id);
            }
            else
            {
                SelfRole._inst.LeaveHide();
                SelfRole._inst.PlaySkill(_data.skill_id);
                BattleProxy.getInstance().send_cast_self_skill(0, _data.skill_id);
            }

            if (_data.skillType == 0)
                BattleProxy.getInstance().sendUseSelfSkill((uint)_data.skill_id);


            //debug.Log("普通攻击的CD " + data.cd);   --> 500
            //doCD(false, data.cd);
            doCD();
        }

        public void onDragout(GameObject go)
        {
            //this.transform.FindChild("click").GetComponent<Animator>().SetBool("isLoop", false);

            recTransform.DOScale(beginScale, 0.15f);
            if (skl_data == null)
                return;

            m_nCurDownID = 0;
            if (skl_data.skill_id == a1_gamejoy.NORNAL_SKILL_ID)
                a1_gamejoy.lastNormalSkillTimer = 0;
        }

        //public void onBtClick()
        //{

        //    if (data == null)
        //        return;

        //    if (!canUse())
        //        return;

        //    if (SelfRole._inst.isPlayingSkill)
        //        return;

        //    if (SelfRole._inst.m_LockRole != null && SelfRole._inst.m_LockRole.isDead)
        //        SelfRole._inst.m_LockRole = null;

        //    if (SelfRole._inst.m_LockRole == null)
        //        SelfRole._inst.m_LockRole = OtherPlayerMgr._inst.FindNearestEnemyOne(SelfRole._inst.m_curModel.position);

        //    if (SelfRole._inst.m_LockRole == null)
        //        SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestMonster(SelfRole._inst.m_curModel.position);

        //    if (SelfRole._inst.m_LockRole == null)
        //        SelfRole._inst.m_LockRole = MonsterMgr._inst.FindNearestFakeMonster(SelfRole._inst.m_curModel.position);


        //    if (SelfRole._inst.m_LockRole != null)
        //    {
        //        SelfRole._inst.LeaveHide();
        //        SelfRole._inst.TurnToRole(SelfRole._inst.m_LockRole);
        //        SelfRole._inst.PlaySkill(_data.skill_id);
        //        if (!SelfRole._inst.m_LockRole.isfake)
        //            BattleProxy.getInstance().send_cast_self_skill(SelfRole._inst.m_LockRole.m_unIID, _data.skill_id);
        //    }
        //    else
        //    {
        //        SelfRole._inst.LeaveHide();
        //        SelfRole._inst.PlaySkill(_data.skill_id);
        //        BattleProxy.getInstance().send_cast_self_skill(0, _data.skill_id);
        //    }
        //    doCD(false, data.cd);
        //}



        public void update(long timestp)
        {
            if (visiable == false)
                return;
            if (skl_data == null)
                return;
            if (skl_data.endCD == 0)
                return;
            float per = 1 - (float)(timestp / skl_data.endCD);
            int cdTime = (int)(skl_data.endCD - timestp);
            _cdmask.fillAmount = (float)(cdTime) / (float)maxCD;
            if (idx > 0)
            {
                int cd_pictime = (cdTime / 1000 + 1);
                if (cd_pictime > 999) cd_pictime = 999;
                a1_gamejoy.inst_skillbar.m_pictext_GJ.SetNodeNum(idx, cd_pictime);
            }

            if (skl_data.endCD < timestp)
            {
                skl_data.endCD = 0;
                _cdmask.fillAmount = 0;
                if (idx > 0)
                    a1_gamejoy.inst_skillbar.m_pictext_GJ.ClearNodeNum(idx);

                //skillbar.Cd_lose = true;
            }
        }



        public float lastSkillUseTimer = 0;
        public uint lastIId = 0;
        public static bool needAutoNextAttack = false;
        uint curNormalSkillTurn = 0;
        public void useSkill(uint skillid, uint sklvl, bool force = false, LGAvatarGameInst mon = null)
        {
            float timer = Time.time;
            if (timer - lastSkillUseTimer < 0.2f)
                return;
            lastSkillUseTimer = timer;

            lgSelfPlayer up = lgSelfPlayer.instance;



            if (!up.canAttack() && force == false)
                return;



            LGAvatarGameInst ctMon = null;
            //  if (data.xml.target_type == 1)
            {
                if (mon == null)
                    ctMon = a1_gamejoy.getAttackTarget(9999999);
                else
                    ctMon = mon;
            }

            if (autofighting && ctMon == null)
                return;


            if (skillid != 1001)
            {
                up.attack(ctMon, false, skillid);
            }
            else
            {
                float curtm = muNetCleint.instance.CurServerTimeStampMS;
                if (_cur_truns_tm + _turns_Interval < curtm || curNormalSkillTurn == 2 || (ctMon != null && lastIId != ctMon.iid))
                {
                    if (ctMon != null)
                        lastIId = ctMon.iid;
                    curNormalSkillTurn = 0;
                }
                else
                    curNormalSkillTurn++;


                uint skid = skillid + curNormalSkillTurn;
                _cur_truns_tm = curtm;

                up.attack(ctMon, false, skid);
            }
        }

    }

}

