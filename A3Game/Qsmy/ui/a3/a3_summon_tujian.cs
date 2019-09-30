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
    class a3_summon_tujian : BaseSummon
    {
        SXML itemXml;
        Transform sumView;
        int curStar = 0;
        int curSumid = 0;
        Transform btns;
        //Transform selectframe;
        public static a3_summon_tujian instans;

        Dictionary<int, GameObject> sumsobj = new Dictionary<int, GameObject>();
        public a3_summon_tujian(Transform trans, string name) : base(trans, name)
        {
            init();
        }
        ScrollControler scrollControler;
        void init()
        {


            tranObj.transform.FindChild("btns/1/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_0");
            tranObj.transform.FindChild("btns/2/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_1");
            tranObj.transform.FindChild("btns/3/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_2");
            tranObj.transform.FindChild("btns/4/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_3");
            tranObj.transform.FindChild("btns/5/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_4");
            tranObj.transform.FindChild("need/GetNeed_1/todo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_6");
            tranObj.transform.FindChild("need/GetNeed_1/name").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_5");
            tranObj.transform.FindChild("need/GetNeed_2/name").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_5");
            tranObj.transform.FindChild("need/GetNeed_2/todo/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_tujian_6");
            tranObj.transform.FindChild("info/minjie/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_2");
            tranObj.transform.FindChild("info/tili/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_3");
            tranObj.transform.FindChild("info/gongji/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_4");
            tranObj.transform.FindChild("info/fangyu/Text").GetComponent<Text>().text = ContMgr.getCont("a3_summon_xilian_5");



            instans = this;
            itemXml = XMLMgr.instance.GetSXML("item");
            sumView = tranObj.transform.FindChild("summonlist/summons/scroll/content");
            btns = tranObj.transform.FindChild("btns");
            //selectframe = tranObj.transform.FindChild("summonlist/summons/scroll/frame");
            getEventTrigerByPath("tach").onDrag = OnDrag;
            for (int i =0;i< btns.childCount;i++) {
                new BaseButton(btns.GetChild(i)).onClick = (GameObject go) => {
                    setBtn(int.Parse (go.name));
                };
            }
            SetList();

            scrollControler = new ScrollControler();
            ScrollRect scroll = tranObj.transform.FindChild("summonlist/summons/scroll").GetComponent<ScrollRect>();
            scrollControler.create(scroll);
        }
        public override void onShowed() {
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_INSUMMON, onNewSum);
            A3_SummonProxy.getInstance().addEventListener(A3_SummonProxy.EVENT_SHOWIDENTIFYANSWER, onGetSum);
            if (select_tujian > 0) {
                SetFristSelect(select_tujian);
                select_tujian = -1;
            }
            else 
                SetFristSelect();
        }
        public override void onClose()
        {
           A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_INSUMMON, onNewSum);
            A3_SummonProxy.getInstance().removeEventListener(A3_SummonProxy.EVENT_SHOWIDENTIFYANSWER, onGetSum);
            SetDispose();
            curSumid = 0;
        }

        public override void onAddNewSmallWin(string name)
        {
            base.onAddNewSmallWin(name);
            switch (name)
            {
                case "uilayer_getsummon":
                    GameObject getTip = getSummonWin()?.GetSmallWin(name);
                    getTip.transform.FindChild("info/gongji/Text").GetComponent<Text>().text = ContMgr.getCont("getsummon_0");
                    getTip.transform.FindChild("info/fangyu/Text").GetComponent<Text>().text = ContMgr.getCont("getsummon_1");
                    getTip.transform.FindChild("info/minjie/Text").GetComponent<Text>().text = ContMgr.getCont("getsummon_2");
                    getTip.transform.FindChild("info/tili/Text").GetComponent<Text>().text = ContMgr.getCont("getsummon_3");
                    getTip.transform.FindChild("info/xingyun/Text").GetComponent<Text>().text = ContMgr.getCont("getsummon_4");
                    getTip.transform.FindChild("use/Text").GetComponent<Text>().text = ContMgr.getCont("getsummon_5");
                    getTip.transform.FindChild("putbag/Text").GetComponent<Text>().text = ContMgr.getCont("getsummon_6");
                    new BaseButton(getTip.transform.FindChild("tach")).onClick = (GameObject go) =>
                    {
                        getTip.SetActive(false);
                        if (getSummonWin().avator_look != null)
                        {
                            GameObject.Destroy(getSummonWin().avator_look);
                        }
                        if (getSummonWin().avatorobj && !getSummonWin().avatorobj.activeSelf)
                        {
                            getSummonWin().avatorobj.SetActive(true);
                        }
                    };
                    break;
            }
        }

        void setAttValue(int id ,int star) {
            if (curSumid <= 0 || curStar <= 0) return;
            SXML xml = sumXml.GetNode("callbeast", "id==" + id);
            SXML attxml = xml.GetNode("star", "star_sum==" + star);
            tranObj.transform.FindChild("info/gongji/value").GetComponent<Text>().text = attxml.GetNode("att").getInt("reset_min")+" - "+ attxml.GetNode("att").getInt("reset_max");
            tranObj.transform.FindChild("info/fangyu/value").GetComponent<Text>().text = attxml.GetNode("def").getInt("reset_min") + " - " + attxml.GetNode("def").getInt("reset_max");
            tranObj.transform.FindChild("info/minjie/value").GetComponent<Text>().text = attxml.GetNode("agi").getInt("reset_min") + " - " + attxml.GetNode("agi").getInt("reset_max");
            tranObj.transform.FindChild("info/tili/value").GetComponent<Text>().text = attxml.GetNode("con").getInt("reset_min") + " - " + attxml.GetNode("con").getInt("reset_max");
        }

        void setBtn(int num = -1) {
            for (int i = 0; i < btns.childCount; i++) {
                btns.GetChild(i).FindChild("b").gameObject.SetActive(false);
                btns.GetChild(i).GetComponent<Button>().interactable = true;
            }
            if (num < 0)
            {
                SXML xml = sumXml.GetNode("callbeast", "id==" + curSumid);
                num = xml.getInt("default_stars");
            }
            btns.GetChild(num - 1).transform.FindChild("b").gameObject.SetActive(true);
            btns.GetChild(num - 1).GetComponent<Button>().interactable = false;
            curStar = num;
            setAttValue(curSumid, curStar);
            setGetItem();
        }
        void SetList() {
            for(int i =0; i< sumView.childCount;i++)
            {
                GameObject.Destroy(sumView.GetChild (i).gameObject);
            }
            sumsobj.Clear();
            List<SXML> sums = sumXml.GetNodeList("callbeast");
            GameObject item = tranObj.transform.FindChild("summonlist/summons/scroll/0").gameObject;
            foreach (SXML it in sums) {
                GameObject clon = GameObject.Instantiate(item) as GameObject;
                clon.transform.SetParent(sumView,false);
                clon.SetActive(true);
                int tpid = it.getInt("id");
                clon.name = tpid.ToString();
                SXML s_xml = itemXml.GetNode("item", "id==" + tpid);
                string file = "icon_item_" + s_xml.getString("icon_file");
                clon.transform.FindChild ("icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                sumsobj[tpid] = clon;
                new BaseButton (clon.transform).onClick=(GameObject go)=>{
                    if (curSumid == tpid) return;
                    curSumid = tpid;
                    setAttValue(curSumid, curStar);
                    setAvator();
                    setGetItem();
                    setStarBtnCount();
                    setBtn();
                    setframe();
                };

            }
        }

        void SetFristSelect() {
            
            sumView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            if (curSumid == 0 && sumView.childCount >0)
            {//界面打开默认选中第一个
                curSumid = int.Parse ( sumView.GetChild(0).gameObject.name);
                setBtn();
                setAvator();
                setGetItem();
                setStarBtnCount();
                setAttValue(curSumid, curStar);
                setframe();
            }
        }
        void SetFristSelect(int selectid)
        {
            if (sumView.childCount > 0) {
                curSumid = selectid;
                setBtn();
                setAvator();
                setGetItem();
                setStarBtnCount();
                setAttValue(curSumid, curStar);
                setframe();
            }
            sumView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        void setframe(){
            foreach (int id in sumsobj.Keys)
            {
                if (id == curSumid)
                    sumsobj[id].transform.FindChild("frame").gameObject.SetActive(true);
                else
                    sumsobj[id].transform.FindChild("frame").gameObject.SetActive(false);
            }
        }
        void setAvator() {
            if (curSumid <= 0) return;
            SetDispose();
            SXML xml = sumXml.GetNode("callbeast", "id==" + curSumid);
            int mid = xml.getInt("mid");
            tranObj.transform.FindChild("name").GetComponent<Text>().text = xml.getString("name");
            SXML mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            int objid = mxml.getInt("obj");
            GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objid);
            float y = float.Parse(mxml.getString("smshow_pos"));
            getSummonWin().avatorobj = GameObject.Instantiate(obj_prefab, new Vector3(-153.541f, 0.778f+ y, 0f), Quaternion.identity) as GameObject;
            foreach (Transform tran in getSummonWin().avatorobj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            Transform cur_model = getSummonWin().avatorobj.transform.FindChild("model");
            var animm = cur_model.GetComponent<Animator>();
            animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            cur_model.Rotate(Vector3.up, 270 - mxml.getInt("smshow_face"));
            float scale = mxml.getFloat("smshow_scale");
            if (scale < 0) { scale = 0.5f; }
            cur_model.transform.localScale = new Vector3(scale, scale, scale);
            GameObject t_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_roleinfo_ui_camera");
            getSummonWin().camobj = GameObject.Instantiate(t_prefab) as GameObject;
            Camera cam = getSummonWin().camobj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                float r_size = cam.orthographicSize * 1920 / 1080 * Screen.height / Screen.width;
                cam.orthographicSize = r_size;
            }
        }

        void SetDispose()
        {
            if (getSummonWin().avatorobj != null) {
                GameObject.Destroy(getSummonWin().avatorobj);
                GameObject.Destroy(getSummonWin().camobj);
                getSummonWin().avatorobj = null;
                getSummonWin().camobj = null;
            }

            if (getSummonWin().avator_look != null) { GameObject.Destroy(getSummonWin().avator_look); }
        }

        void setGetItem() {
            if (curSumid <= 0 || curStar <= 0) return;
            SXML xml = sumXml.GetNode("callbeast", "id==" + curSumid);
            SXML attxml = xml.GetNode("star", "star_sum==" + curStar);
            int itemid = attxml.getInt ("get_type2_itm");
            int num = attxml.getInt("get_type2_num");
            if (itemid <= 0) { tranObj.transform.FindChild("need/GetNeed_1").gameObject.SetActive(false); }
            else {
                tranObj.transform.FindChild("need/GetNeed_1").gameObject.SetActive(true);
                a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)itemid);
                GameObject icon = IconImageMgr.getInstance().createA3ItemIcon(item);
                for (int i = 0; i < tranObj.transform.FindChild("need/GetNeed_1/itemicon").childCount; i++)
                {
                    GameObject.Destroy(tranObj.transform.FindChild("need/GetNeed_1/itemicon").GetChild(i).gameObject);
                }
                icon.transform.SetParent(tranObj.transform.FindChild("need/GetNeed_1/itemicon"), false);
                new BaseButton(icon.transform).onClick = (GameObject go) =>
                {
                    ArrayList arr = new ArrayList();
                    arr.Add((uint)itemid);
                    arr.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                };

                int haveCount = a3_BagModel.getInstance().getItemNumByTpid((uint)itemid);
                string str = "";
                if (haveCount >= num)
                    str = "<color=#00FF56FF>"+haveCount +"/"+ num + "</color>";
                else
                    str = "<color=#f90e0e>" + haveCount +"/"+ num + "</color>";
                tranObj.transform.FindChild("need/GetNeed_1/count").GetComponent<Text>().text = str;
                tranObj.transform.FindChild("need/GetNeed_1/name").GetComponent<Text>().text = item.item_name;
                new BaseButton(tranObj.transform.FindChild("need/GetNeed_1/todo")).onClick = (GameObject go) =>
                {
                    //碎片兑换
                    A3_SummonProxy.getInstance().sendGetsummon((uint)curSumid, (uint)curStar, 2);
                };
            }
           
            int num_item = attxml.getInt("get_type1");
            if (num_item <= 0)
                tranObj.transform.FindChild("need/GetNeed_2").gameObject.SetActive(false);
            else
            {
                int itemId = attxml.getInt("info_itm");
                tranObj.transform.FindChild("need/GetNeed_2").gameObject.SetActive(true);
                SXML s_xml = itemXml.GetNode("item", "id==" + itemId);
                string file = "icon_item_" + s_xml.getString("icon_file");
                tranObj.transform.FindChild("need/GetNeed_2/itemicon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file);
                string file_bg = "icon_itemborder_b039_0" + (curStar - 1);
                tranObj.transform.FindChild("need/GetNeed_2/bg").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite(file_bg);
                string str = "";
                int needitemId = xml.GetNode("star", "star_sum==" + (curStar - 1)).getInt ("info_itm");

                new BaseButton(tranObj.transform.FindChild("need/GetNeed_2/itemicon")).onClick = (GameObject go) =>
                {
                    ArrayList arr = new ArrayList();
                    arr.Add((uint)needitemId);
                    arr.Add(1);
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_MINITIP, arr);
                };
                SXML s_xml_need = itemXml.GetNode("item", "id==" + needitemId);
                int havecount =  a3_BagModel.getInstance().getItemNumByTpid((uint)needitemId);
                if (havecount >= num_item)
                    str = "<color=#00FF56FF>" + havecount + "/" + num_item + "</color>";
                else
                    str = "<color=#f90e0e>" + havecount + "/" + num_item + "</color>";

                tranObj.transform.FindChild("need/GetNeed_2/count").GetComponent<Text>().text = str;
                tranObj.transform.FindChild("need/GetNeed_2/name").GetComponent<Text>().text = s_xml_need.getString("item_name");
                new BaseButton(tranObj.transform.FindChild("need/GetNeed_2/todo")).onClick = (GameObject go) =>
                {
                    //召唤兽兑换
                    Dictionary<uint, a3_BagItemData> items = a3_BagModel.getInstance().getItems();
                    List<uint> sums = new List<uint>();
                    foreach (a3_BagItemData data in items.Values)
                    {
                        if (data.isSummon) {
                            if (data.tpid != curSumid) continue;
                            if (data.summondata.star == (curStar - 1))
                            {
                                sums.Add((uint)data.summondata.id);
                            }
                        }
                    }
                    if (sums.Count >= num_item)
                    {
                        List<uint> To_sums = new List<uint>();
                        for (int i = 0; i < num_item; i++)
                        {
                            To_sums.Add(sums[i]);
                        }
                        A3_SummonProxy.getInstance().sendGetsummon((uint)curSumid, (uint)curStar, 1, To_sums);
                    }
                    else {
                        flytxt.instance.fly(ContMgr.getCont("a3_summon_duihuandaojubuzu"));
                    }
                   
                };
            }
        }



        void OnDrag(GameObject go, Vector2 delta)
        {
            if (getSummonWin().avatorobj != null)
            {
                getSummonWin().avatorobj.transform.Rotate(Vector3.up, -delta.x);
            }
        }

        void setStarBtnCount()
        {
            SXML xml = sumXml.GetNode("callbeast", "id==" + curSumid);
            int minstar = xml.getInt("default_stars");
            Transform starCon = tranObj.transform.FindChild("btns");
            if (minstar <= 0) minstar = 1;
            for (int j = 0;j<5;j++)
            {
                starCon.GetChild(j).gameObject.SetActive(false);
            }
            for (int i = minstar; i <= 5;i++)
            {
                starCon.GetChild(i - 1).gameObject.SetActive(true);
            }
        }


       
        public void ongetsum(a3_BagItemData data)
        {
            GameObject plan = getSummonWin().GetSmallWin("uilayer_getsummon");
            plan.SetActive(true);
            setGetAvator(data.summondata.tpid);
            setinfo_look(data, plan);
            new BaseButton(plan.transform.FindChild("use")).onClick = (GameObject go) => 
            {
                if (A3_SummonModel.getInstance().GetSummons().Count >= A3_SummonModel.getInstance().allcount)
                {
                    flytxt.instance.fly(ContMgr.getCont("a3_summon_liebiaoyiman"));
                }
                else
                {
                    A3_SummonProxy.getInstance().sendUseSummon((uint)data.id);
                }
                plan.SetActive(false);
                if (getSummonWin().avator_look != null)
                {
                    GameObject.Destroy(getSummonWin().avator_look);
                }
                if (getSummonWin().avatorobj && !getSummonWin().avatorobj.activeSelf)
                {
                    getSummonWin().avatorobj.SetActive(true);
                }
            };
            new BaseButton(plan.transform.FindChild("putbag")).onClick = (GameObject go) =>
            {
                flytxt.instance.fly(ContMgr.getCont("a3_summon_putbag"));
                plan.SetActive(false);
                if (getSummonWin().avator_look != null)
                {
                    GameObject.Destroy(getSummonWin().avator_look);
                }
                if (getSummonWin().avatorobj && !getSummonWin().avatorobj.activeSelf)
                {
                    getSummonWin().avatorobj.SetActive(true);
                }
            };
        }
        void setGetAvator(int tpid)
        {
            if (getSummonWin().avatorobj != null) { getSummonWin().avatorobj.SetActive(false); }
            if (getSummonWin().avator_look != null) { GameObject.Destroy(getSummonWin().avator_look); }
            SXML xml = sumXml.GetNode("callbeast", "id==" + tpid);
            int mid = xml.getInt("mid");
            SXML mxml = XMLMgr.instance.GetSXML("monsters.monsters", "id==" + mid);
            int objid = mxml.getInt("obj");
            GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject("monster_" + objid);
            getSummonWin().avator_look = GameObject.Instantiate(obj_prefab, new Vector3(-153.81f, 0.778f, 0f), Quaternion.identity) as GameObject;
            foreach (Transform tran in getSummonWin().avator_look.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
            }
            Transform cur_model = getSummonWin().avator_look.transform.FindChild("model");
            var animm = cur_model.GetComponent<Animator>();
            animm.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            cur_model.gameObject.AddComponent<Summon_Base_Event>();
            cur_model.Rotate(Vector3.up, 270 - mxml.getInt("smshow_face"));
            float scale = mxml.getFloat("smshow_scale");
            if (scale < 0) { scale = 0.5f; }
            cur_model.transform.localScale = new Vector3(scale, scale, scale);
        }
        void setinfo_look(a3_BagItemData data,GameObject pre)
        {
            pre.transform.FindChild("name").GetComponent<Text>().text = data.summondata.name;
            Transform star = pre.transform.FindChild("stars");
            for (int i = 0; i < 5; i++)
            {
                star.GetChild(i).FindChild("b").gameObject.SetActive(false);
            }

            for (int j = 0; j < data.summondata.star; j++)
            {
                star.GetChild(j).FindChild("b").gameObject.SetActive(true);
            }
            SXML xml = sumXml.GetNode("callbeast", "id==" + data.summondata.tpid);
            SXML attxml = xml.GetNode("star", "star_sum==" + data.summondata.star);
            pre.transform.FindChild("info/gongji/value").GetComponent<Text>().text = data.summondata.attNatural + "/" + attxml.GetNode("att").getInt("reset_max");
            pre.transform.FindChild("info/fangyu/value").GetComponent<Text>().text = data.summondata.defNatural + "/" + attxml.GetNode("def").getInt("reset_max");
            pre.transform.FindChild("info/minjie/value").GetComponent<Text>().text = data.summondata.agiNatural + "/" + attxml.GetNode("agi").getInt("reset_max");
            pre.transform.FindChild("info/tili/value").GetComponent<Text>().text = data.summondata.conNatural + "/" + attxml.GetNode("con").getInt("reset_max");
            pre.transform.FindChild("info/xingyun/value").GetComponent<Text>().text = data.summondata.luck + "/" + 100;

            pre.transform.FindChild("info/gongji/slider").GetComponent<Image>().fillAmount = (float)data.summondata.attNatural / (float)attxml.GetNode("att").getInt("reset_max");
            pre.transform.FindChild("info/fangyu/slider").GetComponent<Image>().fillAmount = (float)data.summondata.defNatural / (float)attxml.GetNode("def").getInt("reset_max");
            pre.transform.FindChild("info/minjie/slider").GetComponent<Image>().fillAmount = (float)data.summondata.agiNatural / (float)attxml.GetNode("agi").getInt("reset_max");
            pre.transform.FindChild("info/tili/slider").GetComponent<Image>().fillAmount = (float)data.summondata.conNatural / (float)attxml.GetNode("con").getInt("reset_max");
            pre.transform.FindChild("info/xingyun/slider").GetComponent<Image>().fillAmount = (float)data.summondata.luck / (float)100;

            Transform SkillCon = pre.transform.FindChild("skills");
            for (int i = 0; i < SkillCon.childCount; i++)
            {
                SkillCon.GetChild(i).FindChild("icon/icon").gameObject.SetActive(false);
                SkillCon.GetChild(i).FindChild("lock").gameObject.SetActive(true);
            }
            int idner = 1;
            foreach (summonskill skill in data.summondata.skills.Values)
            {
                Transform skillCell = SkillCon.FindChild(idner.ToString());
                skillCell.FindChild("icon/icon").gameObject.SetActive(true);
                skillCell.FindChild("lock").gameObject.SetActive(false);
                SXML xx = XMLMgr.instance.GetSXML("skill.skill", "id==" + skill.skillid);
                skillCell.FindChild("icon/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_smskill_" + xx.getInt("icon"));
                idner++;
            }

        }

        void onNewSum(GameEvent v) {
            Variant data = v.data;
            if (data.ContainsKey ("summon")) {
                refreSumlist();
            }
        }

        void onGetSum(GameEvent v)
        {
            setGetItem();
        }

    }
}
