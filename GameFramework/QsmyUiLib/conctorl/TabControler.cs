using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace GameFramework
{
    /******
     * TabControler的用法：
     * 1.tabPanel中的button统一以 btn_xxx 和 btn_xxxSel 命名
     * 2.切换页中的panel统一以 panel_xxx 命名
     * 3.切换页panel和tabPanel属于同级目录
     * *****/
    public class TabControler
    {
        private int _selectedIndex = 0;
        private GameObject[] _tabs;
        private GameObject[] _selTabs;
        private GameObject[] _panels;
        public TabControler()
        {

        }
        public void create(GameObject tabs, GameObject main)
        {
            int length = tabs.transform.childCount / 2;
            _tabs = new GameObject[length];
            _selTabs = new GameObject[length];
            _panels = new GameObject[length];
            for (int i = 0; i < length; i++)
            {
                _tabs[i] = tabs.transform.GetChild(i * 2).gameObject;
                _selTabs[i] = tabs.transform.GetChild(i * 2 + 1).gameObject;
                _panels[i] = main.transform.FindChild(tabs.transform.GetChild(i * 2).name.Replace("btn_", "panel_")).gameObject;
                _tabs[i].SetActive(true);
                _selTabs[i].SetActive(false);
                _panels[i].SetActive(false);
            }
            if (_tabs[0] && _panels[0])
            {
                _tabs[0].SetActive(false);
                _selTabs[0].SetActive(true);
                _panels[0].SetActive(true);
            }
        }
        public void setSelectedIndex(int index)
        {
            if (_selectedIndex == index)
            {
                return;
            }
            if (index != -1)
            {
                _tabs[_selectedIndex].SetActive(true);
                _selTabs[_selectedIndex].SetActive(false);
                if (_panels[_selectedIndex])
                {
                    _panels[_selectedIndex].SetActive(false);
                }

                _selectedIndex = index;

                _tabs[_selectedIndex].SetActive(false);
                _selTabs[_selectedIndex].SetActive(true);
                if (_panels[_selectedIndex])
                {
                    _panels[_selectedIndex].SetActive(true);
                }
            }
        }
        public int getIndexByName(string name)
        {
            for (int i = 0; i < _tabs.Length; i++)
            {
                if (_tabs[i].name == name)
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
    }
}
