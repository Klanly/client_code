using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class SvrMissionConfig : configParser
    {
        Variant m_mis_misline_data;
        Variant m_mis_npc_data;
        Variant m_resolve=new Variant();
        private List<Variant> m_hasAttMline = null;//有属性加成奖励的主线任务

        public SvrMissionConfig(ClientConfig m)
            : base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {
            return new SvrMissionConfig(m as ClientConfig);
        }
        public Variant resolve()
        {
            
            if (this.m_conf == null)
                return null;
            for (int i = 0; i < this.m_conf["mission"].Count; i++)
            {
                m_resolve[this.m_conf["mission"][i]["id"]._int] = this.m_conf["mission"][i];
            }
            return m_resolve;
        }
        public Variant get_mission_conf(int misid)
        {
            //resolve();
            //if (m_resolve == null)
            //    return null;
            //return m_resolve[misid];

            if (this.m_conf == null)
                return null;

            return this.m_conf["mission"][misid.ToString()];

        }
        public Variant get_mis_by_line(int mis_line)
        {
            return m_mis_misline_data[mis_line.ToString()];
        }
        public Variant get_missions()
        {
            return this.m_conf["mission"];
        }
        public string getMisName(int misid)
        {
            return LanguagePack.getLanguageText("misName", misid.ToString());
        }
        public string getMisDesc(int misid)
        {
            return LanguagePack.getLanguageText("misDesc", misid.ToString());
        }
        public string getMisGoalDesc(int misid)
        {
            return LanguagePack.getLanguageText("misGoalDesc", misid.ToString());
        }
        public Variant get_mis_by_npc(int npc_iid)
        {
            return m_mis_npc_data[npc_iid.ToString()];
        }
        public Variant get_rmiss()
        {
            return m_conf["rmis"];
        }
        override protected Variant _formatConfig(Variant conf)
        {
            m_mis_misline_data = new Variant();
            m_mis_npc_data = new Variant();
            if (conf != null)
            {
                if (conf.ContainsKey("mission"))
                {
                    Variant newMis = new Variant();
                    Variant mission = conf["mission"];
                    for (int i = 0; i < mission.Count; i++)
                    {
                        Variant m = mission[i];
                        if (m.ContainsKey("accept"))
                        {
                            m["accept"] = m["accept"][0];
                            if (m["accept"].ContainsKey("npc") && m["accept"]["npc"]._str == "")
                            {
                                m["accept"]["npc"] = "0";
                            }
                        }
                        if (m.ContainsKey("awards"))
                        {
                            m["awards"] = m["awards"][0];
                            if (m["awards"].ContainsKey("npc") && m["awards"]["npc"]._str == "")
                            {
                                m["awards"]["npc"] = "0";
                            }
                        }
                        if (!this.m_mis_misline_data.ContainsKey(m["misline"]._str))
                        {
                            m_mis_misline_data[m["misline"]._str] = new Variant();
                        }
                        m_mis_misline_data[m["misline"]._str][m["id"]._str] = m;
                        if (!this.m_mis_npc_data.ContainsKey(m["accept"]["npc"]._str))
                        {
                            m_mis_npc_data[m["accept"]["npc"]._str] = new Variant();
                        }
                        m_mis_npc_data[m["accept"]["npc"]._str][m["id"]._str] = m;

                        newMis[m["id"]._str] = m;

                    }
                    conf["mission"] = newMis;
                }
            }

            return conf;
        }
        override protected void onData()
        {
            format_mission();
        }
        protected void format_mission()
        {
            //m_mis_misline_data = new Variant();
            //m_mis_npc_data = new Variant();
            //Variant mission = this.m_conf["mission"];
            //for (int i = 0; i < mission.Count; i++)
            //{
            //    Variant m = mission[i];
            //    if (m.ContainsKey("accept"))
            //    {
            //        m["accept"] = m["accept"][0];
            //    }
            //    if (m.ContainsKey("awards"))
            //    {
            //        m["awards"] = m["awards"][0];
            //    }

            //    if (!this.m_mis_misline_data.ContainsKey(m["misline"]._str))
            //    {
            //        m_mis_misline_data[m["misline"]._str] = new Variant();
            //    }
            //    m_mis_misline_data[m["misline"]._str][m["id"]._str] = m;
            //    if (!this.m_mis_npc_data.ContainsKey(m["accept"]["npc"]._str))
            //    {
            //        m_mis_npc_data[m["accept"]["npc"]._str] = new Variant();
            //    }
            //    m_mis_npc_data[m["accept"]["npc"]._str][m["id"]._str] = m;
            //}
        }
        public Variant GetRmisShare(int id)
        {
            Variant arr = m_conf["rmis_share"];
            if (arr != null)
            {
                foreach (Variant obj in arr._arr)
                {
                    if (obj != null && obj["id"] == id)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        //委托任务 数据
		private Variant autocomitArr = null;
		
		/**
		 *获得 委托任务
		 */
		public Variant get_autocomit_mis()
        {
			if(autocomitArr == null)
            {
				autocomitArr = new Variant();
				
				Variant misarr = null;
				Variant mobj = null;
				Variant attchkarr = null;
				
				
				foreach(string s in m_mis_misline_data.Keys)
                {
					//主线不用
					if(s == "1")
						continue;
					
					misarr = m_mis_misline_data[s];
					if(misarr == null)
						continue;
					
					foreach(string i in misarr.Keys)
                    {
						mobj = misarr[i];
						
						if(mobj == null)
							continue;
						
						//剔除不是日常任务的,不是委托任务的
						if(!mobj.ContainsKey("dalyrep") || mobj["dalyrep"]._int <= 0 || !mobj.ContainsKey("autocomit_yb"))
							continue;
						
						attchkarr = mobj["accept"]["attchk"];
						
						if(attchkarr == null)
							continue;
						
						int min = 0;
						int max = 0;
						foreach(string m in attchkarr.Keys)
                        {
							
							if(attchkarr[m] == null)
								continue;
							
							//没有等级限定 剔除
							if(attchkarr[m]["name"]._str != "level")
								continue;
							
							if(attchkarr[m].ContainsKey("min")){
								min = attchkarr[m]["min"];
							}
							
							if(attchkarr[m].ContainsKey("max")){
								max = attchkarr[m]["max"];
							}
						}
						Variant p = new Variant();
						if(autocomitArr[min] == null)
                        {
							Variant autoarr = new Variant();
                            p["data"] = misarr[i];
                            p["mx"] = max;
							autoarr.pushBack(p);
							autocomitArr[min] = autoarr;
						}
						else{
                            p["data"] = misarr[i];
                            p["mx"] = max;
							autocomitArr[min].pushBack(p);
						}
						
					}
				}				
			}
			return autocomitArr;
		}


        public Variant get_mis_appawd(int id)
		{
            Variant appawd = m_conf["appawd"] ;
			if(appawd == null)
				return null;
            return appawd[id.ToString()];
		}
        public Variant get_mis_appgoal(int id)
		{
            Variant appgoal = m_conf["appgoal"];
			if(appgoal == null)
				return null;

            foreach (Variant obj in appgoal._arr)
            {
                if (obj != null && obj["id"]._int == id)
                {
                    return obj;
                }
            }

			return appgoal[id.ToString()];
		}
        public Variant get_qamis(int id)
        {
			if(m_conf == null)
				return null;
            return m_conf["qamis"][id.ToString()];
		}
        public Variant get_question(int id){
            if (m_conf == null)
				return null;
            return m_conf["question"][id.ToString()];
		}
        /**
		 * 获取某个每日必做
		 * */
		public Variant GetDmisById(int id)
		{
            if (m_conf["dmis"]!=null)
			{
                return m_conf["dmis"][id.ToString()];
			}
			return null;
		}
        /**
		 * 获取每日必做奖励信息
		 * */
		public Variant GetDmisAwd()
		{
            return m_conf["dmis_awd"];
		}
        //---------主线任务永久奖励Start----------------------
		public Variant GetMlineawd()
		{
            return m_conf["mlineawd"];
		}
        public List<Variant> GetMlineawdByAtt()
		{
            if (m_hasAttMline == null)
			{
                m_hasAttMline = new List<Variant>();
			}
			else
			{
                return m_hasAttMline;
			}
			
			Variant mlineawd = GetMlineawd();
			Variant lineawd = null;
			for (int i = 0; i < mlineawd.Count; i++) 
			{
				lineawd = mlineawd[i];
				if(lineawd.ContainsKey("att"))
				{
                    m_hasAttMline.Add(lineawd);
				}
			}
            return m_hasAttMline;
		}
        /**
		 * 根据上次领奖的id和当前任务id获取最近的一个任务奖励
		 * */
		public Variant GetCurMlineAwd(int lastid)
		{
			Variant mlineawd = GetMlineawd();
			Variant temp=null;
			if(lastid > 0)
			{
				for(int i=0;i<mlineawd.Count;++i)
				{
					temp = mlineawd[i];
					if(lastid==temp["misid"]._int)
					{
						temp = mlineawd[i+1];
						if(temp!=null)
						{
							break;
						}
					}
				}
			}
			else
			{
				temp = mlineawd[0];
			}
			return temp;
		}
        public int GetAwardChapter(int misid)
		{
			Variant mlineawd = GetMlineawd();
			for(int i=0;i<mlineawd.Count;++i)
			{
				Variant temp = mlineawd[i.ToString()];
				if(misid==temp[misid.ToString()]._int)
				{
					return i;
				}
			}
			return 0;
		}
        /**
		 * 获取主线任务奖励信息
		 * */
		public Variant GetMlineawdByItm(int misid,int carr)
		{
			Variant mlineawd = GetMlineawd();
            for(int i=0;i<mlineawd.Count;i++){
                Variant temp=mlineawd[i];
                if(misid==temp[misid.ToString()]._int)
				{
					Variant awardArr = temp["awards"]["award"];
					Variant lineawd=null;
                    for(int j=0;j<awardArr.Count;j++){
                        Variant awd=awardArr[j];
                        if (awd.ContainsKey(carr.ToString()) && awd[carr.ToString()]._int == carr)
						{
							lineawd = awd;
							lineawd[misid.ToString()]._int = temp[misid.ToString()]._int;
							return lineawd;
						}
                    }
					lineawd = awardArr[0];
                    lineawd[misid.ToString()]._int = temp[misid.ToString()]._int;
					return lineawd;
				}
            }
			return null;
		}

        //---------主线任务永久奖励End-----------------------
		//--------------------------获得章节所有主线任务Start------------------------------------------------------------
		private Variant m_allChapterMis = null;//所有章节主线任务，章节判断是根据mlineawd判断
		private Variant m_sortAllChapterMis = null;//排序后的章节主线任务
        public Variant GetChapterMisByChapter(int curChapter)
		{//获得本章节所有主线任务
			if(m_allChapterMis == null)
			{
				m_allChapterMis = new Variant();
				m_sortAllChapterMis = new Variant();
				Variant allMis = get_missions();
				int chapter = 0;
//				var mlineawd:Array = GetMlineawdByAtt();
				Variant mlineawd = GetMlineawd();
				Variant lineAwd = mlineawd[chapter];
				bool hasChangeChapter = true;
				Variant chapterMis = null;
				List<Variant> sortChapterMis = null;
				Variant mis = null;
				for (int i = 0; i < allMis.Count; i++) 
				{
					mis = allMis[i];
					
					if(mis == null)
					{
						continue;
					}
					
					if(mis.ContainsKey("misline") && mis["misline"]._int == 1)
					{
						if(mis["id"]._int > lineAwd["misid"]._int)
						{
							hasChangeChapter = true;
							chapter++;
						}
						
						if(hasChangeChapter)
						{
							hasChangeChapter = false;
							lineAwd = mlineawd[chapter];
							if(lineAwd == null)
							{
								break;
							}
							if(m_allChapterMis[chapter] == null)
							{
                                m_allChapterMis[chapter] = new Variant();
							}
							if(m_sortAllChapterMis[chapter] == null)
							{
                                m_sortAllChapterMis[chapter] = new Variant();
							}
							chapterMis = m_allChapterMis[chapter];
							sortChapterMis =new List<Variant>( m_sortAllChapterMis[chapter].Values);
						}
						
						chapterMis[mis["id"]._str] = mis;
						sortChapterMis.Add(mis);
					}
					else
					{
						continue;
					}
				}
			}
			
			return m_sortAllChapterMis[curChapter];
		}

        public bool IsChapterMis(int curChapter,int misid)
		{//是否是章节中的任务
			if(m_allChapterMis[curChapter] == null)
			{
				return false;
			}
			
			if(m_allChapterMis[curChapter][misid] == null)
			{
				return false;
			}
			
			return true;
		}
    }
}
