using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_teamMemberList:Window
    {
        public static a3_teamMemberList _instance;
        Transform itemMemberPrefab;
        static Transform contant;
        List<itemTeamMember> itemTeamMemberList;
        public  Dictionary<uint, Sprite> iconSpriteDic;                                           
        override public void init()
        {
            _instance = this;
            itemTeamMemberList = new List<itemTeamMember>();
            BaseButton btnClose = new BaseButton(transform.FindChild("main/title/btnClose"));
            btnClose.onClick = onBtnCloseClick;
            itemMemberPrefab = transform.FindChild("main/itemPrefab/itemTeamMember");
            contant = transform.FindChild("main/body/main/Scroll/content");
            BaseButton btnJoin = new BaseButton(transform.FindChild("main/bottom/btnJoin"));
            btnJoin.onClick = onBtnJoinClick;
            getProfessionSprite();



            getComponentByPath<Text>("main/title/bg/Text").text = ContMgr.getCont("a3_teamMemberList_0");
            getComponentByPath<Text>("main/body/title/txtName").text = ContMgr.getCont("a3_teamMemberList_1");
            getComponentByPath<Text>("main/body/title/txtLvl").text = ContMgr.getCont("a3_teamMemberList_2");
            getComponentByPath<Text>("main/body/title/txtKnightage").text = ContMgr.getCont("a3_teamMemberList_3");
            getComponentByPath<Text>("main/body/title/txtMap").text = ContMgr.getCont("a3_teamMemberList_4");
            getComponentByPath<Text>("main/bottom/btnJoin/Text").text = ContMgr.getCont("a3_teamMemberList_5");

        }

        override public void onShowed()
        {
            if (uiData != null)
            {
                itemTeamMemberList.Clear();

                List<ItemTeamData> itdList =(List<ItemTeamData>) uiData[0];
                for (int i = 0; i < itdList.Count; i++)
                {
                    itemTeamMember itm = new itemTeamMember(itemMemberPrefab);
                    itm.Show(itdList[i]);
                    itemTeamMemberList.Add(itm);
                }
            }
          
        }
        void onBtnCloseClick(GameObject g)
        {
            for (int i = 0; i < itemTeamMemberList.Count;i++ )
            {
                Destroy(itemTeamMemberList[i].root.gameObject);
            }
            itemTeamMemberList.Clear();
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_TEAMMEMBERLIST);
        }
        void onBtnJoinClick(GameObject go)
        {
            uint lv = PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl;
            if (lv >= TeamProxy.WatchTeamId_limited)
            {
                TeamProxy.getInstance().SendApplyJoinTeam(TeamProxy.wantedWatchTeamId);
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_TEAMMEMBERLIST);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("a3_team_nolv"));
            }
        }
        void getProfessionSprite()
        {
            iconSpriteDic = new Dictionary<uint, Sprite>();
            for (int i = 2; i <= 5;i++ )
            {
              iconSpriteDic.Add((uint)i, GAMEAPI.ABUI_LoadSprite(getProfession(i)));
            }
        }
        string getProfession(int profession)
        {
            string file = string.Empty;
            switch (profession)
            {
                case 1:
                    break;
                case 2:
                    file = "icon_job_icon_h2";
                    break;
                case 3:
                    file = "icon_job_icon_h3";
                    break;
                case 4:
                    file = "icon_job_icon_h4";
                    break;
                case 5:
                    file = "icon_job_icon_h5";
                    break;
            }
            return file;
        }

        override public void onClosed()
        {
            for (int i = 0; i < itemTeamMemberList.Count;i++ )
            {
                GameObject.Destroy(itemTeamMemberList[i].root.gameObject);
            }
            itemTeamMemberList.Clear();
        }
        class itemTeamMember
        {
           public Transform root;
            public itemTeamMember(Transform trans) { Init(trans); }
            Text txtName;
            Text txtLvl;
            Text txtKnightage;
            Text txtMap;
            Image iconCaptain;
            Toggle toggle;
            Image iconCarr;
            void Init(Transform trans)
            {
                GameObject go = GameObject.Instantiate(trans.gameObject) as GameObject;
                root = go.transform;
                txtName = root.FindChild("texts/txtName").GetComponent<Text>();
                txtLvl = root.FindChild("texts/txtLvl").GetComponent<Text>();
                txtKnightage = root.FindChild("texts/txtKnightage").GetComponent<Text>();
                txtMap = root.FindChild("texts/txtMap").GetComponent<Text>();
                iconCaptain = root.FindChild("texts/txtName/icon").GetComponent<Image>();
                iconCarr = root.FindChild("texts/txtName/iconCarr").GetComponent<Image>();
                toggle = root.FindChild("Toggle").GetComponent<Toggle>();

            }

            public void Show(ItemTeamData itd)
            {
                root.SetParent(a3_teamMemberList.contant);
                ToggleGroup tg = a3_teamMemberList.contant.GetComponent<ToggleGroup>();
                toggle.GetComponent<Toggle>().group = tg;
                root.localScale = Vector3.one;
                root.SetAsLastSibling();
                txtName.text = itd.name;
                // txtLvl.text = itd.zhuan + "转" + itd.lvl + "级";
                txtLvl.text = ContMgr.getCont("worldmap_lv", new List<string>{ itd.zhuan.ToString(), itd.lvl.ToString()});
                txtKnightage.text = itd.knightage;
                txtMap.text = SvrMapConfig.instance.getSingleMapConf((uint)itd.mapId) == null ? "" : SvrMapConfig.instance.getSingleMapConf((uint)itd.mapId)["map_name"]._str;
                iconCaptain.gameObject.SetActive(itd.isCaptain);//= itd.iconCaptain;
                iconCarr.sprite = a3_teamMemberList._instance.iconSpriteDic[itd.carr];
                root.gameObject.SetActive(true);
            }
        }
    }
}
