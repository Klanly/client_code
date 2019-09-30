using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MuGame
{
    class ResetLvLAwardModel : ModelBase<ResetLvLAwardModel>
    {
        List<ResetLvLAwardData> resetLvLAwardList;
        public ResetLvLAwardModel()
            : base()
        {

        }
        public List<ResetLvLAwardData> getAwardListById(uint carr)
        {
            SXML xmlItem = XMLMgr.instance.GetSXML("carrlvl_item.item", "id==" + carr.ToString());
            if (xmlItem == null) return null;

            resetLvLAwardList = new List<ResetLvLAwardData>();
            resetLvLAwardList.Clear();
           
          
            List<SXML> xmlAwardList = xmlItem.GetNodeList("award", null);
            for (int i = 0; i < xmlAwardList.Count; i++)
            {
                    ResetLvLAwardData rlad = new ResetLvLAwardData();
                    rlad.name = xmlAwardList[i].getString("item_name");
                    rlad.icon = xmlAwardList[i].getString("icon_file");
                    resetLvLAwardList.Add(rlad);
              
            }
            return resetLvLAwardList;
        }
    }
    class ResetLvLAwardData
    {
        public string name;
        public string icon;
    }
}
