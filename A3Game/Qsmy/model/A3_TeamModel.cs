using System;
using System.Collections.Generic;
using System.Text;
using Cross;
using UnityEngine;
using UnityEngine.UI;
namespace MuGame
{
  
    public class A3_TeamModel : ModelBase<A3_TeamModel>
    {
      //  public List<TeamPosition> teamlist_position = new List<TeamPosition>();

        public ItemTeamMemberData AffirmInviteData;
        public ItemTeamData NewMemberJoinData;//新队员加入的数据
        public Dictionary<uint, string> cidName = new Dictionary<uint, string>();//组队副本队伍中的cid和name;
        public Dictionary<uint, string> cidNameElse = new Dictionary<uint, string>();//其他目标队伍中的cid和name;
        public bool bein=false;
        public uint ltpids;
        public string getProfessional(uint carr)
        {
            string str = string.Empty;
            if(ContMgr.getCont("profession" + carr)!=null)
                   str = ContMgr.getCont("profession" + carr);
            //switch (carr)
            //{
            //    case 1:
            //        str = "全职业";                
            //        break;
            //    case 2:
            //        str = "战士";
            //        break;
            //    case 3:
            //        str = "法师";
            //        break;
            //    case 5:
            //        str = "刺客";
            //        break;
            //}
            return str;
        }
        public bool Limit_Change_Teammubiao(int obj)
        {

            int i=0 ;
            switch (obj)
            {
                case 0: i = 5; break;
                case 1: i = 4; break;
                case 2: i = 1; break;
                case 3: i = 6; break;
                case 4: i = 7; break;
                case 5: i = 3; break;
                case 6: i = 2; break;

            }            

            int dengji = (int)(PlayerModel.getInstance().up_lvl * 100 + PlayerModel.getInstance().lvl);
            SXML xml = XMLMgr.instance.GetSXML("func_open.team_lv_limit", "id==" + i);
            int limit_dj = xml.getInt("zhuan") * 100 + xml.getInt("lv");

            if (dengji >= limit_dj)
                return true;
            else
                return false;

        }


    }


    public class ItemTeamMemberData
    {
        public ItemTeamMemberData()
        {
            itemTeamDataList = new List<ItemTeamData>();
        }
        public uint leaderCid;
        public bool cofirmed;
        public uint teamId;
        public uint totalCount;
        public uint idxBegin;
        public bool dirJoin;
        public bool membInv;
        public bool meIsCaptain;
        public uint removedIndex;
        public uint ltpid;
        public uint ldiff;
        public List<ItemTeamData> itemTeamDataList=new List<ItemTeamData>();
        public bool IsInMyTeam(string name)
        {
            for (int i = 0; i < itemTeamDataList.Count; i++)
                if (itemTeamDataList[i].name.Equals(name)) return true;
            return false;
        }
        public bool IsInMyTeam(uint cid)
        {
            for (int i = 0; i < itemTeamDataList.Count; i++)
                if (itemTeamDataList[i].cid.Equals(cid)) return true;
            return false;
        }
        public bool GetIndexOfMember(uint cid,out int index)
        {
            index = -1;            
            for (int i = 0; i < itemTeamDataList.Count; i++)
                if (itemTeamDataList[i].cid == cid)
                {
                    index = i;
                    return true;
                }
            return false;
        }
    }
    public class ItemTeamData
       {
           public string name;
           public uint carr;
           public uint zhuan;
           public uint lvl;
           public string knightage;
           public uint mapId;
           public uint curcnt;
           public int MembCount;
           public uint cid;
           public Image iconCaptain;
           public uint teamId;
           public bool isCaptain;
           public int combpt;//战斗力
           public bool online;
           public uint maxcnt;
           public bool showRemoveMemberBtn;
           public uint hp;
           public uint maxHp;
           public uint ltpid;
           public uint ldiff;
           public List<Variant> members;   
       }

    public class TeamPosition
    {
      
        public uint cid;
        public uint  x;
        public uint  y;
       
    }
   
}
