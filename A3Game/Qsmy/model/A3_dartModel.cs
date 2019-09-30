using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class A3_dartModel: ModelBase<A3_dartModel>
    {
        Dictionary<int, clans> dicClan = new Dictionary<int, clans>();
        SXML ss = new SXML();
        List<SXML> listXml = new List<SXML>();
        int length;
        public uint item_id;
        public A3_dartModel():base()
        {
        }
        

        public void init(uint line)
        {
            ss = XMLMgr.instance.GetSXML("clan_escort");
            listXml = ss.GetNodeList("line");
            length = listXml.Count;

            clans cla = new clans();
            for (int i = 0; i < length; i++)
            {
                cla.open_lv_clan = listXml[i].getInt("clan_lvl");
                cla.pathid = listXml[i].getUint("path");
                cla.target_map = listXml[i].getUint("target_map");
                cla.add_money_num = listXml[i].getInt("clan_money");
                cla.item_id = listXml[i].getUint("item_id");
                cla.item_num = listXml[i].getInt("item_num");
                if (!dicClan.ContainsKey(listXml[i].getInt("id")))
                    dicClan.Add(listXml[i].getInt("id"), cla);
            }
            debug.Log(line.ToString()+"is line");
            item_id = dicClan[(int)line].item_id;
        }

    }
}
