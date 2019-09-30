using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_changeName : Window
    {
        private Text _titleText, _textNameTips, _consumeNumber;
        private InputField _content;
        private BaseButton _btnYes, _btnClose;
        private Transform _containerTrans;
        private Image _consumeImage;
        public static Action<string> changeLegionName = null;


        public override void init()
        {

            OnFindChildGo();

            OnAddBtnOnClick();

        }

        public override void onShowed()
        {
            //this.uiData  [0] = 1 ： 表示人物修改  = 2 : 表示军团修改  [1] :   道具的 id   [2] 道具的使用数量

            this.gameObject.transform.SetAsLastSibling();

            _content.text = "";

            if ((int)this.uiData[0] == 1)
            {
                _titleText.text = ContMgr.getCont("roleNameTitle");
                _textNameTips.text = ContMgr.getCont("roleNameTips");
                _containerTrans.gameObject.SetActive(false);

            }
            else if((int)this.uiData[0] == 2)
            {
                _titleText.text =  ContMgr.getCont("changeName_LegionTitle");
                _textNameTips.text = ContMgr.getCont("changeName_LegionTips");
                _containerTrans.gameObject.SetActive(true);
                a3_ItemData itmeVo = a3_BagModel.getInstance().getItemDataById((uint)(int)this.uiData[1]);
                _consumeImage.sprite = GAMEAPI.ABUI_LoadSprite(itmeVo.file);
                _consumeNumber.text = ((int)this.uiData[2]).ToString();

            }


        }

        public override void onClosed()
        {
            OnDispose();
        }

        private void OnFindChildGo()
        {
            _titleText = this.getComponentByPath<Text>("info_bg/topText/Text");
            _textNameTips = this.getComponentByPath<Text>("Text_Tips");
            _content = this.getComponentByPath<InputField>("InputField");
            _btnYes = new BaseButton(this.getTransformByPath("Button_Yes"));
            _btnClose = new BaseButton(this.getTransformByPath("close_bg"));
            _containerTrans = this.getTransformByPath("Consume_Tips");
            _consumeImage = this.getComponentByPath<Image>("Consume_Tips/Image_item");
            _consumeNumber = this.getComponentByPath<Text>("Consume_Tips/Text_Number");
        }

        private void OnAddBtnOnClick() {

            _btnYes.onClick = (go) => {

                bool isContainKeyWord = KeyWord.isContain(_content.text);

                if (isContainKeyWord)
                {
                    flytxt.instance.fly(ContMgr.getCont("changeName_ErrContent"));

                    return;
                }

                if ((int)this.uiData[0] == 1)
                {

                    BagProxy.getInstance().sendUseItems((uint)this.uiData[1], 1, _content.text);

                }

                else if ((int)this.uiData[0] == 2) {

                  if( changeLegionName !=null )  changeLegionName(_content.text);

                }

            };

            _btnClose.onClick = (go) =>
            {
                InterfaceMgr.getInstance().close(InterfaceMgr.A3_CHANGE_NAME);

            };

        }

        private void OnDispose() {

            changeLegionName = null;

        }
 
    }

}
