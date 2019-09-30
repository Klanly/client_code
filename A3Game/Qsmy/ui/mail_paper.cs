//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameFramework;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using Cross;

//namespace MuGame
//{
//    class mail_paper : Window
//    {
//        private Text _titel;
//        private InputField msgInput;
//        private Text textNum;
//        private BaseButton cloBtn;
//        private BaseButton sendBtn;
//        private bool isSend=false;
//        public override void init()
//        {
//            base.init();
//            cloBtn = new BaseButton(transform.FindChild("closeBtn"));
//            cloBtn.onClick = onClickClose;
//            sendBtn = new BaseButton(transform.FindChild("sendBtn"));
//            sendBtn.onClick = onClickSend;

//            _titel = transform.FindChild("titel/bg/text").GetComponent<Text>();
//            textNum = transform.FindChild("msg/bg/num/Text").GetComponent<Text>();
//            msgInput = transform.FindChild("msg/bg/InputField").GetComponent<InputField>();
//        }

//        public override void onShowed()
//        {
//            E_mailProxy.getInstance().addEventListener(E_mailProxy.lis_sendMail_res, sendMailRes);
//            //msgInput.onEndEdit.AddListener(onSubmit);
//            //msgInput.onValueChange.AddListener(onSubmit);
//            base.onShowed();
//            cloBtn.addEvent();
//            sendBtn.addEvent();
//            this.transform.SetAsLastSibling();
//            switch ((int)uiData[0])
//            {
//                case 4:
//                    _titel.text = ContMgr.getCont("mail_send") + uiData[2].ToString();
//                    break;
//                case 1:
//                    _titel.text = ContMgr.getCont("mail_send_fam");
//                    break;
//                default:
//                    break;
//            }
//        }

//        //void onValueChange(string str)
//        //{
//        //    int a = msgInput.text.Length;
//        //    textNum.text = (msgInput.characterLimit - a).ToString();
//        //}

//        //void onSubmit(string str)
//        //{
//        //    msgInput.text = KeyWord.filter(str);
//        //    int a = msgInput.text.Length;
//        //    textNum.text = (msgInput.characterLimit - a).ToString();
//        //}

//        public override void onClosed()
//        {
//            base.onClosed();
//            cloBtn.removeAllListener();
//            sendBtn.removeAllListener();
//            E_mailProxy.getInstance().removeEventListener(E_mailProxy.lis_sendMail_res, sendMailRes);
//            msgInput.text = "";
//            //msgInput.onValueChange.RemoveListener(onSubmit);
//            //msgInput.onValueChange.RemoveListener(onValueChange);
//        }

//        void onClickClose(GameObject go)
//        {
//            InterfaceMgr.getInstance().close(InterfaceMgr.MAILPAPER);
//            isSend = false;
//        }

//        void onClickSend(GameObject go)
//        {
//            if (msgInput.text == ""||isSend)
//                return;
//            switch ((int)uiData[0])
//	        {
//                case 4:
//                    E_mailProxy.getInstance().sendNewMail(4, msgInput.text, (uint)((int)uiData[1]));
//                    break;
//                case 1:
//                    E_mailProxy.getInstance().sendNewMail(1, msgInput.text);
//                    break;
//		        default:
//                    break;
//	        }
//            isSend = true;
//        }

//        void sendMailRes(GameEvent e)
//        {
//            if (this.gameObject.activeSelf == true&&e.data.ContainsKey("res"))
//            {
//                if (e.data["res"] > 0)
//                {
//                    flytxt.instance.fly(ContMgr.getCont("mail_send_suc"), 1);
//                    isSend = false;
//                    if ((int)uiData[0] == 4)
//                    {
//                        mailData data = new mailData();
//                        data.frmcid = (int)uiData[1];
//                        //data.frmsex = PlayerModel.getInstance().sex;
//                        data.msg = msgInput.text;
//                        data.frmname = uiData[2].ToString();
//                        data.time = E_mailProxy.getInstance().getTime(e.data["tm"]);
//                        data.seconds = e.data["tm"]._int32;
//                        data.cid = (int)PlayerModel.getInstance().cid;
//                        if (E_mailModel.getInstance().personalMailDic.ContainsKey((int)uiData[1]))
//                            E_mailModel.getInstance().personalMailDic[(int)uiData[1]].Add(data);
//                        else
//                        {
//                            E_mailModel.getInstance().personalMailDic[data.frmcid] = new List<mailData>();
//                            E_mailModel.getInstance().personalMailDic[data.frmcid].Add(data);
//                        }
//                        string PerMsg = data.frmcid.ToString() + "#!#&" + data.frmsex.ToString() + "#!#&" + data.cid.ToString()
//                            + "#!#&" + data.time + "#!#&" + data.frmname + "#!#&" + data.msg + "#!#&" + data.seconds + "#!#&" + data.clanc + "#)#&";
//                        data.str = PerMsg;
//                        E_mailModel.getInstance().perLocalStr.Add(data);
//                        E_mailModel.getInstance().saveLocalData(E_mailModel.getInstance().perLocalStr, 4);
//                        //FileMgr.saveString(FileMgr.TYPE_MAIL, "per", FileMgr.loadString(FileMgr.TYPE_MAIL, "per") + PerMsg);
//                    }
//                    InterfaceMgr.getInstance().close(InterfaceMgr.MAILPAPER);
//                }
//            }
//            if (this.gameObject.activeSelf == true && e.data.ContainsKey("tp"))
//            {
//                flytxt.instance.fly(ContMgr.getCont("mail_send_suc"), 1);
//                InterfaceMgr.getInstance().close(InterfaceMgr.MAILPAPER);
//            }
             
//        }
//    }
//}
