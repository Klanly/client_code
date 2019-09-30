using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
   public class DropItemUIMgr
    {
       int showTime = 5;
        private static DropItemUIMgr instance;

        public static DropItemUIMgr getInstance()
        {
           if (instance==null)
                   instance=new DropItemUIMgr();
            return instance;
        }
        List<DropItemUI> lPool;
        Dictionary<INameObj, DropItemUI> dItem;
         List<DropItemUI> lItem;
        Transform dropItemUILayer;//新建个掉落物品UI层
        TickItem process;
        public DropItemUIMgr()
        {
            lPool = new List<DropItemUI>();
            dItem = new Dictionary<INameObj, DropItemUI>();
            lItem = new List<DropItemUI>();
            dropItemUILayer = GameObject.Find("dropItemLayer").transform;
            process = new TickItem(onUpdate);
            TickMgr.instance.addTick(process);

        }
       public void show(INameObj dropObj,string name)
        {
            if (!dItem.ContainsKey(dropObj))
           {
               DropItemUI item;
                if (lPool.Count==0)
                {
                    GameObject temp = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_dropItemUI");
                    GameObject go = GameObject.Instantiate(temp) as GameObject;
                    go.transform.SetParent(dropItemUILayer, false);
                    item = new DropItemUI(go.transform);
                }
                else
                {
                    item = lPool[0];
                    item.visiable = true;
                    lPool.RemoveAt(0);
                }
                item.refresh(dropObj, name);
                lItem.Add(item);
                dItem[dropObj] = item;
                if (dropObj is DropItem)
                {
                    item.refresShowName(showTime);
                }
           }
            else
            {
                dItem[dropObj].refresh(dropObj, name);
                dItem[dropObj].refresShowName(showTime);
            }
        }
       public void hideAll()
       {
           dropItemUILayer.gameObject.SetActive(false);
       }
       public void showAll()
       {
           dropItemUILayer.gameObject.SetActive(true);
       }
        public void hideOne(INameObj dropItem)
        {
            if (!dItem.ContainsKey(dropItem)) return;
            DropItemUI item = dItem[dropItem];
            item.clear();
        }
      public void removeDropItem(DropItemUI diu)
       {
           lItem.Remove(diu);
       }
       public void hide(INameObj dropItem)
       {
           if (!dItem.ContainsKey(dropItem)) return;
           DropItemUI item = dItem[dropItem];
           item.visiable = false;
           item.clear();
           dItem.Remove(dropItem);
           lItem.Remove(item);
           lPool.Add(item);
       }
       void    onUpdate(float s)
       {
           if (lItem.Count>0)
           {
               foreach (DropItemUI item in lItem)
               {
                   if (item.gameObject==null)
                   {
                       lItem.Remove(item);
                       continue;
                   }
               }
               foreach (DropItemUI item in lItem)
               {
                   item.update();
               }
           }
       }
    }
  public class DropItemUI : Skin
   {
       public INameObj _dropObj;

       Text txtName;
       public DropItemUI(Transform trans)
            : base(trans)
        {
            iniUI();
        }
       void iniUI()
       {
           this.transform.localScale = Vector3.one;
           txtName = this.getComponentByPath<Text>("txtName");
       }
       public void refresh(INameObj dropObj,string name)
       {
           _dropObj = dropObj;
           if (dropObj is DropItem)
           {
           }
           else
               txtName = null;
           if (txtName)
           {
               txtName.text = name;
               txtName.gameObject.SetActive(true); 
           }
        
       }
       TickItem showtime;
       float times = 0;
       int i;
       void onUpdates(float s)
       {
           times += s;
           if (times >= 1)
           {
               i--;
               if (i == 0)
               {
                   i = 0;
                   if (isDes) return;
                   txtName.gameObject.SetActive(false);
                   TickMgr.instance.removeTick(showtime);
                   GameObject.Destroy(this.gameObject);
                   isDes = true;
                   DropItemUIMgr.getInstance().removeDropItem(this);
                   showtime = null;
               }
               times = 0;
           }
       }
       public void  refresShowName(int time)
       {
           if (time <= 0) return;
           showtime = new TickItem(onUpdates);
           TickMgr.instance.addTick(showtime);
           i = time;

       }

        bool isDes = false;
       public void clear()
       {
            //_dropObj = null;
            // GameObject.Destroy(this.gameObject);
            if (isDes) return;
            txtName.gameObject.SetActive(false);
            TickMgr.instance.removeTick(showtime);
            showtime = null;
            GameObject.Destroy(this.gameObject);
            isDes = true;
            DropItemUIMgr.getInstance().removeDropItem(this);
        }
       public void update()
       {
            //if (_dropObj == null)
            //{
            //    txtName.gameObject.SetActive(false);
            //}else 
            pos = _dropObj.getHeadPos();
        }
   }

}
