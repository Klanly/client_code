using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

namespace MuGame
{
    class a3_autoplay_skill : Window
    {
        static public a3_autoplay_skill Instance;
        static public Action<int,int> OnSkillChoose;
        
        //!--对应挂机界面,技能索引(0~4)
        static public int SkillSeat = -1;

        private GridLayoutGroup grid;
        
        public override void init()
        {
            Instance = this;

            this.getEventTrigerByPath("ig_bg_bg").onClick = onClose;
            grid = getComponentByPath<GridLayoutGroup>("scroll_view/contain");
        }

        public override void onShowed()
        {
            GameObject iconPrefab = GAMEAPI.ABLayer_LoadNow_GameObject("uilayer_a3_autoplay_skill_icon");

            int skcnt = 0;
            foreach (skill_a3Data sk in Skill_a3Model.getInstance().skilldic.Values)
            {
                //!--非本职业，普通技能，未学的技能不可见
                if (sk.carr != PlayerModel.getInstance().profession ||
                    sk.skill_id == a1_gamejoy.NORNAL_SKILL_ID ||
                    sk.now_lv == 0)
                    continue;

                GameObject icon = Instantiate(iconPrefab) as GameObject;
                if (icon == null) continue;

                int skid = sk.skill_id;

                icon.transform.parent = grid.transform;
                icon.name = skid.ToString();
                icon.transform.FindChild("bg/mask/icon").GetComponent<Image>().sprite = GAMEAPI.ABUI_LoadSprite("icon_skill_" + skid.ToString());

                icon.transform.FindChild("bg").GetComponent<Button>().onClick.AddListener(() =>
                {
                    onClose(null);
                    OnSkillChoose(skid, SkillSeat);
                });

                skcnt++;
            }

            if(skcnt == 0)
                flytxt.instance.fly(ContMgr.getCont("a3_autoplay_skill"));
        }

        public override void onClosed()
        {
            for (int i = 0; i < grid.transform.childCount; i++)
            {
                Destroy(grid.transform.GetChild(i).gameObject);
            }
        }

        void onClose(GameObject go)
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.A3_AUTOPLAY_SKILL);
        }
    }
}
