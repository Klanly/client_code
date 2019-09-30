using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using MuGame.Qsmy.model;

namespace MuGame
{
    class Skill_a3Proxy : BaseProxy<Skill_a3Proxy>
    {

        public static uint EUNRINFOS = 1;


        public static uint RUNEINFOS = 0;        //所有信息
        public static uint RUNERESEARCH = 2;     //开始符文研究
        public static uint RUNEADDSPEED = 3;     //加速结果
        public static uint RUNERESEARCHOVER = 4; //研究结束
        public static uint SKILLINFO = 5;        //升级信息
        public static uint SKILLUP = 6;          //一键升级
        public static uint SKILLUPINFO = 7;      //一键升级所需
        public Skill_a3Proxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_SKILL, onLoadSkill);
            addProxyListener(PKG_NAME.C2S_A3_RUNE, onLoadRune);
        }

        public void sendProxy(int skill_id, List<int> groups)
        {
            Variant msg = new Variant();

            if (groups != null)
            {
                msg["skill_groups"] = new Variant();
                for (int i = 0; i < groups.Count; i++)
                {
                    msg["skill_groups"].pushBack(groups[i]);
                }
            }
            else
            {
                msg["skill_id"] = skill_id;
            }
            sendRPC(PKG_NAME.C2S_A3_SKILL, msg);
        }
        public void sendSkillsneed(Variant skills)//一键升级信息
        {
            Variant msg = new Variant();
            msg["skills"] = skills;
            sendRPC(PKG_NAME.C2S_A3_SKILL, msg);
        }
        public void onLoadSkill(Variant data)
        {
            debug.Log("a3技能升级：" + data.dump());
            int res = data["res"];
            if (data["res"] == 1)
            {
               
                Skill_a3Model.getInstance().skillinfos(data["skid"], data["sklvl"]);
                if (skill_a3._instance != null)
                {
                    skill_a3._instance.uprefreshskillinfo(data["skid"], data["sklvl"]);
                    skill_a3._instance.showCanStudy();
                    skill_a3._instance.showLevelupImage();
                    //skill_a3._instance.refresh(data["skid"],data["sklvl"]);
                }
                dispatchEvent(GameEvent.Create(SKILLINFO, this, data));
                //新学技能自动添加到挂机配置
                int acsk = -1;
                int i = 0;
                foreach (var v in AutoPlayModel.getInstance().Skills)
                {
                    if (v == 0)
                    {
                        bool exist = false;
                        foreach (var s in AutoPlayModel.getInstance().Skills)
                        {
                            if (s == data["skid"]) exist = true;
                        }
                        if (!exist)
                        {
                            acsk = i;
                            break;
                        }
                    }
                    i++;
                }
                if (acsk >= 0 && Skill_a3Model.getInstance().skilldic.ContainsKey(data["skid"]) &&
                    Skill_a3Model.getInstance().skilldic[data["skid"]].skillType2 != 1)
                {
                    AutoPlayModel.getInstance().Skills[acsk] = data["skid"];
                    AutoPlayModel.getInstance().WriteLocalData();
                }

                if (!Skill_a3Model.getInstance().skillid_have.Contains((int)data["skid"]))
                    Skill_a3Model.getInstance().skillid_have.Add(data["skid"]);

                //新学技能自动添加到技能栏
                if (data.ContainsKey("sklvl") && data["sklvl"] == 1)
                {
                    if (a3_skillopen.instance != null)
                    {
                        a3_skillopen.instance.open_id = data["skid"];
                        a3_skillopen.instance.refreshInfo();
                    }
                    if (Skill_a3Model.getInstance().skillid_have.Count != 2)
                    {//当为第一个技能时不开启自动拖拽，留给新手指引
                        int xxida = -1;
                        int xxidb = -1;
                        for (int a = 0; a < Skill_a3Model.getInstance().idsgroupone.Length; a++)
                        {
                            if (Skill_a3Model.getInstance().idsgroupone[a] <= 0)
                            {
                                xxida = a;
                                break;
                            }
                        }
                        if (xxida < 0)
                        {
                            for (int a = 0; a < Skill_a3Model.getInstance().idsgrouptwo.Length; a++)
                            {
                                if (Skill_a3Model.getInstance().idsgrouptwo[a] <= 0)
                                {
                                    xxidb = a;
                                    break;
                                }
                            }
                        }
                        if (Skill_a3Model.getInstance().skilldic.ContainsKey(data["skid"]))
                        {
                            if (xxida >= 0)
                            {
                                Skill_a3Model.getInstance().idsgroupone[xxida] = data["skid"];
                                if (a1_gamejoy.inst_skillbar != null && a1_gamejoy.inst_skillbar.skillsetIdx == 1)
                                    a1_gamejoy.inst_skillbar.refreSkill(xxida + 1, data["skid"]);

                            }
                            else if (xxidb >= 0)
                            {
                                Skill_a3Model.getInstance().idsgrouptwo[xxidb] = data["skid"];
                                if (a1_gamejoy.inst_skillbar != null && a1_gamejoy.inst_skillbar.skillsetIdx == 2)
                                    a1_gamejoy.inst_skillbar.refreSkill(xxidb + 1, data["skid"]);

                            }
                            if (skill_a3._instance != null)
                            {
                                skill_a3._instance.openrefreshskillinfo();
                                int x = xxida >= 0 ? 1 : 2;
                                //skill_a3._instance.moveAni(data["skid"], x, xxida);
                            }
                        }
                    }
                }
            }
            else if (data["res"] == 2)
            {
                debug.Log("a3技能组合：" + data.dump());
                Skill_a3Model.getInstance().skillGroups(data["skill_groups"]._arr);
            }
            else if (data["res"] == 4)
            {
                debug.Log("升级：" + data.dump());
                dispatchEvent(GameEvent.Create(SKILLUPINFO, this, data));
            }
            //else if (data["res"]==5)
            //{
            //    debug.Log("确认升级："+data.dump());
            //    dispatchEvent(GameEvent.Create(SKILLUP, this, data));
            //}
            else if (data["res"] < 0)
            {
                Globle.err_output(data["res"]);
                return;
            }

        }

        //符文~~~~~~~~~~~~
        public void sendProxys(int res, int id = 0, bool isfree = false)
        {
            Variant msg = new Variant();
            msg["op"] = res;
            switch (res)
            {
                case 1:
                    break;
                case 2:
                    msg["rune_id"] = id;
                    break;
                case 3:
                    msg["rune_id"] = id;
                    msg["free"] = isfree;
                    break;
            };
            sendRPC(PKG_NAME.C2S_A3_RUNE, msg);

        }

        public void onLoadRune(Variant data)
        {
            int res = data["res"];
            switch (res)
            {
                case 1:
                    debug.Log("符文的信息：" + data.dump());
                    dispatchEvent(GameEvent.Create(RUNEINFOS, this, data));

                    break; ;
                case 2://加个字段，判断是开启符文
                    debug.Log("符文开始研究结果：" + data.dump());
                    Skill_a3Model.getInstance().Reshreinfos(data["id"], -1, data["upgrade_count_down"]);
                    dispatchEvent(GameEvent.Create(RUNERESEARCH, this, data));
                    if (data["upgrade_count_down"] == 0)
                    {
                        if (a3_runeopen.instance != null)
                        {
                            a3_runeopen.instance.open_id = data["id"];
                            a3_runeopen.instance.refreshInfo();
                        }
                    }
                    break;
                case 3:
                    debug.Log("符文加速结果：" + data.dump());
                    Skill_a3Model.getInstance().Reshreinfos(data["id"], data["lvl"], 0);
                    dispatchEvent(GameEvent.Create(RUNEADDSPEED, this, data));
                    break;
                case 4:
                    debug.Log("升级完成给客户端数据：" + data.dump());
                    Skill_a3Model.getInstance().Reshreinfos(data["id"], data["lvl"], 0);
                    dispatchEvent(GameEvent.Create(RUNERESEARCHOVER, this, data));
                    break;
                default:
                    Globle.err_output(data["res"]);
                    break;
            };
        }
    }
}
