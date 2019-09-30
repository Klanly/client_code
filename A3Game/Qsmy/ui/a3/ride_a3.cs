using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MuGame
{
    class ride_a3 : Window
    {
        private RideInfo rideInfo;
        private RideModel rideModel;
        private RideWakan rideWakan;

        private TabControl tab = null;
        private int defaulSelect = -1;
        private BaseButton btn_Close = null;

        private ProfessionAvatar m_proAvatar;
        private GameObject scene_Obj, wingAvatar, avatarCamera, _iconDrag, _quanBg;

        private Dictionary<string, GameObject> _rideAvatarMapping;

        public static ride_a3 _instance = null;

        public bool isShowCompelet = false;

        private Camera camera = null;

        public GameObject scenceTa, scenceTzLg = null;

        public Dictionary<int, GameObject> fxMapping = null;

        public GameObject fxGo = null;

        public override void init()
        {
            base.init();

            FindGameobject();

            SetParentContainer();

            SetTabControl();

            OnBtnAddOnClick();

            this.transform.FindChild("infoContainer/help/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_ride_a3_1");
            this.transform.FindChild("modelContainer/help/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_ride_a3_2");
            this.transform.FindChild("wakanContainer/help/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_ride_a3_3");

            this.transform.FindChild("infoContainer/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_ride_a3_4");
            this.transform.FindChild("modelContainer/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_ride_a3_4");
            this.transform.FindChild("wakanContainer/help/close/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_ride_a3_4");
        }

        public override void onShowed()
        {
            isShowCompelet = true;

            SetState(false, InterfaceMgr.STATE_FUNCTIONBAR);

            CreatAvatar();

            if (_instance == null)
            {
                var id = A3_RideModel.getInstance().GetRideInfo().dress;

                CreatRideAvatar((int)id);

                _instance = this;

            }

            if (tab == null)
            {
                SetTabControl();
            }

            else {

                tab.setSelectedIndex(defaulSelect == -1 ? 0 : 0, true);
            }

            rideInfo.OnAddEventListener();
            rideModel.OnAddEventListener();
            rideWakan.OnAddEventListener();

            _rideAvatarMapping = new Dictionary<string, GameObject>();
            checkRideWakan();
            UiEventCenter.getInstance().onWinOpen(uiName);
            base.onShowed();
        }

        void checkRideWakan() {
            if (A3_RideModel.getInstance().GetRideInfo().lvl < 5) {
                this.transform.FindChild("BtnContainer/wakan").gameObject.SetActive(false);
            }
            else
                this.transform.FindChild("BtnContainer/wakan").gameObject.SetActive(true);
        }



        public override void onClosed()
        {

            SetState(true, InterfaceMgr.STATE_NORMAL);

            base.onClosed();

            OnDisPose();

        }

        private void SetState(bool isshow, int state) {

            GRMap.GAME_CAMERA.SetActive(isshow);
            InterfaceMgr.getInstance().changeState(state);

            var go = transform.FindChild("ig_bg_bg");

            if (go != null) go.gameObject.SetActive(false);

        }

        private void FindGameobject() {

            btn_Close = new BaseButton(this.getGameObjectByPath("close").transform);

            _quanBg = getGameObjectByPath("wakanContainer/bg5");

            _iconDrag = this.getGameObjectByPath("drag");

            this.getEventTrigerByPath("drag").onDrag = (go, delta) => {

                if (wingAvatar != null)
                {
                    wingAvatar.transform.Rotate(Vector3.up, -delta.x);
                }
            };

            if (fxMapping == null) fxMapping = new Dictionary<int, GameObject>();

            for (int i = 0; i < 5; i++)
            {
                fxMapping.Add(i, getGameObjectByPath("zhuanhua_FX/dian/ditu" + i));

            }

            fxGo = getGameObjectByPath("zhuanhua_FX/heji/dian");
        }

        private void OnBtnAddOnClick()
        {
            btn_Close.onClick = OnBtnCloseHandel;

            rideInfo.OnAddOnClick();

            rideModel.OnAddOnClick();

            rideWakan.OnAddOnClick();
        }

        private void SetTabControl() {

            tab = new TabControl();
            tab.onClickHanle = tabhandel;
            tab.create(getGameObjectByPath("BtnContainer"), this.gameObject, defaulSelect == -1 ? 0 : 0, 0, true);

        }

        private void tabhandel(TabControl t)
        {

            int currIndex = t.getSeletedIndex();

            if (defaulSelect != -1 && defaulSelect == currIndex)
            {
                return;
            }

            //if ( currIndex == 2 )
            //{
            //    flytxt.instance.fly( ContMgr.getCont( "a3_not_function" ) );

            //    return;
            //}

            defaulSelect = currIndex;

            SetCurrSelectContainer(defaulSelect);

            switch (defaulSelect)
            {
                case 0: rideInfo.OnParentTabHandle(); break;

                case 1: rideModel.OnParentTabHandle(); break;

                case 2: rideWakan.OnParentTabHandle(); break;

                default:

                    break;
            }
        }

        private void SetCurrSelectContainer(int index) {

            rideInfo.visiable = index == 0 ? true : false;
            rideModel.visiable = index == 1 ? true : false;
            rideWakan.visiable = index == 2 ? true : false;

            if (camera == null)
            {
                return;
            }

            if (index == 2) {

                scenceTa.layer = EnumLayer.LM_ROLE_INVISIBLE;

                scenceTzLg.layer = EnumLayer.LM_ROLE_INVISIBLE;

                camera.cullingMask = (1 << EnumLayer.LM_FX);

                _iconDrag.gameObject.SetActive(false);
            }
            else {

                scenceTa.layer = EnumLayer.LM_FX;

                scenceTzLg.layer = EnumLayer.LM_FX;

                camera.cullingMask = (1 << EnumLayer.LM_FX) | (1 << EnumLayer.LM_ROLE_INVISIBLE);

                _iconDrag.gameObject.SetActive(true);

            }

        }

        private void SetParentContainer() {

            rideInfo = new RideInfo(this.getTransformByPath("infoContainer"));
            rideModel = new RideModel(this.getTransformByPath("modelContainer"));
            rideWakan = new RideWakan(this.getTransformByPath("wakanContainer"));

        }

        private void OnBtnCloseHandel(GameObject go) {

            InterfaceMgr.getInstance().close(InterfaceMgr.RIDE_A3);
        }

        public static List<GameObject> BuildGo(GameObject originalGo, int count)
        {

            List<GameObject> goLst = new List<GameObject>();

            if (!originalGo || count == 0)
            {
                return goLst;
            }

            for (int i = 0; i < count; i++)
            {
                var go = GameObject.Instantiate(originalGo);
                go.transform.localScale = Vector3.one;
                goLst.Add(go);
            }

            return goLst;
        }

        private void CreatAvatar()
        {
            if (wingAvatar == null)
            {
                //string path = "";
                GameObject obj_prefab;
                A3_PROFESSION eprofession = A3_PROFESSION.None;
                if (SelfRole._inst is P2Warrior)
                {
                    eprofession = A3_PROFESSION.Warrior;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_warrior_avatar");
                    if (obj_prefab != null)
                        wingAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-77.06f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else if (SelfRole._inst is P3Mage)
                {
                    eprofession = A3_PROFESSION.Mage;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_avatar");
                    if (obj_prefab != null)
                        wingAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-77.06f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else if (SelfRole._inst is P5Assassin)
                {
                    eprofession = A3_PROFESSION.Assassin;
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_assa_avatar");
                    if (obj_prefab != null)
                        wingAvatar = GameObject.Instantiate(obj_prefab, new Vector3(-77.06f, -0.602f, 14.934f), new Quaternion(0, 90, 0, 0)) as GameObject;
                }
                else
                {
                    return;
                }

                Transform cur_model = wingAvatar.transform.FindChild("model");

                foreach (Transform tran in wingAvatar.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
                }
                //手上的小火球
                if (SelfRole._inst is P3Mage)
                {
                    Transform cur_r_finger1 = cur_model.FindChild("R_Finger1");
                    obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_mage_r_finger_fire");
                    GameObject light_fire = GameObject.Instantiate(obj_prefab) as GameObject;
                    light_fire.transform.SetParent(cur_r_finger1, false);
                }

                m_proAvatar = new ProfessionAvatar();
                int type = SelfRole._inst.get_bodyid() != 0 ? a3_BagModel.getInstance().getEquipTypeBytpId(SelfRole._inst.get_bodyid()) : 0;
                m_proAvatar.Init_PA(eprofession, SelfRole._inst.m_strAvatarPath, "h_", EnumLayer.LM_ROLE_INVISIBLE, EnumMaterial.EMT_EQUIP_H, cur_model, SelfRole._inst.m_strEquipEffPath);
                if (a3_EquipModel.getInstance().active_eqp.Count >= 10)
                {
                    m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEqpIdbyType(3), true);
                }
                m_proAvatar.set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(a3_EquipModel.getInstance().active_eqp.Count));
                m_proAvatar.set_body(SelfRole._inst.get_bodyid(), SelfRole._inst.get_bodyfxid());
                m_proAvatar.set_weaponl(SelfRole._inst.get_weaponl_id(), SelfRole._inst.get_weaponl_fxid());
                m_proAvatar.set_weaponr(SelfRole._inst.get_weaponr_id(), SelfRole._inst.get_weaponr_fxid());
                m_proAvatar.set_wing(SelfRole._inst.get_wingid(), SelfRole._inst.get_windfxid());
                m_proAvatar.set_equip_color(SelfRole._inst.get_equip_colorid());

                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_ride_show_scene");
                scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-76.98f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;
                foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
                {
                    if (tran.gameObject.name == "scene_ta") {
                        tran.gameObject.layer = EnumLayer.LM_FX;
                        scenceTa = tran.gameObject;
                    }
                    else if (tran.gameObject.name == "sc_tz_lg") {

                        tran.gameObject.layer = EnumLayer.LM_FX;
                        scenceTzLg = tran.gameObject;

                    }
                    else {

                        tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层

                    }
                }
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
                avatarCamera = GameObject.Instantiate(obj_prefab) as GameObject;
                camera = avatarCamera.GetComponentInChildren<Camera>();
                camera.transform.localPosition = new Vector3(-76.5f, 14.67f, -18f);
                camera.transform.localEulerAngles = new Vector3(22.436f, 0.8290001f, 360.225f);
                camera.orthographicSize = 2.27f;
                camera.cullingMask = (1 << EnumLayer.LM_FX);
            }
        }

        public void CreatRideAvatar(int id) {

            if (m_proAvatar != null)
            {
                m_proAvatar.Set_Ride(id, (f) => { rideModel.isLoading = false; }, true);

            }

        }

        private void DisposeAvatar() {

            var _lst = _rideAvatarMapping.ToArray();

            for (int i = 0; i < _lst.Length; i++)
            {
                GameObject.Destroy(_lst[i].Value);
                _rideAvatarMapping.Remove(_lst[i].Key);
            }

            m_proAvatar.dispose();

            if (wingAvatar != null) GameObject.Destroy(wingAvatar);
            if (avatarCamera != null) GameObject.Destroy(avatarCamera);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);

            wingAvatar = null;
            avatarCamera = null;
            scene_Obj = null;
            scenceTa = null;
            camera = null;
            scenceTzLg = null;

            m_proAvatar = null;

            _rideAvatarMapping.Clear();
            _rideAvatarMapping = null;

        }

        private void OnDisPose()
        {
            rideInfo.OnDispoes();
            rideModel.OnDispoes();
            rideWakan.OnDispose();
            DisposeAvatar();

            defaulSelect = -1;
            time = 0f;
        }

        private float time = 0f;

        void Update()
        {
            if (time < 0.8f)
            {
                time = time + Time.deltaTime;

            }

            if (time > 0.8f && isShowCompelet)
            {
                isShowCompelet = false;

                if (camera)
                {
                    camera.cullingMask = (1 << EnumLayer.LM_FX) | (1 << EnumLayer.LM_ROLE_INVISIBLE);
                }

            }

            //if ( m_proAvatar != null && time > 0.25f && m_proAvatar.rideGo != null && m_proAvatar.rideGo.transform.parent.name != m_proAvatar.m_curModel.name )
            //{
            //    //m_proAvatar. rideGo.transform.SetParent( m_proAvatar.m_curModel );

            //   
            //}

            if (m_proAvatar != null) {

                m_proAvatar.FrameMove();
            }

            //if ( _quanBg ) { _quanBg.transform.Rotate( Vector3.forward * 5 ); }

        }


        ///-----------------------------------fx
        
        private Action callBack;

        public void PlayFx(int i, Action callBack) {

            if (fxMapping[i] == null)
            {
                return;
            }

            this.callBack = callBack;

            fxMapping[i].gameObject.SetActive(true);

            Invoke("PlayEndFx",0.55f);

        }

        public void PlayEndFx() {

            fxGo.gameObject.SetActive(true);

            Invoke("PlayEndCallbck", 1f);

        }

        public void PlayEndCallbck() {

            if (this.callBack != null) callBack();

            FxClear();
        }

        public void FxClear() {

            callBack = null;

            foreach (var item in fxMapping)
            {
                item.Value.gameObject.SetActive(false);
            }

            fxGo.SetActive(false);
        }

    }

    public class RideInfo : Skin {

        private GameObject _attItem , _parentGo,_constageGo;
        private Text _textLvl , _rideName , _feedNeedNum , _textSpeed; //_itemNum
        private Slider _slider;
        private BaseButton _feed,_autofeed;
        private List<GameObject> _gridGoLst;
        private bool isAutoState = false;
        private bool isShow = false;


        private int num = 1;
        private int haveNum=0;


        GameObject helpcon;
        public RideInfo( Transform trans ) : base( trans )
        {
            _attItem = this.getGameObjectByPath( "att_temp" );
            _textLvl = this.getGameObjectByPath( "lvl" ).GetComponent<Text>();
            _rideName = this.getGameObjectByPath( "rideName" ).GetComponent<Text>();
            _feedNeedNum = this.getGameObjectByPath( "con_stage/text" ).GetComponent<Text>();
            //_itemNum = this.getGameObjectByPath( "con_stage/Num" ).GetComponent<Text>();
            _slider= this.getGameObjectByPath( "con_stage/slider" ).GetComponent<Slider>();
            _feed=new BaseButton( this.getTransformByPath( "con_stage/improve" ) );
            _autofeed=new BaseButton( this.getTransformByPath( "con_stage/improve_auto" ) );
            _parentGo =this.getGameObjectByPath( "att/grid" );
            _constageGo=this.getGameObjectByPath( "con_stage" );
            _textSpeed=this.getComponentByPath<Text>("Text");

            helpcon = this.transform.FindChild("help").gameObject;
            helpcon.SetActive(false);


            a3_ItemData itmeVo = a3_BagModel.getInstance().getItemDataById(A3_RideModel.getInstance().GetUpGradeRideItemId());
            this.getTransformByPath( "con_stage/icon" ).GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itmeVo.file);
        }

        public void OnAddOnClick() {

            _feed.onClick =OnBtnFeedHandle;
            _autofeed.onClick = OnBtnAutoFeedHandle;
            new BaseButton(this.transform.FindChild("help_btn")).onClick = (GameObject go) => {
                helpcon.SetActive(true);
            };
            new BaseButton(helpcon.transform.FindChild("close")).onClick = (GameObject go) => {
                helpcon.SetActive(false);
            };
        }

        private void OnBtnFeedHandle( GameObject go ) {

            if ( haveNum >= num && isAutoState == false)
            {
                A3_RideProxy.getInstance().SendC2S( 2 , "num" , 1 );
            }
            else {
                Globle.err_output(-1104);
                ArrayList data1 = new ArrayList();
                data1.Add(a3_BagModel.getInstance().getItemDataById(A3_RideModel.getInstance().GetUpGradeRideItemId()));
                data1.Add(InterfaceMgr.RIDE_A3);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
            }

        }

        private void OnBtnAutoFeedHandle( GameObject go ) {


            if ( haveNum > 0 && isAutoState == false )
            {
                isAutoState=true;

                var rideModel = A3_RideModel.getInstance();

                float  maxCount = ((float) rideModel.maxExp - ( float ) rideModel.GetRideInfo().exp) / (float)A3_RideModel.getInstance().eachExp;

                uint s2cCount =  (uint) Math.Ceiling( maxCount );

                A3_RideProxy.getInstance().SendC2S( 2 , "num" , s2cCount > (uint) haveNum ? ( uint ) haveNum : s2cCount );

            }
            else
            {

                Globle.err_output( -1104 );
                ArrayList data1 = new ArrayList();
                data1.Add(a3_BagModel.getInstance().getItemDataById(A3_RideModel.getInstance().GetUpGradeRideItemId()));
                data1.Add(InterfaceMgr.RIDE_A3);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data1);
            }

        }

        public void OnParentTabHandle() {

            SetRideInfo();

            isShow = true;

            var id = A3_RideModel.getInstance().GetRideInfo().dress;

            if ( ride_a3._instance != null )
            {
                ride_a3._instance.CreatRideAvatar( ( int ) id );
            }

        }

        private void SetRideInfo() {

            if ( _gridGoLst == null )
            {
                _gridGoLst=new List<GameObject>();

            }

            var rideInfo = A3_RideModel.getInstance().GetRideInfo();

            _textLvl.text = rideInfo.lvl + "";

            _rideName.text = rideInfo.configVo.name;

            StringBuilder str = new StringBuilder(ContMgr.getCont( "ride_speed" ));

            _textSpeed.text = str.Append( rideInfo.lvlconfigVo.speed.ToString()).Append("%").ToString();

            var itemId = A3_RideModel.getInstance().GetUpGradeRideItemId();

            haveNum = a3_BagModel.getInstance().getItemNumByTpid( itemId );

            _feedNeedNum.text = haveNum  + "/" + num.ToString();

            //_itemNum.text =haveNum.ToString();

            if (rideInfo.lvlconfigVo.attMappingLst.ContainsKey(PlayerModel.getInstance().profession))
            {
                SetGrid(rideInfo.lvlconfigVo.attMappingLst[PlayerModel.getInstance().profession], _attItem);
            }

            if ( rideInfo.lvlconfigVo.exp != -1 )
            {
                SetSliderValue( rideInfo.exp , ( uint ) rideInfo.lvlconfigVo.exp );
            }

            _constageGo.SetActive( rideInfo.lvlconfigVo.exp == -1 ? false : true);

        }

        private void SetGrid( List<AttConfigVo> attLst , GameObject originalGo  ) {

            if ( _gridGoLst.Count >= attLst.Count )
            {
                //
            }
            else {

                uint needItemNum = (uint)(attLst.Count - _gridGoLst.Count);

                List<GameObject>  goLst= ride_a3.BuildGo(_attItem,(int)needItemNum);

                _gridGoLst.AddRange( goLst );
            }

            for ( int i = 0 ; i < _gridGoLst.Count ; i++ )
            {
                if ( i >= attLst.Count )
                {

                    _gridGoLst[ i ].SetActive( false );

                }

                else {

                    SetGameObjectData( _gridGoLst[ i ] , attLst[ i ] , _parentGo );

                }
            }

        }

        private void SetGameObjectData( GameObject item , AttConfigVo vo , GameObject parent ) {
            Text text_name = item.transform.FindChild("text_name").GetComponent<Text>();
            Text text_value = item.transform.FindChild("text_value").GetComponent<Text>();
            //text_name.text = Globle.getAttrNameById( vo.att_type );
            text_name.text = Globle.getString(vo.att_type+"_"+ PlayerModel.getInstance().profession);
            text_value.text = vo.add + "";
            item.SetActive( true );
            item.transform.SetParent( parent.transform );
            item.transform.localScale = Vector3.one;
        }

        private void SetSliderValue( uint currExp , uint maxExp ) {

            if (isShow == false)
            {
                _slider.maxValue = maxExp;
                _slider.value = currExp;

                return;
            }

            Tween tween = null;

            if ( isAutoState && _slider.value < maxExp && !(Math.Abs( _slider.maxValue - maxExp ) <= float.Epsilon))
            {
                _slider.maxValue = maxExp;

                tween = DOTween.To((x) => _slider.value = x, _slider.value, _slider.maxValue, 0.5f);

                tween.OnUpdate(()=> {

                    if ( Math.Abs(_slider.value - _slider.maxValue) <=float.Epsilon ) // 达到最高
                    {
                        _slider.value = 0f;

                        DOTween.To((x) => _slider.value = x, _slider.value, currExp, 0.5f);
                    }

                });

                return;

            }

            _slider.maxValue = maxExp;

            if (currExp < _slider.value)
            {
                _slider.value = 0f;
            }

            DOTween.To( (x) => _slider.value = x , _slider.value , currExp , 0.5f );
            
        }

        public void OnAddEventListener() {

            A3_RideProxy.getInstance().addEventListener( ( int ) S2Cenum.RIDE_UPGRADE , OnUpdateData );

        }

        public void OnRemoveEventListener()
        {
            A3_RideProxy.getInstance().removeEventListener( ( int ) S2Cenum.RIDE_UPGRADE , OnUpdateData );
        }

        private void OnUpdateData(GameEvent _event) {

            SetRideInfo();

            if ( isAutoState )
            {
                isAutoState = false;
            }  

            

        }

        public void OnDispoes() {

            if ( _gridGoLst != null )
            {
                for ( int i = 0 ; i < _gridGoLst.Count ; i++ )
                {
                    GameObject.Destroy( _gridGoLst[ i ] );
                }

                _gridGoLst.Clear();

                _gridGoLst=null;

            }

            OnRemoveEventListener();

            isAutoState=false;

            isShow = false;

        }

    }


    public class RideModel : Skin
    {
        private Text _textLvl,_textName, _textTips,_textLimit;
        private BaseButton _btnLock,_btnChange;
        private GameObject _itemGo,_conditionGo;
        private GameObject _parentitemGo,_parentconditionGo,_selectBg,_titleGo,_iconUp;


        private List<GameObject> _golst;
        private List<BaseButton> _btnLst;
        private List<GameObject> _conditiongolst;
        private GameObject currGo;
        private Dictionary<int,GameObject> _goMapping = null;

        public bool isLoading=false;

        GameObject helpcon;
        public RideModel( Transform trans ) : base( trans )
        {
            _textLvl= this.getComponentByPath<Text>( "lvl" );
            _textName= this.getComponentByPath<Text>( "rideName" );
            _btnLock=new BaseButton(this.getTransformByPath( "btnLock" ));
            _itemGo=this.getGameObjectByPath( "rideIconItem" );
            _conditionGo=this.getGameObjectByPath( "conditionItem" );
            _parentitemGo=this.getGameObjectByPath( "Scroll/Container" );
            _parentconditionGo=this.getGameObjectByPath( "conditionContainer" );
            _selectBg= this.getGameObjectByPath( "selectBg" );
            _titleGo= this.getGameObjectByPath( "Text" );
            _btnChange=new BaseButton( this.getTransformByPath( "btnChange" ) );
            _textTips = this.getComponentByPath<Text>("Tips_Text");
            _iconUp = this.getGameObjectByPath("icon_up");
            //_textLimit = this.getComponentByPath<Text>("Text_limit");


            helpcon = this.transform.FindChild("help").gameObject;
            helpcon.SetActive(false);


        }

        public void OnParentTabHandle() {

            var mapping = A3_RideModel.getInstance().GetRideInfo().ridedressMapiping;

            if (mapping != null)
            {
                List<uint> removeLst = new List<uint>();

                foreach (var item in mapping)
                {
                    if ( item.Value.isforever == false && NetClient.instance.CurServerTimeStamp > item.Value.limit  )
                    {
                        removeLst.Add(item.Key);
                    }
                }

                for (int i = 0; i < removeLst.Count; i++)
                {

                   if(mapping.ContainsKey(removeLst[i])) mapping.Remove(removeLst[i]);

                    //ChangeLimitState((int)removeLst[i]);

                }

            }

            SetRideModelInfo();

        }

        private void SetRideModelInfo() {

            var rideInfo = A3_RideModel.getInstance().GetRideInfo();
            _textLvl.text  = rideInfo.lvl.ToString();
            _textName.text = rideInfo.configVo.name.ToString();

            if ( _golst == null )
            {
                _golst=new  List<GameObject>();
                _btnLst = new List<BaseButton>();
                _goMapping = new Dictionary<int, GameObject>();

                BuildItem( A3_RideModel.getInstance().GetAllRideMapping(), _itemGo );
            }

            if ( _btnLst.Count > 0  )
            {
                var _currGo = GetCurrGameObject( ( int ) rideInfo.dress );

                ItemBtnHandle( _currGo == null  ? _btnLst[ 0 ].gameObject : _currGo );
            } 
        }

        private GameObject GetCurrGameObject(int type ) {

            for ( int i = 0 ; i < _btnLst.Count ; i++ )
            {
                if ( _btnLst[i].gameObject.name.Equals( type.ToString()) )
                {
                    return _btnLst[ i ].gameObject;
                }

            }

            return null;

        }

        private void BuildItem( Dictionary<int,ConfigVo> mapping , GameObject org ) {

            if ( _golst.Count >= mapping.Count )
            {

            }
            else {

                int needNum = mapping.Count - _golst.Count ;

                var lst = ride_a3.BuildGo( org , needNum);

                _golst.AddRange( lst );

            }

            var _lst = mapping.ToArray();

            for ( int i = 0 ; i < _golst.Count ; i++ )
            {
                if ( i >= _lst.Length )
                {

                    _golst[ i ].SetActive(false);

                }
                else {

                    SetRideData( _golst[ i ] , _lst[i].Value as RideConfigVo );

                }

            } 

        }

        private void SetRideData( GameObject go , RideConfigVo vo ) {

            Image icon = go.transform.FindChild("Image/icon").GetComponent<Image>();

            var image_Lock = go.transform.FindChild("Image/Image_Lock").gameObject;

            icon.sprite = GAMEAPI.ABUI_LoadSprite( "icon_item_" + vo.icon );

            BaseButton btn = new BaseButton( go.transform.FindChild("Image"));

            btn.onClick = ItemBtnHandle;

            _btnLst.Add( btn );

            icon.transform.parent.gameObject.name = vo.id.ToString();

            go.SetActive( true );

            go.transform.SetParent( _parentitemGo.transform );

            go.transform.localScale = new Vector3(1, 1, 1);

            var rideInfo = A3_RideModel.getInstance().GetRideInfo();

            var mapping = rideInfo.ridedressMapiping;

            image_Lock.SetActive(!mapping.ContainsKey((uint)vo.id));

            if (_goMapping != null )
            {
                _goMapping[vo.id] = go;

            }

        }

        private void BuildConditionItem( List<ConditionVO> lst , GameObject org ) {

            if ( _conditiongolst == null )
            {
                _conditiongolst = new List<GameObject>();

            }

            if ( _conditiongolst.Count >= lst.Count )
            {

            }
            else
            {
                int needNum = lst.Count - _conditiongolst.Count ;

                var _lst = ride_a3.BuildGo( org , needNum);

                _conditiongolst.AddRange( _lst );

            }

            for ( int i = 0 ; i < _conditiongolst.Count ; i++ )
            {
                if ( i >= lst.Count )
                {
                    _conditiongolst[ i ].SetActive( false );
                }
                else {

                    SetConditionData( _conditiongolst [i] , lst [i]);

                }
            }

        }

        private void SetConditionData(GameObject go , ConditionVO vo) {

            Text type = go.transform.FindChild("Text_ConditionTitle").GetComponent<Text>();
            Text value = go.transform.FindChild("Text_Value").GetComponent<Text>();

            if ( vo.type == 1 )
            {
                type.text = ContMgr.getCont( "condition_type"+ vo .type);
                value.text =  A3_RideModel.getInstance().GetRideLvl() + "/"+vo.lvl.ToString();

                bool isGreen =  A3_RideModel.getInstance().GetRideLvl() >=  (int)vo.lvl;

                value.color = isGreen ? Color.green : Color.red;


            } else {

                type.text = a3_BagModel.getInstance().getItemDataById( vo.need_item  ).item_name;
                value.text = a3_BagModel.getInstance().getItemNumByTpid( vo.need_item )+ "/"+vo.need_num.ToString();

                bool isGreen =  a3_BagModel.getInstance().getItemNumByTpid( vo.need_item ) >= (int)vo.need_num;

                value.color = isGreen ? Color.green : Color.red;
            }

            go.SetActive(true);
            go.transform.SetParent( _parentconditionGo.transform );

        }

        private void ItemBtnHandle(GameObject go) {

            if ( currGo != null )
            {
                if ( int.Parse( go.name ) == int.Parse( currGo.name ) || isLoading )
                {
                    return;
                }
            }

            isLoading = true;

            RideConfigVo vo = A3_RideModel.getInstance().GetValueByType<RideConfigVo>(int.Parse(go.name));

            BuildConditionItem( vo.conditionLst , _conditionGo );

            _textName.text = vo.name.ToString();

            currGo = go;

            if ( ride_a3._instance != null )
            {
                ride_a3._instance.CreatRideAvatar( int.Parse( go.name ) );
            }

            SetBtnLockState( vo.id );
        }

        private void SetBtnLockState(int id) {

            var rideInfo= A3_RideModel.getInstance().GetRideInfo();

            var mapping =rideInfo.ridedressMapiping;

            _btnLock.gameObject.SetActive( !mapping.ContainsKey( (uint)id ) );

            _parentconditionGo.SetActive( !rideInfo.ridedressMapiping.ContainsKey( ( uint ) id ) );

            _titleGo.SetActive( !rideInfo.ridedressMapiping.ContainsKey( ( uint ) id ) );

            //if (rideInfo.ridedressMapiping.ContainsKey((uint)id))
            //{
            //    _textLimit.gameObject.SetActive(rideInfo.ridedressMapiping[(uint)id].limit > 0);

            //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            //    string st = dtStart.AddSeconds(rideInfo.ridedressMapiping[(uint)id].limit).ToString("yyyy-MM-dd");

            //    _textLimit.text = st;

            //}
            //else {

            //    _textLimit.gameObject.SetActive(false);

            //}  // 到期时间

            _btnChange.gameObject.SetActive( rideInfo.ridedressMapiping.ContainsKey( ( uint ) id ) && rideInfo.dress != ( uint ) id );

            currGo.transform.FindChild( "Image_Lock" ).gameObject.SetActive( !mapping.ContainsKey( ( uint ) id ) );

            _textTips.gameObject.SetActive(rideInfo.ridedressMapiping.ContainsKey((uint)id) && rideInfo.dress == (uint)id);

            //_iconUp.gameObject.SetActive( rideInfo.ridedressMapiping.ContainsKey((uint)id) && rideInfo.dress == (uint)id );

            if (rideInfo.dress == (uint)id)
            {
                _iconUp.transform.SetParent(currGo.transform);
                _iconUp.transform.localPosition = Vector3.zero;
            } 

            //xuan zhe zhuang tai  tu pian 

            if (mapping.ContainsKey((uint)id))
            {
                _selectBg.transform.SetParent(currGo.transform);

                _selectBg.transform.SetSiblingIndex(1);
            }
            else {

                _selectBg.transform.SetParent(currGo.transform.FindChild("Image_Lock").transform);

                _selectBg.transform.SetSiblingIndex(0);

            }

            _selectBg.transform.localPosition = Vector3.zero;

        }

        public void OnAddOnClick() {

            _btnLock.onClick = ( go ) => {

                int id = int.Parse(currGo.name);

                RideConfigVo vo = A3_RideModel.getInstance().GetValueByType<RideConfigVo>(id);

                int errCode =  GetErrCode( vo.conditionLst);

                if ( errCode < 0 )
                {
                    Globle.err_output( errCode );

                    return;
                }

                if ( vo.condition != -1  )
                {
                    A3_RideProxy.getInstance().SendC2S( (uint) id  , 0);

                }else {

                    A3_RideProxy.getInstance().SendC2S( ( uint ) id , vo.conditionLst[0].type );

                }

            };

            _btnChange.onClick = ( go ) =>

            {
                A3_RideProxy.getInstance().SendC2S( 5 , "dress" , ( uint ) int.Parse( currGo.name ));
            };

            new BaseButton(this.transform.FindChild("help_btn")).onClick = (GameObject go) => {
                helpcon.SetActive(true);
            };
            new BaseButton(helpcon.transform.FindChild("close")).onClick = (GameObject go) => {
                helpcon.SetActive(false);
            };
        }

        private int GetErrCode(List<ConditionVO> lst) {

            for ( int i = 0 ; i < lst.Count ; i++ )
            {
               var type = lst[ i ].type;

                if ( type == 1 )
                {
                    var currLvl = A3_RideModel.getInstance().GetRideInfo().lvl;

                    if ( currLvl < lst[ i ].lvl )
                    {
                        return -5706;
                    }

                }
                else {

                    var itemId = lst[i].need_item;
                    var needNum = lst[i].need_num;
                    var haveNum = a3_BagModel.getInstance().getItemNumByTpid(itemId);

                    if ( haveNum < needNum )
                    {
                        ArrayList data = new ArrayList();
                        data.Add(a3_BagModel.getInstance().getItemDataById(itemId));
                        data.Add(InterfaceMgr.RIDE_A3);
                        InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data);
                        return -1104;
                    }

                }

            }

            return 0;

        }

        private void OnUpDressState( GameEvent _event )
        {
            var rideInfo = A3_RideModel.getInstance().GetRideInfo();
            _btnChange.gameObject.SetActive( rideInfo.ridedressMapiping.ContainsKey( ( uint ) int.Parse( currGo.name ) ) && rideInfo.dress != ( uint ) int.Parse( currGo.name ) );
            //currGo.transform.FindChild( "Image_Lock" ).gameObject.SetActive( !rideInfo.ridedressMapiping.ContainsKey( ( uint ) int.Parse( currGo.name ) ) );
            //currGo.transform.FindChild("icon_up").gameObject.SetActive(rideInfo.ridedressMapiping.ContainsKey((uint)int.Parse(currGo.name)) && rideInfo.dress == (uint)int.Parse(currGo.name));
            _iconUp.transform.SetParent(currGo.transform);
            _iconUp.transform.localPosition = Vector3.zero;
            _textTips.gameObject.SetActive(rideInfo.ridedressMapiping.ContainsKey((uint)int.Parse(currGo.name)) && rideInfo.dress == (uint)int.Parse(currGo.name));

        }

        public void OnAddEventListener()
        {
            A3_RideProxy.getInstance().addEventListener( ( int ) S2Cenum.RIDE_ADD , OnUpdateData );
            A3_RideProxy.getInstance().addEventListener( ( int ) S2Cenum.RIDE_CHANGE , OnUpDressState );
            A3_RideProxy.getInstance().addEventListener( ( int ) S2Cenum.RIDE_LIMIT, OnUpdateLimit);

        }

        private void OnUpdateData(GameEvent _event) {

            if ( int.Parse( currGo.name ) == ( int ) _event.data.getValue( "dress" )._uint )
            {
                SetBtnLockState( int.Parse( currGo.name ) );
            }
            else {

                //
            }
        }

        private void OnUpdateLimit(GameEvent _event)
        {
            if (!_event.data.ContainsKey("lock_dress") || _goMapping == null)
            {
                return;
            }

            var id = (int)_event.data.getValue("lock_dress")._uint;

            ChangeLimitState(id);

        }

        private void ChangeLimitState(int id)
        {
            if (int.Parse(currGo.name) == id)
            {
                SetBtnLockState(id);
            }
            else
            {

                //
            }

            if (_goMapping.ContainsKey(id))
            {
                _goMapping[id].transform.FindChild(id + "/Image_Lock").gameObject.SetActive(A3_RideModel.getInstance().GetRideInfo().ridedressMapiping.ContainsKey((uint)id));

                //限时到期
            }
        }

        public void OnRemoveEventListener()
        {
            A3_RideProxy.getInstance().removeEventListener( ( int ) S2Cenum.RIDE_ADD , OnUpdateData );
            A3_RideProxy.getInstance().removeEventListener( ( int ) S2Cenum.RIDE_CHANGE , OnUpDressState );
            A3_RideProxy.getInstance().removeEventListener( ( int ) S2Cenum.RIDE_LIMIT, OnUpdateLimit);
        }


        public void OnDispoes() {

            _selectBg.transform.SetParent( this.transform );
            _iconUp.transform.SetParent(this.transform);
            if ( _btnLst !=null )
            {
                for ( int i = 0 ; i < _btnLst.Count ; i++ )
                {
                    _btnLst[ i ].dispose();
                }

                _btnLst.Clear();
                _btnLst=null;
            }


            if ( _golst != null)
            {
                for ( int i = 0 ; i < _golst.Count ; i++ )
                {
                    GameObject.Destroy( _golst[ i ] );
                }
                _golst.Clear();
                _golst=null;
            }

            if ( _conditiongolst != null )
            {
                for ( int i = 0 ; i < _conditiongolst.Count ; i++ )
                {
                    GameObject.Destroy( _conditiongolst[ i ] );
                }

                _conditiongolst.Clear();
                _conditiongolst=null;

            }

            if (_goMapping != null)
            {
                foreach (var item in _goMapping)
                {
                    GameObject.Destroy(item.Value);
                }

                _goMapping.Clear();
                _goMapping = null;

            }

           
            currGo = null;
            isLoading=false;

            OnRemoveEventListener();

        }

    }


    public class RideWakan : Skin
    {

        private GameObject _parentGo,_parentNextGo,itemGo,_nextContainer,_btnContainer,_quanBg;
        private BaseButton _btn;
        private TabControl tab;
        private Text _lvl,_ridename,_needNumText,_haveNumText;
        private Dictionary<int,Text> _textMapping;

        private int defaulSelect = -1;
        private List<GameObject> _nextgolst;
        private List<GameObject> _golst;
        private int needNum,haveNum;

        GameObject helpcon;
        public RideWakan( Transform trans ) : base( trans )

        {
            _parentGo = getGameObjectByPath( "currWakanAtt/att/grid" );
            _parentNextGo = getGameObjectByPath( "NextWakanAtt/att/grid" );
            itemGo = getGameObjectByPath( "att_temp" );
            _lvl = getComponentByPath<Text>( "lvl" );
            _ridename = getComponentByPath<Text>( "rideName" );
            _haveNumText = getComponentByPath<Text>( "con_stage/Num" );
            _needNumText = getComponentByPath<Text>( "con_stage/improve/text" );

            //_help = new BaseButton(getTransformByPath( "title/help" ) );
            _btn  = new BaseButton( getTransformByPath( "con_stage/improve" ) );

            _nextContainer= getGameObjectByPath( "NextWakanAtt" );

            _btnContainer=getGameObjectByPath( "con_stage" );

            _textMapping = new Dictionary<int , Text>();

            for ( int i = 0 ; i < 5 ; i++ )
            {
                Text _text =   this.getComponentByPath<Text>("BtnContainer/Image" +(i+1).ToString()+"/Text");

                _textMapping.Add( i+1 , _text );
            }

            tab = new TabControl();
            tab.onClickHanle = tabhandel;
            tab.create( this.getGameObjectByPath( "BtnContainer" ) , this.gameObject , defaulSelect == -1 ? 0 : 0 , 0 , false );

            a3_ItemData itmeVo = a3_BagModel.getInstance().getItemDataById(A3_RideModel.getInstance().GetUpGradeGiftItemId());
            this.getTransformByPath( "con_stage/improve/icon" ).GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite( itmeVo.file );
            helpcon = this.transform.FindChild("help").gameObject;
            helpcon.SetActive(false);

        }

        public void OnParentTabHandle()
        {
            defaulSelect=-1;
            tab.setSelectedIndex( defaulSelect == -1 ? 0 : 0,true);

            var giftMapping = A3_RideModel.getInstance().GetRideInfo().giftMapping;

            for ( int i = 0 ; i < _textMapping.Count ; i++ )
            {
                _textMapping[ i+1 ].text = "Lv." + giftMapping[ ( uint ) i+1].lvl;
            }
        }

        private void SetRideGiftData(uint type)
        {
            var giftVo = A3_RideModel.getInstance().GetRideInfo().giftMapping[type];
            _ridename.text = ContMgr.getCont( "gift_type" + type );
            _lvl.text = giftVo.lvl.ToString();

            if ( _golst == null )
            {
                _golst = new List<GameObject>();
                _nextgolst=new List<GameObject>();
            }

            RideGiftConfigVo configVo = A3_RideModel.getInstance().GetValueByType<RideGiftConfigVo>((int)giftVo.lvl);

            needNum=configVo.num;

            _needNumText.text = configVo.num.ToString();

            var id = A3_RideModel.getInstance().GetUpGradeGiftItemId();

            haveNum = a3_BagModel.getInstance().getItemNumByTpid( id );

            _haveNumText.text = haveNum.ToString();

            BuildGo( configVo.attMaping[(int)type]  , _golst , _parentGo );

            _nextContainer.SetActive( configVo.num != -1 );

            _btnContainer.gameObject.SetActive( configVo.num != -1 );

            if ( configVo.num != -1 )  //满了 -1
            {
                configVo = A3_RideModel.getInstance().GetValueByType<RideGiftConfigVo>( ( int ) giftVo.lvl + 1 );

                BuildGo( configVo.attMaping[ ( int ) type ] , _nextgolst , _parentNextGo );
            }

        }

        private void BuildGo( List<AttConfigVo> attLst , List<GameObject> lst , GameObject parent ) {

            if ( lst.Count >= attLst.Count )
            {

            }else {

                int needNum = attLst.Count  - lst.Count;

                var _lst = ride_a3.BuildGo( itemGo , needNum );

                lst.AddRange( _lst );
           }

            for ( int i = 0 ; i < lst.Count ; i++ )
            {
                if ( i >=  attLst.Count )
                {
                    lst[ i ].SetActive( false );
                }
                else {

                    SetData( lst [i], attLst [i], parent );

                }
            }

        }

        private void SetData(GameObject go , AttConfigVo vo , GameObject parent ) {

            Text type = go.transform.FindChild("text_name").GetComponent<Text>();
            Text value = go.transform.FindChild("text_value").GetComponent<Text>();

             //type.text =Globle.getAttrNameById( vo.att_type );
            type.text =Globle.getString(vo.att_type + "_" + PlayerModel.getInstance().profession);
            value.text = vo.add.ToString();

            go.SetActive( true );
            go.transform.SetParent( parent .transform);
            go.transform.localScale = Vector3.one;

        }

        private void tabhandel( TabControl t) {

            int currIndex = t.getSeletedIndex();

            if ( defaulSelect != -1 && defaulSelect == currIndex )
            {
                return;
            }

            defaulSelect = currIndex;

            SetRideGiftData( (uint)currIndex + 1);

        }

        public void OnAddOnClick() {
            new BaseButton(this.transform.FindChild("help_btn")).onClick = (GameObject go) => {
                helpcon.SetActive(true);
            };
            new BaseButton(helpcon.transform.FindChild("close")).onClick = (GameObject go) => {
                helpcon.SetActive(false);
            };

            _btn.onClick =( go ) =>
            {
                if (_btn.interactable == false)
                {
                    return;
                }

                if ( needNum > haveNum )
                {
                    Globle.err_output( -1104 );
                    ArrayList data = new ArrayList();
                    data.Add(a3_BagModel.getInstance().getItemDataById(A3_RideModel.getInstance().GetUpGradeGiftItemId()));
                    data.Add(InterfaceMgr.RIDE_A3);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMLACK, data);
                    return;
                }
                _btn.interactable = false;

                A3_RideProxy.getInstance().SendC2S(3, "type" , (uint)defaulSelect +1);
            };
        }

        public void OnAddEventListener()
        {
            A3_RideProxy.getInstance().addEventListener( ( int ) S2Cenum.RIDE_UPGRADEGIFT , OnUpdateGiftData );
        }

        private void OnUpdateGiftData( GameEvent _event )
        {
            if ( _event.data.getValue( "type" )._uint  == ( (defaulSelect == -1 ? 0 : defaulSelect)+1) )
            {
                SetRideGiftData((uint)(( defaulSelect == -1 ? 0 : defaulSelect )+1));
            }

            _textMapping[ defaulSelect +1 ].text ="Lv." + _event.data.getValue( "lvl" )._uint.ToString();

            if (ride_a3._instance != null)
            {
                ride_a3._instance.PlayFx( defaulSelect , ()=> {

                    _btn.interactable = true;

                    Debug.LogError("特效播放完了");

                });
            }
        }

        public void OnRemoveEventListener()
        {
            A3_RideProxy.getInstance().removeEventListener( ( int ) S2Cenum.RIDE_UPGRADEGIFT , OnUpdateGiftData );
        }

        public void OnDispose() {

            if ( _nextgolst != null )
            {
                for ( int i = 0 ; i < _nextgolst.Count ; i++ )
                {
                    GameObject.Destroy( _nextgolst[ i ] );
                }

                _nextgolst.Clear();
                _nextgolst=null;
            }


            if ( _golst != null )
            {
                for ( int i = 0 ; i < _golst.Count ; i++ )
                {
                    GameObject.Destroy( _golst[ i ] );
                }

                _golst.Clear();
                _golst=null;
            }

            defaulSelect=-1;

            OnRemoveEventListener();

        }

    }


}
