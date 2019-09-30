using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
	class A3_LegionModel : ModelBase<A3_LegionModel>
	{
        //军团建设
        public int build_count = 0;      /*建设次数*/
        public int build_my_get = 0;     /*个人捐献*/
        public int build_clan_get = 0;   /*军团资金*/
        public Dictionary<int,int> build_awd = new Dictionary<int,int>();/* 奖励领取*/ 

        public List<A3_LegionData> finfname = new List<A3_LegionData>();

        public A3_LegionData myLegion = new A3_LegionData(); 
        public A3_LegionBuffData myLegionbuff = new A3_LegionBuffData();
        public A3_LegionBuffData myLegionbuff_cost = new A3_LegionBuffData();
        public List<A3_LegionData> list = new List<A3_LegionData>();
        public List<A3_LegionData> list2 = new List<A3_LegionData>();
        public Dictionary<int, A3_LegionMember> members = new Dictionary<int, A3_LegionMember>();
		public Dictionary<int, A3_LegionMember> applicant = new Dictionary<int, A3_LegionMember>();
		public Variant logdata ;
		public bool CanAutoApply;
		private SXML itemsXMl;
		public int create_needlv;
		public int create_needzhuan;
		public int create_needmoney;
        public int donate;

        public int change_needId;
        public int change_needNumber;
        public int change_needUser;

        internal Variant mydonate;

        public int showtype = -1;

        public A3_LegionModel(): base() {

			itemsXMl = XMLMgr.instance.GetSXML("clan");
			var cr = itemsXMl.GetNode("create");
			create_needzhuan = cr.getInt("zhuan");
			create_needlv = cr.getInt("lvl");
			create_needmoney = cr.getInt("money_cost");

            var rename = itemsXMl.GetNode("rename");
            change_needId = rename.getInt("item_id");
            change_needNumber = rename.getInt("value");
            change_needUser = rename.getInt("user");

        }

		public void AddMember(Variant data) {
            //debug.Log("kkkk"+data.dump());
			A3_LegionMember am = new A3_LegionMember();
			am.cid = data["cid"];
			am.donate = data["donate"];
			am.clanc = data["clanc"];
			am.name = data["name"];
			am.lvl = data["lvl"];
			am.zhuan = data["zhuan"];
			am.carr = data["carr"];
            am.combpt = data["combpt"];
            am.huoyue = data["active"];
			if (data.ContainsKey("lastlogoff")) {
				am.lastlogoff = data["lastlogoff"];
			}
			members[am.cid] = am;

            if (PlayerModel.getInstance().cid == am.cid)
            {
                if (am.clanc > 1)
                {                    
                   a3_legion.mInstance?.transform.FindChild("s4/tabs/application").gameObject.SetActive(true);
                }
                else
                {
                    a3_legion.mInstance?.transform.FindChild("s4/tabs/application").gameObject.SetActive(false);

                }
                donate = am.donate;
            }
            if (a3_legion_info.mInstance != null)
            {
                a3_legion_info.mInstance.buff_up();
            }
        }

		public void RefreshApplicant(Variant data) {
			Variant da = data["info"];
			applicant.Clear();
			foreach (var v in da._arr)
			{
				A3_LegionMember am = new A3_LegionMember();
				am.cid = v["cid"];
				am.name = v["name"];
				am.lvl = v["lvl"];
				am.zhuan = v["zhuan"];
				am.combpt = v["combpt"];
				am.carr = v["carr"];
				am.tm = v["tm"];
				applicant[am.cid] = am;
			}
		}

		public void AddLog(Variant data) {
			if(logdata==null){
				logdata = data;
				return;
			}
			Variant dd = data["clanlog_list"];
			foreach (Variant v in dd._arr)
			{
				logdata["clanlog_list"]._arr.Add(v);
			}
		}

		public static string GetCarr(int i) {
			string s = default(string);
			switch (i) {
				case 2:
					s = ContMgr.getCont("comm_job2");
					break;
				case 3:
					s = ContMgr.getCont("comm_job3");
                    break;
				case 5:
					s = ContMgr.getCont("comm_job5");
                    break;
			}
			return s;
		}


        public int legion_weihu(int clan_lv)
        {
            SXML repair = XMLMgr.instance.GetSXML("clan.clan_repair", "clan_lv==" + clan_lv);

            return repair.getInt("repair_money");

        }
        public void SetLegionBuff(int lvl)
        {
            if(myLegionbuff.buffs!=null)
             myLegionbuff.buffs.Clear();
            //if (lvl == 0) lvl=1;
            var dv_self = itemsXMl.GetNode("clan_buff", "lvl==" + lvl);
            var bf = dv_self?.GetNodeList("buff");
            myLegionbuff.buffs = new Dictionary<int, int>();
            foreach (var v in bf)
            {
                myLegionbuff.buffs[v.getInt("att_type")] = v.getInt("att_value");
            }
            myLegionbuff.cost_donate = dv_self.getInt("cost_donate");
            myLegionbuff.cost_item = dv_self.getInt("cost_item");
            myLegionbuff.cost_num = dv_self.getInt("cost_num");
            if(lvl<12)
            SetLegionBuff_cost(lvl + 1);
        }
        public void SetLegionBuff_cost(int lvl)
        {
            if (myLegionbuff_cost.buffs != null)
                myLegionbuff_cost.buffs.Clear();
            //if (lvl == 0) lvl=1;
            var dv_self = itemsXMl.GetNode("clan_buff", "lvl==" + lvl);
            var bf = dv_self?.GetNodeList("buff");
            myLegionbuff_cost.buffs = new Dictionary<int, int>();
            foreach (var v in bf)
            {
                myLegionbuff_cost.buffs[v.getInt("att_type")] = v.getInt("att_value");
            }
            myLegionbuff_cost.cost_donate = dv_self.getInt("cost_donate");
            myLegionbuff_cost.cost_item = dv_self.getInt("cost_item");
            myLegionbuff_cost.cost_num = dv_self.getInt("cost_num");

        }

        public void SetMyLegion(int lvl) {
			var dv = itemsXMl.GetNode("clan", "clan_lvl==" + lvl);
			myLegion.member_max = dv.getInt("member");
			myLegion.veteran = dv.getInt("veteran");
			myLegion.elite = dv.getInt("elite");
			myLegion.ordinary = dv.getInt("ordinary");
			myLegion.gold_cost = dv.getInt("money_cost");
			myLegion.exp_cost = dv.getInt("exp_cost");
            var cc = itemsXMl.GetNodeList("clan");
            myLegion.max_lvl = cc.Count;
        }

		public string GetClancToName(int clanc) {
            return ContMgr.getCont("legin_tag" + clanc);
        }

	}
    public class A3_LegionBuffData
    {
        public Dictionary<int, int> buffs;	//Buff
        public int cost_donate;
        public int cost_item;
        public int cost_num;



    }
	public class A3_LegionData
	{
		public int id;			            //id
		public string clname;	            //军团名
		public int lvl;			            //军团等级
		public string name;		            //首领名字
		public int plycnt;		            //人数
		public int combpt;		            //战斗力
		public string notice;	            //公告
		public int clan_pt;		            //军团经验
		public int anabasis_tm;	            //
		public int member_max;	            //最大成员数
		public int exp;			            //经验
		public int exp_cost;	            //升级所需经验
		public int gold;		            //资金
		public int gold_cost;	            //升级所需资金
		public Dictionary<int,int> buffs;	//Buff
		public int veteran;		            //元老数量
		public int elite;		            //精英数量
		public int ordinary;	            //普通成员数量
		public int rankidx;		            //排名
		public int ol_cnt;		            //在线人数
        public int max_lvl;                 //最大等级
        public int  direct_join;            //是否允许所有人加入
        public int huoyue;                  //军团活跃度；
        public int clanc;		            //玩家军团阶级
	}

	public class A3_LegionMember
	{
		public int cid;					//cid
		public int donate;				//捐赠
		public int clanc;
        public int huoyue;              //成员活跃度
		public string name;				//name
		public int lvl;					//等级
		public int zhuan;				//转生等级
		public int carr;				//职业
		public int combpt;				//战斗力
		public int tm;					//注册时间
		public int lastlogoff;			//最后在线时间
	}

	////logtp: 1职务变动；2升级；3捐献；4新成员加入；5离开；6踢出；7创建家族；8修改公告；9批准加入；10转让会长
	//public class A3_LegionLog
	//{

	//}
}
