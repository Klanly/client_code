using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MuGame;


public class CollectRole : MonsterRole
{
    static public CollectRole instance;
    public  float collectTime;
    public bool becollected;

    private string collectStr = "";
    private string collectStr2 = "";
    public override void Init(string prefab_path, int layer, Vector3 pos, float roatate = 0)
    {
        m_fNavStoppingDis = 2f;

        base.Init(prefab_path, EnumLayer.LM_COLLECT, pos, roatate);
        collectTime = tempXMl.getFloat("use_time");
        maxHp = curhp = 1000;
        collectStr = tempXMl.getString("show");
    }

    override protected void Model_Loaded_Over()
    {
        M00000_Default_Event mde = m_curModel.gameObject.AddComponent<M00000_Default_Event>();
        mde.m_monRole = this;
    }

    protected override void onRefresh_ViewType()
    {
        if (viewType == VIEW_TYPE_ALL)
        {
            m_moveAgent.avoidancePriority = 0;
            m_moveAgent.enabled = false;




            Transform eff = m_curModel.transform.FindChild("eff");
            if (eff != null)
            {
                //debug.Log("::::::::::::::::::::::" + A3_TaskModel.getInstance().IfCurrentCollectItem(monsterid) + " " + monsterid);
                //eff.gameObject.SetActive(A3_TaskModel.getInstance().IfCurrentCollectItem(monsterid));
                eff.gameObject.SetActive(true);
            }


        }

    }

    public bool checkCanClick()
    {
        return true;
    }

    public override void onClick()
    {
        if (!A3_TaskModel.getInstance().IfCurrentCollectItem(monsterid))        
            return;
        cd.hide();      
        if (Vector3.Distance(m_curModel.transform.position, SelfRole._inst.m_curModel.transform.position) > 2)
        {
            SelfRole.moveto(m_curModel.transform.position, () =>
            {
                MapProxy.getInstance().sendCollectItem(m_unIID);
                cd.updateHandle = onCD;
                cd.show(() =>
                {
                    m_curAni.SetBool("open", true);
                    MapProxy.getInstance().sendStopCollectItem(false);
                    becollected = true;
                },
               collectTime, true,
                () =>
                {
                    MapProxy.getInstance().sendStopCollectItem(true);
                });

            }, true, 2f);
        }
        else
        {
            MapProxy.getInstance().sendCollectItem(m_unIID);
            cd.updateHandle = onCD;
            cd.show(() =>
            {
                m_curAni.SetBool("open", true);
                MapProxy.getInstance().sendStopCollectItem(false);
                becollected = true;
            },
           collectTime, true,
            () =>
            {

                MapProxy.getInstance().sendStopCollectItem(true);
            });
        }
    }

    public override void dispose()
    {
        if (m_curAni.GetBool("open") == true)
        {
            ConfigUtil.SetTimeout(1400, a);
        }
        else
        {
            base.dispose();
        }
    }

    public void a()
    {
        DoAfterMgr.instacne.addAfterRender(base.dispose);
    }


    public void onCD(cd item)
    {
        int temp = (int)(cd.secCD - cd.lastCD) / 100;
        item.txt.text = collectStr + ((float)temp / 10f).ToString();
    }
    public void onCD2(cd item)
    {
        int temp = (int)(cd.secCD - cd.lastCD) / 100;
        collectStr2 = ContMgr.getCont("openbox");
        item.txt.text = collectStr2 + ((float)temp / 10f).ToString();
    }



    public override void FrameMove(float delta_time)
    {

    }
}

public class CollectBox : CollectRole
{
    //static public CollectBox instance;
    public bool iscollect;
    public override void onClick()
    {

        //if (!A3_TaskModel.getInstance().IfCurrentCollectItem(monsterid))
        //    return;


        if (Vector3.Distance(m_curModel.transform.position, SelfRole._inst.m_curModel.transform.position) >= 0)
        {
            SelfRole.moveto(m_curModel.transform.position, () =>
            {
                MapProxy.getInstance().sendCollectBox(m_unIID);
           
                cd.updateHandle = onCD2;
                cd.show(() =>
                {
                    m_curAni.SetBool("open", true);
                    MapProxy.getInstance().sendStopCollectBox(false);
                  
                    InterfaceMgr.getInstance().close(InterfaceMgr.CD);
                  

                },
              collectTime, true,
                () =>
                {
                   
                    MapProxy.getInstance().sendStopCollectBox(true);
                   
                });

            }, true, 1f);
            //InterfaceMgr.getInstance().close(InterfaceMgr.CD);
        }
        //else
        //{
           
        //    cd.updateHandle = onCD2;
        //    cd.show(() =>
        //    {
        //        MapProxy.getInstance().sendCollectBox(m_unIID);
        //        // InterfaceMgr.getInstance().close(InterfaceMgr.CD);

        //    },
        //  collectTime, true,
        //    () =>
        //    {
               
        //         MapProxy.getInstance().sendStopCollectBox();
        //    });
        //    InterfaceMgr.getInstance().close(InterfaceMgr.CD);
        //}
    }
    //void Update()
    //{
    //    if (iscollect == true)
    //    {
    //        //if ()//正在遭受攻击；
    //        {
    //            MapProxy.getInstance().endCollectBox();
    //            iscollect = false;
    //        }


    //    }


    //}
}
