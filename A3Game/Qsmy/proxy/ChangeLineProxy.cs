using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
namespace MuGame
{
   
    class ChangeLineProxy:BaseProxy<ChangeLineProxy>
    {
        public  Dictionary<int, uint> Line_people = new Dictionary<int, uint>();
        public ChangeLineProxy()
        {
            addProxyListener(PKG_NAME.C2S_CHANGE_LINE, onLineInfo);//线路总数据
            addProxyListener(PKG_NAME.S2C_LINE_CHANGE, onChangeLine);//变换线路
            
        }
        //s2C
        void onLineInfo(Variant data)
        {
            Line_people.Clear();
            debug.Log("收到分线的信息：" + data.dump());
            if(data["line_busy"]!=null)
            {
                for(int i =0;i< data["line_busy"].Count;i++)
                {
                    Line_people[i] = data["line_busy"][i]._uint;
                }
            }
            if (a3_mapChangeLine.instance)
                a3_mapChangeLine.instance.creatrvegrids(data["nor_line"] - 1);
           // worldmap._instance.line_info(data["nor_line"] - 1);
        } 
        void onChangeLine(Variant data)
        {
            //0就是1线,按下标分
            debug.Log("收到转线信息：" + data.dump());
            if(data.ContainsKey("change_limit"))
            {
                //冷却中
                flytxt.instance.fly(ContMgr.getCont("ChangeLineProxy0") +data["change_limit"]+ ContMgr.getCont("ChangeLineProxy1"), 1);
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("ChangeLineProxy2") + data["line"]+1+ ContMgr.getCont("ChangeLineProxy3"), 1);
                if (a3_mapChangeLine.instance != null)
                    a3_mapChangeLine.instance.onBtnCancelClick(null);
                PlayerModel.getInstance().line = data["line"];
                int line = data["line"] + 1;
                InterfaceMgr.doCommandByLua("a1_high_fightgame.change__Line", "ui/interfaces/high/a1_high_fightgame", line.ToString());
            }

        }
        //C2S
        //线路数据
        public void sendLineProxy()
        {
            Variant msg = new Variant();
            sendRPC(PKG_NAME.C2S_LINE_INFO_RES, msg);    
        }
        //选择
        public void select_line(uint l)
        {
            Variant line = new Variant();
            line["idx"] = l;
            sendTPKG(7, line);
        }

    }
}
