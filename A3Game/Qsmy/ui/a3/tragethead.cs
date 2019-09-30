using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cross;
using System.Collections;

namespace MuGame
{
    class tragethead : FloatUi
    {
        public GameObject iconPhy;
        public GameObject iconMagic;
        public GameObject iconJy;

        public GameObject iconWar;
        public GameObject iconMage;
        public GameObject iconAss;

        public GameObject bossBg;
        public GameObject bg;
        public GameObject otherHP;
        public GameObject bosshp1;
        public GameObject fgim;
        public GameObject hp1;
        public GameObject type1;
        public GameObject pro;

        public GameObject fg2;
        public GameObject bossHead;
        public GameObject fg3;
        public GameObject bosshps;

        public Transform transHP;
        public Transform transHP1;

        public Transform bossHpBg;
        public Transform bossHP;
        public Text txtname;
        public Text txtLv;
        public Text bossName;
        public Text bossHPCount;
        public BaseRole role;
        int bosscurhp = 0;
        public int curhp = 0;
        public int maxhp = 0;
        public int count = 0;
        public int max = 0;
        public GameObject golevel;
        TargetMenu menu;
        public bool inFB = false;
        public static tragethead instance;
        bool dead = false;
        Tween tween = null;
        Tween tween1 = null;
        Tween tween2 = null;
        public bool full = false;
        public override void init()
        {
            instance = this;
            iconPhy = getGameObjectByPath("type/phy");
            iconMagic = getGameObjectByPath("type/magic");
            iconJy = getGameObjectByPath("type/jy");

            iconWar = getGameObjectByPath("pro/war");
            iconMage = getGameObjectByPath("pro/mage");
            iconAss = getGameObjectByPath("pro/ass");

            bossBg = getGameObjectByPath("BOSS1");
            bg = getGameObjectByPath("bg");
            otherHP = getGameObjectByPath("hp");
            bosshp1 = getGameObjectByPath("boss_hp");
            fgim = getGameObjectByPath("fgImage");
            hp1 = getGameObjectByPath("hp1");
            type1 = getGameObjectByPath("type");
            pro = getGameObjectByPath("pro");
            fg2 = getGameObjectByPath("fg2");
            fg3 = getGameObjectByPath("bossHead/fg3");

            bosshps = getGameObjectByPath("boss_hps");


            EventTriggerListener.Get(iconWar).onClick = onHeadClick;
            EventTriggerListener.Get(iconMage).onClick = onHeadClick;
            EventTriggerListener.Get(iconAss).onClick = onHeadClick;

            transHP = getTransformByPath("hp");
            txtLv = getComponentByPath<Text>("lv");
            txtname = getComponentByPath<Text>("txt");
            bossName = getComponentByPath<Text>("txt2");
            bossHPCount = getComponentByPath<Text>("txt3");
            transHP1 = getTransformByPath("hp1");

            bossHpBg = getTransformByPath("boss_hps/boss_hp0");
            transform.localScale = Vector3.zero;
            bossHP = getTransformByPath("boss_hp");
            bossHead = getGameObjectByPath("bossHead/1");
            golevel = getGameObjectByPath("level");

            menu = new TargetMenu(getTransformByPath("menu"));
            getComponentByPath<Text>("menu/chat/Text").text = ContMgr.getCont("tragethead_0");
            getComponentByPath<Text>("menu/view/Text").text = ContMgr.getCont("tragethead_1");
            getComponentByPath<Text>("menu/team/Text").text = ContMgr.getCont("tragethead_2");
            getComponentByPath<Text>("menu/union/Text").text = ContMgr.getCont("tragethead_3");
            getComponentByPath<Text>("menu/addfre/Text").text = ContMgr.getCont("tragethead_4");
            getComponentByPath<Text>("menu/black/Text").text = ContMgr.getCont("tragethead_5");
        }

        BaseRole trole;
        bool isCan = true;
        bool isboss = false;

        public void resetTragethead()
        {
            trole = null;
            role = null;
            transform.localScale = new Vector3(0, 1, 0);
        }

        public void Update()
        {
            if (SelfRole._inst == null)
            {
                trole = null;
                role = null;
                return;
            }
            if (isboss)//boss//role != null && role is MonsterRole && role.tempXMl != null && role.tempXMl.getInt("boss_hp") == 1
            {
                curhp = 0;
                maxhp = 0;
            }

            if (trole != null && trole.isDead && transHP1.localScale.x != 0f)//怪物死了血条没清空
            {
                if (isboss)//boss//role != null && role is MonsterRole && role.tempXMl != null && role.tempXMl.getInt("boss_hp") == 1
                {
                    if (role.isDead)
                    {
                        BossHP();
                        DeadHp();
                    }
                    StartCoroutine(Wait1());
                }
                role = null;
            }
            else
            {
                trole = SelfRole._inst.m_LockRole;
                if (trole != null && trole.isDead == true)
                {
                    trole = null;
                }
            }

            if (trole == null || trole.disposed || (trole.isDead && transHP1.localScale.x == 0f))
            {
                if (role != null)
                {
                    if (isboss)//boss死亡//role is MonsterRole && role.tempXMl != null && role.tempXMl.getInt("boss_hp") == 1
                    {

                        dead = role.isDead;
                        if (dead)
                        {
                            BossHP();
                            DeadHp();
                        }
                    //}
                    //if (role is MonsterRole && role.tempXMl != null && role.tempXMl.getInt("boss_hp") == 1)
                    //{
                        StartCoroutine(Wait1());
                    }
                    else
                        transform.localScale = new Vector3(0, 1, 0);
                    role = null;

                }
                if (b_useing == true)
                {
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.setactive_btn", "ui/interfaces/low/a1_low_fightgame", true);
                    b_useing = false;
                }
                return;
            }

            if (role == null && trole != null && trole.isDead == false)//锁定改变
            {
                transform.localScale = Vector3.one;
                refreshInfo();
            }
            else if (SelfRole._inst.m_LockRole != role && isCan)
            {
                refreshInfo();
            }
            else if (role == null && trole != null && trole.isDead == true)
            {
                trole = null;
                transform.localScale = new Vector3(0, 1, 0);
            }

            if (role != null && (curhp != role.curhp || maxhp != role.maxHp))//血量变化
            {
                if (trole.isDead)
                {
                    if (!isboss)//role is MonsterPlayer || (role is MonsterRole && role.tempXMl.getInt("boss_hp") != 1) || role is ProfessionRole
                    {
                        tween2 = transHP.DOScaleX(0, 0.2f);
                        StartCoroutine(Wait());
                    }
                }

                float p = (float)role.curhp / (float)role.maxHp;
                Vector3 vec = new Vector3(p, 1f, 1f);
                if (curhp < role.curhp)
                {
                    if (!isboss)//非boss//role is MonsterRole && !(role is MonsterPlayer) && role.tempXMl.getInt("boss_hp") != 1 || role is ProfessionRole || role is MonsterPlayer
                    {
                        transHP1.DOKill();
                        transHP1.localScale = vec;
                        bossHP.gameObject.SetActive(false);

                    }
                    else
                    {
                        if (isboss)//!(role is MonsterPlayer) && role is MonsterRole && role.tempXMl.getInt("boss_hp") == 1
                        {
                            bossHP.gameObject.SetActive(true);
                        }
                        bossHP.DOKill();
                        bossHpBg.DOKill();
                        bossHP.localScale = new Vector3(1f, 1f, 1f);
                    }
                    BossHP();
                }
                else
                {
                    tween1 = transHP1.DOScaleX(p, 0.2f);
                    isCan = false;
                }
                tween.OnComplete(() => refTrole());
                tween1.OnComplete(() => refTrole());
                tween2.OnComplete(() => refTrole());

                if (p > 1) vec = Vector3.one;
                if (!isboss)//非boss//role is MonsterPlayer || (role is MonsterRole && role.tempXMl.getInt("boss_hp") != 1) || role is ProfessionRole
                {
                    curhp = role.curhp;
                    maxhp = role.maxHp;
                    transHP.localScale = vec;
                    tween2 = transHP.DOScaleX(vec.x, 0.2f);
                }
            }


        }

        void refTrole()
        {
            isCan = true;
        }
        void BossHP()
        {
            if (isboss)//!(role is MonsterPlayer) && role is MonsterRole && role.tempXMl.getInt("boss_hp") == 1
            {
                //Boss血条
                float p = (float)role.curhp / (float)role.maxHp;
                int headid = role.tempXMl.getInt("head");
                int hpCount = role.tempXMl.getInt("hp_count");
                GameObject[] boss_hps = new GameObject[hpCount + 1];
                int everyHp = role.maxHp / hpCount;//每管血的血量取整
                int x = (role.maxHp - role.curhp) / everyHp;
                int v = hpCount - x;// bossHead_id
                int newCount = hpCount;
                if (role.curhp != bosscurhp)//boss血量变化
                {

                    bossName.text = role.roleName;
                    bossHPCount.text = "X" + v;
                    bossHead.GetComponent<Image>().overrideSprite = GAMEAPI.ABUI_LoadSprite("icon_bossHead_" + headid);
                    transform.FindChild("boss_hps/boss_hp0").gameObject.SetActive(true);
                    boss_hps[0] = transform.FindChild("boss_hps/boss_hp0").gameObject;

                    if (count != newCount)
                    {
                        full = false;
                    }
                    #region
                    if (full == false)
                    {
                        if (hpCount >= 10)
                        {
                            if (count > newCount)
                            {
                                for (int j = newCount + 1; j <= count; j++)
                                {
                                    transform.FindChild("boss_hps/boss_hp" + j).gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                if (count == 0)//count为0为第一次点击boss
                                {
                                    for (int j = 0; j < (hpCount - 5) / 5; j++)
                                    {
                                        int yuShu = hpCount % 5;
                                        int k = j + 1;
                                        for (int i = 1; i <= 5; i++)
                                        {
                                            transform.FindChild("boss_hps/boss_hp" + i).gameObject.SetActive(true);
                                            boss_hps[i] = transform.FindChild("boss_hps/boss_hp" + i).gameObject;
                                            GameObject boGo = transform.FindChild("boss_hps/boss_hp" + i).gameObject;

                                            boss_hps[i + 5 * k] = Instantiate(boGo);
                                            boss_hps[i + 5 * k].transform.SetParent(transform.FindChild("boss_hps"));
                                            boss_hps[i + 5 * k].transform.localPosition = boss_hps[i].transform.localPosition;
                                            boss_hps[i + 5 * k].transform.localScale = Vector3.one;
                                            boss_hps[i + 5 * k].name = "boss_hp" + (i + 5 * k);
                                            if (i + 5 * k == hpCount - yuShu)
                                            {
                                                for (int l = 1; l <= yuShu; l++)
                                                {
                                                    transform.FindChild("boss_hps/boss_hp" + l).gameObject.SetActive(true);
                                                    boss_hps[l] = transform.FindChild("boss_hps/boss_hp" + l).gameObject;
                                                    GameObject boGo2 = transform.FindChild("boss_hps/boss_hp" + l).gameObject;

                                                    boss_hps[i + 5 * k + l] = Instantiate(boGo2);
                                                    boss_hps[i + 5 * k + l].transform.SetParent(transform.FindChild("boss_hps"));
                                                    boss_hps[i + 5 * k + l].transform.localPosition = boss_hps[l].transform.localPosition;
                                                    boss_hps[i + 5 * k + l].transform.localScale = Vector3.one;
                                                    boss_hps[i + 5 * k + l].name = "boss_hp" + (i + 5 * k + l);
                                                }
                                            }

                                            if (i + 5 * k == hpCount || i + 5 * k + yuShu == hpCount)
                                            {
                                                full = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool change;
                                    change = (max >= newCount) ? true : false;
                                    if (change == false)
                                    {
                                        for (int j = count + 1; j <= newCount; j++)
                                        {
                                            if (j % 5 > 0)
                                            {
                                                transform.FindChild("boss_hps/boss_hp" + (j % 5)).gameObject.SetActive(true);
                                                boss_hps[(j % 5)] = transform.FindChild("boss_hps/boss_hp" + (j % 5)).gameObject;
                                                GameObject boGo2 = transform.FindChild("boss_hps/boss_hp" + (j % 5)).gameObject;

                                                boss_hps[j] = Instantiate(boGo2);
                                                boss_hps[j].transform.SetParent(transform.FindChild("boss_hps"));
                                                boss_hps[j].transform.localPosition = boss_hps[(j % 5)].transform.localPosition;

                                                boss_hps[j].name = "boss_hp" + j;
                                                boss_hps[j].transform.localScale = new Vector3(1, 1, 0);
                                            }
                                            else
                                            {
                                                transform.FindChild("boss_hps/boss_hp" + (5)).gameObject.SetActive(true);
                                                boss_hps[(5)] = transform.FindChild("boss_hps/boss_hp" + (5)).gameObject;
                                                GameObject boGo2 = transform.FindChild("boss_hps/boss_hp" + (5)).gameObject;

                                                boss_hps[j] = Instantiate(boGo2);
                                                boss_hps[j].transform.SetParent(transform.FindChild("boss_hps"));
                                                boss_hps[j].transform.localPosition = boss_hps[(5)].transform.localPosition;
                                                boss_hps[j].transform.localScale = new Vector3(1, 1, 0);
                                                boss_hps[j].name = "boss_hp" + j;
                                            }

                                            if (j == hpCount)
                                            {
                                                full = true;
                                            }
                                        }
                                    }

                                    max = (max >= count) ? max : count;
                                }

                            }

                            count = hpCount;
                            max = (max >= count) ? max : count;

                        }
                        else
                        {
                            if (count > newCount)
                            {
                                for (int j = newCount + 1; j <= count; j++)
                                {
                                    transform.FindChild("boss_hps/boss_hp" + j).gameObject.SetActive(false);
                                }
                            }
                            else
                            {
                                if (count == 0)
                                {
                                    if (hpCount <= 5)
                                    {
                                        for (int i = 1; i <= hpCount; i++)
                                        {
                                            transform.FindChild("boss_hps/boss_hp" + i).gameObject.SetActive(true);
                                            boss_hps[i] = transform.FindChild("boss_hps/boss_hp" + i).gameObject;
                                        }
                                    }
                                    else
                                    {
                                        int yushu = hpCount % 5;
                                        for (int i = 1; i <= yushu; i++)
                                        {
                                            transform.FindChild("boss_hps/boss_hp" + i).gameObject.SetActive(true);
                                            boss_hps[i] = transform.FindChild("boss_hps/boss_hp" + i).gameObject;
                                            GameObject boGo2 = transform.FindChild("boss_hps/boss_hp" + i).gameObject;

                                            boss_hps[i + 5] = Instantiate(boGo2);
                                            boss_hps[i + 5].transform.SetParent(transform.FindChild("boss_hps"));
                                            boss_hps[i + 5].transform.localPosition = boss_hps[i].transform.localPosition;
                                            boss_hps[i + 5].transform.localScale = new Vector3(1, 1, 0);
                                            boss_hps[i + 5].name = "boss_hp" + (i + 5);
                                            if (i + 5 == hpCount)
                                            {
                                                full = true;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    bool change;
                                    change = (max >= newCount) ? true : false;
                                    if (change == false)
                                    {
                                        for (int j = count + 1; j <= newCount; j++)
                                        {
                                            if (j % 5 > 0)
                                            {
                                                transform.FindChild("boss_hps/boss_hp" + (j % 5)).gameObject.SetActive(true);
                                                boss_hps[(j % 5)] = transform.FindChild("boss_hps/boss_hp" + (j % 5)).gameObject;
                                                GameObject boGo2 = transform.FindChild("boss_hps/boss_hp" + (j % 5)).gameObject;

                                                boss_hps[j] = Instantiate(boGo2);
                                                boss_hps[j].transform.SetParent(transform.FindChild("boss_hps"));
                                                boss_hps[j].transform.localPosition = boss_hps[(j % 5)].transform.localPosition;
                                                boss_hps[j].transform.localScale = new Vector3(1, 1, 0);
                                                boss_hps[j].name = "boss_hp" + j;
                                            }
                                            else
                                            {
                                                transform.FindChild("boss_hps/boss_hp" + 5).gameObject.SetActive(true);
                                                boss_hps[5] = transform.FindChild("boss_hps/boss_hp" + 5).gameObject;
                                                GameObject boGo2 = transform.FindChild("boss_hps/boss_hp" + 5).gameObject;

                                                boss_hps[j] = Instantiate(boGo2);
                                                boss_hps[j].transform.SetParent(transform.FindChild("boss_hps"));
                                                boss_hps[j].transform.localPosition = boss_hps[5].transform.localPosition;
                                                boss_hps[j].transform.localScale = new Vector3(1, 1, 0);
                                                boss_hps[j].name = "boss_hp" + j;
                                            }
                                            if (j == hpCount)
                                            {
                                                full = true;
                                            }
                                        }
                                    }
                                }
                            }

                            count = hpCount;
                            max = (max >= count) ? max : count;
                        }

                    }
                    #endregion

                    for (int i = 1; i <= hpCount; i++)
                    {
                        transform.FindChild("boss_hps/boss_hp" + i).gameObject.SetActive(true);
                        boss_hps[i] = transform.FindChild("boss_hps/boss_hp" + i).gameObject;
                    }

                    for (int i = hpCount + 1; i <= 200; i++)
                    {
                        if (transform.FindChild("boss_hps/boss_hp" + i) != null)
                        {
                            transform.FindChild("boss_hps/boss_hp" + i).gameObject.SetActive(false);
                        }
                    }
                    //Debug.LogError(p + "  " + x + "  " + v);
                    for (int i = 1; i < v; i++)
                    {
                        boss_hps[i].transform.localScale = new Vector3(1, 1, 0);
                    }
                    if (v <= 1)
                        boss_hps[0].transform.SetAsFirstSibling();
                    else
                        boss_hps[0].transform.SetSiblingIndex(boss_hps[v].transform.GetSiblingIndex() - 1);
                    tween1 = boss_hps[v].transform.DOScaleX(hpCount * p - v + 1, 0.2f);
                    for (int i = v + 1; i <= hpCount; i++)
                    {
                        tween2 = boss_hps[i].transform.DOScaleX(0, 0.2f);
                    }
                }
                tween = bossHpBg.DOScaleX(hpCount * p - v + 1, 0.3f);
                bosscurhp = role.curhp;

            }
        }
        void DeadHp()
        {
            int hpCount = role.tempXMl.getInt("hp_count");
            GameObject[] boss_hps = new GameObject[hpCount + 1];
            int everyHp = role.maxHp / hpCount;//每管血的血量取整
            int x = (role.maxHp - role.curhp) / everyHp;
            int v = hpCount - x;// bossHead_id
            for (int i = hpCount; i >= 1; i--)
            {
                boss_hps[i] = transform.FindChild("boss_hps/boss_hp" + i).gameObject;
                tween2 = boss_hps[i].transform.DOScaleX(0, 0.2f);
            }
            tween1 = bossHpBg.DOScaleX(0, 0.003f);
            dead = false;
        }
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.05f);

            transform.localScale = Vector3.zero;
        }
        IEnumerator Wait1()
        {
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(0, 1, 0);
        }
        public void onHeadClick(GameObject go)
        {

            if (inFB == false)
            {
                if (menu.visiable)
                {
                    menu.hide();
                    return;
                }
                menu.show();
            }
            else
            {
                flytxt.instance.fly(ContMgr.getCont("tragethead"));
            }
        }


        bool b_useing = false;

        public void refreshInfo()//更改锁定是调用一次
        {
            role = SelfRole._inst.m_LockRole;
            menu.hide();
            iconMagic.SetActive(false);
            iconJy.SetActive(false);
            iconPhy.SetActive(false);
            iconWar.SetActive(false);
            iconAss.SetActive(false);
            iconMage.SetActive(false);
            bossBg.SetActive(false);
            bg.SetActive(true);
            fgim.SetActive(true);
            hp1.SetActive(true);
            golevel.SetActive(true);
            otherHP.SetActive(true);
            type1.SetActive(true);
            pro.SetActive(true);
            fg2.SetActive(true);
            fg3.SetActive(false);
            bossHead.SetActive(false);
            bosshps.SetActive(false);
            bosshp1.SetActive(false);

            if (b_useing == true)
            {
                InterfaceMgr.doCommandByLua("a1_low_fightgame.setactive_btn", "ui/interfaces/low/a1_low_fightgame", true);
                b_useing = false;
            }

            if (role is CollectRole)//采集怪
            {
                golevel.SetActive(false);
                txtLv.text = "";
                bossName.text = "";
                bossHPCount.text = "";
                txtname.text = role.roleName;
                isboss = false;
            }
            else if (role is MonsterRole)
            {
                if (role.tempXMl != null && role.tempXMl.getInt("boss_hp") == 1)
                {
                    txtname.gameObject.SetActive(false);
                    bossName.gameObject.SetActive(true);
                    bossHPCount.gameObject.SetActive(true);
                    isboss = true;
                }
                else
                {
                    bossName.gameObject.SetActive(false);
                    bossHPCount.gameObject.SetActive(false);
                    txtname.gameObject.SetActive(true);
                    isboss = false;
                }
                if (role.tempXMl != null && role.tempXMl.getInt("atktp") == 1)
                    iconPhy.SetActive(true);
                else if (role.tempXMl != null)
                    iconMagic.SetActive(true);
                if (isboss)//boss
                {
                    bossBg.SetActive(true);
                    bg.SetActive(false);
                    fgim.SetActive(false);
                    otherHP.SetActive(false);
                    hp1.SetActive(false);
                    bosshp1.SetActive(true);
                    type1.SetActive(false);
                    pro.SetActive(false);
                    fg2.SetActive(false);
                    fg3.SetActive(true);
                    bossHead.SetActive(true);
                    bosshps.SetActive(true);
                    if (b_useing == false)
                    {
                        InterfaceMgr.doCommandByLua("a1_low_fightgame.setactive_btn", "ui/interfaces/low/a1_low_fightgame", false);
                        b_useing = true;
                    }
                }
                if (role is P2Warrior && iconWar.activeSelf == false)
                    iconWar.SetActive(true);
                else if (role is P3Mage && iconMage.activeSelf == false)
                    iconMage.SetActive(true);
                else if (role is P5Assassin && iconAss.activeSelf == false)
                    iconAss.SetActive(true);

                if (role is ohterP2Warrior && iconWar.activeSelf == false)
                {
                    iconWar.SetActive(true);
                }
                else if (role is ohterP3Mage && iconMage.activeSelf == false)
                {
                    iconMage.SetActive(true);
                }
                else if (role is ohterP5Assassin && iconAss.activeSelf == false)
                {
                    iconAss.SetActive(true);
                }
                //副本中的怪物显示等级为外观等级
                if (MapModel.getInstance().curLevelId != 0)
                {
                    txtLv.text = MapModel.getInstance().show_instanceLvl(MapModel.getInstance().curLevelId).ToString();
                    txtname.text = role.roleName;
                }
                else
                {
                    txtLv.text = role.tempXMl.getString("lv");
                    txtname.text = role.roleName;
                }
            }
            else
            {
                bossName.gameObject.SetActive(false);
                bossHPCount.gameObject.SetActive(false);
                txtname.gameObject.SetActive(true);
                bosshp1.SetActive(false);
                if (role is ProfessionRole)//玩家
                {
                    txtLv.text = (role as ProfessionRole).lvl.ToString();
                    if (role.roleName != txtname.text)
                        txtname.text = role.roleName;
                    bossBg.SetActive(false);
                    bg.SetActive(true);
                    fgim.SetActive(true);
                    otherHP.SetActive(true);
                    hp1.SetActive(true);
                    bosshp1.SetActive(false);
                    type1.SetActive(true);
                    pro.SetActive(true);
                    fg2.SetActive(true);
                    fg3.SetActive(false);
                    bossHead.SetActive(false);
                    bosshps.SetActive(false);
                    bossHP.gameObject.SetActive(false);
                    isboss = false;
                    if (b_useing == true)
                    {
                        InterfaceMgr.doCommandByLua("a1_low_fightgame.setactive_btn", "ui/interfaces/low/a1_low_fightgame", true);
                        b_useing = false;
                    }
                }

                if (role is P2Warrior && iconWar.activeSelf == false)
                    iconWar.SetActive(true);
                else if (role is P3Mage && iconMage.activeSelf == false)
                    iconMage.SetActive(true);
                else if (role is P5Assassin && iconAss.activeSelf == false)
                    iconAss.SetActive(true);
            }


        }
    }

    class TargetMenu : Skin
    {
      
        public TargetMenu(Transform trans)
            : base(trans)
        {
            initUI();
            hide();
        }

        public void initUI()
        {
            new BaseButton(transform.FindChild("black")).onClick = (GameObject g) =>
            {
                addblack();
            };
            new BaseButton(transform.FindChild("view")).onClick = (GameObject g) =>
            {
                view();
            };
            new BaseButton(transform.FindChild("bg")).onClick = (GameObject g) =>
            {
                hide();
            };
            new BaseButton(transform.FindChild("chat")).onClick = (GameObject g) =>
            {
                chat();
            };
            new BaseButton(transform.FindChild("addfre")).onClick = (GameObject g) =>
            {
                addfriend();
            };
            new BaseButton(transform.FindChild("team")).onClick = (GameObject g) =>
            {
                addTeam();
            };
            new BaseButton(transform.FindChild("union")).onClick = (GameObject g) =>
            {
                addLegion();
            };

        }

        void onCLick(GameObject go)
        {
            hide();
        }

        public void show()
        {
            visiable = true;
        }

        public void hide()
        {
            visiable = false;
        }

        void view()
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_TARGETINFO);
            hide();
        }
        void addblack()
        {
            uint cid = SelfRole._inst.m_LockRole.m_unCID;
            string name = SelfRole._inst.m_LockRole.roleName;
            FriendProxy.getInstance().sendAddBlackList(cid, name);
            hide();
        }
        void chat()
        {
            a3_chatroom._instance.privateChat(SelfRole._inst.m_LockRole.roleName);
            hide();
        }
        void addfriend()
        {
            uint cid = SelfRole._inst.m_LockRole.m_unCID;
            string name = SelfRole._inst.m_LockRole.roleName;
            FriendProxy.getInstance().sendAddFriend(cid, name);
            hide();
        }
        void addTeam()
        {

            uint cid = SelfRole._inst.m_LockRole.m_unCID;
            TeamProxy.getInstance().SendTEAM(cid);

            //if(TeamProxy.getInstance().MyTeamData==null&&)
            //TeamProxy.getInstance().SendInvite(cid);
            TeamProxy.getInstance().trage_cid = cid;

            hide();
            //return;
        }
        void addLegion()
        {
            uint cid = SelfRole._inst.m_LockRole.m_unCID;
            A3_LegionProxy.getInstance().SendInvite(cid);
            hide();
        }

    }


}
