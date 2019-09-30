using System;
using GameFramework;
using Cross;

using System.Collections.Generic;

namespace MuGame
{
    public class ClientActivityViewConf : configParser
    {
        public ClientActivityViewConf( ClientConfig m ) :base( m )
        { 

        }
        public static ClientActivityViewConf create( IClientBase m )
        {
            return new ClientActivityViewConf(m as ClientConfig);
        }

        override protected Variant _formatConfig(Variant conf)
		{
            List<Variant>temparr;
			int i = 0;
			Variant act;
			Variant contents;
			if(conf.ContainsKey("activitys"))
			{
				Variant activitysobj = conf["activitys"][0];
				if(activitysobj!= null)
				{
                    temparr = activitysobj["a"]._arr;
					Variant actData = new Variant();
                    for(int j=0;j<temparr.Count;j++)
					{
                        Variant  active=temparr[j];
						Variant temptchk = transTmchks(active["tmchk"]);
						if(temptchk != null)
						{
							active["tmchk"] = temptchk;
						}
						actData[active["id"]] = active;
						if(active.ContainsKey("allday"))
						{
                            List<Variant>arr;
							arr = active["allday"]._arr ;
							if(arr.Count > 0)
							{
								active["allday"] = arr[0]["value"];
							}
						}
					}
					conf["activitys"] = actData;
				}
			}
			if(conf.ContainsKey("days"))
			{
				Variant daysobj = conf["days"][0];				
				if(daysobj!=null)
				{
					temparr = daysobj["t"]._arr;
					Variant daysData = new Variant();
                    for(int j=0;j<temparr.Count;j++)
					{
                        Variant  ddata=temparr[j];
                        //有问题
                       
                        //contents = ddata["content"].split(",");
                        contents = GameTools.split(ddata["content"], ",", GameConstantDef.SPLIT_TYPE_STRING);

						Variant acts = new Variant();
						for( i = 0; i < contents.Count; ++i )
						{
							act = conf["activitys"][ contents[i] ];
							if( act != null )
							{
								acts._arr.Add( act );
							}						
						}
						daysData[ddata["day"]] = acts;						
					}
					conf["days"] = daysData;
				}
			}
			if(conf.ContainsKey("special"))
			{
				Variant specialobj = conf["special"][0];
				if(specialobj!=null)
				{
					temparr = specialobj["broad"]._arr;
					Variant broadData = new Variant();
                    for(int j=0;j<temparr.Count;j++)
					{
                        Variant  bd=temparr[j];
						bd["tmchk"] = transTmchks(bd["tmchk"]);
						broadData[bd["bid"]] = bd;
					}
					conf["special"] = broadData;
				}
			}
			if(conf.ContainsKey("specialdays"))
			{
				Variant specialdaysobj = conf["specialdays"][0];				
				if(specialdaysobj!=null)
				{
					temparr = specialdaysobj["t"]._arr;
					Variant specialsData = new Variant();
                    for(int j=0;j<temparr.Count;j++)
					{
                        Variant  spedata=temparr[j];
						//contents = spedata["content"].split(",");
                        contents = GameTools.split(spedata["content"], ",", GameConstantDef.SPLIT_TYPE_STRING);
						Variant tempacts = new Variant();
						for( i = 0; i < contents.Count; ++i )
						{
							act = conf["special"][ contents[i] ];
							if( act != null )
							{
								tempacts._arr.Add( act );
							}						
						}
						specialsData[spedata["day"]] = tempacts;						
					}
					conf["specialdays"] = specialsData;
				}
			}
			return conf;
		}

        private Variant transTmchks(Variant tmchks)
		{
			Variant transArr = new Variant();
            if(tmchks !=null)
			foreach ( Variant tmchk in tmchks._arr )
			{
				Variant temp = new Variant();
				if(tmchk.ContainsKey("dtb"))
				{
					string dtb = tmchk["dtb"]._str;
                    //for (int j = 0; j < dtb.Length; j++)
                    //{
                    //    int index = dtb.IndexOf(":", 0);
                    //    if (-1 == index)
                    //        break;
                    //    List<string> dtba = new List<string>();
                    //    dtba.Add(dtb.Substring(0, index));
                    //    //Variant dtba = dtb.split(":");

                    //    Variant data = new Variant();
                    //    data["h"] = Convert.ToInt32(dtba[0]);
                    //    data["min"] = Convert.ToInt32(dtba[1]);
                    //    data["s"] = Convert.ToInt32(dtba[2]);
                    //    temp["dtb"] = data;
                    //}
                    //Variant dtba = dtb.split(":");
                    Variant data = new Variant();
                    Variant dtba = GameTools.split(dtb, ":", GameConstantDef.SPLIT_TYPE_STRING);
                    data["h"] = dtba[0];    // Convert.ToInt32(dtba[0]);
                    data["min"] = dtba[1];//Convert.ToInt32(dtba[1]);
                    data["s"] = dtba[2];    //Convert.ToInt32(dtba[2]);
                    temp["dtb"] = data;
					//temp["dtb"] = {h:int(dtba[0]),min:int(dtba[1]),s:int(dtba[2])};
				}
				if(tmchk.ContainsKey("dte"))
				{
                    string dte = tmchk["dte"]._str;
                    Variant dtea = GameTools.split(dte, ":", GameConstantDef.SPLIT_TYPE_STRING);
					//Variant dtea = dte.split(":");
 //待测试
                    Variant data = new Variant();                                                                                                   
                    data["h"] = dtea[0];    //Convert.ToInt32(dtea[0]);
                    data["min"] = dtea[1]; //Convert.ToInt32(dtea[1]);
                    data["s"] = dtea[2];     // Convert.ToInt32(dtea[2]);
                    temp["dte"] = data;
					//temp.dte = {h:int(dtea[0]),min:int(dtea[1]),s:int(dtea[2])};
				}
				if(tmchk.ContainsKey("optm"))
				{
					temp["optm"] = tmchk["optm"];
				}
				if(tmchk.ContainsKey("cltm"))
				{
					temp["cltm"] = tmchk["cltm"];
				}
				if(tmchk.ContainsKey("cb_optm"))
				{
					temp["cb_optm"] = tmchk["cb_optm"];
				}
				if(tmchk.ContainsKey("cb_cltm"))
				{
					temp["cb_cltm"] = tmchk["cb_cltm"];
				}
				if(tmchk.ContainsKey("tb"))
				{
                    temp["tb"] = ConfigUtil.GetTmchkAbs(tmchk["tb"]);
				}
				if(tmchk.ContainsKey("te"))
				{
					temp["te"] = ConfigUtil.GetTmchkAbs(tmchk["te"]);
				}
				if(tmchk.ContainsKey("type"))
				{
					temp["type"] = tmchk["type"];
				}
				if(null == transArr)
				{
					transArr = new Variant();
				}
				transArr._arr.Add( temp );
			}	
			return transArr;
		}

        //----------------获取数据 	start-------------------------------------------------------
		/**根据副本id获得副本信息
		 */
		public Variant GetActInfoById(uint id)
		{
			if(this.m_conf == null) return null;

			if(m_conf.ContainsKey("activitys"))
			{
				return m_conf["activitys"][id.ToString()];
                //return _conf.activitys[String(id)];
			}
			return null;
		}
        /**
		 *  返回指定星期的数据 1-7 7为星期天
		 */	
		public Variant get_day_data(int day)
		{
			if((m_conf != null) && (m_conf.ContainsKey("days")))
			{
				if(day==0) 
				{
					day=7;	
				}				
				Variant arr = m_conf["days"][day.ToString()];
				Variant sort_arr = new Variant();
				sort_arr = get_all_act_arr(arr);
				return sort_arr;
			}
			return null;
		}
        /**
		 *  返回指定星期的数据 1-7 7为星期天
		 */	
		public Variant get_specialday_data(int day)
		{
			if((m_conf != null) && (m_conf.ContainsKey("specialdays")))
			{
				if(day==0) 
				{
					day=7;
				}					
				Variant arr = m_conf["specialdays"][day.ToString()];
				return arr;
			}
			return null;
		}
        /**将同一活动在不同时间开放 拆分成 不同时间开放某一活动 （即 只考虑一天要开放多少活动 而不管是不是同一活动）
		 *param ori_arr 为每日开启的活动列表， 每个活动可能在多个时间段开启（现将其拆分出来）
		 *return  返回每日会开启多少次的活动数组 保存了 活动开启时间和活动名称等数据（可根据名称到每日活动列表中取数据） 
		 */
		protected Variant get_all_act_arr(Variant ori_arr)
		{
			Variant temp = new Variant();
			for(int i = 0; i < ori_arr.Count; i++)
			{
                Variant oriObj = ori_arr[i];
                if (oriObj.ContainsKey("allday") && oriObj["allday"] > 0)
				{
					//全天开放的活动
                    Variant day_data = oriObj.clone();
                    temp.pushBack(day_data);
				}
				else
				{
					//act_arr存放 多个时间段开启的活动
                    Variant act_arr = oriObj["tmchk"];
					if(act_arr != null)
					{
						for(int j = 0; j < act_arr.Count; j++)
						{
                            Variant dataI = oriObj.clone();//ObjectUtil.deepCloneSimpleObject(ori_arr[i]);
                            Variant dataJ = act_arr[j].clone();//ObjectUtil.deepCloneSimpleObject(act_arr[j]);
							//这里讲活动的 时间数组 分开成单个的
							Variant act_tmchk = new Variant();
							Variant tm_obj = new Variant();
							if(dataJ.ContainsKey("optm") && dataJ["optm"] > 0)
							{
								tm_obj["optm"] = dataJ["optm"];
							}
							if(dataJ.ContainsKey("cltm") && dataJ["cltm"] > 0)
							{
								tm_obj["cltm"] = dataJ["cltm"];
							}
							if(dataJ.ContainsKey("cb_optm") && dataJ["cb_optm"] > 0)
							{
								tm_obj["cb_optm"] = dataJ["cb_optm"];
							}
							if(dataJ.ContainsKey("cb_cltm") && dataJ["cb_cltm"] > 0)
							{
								tm_obj["cb_cltm"] = dataJ["cb_cltm"];
							}
							if(dataJ.ContainsKey("type") && dataJ["type"] > 0)
							{
								tm_obj["type"] = dataJ["type"];
							}
							tm_obj["dtb"] = dataJ["dtb"];
							tm_obj["dte"] = dataJ["dte"];
							if(dataJ.ContainsKey("tb"))
							{
								tm_obj["tb"] = dataJ["tb"];
								tm_obj["dtb"] = dataJ["tb"];//暂时处理
							}
							if(dataJ.ContainsKey("te"))
							{
								tm_obj["te"] = dataJ["te"];
								tm_obj["dte"] = dataJ["te"];//暂时处理
							}
														
							act_tmchk._arr.Add( tm_obj );
							
							dataI["act_tmchk"] = act_tmchk;
							temp._arr.Add(dataI);
						}
					}	
				}
			}
			return temp;
		}
		//----------------获取数据 	end-------------------------------------------------------
    }
}
