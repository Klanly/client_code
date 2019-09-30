using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using Cross;
using UnityEngine;

namespace MuGame
{
    class MoveProxy : BaseProxy<MoveProxy>
    {
        public MoveProxy()
            : base()
        {
            addProxyListener(PKG_NAME.S2C_POS_CORRECT, pos_correct);
            addProxyListener(PKG_NAME.S2C_MOVE, move);
            addProxyListener(PKG_NAME.S2C_STOP, on_stop);
        }

        private uint m_unLastReqX = 0; //最后请求发送的位置X
        private uint m_unLastReqY = 0; //最后请求发送的位置Y
        private uint m_unLastReqToX = 0;
        private uint m_unLastReqToY = 0;
        private uint m_unLastReqTM = 0;
        private uint m_unLastReqCount = 0; //离最后一次请求的帧频
        private uint m_unReqMoreCount = 0;
        private uint m_unSendMoveMsgCount = 6;
        private float m_unLastReqRadian = 0;
        private void ReqChangeMoveMsg(uint x, uint y, uint tox, uint toy, uint tm, float radian = -1f, bool force = false)
        {
            m_unLastReqX = x;
            m_unLastReqY = y;
            m_unLastReqToX = tox;
            m_unLastReqToY = toy;
            m_unLastReqTM = tm;
            m_unLastReqCount = 0;
            if (radian!=-1f)
            m_unLastReqRadian = radian;

            //TrySendMoveMsg(false, force);
        }

        //原理告诉服务器现在的位置和要去向的地方,（服务器处理：相信客户端现在的位置，并和上次记录的交验避免移动过快，比对要移动到的位置，
        //如果已经广播过去向的位置，就不广播了）
        //每分钟2条位置消息，来同步自己的位置
        private float m_fSyncPosTime = 0f;
        public void resetFirstMove()
        {
            m_bFirst = true;
        }
        public void TrySyncPos(float dt)
        {
            if (m_fSyncPosTime < 0f)
            {
                if (m_bFirst)
                {
                    if (m_unLastSendX != m_unLastReqX || m_unLastSendY != m_unLastReqY
                        || m_unLastSendToX != m_unLastReqToX || m_unLastSendToY != m_unLastReqToY || MapProxy.getInstance().change_map)
                    {
                        m_unLastSendX = m_unLastReqX;
                        m_unLastSendY = m_unLastReqY;
                        m_unLastSendToX = m_unLastReqToX;
                        m_unLastSendToY = m_unLastReqToY;

                        m_bFirst = false;
                    }
                }
                if ((m_unLastSendX != m_unLastReqX || m_unLastSendY != m_unLastReqY 
                    || m_unLastSendToX != m_unLastReqToX || m_unLastSendToY != m_unLastReqToY))
                {
                    m_unLastSendX = m_unLastReqX;
                    m_unLastSendY = m_unLastReqY;
                    m_unLastSendToX = m_unLastReqToX;
                    m_unLastSendToY = m_unLastReqToY;

                    Variant msg = Variant.alloc();
                    msg["frm_x"] = m_unLastSendX;
                    msg["frm_y"] = m_unLastSendY;
                    msg["to_x"] = m_unLastSendToX;
                    msg["to_y"] = m_unLastSendToY;
                    msg["start_tm"] = m_unLastReqTM;
                    msg["radian"] = m_unLastReqRadian;
                    sendRPC(PKG_NAME.C2S_MOVE, msg);
                    // debug.Log("发送 移动 " + m_unLastSendX + " " + m_unLastSendY + "   to  " + m_unLastSendToX + " " + m_unLastSendToY);
                    Variant.free(msg);

                    m_fSyncPosTime = 0.5f;
                }
            }
            else
            {
                m_fSyncPosTime -= dt;
            }
        }
        public void sendVec()
        {
            if (!SelfRole._inst.isDead)
            {
                m_unLastSendX = 0 ;
                m_unLastSendY = 0;
                m_unLastSendToX =1;
                m_unLastSendToY = 1;

                Variant msg = Variant.alloc();
                msg["frm_x"] = m_unLastSendX;
                msg["frm_y"] = m_unLastSendY;
                msg["to_x"] = m_unLastSendToX;
                msg["to_y"] = m_unLastSendToY;
                msg["start_tm"] = m_unLastReqTM;
                msg["radian"] = m_unLastReqRadian;
                sendRPC(PKG_NAME.C2S_MOVE, msg);
                // debug.Log("发送 移动 " + m_unLastSendX + " " + m_unLastSendY + "   to  " + m_unLastSendToX + " " + m_unLastSendToY);
                Variant.free(msg);
            }
        }


        //public void TrySendMoveMsg(bool inupdate,bool force=false)
        //{
        //    if (inupdate)
        //    {
        //        m_unLastReqCount++;
        //    }

        //    if (m_unLastReqCount < 2)
        //    {
        //        m_unReqMoreCount++;
        //    }

        //    if (m_unSendMoveMsgCount > 0 && !force)
        //    {
        //        if (inupdate)
        //        {
        //            m_unSendMoveMsgCount--;
        //        }

        //        return;
        //    }

        //    if (m_unLastReqCount > 8 || force)
        //    {
        //        //如果8帧中没有更新了， 位置了，发送要移动到的位置
        //        m_unSendMoveMsgCount = 6;
        //        m_unReqMoreCount = 0;
        //        SendMoveMsgToServer(m_unLastReqX, m_unLastReqY, m_unLastReqToX, m_unLastReqToY, m_unLastReqTM,m_unLastReqRadian);
        //    }
        //    else
        //    {
        //        //如果位置一直在改变，这样位置的同步就不可期了，那就先告诉服务器现在的位置
        //        if (m_unReqMoreCount > 8)
        //        {
        //            m_unSendMoveMsgCount = 6;
        //            m_unReqMoreCount = 0;
        //            SendMoveMsgToServer(m_unLastReqX, m_unLastReqY, m_unLastReqX, m_unLastReqY, m_unLastReqTM, m_unLastReqRadian);
        //        }
        //    }
        //}

        private bool m_bFirst = true;
        private float m_fLastSendTime;
        private uint m_unLastSendX = 0; //最后发给服务器的位置X
        private uint m_unLastSendY = 0; //最后发给服务器的位置Y
        private uint m_unLastSendToX = 0; //最后发给服务器的目标X
        private uint m_unLastSendToY = 0; //最后发给服务器的目标Y
        public bool SendMoveMsgToServer(Vector3 curPos, Vector3 tarPos)
        {
            return false;//SendMoveMsgToServer((uint)curPos.x, (uint)curPos.z, (uint)tarPos.x, (uint)tarPos.z, (uint)lgSelfPlayer.instance.g_mgr.g_netM.CurServerTimeStampMS, m_unLastReqRadian);
        }
        private bool SendMoveMsgToServer(uint x, uint y, uint tox, uint toy, uint tm,float radian)
        {
            if (muNetCleint.instance.CurServerTimeStamp - m_fLastSendTime < 0.5f) return false;
            if (m_unLastSendX != x || m_unLastSendY != y || m_unLastSendToX != tox || m_unLastSendToY != toy)
            {
                m_unLastSendX = x;
                m_unLastSendY = y;
                m_unLastSendToX = tox;
                m_unLastSendToY = toy;
                
                Variant msg = Variant.alloc();
                msg["frm_x"] = m_unLastSendX;
                msg["frm_y"] = m_unLastSendY;
                msg["to_x"] = m_unLastSendToX;
                msg["to_y"] = m_unLastSendToY;
                msg["start_tm"] = tm;
                msg["radian"] = radian;
                sendRPC(PKG_NAME.C2S_MOVE, msg);
                m_fLastSendTime = muNetCleint.instance.CurServerTimeStamp;
                m_fSyncPosTime = 0.5f;
                Variant.free(msg);

                debug.Log("发送 移动 " + m_unLastSendX + " " + m_unLastSendY + "   to  " + m_unLastSendToX + " " + m_unLastSendToY);
                //   + " " + tm + " " + radian
                //   );

                return true;
            }

            return false;
        }


        public void sendstop(uint x, uint y, uint face, float tm,bool force =false)
        {
            ReqChangeMoveMsg(x, y, x, y, (uint)tm, -1, force);
        }

        //public void sendmoveto(float frm_x, float frm_y, float to_x, float to_y, float start_tm,bool force=false)
        //{
        //    //Variant msg = Variant.alloc();
        //    //msg["frm_x"] = frm_x;
        //    //msg["frm_y"] = frm_y;
        //    //msg["to_x"] = to_x;
        //    //msg["to_y"] = to_y;
        //    //msg["start_tm"] = start_tm;
        //    //sendRPC(PKG_NAME.C2S_MOVE, msg);
        //    //Variant.free(msg);

        //    ReqChangeMoveMsg((uint)frm_x, (uint)frm_y, (uint)to_x, (uint)to_y, (uint)start_tm,-1f, force);
        //}

        //public void sendmoveRadian(float frm_x, float frm_y, float radian, float start_tm)
        //{
        //    //float ori = (radian * 180) / (float)(Math.PI * 100);
        //    LGMap lgm =  GRClient.instance.g_gameM.getObject(OBJECT_NAME.LG_MAP) as LGMap;
        //    Vec2 tp = lgm.getFarthestGPosByOri(frm_x, frm_y, radian * (float)Math.PI / 180);

        //    float to_x = (float)(tp.x ) + GameConstant.GEZI;
        //    float to_y = (float)(tp.y ) + GameConstant.GEZI;
        //    ReqChangeMoveMsg((uint)frm_x, (uint)frm_y, (uint)to_x, (uint)to_y, (uint)start_tm, radian);

        //    ////if (moveTick > 20)
        //    //{
        //    //    //    debug.Log("处理移动消息.9..");
        //    //    //    moveTick = 0;
        //    //    Variant msg = Variant.alloc();
        //    //    msg["frm_x"] = frm_x;
        //    //    msg["frm_y"] = frm_y;
        //    //    msg["iid"] = 0;
        //    //    msg["start_tm"] = start_tm;
        //    //    msg["radian"] = radian;
        //    //    sendRPC(PKG_NAME.C2S_MOVE, msg);
        //    //    Variant.free(msg);
        //    //}
        //}

        public void pos_correct(Variant msgData)
        {
            debug.Log("KKKKUUUUU"+ msgData.dump ());
            uint iid = msgData["iid"]._uint;
            ProfessionRole pr = OtherPlayerMgr._inst.GetOtherPlayer(iid);
            if (pr != null)
            {

            }
            else {
                float to_x = msgData["x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                float to_y = msgData["y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                MonsterRole role = MonsterMgr._inst.getServerMonster(iid);
                if (role != null) {
                    Vector3 vec = new Vector3(to_x, role.m_curModel.position.y, to_y);
                    if (GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3342]  || GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3358])
                    {//解决多层阻挡点寻路的问题
                        role.pos_correct(vec);
                    }
                    else
                    {
                        NavMeshHit hit;
                        NavMesh.SamplePosition(vec, out hit, 100f, role.m_layer);
                        role.pos_correct(hit.position);
                    }
                }
            }


        }

        public void move(Variant msgData)
        {
            //RoleMgr._instance.onMove(msgData);
            uint iid = msgData["iid"]._uint;
            ProfessionRole pr = OtherPlayerMgr._inst.GetOtherPlayer(iid);
            if (a3_insideui_fb.instance != null && PlayerModel.getInstance().inFb)
            {
                a3_insideui_fb.instance.Cancel();
            }
            if (pr != null)
            {
                float to_x = msgData["to_x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                float to_y = msgData["to_y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;

                Vector3 vec = new Vector3(to_x, pr.m_curModel.position.y, to_y);
                if (GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3342])
                {//解决多层阻挡点寻路的问题
                    pr.SetDestPos(vec);
                }
                else
                {
                    NavMeshHit hit;
                    
                    NavMesh.SamplePosition(vec, out hit, 100f, pr.m_layer);
                    pr.SetDestPos(hit.position);
                }


            }
            else
            {
                float to_x = msgData["to_x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                float to_y = msgData["to_y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
                MonsterRole role = MonsterMgr._inst.getServerMonster(iid);
                if (role != null)
                {
                    Vector3 vec = new Vector3(to_x, role.m_curModel.position.y, to_y);
                    if (GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3342] || GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3358])
                    {//解决多层阻挡点寻路的问题
                        role.SetDestPos(vec);
                    }
                    else
                    {
                        NavMeshHit hit;
                        NavMesh.SamplePosition(vec, out hit, 100f, role.m_layer);


                        role.SetDestPos(hit.position);
                    }
                }

            }



            //LGAvatarGameInst to = getRoleByIID(data["iid"]);
            //if (to == null) return;
            //to.addMoving(data);


            //Variant moveinfo = new Variant();
            //if (data == null || !data.ContainsKey("to_x"))
            //{
            //    GameTools.PrintError("addMoving err!");
            //    return;
            //}

            ////debug.Log("收到移动消息 " + data.dump());
            //float to_x = data["to_x"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
            //float to_y = data["to_y"]._float / GameConstant.PIXEL_TRANS_UNITYPOS;
            //debug.Log("收到移动消息 " + to_x + "  " + to_y);

            ////setMoveInfo(moveinfo);
        }

        public void on_stop(Variant msgData)
        {
          //  RoleMgr._instance.onStop(msgData);
        }

        public Vector3 GetLastSendXY() {
            return new Vector3(m_unLastSendToX,0, m_unLastSendToY);
        }
    }
}
