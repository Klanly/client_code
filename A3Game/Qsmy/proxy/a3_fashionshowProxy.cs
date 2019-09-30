using Cross;
using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MuGame
{
    class a3_fashionshowProxy : BaseProxy<a3_fashionshowProxy>
    {

        public static uint CHANGEFA = 2;
        public a3_fashionshowProxy()
        {
            addProxyListener(PKG_NAME.C2S_FASHIONSHOW, onfashionshow_info);
        }

        void onfashionshow_info(Variant data)
        {
            debug.Log("受到时装的协议：" + data.dump());
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            switch (res)
            {
                 //0:武器  1:胸甲
                case 1:
                    //身上穿的

                    if(data["dress_list"] != null && data["dress_list"].Count > 0)
                    {
                        A3_FashionShowModel.getInstance().nowfs[0]= data["dress_list"][0]._int;
                        A3_FashionShowModel.getInstance().nowfs[1] = data["dress_list"][1]._int;
                        if (data["dress_show"])
                        {
                            A3_FashionShowModel.getInstance().first_nowfs[0] = data["dress_list"][0]._int;
                            A3_FashionShowModel.getInstance().first_nowfs[1] = data["dress_list"][1]._int;
                        }
                        else
                            A3_FashionShowModel.getInstance().first_nowfs[0] = A3_FashionShowModel.getInstance().first_nowfs[1] = 0;
                    }
                    else
                    {
                        A3_FashionShowModel.getInstance().nowfs[0] = A3_FashionShowModel.getInstance().nowfs[1] = 0;
                        A3_FashionShowModel.getInstance().first_nowfs[0] = A3_FashionShowModel.getInstance().first_nowfs[1] = 0;
                    }
                    //拥有的
                    if (data["dressments"] != null && data["dressments"].Count > 0)
                    {
                        for (int i = 0; i < data["dressments"].Count; i++)
                        {
                            A3_FashionShowModel.getInstance().dic_have_fs[data["dressments"][i]["dress"]] = data["dressments"][i]["limit"];
                        }                        
                    }
                    else
                        A3_FashionShowModel.getInstance().dic_have_fs.Clear();

                    A3_FashionShowModel.getInstance().dress_show = data["dress_show"];
                    break;
                    
                case 2:
                    if (data["dress_list"] != null && data["dress_list"].Count > 0)
                    {
                        A3_FashionShowModel.getInstance().nowfs[0] = data["dress_list"][0]._int;
                        A3_FashionShowModel.getInstance().nowfs[1] = data["dress_list"][1]._int;
                        if (A3_FashionShowModel.getInstance().dress_show)
                        {
                            A3_FashionShowModel.getInstance().first_nowfs[0] = data["dress_list"][0]._int;
                            A3_FashionShowModel.getInstance().first_nowfs[1] = data["dress_list"][1]._int;
                        }
                        else
                            A3_FashionShowModel.getInstance().first_nowfs[0] = A3_FashionShowModel.getInstance().first_nowfs[1] = 0;
                    }
                    else
                    {
                        A3_FashionShowModel.getInstance().nowfs[0] = A3_FashionShowModel.getInstance().nowfs[1] = 0;
                        A3_FashionShowModel.getInstance().first_nowfs[0] = A3_FashionShowModel.getInstance().first_nowfs[1] = 0;
                        a3_fashionshow.ShowHide_SelefFashion();
                    }
                    if(a3_fashionshow._insatnce!=null)
                        a3_fashionshow._insatnce.RefreshSelfObj();

                  //  dispatchEvent(GameEvent.Create(CHANGEFA, this, data));
                    break;
                case 3:
                      A3_FashionShowModel.getInstance().dic_have_fs[data["dress"]._int] = data["limit"]._uint;
                      if(a3_fashionshow._insatnce!=null)
                          a3_fashionshow._insatnce.RefreshObjs(data["dress"]._int);
                    break;
                case 4:
                    A3_FashionShowModel.getInstance().dress_show = data["dress_show"];
                    if(data["dress_show"]._bool)
                    {
                  
                        A3_FashionShowModel.getInstance().first_nowfs[0] = A3_FashionShowModel.getInstance().nowfs[0];
                        A3_FashionShowModel.getInstance().first_nowfs[1] = A3_FashionShowModel.getInstance().nowfs[1];
                        if (a3_fashionshow._insatnce != null)
                           a3_fashionshow._insatnce.Show_this_Fashion();
                    }
                    else
                    {

                   
                        A3_FashionShowModel.getInstance().first_nowfs[0] =0;
                        A3_FashionShowModel.getInstance().first_nowfs[1] =0;

                        if(a3_fashionshow._insatnce)
                            a3_fashionshow._insatnce.Hide_this_Fashion();
                    }

                    break;
            }
        }

        public void SendProxys(int op, List<int> lsts, int id = -1, int type = -1, bool show = false)
        {
            Variant msg = new Variant();
            msg["op"] = op;
            switch(op)
            {
                case 1:
                    break;
                case 2:
                    if (lsts != null)
                    {
                        msg["list"] = new Variant();
                        for (int i = 0; i < lsts.Count; i++)
                        {
                            msg["list"].pushBack(lsts[i]);
                        }
                    }
                    break;
                case 3:
                    msg["dress"] = id;
                    msg["type"] = type;

                    break;
                case 4:
                    msg["dress_show"] = show;
                    break;
            }
            sendRPC(PKG_NAME.C2S_FASHIONSHOW, msg);
        }
    }
}
