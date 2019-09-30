using System.Collections.Generic;
using Cross;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace MuGame
{
	class a3_wantlvup : Window
	{
		Text cs_rotine;
		Text cs_expfb;
		Text cs_sczghd;

		public override void init() {
			cs_rotine = getTransformByPath("cells/scroll/content/routine/name/dj").GetComponent<Text>();
			cs_expfb = getTransformByPath("cells/scroll/content/expfb/name/dj").GetComponent<Text>();
			cs_sczghd = getTransformByPath("cells/scroll/content/sczghd/name/dj").GetComponent<Text>();
			new BaseButton(getTransformByPath("btn_close")).onClick = (GameObject g) => {
				InterfaceMgr.getInstance().close(InterfaceMgr.A3_WANTLVUP);
			};
			new BaseButton(getTransformByPath("cells/scroll/content/routine/go")).onClick = routine_go;
			new BaseButton(getTransformByPath("cells/scroll/content/expfb/go")).onClick = expfb_go;
			new BaseButton(getTransformByPath("cells/scroll/content/sczghd/go")).onClick = sczghd_go;
            //new BaseButton(getTransformByPath("cells/scroll/content/zhuxian/go")).onClick = zhuxian_go;
            new BaseButton(getTransformByPath("cells/scroll/content/guaji/go")).onClick = sczghd_go;
            new BaseButton(getTransformByPath("cells/scroll/content/entrustTask/go")).onClick = entrust_go;


            getComponentByPath<Text>("bg/Text").text = ContMgr.getCont("a3_wantlvup_0");
            getComponentByPath<Text>("cells/scroll/0/go/text").text = ContMgr.getCont("a3_wantlvup_1");
            getComponentByPath<Text>("cells/scroll/content/zhuxian/name").text = ContMgr.getCont("a3_wantlvup_2");
            getComponentByPath<Text>("cells/scroll/content/zhuxian/des").text = ContMgr.getCont("a3_wantlvup_3");
            getComponentByPath<Text>("cells/scroll/content/zhuxian/go/text").text = ContMgr.getCont("a3_wantlvup_1");
            getComponentByPath<Text>("cells/scroll/content/entrustTask/name").text = ContMgr.getCont("a3_wantlvup_4");
            getComponentByPath<Text>("cells/scroll/content/entrustTask/des").text = ContMgr.getCont("a3_wantlvup_5");
            getComponentByPath<Text>("cells/scroll/content/entrustTask/go/text").text = ContMgr.getCont("a3_wantlvup_1");
            getComponentByPath<Text>("cells/scroll/content/routine/name").text = ContMgr.getCont("a3_wantlvup_6");
            getComponentByPath<Text>("cells/scroll/content/routine/des").text = ContMgr.getCont("a3_wantlvup_7");
            getComponentByPath<Text>("cells/scroll/content/routine/go/text").text = ContMgr.getCont("a3_wantlvup_1");
            getComponentByPath<Text>("cells/scroll/content/expfb/name").text = ContMgr.getCont("a3_wantlvup_8");
            getComponentByPath<Text>("cells/scroll/content/expfb/des").text = ContMgr.getCont("a3_wantlvup_9");
            getComponentByPath<Text>("cells/scroll/content/expfb/go/text").text = ContMgr.getCont("a3_wantlvup_1");
            getComponentByPath<Text>("cells/scroll/content/sczghd/name").text = ContMgr.getCont("a3_wantlvup_10");
            getComponentByPath<Text>("cells/scroll/content/sczghd/des").text = ContMgr.getCont("a3_wantlvup_11");
            getComponentByPath<Text>("cells/scroll/content/sczghd/go/text").text = ContMgr.getCont("a3_wantlvup_1");
            getComponentByPath<Text>("cells/scroll/content/guaji/name").text = ContMgr.getCont("a3_wantlvup_12");
            getComponentByPath<Text>("cells/scroll/content/guaji/des").text = ContMgr.getCont("a3_wantlvup_13");
            getComponentByPath<Text>("cells/scroll/content/guaji/go/text").text = ContMgr.getCont("a3_wantlvup_1");
        }

		public override void onShowed() {
			refresh();
		}

		public override void onClosed() {
            
		}

		void refresh() {
			var dd = A3_TaskModel.getInstance().GetDailyTask();
			if (dd != null) {
				cs_rotine.text = "(" + (A3_TaskModel.getInstance().GetTaskMaxCount(dd.taskId) - dd.taskCount) + "/" +  A3_TaskModel.getInstance().GetTaskMaxCount(dd.taskId) + ")";
                this.transform.FindChild("cells/scroll/content/routine").gameObject.SetActive(true);
            }
			else {
                //cs_rotine.text = "(未开启)";
                this.transform.FindChild("cells/scroll/content/routine").gameObject.SetActive(false);
			}

			Variant data = SvrLevelConfig.instacne.get_level_data(101);
			int max_times = data["daily_cnt"]+ A3_VipModel.getInstance().expFb_count;
			int use_times = 0;
			if (MapModel.getInstance().dFbDta.ContainsKey(101)) {
				use_times = Mathf.Min(MapModel.getInstance().dFbDta[101].cycleCount, max_times);
			}
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EXP_DUNGEON)) {
				cs_expfb.text = "(" + (max_times - use_times) + "/" + max_times + ")";
                this.transform.FindChild("cells/scroll/content/expfb").gameObject.SetActive(true);
			}
			else {
                //cs_expfb.text = "(未开启）";
                this.transform.FindChild("cells/scroll/content/expfb").gameObject.SetActive(false);
            }

            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.ENTRUST_TASK))
            {
                TaskData entrustTask;
                if ((entrustTask = A3_TaskModel.getInstance().GetEntrustTask()) != null)
                {
                    int curCount, maxCount;
                    maxCount = XMLMgr.instance.GetSXML("task.emis_limit").getInt("emis_limit") * XMLMgr.instance.GetSXML("task.emis_limit").getInt("loop_limit");
                    curCount = maxCount - (entrustTask.taskCount + entrustTask.taskLoop * XMLMgr.instance.GetSXML("task.emis_limit").getInt("emis_limit"));
                    curCount = curCount > 0 ? curCount : 0;
                    this.transform.FindChild("cells/scroll/content/entrustTask").gameObject.SetActive(true);
                    this.transform.FindChild("cells/scroll/content/entrustTask/name/dj").GetComponent<Text>().text =
                        string.Format("{0}/{1}", curCount, maxCount);
                }
                else
                    this.transform.FindChild("cells/scroll/content/entrustTask").gameObject.SetActive(false);

            }
            else                
                this.transform.FindChild("cells/scroll/content/entrustTask").gameObject.SetActive(false);

            if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.AUTO_PLAY))
            {
                if (GeneralProxy.getInstance().active_open)
                {
                    getTransformByPath("cells/scroll/content/sczghd/go").GetComponent<Button>().interactable = true;
                    this.transform.FindChild("cells/scroll/content/sczghd").gameObject.SetActive(true);
                    this.transform.FindChild("cells/scroll/content/guaji").gameObject.SetActive(false);
                    //getTransformByPath("cells/scroll/content/sczghd/go/text").GetComponent<Text>().text = "(已经开启)";
                }
                else
                {
                    this.transform.FindChild("cells/scroll/content/sczghd").gameObject.SetActive(false);
                    this.transform.FindChild("cells/scroll/content/guaji").gameObject.SetActive(true);
                    //getTransformByPath("cells/scroll/content/sczghd/go").GetComponent<Button>().interactable = false;
                    //getTransformByPath("cells/scroll/content/sczghd/go/text").GetComponent<Text>().text = "(等待开启)";
                }
            }
            else
            {
                getTransformByPath("cells/scroll/content/sczghd/go").GetComponent<Button>().interactable = false;
                this.transform.FindChild("cells/scroll/content/sczghd").gameObject.SetActive(false);
                this.transform.FindChild("cells/scroll/content/guaji").gameObject.SetActive(false);
            }
			cs_sczghd.text = "";
            want_to();

        }

        int map_x;
        int map_y;
        int map_id;
        void want_to()
        {
            map_x = 0;
            map_y = 0;
            map_id = 1;

            SXML s_xml = XMLMgr.instance.GetSXML("god_light");
            List<SXML> s_xml_info = s_xml.GetNodeList("player_info");
            if (s_xml_info != null)
            {
                foreach (SXML x in s_xml_info)
                {
                    if (x.getInt("zhuan") < PlayerModel.getInstance().up_lvl)
                    {

                    }
                    else if (x.getInt("zhuan") == PlayerModel.getInstance().up_lvl)
                    {
                        if (x.getInt("lv") > PlayerModel.getInstance().lvl)
                            break;
                    }
                    else
                    {
                        break;
                    }
                    map_id = x.getInt("map_id");
                    map_x = x.getInt("map_x");
                    map_y = x.getInt("map_y");
                }
            }

        }
		void routine_go(GameObject go) {
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.DAILY_TASK, true)) {
				ArrayList arr = new ArrayList();
				arr.Add(20005);
				InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TASK, arr);
				InterfaceMgr.getInstance().close(InterfaceMgr.A3_WANTLVUP);
			}
		}
        //void zhuxian_go(GameObject go)
        //{
        //    InterfaceMgr.getInstance().open(InterfaceMgr.A3_TASK);
        //    InterfaceMgr.getInstance().close(InterfaceMgr.A3_WANTLVUP);
        //}

		void expfb_go(GameObject go)
        { 
			if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.EXP_DUNGEON, true)) {
				//ArrayList arr = new ArrayList();
				//arr.Add("exp");
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_COUNTERPART/*, arr*/);
				InterfaceMgr.getInstance().close(InterfaceMgr.A3_WANTLVUP);
			}
		}

		void sczghd_go(GameObject go) {
			InterfaceMgr.getInstance().close(InterfaceMgr.A3_WANTLVUP);
            SelfRole.moveto(map_id, new Vector3(map_x,0, map_y));
        }

        void entrust_go(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_WANTLVUP);
            TaskData entrustTask = A3_TaskModel.getInstance().GetEntrustTask();
            if (entrustTask != null)
                a3_task_auto.instance.RunTask(entrustTask, clickAuto: true);
        }
	}
}
