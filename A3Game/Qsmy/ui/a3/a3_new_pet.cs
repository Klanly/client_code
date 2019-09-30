using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
using MuGame.role;

namespace MuGame
{
    class a3_new_pet : Window
    {
        public static a3_new_pet instance;
        int petNum = 0;
        List<SXML> list;
        SXML xml;
        GameObject iconTemp;
        GameObject goIcon;
        GameObject sliderGo;
        Transform cont;
        float sizeY;
        public int petid = 2;  
        //倒计时专用
        long year = 0;
        long month = 0;
        long day = 0;
        long hour = 0;
        long min = 0;
        long sen = 0;
        long all = 0;
        //==========
        public bool first = false;
        //private PetBird m_myPetBird = null;
        public long lastTime = 0;//饲料剩余时间
        public long addTime = 0;
        public long last_times = 0;
        public long feedPetTime;
        petGO petgo;
        Text timeGo;
        public bool openEveryLogin = false;
        public GameObject pet_avater;

        GameObject zdsq,
                   lxjy;

        GameObject usepet_btn;
        Text pet_name;
        public override void init()
        {
            #region 初始化汉字
            getComponentByPath<Text>("timeBuy/buyIt/Text").text = ContMgr.getCont("a3_new_pet_00");
            getComponentByPath<Text>("timeBuy/use/Text").text = ContMgr.getCont("a3_new_pet_0000");
            getComponentByPath<Text>("help/descTxt").text = ContMgr.getCont("a3_new_pet_11")+"\n" + ContMgr.getCont("a3_new_pet_1212")+"\n"+ ContMgr.getCont("a3_new_pet_1313")+"\n"+ ContMgr.getCont("a3_new_pet_1414");
            getComponentByPath<Text>("help/btn/Text").text = ContMgr.getCont("a3_new_pet_22");
            getComponentByPath<Text>("buyshow/scollBuy/content/buy0/free").text = ContMgr.getCont("a3_new_pet_33");
            getComponentByPath<Text>("buyshow/scollBuy/content/buy0/image/buy").text = ContMgr.getCont("a3_new_pet_44");
            getComponentByPath<Text>("buyshow/scollBuy/content/buy1/1").text = ContMgr.getCont("a3_new_pet_55");
            getComponentByPath<Text>("buyshow/scollBuy/content/buy1/image/buy").text = ContMgr.getCont("a3_new_pet_66");
            getComponentByPath<Text>("buyshow/scollBuy/content/buy2/3").text = ContMgr.getCont("a3_new_pet_77");
            getComponentByPath<Text>("buyshow/scollBuy/content/buy2/image/buy").text = ContMgr.getCont("a3_new_pet_88");
            getComponentByPath<Text>("zdsq/info").text = ContMgr.getCont("a3_new_pet_99");
            getComponentByPath<Text>("lxjy/info").text = ContMgr.getCont("a3_new_pet_1010");
            getComponentByPath<Text>("lxjy/info2").text = ContMgr.getCont("a3_new_pet_1111");


            getComponentByPath<Text>("buyshow/scollBuy/content/buy3/image/buy").text = ContMgr.getCont("a3_new_pet_89");
            #endregion




            instance = this;
            pet_name = getComponentByPath<Text>("pet_name/Text");
            usepet_btn = getGameObjectByPath("timeBuy/use");
            zdsq = getGameObjectByPath("zdsq");
            lxjy = getGameObjectByPath("lxjy");
            new BaseButton(getTransformByPath("btn_zdsq")).onClick = (GameObject go) =>{zdsq.SetActive(true);};
            new BaseButton(getTransformByPath("btn_lxjy")).onClick = (GameObject go) => { lxjy.SetActive(true); };
            BaseButton lxjyclose = new BaseButton(getTransformByPath("lxjy/close"));
            BaseButton lxjyclose1 = new BaseButton(getTransformByPath("lxjy/close1"));
            lxjyclose.onClick = lxjyclose1.onClick = (GameObject go) => { lxjy.SetActive(false); };
            BaseButton zdsqclose = new BaseButton(getTransformByPath("zdsq/close"));
            BaseButton zdsqclose1 = new BaseButton(getTransformByPath("zdsq/close1"));
            zdsqclose.onClick = zdsqclose1.onClick = (GameObject go) => { zdsq.SetActive(false); };
            //if (PlayerModel.getInstance().last_time == -1)
            //{
            //    lastTime = -1;
            //    getGameObjectByPath("timeBuy/buyIt").SetActive(false);
            //}
            if (PlayerModel.getInstance().last_time <= 0)
            {
                lastTime = 0;
                usepet_btn.SetActive(false);
            }
            else if (PlayerModel.getInstance().last_time > 0)
            {
                usepet_btn.SetActive(true);
                lastTime = PlayerModel.getInstance().last_time - muNetCleint.instance.CurServerTimeStamp;//当前饲料剩余时间
            }

            first = PlayerModel.getInstance().first;
            debug.Log("first:" + first.ToString() + "  last_time:" + lastTime);
            petgo = new petGO();
            timeGo = transform.FindChild("timeBuy/Text").GetComponent<Text>();
            sliderGo = getGameObjectByPath("timeBuy/expbar/slider");
            if (first)
            {
                getGameObjectByPath("buyshow/scollBuy/content/buy0").SetActive(true);
            }
            else
                getGameObjectByPath("buyshow/scollBuy/content/buy0").SetActive(false);

            //A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, getlastTime);
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_PET, getPet);
            //A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_SHOW_PET, showPet);
            //A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_FEED_PET, feedPet);
            //A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_HAVE_PET, havePet);
            //if (lastTime <= -1)
            //{
            //    lastTime = -1;
            //    getGameObjectByPath("timeBuy/buyIt").SetActive(false);
            //}

            #region button
            new BaseButton(getTransformByPath("title/btn_close")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_NEW_PET);
            };
            BaseButton touchClose = new BaseButton(getTransformByPath("buyshow/touchClose"));
            touchClose.onClick = close;
            new BaseButton(getTransformByPath("showSth/help")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("help").SetActive(true);
            };
            new BaseButton(getTransformByPath("help/btn")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("help").SetActive(false);
            };
            //金币钻石绑钻按钮
            new BaseButton(getTransformByPath("top/jingbi/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EXCHANGE);
                if (a3_exchange.Instance != null)
                    a3_exchange.Instance.transform.SetAsLastSibling();
            };
            new BaseButton(getTransformByPath("top/zuanshi/Image")).onClick = (GameObject go) =>
            {
                toback = true;
                InterfaceMgr.getInstance().close(this.uiName );
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RECHARGE);
            };
            new BaseButton(getTransformByPath("top/bangzuan/Image")).onClick = (GameObject go) =>
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_HONOR);
                if (a3_honor.instan != null)
                    a3_honor.instan.transform.SetAsLastSibling();
            };
            //===============
            //购买饲料
            new BaseButton(getTransformByPath("timeBuy/buyIt")).onClick = (GameObject go) =>
            {
                //if (all <= -1)
                //{
                //    flytxt.instance.fly(ContMgr.getCont("a3_new_pet_long"));
                //}
                //else
                    getGameObjectByPath("buyshow").SetActive(true);
            };
            //免费一天-----1\2\永久
            new BaseButton(getTransformByPath("buyshow/scollBuy/content/buy0/image")).onClick = (GameObject go) =>
            {
                A3_PetProxy.getInstance().SendTime(1);
                getGameObjectByPath("buyshow").SetActive(false);
                getGameObjectByPath("buyshow/scollBuy/content/buy0").SetActive(false);
            };
            new BaseButton(getTransformByPath("buyshow/scollBuy/content/buy1/image")).onClick = (GameObject go) =>
            {
                A3_PetProxy.getInstance().SendTime(2);
                getGameObjectByPath("buyshow").SetActive(false);
            };
            new BaseButton(getTransformByPath("buyshow/scollBuy/content/buy2/image")).onClick = (GameObject go) =>
            {
                A3_PetProxy.getInstance().SendTime(3);
                getGameObjectByPath("buyshow").SetActive(false);
            };
            new BaseButton(getTransformByPath("buyshow/scollBuy/content/buy3/image")).onClick = (GameObject go) =>
            {
                getGameObjectByPath("buyshow").SetActive(false);
                A3_PetProxy.getInstance().SendTime(4);
                List<SXML> buyList = XMLMgr.instance.GetSXMLList("newpet.buy.b", "id==" + 4);
                int zuan = buyList[0].getInt("zuan");
                //if (zuan <= PlayerModel.getInstance().gold)
                //    getGameObjectByPath("timeBuy/buyIt").SetActive(false);

            };
            //===============
            #endregion
            iconTemp = getGameObjectByPath("icon_temp");
            xml = XMLMgr.instance.GetSXML("newpet");
            list = xml.GetNodeList("pet");
            petNum = list.Count;
            cont = getTransformByPath("scoll/cont");
            sizeY = cont.GetComponent<GridLayoutGroup>().cellSize.y;
            cont.GetComponent<RectTransform>().sizeDelta = new Vector2(130f, sizeY * petNum);
            transform.FindChild("ig_bg_bg").gameObject.SetActive(false);

            getEventTrigerByPath("rotate").onDrag = OnDrag;

            #region 宠物icon按钮
            A3_PetModel petmodel = A3_PetModel.getInstance();
            getTransformByPath("shuxing/exp/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_1") + list[0].getInt("exp") + "%";
            getTransformByPath("shuxing/gold/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_2") + list[0].getInt("gold") + "%";
            getTransformByPath("shuxing/equip/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_3") + list[0].getInt("arm") + "%";
            getTransformByPath("shuxing/mate/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_4") + list[0].getInt("mat") + "%";

            foreach (var item in list)
            {
                goIcon = Instantiate(iconTemp) as GameObject;
                goIcon.transform.SetParent(getTransformByPath("scoll/cont"));
                goIcon.name = item.getString("pet_id");
                goIcon.SetActive(true);
                goIcon.transform.localScale = Vector3.one;
                String str = "scoll/cont/" + goIcon.name + "/icon_bg/icon";
                transform.FindChild("scoll/cont/" + list[0].getString("pet_id") + "/icon_bg/image_on").gameObject.SetActive(true);
                string names = item.getString("mod");
               // if (names == "jiexi")
                  //  names = "yingwu";
                getTransformByPath(str).GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_pet_" + names);
                new BaseButton(transform.FindChild("scoll/cont/" + goIcon.name + "/icon_bg/icon")).onClick = (GameObject go) =>
                {
                    chosenpetBtnOnClick(go,false);
                };
                new BaseButton(transform.FindChild("scoll/cont/" + goIcon.name + "/lock")).onClick = (GameObject go) =>
                {
                    chosenpetBtnOnClick(go, true);
                };
            }
            timeSet(lastTime);
            CancelInvoke("time");
            InvokeRepeating("time", 0, 1);
            if (A3_PetModel.curPetid != 0)
            {
                transform.FindChild("scoll/cont/" + A3_PetModel.curPetid + "/icon_bg/yuxuan_on").gameObject.SetActive(true);
            }
            #endregion
            getPet0();
            if (a3_everydayLogin.instans != null && a3_everydayLogin.instans.open)
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_EVERYDAYLOGIN);
                openEveryLogin = true;
            }
            
            new BaseButton(usepet_btn.transform).onClick = (GameObject go) =>
            {

                if (A3_PetModel.curPetid == old_petid)
                {
                    flytxt.instance.fly(ContMgr.getCont("pet_this_old"));
                    return;
                }
                for(int i=0; i<getTransformByPath("scoll/cont").childCount;i++)
                {
                    if(int.Parse(getTransformByPath("scoll/cont").GetChild(i).gameObject.name)== petid)
                        getTransformByPath("scoll/cont").GetChild(i).FindChild("icon_bg/yuxuan_on").gameObject.SetActive(true);
                    else
                        getTransformByPath("scoll/cont").GetChild(i).FindChild("icon_bg/yuxuan_on").gameObject.SetActive(false);
                }
                    A3_PetProxy.getInstance().SendPetId(petid);//发送换宠物
            };
        }

        int old_petid = -1;
        void chosenpetBtnOnClick(GameObject go, bool islock)
        {
           
            if (all <= 0)
                usepet_btn.SetActive(false);
            else
              usepet_btn.SetActive(islock?false:true);
            
            //if(islock)
            //{
            //    flytxt.instance.fly(ContMgr.getCont("a3_new_pet_nolock"));
            //    return;
            //} 

            string name=go.name=="lock" ? go.transform.parent.name :go.transform.parent.parent.name;
            for (int i = 0; i < list.Count; i++)
            {
                transform.FindChild("scoll/cont/" + list[i].getString("pet_id") + "/icon_bg/image_on").gameObject.SetActive(true);
                if (name == list[i].getString("pet_id"))
                {
                    int exp = list[i].getInt("exp");
                    int gold = list[i].getInt("gold");
                    int arm = list[i].getInt("arm");
                    int mat = list[i].getInt("mat");
                    petid = int.Parse(name);
                    //switch (name)
                    //{
                    //    case "eagle": petid = 2; break;
                    //    case "yingwu": petid = 5; break;
                    //    case "yaque": petid = 3; break;
                    //    default:
                    //        break;
                    //}
                    getTransformByPath("shuxing/exp/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_1") + exp + "%";
                    getTransformByPath("shuxing/gold/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_2") + gold + "%";
                    getTransformByPath("shuxing/equip/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_3") + arm + "%";
                    getTransformByPath("shuxing/mate/Text").GetComponent<Text>().text = ContMgr.getCont("a3_new_pet_4") + mat + "%";

                    //transform.FindChild("scoll/cont/" + name + "/icon_bg/image_on").gameObject.SetActive(true);
                   // A3_PetProxy.getInstance().SendPetId(petid);//发送换宠物
                }
                else
                {
                    transform.FindChild("scoll/cont/" + list[i].getString("pet_id") + "/icon_bg/image_on").gameObject.SetActive(false);
                   // transform.FindChild("scoll/cont/" + list[i].getString("pet_id") + "/icon_bg/yuxuan_on").gameObject.SetActive(false);

                }
            }
            petgo.createAvatar();
            petgo.creatPetAvatar(petid);
            pet_name.text = XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + petid).getString("name");
            old_petid = petid;
            debug.Log(feedPetTime + ":::::::::::::");
            //即时换宠物
            //if ((feedPetTime == 0 || feedPetTime - muNetCleint.instance.CurServerTimeStamp < 1) && lastTime == 0)
            //{
            //    if (feedPetTime == -1)
            //    {
            //        SelfRole._inst.ChangePetAvatar(petid, 0);
            //    }
            //    else
            //        flytxt.instance.fly(ContMgr.getCont("pet_buy_feed"));
            //}
            //else
            //    SelfRole._inst.ChangePetAvatar(petid, 0);//即时换宠物
        }

        public bool toback = false;
        #region top变化
        public void refreshMoney()
        {
            Text money = transform.FindChild("top/jingbi/image/num").GetComponent<Text>();
            money.text = Globle.getBigText(PlayerModel.getInstance().money);
        }
        public void refreshGold()
        {
            Text gold = transform.FindChild("top/zuanshi/image/num").GetComponent<Text>();
            gold.text = PlayerModel.getInstance().gold.ToString();
        }
        public void refreshGift()
        {
            Text gift = transform.FindChild("top/bangzuan/image/num").GetComponent<Text>();
            gift.text = PlayerModel.getInstance().gift.ToString();
        }
        void onMoneyChange(GameEvent e)
        {
            Variant info = e.data;
            if (info.ContainsKey("money"))
            {
                refreshMoney();
            }
            if (info.ContainsKey("yb"))
            {
                refreshGold();
            }
            if (info.ContainsKey("bndyb"))
            {
                refreshGift();
            }
        }
        #endregion

        void OnDrag(GameObject go, Vector2 delta)
        {
            if (pet_avater != null)
            {
                pet_avater.transform.Rotate(Vector3.up, -delta.x);
            }
        }
        void showPet(GameEvent e)
        {
            SelfRole._inst.ChangePetAvatar((int)A3_PetModel.curPetid, 0);
        }
        void feedPet(GameEvent e)
        {
            feedPetTime = e.data["pet_food_last_time"];
        }
        void getlastTime(GameEvent e)
        {

            addTime = e.data["pet_food_last_time"] - muNetCleint.instance.CurServerTimeStamp;//总的剩余时间
            //if (e.data["pet_food_last_time"] == -1)
            //{
            //    addTime = -1;
            //}
            //if (addTime == -1)
            //{
            //    timeGo.text = ContMgr.getCont("pet_forever");
            //}
            if (e.data.ContainsKey("first_pet_food"))
            {
                first = e.data["first_pet_food"];
            }
            timeSet(addTime);//重新计算时间
            for (int i = 0; i < getTransformByPath("scoll/cont").childCount; i++)
            {
                if (int.Parse(getTransformByPath("scoll/cont").GetChild(i).gameObject.name) == A3_PetModel.curPetid)
                    getTransformByPath("scoll/cont").GetChild(i).FindChild("icon_bg/yuxuan_on").gameObject.SetActive(true);
                else
                    getTransformByPath("scoll/cont").GetChild(i).FindChild("icon_bg/yuxuan_on").gameObject.SetActive(false);
            }
        }
        void getPet(GameEvent e)//修改宠物是否可用
        {
            A3_PetModel.getInstance().petId = e.data["pet"]["id_arr"]._arr;
            for (int i = 0; i < A3_PetModel.getInstance().petId.Count; i++)
            {
                int petGo = A3_PetModel.getInstance().petId[i];
                string str = "";
                str= XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + petGo).getString("mod");
                //switch (petGo)
                //{
                //    case 2: str = "eagle"; break;
                //    case 5: str ="yingwu"; break;
                //    case 3: str = "yaque"; break;
                //    default:
                //        break;
                //}
                transform.FindChild("scoll/cont/" + petGo + "/lock").gameObject.SetActive(false);
                transform.FindChild("scoll/cont/" + petGo + "/lock/image_lock").gameObject.SetActive(false);
            }
        }

        public   void havePet(GameEvent e)
        {
            last_times = 0;
            all = 0;
            CancelInvoke("time");
            InvokeRepeating("time", 0, 1);
        }
        void getPet0()//修改宠物是否可用
        {
            if (A3_PetModel.getInstance().petId == null)
            {
                return;
            }
            for (int i = 0; i < A3_PetModel.getInstance().petId.Count; i++)
            {
                int petGo = A3_PetModel.getInstance().petId[i];
                String str = "";
                str = XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + petGo).getString("mod");
                //switch (petGo)
                //{
                //    case 2: str = "eagle"; break;
                //    case 5: str = "yingwu"; break;
                //    case 3: str = "yaque"; break;
                //    default:
                //        break;
                //}
                transform.FindChild("scoll/cont/" + petGo + "/lock").gameObject.SetActive(false);
                transform.FindChild("scoll/cont/" + petGo + "/lock/image_lock").gameObject.SetActive(false);
            }
        }
        public override void onShowed()
        {
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, getlastTime);
            //A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_GET_PET, getPet);
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_SHOW_PET, showPet);
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_FEED_PET, feedPet);
            A3_PetProxy.getInstance().addEventListener(A3_PetProxy.EVENT_HAVE_PET, havePet);
            toback = false;
            UIClient.instance.addEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            refreshMoney();
            refreshGold();
            refreshGift();
            petgo.createAvatar();
            petgo.creatPetAvatar(petid);
            petgo.canshow = true;
            if (A3_PetModel.showbuy)
                getGameObjectByPath("buyshow").SetActive(true);
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_FUNCTIONBAR);
            GRMap.GAME_CAMERA.SetActive(false);

            List<int> lst = new List<int>();
            lst.Clear();
            for (int i = 0; i < A3_PetModel.getInstance().petId.Count; i++)
            {
                lst.Add(A3_PetModel.getInstance().petId[i]);
            }
            if (lst.Contains(petid))
            {
                chosenpetBtnOnClick(transform.FindChild("scoll/cont/" + petid + "/icon_bg/icon").gameObject, false);
            }
            else
            {
                chosenpetBtnOnClick(transform.FindChild("scoll/cont/" + petid + "/lock").gameObject, true);
            }


            
              UiEventCenter.getInstance().onWinOpen(uiName);

        }
        //当前使用宠物显示
        void showNowPet()
        {
            for(int i=0;i<transform.Find("scoll/cont").transform.childCount;i++)
            {
              //  if(transform.Find("scoll/cont").transform.GetChild(i).transform.FindChild())
            }
        }
        public override void onClosed()
        {
            A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_GET_LAST_TIME, getlastTime);
            //A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_GET_PET, getPet);
            A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_SHOW_PET, showPet);
            A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_FEED_PET, feedPet);
            A3_PetProxy.getInstance().removeEventListener(A3_PetProxy.EVENT_HAVE_PET, havePet);
            UIClient.instance.removeEventListener(UI_EVENT.ON_MONEY_CHANGE, onMoneyChange);
            A3_PetModel.showbuy = false;
            petgo.canshow = false;
            petgo.disposeAvatar();
            InterfaceMgr.getInstance().changeState(InterfaceMgr.STATE_NORMAL);
            GRMap.GAME_CAMERA.SetActive(true);
            if (openEveryLogin)
            {
                welfareProxy.getInstance()._isShowEveryDataLogin = false;
                welfareProxy.getInstance().sendWelfare(welfareProxy.ActiveType.selfWelfareInfo);
                openEveryLogin = false;
            }
        }
        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="add"></param>
        void timeSet(long add)//添加时间和剩余时间
        {
            last_times = add;
            debug.Log("宠物饲料总时间" + last_times.ToString() + "秒");
            year = last_times / (3600 * 24 * 30 * 12);
            month = last_times / (3600 * 24 * 30) - year * 12;
            day = last_times / (3600 * 24) - month * 30 - year * 12 * 30;
            hour = last_times / 3600 - day * 24 - month * 30 * 24 - year * 12 * 30 * 24;
            min = last_times / 60 - hour * 60 - day * 24 * 60 - month * 30 * 24 * 60 - year * 12 * 30 * 24 * 60;
            sen = last_times - min * 60 - hour * 3600 - day * 24 * 3600 - month * 30 * 24 * 3600 - year * 12 * 30 * 24 * 3600;
            all = last_times;
            //if (add <=0 /*|| lastTime <=0*/)
            //{
            //    all = 0;
            //}
            CancelInvoke("time");
            InvokeRepeating("time", 0, 1);
        }

        void time()
        {
            if (year <= 0)
            {
                year = 0;
            }
            if (month <= 0 && day <= 0 && hour <= 0 && min <= 0 && sen <= 0)
            {
                if (year > 0)
                {
                    month = 11;
                    day = 29; hour = 23; min = 59; sen = 59;
                    year--;
                }
                else
                    month = 0;

            }
            if (day <= 0 && hour <= 0 && min <= 0 && sen <= 0)
            {
                if (month > 0)
                {
                    day = 29; hour = 23; min = 59; sen = 59;
                    month--;
                }
                else
                    day = 0;

            }
            if (hour <= 0 && min <= 0 && sen <= 0)
            {
                if (day > 0)
                {
                    hour = 23; min = 59; sen = 59;
                    day--;
                }
                else
                    hour = 0;

            }
            if (min <= 0 && sen <= 0)
            {
                if (hour > 0)
                {
                    min = 59; sen = 59;
                    hour--;
                }
                else
                    min = 0;

            }

            if (sen <= 0)
            {
                if (min > 0)
                {
                    sen = 59;
                    min--;
                }
                else
                    sen = 0;

            }


            if (all <= 0)
            {
                timeGo.text = ContMgr.getCont("pet_invalid");
                all = 0;
                usepet_btn.SetActive(false);
                for (int i = 0; i < getTransformByPath("scoll/cont").childCount; i++)
                {
                   getTransformByPath("scoll/cont").GetChild(i).FindChild("icon_bg/yuxuan_on").gameObject.SetActive(false);
                }
                CancelInvoke("time");
            }
            //else if (all == -1)
            //{
            //    timeGo.text = ContMgr.getCont("pet_forever");
            //}
            else
            {
                // timeGo.text = "剩余时间:" + year + "年" + month + "月" + day + "天" + hour + "时" + min + "分" + sen + "秒";
                timeGo.text = ContMgr.getCont("a3_new_pet_time", new List<string>() { year.ToString(), month.ToString(), day.ToString(), hour.ToString(), min.ToString(), sen.ToString() });
                sen--;           
                all--;
            }


        }
        //===========================
        void close(GameObject go)
        {
            getGameObjectByPath("buyshow").SetActive(false);
        }
    }
    class petGO : MonsterRole
    {
        //private PetBird m_myPetBird = null;
        //private PetBird m_myPetBird = null;
        private GameObject m_SelfObj;
        private GameObject scene_Camera;
        private GameObject scene_Obj;
        //GameObject pathPrefab;
        //GameObject path = null;
        GameObject bird = null;
        public bool canshow = true;
        public void creatPetAvatar(int carr)
        {
            if (m_SelfObj != null)
            {
                GameObject.Destroy(m_SelfObj.gameObject);
                m_SelfObj = null;
            }
            Transform stop = SelfRole._inst.m_curModel.FindChild("birdstop");
            ProfessionAvatar m_proAvatar = new ProfessionAvatar();
            string str = "";
            GameObject birdPrefab;
            str = XMLMgr.instance.GetSXML("newpet.pet", "pet_id==" + carr).getString("mod");
            //switch (carr)
            //{
            //    case 2: str = "eagle"; break;
            //    case 5: str = "yingwu"; break;
            //    case 3: str = "yaque"; break;
            //    default:
            //        break;
            //}
            birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_" + str);

            m_SelfObj = GameObject.Instantiate(birdPrefab, new Vector3(-76.38f, 0.3f, 14.934f), new Quaternion(0, 180, 0, 0)) as GameObject;
            bird = m_SelfObj;
            a3_new_pet.instance.pet_avater = m_SelfObj;
            foreach (Transform tran in m_SelfObj.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;// 更改物体的Layer层
            }
            Transform cur_model = m_SelfObj.transform.FindChild("model");

            if (birdPrefab == null /*|| pathPrefab == null*/)
                return;

            if (bird == null /*|| path == null*/)
                return;
            bird.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            //m_myPetBird = bird.AddComponent<PetBird>();
            //m_myPetBird.Path = path;

        }
        public void createAvatar()
        {
            if (m_SelfObj == null)
            {
                GameObject obj_prefab;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_scene_ui_camera");
                scene_Camera = GameObject.Instantiate(obj_prefab) as GameObject;
                obj_prefab = GAMEAPI.ABFight_LoadPrefab("model_avatar_ui_petShow_scene");
                scene_Obj = GameObject.Instantiate(obj_prefab, new Vector3(-77.38f, -0.49f, 15.1f), new Quaternion(0, 180, 0, 0)) as GameObject;

                foreach (Transform tran in scene_Obj.GetComponentsInChildren<Transform>())
                {
                    if (tran.gameObject.name == "scene_ta")
                        tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
                    else
                        tran.gameObject.layer = EnumLayer.LM_FX;// 更改物体的Layer层
                }
            }
        }
        public void disposeAvatar()
        {
            if (m_SelfObj != null) GameObject.Destroy(m_SelfObj);
            if (scene_Obj != null) GameObject.Destroy(scene_Obj);
            if (scene_Camera != null) GameObject.Destroy(scene_Camera);
        }
    }
}