using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;

using Object = UnityEngine.Object;

namespace MuGame
{
    class a3_mail : Window
    {
        private const int clickWaitMSec = 3;
        public static int expshowid;
        private readonly List<A3_MailSimple> sortedList = new List<A3_MailSimple>();
        private readonly Dictionary<uint, GameObject> title_gos = new Dictionary<uint, GameObject>();
        private int read_id = -1;
        private Text mailtitle;
        private Text mailcontent;
        private Text fajianren2;
        private Text time2;
        private Text hint;
        private Text tx_infos;
        private Button del1;
        private Button get;
        private Button del2;
        private GridLayoutGroup itmGrid;
        private Text cnt;
        private GridLayoutGroup coinGrid;
        public Vector3 containPos = new Vector3();
        public override void init()
        {
            inText();
            BaseButton closeBtn = new BaseButton(getTransformByPath("closeBtn")) { onClick = OnClose };

            cnt = getComponentByPath<Text>("left/cnt");
            mailtitle = getComponentByPath<Text>("right1/mailtitle");
            mailcontent = getComponentByPath<Text>("right1/mailcontent");
            fajianren2 = getComponentByPath<Text>("right1/fajianren2");
            time2 = getComponentByPath<Text>("right1/time2");
            hint = getComponentByPath<Text>("right1/hint");
            del1 = getComponentByPath<Button>("right2/del1");
            get = getComponentByPath<Button>("right2/get");
            del2 = getComponentByPath<Button>("right2/del2");
            itmGrid = getComponentByPath<GridLayoutGroup>("right2/scroll_view/contain");
            coinGrid = getComponentByPath<GridLayoutGroup>("right2/grid");
            containPos = getTransformByPath("right2/scroll_view/contain").localPosition;
            tx_infos = getComponentByPath<Text>("right2/tx_infos");

            get.onClick.AddListener(OnGetBtnClick);
            del1.onClick.AddListener(OnRemoveClick);
            del2.onClick.AddListener(OnRemoveClick);

            getComponentByPath<Button>("left/allget").onClick.AddListener(OnAllGetClick);
            getComponentByPath<Button>("left/alldel").onClick.AddListener(OnDeleteAllClick);

        }


        void inText()
        {
            this.transform.FindChild("left/alldel/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_1");//全部删除
            this.transform.FindChild("left/allget/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_2");//全部领取
            this.transform.FindChild("right2/del2/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_3");//删除
            this.transform.FindChild("right2/del1/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_3");//删除
            this.transform.FindChild("right2/get/Text").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_4");//领取
            this.transform.FindChild("right1/zhuti").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_5");//主题：
            this.transform.FindChild("right1/mailtitle").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_6");//邮件主题
            this.transform.FindChild("right1/fajianren").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_7");//发件人：
            this.transform.FindChild("right1/time1").GetComponent<Text>().text = ContMgr.getCont("uilayer_a3_mail_8");//日期：
        }
        public override void onShowed()
        {
            if (a3_expbar.instance)
                a3_expbar.instance.RemoveLightTip("mail");
            A3_MailProxy.getInstance().addEventListener(A3_MailProxy.MAIL_NEW_MAIL, OnGetNewMail);
            A3_MailProxy.getInstance().addEventListener(A3_MailProxy.MAIL_NEW_MAIL_CONTENT, OnGetMailContent);
            A3_MailProxy.getInstance().addEventListener(A3_MailProxy.MAIL_GET_ATTACHMENT, OnGetAttachment);
            A3_MailProxy.getInstance().addEventListener(A3_MailProxy.MAIL_REMOVE_ONE, OnRemoveOne);
            A3_MailProxy.getInstance().addEventListener(A3_MailProxy.MAIL_GET_ALL, OnGetAll);
            A3_MailProxy.getInstance().addEventListener(A3_MailProxy.MAIL_DELETE_ALL, OnDeleteAll);
            RefreshMailList();
            if (sortedList != null && sortedList.Count > 0)
            {
                OnMailTitleClick(title_gos[sortedList[0].id]);
            }
            else
            {
                RefreshMailContent(read_id);
            }
            if (uiData != null && uiData.Count > 0)
            {
                OnMailTitleClick(title_gos[(uint)expshowid]);
            }
            else
            {
                RefreshMailContent(read_id);
            }
            GRMap.GAME_CAMERA.SetActive(false);
        }

        public override void onClosed()
        {
            A3_MailProxy.getInstance().removeEventListener(A3_MailProxy.MAIL_NEW_MAIL, OnGetNewMail);
            A3_MailProxy.getInstance().removeEventListener(A3_MailProxy.MAIL_NEW_MAIL_CONTENT, OnGetMailContent);
            A3_MailProxy.getInstance().removeEventListener(A3_MailProxy.MAIL_GET_ATTACHMENT, OnGetAttachment);
            A3_MailProxy.getInstance().removeEventListener(A3_MailProxy.MAIL_REMOVE_ONE, OnRemoveOne);
            A3_MailProxy.getInstance().removeEventListener(A3_MailProxy.MAIL_GET_ALL, OnGetAll);
            A3_MailProxy.getInstance().removeEventListener(A3_MailProxy.MAIL_DELETE_ALL, OnDeleteAll);
            GRMap.GAME_CAMERA.SetActive(true);
        }

        private void OnClose(GameObject go)
        {
            a3_expbar.instance.HideMailHint();
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_MAIL);

            var etor = title_gos.GetEnumerator();
            while (etor.MoveNext())
            {
                Destroy(etor.Current.Value);
            }
        }

        private void SortMailList()
        {
            sortedList.Clear();

            A3_MailModel mm = A3_MailModel.getInstance();
            foreach (var mail in mm.mail_simple)
            {
                sortedList.Add(mail.Value);
            }
            int count = sortedList.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (sortedList[i].tm < sortedList[j].tm)
                    {
                        A3_MailSimple temp = sortedList[i];
                        sortedList[i] = sortedList[j];
                        sortedList[j] = temp;
                    }
                }
            }
        }

        private void RefreshMailList()
        {
            SortMailList();

            GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_mail_title");

            GameObject listCon = transform.FindChild("left/scroll_view/contain").gameObject;

            listCon.transform.DetachChildren();
            for (int i = 0; i < sortedList.Count; i++)
            {
                uint id = sortedList[i].id;
                GameObject go = null;
                title_gos.TryGetValue(id, out go);
                if (go == null)
                {
                    go = GameObject.Instantiate(prefab) as GameObject;
                    title_gos[id] = go;

                    BaseButton btn = new BaseButton(go.transform);
                    btn.onClick = OnMailTitleClick;
                }
                go.transform.localScale = new Vector3(1, 1, 1);
                go.name = id.ToString();
                go.transform.SetParent(listCon.transform, false);
                RefreshMailTitle(id);
            }
            RefreshMailCnt();


        }

        private void RefreshMailCnt()
        {
            cnt.text = ContMgr.getCont("mail_cnt", new List<string> { A3_MailModel.getInstance().mail_simple.Count.ToString() });

        }

        private void RefreshMailTitle(uint id)//更新邮件的标题栏
        {
            GameObject go = null;
            title_gos.TryGetValue(id, out go);
            if (go == null)
                return;

            Image icon = go.transform.FindChild("icon").GetComponent<Image>();
            Image icon03 = go.transform.FindChild("icon").GetComponent<Image>();
            Text title = go.transform.FindChild("title").GetComponent<Text>();
            Text from = go.transform.FindChild("from").GetComponent<Text>();
            
            A3_MailSimple mdata = A3_MailModel.getInstance().mail_simple[id];
            if (mdata.has_itm)
            {
                if (mdata.got_itm)
                {//!--有附件已领
                    icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_itemborder_b039_03");
                    icon03.sprite = GAMEAPI.ABUI_LoadSprite("icon_mail_mail_sign2");
                }
                else
                {//!--有附件未领
                    icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_itemborder_b039_04");
                    icon03.sprite = GAMEAPI.ABUI_LoadSprite("icon_mail_mail_sign1");
                }
            }
            else
            {//!--无附件
                icon.sprite = GAMEAPI.ABUI_LoadSprite("icon_itemborder_b039_01");
            }

            if (mdata.flag)
            {//!--已读
                title.text = "<color=#808080>" + mdata.title + "</color>";
                from.text = "<color=#808080>" + mdata.tp + "</color>";
            }
            else
            {//!--未读
                title.text = "<color=#FFFFFF>" + mdata.title + "</color>";
                from.text = "<color=#FFFFFF>" + mdata.tp + "</color>";
            }
        }

        private void RefreshMailContent(int id)//更新邮件的信息
        {
            mailtitle.gameObject.SetActive(false);
            mailcontent.gameObject.SetActive(false);
            fajianren2.gameObject.SetActive(false);
            time2.gameObject.SetActive(false);
            hint.gameObject.SetActive(false);
            del1.gameObject.SetActive(false);
            get.gameObject.SetActive(false);
            del2.gameObject.SetActive(false);
            tx_infos.gameObject.SetActive(false);
            for (int i = 0; i < itmGrid.transform.childCount; i++)
            {
                Destroy(itmGrid.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < coinGrid.transform.childCount; i++)//创造网格
            {
                Destroy(coinGrid.transform.GetChild(i).gameObject);
            }

            if (A3_MailModel.getInstance().mail_simple.Count == 0)
            {
                hint.gameObject.SetActive(true);
                hint.text = ContMgr.getCont("mail_hint_1");
                return;
            }

            if (id == -1 || !A3_MailModel.getInstance().mail_details.ContainsKey((uint)id))
            {//未选中邮件或邮件数据不存在
                hint.gameObject.SetActive(true);
                hint.text = ContMgr.getCont("mail_hint_2");
            }
            else
            {
                A3_MailDetail mdetail = A3_MailModel.getInstance().mail_details[(uint)id];

                mailtitle.gameObject.SetActive(true);
                mailtitle.text = mdetail.ms.title;

                fajianren2.gameObject.SetActive(true);
                fajianren2.text = mdetail.ms.tp;

                time2.gameObject.SetActive(true);
                string ts = MuGame.Globle.getStrTime((int)mdetail.ms.tm);
                time2.text = ts;

                mailcontent.gameObject.SetActive(true);
                mailcontent.text = mdetail.msg;

                if (!mdetail.ms.has_itm)
                {//!--没有附件
                    del2.gameObject.SetActive(true);
                }
                else
                {//!--有附件
                    GameObject icon;
                    if (mdetail.money != 0)
                    {
                        CreateCoinIcon("coin1", mdetail.money);
                    }
                    if (mdetail.yb != 0)
                    {
                        CreateCoinIcon("coin2", mdetail.yb);
                    }
                    if (mdetail.bndyb != 0)
                    {
                        CreateCoinIcon("coin3", mdetail.bndyb);
                    }
                    for (int i = 0; i < mdetail.itms.Count; i++)
                    {
                        a3_BagItemData data = mdetail.itms[i];
                        icon = IconImageMgr.getInstance().createA3ItemIcon(data, true, data.num);
                        icon.transform.SetParent(itmGrid.transform, false);
                        if (data.num <= 1)
                            icon.transform.FindChild("num").gameObject.SetActive(false);

                        BaseButton bs_bt = new BaseButton(icon.transform);
                        bs_bt.onClick = delegate (GameObject go) { this.onMailItemClick(data); };
                    }

                    if (mdetail.ms.got_itm)
                    {//!--已领取
                        del2.gameObject.SetActive(true);
                        tx_infos.gameObject.SetActive(true);
                        tx_infos.text = ContMgr.getCont("mail_hint_0");
                        for (int i = 0; i < itmGrid.transform.childCount; i++)
                        {
                            Destroy(itmGrid.transform.GetChild(i).gameObject);
                        }
                        for (int i = 0; i < coinGrid.transform.childCount; i++)//创造网格
                        {
                            Destroy(coinGrid.transform.GetChild(i).gameObject);
                        }
                    }
                    else
                    {//!--未领取
                        del1.gameObject.SetActive(true);
                        get.gameObject.SetActive(true);
                    }
                }
            }

        }

        void onMailItemClick(a3_BagItemData itmdata)//点击邮件项目
        {
            if (itmdata.isEquip)
            {
                ArrayList data = new ArrayList();
                data.Add(itmdata);
                data.Add(equip_tip_type.Comon_tip);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EQUIPTIP, data);
            }
            else if (itmdata.isSummon)
            {
                ArrayList data = new ArrayList();
                data.Add(itmdata);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3TIPS_SUMMON, data);
            }
            else
            {
                ArrayList data = new ArrayList();
                data.Add(itmdata);
                data.Add(equip_tip_type.Comon_tip);
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_ITEMTIP, data);
            }
        }

        private void OnMailTitleClick(GameObject evt_go)//点击邮件列表
        {
            GameObject go;
            if (read_id != -1)
            {
                go = null;
                title_gos.TryGetValue((uint)read_id, out go);
                if (go != null)
                {
                    go.transform.FindChild("select").gameObject.SetActive(false);
                }
            }

            read_id = int.Parse(evt_go.name);
            go = null;
            title_gos.TryGetValue((uint)read_id, out go);
            if (go != null)
            {
                go.transform.FindChild("select").gameObject.SetActive(true);
                getTransformByPath("right2/scroll_view/contain").localPosition =containPos;
            }

            if (A3_MailModel.getInstance().mail_details.ContainsKey((uint)read_id))
            {
                RefreshMailContent(read_id);
            }
            else
            {
                A3_MailProxy.getInstance().GetMailContent((uint)read_id);
            }

        }

        long lastClick = 0;
        private void OnGetBtnClick()
        {
            if (muNetCleint.instance.CurServerTimeStampMS - lastClick > clickWaitMSec)
            {
                A3_MailProxy.getInstance().GetMailAttachment((uint)read_id);
                lastClick = muNetCleint.instance.CurServerTimeStampMS;
            }
        }

        private void OnAllGetClick()
        {
            A3_MailProxy.getInstance().GetAllAttachment();
        }

        private void OnDeleteAllClick()
        {
            if (A3_MailModel.getInstance().HasItemInMails())
                MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("mail_hint_3"), ConfirmDeleteAll);
            else
                ConfirmDeleteAll();
        }

        private void ConfirmDeleteAll()
        {
            A3_MailProxy.getInstance().DeleteAll();
        }

        private void OnRemoveClick()
        {
            if (A3_MailModel.getInstance().HasItemInMail((uint)read_id))
                MsgBoxMgr.getInstance().showConfirm(ContMgr.getCont("mail_hint_4"), ConfirmDelete);
            else
                ConfirmDelete();

        }

        private void ConfirmDelete()
        {
            A3_MailProxy.getInstance().RemoveMail((uint)read_id);
        }

        private void OnGetNewMail(GameEvent e)
        {
            RefreshMailList();
        }

        private void OnGetMailContent(GameEvent e)
        {
            uint mailid = (uint)e.orgdata;
            RefreshMailContent((int)mailid);
            RefreshMailTitle(mailid);
        }

        private void OnGetAttachment(GameEvent e)
        {
            flytxt.instance.fly(ContMgr.getCont("mail_hint_5"));
            uint mailid = (uint)e.orgdata;
            RefreshMailContent((int)mailid);
            RefreshMailTitle(mailid);

            A3_MailDetail mdetail = A3_MailModel.getInstance().mail_details[(uint)mailid];
            for ( int i = 0 ; i < mdetail.itms.Count ; i++ )
            {
                a3_BagItemData data = mdetail.itms[i];
                flytxt.instance.fly( a3_BagModel.getInstance().getItemDataById( data.tpid ).item_name +"*" + (data.num<=1?1:data.num));
            }
          
        }

        private void OnRemoveOne(GameEvent e)
        {
            uint mailid = (uint)e.orgdata;

            GameObject go = null;
            title_gos.TryGetValue(mailid, out go);
            if (go != null && read_id == mailid)
            {
                title_gos.Remove(mailid);
                Destroy(go);
                RefreshMailContent(read_id);
            }
            RefreshMailCnt();

            uint nextid = 0;
            if (mailid == sortedList[sortedList.Count - 1].id)
            {
                sortedList.Remove(sortedList[sortedList.Count - 1]);
                if (sortedList != null && sortedList.Count > 0)
                {
                    nextid = sortedList[0].id;
                }
                else
                {
                    RefreshMailContent(read_id);
                }
            }

            else
            {
                for (int i = 0; i < sortedList.Count; i++)
                {
                    if (mailid == sortedList[i].id)
                    {
                        nextid = sortedList[i + 1].id;

                        sortedList.RemoveAt(i);
                        break;
                    }
                }
            }
            GameObject go1 = null;
            title_gos.TryGetValue(nextid, out go1);
            if (go1 != null)
            {
                OnMailTitleClick(go1);
            }
            flytxt.instance.fly(ContMgr.getCont("mail_hint_6"));

        }

        private void OnGetAll(GameEvent e)
        {
            RefreshMailList();
            RefreshMailContent(read_id);
        }

        private void OnDeleteAll(GameEvent e)
        {
            Variant data = e.data;
            Variant get_ids = data["ids"];
            if (get_ids._arr.Count > 0)
            {
                flytxt.instance.fly(ContMgr.getCont("mail_hint_9"));
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("mail_hint_8"));
            }

            RefreshMailList();
            RefreshMailContent(read_id);
        }

        private void CreateCoinIcon(string coin_icon, uint num)
        {
            GameObject prefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_mail_coin");
            GameObject go = GameObject.Instantiate(prefab) as GameObject;go.transform.localScale = Vector3.one;
            Image image = go.transform.FindChild("icon").GetComponent<Image>();
            image.sprite = GAMEAPI.ABUI_LoadSprite("icon_coin_" + coin_icon);
            Text number = go.transform.FindChild("num").GetComponent<Text>();
            number.text = num.ToString();
            
            go.transform.SetParent(coinGrid.transform);
            go.transform.localScale = Vector3.one;
        }
    }
}
