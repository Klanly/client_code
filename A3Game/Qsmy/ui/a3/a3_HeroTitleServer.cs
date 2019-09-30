using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class a3_HeroTitleServer : BaseProxy<a3_HeroTitleServer>
    {
        public static uint GET_TITLE = 1;
        public static uint SET_TITLE = 2;
        public static uint SET_SHOW_TITLE = 4;

        public List<TitleData> gainTitleIdList = new List<TitleData>();
        public int eqpTitleId = 0;
        public bool isShowTitle = true;
        public List<RoleTitleXmlData> roleTitleXmlData = new List<RoleTitleXmlData>();
        public Dictionary<int, RoleTitleXmlData> roleTitleData_Dic = new Dictionary<int, RoleTitleXmlData>();

        public static uint SET_UI_ICON = 3;

        public static uint LOCKE_TITLE = 5;

        public a3_HeroTitleServer()
        {
            addProxyListener(PKG_NAME.C2S_A3_HEROTITLE, AuctionOP);
            TitleData_Init();
        }

        public void AuctionOP(Variant data)
        {

            debug.Log("头衔:" + data.dump());
            int res = data["res"];
            switch (res)
            {
                case 1:

                    if (data.ContainsKey("title_list"))
                    {
                        foreach (Variant item in data["title_list"]._arr)
                        {
                            TitleData _titledata = new TitleData(item["title_id"]._uint, item["limit"]._uint, item["forever"]._bool);

                            gainTitleIdList.Add(_titledata);

                        };
                    }

                    if (data.ContainsKey("title_sign")) eqpTitleId = data["title_sign"]._int;

                    if (data.ContainsKey("title_sign_display")) isShowTitle = data["title_sign_display"]._bool;

                    break;

                case 2:

                    if (data.ContainsKey("title_sign")) eqpTitleId = data["title_sign"]._int;

                    if (data.ContainsKey("title_sign_display")) isShowTitle = data["title_sign_display"]._bool;

                    if (data.ContainsKey("lock_title"))
                    {
                        for (int i = 0; i < gainTitleIdList.Count; i++)
                        {
                            if (gainTitleIdList[i].title_id == data["lock_title"]._uint)
                            {
                                gainTitleIdList.RemoveAt(i);

                                break;
                            }
                        }

                        dispatchEvent(GameEvent.Create(LOCKE_TITLE, this, data));
                    }

                    dispatchEvent(GameEvent.Create(SET_UI_ICON, this, data));

                    break;

                case 3:

                    TitleData titledata = new TitleData(data["new_title_sign"]._uint, data["limit"]._uint, data["forever"]._bool);

                    gainTitleIdList.Add(titledata);

                    break;

                case 4:

                    if (data.ContainsKey("display")) {

                        isShowTitle = data["display"]._bool;

                        dispatchEvent(GameEvent.Create(SET_SHOW_TITLE, this, data));

                    } 

                    break;

                default:

                    Globle.err_output(res);

                    break;
            }

        }

        public void SendMsg(uint id, Variant data = null)
        {
            Variant msg = new Variant();
            msg["op"] = id;
            if (id == SET_TITLE)
            {
                msg["title_sign"] = data["title_sign"];
            }
            else if (id == SET_SHOW_TITLE)
            {
                msg["display"] = data["display"];
            }
            sendRPC(PKG_NAME.C2S_A3_HEROTITLE, msg);

        }
        private void TitleData_Init()
        {
            List<SXML> s_xml = XMLMgr.instance.GetSXMLList("title.titlelist", null);

            for (int i = 0; i < s_xml.Count; i++)
            {
                RoleTitleXmlData xmlData = new RoleTitleXmlData();
                xmlData.title_id = int.Parse(s_xml[i].getString("title_id"));
                xmlData.title_img = s_xml[i].getString("title_img");
                roleTitleData_Dic[xmlData.title_id] = xmlData;
            }
        }
    }

}
