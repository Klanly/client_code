using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using Cross;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace MuGame
{
    public class PlayerChatUIMgr
    {
        private static PlayerChatUIMgr instacne;
        public static PlayerChatUIMgr getInstance()
        {
            if (instacne == null)
                instacne = new PlayerChatUIMgr();
            return instacne;
        }


        TickItem process;

        private List<PlayerChatItem> lPool;
        private Dictionary<INameObj, PlayerChatItem> dItem;
        private List<PlayerChatItem> lItem;
        private Transform playerChatLayer;
        public PlayerChatUIMgr()
        {
            lItem = new List<PlayerChatItem>();
            dItem = new Dictionary<INameObj, PlayerChatItem>();
            lPool = new List<PlayerChatItem>();
            playerChatLayer = GameObject.Find("playerChat").transform;
            process = new TickItem(onUpdate);

            lActiveItem = new List<ActiveChatItem>();
            lActiveItemPool = new List<ActiveChatItem>();

            TickMgr.instance.addTick(process);
        }



        public void show(INameObj avatar,string msg)
        {
            if (!dItem.ContainsKey(avatar))
            {

                PlayerChatItem item;
                if (lPool.Count == 0)
                {
                    GameObject temp = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_chat_user");
                    GameObject go = GameObject.Instantiate(temp) as GameObject;
                    go.transform.SetParent(playerChatLayer, false);
                    item = new PlayerChatItem(go.transform);
                }
                else
                {
                    item = lPool[0];
                    item.visiable = true;
                    lPool.RemoveAt(0);
                }

                item.refresh(avatar, msg);
                lItem.Add(item);
                dItem[avatar] = item;
           
                if (avatar is ProfessionRole)
                {
                    
                    item.refresShowChat(4);
                }
              
            }
            else
            {
                dItem[avatar].refresh(avatar, msg);
                dItem[avatar].refresShowChat(3);
            }
            
        }
      
        public void hideAll()
        {
            playerChatLayer.gameObject.SetActive(false);
        }
        public void showAll()
        {
            playerChatLayer.gameObject.SetActive(true);
        }

        public void hide(INameObj role)
        {
            if (role.roleName == PlayerModel.getInstance().name)
            {
                debug.Log("a");
            }
            if (!dItem.ContainsKey(role))
                return;
            PlayerChatItem item = dItem[role];
            item.clear();
            item.visiable = false;
            dItem.Remove(role);
            lItem.Remove(item);
            lPool.Add(item);
        }  
        private int tick = 0;
        private void onUpdate(float s)
        {

            if (lItem.Count > 0)
            {
                foreach (PlayerChatItem item in lItem)
                {
                    item.update();
                }
            }

            if (lActiveItem.Count > 0)
            {
                List<ActiveChatItem> l = new List<ActiveChatItem>();
                foreach (ActiveChatItem activeItem in lActiveItem)
                {
                    if (activeItem.update())
                    {
                        l.Add(activeItem);
                    }
                }

                foreach (ActiveChatItem activeItem in l)
                {
                    clearActive(activeItem);
                }

            }

        }


        private List<ActiveChatItem> lActiveItem;
        private List<ActiveChatItem> lActiveItemPool;
        public void clearActive(ActiveChatItem item)
        {
            item.clear();
            lActiveItem.Remove(item);
            lActiveItemPool.Add(item);
        }
    }

    public class ActiveChatItem : Skin
    {
        private Animator ani;
        private GRAvatar _avatar;

        public ActiveChatItem(Transform trans)
            : base(trans)
        {
            initUI();
        }

        void initUI()
        {
            ani = __mainTrans.GetComponent<Animator>();
        }

        public void clear()
        {
            _avatar = null;
        }

        public bool update()
        {
            pos = _avatar.getHeadPos();

            if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                this.visiable = false;
                return true;
            }
            return false;
        }
    }


    class PlayerChatItem : Skin
    {
        public INameObj _avatar;
   
       
        public Text txtChat;
        private Image bg;
        private Image bgImage;
        private bool chatshowed = false;
        private Image titleIcon;
        float expandWidth, expandHeight;

        public PlayerChatItem(Transform trans)
            : base(trans)
        {
            initUI();
        }

        public void clear()
        {
            _avatar = null;

        }

        void initUI()
        {
            bg = this.getComponentByPath<Image>("bg");
            bgImage = this.getComponentByPath<Image>("bgImage");
            expandWidth = bgImage.rectTransform.sizeDelta.x;
            expandHeight = bgImage.rectTransform.sizeDelta.y;

            txtChat = getComponentByPath<Text>("bgImage.uchat");
            titleIcon = getComponentByPath<Image>("title");

            bg.gameObject.SetActive(false);
            titleIcon.gameObject.SetActive(false);
            bgImage.gameObject.SetActive(false);
        
        }
        public void refresh(INameObj avatar,string msg)
        {
            _avatar = avatar;
            if (avatar is ProfessionRole)
            {
                
            }
            else
                txtChat = null;
            if (txtChat)
            {
                txtChat.text = GetChatText(msg);
                if (bgImage.gameObject.transform.parent.gameObject.GetComponent<RectTransform>().localPosition.x <= -568)
                {
                    txtChat.gameObject.SetActive(false);
                    bgImage.gameObject.SetActive(false);
                }
                else
                {
                    txtChat.gameObject.SetActive(true);
                    bgImage.gameObject.SetActive(true);
                }
                bgImage.rectTransform.sizeDelta = new Vector2(txtChat.preferredWidth + expandWidth/* + 10*/, txtChat.preferredHeight + expandHeight/* + 10*/);
            }

      
        }
        private string GetChatText(string msg)
        {
            int maxNewLine = 1, eachLineLimit, maxCount;
            if (a3_chatroom._instance != null)
                eachLineLimit = a3_chatroom._instance.lengthLimit;
            else
                eachLineLimit = 20;
            maxCount = eachLineLimit * (maxNewLine + 1);
            for (int i = 0; i < msg.Length; i++)
            {
                int curTextLen = Encoding.Default.GetByteCount(msg.Substring(0, i));
                if (curTextLen > maxCount)
                    return msg.Substring(0, i - 1);
                else if (maxNewLine > 0 && curTextLen > eachLineLimit)
                {
                    msg = msg.Insert(i - 1, "\n");
                    maxNewLine--;
                }
            }
            return msg;
        }
        TickItem showtime;
        float times = 0;
        int i;
        bool isself = false;
     
        
        void onUpdates(float s)
        {
            times += s;
            if (times >= 1)
            {
                i--;
                if (i ==0)
                {
                    i = 0;
                    bgImage.gameObject.SetActive(false);
                    txtChat.gameObject.SetActive(false);
                    TickMgr.instance.removeTick(showtime);
                    showtime = null;
                }
                times = 0;
            }

            
        }
        public void refresShowChat(int time, bool ismyself = false)
        {
            debug.Log("时间是多少::::::::：" + time);
            if (time <= 0)
            {
                bgImage.gameObject.SetActive(false);
                return;
            }
            else

            {
                if (bgImage.gameObject.transform.parent.gameObject.GetComponent<RectTransform>().localPosition.x <= -568)
                {
                    
                    bgImage.gameObject.SetActive(false);
                }
                else
                {
                    bgImage.gameObject.SetActive(true);
                }
            }
                //bgImage.gameObject.SetActive(true);
            if (bgImage != null)
            {
                showtime = new TickItem(onUpdates);
                TickMgr.instance.addTick(showtime);
            }
            i = time;
            if (ismyself)
                isself = true;
            else
                isself = false;
        }

        public void update()
        {
            pos = _avatar.getHeadPos();
        }

       
    }
    
}
