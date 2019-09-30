using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

//ReSharper disable once CheckNamespace
namespace MuGame
{
    class A3_PetProxy : BaseProxy<A3_PetProxy>
    {
        public static uint EVENT_GET_LAST_TIME = 1201;
        public static uint EVENT_GET_PET = 1202;
        public static uint EVENT_HAVE_PET = 1203;
        public static uint EVENT_SHOW_PET = 1204;
        public static uint EVENT_FEED_PET = 1205;
        public static uint PET_RENEW = 1206;
        public static uint CHANGE_PET = 1207;

        public static uint EVENT_USE_PETFEED = 0;
        public A3_PetProxy()
        {
            //TODO 注册客户端向服务器发送的消息回调
            addProxyListener(PKG_NAME.S2C_PET_RES, OnPet);
        }

        #region client to server msg

        public void GetPets()
        {
            Variant msg = new Variant();
            msg["op"] = 1;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }

        public void SendPetId(int id)
        {
            Variant msg = new Variant();
            msg["op"] = 14;
            msg["pet_id"] = id;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }
        public void SendTime(int food_id)
        {
            Variant msg = new Variant();
            msg["op"] = 15;
            msg["food_id"] = food_id;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }
        public void Feed()
        {
            Variant msg = new Variant();
            msg["op"] = 2;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }

        public void Bless(bool useyb = false)
        {
            Variant msg = new Variant();
            msg["op"] = 3;
            msg["use_yb"] = useyb ? 1 : 0;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }

        public void Stage(int crystal_num)
        {
            Variant msg = new Variant();
            msg["op"] = 4;
            msg["crystal_num"] = crystal_num;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }

        public void SetAutoFeed(uint isOn)
        {
            Variant msg = new Variant();
            msg["op"] = 8;
            msg["auto_feed"] = isOn;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }
        public void SetAutoBuy(uint isOn)
        {
            Variant msg = new Variant();
            msg["op"] = 13;
            msg["auto_buy_feeds"] = isOn;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }

        public void OneKeyBless(bool useyb = false)
        {
            Variant msg = new Variant();
            msg["op"] = 12;
            msg["use_yb"] = useyb ? 1 : 0;
            sendRPC(PKG_NAME.C2S_PET, msg);
        }

        #endregion

        #region server to client callback

        public void OnPet(Variant data)
        {
            debug.Log("宠物信息:" + data.dump());
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            switch (res)
            {
                #region  =======没用的=======
                //case 1: //返回宠物所有信息
                //    //OnGetPets(data);
                //    break;
                //case 3:
                //    //OnBless(data);
                //    break;
                //case 4:
                //    //OnStage(data);
                //    break;
                //case 5: //挂机时获得经验
                //    //OnAutoPlayUpgrade(data);
                //    break;
                //case 8: //设置自动喂养
                //    //OnSyncAutoFeed(data);
                //    break;
                //case 10: //同步饥饿度
                //    //dispatchEvent(GameEvent.Create(EVENT_USE_PETFEED, this, data));
                //    //OnSyncStarvation(data);
                //    break;
                //case 11: //其他玩家宠物形象改变
                //    //OnOtherPlayerPetChange(data);
                //    break;
                //case 12:
                //    //OnOneKeyBless(data);
                //    break;
                #endregion
                case 13:
                    A3_PetModel.showrenew = true;//第一次登录时有宠物没有饲料
                    break;
                case 14:
                    OnPetId(data);//其他玩家宠物改变
                    if (data["iid"] == PlayerModel.getInstance().iid)
                    {
                        A3_PetModel.curPetid = data["pet_id"];
                        dispatchEvent(GameEvent.Create(CHANGE_PET, this, data));
                        if (SelfRole._inst != null)
                            SelfRole._inst.ChangePetAvatar(data["pet_id"]._int, 0);
                    }

                    break;
                case 15:
                    GetTime(data);//购买饲料
                    break;
                case 16:
                    Getpet(data);//获得宠物
                    break;
                case 17:
                    petHas(data);//宠物饲料到期
                    break;
                default:

                    break;
            }
        }

        private void petHas(Variant data)
        {
            debug.Log("宠物饲料到期:"+data.dump());
            if(a3_new_pet.instance)
            {
                a3_new_pet.instance.havePet(null);
            }
            dispatchEvent(GameEvent.Create(EVENT_HAVE_PET, this, data));
            if (SelfRole._inst != null)
                SelfRole.showPet(data);
        }


        private void OnPetId(Variant data)
        {
            debug.Log(data.dump());
            OtherPlayerMgr._inst.otherPlayPetAvatarChange(data["iid"], data["pet_id"], 0);


        }
        private void GetTime(Variant data)
        {
            debug.Log(data.dump());
            PlayerModel.getInstance().last_time = data["pet_food_last_time"];
            dispatchEvent(GameEvent.Create(EVENT_GET_LAST_TIME, this, data));
            dispatchEvent(GameEvent.Create(EVENT_SHOW_PET, this, data));
            dispatchEvent(GameEvent.Create(EVENT_FEED_PET, this, data));
            flytxt.instance.fly( ContMgr.getCont( "a3_pet_getTime" ) );
        }
        private void Getpet(Variant data)
        {
            if (!PlayerModel.getInstance().havePet)
                PlayerModel.getInstance().havePet = true;//刷新palyermodel里面数据是否有pet
            debug.Log(data.dump());
            A3_PetModel.getInstance().petId = data["pet"]["id_arr"]._arr;
            A3_PetModel.curPetid = (uint)data["pet"]["id"]._int;
            A3_PetModel.getInstance().Tpid = (uint)data["pet"]["id"]._int;
            dispatchEvent(GameEvent.Create(EVENT_GET_PET, this, data));

            if (SelfRole._inst != null)
                SelfRole.OnPetHaveChange(data);
        }
        #region    =========没用的========
        private void OnBless(Variant data)
        {
            A3_PetModel cpet = A3_PetModel.getInstance();
            cpet.Tpid = data["id"];
            cpet.Level = data["level"];
            cpet.Exp = data["exp"];
        }
        private void OnGetPets(Variant data)
        {
            if (data.ContainsKey("pet"))
            {
                Variant spet = data["pet"];
                A3_PetModel cpet = A3_PetModel.getInstance();
                cpet.Tpid = spet["id"];
                //cpet.Stage = spet["stage"];
                //cpet.Level = spet["level"];
                //cpet.Exp = spet["exp"];
                //if (spet.ContainsKey("starvation")) cpet.Starvation = spet["starvation"];
                //if (spet.ContainsKey("auto_feed"))
                //{
                //    if (spet["auto_feed"] == 0) { cpet.Auto_feed = false; }
                //    else { cpet.Auto_feed = true; }
                //}
                //if (spet.ContainsKey("auto_buy_feeds"))
                //{
                //    if (spet["auto_buy_feeds"] == 0) { cpet.Auto_buy = false; }
                //    else { cpet.Auto_buy = true; }
                //}

            }
        }
        private void OnStage(Variant data)
        {
            A3_PetModel cpet = A3_PetModel.getInstance();
            cpet.Tpid = data["id"];
            cpet.Stage = data["stage"];
            cpet.Level = 1;
            cpet.Exp = 0;
            flytxt.instance.fly(ContMgr.getCont("petmodel_upok"));
        }

        private void OnAutoPlayUpgrade(Variant data)
        {
            A3_PetModel cpet = A3_PetModel.getInstance();
            cpet.Tpid = data["id"];
            cpet.Exp = data["exp"];
            if (data.ContainsKey("level"))
            {
                cpet.Level = data["level"];
            }
        }

        private void OnSyncStarvation(Variant data)
        {
            A3_PetModel cpet = A3_PetModel.getInstance();
            cpet.Starvation = data["starvation"];
            if (a3_pet_skin.instan != null)
            {
                a3_pet_skin.instan.OnStarvationChange();
            }
        }

        private void OnSyncAutoFeed(Variant data)
        {
            A3_PetModel cpet = A3_PetModel.getInstance();
            if (data["auto_feed"] == 0) { cpet.Auto_feed = false; }
            else { cpet.Auto_feed = true; }
            //debug.Log(cpet.Auto_feed + "::::::::" + a3_pet_skin.instan.autofeed.isOn);
        }
        private void OnSyncAutoBuy(Variant data)
        {
            A3_PetModel cpet = A3_PetModel.getInstance();

            if (data["auto_buy_feeds"] == 0) { cpet.Auto_buy = false; }
            else { cpet.Auto_buy = true; }
            int num = a3_BagModel.getInstance().getItemNumByTpid(cpet.GetFeedItemTpid());
            if (cpet.Auto_buy && num <= 0)
            {
                A3_PetModel.getInstance().AutoBuy();
            }
            //debug.Log(cpet.Auto_buy + "::::::::" + a3_pet_skin.instan.autobuy.isOn);
        }

        private void OnOtherPlayerPetChange(Variant data)
        {
            OtherPlayerMgr._inst.PlayPetAvatarChange(data["iid"], data["id"], 0);
        }

        private void OnOneKeyBless(Variant data)
        {
            A3_PetModel cpet = A3_PetModel.getInstance();
            if (data.ContainsKey("multi_res"))
            {
                int res = data["multi_res"];
                if (res < 0)
                {
                    Globle.err_output(res);
                }
            }

            cpet.Tpid = data["id"];
            cpet.Level = data["level"];
            cpet.Exp = data["exp"];
        }
        #endregion
        #endregion
    }
}
