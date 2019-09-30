using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HiddenItem : MonoBehaviour
{
    public bool useAni = false;
    public float hideSec = 5;
    public void hide()
    {
        if (useAni)
        {
            Animator ani = gameObject.GetComponent<Animator>();
            if (ani != null)
                ani.SetTrigger("hide");
            else
                gameObject.SetActive(false);
        }
        else if (hideSec == 0)
        {
            dispose();
            return;
        }

        if (hideSec > 0f)
            Invoke("dispose", hideSec);
    }

    void dispose()
    {
        Destroy(gameObject);
    }

}

