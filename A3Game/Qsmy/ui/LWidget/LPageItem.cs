using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LPageItem : MonoBehaviour
{
    public Transform prefabItemParent;
    private static int currentPage = 0;
    public static Action ResetPage { get { return () => { currentPage = 0; }; } }
    public void Init(List<GameObject> datas)
    {
        for (int i = 0; i < datas.Count; i++)
            datas[i].transform.SetParent(prefabItemParent, false);
    }

    public void Init(List<GameObject> datas, GameObject prefab, string path, string[,] itemInfo)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            prefab.transform.GetChild(0).GetComponent<Text>().text = itemInfo[0, i + currentPage * 6];
            prefab.transform.GetChild(1).GetComponent<Text>().text = itemInfo[1, i + currentPage * 6];
            Transform parentTran = (Instantiate(prefab) as GameObject).transform;
            parentTran.gameObject.SetActive(true);
            Transform newTran = parentTran.FindChild(path);
            datas[i].transform.SetParent(newTran, false);
            datas[i].transform.FindChild("ClickArea").SetParent(parentTran,false);
            parentTran.transform.SetParent(prefabItemParent, false);
        }
        currentPage++;
    }
}
