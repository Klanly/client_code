using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;
using System.Text;
namespace MuGame
{
    class GRAvatar : GRBaseImpls, IObjectPlugin
    {
        public int m_nSex = -1;
        public int[] m_nOtherDress = { 0, 0, 0, 0 };

        private bool _isMain = false;
        private bool _isCreateName = false;
        public string _name;
        public string _id;
        public string _fileName;
        private List<IUIImageNum> _imgNums = new List<IUIImageNum>();
        private float _speed = 3f;

        //private _graphChaSpriteHost _chaSprHost;

        private GRCharacter3D m_skMount;//角色的坐骑
        private bool m_blinkToMount = false;

        private Transform m_tfChar;
        private Transform m_tfMount;

        private float m_ftargetRot_Y = 0f;

        public LGAvatarGameInst lgAvatar;

        private GameObject m_rib_Wp;
        private bool m_rib_Active;

        public GRAvatar(muGRClient ctrl)
            : base(ctrl)
        {

            _charInfo = new Variant();
            _charInfo["x"] = 0;
            _charInfo["y"] = 0;
            _charInfo["name"] = "";
            _charInfo["ani"] = GameConstant.ANI_IDLE_NORMAL;
            _charInfo["ori"] = 0;
            _charInfo["loop"] = false;
            _charInfo["avatarid"] = "";
            _charInfo["equip"] = new Variant();

            //_chaSprHost = new _graphChaSpriteHost();
        }

        override public void initLg(lgGDBase lgbase)
        {
            if (lgbase is LGAvatarGameInst)
                lgAvatar = lgbase as LGAvatarGameInst;
        }

        public void startMount(string id)
        {
            if (m_skMount == null)
            {
                m_skMount = GRClient.instance.world.createEntity(Define.GREntityType.CHARACTER) as GRCharacter3D;
                m_skMount.load(GraphManager.singleton.getCharacterConf(id));

                m_tfChar = m_char.gameObject.transform.parent;
                m_tfMount = m_skMount.gameObject.transform.parent;

                m_blinkToMount = true;

                m_char.playAnimation("mount_idle", 0);
            }
        }

        public void disMount()
        {
            if (m_skMount != null)
            {
                if (m_blinkToMount == false)
                {
                    //将所有的东西回归原位
                    m_char.gameObject.transform.SetParent(m_tfChar, false);
                    m_skMount.gameObject.transform.SetParent(m_tfMount, false);
                }

                m_skMount.dispose();
                m_skMount = null;

                m_char.playAnimation("idle", 0);
            }
        }

        private Transform getMountBone(Transform root)
        {
            if (root.name.Equals("mount"))
            {
                return root;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform one_tree = getMountBone(root.GetChild(i));
                if (one_tree != null) return one_tree;
            }

            return null;
        }

        private Transform getWpRibbon(Transform root)
        {
            if (root.name.Equals("RightHand"))
            {
                return root;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform one_tree = getWpRibbon(root.GetChild(i));
                if (one_tree != null) return one_tree;
            }

            return null;
        }

        public void RefreshOtherAvatar()
        {
            if (m_char == null || m_nSex < 0)
            {
                return;
            }

            m_char.removeAvatar("RightHand");
            m_char.removeAvatar("body");
            m_char.removeAvatar("hair");
            m_char.removeAvatar("Bip01 Spine1");
            string sex_id = m_nSex.ToString();
            for (int i = 0; i < m_nOtherDress.Length; i++)
            {
                int p_id = m_nOtherDress[i];
                if (p_id > 0)
                {
                    if (GraphManager.singleton.getAvatarConf(sex_id, p_id.ToString() + sex_id) != null)
                        m_char.applyAvatar(GraphManager.singleton.getAvatarConf(sex_id, p_id.ToString() + sex_id));
                    else if (i == 0)
                        m_char.applyAvatar(GraphManager.singleton.getAvatarConf(sex_id, "1000" + sex_id));
                }
                else
                {
                    if (0 == i)
                    {
                        m_char.applyAvatar(GraphManager.singleton.getAvatarConf(sex_id, "1000" + sex_id));
                    }
                }
            }




        }


        private string avatarid
        {
            get
            {
                return _charInfo["avatarid"]._str;
            }
        }

        public static IObjectPlugin create(IClientBase m)
        {
            return new GRAvatar(m as muGRClient);
        }
        override public void init()
        {
            initChaSprHost(140, 10, 160, 150, 130);

        }
        override protected void onSetSceneCtrl()
        {
           // m_ctrl.addEventListener(GAME_EVENT.SPRITE_SET_VISIBLE, setVisible);


            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_ANI, setAni);
            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_ORI, setOri);
            m_ctrl.addEventListener(GAME_EVENT.SPRITE_DISPOSE, ondispose);
            m_ctrl.addEventListener(GAME_EVENT.SPRITE_SET_DATA, setData);
            //   m_ctrl.addEventListener(GAME_EVENT.SPRITE_SET_XY, upDateView);

            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_CHANGE_AVATAR, onChangeAvatar);
            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_REMOVE_AVATAR, onRemoveAvatar);
            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_SET_HP, setHp);
            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_ADD_TITLE, onAddTitle);
            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_REMOVE_TITLE, onRemoveTitle);
            //m_ctrl.addEventListener(GAME_EVENT.SPRITE_REMOVE_EFF, onRemoveEff);


            // m_ctrl.g_mgr.addEventListener(GAME_EVENT.SPRITE_GR_CAMERA_MOVE, onMainPlayerMove);

        }

        void onDie(GameEvent e)
        {


        }

        protected void initChaSprHost(float titleHeight, float groundHeight, float dynamicHeight, float hpHeight, float dpHeight)
        {
            //_chaSprHost.titleHeight = titleHeight;
            //_chaSprHost.groundHeight = groundHeight;
            //_chaSprHost.dynamicHeight = dynamicHeight;
            //_chaSprHost.hpHeight = hpHeight;
            //_chaSprHost.dpHeight = dpHeight;
        }

        public Variant _charInfo;
        public GameObject m_charShadow;
        override protected void onSetGraphImpl()
        {
            (m_char as GREntity3D).addEventListener(Define.EventType.RAYCASTED, onMouseDown);
            //this.m_shadow = this.g_mgr.createGraphCharShadow(GameConstant.DEFAULT_SHADOW);

            //加入影子
            m_charShadow = GameObject.Instantiate(gameST.SHADOW_PREFAB) as GameObject;
            m_charShadow.transform.SetParent(m_char.gameObject.transform.parent, false);

            RefreshOtherAvatar();
        }

        private void onMouseDown(Cross.Event e)
        {
            dispatchEvent(
                GameEvent.Createimmedi(GAME_EVENT.SPRITE_ON_CLICK, this, null)
            );
        }
        //private void onChangeAvatar(GameEvent e)
        //{
        //    changeAvatar(e.data);
        //}
        //private void changeAvatar(Variant eqp)
        //{
        //    _charInfo["equip"].pushBack(eqp);
        //    uint tpid = eqp["tpid"]._uint;
        //    avatarInfo ainfo = this.g_mgr.getEqpAvatarInfo(tpid, eqp);
        //    if (ainfo != null)
        //        posMapAdd(ainfo);
        //}
        //private void onRemoveAvatar(GameEvent e)
        //{
        //    removeAvatar(e.data);
        //}

        private void removeAvatar(Variant eqp)
        {
            uint iid = eqp["id"]._uint;
            List<Variant> arr = _charInfo["equip"]._arr;
            for (int i = arr.Count - 1; i >= 0; i--)
            {
                Variant e = arr[i];
                if (e["id"]._uint == eqp["id"]._uint)
                {
                    arr.RemoveAt(i);
                }
            }
            //todo remove _charInfo["equip"] mark
            //avatarInfo ainfo = this.g_mgr.getEqpAvatarInfo( tpid );
            posMapRmv(eqp["tpid"]._uint, iid);
        }

        private Dictionary<uint, List<avatarInfo>> _avatarMap = new Dictionary<uint, List<avatarInfo>>();
        private Dictionary<uint, Variant> m_conf = new Dictionary<uint, Variant>();
        private Dictionary<uint, bool> m_bool = new Dictionary<uint, bool>();
        private void posMapAdd(avatarInfo ainfo)
        {
            Variant conf;
            if (ainfo.pos == GameConstant.EQP_POS_WEAPON)
            {
                if (m_bool.Count > 0)
                {
                    foreach (bool v in m_bool.Values)
                    {
                        if (v)
                            ainfo.isLeft = false;
                        else
                            ainfo.isLeft = true;
                    }
                }
                if (!m_bool.ContainsKey(ainfo.iid))
                    m_bool.Add(ainfo.iid, ainfo.isLeft);
            }
            conf = this.g_mgr.getAvatarConf(avatarid, ainfo.avatarid, ainfo.isLeft);
            if (conf == null) return;
            string mtid = "";
            if (ainfo.flvl > 0)
            {
                mtid = itmesConf.GetItemFlvlmat(ainfo.tpid, ainfo.flvl);

            }

            if (mtid != "")
            {
                m_char.applyAvatar(conf, mtid);
            }
            else
            {
                m_char.applyAvatar(conf);
            }

            if (!m_conf.ContainsKey(ainfo.iid))
                m_conf.Add(ainfo.iid, conf);


        }
        private void posMapRmv(uint tpid, uint iid)
        {
            //List<avatarInfo> arr;
            //if (!_avatarMap.ContainsKey(tpid))
            //{
            //    return;
            //}
            //avatarInfo ainfo;
            //arr = _avatarMap[tpid];
            //Variant conf;
            //if (arr.Count > 1)
            //{
            //    ainfo = arr[1];
            //    conf = this.g_mgr.getAvatarConf(avatarid, ainfo.avatarid, ainfo.isLeft);
            //    m_char.removeAvatar(conf["part"]._str);

            //    //todo refresh flvl effect
            //    if (ainfo.iid == iid)
            //    {
            //        arr.RemoveAt(1);
            //    }
            //    else
            //    {
            //        arr.RemoveAt(0);
            //    }
            //}
            //else
            //{
            //    ainfo = arr[0];
            //    conf = this.g_mgr.getAvatarConf(avatarid, ainfo.avatarid);
            //    if (conf == null) return;
            //    m_char.removeAvatar(conf["part"]._str);
            //    arr.Clear();
            //}
            if (m_conf.ContainsKey(iid))
            {
                Variant conf = m_conf[iid];
                m_char.removeAvatar(conf["part"]._str);
                m_conf.Remove(iid);
                if (m_bool.ContainsKey(iid))
                    m_bool.Remove(iid);
            }
        }


        //private void setOri(GameEvent e)
        //{
        //    setOri(e.data);
        //}

        public void setOri(float f_rot)
        {
            _charInfo["ori"] = f_rot;// data["ori"]._float;

            //if (m_char == null)
            //{
            //    return;
            //}

            float rot = 90f - _charInfo["ori"]._float;
            //rot += 360f;

            //if (rot == 90 || rot == 270)
            //{
            //    //rot += 180;
            //}
            //else
            //{
            //    rot = 360 - rot;
            //}

            //m_char.rotY = rot;

            //if (rot < 0)
            //{
            //    rot += 360f;
            //}

            if (rot > 360f)
            {
                rot -= 360f;
            }

            if (rot < 0f)
            {
                rot += 360f;
            }

            //debug.Log("改变人物的朝向 " + rot);

            m_ftargetRot_Y = rot;
            //m_char.rotY = rot;
        }

        //private void setVisible(GameEvent e)
        //{
        //    bool v = e.data["visible"]._bool;
        //    _charInfo["visible"] = v;

        //    if (m_char == null)
        //    {
        //        return;
        //    }
        //    if (v)
        //        PlayerNameUIMgr.getInstance().show(this);
        //    else
        //        PlayerNameUIMgr.getInstance().hide(this);

        //    //if (this.m_shadow != null)
        //    //    this.m_shadow.showAvatar(v);

        //    m_char.showAvatar(v);
        //}

        public void setAni(string anim, int loop)
        {
            if (m_char == null)
            {
                return;
            }

            if (m_skMount != null)
            {
                if (0 == loop)
                {
                    m_skMount.playAnimation(anim, 0);
                    m_char.playAnimation("mount_" + anim, 0);
                }
            }
            else
            {
                //debug.Log("setAni " + anim);
                m_char.playAnimation(anim, loop);
            }
        }

        private void setAni(GameEvent e)
        {
            //return;
            //setAni(e.data);
        }

        private void setAni(Variant data)
        {
            //return;
            //_charInfo["ani"] = data["ani"];
            //_charInfo["loop"] = data["loop"];

            //if (m_char == null)
            //{
            //    return;
            //}

            //if (m_skMount != null)
            //{
            //    //debug.Log("处理坐骑动作。。。。。。。。。。。。。。。。。。");
            //    bool bloop = data["loop"];
            //    if (bloop)
            //    {
            //        m_skMount.playAnimation(data["ani"], true);
            //        m_char.playAnimation("mount_" + data["ani"], true);
            //    }
            //}
            //else
            //{
            //    m_char.playAnimation(data["ani"], data["loop"]);
            //}
        }


        //private void setEquip(Variant data)
        //{
        //    if (m_char == null)
        //    {
        //        _charInfo["equip"] = data["equip"];
        //        return;
        //    }
        //    Variant arr = data["equip"];
        //    if (arr == null) return;
        //    for (int i = 0; i < arr.Count; i++)
        //    {
        //        Variant eqp = arr[i];
        //        changeAvatar(eqp);
        //    }
        //}

        //private void onAniEnd(Event se)
        //{
        //    dispatchEvent(
        //        GameEvent.Createimmedi(GAME_EVENT.SPRITE_ANI_END, this, null)
        //    );
        //}


        public GRCharacter3D m_char
        {
            get
            {
                return m_gr as GRCharacter3D;
            }
        }

        //private GRCharacter3D m_shadow
        //{
        //    get
        //    {

        //        return m_gr_shadow as GRCharacter3D;
        //    }
        //    set
        //    {
        //        m_gr_shadow = value;
        //    }
        //}

        //public void refreshVipLv(uint lv)
        //{
        //    PlayerNameUIMgr.getInstance().refreshVipLv(this, lv);
        //}

        //private void onMainPlayerMove(GameEvent e)
        //{
        //    if (m_char == null) return;

        //    if (_isCreateName)
        //    {
        //        changeGroundSprPos();
        //    }
        //    _changeNumPos();
        //}
        private void setData(GameEvent e)
        {
            Variant data = e.data;
            Variant avatarConf = data["avatarConf"];

            if (data.ContainsKey("isCreateName"))
            {
                _isCreateName = data["isCreateName"]._bool;
            }

            if (data.ContainsKey("isMain"))
            {
                _isMain = data["isMain"]._bool;
                _isCreateName = true;
            }


            ////jason 各个角色进入到视野 不是主角先屏蔽了
            //    if (false == _isMain && data["name"]!="蜘蛛") return;


            if (avatarConf == null)
            {
                //DebugTrace.print("name:" + _name + "x:" + data["x"] + "y:" + data["y"] + "z:" + data["z"]);
                return;
            }

            if (data.ContainsKey("name"))
            {
                _name = data["name"]._str;
                _charInfo["name"] = _name;
                //if (_isCreateName)
                //{
                //    if (data.ContainsKey("titleConf"))
                //    {
                //        addTitleSprite(data["titleConf"]);
                //    }
                //}
            }
            _charInfo["avatarid"] = avatarConf["id"]._str;
            _id = _charInfo["avatarid"];

            string filepath = avatarConf["file"]._str;
            string[] arr = filepath.Split(new char[] { '/' });
            _fileName = arr[arr.Length - 1];

            if (data.ContainsKey("hp") && data.ContainsKey("max_hp"))
            {
                curHp = _charInfo["hp"] = data["hp"];
                MaxHp = _charInfo["max_hp"] = data["max_hp"];
              //  PlayerNameUIMgr.getInstance().show(this);
            }


            ////角色的管理器已经更换 jason
            //if (data.ContainsKey("iid") && false == _isMain)
            //{
            //    string str_iid = data["iid"];
            //    string str_name = data["name"];
            //    debug.Log("有玩家进入 id " + str_iid + "  名字 " + str_name);
            //    Transform one = U3DAPI.DEF_TRANSFORM;
            //    one.position = SelfRole._inst.m_curModel.position;
            //    OtherPlayerMgr._inst.AddOtherPlayer(one, 1);
            //}


            return;

            if (m_char == null)
            {

                setGraphImpl(this.g_mgr.createGraphChar(avatarConf));

                //jason 给主角那把武器
                if (true == _isMain)
                {
                    //(m_gr as GRCharacter3D).applyAvatar(GraphManager.singleton.getAvatarConf("1", "10000")); //男角色的默认武器
                    lgSelfPlayer.instance.RefreshPlayerAvatar();
                }

                //m_char.addEventListener(Define.EventType.SKANIM_END, onAniEnd);
                m_char.m_ActCharActOver_CB = charactPlayOver;
            }

            headoffset = new UnityEngine.Vector3(0, m_char.chaHeight, 0);

            //setAni(data);
            setOri(data["ori"]._float);
            //setEquip(data);
            updateXYZ(data["x"], data["y"], data["z"]);
            inited = true;
            onLoadFin();
        }
        public bool inited = false;


        virtual protected bool charactPlayOver()
        {
            //dispatchEvent(
            //    GameEvent.Createimmedi(GAME_EVENT.SPRITE_ANI_END, this, null)
            //);


            return lgAvatar.onAniEnd();



            //如果已经加入的动作请返回true  false表示这个动作结束就没有动作了

            //if (aniEndHandle == null || !aniEndHandle())
            //return false;
            //else 
            //    return true;
        }

        virtual protected void onLoadFin()
        {



            if (!lgAvatar.destroy)
                lgAvatar.onGrLoaded(this);
        }

        //data { x:, y: }
        private void upDateView(GameEvent e)
        {
            Vec3 p = m_ctrl.getPoss();
            updateXYZ(p.x, p.y, p.z);
        }

        public bool updateY()
        {
            return false;

            float tempx = pTrans.position.x;
            float tempy = pTrans.position.z;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(new Vector3(tempx,50f, tempy), out hit, 100f, 1 << NavMesh.GetNavMeshLayerFromName("Default")))
            {
                Vector3 vec = hit.position;
                updateXYZ(tempx, vec.y, tempy);
                return true;
            }

            return false;
        }

        private void updateXYZ(float unity_x, float unity_y, float unity_z)
        {
            if (m_char == null) return;
            //_charInfo["x"] = unity_x;
            //_charInfo["y"] = unity_y + GameConstant.CHAR_OFFSET_Y;
            //_charInfo["z"] = unity_z;
            //m_char.x = _charInfo["x"];
            //m_char.y = _charInfo["y"];
            //m_char.z = _charInfo["z"];

            if (pTrans == null)
                return;

            pTrans.position = new Vector3(unity_x, unity_y, unity_z);

            if (_isMain)
            {
                //DebugTrace.contentAdd( " #################  m_char.x["+m_char.x+"]    m_char.y["+m_char.y+"]    m_char.z["+m_char.z+"]  "  );
                lgcam.updateMainPlayerPos(unity_x, unity_y, unity_z);
            }

            if (!_isMain && _isCreateName)
            {
                changeGroundSprPos();
            }

            //updateXYZforShadow();
        }

        //private void updateXYZforShadow()
        //{
        //    if (this.m_shadow == null) return;
        //    this.m_shadow.x = this.m_char.x;
        //    this.m_shadow.y = this.m_char.y + GameConstant.SHADOW_OFFSET_Y;
        //    this.m_shadow.z = this.m_char.z;
        //}

        // === objs ===
        public LGCamera lgcam
        {
            get
            {
                return g_mgr.g_gameM.getObject(OBJECT_NAME.LG_CAMERA) as LGCamera;
            }
        }
        public ClientItemsConf itmesConf
        {
            get
            {
                return g_mgr.g_gameConfM.getObject(OBJECT_NAME.CONF_LOCAL_ITEMS) as ClientItemsConf;
            }
        }

        public UnityEngine.Vector3 getHeadPos()
        {
            //if (lastNamePlayerPos != null && lastNamePlayerPos.x == m_char.pos.x && lastNamePlayerPos.y == m_char.pos.y && lastNamePlayerPos.z== m_char.pos.z)
            //    return lastHeadpos;

            if (m_char == null)
                return UnityEngine.Vector3.zero;




            UnityEngine.Vector3 v3 = pTrans.position + headoffset;
            if (GRMap.GAME_CAM_CAMERA == null)
                return UnityEngine.Vector3.zero;


            if (InterfaceMgr.ui_Camera_cam == null)
                return UnityEngine.Vector3.zero;

            if (InterfaceMgr.ui_Camera_cam != null && !InterfaceMgr.ui_Camera_cam.gameObject.active)
                InterfaceMgr.ui_Camera_cam.gameObject.SetActive(true);

            v3 = GRMap.GAME_CAM_CAMERA.WorldToScreenPoint(v3);

            v3.z = 0f;
            lastHeadpos = v3;
            return v3;
        }

        private float m_fhitFlash_time = 0f;
        private bool m_bhitFlashGoUp = false;
        private bool m_bDoHitFlash = false;

        private Camera cameraText;

        private UnityEngine.Vector3 lastHeadpos;
        private UnityEngine.Vector3 lastNamePlayerPos;
        private UnityEngine.Vector3 headoffset;
        private Vector3 middleoffset;
        private Transform _pTrans;
        public Transform pTrans
        {
            get
            {
                if (_pTrans == null)
                {
                    if (m_char == null || m_char.gameObject == null)
                        return null;

                    _pTrans = m_char.gameObject.transform.parent.transform;
                }
                return _pTrans;
            }

        }

        public void onHurt(Variant d)
        {
            UnityEngine.Vector3 v3 = getHeadPos();
            if (d.ContainsKey("ft"))
            {
                if (d.ContainsKey("criatk"))
                    FightText.play(d["ft"], v3, d["dmg"], d["criatk"]);
                else
                    FightText.play(d["ft"], v3, d["dmg"]);
            }

            if (d.ContainsKey("hurteff"))
            {
                if (middleoffset == Vector3.zero)
                    middleoffset = new Vector3(0, m_char.chaHeight * 0.5f, 0);
                if (pTrans == null)
                    return;

                //if (m_char.curAnimName == "idle")
                if (false == lgAvatar.IsDie() && false == lgAvatar.m_bSinging)
                {
                    //debug.Log("播放受击动作");

                    if (m_char.curAnimName == "idle" || m_char.curAnimName == "run" || m_char.curAnimName == "hurt")
                        setAni("hurt", 1);
                }
                 
                uint skillid = 1001;
                if (d.ContainsKey("skill_id"))
                    skillid = d["skill_id"];

                SkillHitedXml hitxml = SkillModel.getInstance().getSkillXml(skillid).hitxml;
                if (hitxml != null)
                {
                    m_char.setMtlColor(gameST.HIT_Rim_Color_nameID, hitxml.rimColor);//new Color(1, 0, 0)
                    m_char.setMtlColor(gameST.HIT_Main_Color_nameID, hitxml.mainColor);//new Color(0.25f, 0.25f, 0.25f)
                    m_bhitFlashGoUp = true;
                    m_bDoHitFlash = true;
                }

                MapEffMgr.getInstance().play("hurt", pTrans.position + middleoffset, pTrans.eulerAngles, 0f);
            }



            //this.m_char.gameObject.renderer.material = null;
            //_hurtTm = this.g_mgr.g_netM.CurServerTimeStampMS;
            //Variant data = e.data;
            //if (data.ContainsKey("dmg"))
            //{
            //   creatImgNum(0, data["dmg"]);
            //}
        }

        public void playSong(uint skillid)
        {
            if (pTrans == null)
                return;

            //播放吟唱特效
            //MapEffMgr.getInstance().play("warnning_1", pTrans, UnityEngine.Random.Range(0f, 359f), 0f);

            SkillXmlData skillxml = SkillModel.getInstance().getSkillXml(skillid);
            MapEffMgr.getInstance().play(skillxml.eff + "_song", pTrans, UnityEngine.Random.Range(0f, 359f), 0f);
        }

        public void playDrop_jinbi_fx()
        {
            //if (pTrans == null)
            //{
            //    if (m_char == null || m_char.gameObject == null)
            //        return;

            //    pTrans = m_char.gameObject.transform.parent.transform;
            //}

            //MapEffMgr.getInstance().play("drop_jinbin", pTrans, UnityEngine.Random.Range(0f, 359f), 0f);

            //if (MapProxy.MAP_DROP_MONEY <= 0) return;

            //debug.Log("掉落金币 " + MapProxy.MAP_DROP_MONEY);

        //    MapProxy.MAP_DROP_MONEY = 0;

            //新爆金币策略,取模型的Bip001骨骼来爆
            if (m_char != null && m_char.gameObject != null)
            {
                Transform cur_sk = m_char.gameObject.transform;
                if (cur_sk.childCount > 0)
                {
                    Transform cur_model = cur_sk.GetChild(0);
                    if (cur_model.childCount > 0)
                    {
                        Transform first_bone = cur_model.GetChild(0);
                        Vector3 pos_t = first_bone.localToWorldMatrix.MultiplyPoint(Vector3.zero);
                        MapEffMgr.getInstance().play("drop_jinbin", pos_t, Quaternion.identity, 0f);
                    }
                }
            }
        }

        public int curHp;
        public int MaxHp;

        //private void setHp(GameEvent e)
        //{
        //    Variant data = e.data;
        //    curHp = data["cur"];
        //    MaxHp = data["max"];
        //    PlayerNameUIMgr.getInstance().refreshHp(this, data);
        //    //SetDpShowInfo(data);
        //}

        public void setUIHp(int cur, int max)
        {
            curHp = cur;
            MaxHp = max;
        //    PlayerNameUIMgr.getInstance().refreshHp(this, cur, max);
        }

        public void onAddEff(Variant d)
        {
            string effid = d["effid"]._str;
            float angle = 0;
            bool loop = false;
            bool single = true;
            if (d.ContainsKey("loop"))
            {
                loop = d["loop"]._bool;
            }
            if (d.ContainsKey("angle"))
            {
                angle = d["angle"]._float;
            }
            if (d.ContainsKey("single"))
            {
                single = d["single"]._bool;
            }
            IGREffectParticles eff;

            //if (eff != null)
            //{
            //    if (angle != 0)
            //        eff.rotY = 450f - angle;
            //    else
            //        eff.rotY = m_char.rotY;


            //    eff.loop = loop;
            //    eff.play();
            //}
        }

        //protected void onRemoveEff(GameEvent e)
        //{
        //    Variant d = e.data;
        //    string effid = d["effid"]._str;
        //    IGREffectParticles eff = getSingleEffect(effid);
        //    if (eff != null)
        //    {
        //        eff.stop();
        //    }
        //}


        private void _setProgressData()
        {

        }
        private void creatImgNum(int state, int num)
        {
            //if (m_char == null)
            //    return;

            //IUIImageNum imgnum = UIManager.singleton.createControl("imagenum", "") as IUIImageNum;
            //if (state == 3)
            //{
            //    //TODO  判读哪种类型的字   _imgnum.assetDesc = "textToimgReds";
            //}



            //imgnum.assetDesc = "textToimgReds";
            //Vec3 v = imgnum.changePot(m_char.pos + new Vec3(0, 1.8f, 0));
            //imgnum.x = v.x;
            //imgnum.y = 720 - v.y;
            //imgnum.num = num + "";
            //_imgNums.Add(imgnum);
        }

        //private void _changeNumPos()
        //{
        //    for (int i = 0; i < _imgNums.Count; i++)
        //    {
        //        Vec3 v = _imgNums[i].changePot(m_char.pos + new Vec3(0, 1.8f, 0));
        //        _imgNums[i].x = v.x;
        //        _imgNums[i].y = 720 - v.y;
        //    }
        //}

        private void _imgNumMove(float tmSlice)
        {
            for (int i = 0; i < _imgNums.Count; i++)
            {
                _imgNums[i].y -= _speed * tmSlice * 60;
                _imgNums[i].movedistance += _speed * tmSlice * 60;
                if (_imgNums[i].movedistance > 100)
                {
                    _imgNums[i].dispose();
                    _imgNums.RemoveAt(i);
                    i--;
                }
            }
        }

        private void _disposeImgNum()
        {
            for (int i = 0; i < _imgNums.Count; i++)
            {
                _imgNums[i].dispose();
                _imgNums.RemoveAt(i);
                i--;
            }
        }

        private void ondispose(GameEvent e)
        {
            //   m_ctrl.g_mgr.removeEventListener(GAME_EVENT.SPRITE_GR_CAMERA_MOVE, onMainPlayerMove);
            this.dispose();
        }

        override public void dispose()
        {
            disMount();

            _disposeImgNum();
            //if (_chaSprHost != null)
            //{
            //    _chaSprHost.dispose();
            //    _chaSprHost = null;
            //}
         //   PlayerNameUIMgr.getInstance().hide(this);
            if (this.m_char != null) this.g_mgr.deleteEntity(this.m_char);

            //if (this.m_shadow != null) this.g_mgr.deleteEntity(this.m_shadow);
        }

        public void showWpRibbon()
        {
            //if (m_rib_Wp == null || m_rib_Active == false) return;

            //m_rib_Active = true;
            //m_rib_Wp.SetActive(true);
        }

        public void hideWpRibbon()
        {
            //if (m_rib_Wp == null || m_rib_Active == true) return;

            //m_rib_Active = false;
            //m_rib_Wp.SetActive(false);
        }

        //public bool linkWpRibbon()
        //{
        //    if (m_rib_Wp != null) return false;

        //    Transform t_rib = getWpRibbon(m_char.gameObject.transform);
        //    if (t_rib != null)
        //    {
        //        GameObject rib_prefab = U3DAPI.U3DResLoad<GameObject>("ribbon/rib_1");
        //        m_rib_Wp = GameObject.Instantiate(rib_prefab) as GameObject;

        //        m_rib_Wp.transform.SetParent(t_rib, false);
        //        m_rib_Active = true;
        //    }

        //    return true;
        //}




        private int m_nBornStep = 0;
        private float m_fBornAlpha = 0f;
        private float _hurtTm = 0;
        private float time;
        override public void updateProcess(float tmSlice)
        {
            if (m_blinkToMount && m_skMount != null)
            {
                if (m_skMount.gameObject.transform.childCount > 0)
                {
                    m_blinkToMount = false;
                    m_skMount.gameObject.transform.SetParent(m_char.gameObject.transform.parent.transform, false);

                    //将主角绑到坐骑上
                    Transform t_mount = getMountBone(m_skMount.gameObject.transform);// m_skMount.gameObject.transform.FindChild("mount");
                    if (t_mount != null)
                    {
                        debug.Log("坐骑的位置 绑定成功");

                        GameObject linker = new GameObject();
                        linker.transform.eulerAngles = new Vector3(90f, 270f, 0f);

                        linker.transform.SetParent(t_mount, false);
                        m_char.gameObject.transform.SetParent(linker.transform, false);
                    }
                }
            }

            //if (lgAvatar is LGAvatarMonster)
            //{
            //    //出生的alpha效果
            //    if (0 == m_nBornStep)
            //    {
            //        if (m_char.changeMtl(gameST.BORN_MTL))
            //        {
            //            m_nBornStep = 1;
            //        }
            //    }
            //    else if (1 == m_nBornStep)
            //    {
            //        m_fBornAlpha += tmSlice;
            //        if (m_fBornAlpha >= 1f)
            //        {
            //            m_nBornStep = 2;
            //            m_char.changeMtl(gameST.CHAR_MTL);
            //        }
            //        else
            //        {
            //            m_char.setMtlColor(gameST.HIT_Main_Color_nameID, new Color(1f, 1f, 1f, m_fBornAlpha));
            //        }
            //    }
            //}

            if (m_bDoHitFlash && m_nBornStep > 1)
            {
                if (m_bhitFlashGoUp)
                {
                    m_fhitFlash_time += tmSlice * 10f;

                    if (m_fhitFlash_time > 0.25f)
                    {
                        m_fhitFlash_time = 0.25f;
                        m_bhitFlashGoUp = false;
                    }

                    m_char.setMtlFloat(gameST.HIT_Rim_Width_nameID, m_fhitFlash_time * 6);
                }
                else
                {
                    m_fhitFlash_time -= tmSlice;
                    if (m_fhitFlash_time <= 0.0f)
                    {
                        m_fhitFlash_time = 0f;
                        m_bDoHitFlash = false;
                        m_char.setMtlColor(gameST.HIT_Rim_Color_nameID, new Color(0, 0, 0));
                        m_char.setMtlColor(gameST.HIT_Main_Color_nameID, new Color(1f, 1f, 1f));
                    }
                    else
                    {
                        m_char.setMtlFloat(gameST.HIT_Rim_Width_nameID, m_fhitFlash_time * 6);
                    }
                }
            }

            if (m_char != null && m_char.rotY != m_ftargetRot_Y)
            {
                //Debug.Log("update ---------------------------------" + m_char.rotY);

                float rot_step = m_ftargetRot_Y - m_char.rotY;
                float frot_step_abs = Mathf.Abs(rot_step);
                if (frot_step_abs < 180f)
                {
                    if (frot_step_abs < 4f)
                    {
                        m_char.rotY = m_ftargetRot_Y;
                    }
                    else
                    {
                        m_char.rotY = m_char.rotY + rot_step * 0.25f;
                    }
                }
                else
                {
                    //需要向0度靠近，以改变值
                    if (m_char.rotY > 180f)
                    {
                        m_char.rotY += 16f;
                        //if (m_char.rotY >= 360f) m_char.rotY -= 360f;
                        if (m_char.rotY >= 360f) m_char.rotY = 1f;
                    }
                    else
                    {
                        m_char.rotY -= 16f;
                        //if (m_char.rotY < 0f) m_char.rotY += 360f;
                        if (m_char.rotY < 0f) m_char.rotY = 359f;
                    }
                }
            }


            float curTm = this.g_mgr.g_netM.CurServerTimeStampMS;
            _imgNumMove(tmSlice);
            //if (_chaSprHost != null)
            //{
            //    if (curTm - _hurtTm > 20000)//20秒没有受到伤害则隐藏血条
            //    {
            //        _hurtTm = 0;
            //        _chaSprHost.ShowDp(false);
            //    }
            //    _chaSprHost.process(tmSlice);
            //}
            time += tmSlice;
            //if (time > 0.25f)
            //{
            //    if (_isMain)
            //        lgcam.obj_mask(m_char.pos);
            //    time = 0;
            //}
        }
        protected void onAddTitle(GameEvent e)
        {
            //Variant data = e.data;
            //addTitleSprite(data);
        }
        //protected void onRemoveTitle(GameEvent e)
        //{
        //    Variant data = e.data;
        //    removeTitleSprite(data["tp"], data["showtp"]._int);
        //}

        //------------------------------------头顶血条Start-------------------------------------------
        protected Boolean _isInitDp = false;
        protected Boolean _isLoad = false;
        protected Variant _dpConf;
        public void ShowChaDp(Boolean flag)
        {
            //if (_chaSprHost == null)
            //{
            //    return;
            //}
            //if (_isInitDp)
            //{
            //    _chaSprHost.ShowDp(flag);
            //}
            //else if (flag && !_isLoad)
            //{
            //    _isLoad = true;
            //    Variant conf = (this.g_mgr.g_gameConfM as muCLientConfig).localFWGame.GetChaBarConf("dp");
            //    _dpConf = conf;
            //    _chaSprHost.InitDpSpr(conf, initDpFin);
            //}
        }
        //public function DpIsShow():Boolean
        //{
        //    return _chaSprHost.DpIsShow();
        //}
        protected void initDpFin()
        {
            //_isInitDp = true;
            //_isLoad = false;
            //if (_dpConf != null && _dpConf.ContainsKey("posx"))
            //{
            //    _chaSprHost.dpPosX = _dpConf["posx"]._float;
            //}
            //_chaSprHost.ShowDp(true);
            //if (_dpShowInfo != null)
            //{
            //    SetDpShowInfo(_dpShowInfo);
            //    _dpShowInfo = null;
            //}
        }
        private Variant _dpShowInfo;
        public void SetDpShowInfo(Variant info)
        {
            //jason屏蔽血条显示            
            return;

            ShowChaDp(true);
            //if (!_isInitDp)
            //{
            //    _dpShowInfo = info;
            //}
            //else if (_chaSprHost != null)
            //{
            //    _chaSprHost.UpDataDpBar(info, _dpConf);
            //}
        }
        //protected function _getChaHpConf():Object
        //{
        //    return null;
        //}

        //protected var _isShowHp:Boolean = false;
        //protected var _charHpConf:Object = null;
        //protected var _hasInitHpSpr:Boolean = false;
        //protected var _isInitHpSpr:Boolean = false;
        //public function showChaHp(b:Boolean):void
        //{
        //    if(_isShowHp == b)
        //    {
        //        return ;
        //    }

        //    _isShowHp = b;
        //    _charHpConf = _getChaHpConf();

        //    if(!_hasInitHpSpr && _charHpConf != null && !_isInitHpSpr)
        //    {
        //        _isInitHpSpr= true;
        //        _chaSprHost.InitHpSpr(_charHpConf,showChaHpCB);
        //    }

        //    onShowChaHp();
        //    if(_currShowCha)
        //    {
        //        adjustTitleHeight();
        //    }
        //}

        //protected function onShowChaHp():void
        //{
        //    if(!_hasInitHpSpr)
        //    {
        //        return;
        //    }

        //    _chaSprHost.showHp(_isShowHp);
        //}

        //protected function showChaHpCB():void
        //{
        //    _hasInitHpSpr = true;
        //    onShowChaHp();
        //}

        //public function setChaTitleHp(hp:int,maxhp:int,isPlayAni:Boolean):void
        //{
        //    if(!isPlayAni)
        //    {
        //        _chaSprHost.setHp(hp,maxhp);
        //    }
        //    else
        //    {
        //        _chaSprHost.playHpAni(hp,maxhp);
        //    }
        //}
        //------------------------------------头顶血条End--------------------------------------------

        protected void changeGroundSprPos()
        {
            ////jason暂时屏蔽头顶血条
            //return;

            //if (_chaSprHost == null || _chaSprHost.displyObj == null)
            //    return;

            //Vec3 v = _chaSprHost.displyObj.changePot(m_char.pos + new Vec3(0, 1.8f, 0));
            //_chaSprHost.x = v.x;
            //_chaSprHost.y = CrossApp.singleton.height - v.y + 70;

        }
        //public void addDynamicSprite(Variant info)
        //{
        //    //Variant dynConf = getChaDynamicSprConf(info["tp"]);
        //    //if (dynConf != null && _chaSprHost != null)
        //    //{
        //    //    _chaSprHost.addDynamicSpr(dynConf, info["showtp"], info["showInfo"]);
        //    //}
        //}
        //public void addTitleSprite(Variant info)
        //{
        //    //Variant titleConf = getChaTitleSprConf(info["tp"]);
        //    //if (titleConf != null && _chaSprHost != null)
        //    //{
        //    //    _chaSprHost.addTitleSpr(info["tp"], titleConf, info["showtp"], info["showInfo"]);
        //    //}
        //}
        //public void removeTitleSprite(string tp, int subtp = 0)
        //{
        //    if (_chaSprHost != null)
        //    {
        //        _chaSprHost.removeTitleSpr(tp, subtp);
        //    }
        //}

        //protected Variant getChaTitleSprConf(string tp)
        //{
        //    return (this.g_mgr.g_gameConfM as muCLientConfig).localFWGame.GetChaTitleConf(tp);
        //}
        //protected Variant getChaDynamicSprConf(string tp)
        //{
        //    return (this.g_mgr.g_gameConfM as muCLientConfig).localFWGame.GetChaDynamicConf(tp);
        //}

    }

    class _graphChaSprite
    {
        protected bool _disposed = false;
        public Variant userdata;

        public _graphChaSprite()
        {

        }

        virtual public void initShowInfo(Variant data, Action<_graphChaSprite> cb)
        {

        }

        virtual public void update(float tmSlice)
        {

        }
        virtual public float width
        {
            get
            {
                return 0;
            }
        }

        virtual public float height
        {
            get
            {
                return 0;
            }
        }

        virtual public IUIBaseControl dispObj
        {
            get
            {
                return null;
            }
        }
        virtual public void dispose()
        {
            _disposed = true;
        }
    }

    class _graphChatSprite : _graphChaSprite
    {
        public _graphChatSprite()
            : base()
        {

        }

        public override void initShowInfo(Variant data, Action<_graphChaSprite> cb)
        {
            base.initShowInfo(data, cb);
        }

        public override void update(float tmSlice)
        {
            base.update(tmSlice);
        }

        public override void dispose()
        {
            base.dispose();
        }
    }

    class _graphChaTxtSprite : _graphChaSprite
    {
        protected IUIText _textBitmap;
        protected Variant _normalFmt = null;
        public _graphChaTxtSprite()
            : base()
        {
            //_textBitmap = UIManager.singleton.createControl("text", "text_chaTitle") as IUIText;
        }
        public override void initShowInfo(Variant data, Action<_graphChaSprite> cb)
        {
            _normalFmt = data["fmt"];
            if (_normalFmt != null)
            {
                _textBitmap.text = data["text"];
                if (_normalFmt.ContainsKey("color"))
                {
                    _textBitmap.color = _normalFmt["color"];
                }
                if (_normalFmt.ContainsKey("size"))
                {
                    _textBitmap.fontSize = _normalFmt["size"]._int;
                }
            }

            Style2D style = data["style"]._val as Style2D;
            if (style != null)
            {
                _textBitmap.align = style;
            }


            _textBitmap.visible = data.ContainsKey("visible") ? data["visible"]._bool : true;

            if (cb != null)
            {
                cb(this);
            }

        }

        public override void update(float tmSlice)
        {
            base.update(tmSlice);
        }

        public override IUIBaseControl dispObj
        {
            get
            {
                return _textBitmap;
            }
        }

        public override float width
        {
            get
            {
                if (_textBitmap != null)
                {
                    return _textBitmap.width;
                }
                return base.width;
            }
        }
        public override float height
        {
            get
            {
                if (_textBitmap != null)
                {
                    return _textBitmap.height;
                }
                return base.height;
            }
        }

        public override void dispose()
        {
            base.dispose();
            if (_textBitmap != null)
            {
                _textBitmap.dispose();
                _textBitmap = null;
            }
        }

        public void setText(String str)
        {
            _textBitmap.text = str;
        }
    }

    class _graphChaBmpSprite : _graphChaSprite
    {
        private IUIImageBox bmp;
        public _graphChaBmpSprite()
            : base()
        {
            //bmp = UIManager.singleton.createControl("image", "chaBmp_imageBox") as IUIImageBox;
        }

        public override void initShowInfo(Variant data, Action<_graphChaSprite> cb)
        {
            if (data.ContainsKey("width"))
            {
                bmp.width = data["width"]._float;
            }
            if (data.ContainsKey("height"))
            {
                bmp.height = data["height"]._float;
            }
            if (data.ContainsKey("file"))
            {
                bmp.file = data["file"];
            }
            else if (data.ContainsKey("res"))
            {
                bmp.file = data["res"];
            }
            bmp.createUIObject();

            if (cb != null)
            {
                cb(this);
            }
        }

        public override IUIBaseControl dispObj
        {
            get
            {
                return bmp;
            }
        }

        public override void dispose()
        {
            base.dispose();
            if (bmp != null)
            {
                bmp.dispose();
                bmp = null;
            }
        }
    }

    class _graphChaAniSprite : _graphChaSprite
    {
        private IUIImageBox aniBox;
        private int _spaceTm = 0;
        private int _playTm = 0;

        public _graphChaAniSprite()
            : base()
        {
            //aniBox = UIManager.singleton.createControl("image", "chaAni_imagebox") as IUIImageBox;
        }

        public override void initShowInfo(Variant data, Action<_graphChaSprite> cb)
        {
            if (data != null)
            {
                if (data.ContainsKey("sptm") && data["sptm"]._int > 0)
                {
                    _spaceTm = data["sptm"]._int;
                }
                else
                {
                    _spaceTm = 0;
                }
                if (data.ContainsKey("playtm") && data["playtm"]._int > 0)
                {
                    _playTm = data["playtm"]._int;
                }
                else
                {
                    _playTm = 0;
                }
                if (data.ContainsKey("width"))
                {
                    aniBox.width = data["width"]._float;
                }
                if (data.ContainsKey("height"))
                {
                    aniBox.height = data["height"]._float;
                }
            }
            if (aniBox.file != data["res"]._str)
            {
                aniBox.loadImgFun(
                    (IUIImageBox imgbox) =>
                    {
                        if (aniBox.imageBmp != null)
                        {
                            aniBox.imageBmp.loop = true;
                            //aniBox.imageBmp.play();
                        }
                    });
                aniBox.file = data["res"]._str;

            }
            else
            {
                if (aniBox.imageBmp != null)
                {
                    aniBox.imageBmp.loop = true;
                    aniBox.imageBmp.frame = 0;
                    //aniBox.imageBmp.Play();
                }
            }


            if (cb != null)
            {
                cb(this);
            }
        }

        public override float width
        {
            get
            {
                if (aniBox != null)
                {
                    return aniBox.width;
                }
                return base.width;
            }
        }
        public override float height
        {
            get
            {
                if (aniBox != null)
                {
                    return aniBox.height;
                }
                return base.height;
            }
        }

        public override IUIBaseControl dispObj
        {
            get
            {
                return aniBox;
            }
        }

        public override void update(float tmSlice)
        {
            base.update(tmSlice);
        }

        public override void dispose()
        {
            base.dispose();
            if (aniBox != null)
            {
                aniBox.dispose();
                aniBox = null;
            }
        }
    }

    class _graphChaImgNumSprite : _graphChaSprite
    {
        public _graphChaImgNumSprite()
            : base()
        {

        }

        public override void initShowInfo(Variant data, Action<_graphChaSprite> cb)
        {
            base.initShowInfo(data, cb);
        }

        public override void dispose()
        {
            base.dispose();
        }
    }
    class _graphChaProgressSprite : _graphChaSprite
    {
        private IUIProgressBar progressBar;
        public _graphChaProgressSprite()
            : base()
        {
            //progressBar = UIManager.singleton.createControl("progress", "chaDp_progressBar") as IUIProgressBar;
        }

        public override void initShowInfo(Variant data, Action<_graphChaSprite> cb)
        {
            if (data.ContainsKey("width"))
            {
                progressBar.width = data["width"]._float;
            }
            if (data.ContainsKey("height"))
            {
                progressBar.height = data["height"]._float;
            }

            if (data.ContainsKey("res"))
            {
                progressBar.file = data["res"];
                progressBar.createUIObject();
            }

            if (cb != null)
            {
                cb(this);
            }
        }

        public float maxNum
        {
            get
            {
                return progressBar.maxNum;
            }
            set
            {
                progressBar.maxNum = value;
            }
        }

        public float num
        {
            get
            {
                return progressBar.value;
            }
            set
            {
                progressBar.value = value;
            }
        }

        public override IUIBaseControl dispObj
        {
            get
            {
                return progressBar;
            }
        }

        public override void dispose()
        {
            base.dispose();
            if (progressBar != null)
            {
                progressBar.dispose();
                progressBar = null;
            }
        }
    }

    class _graphChaSpriteHost
    {
        protected const int ORI_CERTER = 0;
        protected const int ORI_LEFT = 1;
        protected const int ORI_RIGHT = 2;
        protected const int ORI_TOP = 3;

        protected IUIContainer _groundSpr; // 脚底的图片、文字
        protected IUIContainer _titleSpr; // 头顶的图片、文字
        protected IUIContainer _dynamicSpr;// 动态改变位置的图片、文字
        protected IUIContainer _hpSpr;//血条
        protected IUIContainer _dpSpr;//防护值条
        protected float _groundHeight = 0;

        protected Variant _titleSprites;
        protected Variant _titleOriData;
        protected List<_graphChaAniSprite> _aniSprites;
        protected List<_graphChatSprite> _chatSprites;
        protected List<Variant> _dynamicSprites;

        protected Boolean _disposed = false;
        public _graphChaSpriteHost()
        {
            _aniSprites = new List<_graphChaAniSprite>();
            _chatSprites = new List<_graphChatSprite>();
            _dynamicSprites = new List<Variant>();

            _titleSprites = new Variant();
            _titleOriData = new Variant();

            //_groundSpr = UIManager.singleton.createControl("container", "container_chaGround") as IUIContainer;
            //_titleSpr = UIManager.singleton.createControl("container", "container_chaTitle") as IUIContainer;
            //_dynamicSpr = UIManager.singleton.createControl("container", "container_chaDynamic") as IUIContainer;
            //_hpSpr = UIManager.singleton.createControl("container", "container_chaHp") as IUIContainer;
            //_dpSpr = UIManager.singleton.createControl("container", "container_chaDp") as IUIContainer;

            _groundSpr.addChild(_titleSpr);
            _groundSpr.addChild(_dynamicSpr);
            _groundSpr.addChild(_hpSpr);
            _groundSpr.addChild(_dpSpr);
            _hpSpr.visible = false;
            _dpSpr.visible = false;

            //UIManager.singleton.afterUIRoot.addChild(_groundSpr);

        }

        public IUIContainer displyObj
        {
            get
            {
                return _groundSpr;
            }
        }

        static protected Variant _displayStyle = new Variant();

        static public Variant GetDisplayStyle(string id)
        {
            Variant ret = _displayStyle[id];
            if (ret != null) return ret;

            //Style2D st = null;
            //Variant conf = UIManager.singleton.getStyleConfig(id);
            //if (conf != null)
            //{
            //    ret = new Variant();
            //    st = new Style2D();
            //    st.applyStyleConf(conf);
            //    ret._val = st;
            //}
            //_displayStyle[id] = ret;
            return ret;
        }

        static public _graphChaSprite createChaSprite(string tp)
        {
            switch (tp)
            {
                case "txt":
                    return new _graphChaTxtSprite();
                case "img":
                    return new _graphChaBmpSprite();
                case "ani":
                    return new _graphChaAniSprite();
                case "numimg":
                    return new _graphChaImgNumSprite();
                case "chat":
                    return new _graphChatSprite();
                case "progress":
                    return new _graphChaProgressSprite();
            }
            return null;
        }

        public float titleHeight
        {
            get
            {
                return _titleSpr.y;
            }
            set
            {
                _titleSpr.y = -value;
            }
        }

        public float groundHeight
        {
            get
            {
                return _groundHeight;
            }
            set
            {
                _groundHeight = value;
            }
        }

        public float dynamicHeight
        {
            get
            {
                return _dynamicSpr.y;
            }
            set
            {
                _dynamicSpr.y = -value;
            }
        }

        public float hpHeight
        {
            get
            {
                return -_hpSpr.y;
            }
            set
            {
                _hpSpr.y = -value;
            }
        }
        public float dpHeight
        {
            get
            {
                return -_dpSpr.y;
            }
            set
            {
                _dpSpr.y = -value;
            }
        }

        public float x
        {
            set
            {
                _groundSpr.x = value;
            }
        }
        public float y
        {
            set
            {
                _groundSpr.y = value + _groundHeight;
            }
        }

        public void addTitleSpr(string tp, Variant titleConf, int showtp = 0, Variant showInfo = null)
        {
            Variant titlesInfo = _titleSprites[tp];
            if (titlesInfo == null)
            {
                if (titleConf == null) return;
                titlesInfo = new Variant();
                titlesInfo["conf"] = titleConf;
                titlesInfo["sprs"] = new Variant();
                titlesInfo["sprs"]._val = new Dictionary<string, Variant>();
                _titleSprites[tp] = titlesInfo;
            }

            Variant showConf = titlesInfo["conf"]["show"][showtp.ToString()];

            //jason屏蔽头顶名字显示
            if (showConf != null && false)
            {
                _graphChaSprite spr = createChaSprite(showConf["tp"]);
                if (showInfo == null) showInfo = new Variant();

                _adjustShowInfo(showConf, showInfo);

                if (titlesInfo["sprs"].ContainsKey(showtp.ToString()))
                {
                    removeTitleSpr(tp, showtp);
                }
                spr.userdata = GameTools.createGroup("tp", tp, "showtp", showtp, "conf", titlesInfo["conf"]);
                titlesInfo["sprs"][showtp.ToString()] = GameTools.createGroup("spr", spr, "add", false);//add 是否已经添加到显示队列中

                spr.initShowInfo(showInfo, initTitleFinish);
            }
        }

        protected void initTitleFinish(_graphChaSprite spr)
        {
            if (_disposed)
            {
                spr.dispose();
                return;
            }

            Variant titlesInfo = _titleSprites[spr.userdata["tp"]];
            if (titlesInfo == null)
            {
                spr.dispose();
                return;
            }

            Dictionary<string, Variant> sprInfo = titlesInfo["sprs"][spr.userdata["showtp"]._str]._val as Dictionary<string, Variant>;
            if (sprInfo == null || sprInfo["spr"]._val != spr)
            {
                spr.dispose();
                return;
            }

            if ((sprInfo["spr"]._val as _graphChaSprite).dispObj == null)
            {
                titlesInfo["sprs"][spr.userdata["showtp"]._str] = null;
                return;
            }

            sprInfo["add"] = true;
            _titleSpr.addChild(spr.dispObj);
            _insertOriSprite(spr, spr.userdata["conf"]["ori"], this._titleOriData);

            if (spr is _graphChaAniSprite)
            {
                _aniSprites.Add(spr as _graphChaAniSprite);
            }

            if (spr is _graphChatSprite)
            {
                _chatSprites.Add(spr as _graphChatSprite);
            }
        }

        public void removeTitleSpr(String tp, int showtp = 0, Boolean updatePos = true)
        {
            int idx;
            Variant titlesInfo = _titleSprites[tp];
            if (titlesInfo != null)
            {
                Variant sprInfo = titlesInfo["sprs"][showtp.ToString()];
                if (sprInfo != null)
                {
                    titlesInfo["sprs"][showtp.ToString()] = null;
                    if (sprInfo["add"]._bool)
                    {
                        _titleSpr.removeChild((sprInfo["spr"]._val as _graphChaSprite).dispObj);
                        if (sprInfo["spr"]._val is _graphChaAniSprite)
                        {
                            idx = _aniSprites.IndexOf(sprInfo["spr"]._val as _graphChaAniSprite);
                            if (idx >= 0)
                            {
                                _aniSprites.RemoveAt(idx);
                            }
                        }

                        if (sprInfo["spr"]._val is _graphChatSprite)
                        {
                            idx = _chatSprites.IndexOf(sprInfo["spr"]._val as _graphChatSprite);
                            if (idx >= 0)
                            {
                                _chatSprites.RemoveAt(idx);
                            }
                        }

                        int ori = titlesInfo["conf"]["ori"];
                        Variant oriInfo = _titleOriData[ori];

                        idx = (oriInfo["sprs"]._val as List<_graphChaSprite>).IndexOf(sprInfo["spr"]._val as _graphChaSprite);
                        (oriInfo["sprs"]._val as List<_graphChaSprite>).RemoveAt(idx);
                        if (updatePos)
                        {
                            _updateOriSprite(oriInfo, _titleOriData);
                        }
                    }
                    (sprInfo["spr"]._val as _graphChaSprite).dispose();
                }
            }
        }

        protected void _adjustShowInfo(Variant showConf, Variant showInfo)
        {
            if (showConf.ContainsKey("sptm")) showInfo["sptm"] = showConf["sptm"];
            if (showConf.ContainsKey("res")) showInfo["res"] = showConf["res"];
            if (showConf.ContainsKey("imageNum")) showInfo["imageNum"] = showConf["imageNum"];
            if (showConf.ContainsKey("style"))
            {
                showInfo["style"] = GetDisplayStyle(showConf["style"]);
            }

            if (showInfo.ContainsKey("text"))
            {
                Variant fmt = new Variant();
                if (showConf.ContainsKey("fmt"))
                {
                    fmt = GameTools.mergeSimpleObject(showConf["fmt"][0], fmt);
                }
                if (showInfo.ContainsKey("fmt"))
                {
                    fmt = GameTools.mergeSimpleObject(showInfo["fmt"][0], fmt);
                }
                showInfo["fmt"] = fmt;
            }
            //chat添加
            if (showConf.ContainsKey("g9")) showInfo["g9"] = showConf["g9"][0];
            if (showConf.ContainsKey("imgprop")) showInfo["imgprop"] = showConf["imgprop"][0];
            if (showConf.ContainsKey("textprop")) showInfo["textprop"] = showConf["textprop"][0];
            if (showConf.ContainsKey("tm")) showInfo["tm"] = showConf["tm"];
            if (showConf.ContainsKey("uv")) showInfo["uv"] = showConf["uv"][0];
            if (showConf.ContainsKey("playtm")) showInfo["playtm"] = showConf["playtm"];
            if (showConf.ContainsKey("width")) showInfo["width"] = showConf["width"];
            if (showConf.ContainsKey("height")) showInfo["height"] = showConf["height"];

        }

        protected Variant _updateSpritesPos(List<_graphChaSprite> sprs, uint ori, int stx, int sty)
        {
            float offx = 0;
            float offy = 0;
            float tmp;
            float sp = 0;
            switch (ori)
            {
                case ORI_CERTER:
                    foreach (_graphChaSprite spr in sprs)
                    {
                        tmp = spr.width / 2;
                        spr.dispObj.x = -tmp;
                        if (tmp > offx && spr.userdata["conf"]["applyoff"] != null)
                        {
                            offx = tmp;
                        }

                        tmp = spr.height;
                        spr.dispObj.y = -tmp;
                        if (tmp > offy && spr.userdata["conf"]["applyoff"] != null)
                        {
                            offy = tmp;
                        }
                    }
                    break;
                case ORI_LEFT://left			
                    offx = stx;
                    foreach (_graphChaSprite spr in sprs)
                    {
                        sp = 0;
                        if (spr.userdata["conf"].ContainsKey("sp")) sp = spr.userdata["conf"]["sp"]._float;
                        tmp = offx + spr.width + sp;
                        spr.dispObj.x = -tmp;
                        spr.dispObj.y = -spr.height;
                        if (spr.userdata["conf"]["applyoff"] != null)
                        {
                            offx = tmp;
                        }
                    }
                    break;
                case ORI_RIGHT://right
                    offx = stx;
                    foreach (_graphChaSprite spr in sprs)
                    {
                        sp = 0;
                        if (spr.userdata["conf"].ContainsKey("sp")) sp = spr.userdata["conf"]["sp"]._float;
                        tmp = offx + sp;
                        spr.dispObj.x = -tmp;
                        spr.dispObj.y = -spr.height;
                        if (spr.userdata["conf"]["applyoff"] != null)
                        {
                            offx = tmp + spr.width;
                        }
                    }
                    break;
                case ORI_TOP://top				
                    offy = sty;
                    foreach (_graphChaSprite spr in sprs)
                    {
                        sp = 0;
                        if (spr.userdata["conf"].ContainsKey("sp")) sp = spr.userdata["conf"]["sp"]._float;
                        tmp = offy + spr.height + sp;
                        spr.dispObj.y = -tmp;
                        spr.dispObj.x = -spr.width / 2;
                        if (spr.userdata["conf"]["applyoff"] != null)
                        {
                            offy = tmp;
                        }
                    }
                    break;
            }
            return GameTools.createGroup("offx", offx, "offy", offy);
        }

        protected void _updateOriSprite(Variant oriData, Variant oriDatas)
        {
            Variant otherOri;
            Variant nerOff_c;
            Variant newOff;
            if (oriData["ori"]._int == ORI_CERTER)
            {
                nerOff_c = _updateSpritesPos(oriData["sprs"]._val as List<_graphChaSprite>, ORI_CERTER, 0, 0);

                Variant otherOriData = oriDatas["other"];
                if (oriData["offx"]._float < nerOff_c["offx"]._float)
                {
                    oriData["offx"] = nerOff_c["offx"];

                    otherOri = oriDatas[ORI_LEFT];
                    if (otherOri != null)
                    {
                        newOff = _updateSpritesPos(otherOri["sprs"]._val as List<_graphChaSprite>, ORI_LEFT, oriData["offx"], 0);
                        otherOri["offx"] = newOff["offx"];
                        otherOri["offy"] = newOff["offy"];
                    }
                    otherOri = oriDatas[ORI_RIGHT];
                    if (otherOri != null)
                    {
                        newOff = _updateSpritesPos(otherOri["sprs"]._val as List<_graphChaSprite>, ORI_RIGHT, oriData["offx"], 0);
                        otherOri["offx"] = newOff["offx"];
                        otherOri["offy"] = newOff["offy"];
                    }
                }
                if (oriData["offy"] < nerOff_c["offy"])
                {
                    oriData["offy"] = nerOff_c["offy"];
                    if (otherOriData != null)
                    {
                        otherOri = otherOriData[ORI_TOP];
                        if (otherOri != null)
                        {
                            newOff = _updateSpritesPos(otherOri["sprs"]._val as List<_graphChaSprite>, ORI_TOP, 0, oriData["offy"]);
                            otherOri["offx"] = newOff["offx"];
                            otherOri["offy"] = newOff["offy"];
                        }
                    }
                    otherOri = oriDatas[ORI_TOP];
                    if (otherOri != null)
                    {
                        newOff = _updateSpritesPos(otherOri["sprs"]._val as List<_graphChaSprite>, ORI_TOP, 0, oriData["offy"]);
                        otherOri["offx"] = newOff["offx"];
                        otherOri["offy"] = newOff["offy"];
                    }
                }
            }
            else
            {
                Variant certerOri = oriDatas[ORI_CERTER];
                if (certerOri != null)
                {
                    newOff = _updateSpritesPos(oriData["sprs"]._val as List<_graphChaSprite>, oriData["ori"], certerOri["offx"], certerOri["offy"]);
                }
                else
                {
                    newOff = _updateSpritesPos(oriData["sprs"]._val as List<_graphChaSprite>, oriData["ori"], 0, 19);
                }
                oriData["offx"] = newOff["offx"];
                oriData["offy"] = newOff["offy"];
            }
        }

        protected void _insertOriSprite(_graphChaSprite newspr, int ori, Variant oriDatas)
        {
            Variant oriData = oriDatas[ori];
            if (oriData == null)
            {
                oriData = GameTools.createGroup("sprs", new List<_graphChaSprite>() { newspr }, "offx", 0, "offy", 0, "ori", ori);
                _titleOriData[ori] = oriData;
            }
            else
            {
                if (ori == ORI_CERTER)
                {
                    (oriData["sprs"]._val as List<_graphChaSprite>).Add(newspr);
                }
                else
                {
                    int i = (oriData["sprs"]._val as List<_graphChaSprite>).Count;
                    for (; i > 0; --i)
                    {
                        if (_compareSprite(((oriData["sprs"]._val as List<_graphChaSprite>)[i - 1]).userdata, newspr.userdata) <= 0)
                        {
                            break;
                        }
                    }
                    if (i >= (oriData["sprs"]._val as List<_graphChaSprite>).Count)
                    {
                        (oriData["sprs"]._val as List<_graphChaSprite>).Add(newspr);
                    }
                    else
                    {
                        (oriData["sprs"]._val as List<_graphChaSprite>).Insert(i, newspr);
                    }
                }
            }

            _updateOriSprite(oriData, oriDatas);
        }

        protected int _compareSprite(Variant first, Variant second)
        {
            Variant fconf = first["conf"];
            Variant sconf = second["conf"];
            if (fconf["idx"]._int < sconf["idx"]._int)
            {
                return -1;
            }
            else if (fconf["idx"]._int > sconf["idx"]._int)
            {
                return 1;
            }
            else if ((fconf.ContainsKey("sort")) && fconf == sconf)
            {
                Variant sortConf = fconf["sort"];
                int fidx = sortConf._arr.IndexOf(first["showtp"]);
                int sidx = sortConf._arr.IndexOf(second["showtp"]);
                if (fidx < sidx)
                {
                    return -1;
                }
                else if (fidx > sidx)
                {
                    return 1;
                }
            }
            return 0;
        }

        //------------------------------- dynamic sprite start----------------------
        protected List<Variant> _showDynArr = new List<Variant>();

        protected const int DAT_DEFAULT = 0;
        protected List<Action<float, Variant>> _dynamicAniFun;
        protected void _initDynamicAniFuns()
        {
            _dynamicAniFun = new List<Action<float, Variant>>();
            _dynamicAniFun.Add(_dynamicDefaultAni); //DAT_DEFAULT
            _dynamicAniFun.Add(_dynamicAniTp1);
            _dynamicAniFun.Add(_dynamicAniTp2);
            _dynamicAniFun.Add(_dynamicAniTp3);
        }

        //public void addDynamicSpr(Variant conf, uint showtp, Variant showInfo)
        //{
        //    Variant showConf = conf["show"][showtp.ToString()];
        //    if (showConf)
        //    {
        //        if (_dynamicAniFun == null)
        //        {
        //            _initDynamicAniFuns();
        //        }

        //        uint anitp = showInfo["anitp"]._uint;
        //        if (anitp >= _dynamicAniFun.Count)
        //        {
        //            return;
        //        }

        //        _graphChaSprite spr = createChaSprite(showConf["tp"]._str);
        //        if (showInfo == null) showInfo = new Variant();

        //        _adjustShowInfo(showConf, showInfo);

        //        spr.userdata = GameTools.createGroup("tp", conf["tp"]._str, "loaded", false);
        //        spr.initShowInfo(showInfo, initDynamicFinish);


        //        Variant aniconf = conf["ani"][anitp.ToString()];
        //        Variant info = GameTools.createGroup("spr", spr);
        //        info["aniConf"] = aniconf;
        //        info["showInfo"] = showInfo;
        //        info["stm"] = GameTools.getTimer() + aniconf["dtm"]._float;
        //        info["etm"] = info["stm"]._float + aniconf["ttm"]._float;
        //        _dynamicSprites.Add(info);
        //    }
        //}

        protected void initDynamicFinish(_graphChaSprite spr)
        {
            if (_disposed)
            {
                spr.dispose();
                return;
            }

            spr.userdata["loaded"] = true;
        }

        protected void createDynamicAni(Variant info)
        {
            Variant showInfo = info["showInfo"];

            IUIBaseControl dip = (info["spr"]._val as _graphChaSprite).dispObj;
            Variant aniInfo = GameTools.createGroup("stm", info["stm"]._float, "etm", info["etm"]._float, "stx", 0, "sty", 0, "dir", showInfo["dir"]._int);
            aniInfo["spr"] = info["spr"];
            aniInfo["conf"] = info["aniConf"];
            // to do init pos
            switch (info["aniConf"]["tp"]._int)
            {
                case DAT_DEFAULT:
                    aniInfo["stx"] = -dip.width / 2;
                    aniInfo["sty"] = 30;
                    aniInfo["criatk"] = showInfo["criatk"]._bool;
                    break;
                case 1:
                    aniInfo["stx"] = -dip.width / 2;
                    aniInfo["sty"] = 30;
                    break;
                case 2:
                    aniInfo["stx"] = -dip.width / 2;
                    aniInfo["sty"] = 30;
                    break;
                case 3:
                    aniInfo["stx"] = -dip.width / 2;
                    aniInfo["sty"] = 30;
                    aniInfo["criatk"] = showInfo["criatk"]._bool;
                    break;
            }

            dip.x = aniInfo["stx"]._float;
            dip.y = aniInfo["sty"]._float;
            _dynamicSpr.addChild(dip);

            _showDynArr.Add(aniInfo);
        }

        protected void handleDynamicSprites()
        {
            float currTm = GameTools.getTimer();
            for (int i = _dynamicSprites.Count - 1; i >= 0; --i)
            {
                Variant info = _dynamicSprites[i];
                if (info["stm"]._float <= currTm)
                {
                    if (info["etm"]._float <= currTm)
                    {
                        (info["spr"]._val as _graphChaSprite).dispose();
                        _dynamicSprites.RemoveAt(i);
                        continue;
                    }

                    if (!(info["spr"]._val as _graphChaSprite).userdata["loaded"]._bool)
                        continue;

                    _dynamicSprites.RemoveAt(i);

                    createDynamicAni(info);
                }
            }
        }

        protected void dynamicAniProcess()
        {
            float currTm = GameTools.getTimer();
            for (int i = _showDynArr.Count - 1; i >= 0; --i)
            {
                Variant aniInfo = _showDynArr[i];
                if (currTm > aniInfo["etm"]._float)
                {//结束
                    _dynamicSpr.removeChild((aniInfo["spr"]._val as _graphChaSprite).dispObj);
                    (aniInfo["spr"]._val as _graphChaSprite).dispose();
                    _showDynArr.RemoveAt(i);
                    continue;
                }

                _dynamicAniFun[aniInfo["conf"]["tp"]._int](currTm, aniInfo);
            }
        }


        //-------------------------头顶冒字动画Start--------------------
        //不需要修理 结束
        protected void _dynamicDefaultAni(float currTm, Variant aniInfo)
        {
            //		const c_aniTm:int = 1500;//动画总时间
            //		const c_scaleTm1:int = 100;//大小动画时间
            //		const c_scaleTm2:int = 300;//小动画时间
            //		const c_alphaTm:int = 300;//透明动画时间
            Variant aniConf = aniInfo["conf"];
            float lostTm = currTm - aniInfo["stm"];
            _graphChaSprite spr = aniInfo["spr"]._val as _graphChaSprite;

            float radio;
            if (aniInfo["criatk"]._bool)
            {
                if (lostTm < aniConf["scaletm_b"]._float)
                {
                    radio = lostTm / aniConf["scaletm_b"]._float + 1;
                    //spr.dispObj.scaleX = radio;
                    //spr.dispObj.scaleY = radio;
                    spr.dispObj.scale = radio;
                }
                else if (lostTm < aniConf["scaletm_s"]._float + aniConf["scaletm_b"]._float)
                {
                    radio = 2 - (lostTm - aniConf["scaletm_b"]._float) / aniConf["scaletm_s"]._float;
                    //spr.dispObj.scaleX = radio;
                    //spr.dispObj.scaleY = radio;
                    spr.dispObj.scale = radio;
                }
                else
                {
                    radio = 1;
                    spr.dispObj.scale = radio;
                    //spr.dispObj.scaleX = 1;
                    //spr.dispObj.scaleY = 1;				
                    aniInfo["criatk"] = false;
                }
                spr.dispObj.x = aniInfo["stx"]._float - (radio - 1) * spr.dispObj.width / 4;//txt.textWidth/2;
            }
            radio = lostTm / aniConf["ttm"]._float - 1;

            spr.dispObj.y = aniInfo["sty"]._float - 120 * (radio * radio * radio + 1);

            if (aniConf["ttm"]._float - lostTm < aniConf["alphatm"]._float)
            {
                spr.dispObj.alpha = (aniConf["ttm"]._float - lostTm) / aniConf["alphatm"]._float;
            }
        }

        protected void _dynamicAniTp1(float currTm, Variant aniInfo)
        {
            Variant aniConf = aniInfo["conf"];
            float lostTm = currTm - aniInfo["stm"]._float;
            _graphChaSprite spr = aniInfo["spr"]._val as _graphChaSprite;
            float distance = aniConf["distance"]._float;
            if (aniInfo["dri"]["x"]._float > 0)
            {
                distance = distance - spr.dispObj.width;
            }
            float t;
            float cx;
            float cy;
            float d;
            float bx;
            float by;
            float realtm = aniConf["ttm"]._float - aniConf["stop_ttm"]._float;
            if (lostTm < aniConf["stop_s"]._float)
            {
                t = lostTm;
                d = aniConf["stop_s"]._float;
                bx = aniInfo["stx"]._float;
                by = aniInfo["sty"]._float;
            }
            else if (lostTm > aniConf["stop_s"]._float + aniConf["stop_ttm"]._float)
            {
                t = lostTm - aniConf["stop_ttm"]._float - aniConf["stop_s"]._float;
                d = realtm - aniConf["stop_s"]._float;
                bx = distance * aniInfo["dri"]["x"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["stx"]._float;
                by = distance * aniInfo["dri"]["y"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["sty"]._float;
            }
            else
            {
                t = 0;
                d = 0;
                bx = distance * aniInfo["dri"]["x"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["stx"]._float;
                by = distance * aniInfo["dri"]["y"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["sty"]._float;
            }
            cx = distance * aniInfo["dri"]["x"]._float * (d / realtm);
            cy = distance * aniInfo["dri"]["y"]._float * (d / realtm);
            d += (float)GameTools.randomInst.NextDouble() * 50;
            Variant att = GameTools.createGroup("duration", d, "change", cx, "begin", bx);
            float x = (float)Algorithm.TweenExpoEaseOut(att, t);
            spr.dispObj.x = x;
            att = GameTools.createGroup("duration", d, "change", cy, "begin", by);
            spr.dispObj.y = (float)Algorithm.TweenExpoEaseOut(att, t);

            if (aniConf["ttm"]._float - lostTm < aniConf["alphatm"]._float)
            {
                spr.dispObj.alpha = (aniConf["ttm"]._float - lostTm) / aniConf["alphatm"]._float;
            }
        }

        //心跳效果动画
        protected void _dynamicAniTp2(float currTm, Variant aniInfo)
        {
            Variant aniConf = aniInfo["conf"];
            float lostTm = currTm - aniInfo["stm"]._float;
            _graphChaSprite spr = aniInfo["spr"]._val as _graphChaSprite;
            float distance = aniConf["distance"]._float;
            float t;
            float cx;
            float cy;
            float d;
            float bx;
            float by;
            float realtm = aniConf["ttm"]._float - aniConf["stop_ttm"]._float;
            if (lostTm < aniConf["stop_s"]._float)
            {
                t = lostTm;
                d = aniConf["stop_s"]._float;
                bx = aniInfo["stx"]._float;
                by = aniInfo["sty"]._float;
            }
            else if (lostTm > aniConf["stop_s"]._float + aniConf["stop_ttm"]._float)
            {
                t = lostTm - aniConf["stop_ttm"]._float - aniConf["stop_s"]._float;
                d = realtm - aniConf["stop_s"]._float;
                bx = distance * aniInfo["dri"]["x"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["stx"]._float;
                by = distance * aniInfo["dri"]["y"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["sty"]._float;
            }
            else
            {
                t = 0;
                d = 0;
                bx = distance * aniInfo["dri"]["x"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["stx"]._float;
                by = distance * aniInfo["dri"]["y"]._float * (aniConf["stop_s"]._float / realtm) + aniInfo["sty"]._float;
            }
            cx = distance * aniInfo["dri"]["x"]._float * (d / realtm);
            cy = distance * aniInfo["dri"]["y"]._float * (d / realtm);
            d += (float)GameTools.randomInst.NextDouble() * 50;
            Variant att = GameTools.createGroup("duration", d, "change", cx, "begin", bx);
            spr.dispObj.x = (float)Algorithm.TweenExpoEaseOut(att, t);
            GameTools.createGroup("duration", d, "change", cy, "begin", by);
            spr.dispObj.y = (float)Algorithm.TweenExpoEaseOut(att, t);


            if (aniConf["ttm"]._float - lostTm < aniConf["alphatm"]._float)
            {
                spr.dispObj.alpha = (aniConf["ttm"]._float - lostTm) / aniConf["alphatm"]._float;
            }
        }

        //自己掉血动画
        protected void _dynamicAniTp3(float currTm, Variant aniInfo)
        {
            //		const c_aniTm:int = 1500;//动画总时间
            //		const c_scaleTm1:int = 100;//大小动画时间
            //		const c_scaleTm2:int = 300;//小动画时间
            //		const c_alphaTm:int = 300;//透明动画时间
            Variant aniConf = aniInfo["conf"];
            float lostTm = currTm - aniInfo["stm"]._float;
            _graphChaSprite spr = aniInfo["spr"]._val as _graphChaSprite;

            float radio;
            if (aniInfo["criatk"]._bool)
            {
                if (lostTm < aniConf["scaletm_b"]._float)
                {
                    radio = lostTm / aniConf["scaletm_b"]._float + 1;
                    spr.dispObj.scale = radio;
                    //spr.dispObj.scaleX = radio;
                    //spr.dispObj.scaleY = radio;
                }
                else if (lostTm < aniConf["scaletm_s"]._float + aniConf["scaletm_b"]._float)
                {
                    radio = 2 - (lostTm - aniConf["scaletm_b"]._float) / aniConf["scaletm_s"]._float;
                    spr.dispObj.scale = radio;
                    //spr.dispObj.scaleX = radio;
                    //spr.dispObj.scaleY = radio;
                }
                else
                {
                    radio = 1;
                    spr.dispObj.scale = radio;
                    //spr.dispObj.scaleX = 1;
                    //spr.dispObj.scaleY = 1;				
                    aniInfo["criatk"] = false;
                }
                spr.dispObj.x = aniInfo["stx"]._float - (radio - 1) * spr.dispObj.width / 4;//txt.textWidth/2;
            }
            radio = lostTm / aniConf["ttm"]._float - 1;

            spr.dispObj.y = aniInfo["sty"]._float - 60 * (radio * radio * radio + 1);

            if (aniConf["ttm"]._float - lostTm < aniConf["alphatm"]._float)
            {
                spr.dispObj.alpha = (aniConf["ttm"]._float - lostTm) / aniConf["alphatm"]._float;
            }
        }
        //-------------------------头顶冒字动画End-----------------------


        //--------------------------------头顶血条 start------------------------------------------------------
        protected Action _initDpCallBack = null;
        protected Dictionary<string, Variant> _dpInfo = new Dictionary<string, Variant>();
        private int _dpNum = 0;

        public float dpPosX
        {
            set
            {
                _dpSpr.x = -value;
            }
        }
        public float dpPosY
        {
            set
            {
                _dpSpr.y = -value;
            }
        }

        public void ShowDp(Boolean flag)
        {
            _dpSpr.visible = flag;
        }
        public void InitDpSpr(Variant conf, Action initCB)
        {
            _initDpCallBack = initCB;
            _dpNum = conf["show"].Count;
            foreach (Variant obj in conf["show"].Values)
            {
                _graphChaSprite spr = createChaSprite(obj["tp"]);
                _dpInfo[obj["layer"]] = GameTools.createGroup("spr", spr);
                _dpInfo[obj["layer"]]["info"] = obj;
                if ("txt" == obj["tp"])
                {
                    Variant showObj = obj.clone();
                    if (obj.ContainsKey("style"))
                    {
                        showObj["style"] = GetDisplayStyle(obj["style"]);
                    }

                    Variant fmt = new Variant();
                    if (obj.ContainsKey("fmt"))
                    {
                        GameTools.mergeSimpleObject(obj["fmt"][0], fmt);
                    }
                    showObj["fmt"] = fmt;

                    spr.initShowInfo(showObj, initDpSprFin);
                }
                else
                {
                    spr.initShowInfo(obj, initDpSprFin);
                }
            }
        }

        protected void initDpSprFin(_graphChaSprite dpSpr)
        {
            --_dpNum;
            if (0 == _dpNum)
            {
                foreach (string key in _dpInfo.Keys)
                {
                    Variant info = _dpInfo[key];
                    Variant conf = info["info"];
                    _graphChaSprite spr = info["spr"]._val as _graphChaSprite;
                    if (conf.ContainsKey("width"))
                    {
                        spr.dispObj.width = conf["width"]._float;
                    }
                    if (conf.ContainsKey("height"))
                    {
                        spr.dispObj.height = conf["height"]._float;
                    }
                    if (conf.ContainsKey("offy") && conf["offy"]._float != 0)
                    {
                        spr.dispObj.y += conf["offy"]._float;
                    }
                    if (conf.ContainsKey("offx") && conf["offx"]._float != 0)
                    {
                        spr.dispObj.x += conf["offx"]._float;
                    }
                    _dpSpr.addChild(spr.dispObj);
                }
                if (null != _initDpCallBack)
                {
                    _initDpCallBack();
                }
            }
        }

        protected Variant getDpAni(int idx)
        {
            string aniId = "ani" + idx;
            foreach (Variant obj in _dpInfo.Values)
            {
                if (obj["info"]["id"]._str == aniId)
                {
                    return obj;
                }
            }
            return null;
        }
        protected Variant getDpBar(int idx)
        {
            string barId = "bar" + idx;
            foreach (Variant obj in _dpInfo.Values)
            {
                if (obj["info"]["id"]._str == barId)
                {
                    return obj;
                }
            }
            return null;
        }
        protected Variant _dpAniInfo;
        public void UpDataDpBar(Variant info, Variant conf)
        {
            Variant bar = getDpBar(0);
            if (bar != null)
            {
                _graphChaProgressSprite spr = bar["spr"]._val as _graphChaProgressSprite;
                spr.maxNum = info["max"];
                spr.num = info["cur"];

                //DebugTrace.print("maxNum: " + spr.maxNum);
                //DebugTrace.print("num: " + spr.num);
            }


            if (_dpAniInfo == null)
            {
                _dpAniInfo = new Variant();
            }
            _dpAniInfo["playing"] = true;
            _dpAniInfo["duration"] = conf["ani"]["ttm"]._float;
            _dpAniInfo["currTm"] = 0;
            _dpAniInfo["startVal"] = info["last"]._float / info["max"]._float;
            _dpAniInfo["curhp"] = info["cur"];
            _dpAniInfo["maxhp"] = info["max"];
        }

        protected void disposeDp()
        {
            if (_dpInfo.Count > 0 && _dpSpr != null)
            {
                foreach (string key in _dpInfo.Keys)
                {
                    Variant info = _dpInfo[key];
                    _dpSpr.removeChild((info["spr"]._val as _graphChaSprite).dispObj);
                    (info["spr"]._val as _graphChaSprite).dispose();
                }
            }

            _dpInfo = new Dictionary<string, Variant>();
            _dpSpr.visible = false;
            _dpSpr = null;
        }
        protected void dpAniProcess(float tm)
        {
            _dpAniInfo["currTm"] += tm * 1000;
            float tmp = _dpAniInfo["currTm"]._float / _dpAniInfo["duration"]._float;
            if (tmp >= 1)
            {
                tmp = 1;
                _dpAniInfo["playing"] = false;
                if (0 >= _dpAniInfo["curhp"]._float || _dpAniInfo["curhp"]._float == _dpAniInfo["maxhp"]._float)
                {
                    _dpSpr.visible = false;
                }
            }
        }

        //--------------------------------头顶血条 end------------------------------------------------------

        public void process(float timeSlice)
        {
            //if(_dynamicSprites.length > 0)
            //{
            //    handleDynamicSprites();
            //}

            if (_showDynArr.Count > 0)
            {
                dynamicAniProcess();
            }

            foreach (_graphChaSprite aniSpr in _aniSprites)
            {
                aniSpr.update(timeSlice);
            }

            foreach (_graphChaSprite chatSpr in _chatSprites)
            {
                chatSpr.update(timeSlice);
            }
            //-----------------------------------------------------------------------
            //_hpAniProcess(timeSlice);
            if (_dpAniInfo != null && _dpAniInfo["playing"]._bool)
            {
                dpAniProcess(timeSlice);
            }
        }

        public void dispose()
        {
            _disposed = true;

            _aniSprites = null;
            _chatSprites = null;

            if (_titleSpr != null)
            {
                _titleSpr.dispose();
                _titleSpr = null;
            }
            _dynamicSpr = null;

            if (_titleSprites != null && _titleSprites.Count > 0)
            {
                foreach (Variant tmpData in _titleSprites.Values)
                {
                    foreach (Variant spData in tmpData["sprs"].Values)
                    {
                        (spData["spr"]._val as _graphChaSprite).dispose();
                    }
                }
            }

            _titleSprites = null;
            _titleOriData = null;

            disposeDp();

            if (_groundSpr != null)
            {
                _groundSpr.dispose();
            }
            _groundSpr = null;
        }
    }
}