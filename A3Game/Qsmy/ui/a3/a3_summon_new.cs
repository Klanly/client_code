using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using Cross;
using UnityEngine.UI;
using System.Collections;

namespace MuGame
{
    class BaseSummon : Skin {
        public BaseSummon(Transform trans,string name) : base(trans)
        {
            TransName = name;
            tranObj = trans.gameObject;
        }
        public string TransName = null;
        public GameObject tranObj = null;
        public uint CurSummonID = 0;
        public int select_tujian = -1;
        public SXML sumXml = XMLMgr.instance.GetSXML("callbeast");
        public int TopType = 0;

        public void refreCurSummonID()
        {
            CurSummonID = getSummonWin().getCurSummonID();
        }

        public void closeWin(string name)
        {
            if (getSummonWin() == null) return;
            if (getSummonWin().smallWin.ContainsKey(name)) {
                getSummonWin().smallWin[name].SetActive(false);
            }
        }

        public void  refreSumlist(int select = -1 , bool lianxie = false, int screen = 0)
        {
            if (getSummonWin() == null) return;
            getSummonWin().ref_SetSumlist(select , lianxie, screen);
        }
        public a3_summon_new getSummonWin()
        {
            if (a3_summon_new.getInstance != null)
            {
                return a3_summon_new.getInstance;
            }
            else
                return null;
        }

        virtual public void _updata() { }
        virtual public void onAddNewSmallWin(string name) { }
        virtual public void onCurSummonIdChange() {

            if (getSummonWin().CurTranType != 0)
            {
                if (!A3_SummonModel.getInstance().Checklvl(getSummonWin().CurTranType, CurSummonID))
                {
                    getSummonWin().black_Tab();
                }
            }
        }
        virtual public void onShowed() { }
        virtual public void onClose() { }

        void dispose()
        {
            if (getSummonWin().avatorobj != null)
            {
                GameObject.Destroy(getSummonWin().avatorobj);
                GameObject.Destroy(getSummonWin().camobj);
                getSummonWin().avatorobj = null;
                getSummonWin().camobj = null;
            }

            if (getSummonWin().avator_look != null) { GameObject.Destroy(getSummonWin().avator_look); }
        }

    }
    class a3_summon_new : Window
    {
        private BaseSummon Curtran = null;
        public int CurTranType = 0;
        private Transform contents;
        private Transform Btns;
        private Transform summonlist;
        private Transform summonVeiw;
        private Text listCount;

        //private GameObject selectframe;
        //private GameObject _selecobj;
        private A3_SummonModel model;
        public static a3_summon_new getInstance;
        public Transform smallwinCon;
        private Dictionary<string, BaseSummon> _Trans = new Dictionary<string, BaseSummon>();
        Dictionary<uint, GameObject> summonObj = new Dictionary<uint, GameObject>();
        public Dictionary<string, GameObject> smallWin = new Dictionary<string, GameObject>();
        private uint CurSummonID = 0;//使用setCurSummonID对其赋值

        public GameObject avatorobj;
        public GameObject camobj;
        public GameObject avator_look;

        ScrollControler scrollControler;
        public override void init()
        {

            getComponentByPath<Text>("tabs/shuxing/Text").text = ContMgr.getCont("a3_summon_new_0");
            getComponentByPath<Text>("tabs/xilian/Text").text = ContMgr.getCont("a3_summon_new_1");
            getComponentByPath<Text>("tabs/shouhun/Text").text = ContMgr.getCont("a3_summon_new_2");
            getComponentByPath<Text>("tabs/ronghe/Text").text = ContMgr.getCont("a3_summon_new_3");
            getComponentByPath<Text>("tabs/tunshi/Text").text = ContMgr.getCont("a3_summon_new_4");
            getComponentByPath<Text>("tabs/tujian/Text").text = ContMgr.getCont("a3_summon_new_5");
            getComponentByPath<Text>("tabs/lianxie/Text").text = ContMgr.getCont("a3_summon_new_6");

            getInstance = this;
            model = A3_SummonModel.getInstance();
            contents = this.transform.FindChild("contents");
            Btns = this.transform.FindChild("tabs");
            summonlist = this.transform.FindChild("summonlist");
            summonVeiw = summonlist.FindChild("summons/scroll/content");
            //selectframe = summonlist.FindChild("summons/scroll/frame").gameObject;
            smallwinCon = this.transform.FindChild("small_veiw");
            listCount = this.transform.FindChild("summonlist/num").GetComponent<Text>();
            for (int i = 0;i< Btns.childCount;i++)
            {
                new BaseButton(Btns.GetChild(i)).onClick = (GameObject go) => 
                {
                    onTab(go.name);
                };
            }
            new BaseButton(this.transform.FindChild("btn_close")).onClick = (GameObject go) => 
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_SUMMON_NEW);
            };
            scrollControler = new ScrollControler();
            ScrollRect scroll = transform.FindChild("summonlist/summons/scroll").GetComponent<ScrollRect>();
            scrollControler.create(scroll);

        }
        public override void onShowed()
        {
            if (getInstance == null) return;
            if (uiData != null && uiData.Count > 0){
                int tpid = -1;
                if (uiData.Count > 1)
                    tpid = (int)uiData[1];

                onTab((string)uiData[0], tpid);
            }
            else {
                if (model.GetSummons().Count <= 0)
                    onTab("tujian");
                else
                    onTab("shuxing");
            }

            refresh_summon_list();

            UiEventCenter.getInstance().onWinOpen(uiName);
        }
        public uint getCurSummonID() {
            return CurSummonID;
        }

        public void setCurSummonID( uint value) {
            if (value != CurSummonID && Curtran!= null) {
                CurSummonID = value;
                Curtran.refreCurSummonID();//刷新子界面当前召唤兽id
                Curtran.onCurSummonIdChange();
            }
        }


        void clearCon_summon(bool lianxie = false,int screen = -1) {
            if(!lianxie && screen < 0)
                setCurSummonID(0);
            if (ConClon != null)
            Destroy(ConClon);
            summonObj.Clear();

        }
        GameObject ConClon;
        public void refresh_summon_list(int select = -1, bool lianxie = false ,int screen = 0)
        {
            clearCon_summon(lianxie, screen);
            GameObject item = summonlist.FindChild("summons/scroll/0").gameObject;
            ConClon = Instantiate(summonVeiw.gameObject) as GameObject;
            ConClon.SetActive(true);
            ConClon.transform.SetParent(summonlist.FindChild("summons/scroll"),false);
            summonlist.FindChild("summons/scroll").GetComponent<ScrollRect>().content = ConClon.GetComponent <RectTransform>();
            Dictionary <uint, a3_BagItemData> sT = model.GetSummons(true);
            foreach (a3_BagItemData it in sT.Values)
            {
                if (screen != 0)
                {
                    if (!A3_SummonModel.getInstance().Checklvl(screen, (uint)it.summondata.id))
                    {
                        continue;
                    }
                }
                if (lianxie) {

                    if (it.summondata .id == getCurSummonID()) {
                        bool change = true;
                        foreach (int idx in it.summondata.linkdata.Keys) {
                            if (it.summondata.linkdata[idx].type != 0) { 
                                change = false;
                                break;
                            }
                            if (change) setCurSummonID(0);
                        }
                    }
                    if (A3_SummonModel .getInstance ().link_list .Contains ((uint)it.summondata.id )) { continue; }
                    if (it.summondata.linkdata == null) { continue; }
                    if (!A3_SummonModel.getInstance().Checklvl(6,(uint)it.summondata.id))
                    {
                        continue;
                    }
                    bool have = false;
                    foreach (int idx in it.summondata.linkdata.Keys)
                    {
                        if (it.summondata.linkdata[idx].type != 0)
                        {
                            have = true;
                            break;
                        }
                    }
                    if (have == false) { continue; }
                }
                GameObject clon = Instantiate(item) as GameObject;
                clon.SetActive(true);
                clon.transform.SetParent(ConClon.transform, false);
                
                
                if ((uint)it.summondata.id == A3_SummonModel.getInstance().nowShowAttackID)
                    clon.transform.FindChild("fighting").gameObject.SetActive(true);
                else
                    clon.transform.FindChild("fighting").gameObject.SetActive(false);

                if (A3_SummonModel.getInstance().link_list.Contains((uint)it.summondata.id)) {
                    clon.transform.FindChild("articles").gameObject.SetActive(true);
                }else
                    clon.transform.FindChild("articles").gameObject.SetActive(false);

                clon.name = it.summondata.id.ToString();
                new BaseButton(clon.transform).onClick = (GameObject go) => {
                    setCurSummonID((uint)it.summondata.id);
                    setframe();
                };
                summonObj[(uint)it.summondata.id] = clon;
                setSum_one((uint)it.summondata.id);
            }
            ConClon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            setListCount();

            if (select == -1)
                SetFristSelect();
            else {
                if (summonObj.ContainsKey((uint)select))
                {
                    setCurSummonID((uint)select);
                    setframe();
                }
                else
                    SetFristSelect();
            }
        }


        public void ref_SetSumlist(int select = -1 , bool lianxie = false, int screen= 0 )//刷新列表和选中id
        {
            refresh_summon_list(select ,lianxie, screen);
            if (summonObj.Count <= 0) {
                if (Curtran.TransName != "tujian" && !lianxie)
                {
                    onTab("tujian");
                }
            }

            
        }
        public void setSum_one(uint id)
        {
            if (summonObj.ContainsKey(id))
            {
                a3_BagItemData it = model.GetSummons()[id];
                summonObj[id].transform.FindChild("lv").GetComponent<Text>().text = it.summondata.level.ToString();
                SetIcon(it, summonObj[id].transform.FindChild("icon"));
                setStar(summonObj[id].transform.FindChild("stars"), it.summondata.star);
            }
        }

        void SetFristSelect( )//初始化默认选择第一个
        {
            if ((getCurSummonID() <= 0 && ConClon.transform.childCount >0) ) {
                setCurSummonID (uint.Parse (ConClon.transform.GetChild(0).gameObject.name));
                
            }
            setframe();
        }

        public void setframe()
        {
            foreach (uint id in summonObj.Keys)
            {
                if (id == getCurSummonID())
                    summonObj[id].transform.FindChild("frame").gameObject.SetActive(true);
                else
                    summonObj[id].transform.FindChild("frame").gameObject.SetActive(false);
            }
        }

        public GameObject SetIcon(a3_BagItemData data, Transform parent, int num = -1) {
            for (int i=0;i< parent.childCount;i++) {
                GameObject.Destroy(parent.GetChild (i).gameObject);
            }
             data.confdata.borderfile = "icon_itemborder_b039_0" + (data.summondata.star);
            GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(data.confdata, false, num, 1, false, -1, 0, false, false);
            icon.transform.SetParent(parent, false);
            icon.transform.localScale = new Vector3(.9f, .9f, 0);
            return icon;
        }
        public void setStar(Transform starRoot, int num)
        {
            for (int i = 0; i < 5; i++)
            {
                starRoot.GetChild(i).FindChild("b").gameObject.SetActive(false);
            }

            for (int j = 0; j < num; j++)
            {
                starRoot.GetChild(j).FindChild("b").gameObject.SetActive(true);
            }
        }


        public void black_Tab() {
            onTab("shuxing");
        }
        void setListCount()
        {
            int count = summonObj.Count;
            listCount.text = count + "/"+A3_SummonModel.getInstance().allcount;
        }

        int GetType(string str) {
            switch (str) {
                case "shuxing":return 1;
                case "xilian": return 2;
                case "shouhun": return 3;
                case "ronghe": return 4;
                case "tunshi": return 5;
                case "lianxie": return 6;
                case "tujian": return 0;
            }
            return 0;
        }



        public override void onClosed() {
            if (Curtran != null)
            {
                Curtran.onClose();
                Curtran = null;
            }
            clearCon_summon();
        }

        void onTab(string tran,int select_tujian = -1) {
            if (model.GetSummons().Count <= 0 && tran != "tujian")
            {
                flytxt.instance.fly(ContMgr.getCont("a3_summon_unllSummon"));
                return;
            }

            if (!A3_SummonModel.getInstance().Checklvl(GetType(tran), CurSummonID) && tran != "tujian" && A3_SummonModel.getInstance().GetSummons().ContainsKey(CurSummonID))
            {
                flytxt.instance.fly("召唤兽等级不足" + A3_SummonModel.getInstance().limitList[GetType(tran)].lvl + "级");
                return;
            }
            switch (tran) {
                case "shuxing": CurTranType = 1; break;
                case "xilian": CurTranType = 2; break;
                case "shouhun": CurTranType = 3; break;
                case "ronghe": CurTranType = 4; break;
                case "tunshi": CurTranType = 5; break;
                case "lianxie": CurTranType = 6; break;
                case "tujian": CurTranType = 0; break;

            }

            for (int i = 0; i < Btns.childCount; i++)
            {
                Btns.GetChild(i).GetComponent<Button>().interactable = true;
            }
            Btns.FindChild(tran).GetComponent<Button>().interactable = false;
            if (Curtran!= null && Curtran.TransName == tran)
            {
                return;
            }
            foreach (BaseSummon am in _Trans.Values) {
                if(am != null)
                    am.tranObj.SetActive(false);
            }
            if (!_Trans.Keys.Contains(tran) || _Trans[tran] == null)
            {
                GameObject prefab = null;
                GameObject panel = null;
                switch (tran)
                {
                    case "shuxing":
                       
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_summon_shuxing");// InterfaceMgr.doGetAssert(GAMEAPI.ABLayer_LoadNow_GameObject, "a3_summon_shuxing");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        _Trans[tran] = new a3_summon_shuxing(panel.transform, tran);
                        break;
                    case "xilian":
                        
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_summon_xilian");// ; InterfaceMgr.doGetAssert(GAMEAPI.ABLayer_LoadNow_GameObject, "a3_summon_xilian");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        _Trans[tran] = new a3_summon_xilian(panel.transform, tran);
                        break;
                    case "shouhun":
                       
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_summon_shouhun");// InterfaceMgr.doGetAssert(GAMEAPI.ABLayer_LoadNow_GameObject, "a3_summon_shouhun");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        _Trans[tran] = new a3_summon_shouhun(panel.transform, tran);
                        break;
                    case "ronghe":
                       
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_summon_ronghe"); //InterfaceMgr.doGetAssert(GAMEAPI.ABLayer_LoadNow_GameObject, "a3_summon_ronghe");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        _Trans[tran] = new a3_summon_ronghe(panel.transform, tran);
                        break;      
                    case "tunshi":
                       
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_summon_tunshi"); //InterfaceMgr.doGetAssert(GAMEAPI.ABLayer_LoadNow_GameObject, "a3_summon_tunshi");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        _Trans[tran] = new a3_summon_tunshi(panel.transform, tran);
                        break;
                    case "lianxie":
                        
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_summon_lianxie");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        _Trans[tran] = new a3_summon_lianxie(panel.transform, tran);
                        break;
                    case "tujian":
                       
                        prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_summon_tujian"); //InterfaceMgr.doGetAssert(GAMEAPI.ABLayer_LoadNow_GameObject, "a3_summon_tujian");
                        panel = GameObject.Instantiate(prefab) as GameObject;
                        _Trans[tran] = new a3_summon_tujian(panel.transform, tran);
                        break;
                }
                panel.transform.SetParent(contents, false);
            }
            Curtran?.onClose();
            Curtran = _Trans[tran];
            Curtran.refreCurSummonID();

            refresh_summon_list(-1, false ,  GetType(tran));
            if (tran == "tujian" && select_tujian > 0)
            {//图鉴设置初始选定召唤兽
                Curtran.select_tujian = select_tujian;
            }
            Curtran?.onShowed();
            Curtran?.gameObject.SetActive(true);
            if (Curtran.TransName != "tujian")
            {
                summonlist.gameObject.SetActive(true);
            }
            else {
                summonlist.gameObject.SetActive(false);
            }
        }

        public void refreshZHSlist_chuzhan()
        {
            foreach (uint idd in summonObj.Keys)
            {
                if (idd == A3_SummonModel.getInstance().nowShowAttackID)
                {
                    summonObj[idd].transform.FindChild("fighting").gameObject.SetActive(true);
                }
                else
                {
                    summonObj[idd].transform.FindChild("fighting").gameObject.SetActive(false);
                }
            }
        }

        void Update()
        {
            if (Curtran != null) Curtran._updata();
        }

        public GameObject GetSmallWin(string winName)
        {
            if (!smallWin.ContainsKey(winName))
            {
                GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject(winName);
                if (prefab == null) return null;
                GameObject panel = GameObject.Instantiate(prefab) as GameObject;
                smallWin[winName] = panel;
                panel.transform.SetParent(smallwinCon, false);
                if (Curtran != null) Curtran.onAddNewSmallWin(winName);
            }
            return smallWin[winName];
        }

    }
}
