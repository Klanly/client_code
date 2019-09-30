using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{
    public class NewbieTeachItem
    {
        //   public static int curId = -1;


        public delegate NewbieTeachItem itemDelegate(string[] arr);
        //    public static List<NewbieTeachItem> l = new List<NewbieTeachItem>();
        public static Dictionary<string, itemDelegate> dListener = new Dictionary<string, itemDelegate>();
        public static Dictionary<string, TeachDoObj> dDo = new Dictionary<string, TeachDoObj>();
        public static void initCommand()
        {
            regItem("onlv", NbLevel.create);
            regItem("onclick", NbClick.create);
            regItem("onwin", NbWinOpen.create);
            regItem("onwinclose", NbWinClose.create);
            regItem("stop", NbStop.create);
            regItem("go", NbGo.create);
            regItem("onmap", NbChangeWorld.create);
            regItem("delay", NbDelay.create);
            regItem("newskill", NbNewSkill.create);
            regItem("ontask", NbTask.create);
            regItem("onskillon", NbSkillOn.create);
            regItem("onJoystick", NbJoystick.create);
            regItem("onaddpoint", NbAddPoint.create);
            regItem("oninitFb", NbFBInit.create);

            //regDoItem("showteach", NbDoItems.showTeachWin, 3);
            //regDoItem("showteachHero", NbDoItems.showWithHeroCamera, 3);

            regDoItem("st", NbDoItems.showWithNorlCamera, 4);
            regDoItem("std", NbDoItems.showWithOutClick, 2); //mark可以穿透
            regDoItem("stn", NbDoItems.showWithNext, 4);  //显示提示指引
            regDoItem("sts", NbDoItems.showWithClick, 2); //只点某个按钮
            regDoItem("sta", NbDoItems.showWithOutAvatar, 3); //不显示avatar对话
          
            
            regDoItem("hideteach", NbDoItems.hideTeachWin, 1);
            regDoItem("closeallwin", NbDoItems.closeAllwin, 1);
            regDoItem("stopmove", NbDoItems.stopMove, 1);
            regDoItem("stopmove1", NbDoItems.stopmove1, 1);
            regDoItem("hidefui", NbDoItems.hidefloatUI, 1);
            regDoItem("showfui", NbDoItems.showfloatUI, 1);
            regDoItem("playact", NbDoItems.playact, 3);
            regDoItem("playeff", NbDoItems.playEff, 3);
            regDoItem("continue", NbDoItems.ContinueDo, 1);
            regDoItem("openskill", NbDoItems.openSkill, 2);
            regDoItem("showobj", NbDoItems.showObj, 3);
            regDoItem("stl", NbDoItems.showTeachLine, 3);
            regDoItem("endnewbie", NbDoItems.endNewbie, 1);
            regDoItem("dialog", NbDoItems.showDialog, 3);
            regDoItem("cleargo", NbDoItems.clearLGo, 1);
            regDoItem("playcameraani", NbDoItems.playCameani, 2);
            regDoItem("endstory", NbDoItems.endStory, 2);
            regDoItem("skilldraw", NbDoItems.skillDraw,1);
            regDoItem("closeexpbar", NbDoItems.closeexpbar,1);
            regDoItem("openexpbar", NbDoItems.openexpbar, 1);
            
        }
        private static void regItem(string name, itemDelegate creatFun)
        {
            dListener[name] = creatFun;
        }
        private static void regDoItem(string name, Action<object[], Action> fun, int paramNum)
        {
            TeachDoObj obj = new TeachDoObj();
            obj._doFun = fun;
            obj._pramNum = paramNum;

            dDo[name] = obj;
        }

        public static NewbieTeachItem initWithStr(string str)
        {

            string[] arrMain = str.Split(':');
            if (arrMain.Length != 3 && arrMain.Length != 2)
            {
                Debug.LogError("新手脚本初始化错误,冒号：" + str);
                return null;
            }

            string[] arrTeach = arrMain[0].Split(',');



            string tempname = arrTeach[0];

            if (!dListener.ContainsKey(tempname))
            {
                Debug.LogError("新手脚本初始化错误文字错误：" + arrTeach[0] + ":" + arrMain[0]+"::::"+ str);
                return null; 
            }

            NewbieTeachItem item = dListener[tempname](arrTeach);
            if (item == null)
            {
                Debug.LogError("初始化新手错误:" + arrMain[0]);
                return null;
            }
            if (arrTeach[0] == "stop")
            {
                item.checkToDo = null;
                string[] arrDo = arrMain[1].Split('|');
                item.initDo(arrDo);
            }
            else
            {
                item.initCheck(arrMain[1]);
                string[] arrDo = arrMain[2].Split('|');
                item.initDo(arrDo);
            }

            return item;
        }



        public void initDo(object[] dos)
        {
            for (int i = 0; i < dos.Length; i++)
            {
                //debug.Log(":" + (dos[i] as string));
                string[] args = (dos[i] as string).Split(',');
                string doName = args[0] as string;
              
                TeachDoObj obj = dDo[doName];
                if (args.Length < obj._pramNum)
                {
                    Debug.LogError("新手指引参数不足"+ ":" + (dos[i] as string));
                    return;
                }




                obj._param = args;
                if (doName == "st" || doName == "sth" || doName == "stn" || doName == "sts" || doName == "sta")
                    obj.forcedo = forceDoNext;
                //  debug.Log(":" + doName + " " + obj._param + " " + dDo[doName]);
                needToDo.Add(obj);
            }
        }

        public void initCheck(string str)
        {
            if (str == "")
                checkToDo = null;
            else
                checkToDo = new NbCheckItems(str);
        }
        public int id;
        public int idx = -1;
        public NbCheckItems checkToDo;
        public List<TeachDoObj> needToDo = new List<TeachDoObj>();
        public NewbieTeachItem nextItem;
        public NewbieTeachItem()
        {
        }


        public void forceDoNext()
        {
            if (nextItem != null)
                nextItem.doit(true);
            //else
            //    curId = -1;
        }

        public void save()
        {
            if (NewbieModel.getDoneId(id))
                return;
            string str = FileMgr.loadString(FileMgr.TYPE_NEWBIE, "n");
            if (str == "")
                str = id.ToString();
            else
                str += "," + id;
            FileMgr.saveString(FileMgr.TYPE_NEWBIE, "n", str);
        }


        public bool doit(bool byforce = false, bool fromHandle = false)
        {
            if (check(fromHandle) || (byforce && this is NbStop) || this is NbGo)
            {
                if (idx == 0)
                {
                    NewbieModel.getInstance().first_show = true;
                    if (a3_liteMinimap.instance)
                    {
                        a3_liteMinimap.instance.changeCtr_NB();
                    }

                    if (id == 1)
                        InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.DIALOG);

                    if (id == 11)
                        LotteryModel.getInstance().isNewBie = true;
                    //else if (id == 8)
                    if (id == 14)


                        //InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.FB_3D);
                        //else
                        //InterfaceMgr.getInstance().closeAllWin(InterfaceMgr.FB_WIN);
                        //if(idx != 8)
                        InterfaceMgr.getInstance().closeAllWin();

                    if (id == 15) {
                        a3_BagModel.getInstance().jilu.Clear();
                    }
                    InterfaceMgr.getInstance().closeFui_NB();
                    save();
                }
                else
                    NewbieModel.getInstance().first_show = false;
                removeListener();
                // NewbieModel.getInstance().hide();

                //  curId = id;
                addedLinstener = false;
                doTeach();


                if (nextItem != null)
                    nextItem.doit();
                //else
                //    curId = -1;
                return true;
            }
            else if (addedLinstener == false)
            {
                //if (!l.Contains(this))
                //    l.Add(this);

                addedLinstener = true;
                addListener();

            }
            return false;
        }

        protected void onHanlde(GameEvent e)
        {
            if (doit(false, true))
            {
                if (addedLinstener)
                    removeListener();
                addedLinstener = false;
                //if (l.Contains(this))
                //    l.Remove(this);
            }
        }
        public void doTeach()
        {
            for (int i = 0; i < needToDo.Count; i++)
            {
                if (needToDo[i]._doFun != null)
                {
                    needToDo[i]._doFun(needToDo[i]._param, needToDo[i].forcedo);
                }
            }
            needToDo.Clear();
        }



        protected bool addedLinstener = false;
        protected int paramsNum = 0;
        public bool check(bool fromHandle)
        {
            if (this is NbStop)
                return false;

            //if (id != curId && curId>0)
            //    return false;

            if (checkToDo == null)
                return fromHandle;

            if (addedLinstener == false && idx != 0)
                return false;

            if (addedLinstener == false && idx == 0 && (this is NbWinClose ))
                return false;

            return checkToDo.doCheck();
        }
        virtual public void addListener()
        {

        }
        virtual public void removeListener()
        {

        }


    }


    //struct先保留，临时修改新手指引的bug
    public struct TeachDoObj
    {
        public Action<object[], Action> _doFun;
        public Action forcedo;
        public int _pramNum;
        public object[] _param;
    }
}
