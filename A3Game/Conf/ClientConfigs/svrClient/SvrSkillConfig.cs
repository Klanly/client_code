using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrSkillConfig : configParser
    {
        public SvrSkillConfig( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrSkillConfig(m as ClientConfig);
        }

		override protected Variant _formatConfig(Variant conf)
		{
			conf["skill"] = GameTools.array2Map( conf["skill"], "id" );
            conf["state"] = GameTools.array2Map( conf["state"], "id" );
            foreach (Variant skill in conf["skill"].Values)
            {
                if (skill.ContainsKey("lv"))
                {
                    skill["lv"] = GameTools.array2Map(skill["lv"], "lv");
                }
            }
			return conf;
		}
        public Variant get_state_desc(uint state_id)
		{
			return this.m_conf["state"][state_id.ToString()];
		}
        /**
		 *获得祝福配置。 
		 */
		public Variant get_bless_data(int blessid)
		{
            return this.m_conf["bstate"][blessid];
		}

        /**
		 *按照祝福的类型获得祝福对应数据
		 * @param stp 类型 
		 */
		public Variant get_bless_by_tp(int stp)
		{
			Variant so = new Variant();
            foreach(string i in m_conf["bstate"].Values)
			{
				so = m_conf["bstate"][i];			
				if( so && so.ContainsKey("eff") )
				{
					if(stp == so["eff"]["tp"])
					{
						return so;
					}
				}		
			}		
			return null;
		}

        /**
		 *按照祝福的编号获得祝福的类型
		 * @param bid 祝福编号 
		 */
		public int get_bless_tp_by_id(int bid)
		{
			Variant so = get_bless_data(bid);
			if( so && so.ContainsKey("eff") )
			{
				return so["eff"]["tp"]._int;
			}
			return 0;
		}

        public Variant get_skill_conf(uint skill_id)
		{
            Variant skill=this.m_conf["skill"][ skill_id.ToString()];
            if (skill != null)
            {
                if (!skill.ContainsKey("act_c"))
                    skill["lv"]["1"]["act_c"] = 0;
                if (!skill.ContainsKey("agry_c"))
                    skill["lv"]["1"]["agry_c"] = 0;
                if (!skill.ContainsKey("hp_c"))
                    skill["lv"]["1"]["hp_c"] = 0;
            }
            
			return skill;
		}
		
		public Variant get_skill_lv_desc(uint skill_id,uint skill_lvl)
		{
			Variant skill_desc = get_skill_conf( skill_id );		
			if( skill_desc )
			{
				return skill_desc["lv"][skill_lvl];
			}		
			return null;
		}

        //-------------------------------------------------------------- 解析 -------------------------------------------
		public Variant on_skill_data(ByteArray data)
		{
            //try
            //{
            //    this.m_conf = Package.inst.unserialize_obj(data);
            //    _calc_skill_lvl_data();
            //}
            //catch(par:*)
            //{
            //    DebugTrace.add(DebugTrace.DTT_ERR, "SvrSkillConfig on_skill_data err:" + par);			
            //}
			
			return true;
		}

        	//-------------------------------------------------------------
		private void _calc_skill_lvl_data()
		{
			// 填充默认属性值，计算每个等级的技能数据
			
			Variant skconfs = this.m_conf;
			foreach(string skid in skconfs["skill"].Keys)
			{
				Variant skilconf = skconfs["skill"][skid];
				if(!(skilconf.ContainsKey("lv")))
				{
					continue;
				}
				
				//debug.dbginfo("skilconf: "+skilconf.name+" before lv count["+skilconf.lv.len()+"]");
				
				Variant lv_dis_ary = new Variant();
                foreach(string lv in skconfs["lv"].Keys)
				{
					Variant lvc = skilconf["lv"][lv];
					int idx = 0;
					for(; idx < lv_dis_ary.Count; ++idx)
					{
						if(lv_dis_ary[idx] == null)
							continue;
						if(lvc["lv"] < lv_dis_ary[idx]["lv"])
						{	
							break;
						}
					}
                    //不知道如何翻
					//lv_dis_ary.splice(idx, 0, lvc);
				}
				
				Variant pre_lvconf = new Variant();
                foreach(string lv_dis_idx in lv_dis_ary.Keys)
				//for(var lv_dis_idx:String in lv_dis_ary)
				{
					Variant lvconf = lv_dis_ary[lv_dis_idx];
					if(pre_lvconf == null)
					{
						pre_lvconf = lvconf;
						continue;
					}
					
					int cur_lv = pre_lvconf["lv"] + 1;
					int lv_dist = lvconf["lv"] - pre_lvconf["lv"];
					//debug.dbginfo("skilconf: "+skilconf.name+" add lv["+pre_lvconf.lv+"] to lv["+lvconf.lv+"]\n");
					
					for(; cur_lv < lvconf["lv"]; ++cur_lv)
					{
						int cur_lv_dist = cur_lv - pre_lvconf["lv"];
						Variant cur_lv_conf = new Variant();//ObjectUtil.deepCloneSimpleObject(pre_lvconf);//不知道怎么翻

						//debug.dumpObject(pre_lvconf);
						//debug.dumpObject(cur_lv_conf);
						
						cur_lv_conf["lv"] = cur_lv;
						//debug.dbginfo("cur_lv_conf["+cur_lv_conf+"] pre_lvconf["+pre_lvconf+"] lvconf["+lvconf+"]\n");
						
						if(cur_lv_conf.ContainsKey("hp_c")) cur_lv_conf["hp_c"] = Convert.ToInt32((cur_lv_conf["hp_c"] + (lvconf["hp_c"] - cur_lv_conf["hp_c"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("mp_c")) cur_lv_conf["mp_c"] =Convert.ToInt32((cur_lv_conf["mp_c"] + (lvconf["mp_c"] - cur_lv_conf["mp_c"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("agry_c")) cur_lv_conf["agry_c"] = Convert.ToInt32((cur_lv_conf["agry_c"] + (lvconf["agry_c"] - cur_lv_conf["agry_c"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("act_c")) cur_lv_conf["act_c"] = Convert.ToInt32((cur_lv_conf["act_c"] + (lvconf["act_c"] - cur_lv_conf["act_c"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("cd")) cur_lv_conf["cd"] = Convert.ToInt32((cur_lv_conf["cd"] + (lvconf["cd"] - cur_lv_conf["cd"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("lvexp")) cur_lv_conf["lvexp"] = Convert.ToInt32((cur_lv_conf["lvexp"] + (lvconf["lvexp"] - cur_lv_conf["lvexp"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("gld_cost")) cur_lv_conf["gld_cost"] = Convert.ToInt32((cur_lv_conf["gld_cost"] + (lvconf["gld_cost"] - cur_lv_conf["gld_cost"]) * cur_lv_dist / lv_dist));	             
						if(cur_lv_conf.ContainsKey("plv")) cur_lv_conf["plv"] = Convert.ToInt32((cur_lv_conf["plv"] + (lvconf["plv"] - cur_lv_conf["plv"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("teleport")) cur_lv_conf["teleport"] = Convert.ToInt32((cur_lv_conf["teleport"] + (lvconf["teleport"] - cur_lv_conf["teleport"]) * cur_lv_dist / lv_dist));
						if(cur_lv_conf.ContainsKey("upitmcnt")) cur_lv_conf["upitmcnt"] = Convert.ToInt32((cur_lv_conf["upitmcnt"] + (lvconf["upitmcnt"] - cur_lv_conf["upitmcnt"]) * cur_lv_dist / lv_dist));
						
						if(cur_lv_conf.ContainsKey("sres"))
						{
							if(cur_lv_conf["sres"].ContainsKey("atk_dmg")) cur_lv_conf["sres"]["atk_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["atk_dmg"] + (lvconf["sres"]["atk_dmg"] - cur_lv_conf["sres"]["atk_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("matk_dmg")) cur_lv_conf["sres"]["matk_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["matk_dmg"] + (lvconf["sres"]["matk_dmg"] - cur_lv_conf["sres"]["matk_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("nag_dmg")) cur_lv_conf["sres"]["nag_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["nag_dmg"] + (lvconf["sres"]["nag_dmg"] - cur_lv_conf["sres"]["nag_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("pos_dmg")) cur_lv_conf["sres"]["pos_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["pos_dmg"] + (lvconf["sres"]["pos_dmg"] - cur_lv_conf["sres"]["pos_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("voi_dmg")) cur_lv_conf["sres"]["voi_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["voi_dmg"] + (lvconf["sres"]["voi_dmg"] - cur_lv_conf["sres"]["voi_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("poi_dmg")) cur_lv_conf["sres"]["poi_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["poi_dmg"] + (lvconf["sres"]["poi_dmg"] - cur_lv_conf["sres"]["poi_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("hp_dmg")) cur_lv_conf["sres"]["hp_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["hp_dmg"] + (lvconf["sres"]["hp_dmg"] - cur_lv_conf["sres"]["hp_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("mp_dmg" )) cur_lv_conf["sres"]["mp_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["mp_dmg"] + (lvconf["sres"]["mp_dmg"] - cur_lv_conf["sres"]["mp_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("agry_dmg")) cur_lv_conf["sres"]["agry_dmg"] = Convert.ToInt32((cur_lv_conf["sres"]["agry_dmg"] + (lvconf["sres"]["agry_dmg"] - cur_lv_conf["sres"]["agry_dmg"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("state_tm")) cur_lv_conf["sres"]["state_tm"] = Convert.ToInt32((cur_lv_conf["sres"]["state_tm"] + (lvconf["sres"]["state_tm"] - cur_lv_conf["sres"]["state_tm"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["sres"].ContainsKey("state_par")) cur_lv_conf["sres"]["state_par"] = Convert.ToInt32((cur_lv_conf["sres"]["state_par"] + (lvconf["sres"]["state_par"] - cur_lv_conf["sres"]["state_par"]) * cur_lv_dist / lv_dist));
						}
						
						this._calc_tres_lvl_conf_data(cur_lv_conf, lvconf, cur_lv_dist, lv_dist);
						
						if(cur_lv_conf.ContainsKey("fly"))
						{
							if(cur_lv_conf["fly"].ContainsKey("speed")) cur_lv_conf["fly"]["speed"] = Convert.ToInt32((cur_lv_conf["fly"]["speed"] + (lvconf["fly"]["speed"] - cur_lv_conf["fly"]["speed"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["fly"].ContainsKey("rang")) cur_lv_conf["fly"]["rang"] = Convert.ToInt32((cur_lv_conf["fly"]["rang"] + (lvconf["fly"]["rang"] - cur_lv_conf["fly"]["rang"]) * cur_lv_dist / lv_dist));
						}
						
						if(cur_lv_conf.ContainsKey("jump"))
						{
							if(cur_lv_conf["jump"].ContainsKey("speed")) cur_lv_conf["jump"]["speed"] = Convert.ToInt32((cur_lv_conf["jump"]["speed"] + (lvconf["jump"]["speed"] - cur_lv_conf["jump"]["speed"]) * cur_lv_dist / lv_dist));
							if(cur_lv_conf["jump"].ContainsKey("rang")) cur_lv_conf["jump"]["rang"] = Convert.ToInt32((cur_lv_conf["jump"]["rang"] + (lvconf["jump"]["rang"] - cur_lv_conf["jump"]["rang"]) * cur_lv_dist / lv_dist));
							
							this._calc_tres_lvl_conf_data(cur_lv_conf["jump"], lvconf["jump"], cur_lv_dist, lv_dist);
						}
						// TO DO : add more
						
						skilconf["lv"][cur_lv] = cur_lv_conf;
					}
					
					pre_lvconf = lvconf;
				}
				
				if(m_conf.ContainsKey("clanskill"))
				{
					Variant clan_skill = new Variant();
					foreach(Variant clanskill in m_conf["clanskill"].Keys)
					{
						Variant lvs = new Variant();
						foreach(Variant _lv in clanskill["lv"].Keys)
						{
							lvs[_lv["lv"]] = _lv;
						}
                        Variant _clan_skill = new Variant();
                        _clan_skill["id"] = clanskill["id"];
                        _clan_skill["lv"] = lvs;
						//clan_skill[clanskill["id"]] = {id:clanskill.id,lv:lvs};
                        clan_skill[clanskill["id"]] = _clan_skill;
					}
					m_conf["clanskill"] = clan_skill;
				}
			}
		}

        private void _calc_tres_lvl_conf_data(Variant cur_lv_conf,Variant lvconf,int cur_lv_dist,int lv_dist)
		{
			if(cur_lv_conf.ContainsKey("tres"))
			{
				//sys.dumpobj(pre_lvconf.tres);
				//sys.dumpobj(cur_lv_conf.tres);
				//sys.dumpobj(lvconf.tres);
				foreach (string idx in cur_lv_conf["tres"].Keys)
				{
					Variant tres = cur_lv_conf["tres"][idx];
					if(tres.ContainsKey("atk_dmg")) tres["atk_dmg"] = Convert.ToInt32((tres["atk_dmg"] + (lvconf["tres"][idx]["atk_dmg"] - tres["atk_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("matk_dmg")) tres["matk_dmg"] = Convert.ToInt32((tres["matk_dmg"] + (lvconf["tres"][idx]["matk_dmg"] - tres["matk_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("nag_dmg")) tres["nag_dmg"] = Convert.ToInt32((tres["nag_dmg"] + (lvconf["tres"][idx]["nag_dmg"] - tres["nag_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("pos_dmg")) tres["pos_dmg"] = Convert.ToInt32((tres["pos_dmg"] + (lvconf["tres"][idx]["pos_dmg"] - tres["pos_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("voi_dmg")) tres["voi_dmg"] = Convert.ToInt32((tres["voi_dmg"] + (lvconf["tres"][idx]["voi_dmg"] - tres["voi_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("poi_dmg")) tres["poi_dmg"] = Convert.ToInt32((tres["poi_dmg"] + (lvconf["tres"][idx]["poi_dmg"] - tres["poi_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("hp_dmg")) tres["hp_dmg"] = Convert.ToInt32((tres["hp_dmg"] + (lvconf["tres"][idx]["hp_dmg"] - tres["hp_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("mp_dmg")) tres["mp_dmg"] = Convert.ToInt32((tres["mp_dmg"] + (lvconf["tres"][idx]["mp_dmg"] - tres["mp_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("agry_dmg")) tres["agry_dmg"] = Convert.ToInt32((tres["agry_dmg"] + (lvconf["tres"][idx]["agry_dmg"] - tres["agry_dmg"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("state_tm")) tres["state_tm"] = Convert.ToInt32((tres["state_tm"] + (lvconf["tres"][idx]["state_tm"] - tres["state_tm"]) * cur_lv_dist / lv_dist));
					if(tres.ContainsKey("state_par")) tres["state_par"] = Convert.ToInt32((tres["state_par"] + (lvconf["tres"][idx]["state_par"] - tres["state_par"]) * cur_lv_dist / lv_dist));
				}
			}
			
			if(cur_lv_conf.ContainsKey("trang"))
			{
				if(cur_lv_conf["trang"].ContainsKey("cirang")) cur_lv_conf["trang"]["cirang"] = Convert.ToInt32((cur_lv_conf["trang"]["cirang"] + (lvconf["trang"]["cirang"] - cur_lv_conf["trang"]["cirang"]) * cur_lv_dist / lv_dist));
				if(cur_lv_conf["trang"].ContainsKey("fan")) 
				{
					if(cur_lv_conf["trang"]["fan"].ContainsKey("angel")) cur_lv_conf["trang"]["fan"]["angel"] = Convert.ToInt32((cur_lv_conf["trang"]["fan"]["angel"] + (lvconf["trang"]["fan"]["angel"] - cur_lv_conf["trang"]["fan"]["angel"]) * cur_lv_dist / lv_dist));
					if(cur_lv_conf["trang"]["fan"].ContainsKey("rang")) cur_lv_conf["trang"]["fan"]["rang"] = Convert.ToInt32((cur_lv_conf["trang"]["fan"]["rang"] + (lvconf["trang"]["fan"]["rang"] - cur_lv_conf["trang"]["fan"]["rang"]) * cur_lv_dist / lv_dist));
				}
				if(cur_lv_conf["trang"].ContainsKey("maxi")) cur_lv_conf["trang"]["maxi"] = Convert.ToInt32((cur_lv_conf["trang"]["maxi"] + (lvconf["trang"]["maxi"] - cur_lv_conf["trang"]["maxi"]) * cur_lv_dist / lv_dist));
			}
						
		}
		
		//---------------------------------帮派技能-------------------------------
		/**
		 * @return 帮派技能配置 [] 
		 */
		public Variant get_clanskill_conflist()
		{
			return m_conf["clanskill"];
		}
		/**
		 * @return 指定skid的技能配置
		 */
		public Variant get_clanskill_conf(uint skid)
		{
			if(m_conf["clanskill"])
			{
				return 	m_conf["clanskill"][skid];
			}
			return null;
		}
    }
}
