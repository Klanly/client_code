using UnityEngine;

public class PT_FFTextNum
{
    public int max_UIOBJ_text_num = 1;

    public int m_nBottomPos = 0;
    public int m_nTopPos = 0;
    public float m_fWight_Half = 2f;//字宽的一半
    public Transform[] m_tfNumObj = null;

    //public bool m_btestlog = false;

    public Transform PopOneSleepNum()
    {
        if (m_nTopPos - m_nBottomPos >= max_UIOBJ_text_num) return null;
        int pos = m_nTopPos % max_UIOBJ_text_num;
        ++m_nTopPos;

        //if(m_btestlog) Debug.Log("m_nTopPos = " + m_nTopPos);
        Transform res_back = m_tfNumObj[pos];
        m_tfNumObj[pos] = null;

        if (res_back == null)
        {
            Debug.LogError("FFTextNum res_back is null");
        }
        return res_back;
    }

    public void PushBackOneNum(Transform tfnum)
    {
        int pos = m_nBottomPos % max_UIOBJ_text_num;
        m_tfNumObj[pos] = tfnum;
        ++m_nBottomPos;
    }

}

public class PT_FFTextNode
{
    public int m_nCurNum = -1;
    public Transform m_tfNumNode = null;
    public int[] m_nLinkNumPos = { 0, 0, 0, 0, 0, 0 };
    public Transform[] m_tfLinkNumObj = { null, null, null, null, null, null };

    public void Start(int num, PT_FFTextNum[] s_ffTextNums)
    {
        m_nCurNum = num;

        if (num <= 0) return;

        //Debug.Log("FlyText = " + num);
        //m_tfNumNode.SetParent(null, false);

        if (num > 999999) num = 999999;

        int[] n = { 0, 0, 0, 0, 0, 0 };
        n[0] = num / 100000; num = num - n[0] * 100000;
        n[1] = num / 10000; num = num - n[1] * 10000;
        n[2] = num / 1000; num = num - n[2] * 1000;
        n[3] = num / 100; num = num - n[3] * 100;
        n[4] = num / 10; num = num - n[4] * 10;
        n[5] = num;

        int linkerpos = -1;
        float right_pos = 0f;
        bool no_enough_num = false;
        for (int i = 0; i < 6; ++i)
        {
            if (linkerpos == -1 && n[i] == 0)
            {
                continue;
            }
            else
            {
                PT_FFTextNum fftn = s_ffTextNums[n[i]];
                Transform tfnum = fftn.PopOneSleepNum();
                if (tfnum != null)
                {
                    right_pos += fftn.m_fWight_Half;
                    tfnum.SetParent(m_tfNumNode, false);
                    tfnum.localPosition = new Vector3(right_pos, 0f, 0f);
                    right_pos += fftn.m_fWight_Half;

                    tfnum.localRotation = Quaternion.identity;
                    tfnum.localScale = Vector3.one;

                    ++linkerpos;

                    m_nLinkNumPos[linkerpos] = n[i];
                    m_tfLinkNumObj[linkerpos] = tfnum;
                }
                else
                {
                    no_enough_num = true;
                    break;
                }
            }
        }

        if (no_enough_num)
        {
            //将数字移出去
            for (int i = 0; i < 6; ++i)
            {
                if (m_tfLinkNumObj[i] != null)
                {
                    m_tfLinkNumObj[i].localPosition = new Vector3(0f, 65535f, 0f);
                }
            }
        }
        else
        {
            for (int i = 0; i < 6; ++i)
            {
                float len_half = right_pos / 2f;
                if (m_tfLinkNumObj[i] != null)
                {
                    m_tfLinkNumObj[i].Translate(-len_half, 0f, 0f);
                }
            }
        }
    }

    public void Clear(ref Transform s_tfTempPool,  ref PT_FFTextNum[] s_ffTextNums)
    {
        m_nCurNum = -1;
        for (int i = 0; i < 6; ++i)
        {
            if (m_tfLinkNumObj[i] != null)
            {
                s_ffTextNums[m_nLinkNumPos[i]].PushBackOneNum(m_tfLinkNumObj[i]);
                m_tfLinkNumObj[i].SetParent(s_tfTempPool);
                m_tfLinkNumObj[i] = null;
            }
        }
    }
}

public class PicText
{
    //不显示的数字放这里
    private Transform s_tfTempPool = null;

    //1234567890这十个数字
    private PT_FFTextNum[] s_ffTextNums = new PT_FFTextNum[10];

    //数字的节点
    private PT_FFTextNode[] s_tfFlyNode = null;

    public void Init(GameObject data_obj, int nodecount, int num_count, float width_half)
    {
        s_tfFlyNode = new PT_FFTextNode[nodecount];

        s_tfTempPool = data_obj.transform;
        data_obj.SetActive(false);
        //s_tfTempPool.hideFlags = HideFlags.HideInHierarchy;

        Init_AddNum(data_obj, "0", 0, num_count, width_half);
        Init_AddNum(data_obj, "1", 1, num_count, width_half);
        Init_AddNum(data_obj, "2", 2, num_count, width_half);
        Init_AddNum(data_obj, "3", 3, num_count, width_half);
        Init_AddNum(data_obj, "4", 4, num_count, width_half);
        Init_AddNum(data_obj, "5", 5, num_count, width_half);
        Init_AddNum(data_obj, "6", 6, num_count, width_half);
        Init_AddNum(data_obj, "7", 7, num_count, width_half);
        Init_AddNum(data_obj, "8", 8, num_count, width_half);
        Init_AddNum(data_obj, "9", 9, num_count, width_half);

        //s_ffTextNums[9].m_btestlog = true;
        for (int i = 0; i < nodecount; ++i)
        {
            s_tfFlyNode[i] = new PT_FFTextNode();
        }
    }

    private void Init_AddNum(GameObject data_obj, string name, int pos, int pool_size, float width_half)
    {
        GameObject num_obj = data_obj.transform.FindChild(name).gameObject;
        PT_FFTextNum pt_num = new PT_FFTextNum();
        pt_num.m_fWight_Half = width_half;
        pt_num.max_UIOBJ_text_num = pool_size;
        pt_num.m_tfNumObj = new Transform[pt_num.max_UIOBJ_text_num];

        for (int i = 0; i < pt_num.max_UIOBJ_text_num; ++i)
        {
            pt_num.m_tfNumObj[i] = GameObject.Instantiate<GameObject>(num_obj).transform;
            pt_num.m_tfNumObj[i].transform.SetParent(s_tfTempPool, false);
        }

        GameObject.Destroy(num_obj);

        s_ffTextNums[pos] = pt_num;
        pt_num = null;
    }

    public void SetNodeID(int id, Transform linker)
    {
        if( id >=0 && id < s_tfFlyNode.Length )
        {
            s_tfFlyNode[id].m_tfNumNode = linker;
        }
        else
        {
            Debug.LogError("SetNodeID = " + id);
        }
    }

    public void SetNodeNum(int id, int num)
    {
        if (id >= 0 && id < s_tfFlyNode.Length)
        {
            if (s_tfFlyNode[id].m_nCurNum == num) return;

            //Debug.Log("SetNodeNumSetNodeNumSetNodeNumSetNodeNumSetNodeNumSetNodeNumSetNodeNumSetNodeNumSetNodeNum");

            s_tfFlyNode[id].Clear(ref s_tfTempPool, ref s_ffTextNums);
            if( num >= 0 )
            {
                s_tfFlyNode[id].Start(num, s_ffTextNums);
            }
        }
        else
        {
            Debug.LogError("SetNodeNum = " + id);
        }
    }

    public void ClearNodeNum(int id)
    {
        if (id >= 0 && id < s_tfFlyNode.Length)
        {
            s_tfFlyNode[id].Clear(ref s_tfTempPool, ref s_ffTextNums);
        }
        else
        {
            Debug.LogError("ClearNodeNum = " + id);
        }
    }
}

