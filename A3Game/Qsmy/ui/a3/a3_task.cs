using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame.Qsmy.ui.a3
{
    class a3_task : Window
    {

        //private GameObject Button01;
        //public GameObject Button02;
        //public GameObject Button03;

        public override void init()
        {
            Debug.Log("");//打开按钮
            

            //关闭按钮

            BaseButton Button = new BaseButton(transform.FindChild("Image/Button"));
            Button.onClick = onClose;
            //打开主线任务
            BaseButton Button01 = new BaseButton(transform.FindChild("Image/Image01/Button01"));
            Button01.onClick = onOpen;
            //打开支线任务
            BaseButton Button02 = new BaseButton(transform.FindChild("Image/Image01/Button02"));

            Button02.onClick = wend;
            //打开讨伐任务
            BaseButton Button03 = new BaseButton(transform.FindChild("Image/Image01/Button03"));
            Button03.onClick = weu;

        }
        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_TASK);//从主脚本中获取实力带入到现在脚本中
        }
        void onOpen(GameObject go)
        {
            InterfaceMgr.getInstance().open(InterfaceMgr.A3_TASK);
            //任务界面

            Transform bar = transform.FindChild("Image/Image01/Image03");
            bar.gameObject.SetActive(true);
            Transform bbr = transform.FindChild("Image/Image01/Image04");
            bbr.gameObject.SetActive(true);
            Transform bcr = transform.FindChild("Image/Image01/Image05");
            bcr.gameObject.SetActive(false);
            Transform bdr = transform.FindChild("Image/Image01/Image06");
            bdr.gameObject.SetActive(false);
            Transform ber = transform.FindChild("Image/Image01/Image08");
            ber.gameObject.SetActive(false);
            Transform bfr = transform.FindChild("Image/Image01/Image09");
            bfr.gameObject.SetActive(false);

        }

        void wend(GameObject go)
        {
            InterfaceMgr.getInstance().open(InterfaceMgr.A3_TASK);
            Transform bar = transform.FindChild("Image/Image01/Image03");
            bar.gameObject.SetActive(false);
            Transform bbr = transform.FindChild("Image/Image01/Image04");
            bbr.gameObject.SetActive(false);
            Transform bcr = transform.FindChild("Image/Image01/Image05");
            bcr.gameObject.SetActive(true);
            Transform bdr = transform.FindChild("Image/Image01/Image06");
            bdr.gameObject.SetActive(true);
            Transform ber = transform.FindChild("Image/Image01/Image08");
            ber.gameObject.SetActive(false);
            Transform bfr = transform.FindChild("Image/Image01/Image09");
            bfr.gameObject.SetActive(false);


        }
        void weu(GameObject go)
        {
            InterfaceMgr.getInstance().open(InterfaceMgr.A3_TASK);
            Transform bar = transform.FindChild("Image/Image01/Image03");
            bar.gameObject.SetActive(false);
            Transform bbr = transform.FindChild("Image/Image01/Image04");
            bbr.gameObject.SetActive(false);
            Transform bcr = transform.FindChild("Image/Image01/Image05");
            bcr.gameObject.SetActive(false);
            Transform bdr = transform.FindChild("Image/Image01/Image06");
            bdr.gameObject.SetActive(false);
            Transform ber = transform.FindChild("Image/Image01/Image08");
            ber.gameObject.SetActive(true);
            Transform bfr = transform.FindChild("Image/Image01/Image09");
            bfr.gameObject.SetActive(true);

 
        }

    }

}
