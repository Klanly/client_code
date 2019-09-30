
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{

    public class TabControl
    {
        public Action<TabControl> onClickHanle;
        private int _selectedIndex = -1;
        private List<Button> _tabs;
        public bool canClick = true;

        public int m_hideType = 0;

        public bool m_isClicked = false;  //是否允许重复点多次

        public TabControl()
        {

        }

        public void create(GameObject tabs, GameObject main, int selectedIdx = 0,int hideType=0, bool isClicked = false)
        {
            m_hideType = hideType;
            m_isClicked = isClicked;
            int length = tabs.transform.childCount;
            _tabs = new List<Button>();
            for (int i = 0; i < length; i++)
            {
                _tabs.Add(tabs.transform.GetChild(i).GetComponent<Button>());
                EventTriggerListener.Get(_tabs[i].gameObject).onClick = onClick;
            }

            if (selectedIdx >= 0)
            {
                setSelectedIndex(selectedIdx,true);
            }
        }
        public void setSelectedIndex(int index,bool forceRunHandle=false)
        {
            if (_selectedIndex == index)
            {
                if ((forceRunHandle && onClickHanle != null) || (m_isClicked && onClickHanle != null))
                    onClickHanle(this);
                return;
            }
               
            unSelectAll();

            Button bt = _tabs[index];
            if (m_hideType == 1)
                bt.transform.localScale = Vector3.zero;
            else
            bt.interactable = true;
            _selectedIndex = index;

            if (onClickHanle != null)
                onClickHanle(this);
        }

        private Dictionary<int, int> dUnable = new Dictionary<int, int>();
        public void setEnable(int idx, bool enable)
        {
            if (enable)
            {
                if (dUnable.ContainsKey(idx))
                    dUnable.Remove(idx);
            }
            else
            {
                dUnable[idx] = 1;
            }
        }

        private void onClick(GameObject go)
        {
            if (!canClick)
                return;

            int idx = _tabs.IndexOf(go.GetComponent<Button>());

            if (dUnable.ContainsKey(idx))
                return;

            setSelectedIndex(idx);
        }

        public void unSelectAll()
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                //  _tabs[i].enabled = false;
                if (m_hideType == 1)
                    _tabs[i].transform.localScale = Vector3.one;
                else
                    _tabs[i].interactable = false;
            }
        }

        public int getIndexByName(string name)
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_tabs[i].gameObject.name == name)
                {
                    return i;
                }
            }
            return 0;
        }
        public int getSeletedIndex()
        {
            return _selectedIndex;
        }

        public void dispose()
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                EventTriggerListener.Get(_tabs[i].gameObject).clearAllListener();
                onClickHanle = null;
            }
            _tabs = null;
        }

        public void forEach(Action<int> fun)
        {
            if (fun == null)
                return;

            for (int i = 0; i < _tabs.Count; i++)
            {
                fun(i);
            }
        }

    }
}


