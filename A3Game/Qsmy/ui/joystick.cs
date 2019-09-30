//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameFramework;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using UnityEngine.EventSystems;
//namespace MuGame
//{
//    public class joystick : FloatUi
//    {
//        static public joystick instance;

//        RectTransform cv;

//        Animator ani;

//        private Vector3 Origin;
//        private Vector3 touchOrigin;
//        private Vector3 _deltaPos;
//        private bool _drag = false;
//        float dis;
//        private float MoveMaxDistance = 80;            //最大拖动距离  
//        public Vector3 MovePosiNorm;  //标准化移动的距离  
//        private float ActiveMoveDistance = 20;              //激活移动的最低距离  

//        float true_delta_x = 0.0f;
//        float true_delta_y = 0.0f;

//        public bool moveing = false;

//        //static private float angleOffset = 0;//90
//        //static public void refreshAngleOffset(float ry)
//        //{
//        //    angleOffset = ry + 45;
//        //}
//        GameObject stick;
//        public GameObject Stick => stick;
//        Transform stickTrans;
//        Transform stickBgTrans;

//        Image stick_ig;
//        Image stick_igbg;

//        //ping值显示
//        //GameObject normal;
//       // GameObject slow;
//        //Text ping;
//        public override void init()
//        {
//            instance = this;
//            alain();
//            stick = this.getGameObjectByPath("stick");
//            //GameObject touch = this.getGameObjectByPath("touch");
//            stickTrans = stick.transform;
//            stickBgTrans = this.getGameObjectByPath("Image").transform;
//            stick_ig = stickTrans.GetComponent<Image>();
//            stick_igbg = transform.FindChild("Image").GetComponent<Image>();
//            Origin = stickTrans.localPosition;//设置原点  
//            touchOrigin = Origin;
//            if (cemaraRectTran == null)
//                cemaraRectTran = GameObject.Find("canvas_main").GetComponent<RectTransform>();

//            cv = cemaraRectTran;

//            ani = transform.GetComponent<Animator>();

//            EventTriggerListener.Get(stick).onDrag = OnDrag;
//            EventTriggerListener.Get(stick).onUp = (v) => { moveing = false; };
//            EventTriggerListener.Get(stick).onDragOut = OnDragOut; 
//            if(EventTriggerListener.Get(stick).onDown != null)           
//                EventTriggerListener.Get(stick).onDown += OnMoveStart;
//            else
//                EventTriggerListener.Get(stick).onDown = OnMoveStart;

//            //EventTriggerListener.Get(touch).onDrag = OnDrag1;
//            //EventTriggerListener.Get(touch).onDragOut = OnDragOut1;
//            //EventTriggerListener.Get(touch).onDown = OnMoveStart1;

//            //if (false == Globle.A3_DEMO)
//            {
//                MoveProxy.getInstance();
//                BattleProxy.getInstance();
//                //normal = transform.FindChild("normal").gameObject;
//               // slow = transform.FindChild("slow").gameObject;
//               // ping = transform.FindChild("ping").GetComponent<Text>();
//            }
//            EventTriggerListener.Get(stick).onDown += (GameObject go) => 
//            {
//                SelfRole.fsm.manBeginPos = SelfRole._inst.m_curModel.transform.position;
//            };            
//        }

//        // Update is called once per frame  

//        public void onoffAni(bool onoff)
//        {
//             ani.SetBool("onoff", onoff);
//        }
      
//        void Update()
//        {
//            if (MouseClickMgr.instance == null) return;

//            //if (Input.touchCount >= 2)
//            //{
//            //    if (false == EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && false == EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
//            //    {
//            //        moveing = false;
//            //    }
//            //}

//            if (false == Globle.A3_DEMO)
//            {
//                //if (muNetCleint.instance.curServerPing < 500)
//                //{
//                //    //normal.SetActive(true);
//                //   // slow.SetActive(false);
//                //   // ping.text = Globle.getColorStrByQuality("ping:" + muNetCleint.instance.curServerPing.ToString(), 2);
//                //}
//                //else
//                //{
//                //    //normal.SetActive(false);
//                //   // slow.SetActive(true);
//                //  //  ping.text = Globle.getColorStrByQuality("ping:" + muNetCleint.instance.curServerPing.ToString(), 6);
//                //}
//                if (MouseClickMgr.instance.m_bTowTouchZoom)
//                {
//                    moveing = false;
//                }


//                if (moveing)
//                {
//                    dis = Vector3.Distance(new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0), touchOrigin);//拖动距离，这不是最大的拖动距离，是根据触摸位置算出来的  
//                    if (dis >= MoveMaxDistance)       //如果大于可拖动的最大距离  
//                    {
//                        Vector3 vec = touchOrigin + (new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0) - touchOrigin) * MoveMaxDistance / dis;  //求圆上的一点：(目标点-原点) * 半径/原点到目标点的距离  
//                        stickTrans.localPosition = vec;
//                    }

//                    if (Vector3.Distance(stickTrans.localPosition, touchOrigin) > ActiveMoveDistance) //距离大于激活移动的距离  
//                    {
//                        MovePosiNorm = (stickTrans.localPosition - touchOrigin).normalized;
//                        MovePosiNorm = new Vector3(MovePosiNorm.x, 0, MovePosiNorm.y);
//                    }
//                    else
//                        MovePosiNorm = Vector3.zero;

//                    Color color = stick_ig.color;
//                    color.a = 0.5f + (dis / MoveMaxDistance) / 2;
//                    stick_ig.color = color;
//                    stick_igbg.color = color;




//                    //float angle = Mathf.Atan2(MovePosiNorm.x, MovePosiNorm.z) * Mathf.Rad2Deg;
//                    // float angle = Mathf.Atan2(MovePosiNorm.z, MovePosiNorm.x) * Mathf.Rad2Deg;

//                    // UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_PLAY_MOV, this, angle-90));

//                    //debug.Log("摇杆的角度" + angle);

//                    //lgSelfPlayer.instance.onJoystickMove(angle - angleOffset);

//                    //  lgSelfPlayer.instance.onJoystickMove(angle);
//                }
//                else
//                {
//                    Color color = stick_ig.color;
//                    color.a = 0.5f;
//                    stick_ig.color = color;
//                    stick_igbg.color = color;
//                }
//            }
//            else
//            {
//                //移动将重新处理
//                if (moveing)
//                {
//                    dis = Vector3.Distance(new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0), touchOrigin);//拖动距离，这不是最大的拖动距离，是根据触摸位置算出来的  
//                    if (dis >= MoveMaxDistance)       //如果大于可拖动的最大距离  
//                    {
//                        Vector3 vec = touchOrigin + (new Vector3(touchOrigin.x + true_delta_x, touchOrigin.y + true_delta_y, 0) - touchOrigin) * MoveMaxDistance / dis;  //求圆上的一点：(目标点-原点) * 半径/原点到目标点的距离  
//                        stickTrans.localPosition = vec;
//                    }

//                    if (Vector3.Distance(stickTrans.localPosition, touchOrigin) > ActiveMoveDistance) //距离大于激活移动的距离  
//                    {
//                        MovePosiNorm = (stickTrans.localPosition - touchOrigin).normalized;
//                        MovePosiNorm = new Vector3(MovePosiNorm.x, 0, MovePosiNorm.y);
//                    }
//                    else
//                        MovePosiNorm = Vector3.zero;

//                    Color color = stick_ig.color;
//                    color.a = 0.5f + (dis / MoveMaxDistance) / 2;
//                    stick_ig.color = color;
//                    stick_igbg.color = color;


//                    //处理角色的移动(加入主角的移动)
//                    if (MovePosiNorm != Vector3.zero)
//                    {
//                        if(!SelfRole._inst.isDead)
//                            SelfRole._inst.StartMove(MovePosiNorm.x, MovePosiNorm.z);

//                        //if (tempGo == null)
//                        //{
//                        //    tempGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                        //    tempGo.layer = EnumLayer.LM_FX;
//                        //}


//                        //float angle = Mathf.Atan2(MovePosiNorm.z, MovePosiNorm.x) * Mathf.Rad2Deg;
//                        //angle = SceneCamera.m_curCamGo.transform.eulerAngles.y - angle;
//                        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
//                        //Vector3 newPos = rotation * new Vector3(2f, 0f, 0f);
//                        //tempGo.transform.position = newPos + SelfRole._inst.m_curModel.position;

//                    }





//                }
//                else
//                {
//                    Color color = stick_ig.color;
//                    color.a = 0.5f;
//                    stick_ig.color = color;
//                    stick_igbg.color = color;
//                }


//            }
//        }

//        void MiouseDown()
//        {
//            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
//            {
//            }
//            else
//            {
//                touchOrigin = Origin;
//                stickTrans.localPosition = Origin;
//            }
//        }

//        Vector3 result;

//        private Vector3 _checkPosition(Vector3 movePos, Vector3 _offsetPos)
//        {
//            result = movePos + _offsetPos;
//            return result;
//        }


//        void OnDrag(GameObject go, Vector2 delta)
//        {
//            if (!moveing)
//            {
//                OnDragOut(null);
//                return;
//            }

//            if (!_drag)
//            {
//                _drag = true;
//            }
//            _deltaPos = delta;

//            true_delta_x = true_delta_x + _deltaPos.x;
//            true_delta_y = true_delta_y + _deltaPos.y;
            
//            stickTrans.localPosition += new Vector3(_deltaPos.x, _deltaPos.y, 0);

//            if(SelfRole.fsm.Autofighting)
//                SelfRole.fsm.Pause();
//            else
//            {
//                //if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
//                //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
//                //if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
//                SelfRole.fsm.Stop();
//            }
//        }

//        [SerializeField]
//        float MaxAllowedDistance = 3f;
//        public void OnDragOut(GameObject go = null)
//        {
//            stop();

//            if (SelfRole.fsm.Autofighting)
//            {
//                if (SelfRole.fsm.CheckJoystickMoveDis(SelfRole._inst.m_curModel.position, StateInit.Instance?.maxAllowedDistance ?? MaxAllowedDistance))
//                {
//                    //if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
//                    //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
//                    //if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
//                    SelfRole.fsm.Stop();
//                }
//                else
//                {
//                    StateInit.Instance.Origin = SelfRole._inst.m_curModel.position;
//                    SelfRole.fsm.Resume();
//                }
//            }
//            else
//            {
//                //if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
//                //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
//                //if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);
//                SelfRole.fsm.Stop();
//            }
//        }

//        public void OnDragOut_wait(float t = 0.2f)
//        {
//            CancelInvoke("timeGo");
//            Invoke("timeGo", t);
//        }
//        void timeGo()
//        {
//            OnDragOut(null);
//        }

//        public void stop()
//        {
//            _drag = false;
//            stickTrans.localPosition = Origin;
//            stickBgTrans.localPosition = Origin;
//            touchOrigin = Origin;
//            //  if (PlayerMoveControl.moveEnd != null) PlayerMoveControl.moveEnd();
//            //  UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.UI_ACT_PLAY_STOP, this, null));

//            if (false == Globle.A3_DEMO)
//            {
//                lgSelfPlayer.instance.onJoystickEnd();
//            }
//            else
//            {
//                SelfRole._inst.StopMove();
//            }

          
//            moveing = false;

//            true_delta_x = 0.0f;
//            true_delta_y = 0.0f;
//        }


//        void OnMoveStart(GameObject go)
//        {
//            moveing = true;
//            //TaskModel.getInstance().isSubTask = false;
//            worldmap.Desmapimg();//消除地图移动路径
//            debug.Log("I'm moving");
//            skillbar.canClick = false;
//            if (a3_dartproxy.getInstance().dartHave)
//            {
//                a3_dartproxy.getInstance().gotoDart = false;
//                a3_liteMinimap.instance.getGameObjectByPath("goonDart").SetActive(true);
//            }
//            if (a3_expbar.instance != null) a3_expbar.instance.On_Btn_Down();
//            //InterfaceMgr.doCommandByLua("a1_low_fightgame.setToggle", "ui/interfaces/low/a1_low_fightgame", true);
//            // if (a3_liteMinimap.instance != null) a3_liteMinimap.instance.onTogglePlusClick(false);

//            //新手副本指引移动
//            UiEventCenter.getInstance().onStartMove();
//        }
//        void OnDragOut1(GameObject go)
//        {
//            stopDrag();
//        }

//        public void stopDrag()
//        {
//            _drag = false;
//            stickTrans.localPosition = Origin;
//            stickBgTrans.localPosition = Origin;
//            touchOrigin = Origin;

//            if (false == Globle.A3_DEMO)
//            {
//                lgSelfPlayer.instance.onJoystickEnd();
//            }
//            else
//            {
//                SelfRole._inst?.StopMove();
//            }


//            moveing = false;

//            true_delta_x = 0.0f;
//            true_delta_y = 0.0f;
//        }


//        void OnDrag1(GameObject go, Vector2 delta)
//        {
//            if (!moveing)
//            {
//                OnDragOut1(null);
//                return;
//            }

//            if (!_drag)
//            {
//                _drag = true;
//            }
//            _deltaPos = delta;

//            true_delta_x = true_delta_x + _deltaPos.x;
//            true_delta_y = true_delta_y + _deltaPos.y;

//            stickTrans.localPosition += new Vector3(_deltaPos.x, _deltaPos.y, 0);
//        }
//        void OnMoveStart1(GameObject go)
//        {
//            //debug.Log("设置摇杆点");
//            Vector3 touch_pos = Input.mousePosition;
//            if (Input.touchCount > 1)
//            {
//                touch_pos.x = 99999.9f;
//                //找一个离摇杆最近的触点来处理位置，以x轴向做对比
//                for (int i = 0; i < Input.touchCount; i++)
//                {
//                    Touch t = Input.GetTouch(i); //0,0的坐标为左下角
//                    if (touch_pos.x > t.position.x)
//                    {
//                        touch_pos.x = t.position.x;
//                        touch_pos.y = t.position.y;
//                    }
//                }
//            }

//            moveing = true;
//            dis = Vector3.Distance(stickTrans.position, touch_pos);
//            Vector3 pos = new Vector3(touch_pos.x / Screen.width * cv.rect.width - cv.rect.width / 2 - transform.localPosition.x,
//                touch_pos.y / Screen.height * cv.rect.height - cv.rect.height / 2 - transform.localPosition.y);
//            stickBgTrans.localPosition = pos;
//            stickTrans.localPosition = pos;
//            touchOrigin = pos;
//            //Vector3 vec = Origin + (Input.mousePosition - stickTrans.position) * MoveMaxDistance / dis;  //求圆上的一点：(目标点-原点) * 半径/原点到目标点的距离  
//            //stickTrans.localPosition = vec;

//            SelfRole.fsm.Pause();
//        }
//    }
//}
