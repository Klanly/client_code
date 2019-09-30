using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cross;
using GameFramework;
namespace MuGame
{
    public class ClientGeneralConf : configParser
    {
        public static ClientGeneralConf instance;
        public ClientGeneralConf(ClientConfig m)
            : base(m)
        {
            instance = this;
        }
        public static ClientGeneralConf create(IClientBase m)
        {

            return new ClientGeneralConf(m as ClientConfig);
        }


        override protected void onData()
        {
            //DebugTrace.print("ClientGeneralConf onData!");
        }
        override protected Variant _formatConfig(Variant conf)
        {
            if (conf.ContainsKey("carr"))
            {
                Variant carrObj = new Variant();
                foreach (Variant carrConf in conf["carr"]._arr)
                {
                    carrConf["sex"] = GameTools.array2Map(carrConf["sex"], "id");
                    carrObj[carrConf["id"]] = carrConf;
                }
                conf["carr"] = carrObj;
            }
            if (conf.ContainsKey("ltrans"))
            {
                Variant ltransObj = new Variant();
                foreach (Variant ltransConf in conf["ltrans"]._arr)
                {
                    ltransConf["t"] = GameTools.array2Map(ltransConf["t"], "txt");
                    ltransObj[ltransConf["group"]] = ltransConf;
                }
                conf["ltrans"] = ltransObj;
            }
            if (conf.ContainsKey("follow"))
            {
                conf["follow"] = GameTools.array2Map(conf["follow"], "id");
            }
            //if (conf.ContainsKey("compose"))
            //{
            //    conf["compose"] = GameTools.array2Map(conf["compose"], "id",GameTools.ClassArray);
            //}
            if (conf.ContainsKey("hots"))
            {
                conf["hots"] = GameTools.array2Map(conf["hots"], "tp");
            }
            if (conf.ContainsKey("worldChat"))
            {
                conf["worldChat"] = GameTools.array2Map(conf["worldChat"], "tp");
            }
            if (conf.ContainsKey("chatFace"))
            {
                conf["chatFace"] = GameTools.array2Map(conf["chatFace"], "tp");
            }
            if (conf.ContainsKey("mapMusic"))
            {
                conf["mapMusic"] = GameTools.array2Map(conf["mapMusic"], "mapid");
            }
            if (conf.ContainsKey("mapsound"))
            {
                conf["mapsound"] = GameTools.array2Map(conf["mapsound"], "mapid");
            }
            if (conf.ContainsKey("normalAttsound"))
            {
                conf["normalAttsound"] = GameTools.array2Map(conf["normalAttsound"], "id");
            }
            if (conf.ContainsKey("outAttsound"))
            {
                conf["outAttsound"] = GameTools.array2Map(conf["outAttsound"], "id");

            }
            if (conf.ContainsKey("skillsound"))
            {
                conf["skillsound"] = GameTools.array2Map(conf["skillsound"], "id");

            }
            if (conf.ContainsKey("dropsound"))
            {
                conf["dropsound"] = GameTools.array2Map(conf["dropsound"], "id");
            }
            if (conf.ContainsKey("picksound"))
            {
                conf["picksound"] = GameTools.array2Map(conf["picksound"], "id");
            }
            if (conf.ContainsKey("chafilter"))
            {
                conf["chafilter"] = GameTools.array2Map(conf["chafilter"], "tp");
            }
            if (conf.ContainsKey("npcsound"))
            {
                conf["npcsound"] = GameTools.array2Map(conf["npcsound"], "id");
            }
            if (conf.ContainsKey("monsound"))
            {
                conf["monsound"] = GameTools.array2Map(conf["monsound"], "id");
            }
            if (conf.ContainsKey("monAttsound"))
            {
                conf["monAttsound"] = GameTools.array2Map(conf["monAttsound"], "id");
            }
            if (conf.ContainsKey("othersound"))
            {
                conf["othersound"] = GameTools.array2Map(conf["othersound"], "id");
            }
            if (conf.ContainsKey("createPro"))
            {
                conf["createPro"] = GameTools.array2Map(conf["createPro"], "tp");
            }

            if (conf.ContainsKey("skill"))
            {
                _rotateskill = new Variant();
                foreach (Variant rotateskill in conf["skill"][0]["rotateskill"]._arr)
                {
                    _rotateskill[rotateskill["sid"]._str ] = rotateskill;
                }
                _carrSkillAction = GameTools.array2Map(conf["skill"][0]["carr"], "carr");
            }
            if (conf.ContainsKey("lottery"))
            {
                conf["lottery"] = GameTools.array2Map(conf["lottery"], "tp");
            }
            if (conf.ContainsKey("mapsize"))
            {
                conf["mapsize"] = GameTools.array2Map(conf["mapsize"], "tp");
            }
            if (conf.ContainsKey("mapsizeinfo"))
            {
                conf["mapsizeinfo"] = GameTools.array2Map(conf["mapsizeinfo"], "tp");
            }
            if (conf.ContainsKey("worldboss"))
            {
                conf["worldboss"] = GameTools.array2Map(conf["worldboss"], "tp");
            }
            if (conf.ContainsKey("worldmap"))
            {
                conf["worldmap"] = GameTools.array2Map(conf["worldmap"], "tp");
            }
            if (conf.ContainsKey("mapdropitems"))
            {
                conf["mapdropitems"] = GameTools.array2Map(conf["mapdropitems"], "tp");
            }
            if (conf.ContainsKey("common"))
            {
                conf["common"] = GameTools.array2Map(conf["common"], "tp");
            }
            if (conf.ContainsKey("lackprompt"))
            {//物品不足提示
                Variant promptConf = new Variant();
                foreach (Variant promptObj in conf["lackprompt"]._arr)
                {
                    promptObj["type"] = GameTools.array2Map(promptObj["type"], "id");
                    promptConf = promptObj["type"];
                }
                conf["lackprompt"] = promptConf;
            }
            if (conf.ContainsKey("level"))
            {
                conf["level"] = GameTools.array2Map(conf["level"], "tp");
            }
            if (conf.ContainsKey("lvlbuff"))
            {
                conf["lvlbuff"] = GameTools.array2Map(conf["lvlbuff"], "tp");
            }
            if (conf.ContainsKey("mapLanguage"))
            {
                _mapLanguageConf = new Variant();
                foreach (Variant tmp in conf["mapLanguage"].Values)
                {
                    _mapLanguageConf[tmp["mapid"]] = tmp;
                }
            }
            if (conf.ContainsKey("buybuff"))
            {
                conf["buybuff"] = GameTools.array2Map(conf["buybuff"], "tp");
            }
            if (conf.ContainsKey("mapautogame"))
            {
                conf["mapautogame"] = GameTools.array2Map(conf["mapautogame"], "tp");
            }
            if (conf.ContainsKey("randpos"))
            {
                conf["randpos"] = GameTools.array2Map(conf["randpos"], "tp");
            }
            if (conf.ContainsKey("actinfo"))
            {
                conf["actinfo"] = GameTools.array2Map(conf["actinfo"][0]["type"], "id");
            }
            if (conf.ContainsKey("itemsFeatures"))
            {
                conf["itemsFeatures"] = GameTools.array2Map(conf["itemsFeatures"], "id");
            }
            if (conf.ContainsKey("autoPoint"))
            {
                Variant autoPointConf = new Variant();
                foreach (Variant autoPointObj in conf["autoPoint"]._arr)
                {
                    autoPointObj["carr"] = GameTools.array2Map(autoPointObj["carr"], "carr");
                    autoPointConf = autoPointObj["carr"];
                }
                conf["autoPoint"] = autoPointConf;
            }
            if (conf.ContainsKey("vipAddPoint"))
            {
                Variant vipAddPointConf = new Variant();
                foreach (Variant vipPointObj in conf["vipAddPoint"]._arr)
                {
                    vipPointObj["vip"] = GameTools.array2Map(vipPointObj["vip"], "tp");
                    vipAddPointConf = vipPointObj["vip"];
                }
                conf["vipAddPoint"] = vipAddPointConf;
            }
            if (conf.ContainsKey("vipState"))
            {
                Variant vipStateConf = new Variant();
                foreach (Variant vipStateObj in conf["vipState"]._arr)
                {
                    vipStateObj["vip"] = GameTools.array2Map(vipStateObj["vip"], "lvl");
                    vipStateConf = vipStateObj["vip"];
                }
                conf["vipState"] = vipStateConf;
            }
            if (conf.ContainsKey("tranmission"))
            {
                conf["tranmission"] = GameTools.array2Map(conf["tranmission"], "id");
            }
            if (conf.ContainsKey("NPC"))
            {
                readNpcHelp(conf["NPC"][0]["npcHelp"]);
                readNpcMarket(conf["NPC"][0]["npcMarket"]);
                readRanShop(conf["NPC"][0]["npcRanShop"]);
                readGift(conf["NPC"][0]["npcGift"]);
                readTransfer(conf["NPC"][0]["npcTransfer"]);
                readMarry(conf["NPC"][0]["npcMarry"]);
                readPkKing(conf["NPC"][0]["npcPkKing"]);
                readLevel(conf["NPC"][0]["npcLevel"]);
            }
            if (conf.ContainsKey("npcfun"))
            {
                conf["npcfun"] = GameTools.array2Map(conf["npcfun"], "id");
            }
            if (conf.ContainsKey("npcshow"))
            {
                conf["npcshow"] = GameTools.array2Map(conf["npcshow"], "id");
            }
            if (conf.ContainsKey("broad_citywar_buybuff"))
            {
                conf["broad_citywar_buybuff"] = GameTools.array2Map(conf["broad_citywar_buybuff"], "tp");
            }
            if (conf.ContainsKey("boss_die_eff"))
            {
                conf["boss_die_eff"] = GameTools.array2Map(conf["boss_die_eff"], "tp");
            }
            if (conf.ContainsKey("hotKey"))
            {
                conf["hotKey"] = GameTools.array2Map(conf["hotKey"][0]["k"], "code");
            }
            if (conf.ContainsKey("broad_rmis_desc"))
            {
                conf["broad_rmis_desc"] = GameTools.array2Map(conf["broad_rmis_desc"], "tp");
            }
            if (conf.ContainsKey("broad_boss"))
            {
                conf["broad_boss"] = GameTools.array2Map(conf["broad_boss"], "tp");
            }
            if (conf.ContainsKey("broad_mall_items"))
            {
                conf["broad_mall_items"] = GameTools.array2Map(conf["broad_mall_items"], "tp");
            }
            if (conf.ContainsKey("broad_items"))
            {
                conf["broad_items"] = GameTools.array2Map(conf["broad_items"], "tp");
            }
            if (conf.ContainsKey("dmis"))
            {
                conf["dmis"] = GameTools.array2Map(conf["dmis"], "tp");
            }
            if (conf.ContainsKey("warm_hint"))
            {
                conf["warm_hint"] = GameTools.array2Map(conf["warm_hint"][0]["lvl"], "lvlid");
            }
            if (conf.ContainsKey("ol_award"))
            {
                conf["ol_award"] = GameTools.array2Map(conf["ol_award"], "gid");
            }
            if (conf.ContainsKey("auto_buff"))
            {
                conf["auto_buff"] = GameTools.array2Map(conf["auto_buff"][0]["skillbuff"], "sid");
            }
            if (conf.ContainsKey("animation"))
            {
                conf["animation"] = GameTools.array2Map(conf["animation"], "tp");
            }
            if (conf.ContainsKey("missionIcon"))
            {
                conf["missionIcon"] = GameTools.array2Map(conf["missionIcon"], "chapter");
            }
            if (conf.ContainsKey("mlineTip"))
            {
                conf["mlineTip"] = GameTools.array2Map(conf["mlineTip"], "chapter");
            }
            if (conf.ContainsKey("mlineshow3D"))
            {
                conf["mlineshow3D"] = GameTools.array2Map(conf["mlineshow3D"], "tpid");
            }
            if (conf.ContainsKey("monatk"))
            {
                _monAtk = new Variant();
                foreach (Variant monAtk in conf["monatk"]._arr)
                {
                    if (monAtk.ContainsKey("remote"))
                    {
                        monAtk["remote"] = monAtk["remote"][0];
                        monAtk["remote"]["speed"] = monAtk["remote"]["speed"]._float / 1000.0;
                    }
                    _monAtk[monAtk["monid"]] = monAtk;
                }
            }
            if (conf.ContainsKey("plyatk"))
            {
                _plyAtk = new Variant();
                foreach (Variant plyAtk in conf["plyatk"]._arr)
                {
                    if (plyAtk.ContainsKey("remote"))
                    {
                        plyAtk["remote"] = plyAtk["remote"][0];
                        plyAtk["remote"]["speed"] = plyAtk["remote"]["speed"]._float / 1000.0;
                    }
                    _plyAtk[plyAtk["carr"]] = plyAtk;
                }
            }
            if (conf.ContainsKey("misTrackShowLevel"))
            {
                conf["misTrackShowLevel"] = GameTools.array2Map(conf["misTrackShowLevel"][0]["mission"], "id");
            }
            if (conf.ContainsKey("monsters"))
            {
                conf["monsters"] = GameTools.array2Map(conf["monsters"], "tp");
            }
            if (conf.ContainsKey("avacha"))
            {
                _avaCha = new Variant();
                foreach (Variant avacha in conf["avacha"]._arr)
                {
                    _avaCha[avacha["id"]] = avacha;
                }
            }
            if (conf.ContainsKey("stateDisTip"))
            {
                conf["stateDisTip"] = GameTools.array2Map(conf["stateDisTip"], "id");
            }
            if (conf.ContainsKey("stateAddTip"))
            {
                conf["stateAddTip"] = GameTools.array2Map(conf["stateAddTip"], "id");
            }
            if (conf.ContainsKey("mapNameImg"))
            {
                conf["mapNameImg"] = GameTools.array2Map(conf["mapNameImg"], "mapid");
            }
            if (conf.ContainsKey("skillList"))
            {
                conf["skillList"] = GameTools.array2Map(conf["skillList"], "carr");
            }
            if (conf.ContainsKey("links"))
            {
                conf["links"] = GameTools.array2Map(conf["links"], "tp");
            }
            if (conf.ContainsKey("playerguide"))
            {
                conf["playerguide"] = GameTools.array2Map(conf["playerguide"], "tp");
            }
            if (conf.ContainsKey("updateboard"))
            {
                conf["updateboard"] = GameTools.array2Map(conf["updateboard"], "tp");
            }
            if (conf.ContainsKey("win_awd"))
            {
                conf["win_awd"] = GameTools.array2Map(conf["win_awd"], "ltpid");
            }
            if (conf.ContainsKey("lose_awd"))
            {
                conf["lose_awd"] = GameTools.array2Map(conf["lose_awd"], "ltpid");
            }
            if (conf.ContainsKey("acura"))
            {
                conf["acura"] = GameTools.array2Map(conf["acura"], "tpid");
            }
            if (conf.ContainsKey("mapachieve"))
            {
                conf["mapachieve"] = GameTools.array2Map(conf["mapachieve"], "idx");
            }
            if (conf.ContainsKey("achieve"))
            {
                conf["achieve"] = GameTools.array2Map(conf["achieve"], "id");
            }

            if (conf.ContainsKey("openeff"))
            {
                conf["openeff"] = GameTools.array2Map(conf["openeff"], "id");
            }
            if (conf.ContainsKey("hfactivity"))
            {
                conf["hfactivity"] = GameTools.array2Map(conf["hfactivity"], "tp");
            }
            if (conf.ContainsKey("bfactivity"))
            {
                conf["bfactivity"] = GameTools.array2Map(conf["bfactivity"], "tp");
            }
            if (conf.ContainsKey("mislinks"))
            {
                conf["mislinks"] = GameTools.array2Map(conf["mislinks"][0]["mis"], "id");
            }
            if (conf.ContainsKey("vipawd"))
            {
                conf["vipawd"] = GameTools.array2Map(conf["vipawd"], "id");
            }
            if (conf.ContainsKey("logintile"))
            {
                conf["logintile"] = GameTools.array2Map(conf["logintile"], "id");
            }
            if (conf.ContainsKey("attchangeshow"))
            {
                conf["attchangeshow"] = GameTools.array2Map(conf["attchangeshow"], "tp");
            }
            if (conf.ContainsKey("clientItem"))
            {
                conf["clientItem"]["itm"] = GameTools.array2Map(conf["clientItem"][0]["itm"], "tp");
                conf["clientItem"]["item"] = GameTools.array2Map(conf["clientItem"][0]["item"], "tpid");
            }
            if (conf.ContainsKey("misaction"))
            {
                conf["misaction"] = GameTools.array2Map(conf["misaction"][0]["mis"], "misid");
            }
            if (conf.ContainsKey("hidelvltips"))
            {
                conf["hidelvltips"] = GameTools.array2Map(conf["hidelvltips"][0]["lvl"], "tpid");
            }
            if (conf.ContainsKey("posavatar"))
            {
                conf["posavatar"] = GameTools.array2Map(conf["posavatar"][0]["avt"], "stid");
            }
            if (conf.ContainsKey("exattchain"))
            {
                conf["exattchain"]["fp"] = GameTools.array2Map(conf["exattchain"][0]["fp"], "id");
                conf["exattchain"]["exatt"] = GameTools.array2Map(conf["exattchain"][0]["exatt"], "id");
                conf["exattchain"]["flvl"] = GameTools.array2Map(conf["exattchain"][0]["flvl"], "id");
                conf["exattchain"]["exattInfo"] = GameTools.array2Map(conf["exattchain"][0]["exattInfo"], "id");
                conf["exattchain"]["flvlInfo"] = GameTools.array2Map(conf["exattchain"][0]["flvlInfo"], "id");
                conf["exattchain"]["color"] = GameTools.array2Map(conf["exattchain"][0]["color"], "id");
            }
            if (conf.ContainsKey("combptExatt"))
            {
                _combptExatt = new Variant();
                foreach (Variant exatt in conf["combptExatt"]._arr)
                {
                    Variant temp = new Variant();
                    foreach (Variant obj in exatt["grade"]._arr)
                    {
                        temp[obj["lvl"]._int] = obj["val"];
                    }
                    _combptExatt[exatt["name"]._str] = temp;
                }
            }

            if (conf.ContainsKey("randshopshow"))
            {
                conf["randshopshow"] = GameTools.array2Map(conf["randshopshow"][0]["show"], "tpid");
            }
            if (conf.ContainsKey("npctopeff"))
            {
                conf["npctopeff"] = GameTools.array2Map(conf["npctopeff"], "id");
            }
            if (conf.ContainsKey("veapon"))
            {
                conf["veapon"] = GameTools.array2Map(conf["veapon"], "tp");
            }
            if (conf.ContainsKey("respawn"))
            {
                Variant strength = GameTools.array2Map(conf["respawn"][0]["strength"], "id",GameTools.ClassArray);
                Variant banlvl = GameTools.array2Map(conf["respawn"][0]["banlvl"], "tpid", GameTools.ClassArray);
                conf["respawn"] = GameTools.createGroup("strength", strength, "banlvl", banlvl);
            }
            if (conf.ContainsKey("mount"))
            {
                conf["mount"] = GameTools.array2Map(conf["mount"], "qual");
            }
            if (conf.ContainsKey("chatlayer"))
            {
                conf["chatlayer"] = conf["chatlayer"][0];
            }
            if (conf.ContainsKey("tower"))
            {
                conf["tower"] = GameTools.array2Map(conf["tower"][0]["lvlmis"], "tpid");
            }
            if (conf.ContainsKey("attackSkill"))
            {
                Variant skills = new Variant();
                foreach (Variant ski in conf["attackSkill"]._arr)
                {
                    skills[ski["carr"]] = GameTools.split(ski["skid"]._str, ",");
                }
                conf["attackSkill"] = skills;
            }
            if (conf.ContainsKey("clkuseitem"))
            {
                conf["clkuseitem"] = GameTools.array2Map(conf["clkuseitem"], "tpid");
            }
            if (conf.ContainsKey("dailyMis"))
            {
                conf["dailyMis"] = GameTools.array2Map(conf["dailyMis"], "mis");
            }
            if (conf.ContainsKey("diemove"))
            {
                conf["diemove"] = GameTools.array2Map(conf["diemove"], "tp");
            }
            if (conf.ContainsKey("mondiemove"))
            {
                conf["mondiemove"] = conf["mondiemove"][0];
            }
            if (conf.ContainsKey("screenShake"))
            {
                Variant effArr = conf["screenShake"][0]["e"];
                if (effArr != null)
                {
                    Variant shakeObj = new Variant();
                    foreach (Variant shake in effArr._arr)
                    {
                        shakeObj[shake["name"]] = shake;
                    }
                    conf["screenShake"] = shakeObj;
                }
            }
            if (conf.ContainsKey("actpuzzle"))
            {
                conf["actpuzzle"] = conf["actpuzzle"][0];
            }
            if (conf.ContainsKey("hideAll"))
            {
                conf["hideAll"] = GameTools.array2Map(conf["hideAll"][0]["lvl"], "tpid");
            }
            if (conf.ContainsKey("openpkmode"))
            {
                conf["openpkmode"] = GameTools.array2Map(conf["openpkmode"][0]["lvl"], "tpid");
            }
            if (conf.ContainsKey("replaceAni"))
            {
                conf["replaceAni"] = GameTools.array2Map(conf["replaceAni"], "cid");
                foreach (Variant reps in conf["replaceAni"].Values)
                {
                    reps["rep"] = GameTools.array2Map(reps["rep"], "ani");
                }
            }
            if (conf.ContainsKey("clanhandle"))
            {
                conf["clanhandle"] = GameTools.array2Map(conf["clanhandle"], "id");
            }
            if (conf.ContainsKey("chatAchieve"))
            {
                conf["chatAchieve"] = GameTools.array2Map(conf["chatAchieve"], "id");
            }
            if (conf.ContainsKey("quiz"))
            {
                conf["quiz"] = GameTools.array2Map(conf["quiz"], "id");
            }
            if (conf.ContainsKey("mislinktips"))
            {
                conf["mislinktips"] = GameTools.array2Map(conf["mislinktips"][0]["mis"], "id");
            }
            if (conf.ContainsKey("reminditm"))
            {
                conf["reminditm"] = GameTools.array2Map(conf["reminditm"], "tpid");
            }
            if (conf.ContainsKey("weekgoal"))
            {
                conf["weekgoal"] = GameTools.array2Map(conf["weekgoal"], "id");
            }
            if (conf.ContainsKey("resetlevel"))
            {
                conf["resetlevel"] = GameTools.array2Map(conf["resetlevel"], "lvl");
            }
            if (conf.ContainsKey("weekgoalcarr"))
            {
                conf["weekgoalcarr"] = GameTools.array2Map(conf["weekgoalcarr"], "carr");
            }
            if (conf.ContainsKey("itmpkgs"))
            {
                conf["itmpkgs"] = GameTools.array2Map(conf["itmpkgs"], "id");
            }
            if (conf.ContainsKey("dropitmpkg"))
            {
                conf["dropitmpkg"] = GameTools.array2Map(conf["dropitmpkg"][0]["dtpid"], "id");
            }
            if (conf.ContainsKey("multiPosKil"))
            {
                conf["multiPosKil"] = GameTools.array2Map(conf["multiPosKil"], "id");
            }
            if (conf.ContainsKey("expdouble"))
            {
                conf["expdouble"] = conf["expdouble"][0];
            }
            if (conf.ContainsKey("monori"))
            {
                conf["monori"] = GameTools.array2Map(conf["monori"][0]["mon"], "mid");
            }
            if (conf.ContainsKey("options"))
            {
                conf["options"] = GameTools.array2Map(conf["options"][0]["option"], "id");
            }
            if (conf.ContainsKey("ownermon"))
            {
                conf["ownermon"] = GameTools.array2Map(conf["ownermon"], "mid");
            }
            if (conf.ContainsKey("transferitm"))
            {
                conf["transferitm"] = GameTools.array2Map(conf["transferitm"], "carr");
            }
            if (conf.ContainsKey("itemRecipe"))
            {
                conf["itemRecipe"] = GameTools.array2Map(conf["itemRecipe"][0]["info"], "needid");
            }
            if (conf.ContainsKey("comboard"))
            {
                conf["comboard"] = GameTools.array2Map(conf["comboard"], "tpid");
            }
            if (conf.ContainsKey("doubleAttDesc"))
            {
                conf["doubleAttDesc"] = GameTools.array2Map(conf["doubleAttDesc"][0]["item"], "tpid");
            }
            if (conf.ContainsKey("showcolor"))
            {
                conf["showcolor"] = GameTools.array2Map(conf["showcolor"], "type");
            }
            if (conf.ContainsKey("shopFilter"))
            {
                conf["shopFilter"] = GameTools.array2Map(conf["shopFilter"], "lvl");
                foreach (Variant sconf in conf["shopFilter"].Values)
                {
                    string str = sconf["items"];
                    Variant sarr = GameTools.split(str, ",");
                    sconf["items"] = sarr;
                }
            }

            if (conf.ContainsKey("rateItem"))
            {
                conf["rateItm"] = GameTools.array2Map(conf["rateItm"], "itmid");
            }

            if (conf.ContainsKey("mulitCompose"))
            {
                conf["mulitCompose"] = GameTools.array2Map(conf["mulitCompose"], "id");
                foreach (Variant m in conf["mulitCompose"].Values)
                {
                    m["carr"] = GameTools.array2Map(m["carr"], "id");
                    foreach (Variant carr in m["carr"].Values)
                    {
                        carr["recipe"] = GameTools.split(carr["recipe"]._str, ",");
                        //for(int i = 0; i < carr["recipe"].Count; i++)
                        //{
                        //    carr.recipe[i] = int(carr.recipe[i]);
                        //}
                    }
                }
            }
            if (conf.ContainsKey("rideskillimg"))
            {
                conf["rideskillimg"] = GameTools.array2Map(conf["rideskillimg"], "qual");
            }
            if (conf.ContainsKey("checkGroup"))
            {
                conf["checkGroup"] = GameTools.array2Map(conf["checkGroup"], "id");
            }
            if (conf.ContainsKey("crosswar"))
            {
                conf["crosswar"] = GameTools.array2Map(conf["crosswar"], "id");
            }
            if (conf.ContainsKey("fashion"))
            {
                conf["fashion"] = GameTools.array2Map(conf["fashion"], "id");
            }
            if (conf.ContainsKey("decompgrade"))
            {
                conf["decompgrade"] = GameTools.array2Map(conf["decompgrade"], "id");
            }
            if (conf.ContainsKey("hasRateItm"))
            {
                conf["hasRateItm"] = GameTools.array2Map(conf["hasRateItm"], "itmid");
            }
            if (conf.ContainsKey("crosswarAchieve"))
            {
                conf["crosswarAchieve"] = conf["crosswarAchieve"][0];
            }
            if (conf.ContainsKey("boxId"))
            {
                conf["boxId"] = GameTools.array2Map(conf["boxId"], "id");
            }
            if (conf.ContainsKey("mapeffect"))
            {
                conf["mapeffect"] = GameTools.array2Map(conf["mapeffect"], "lpid");
                foreach (Variant meff in conf["mapeffect"].Values)
                {
                    meff["blockZone"] = GameTools.array2Map(meff["blockZone"], "id");
                }
            }
            if (conf.ContainsKey("lvlNeedHide"))
            {
                conf["lvlNeedHide"] = GameTools.array2Map(conf["lvlNeedHide"], "id");
            }
            if (conf.ContainsKey("monwarn"))
            {
                conf["monwarn"] = GameTools.array2Map(conf["monwarn"], "sid");
            }
            if (conf.ContainsKey("redPaper"))
            {
                conf["redPaper"] = GameTools.array2Map(conf["redPaper"], "id");
            }
            if (conf.ContainsKey("mapstateff"))
            {
                conf["mapstateff"] = GameTools.array2Map(conf["mapstateff"], "sid");
            }
            if (conf.ContainsKey("systemset"))
            {
                conf["systemset"] = GameTools.array2Map(conf["systemset"], "tp");
            }
            if (conf.ContainsKey("secondInterface"))
            {
                conf["secondInterface"] = GameTools.array2Map(conf["secondInterface"][0]["face"], "oid");
            }
            if (conf.ContainsKey("levelhall"))
            {
                conf["levelhall"] = GameTools.array2Map(conf["levelhall"], "id");
            }
            if (conf.ContainsKey("transcriptinfo"))
            {
                conf["transcriptinfo"] = GameTools.array2Map(conf["transcriptinfo"], "tpid");
            }
            return conf;
        }
        //------------------------地图技能持续特效-------------------------------------
        public Variant GetMapstatEff(uint sid)
        {
            if (m_conf.ContainsKey("mapstateff"))
            {
                return m_conf["mapstateff"][sid];
            }
            return null;
        }
        //------------------------掉东西-------------------------------------
        public Variant GetRedPaper(uint sid)
        {
            if (m_conf.ContainsKey("redPaper"))
            {
                return m_conf["redPaper"][sid];
            }
            return null;
        }
        //-----------------------怪物预警特效配置--------------------------------------
        public Variant GetMonWarningEff(uint sid)
        {
            if (m_conf.ContainsKey("monwarn"))
            {
                return m_conf["monwarn"][sid];
            }
            return null;
        }
        //-----------------------开启宝箱广播配置--------------------------------------
        public uint GetBoxId(uint id)
        {
            uint type = 0;
            foreach (Variant arr in m_conf["boxId"]._arr)
            {
                Variant boxArr = GameTools.split(arr["id"], ",");
                for (int i = 0; i < boxArr.Count; i++)
                {
                    if (id == boxArr[i])
                    {
                        type = arr["type"];
                        break;
                    }
                }
            }
            return type;
        }
        //-----------------------锻造提炼配置--------------------------------------
        public Variant GetDecompGradeConf()
        {
            if (m_conf.ContainsKey("decompgrade"))
            {
                return m_conf["decompgrade"][0];
            }
            return null;
        }

        //-----------------------双属性描述配置--------------------------------------
        public Variant GetDoubleAttDescConf(uint tpid)
        {
            if (m_conf.ContainsKey("doubleAttDesc"))
            {
                return m_conf["doubleAttDesc"][tpid];
            }
            return null;
        }
        //-----------------------合成广播配置--------------------------------------
        public Variant GetComboardConf(uint tpid)
        {
            if (m_conf.ContainsKey("comboard"))
            {
                return m_conf["comboard"][tpid];
            }
            return null;
        }
        //获取合成宝石配方
        public Variant GetItemRecipe(uint needid)
        {
            if (m_conf.ContainsKey("itemRecipe"))
            {
                return m_conf["itemRecipe"][needid];
            }
            return null;
        }

        public Variant GetTransferItem(int carr)
        {
            if (m_conf.ContainsKey("transferitm"))
            {
                return m_conf["transferitm"][carr.ToString()];
            }
            return null;
        }
        public Variant GetTransferAllMis(int carr)
        {
            Variant arr = new Variant();
            Variant tranItms = GetTransferItem(carr);
            if (null != tranItms && null != tranItms["item"])
            {
                foreach (Variant obj in tranItms["item"]._arr)
                {
                    if (null != obj["misid"])
                    {
                        Variant tmps = GameTools.split(obj["misid"]._str, ",",GameConstantDef.SPLIT_TYPE_STRING);
                        arr._arr.AddRange(tmps._arr);
                    }
                }
            }
            /// 大于0：左大于右
            /// 等于0：左等于右
            /// 小于0：左小于右
            arr._arr.Sort((left, right) =>
                {
                    int ret = 0;
                    int iLeft = int.Parse(left._str);
                    int iRight = int.Parse(right._str);
                    if (iLeft > iRight) ret = 1;
                    if (iLeft == iRight) ret = 0;
                    if (iLeft < iRight) ret = -1;
                    return ret;
                });
            return arr;
        }
        //----------------------------召唤怪物配置--------------------------
        public Variant GetOwnerMonConf(int mid)
        {
            if (m_conf.ContainsKey("ownermon"))
            {
                return m_conf["ownermon"][mid.ToString()];
            }
            return null;
        }
        //----------------------------平台功能开放控制配置---------------------------
        private Variant _options;
        public Variant GetPlatOption()
        {
            if (_options == null)
            {
                _options = new Variant();
            }
            if (m_conf.ContainsKey("options"))
            {
                foreach (Variant obj in m_conf["options"]._arr)
                {
                    Variant plats = GameTools.split(obj["plat"]._str, ",");
                    for (int i = 0; i < plats.Count; i++)
                    {
                        if (plats[i] == "")
                        {
                            continue;
                        }

                        //plats[i] = int(plats[i]);
                    }
                    //_options[obj["id"]] = {obj id.id, plats plat, uint open(obj.open)};
                    GameTools.createGroup("id", obj["id"], "plat", plats, "open", obj["open"]);
                }
            }

            return _options;
        }
        //----------------------------怪物朝向配置---------------------------
        public Variant GetMonOriConf(int mid)
        {
            if ( m_conf.ContainsKey("monori") )
            {
                return m_conf[ "monori" ][ mid.ToString() ];
            }

            return null;
        }

        public String GetExpDoubleTime()
        {
            if (m_conf.ContainsKey("expdouble"))
            {
                return m_conf["expdouble"]["time"];
            }
            return null;
        }
        //----------------------------掉落包配置---------------------------
        public Variant GetDropItmpkg(uint dpid)
        {
            if (m_conf.ContainsKey("dropitmpkg"))
            {
                return m_conf["dropitmpkg"][dpid];
            }

            return null;
        }
        //----------------------------打包物品配置---------------------------
        public Variant GetItmpkgsByType(string type)
        {
            if (m_conf.ContainsKey("itmpkgs"))
            {
                foreach (Variant pkgs in m_conf["itmpkgs"]._arr)
                {
                    if (pkgs["type"] == type)
                    {
                        return pkgs["item"];
                    }
                }
            }
            return null;
        }
        //----------------------------七天目标配置---------------------------
        public Variant GetWeekGoalByCarr(uint carr)
        {
            if (m_conf.ContainsKey("weekgoalcarr"))
            {
                Variant carrObj = m_conf["weekgoalcarr"][carr];
                Variant ids = GameTools.createGroup(carrObj["id"]._str, ",");
                //for(int i = 0; i < ids.Count; i++)
                //{
                //    ids[i] = uint(ids[i]);
                //}
                return ids;
            }
            return null;
        }
        public Variant GetWeekGoalById(uint id)
        {
            if (m_conf.ContainsKey("weekgoal"))
            {
                return m_conf["weekgoal"][id];
            }
            return null;
        }
        public Variant GetWeekGoalByName(uint id, string name)
        {
            if (m_conf.ContainsKey("weekgoal"))
            {
                Variant goal = m_conf["weekgoal"][id]["goal"];
                foreach (Variant obj in goal._arr)
                {
                    if (obj["name"] == name)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        //----------------------------指定物品提示弹窗---------------------------
        public Variant GetRemindItem(uint tpid)
        {
            if (m_conf.ContainsKey("reminditm"))
            {
                return m_conf["reminditm"][tpid];
            }
            return null;
        }
        //----------------------------任务链接提示---------------------------
        public Variant GetMisLinkTips(int id)
        {
            if (m_conf.ContainsKey("mislinktips"))
            {
                return m_conf["mislinktips"][id];
            }
            return null;
        }

        //----------------------------称号---------------------------
        public Variant GetChatAchieveShow(int id)
        {
            if (m_conf.ContainsKey("chatAchieve"))
            {
                Variant shows = m_conf["chatAchieve"][0]["show"];
                foreach (Variant showObj in shows.Values)
                {
                    if (showObj["id"] == id)
                    {
                        return showObj;
                    }
                }

            }
            return null;
        }
        private Variant _achiveSort;
        public Variant GetChatAchieveSort()
        {
            if (_achiveSort == null && m_conf.ContainsKey("chatAchieve"))
            {
                if (m_conf["chatAchieve"]["0"].ContainsKey("sort"))
                {
                    String sortStr = m_conf["chatAchieve"]["0"]["sort"];
                    Variant sortArr = GameTools.split(sortStr, ",");
                    for (int i = 0; i < sortArr.Count; i++)
                    {
                        sortArr[i] = (sortArr[i]);
                    }
                    _achiveSort = sortArr;
                }
            }
            return _achiveSort;
        }
        //----------------------------战盟升级功能操作---------------------------
        public Variant GetClanHandleConf()
        {
            return conf["clanhandle"];
        }
        //----------------------------进入指定副本开启PK模式相关---------------------------
        /**
         * 进入副本开启Pk模式配置
         */
        public Variant GetOpenPKModeConf(int ltpid)
        {
            if (m_conf.ContainsKey("openpkmode"))
            {
                return m_conf["openpkmode"][ltpid];
            }
            return null;
        }
        //----------------------------进入指定副本屏蔽相关---------------------------
        /**
         * 进入副本屏蔽所有配置
         */
        public Variant GetHideAllConf(int ltpid)
        {
            if (m_conf.ContainsKey("hideAll"))
            {
                return m_conf["hideAll"][ltpid];
            }
            return null;
        }
        //----------------------------拼图活动相关---------------------------
        /**
         * 拼图合成配置
         */
        public Variant GetActPuzzleRecipe()
        {
            if (m_conf["actpuzzle"].ContainsKey("comp"))
            {
                return m_conf["actpuzzle"]["comp"];
            }
            return null;
        }
        /**
         * 拼图有关道具显示图片配置
         */
        public Variant GetActPuzzleItmConf()
        {
            if (m_conf["actpuzzle"].ContainsKey("itm"))
            {
                return m_conf["actpuzzle"]["itm"];
            }
            return null;
        }
        //获得地图link点特效
        public String GetMapLinkEff(int mapid, int go_to)
        {
            if (m_conf.ContainsKey("linkEffect"))
            {
                foreach (Variant obj in m_conf["linkEffect"]._arr)
                {
                    if (obj["mapid"]._int == mapid)
                    {
                        foreach (Variant data in obj["map"]._arr)
                        {
                            if (data["goto"]._int == go_to)
                            {
                                return data["effID"];
                            }
                        }
                    }
                }
            }
            return "";
        }
        //----------------------------屏幕震动-----------------------------
        public Variant GetShakeConf(string id)
        {
            return m_conf["screenShake"][id];
        }
        //----------------------------怪物击退相关---------------------------
        /**
         * 获取一个随机的击退类型
         * */
        public Variant GetRandomDieMove(int dir)
        {
            if (m_conf["diemove"])
            {
                int len = m_conf["diemove"].Count;
                int randNum = ConfigUtil.getRandom(0, len);//越界则表示无击退

                return GetDieMove(randNum, dir);
            }
            return null;
        }
        /**
		 * 通过类型和方向获取死亡击退配置
		 * */
        public Variant GetDieMove(int tp, int dir)
        {
            Variant diemove = m_conf["diemove"][tp];
            if (diemove)
            {
                foreach (Variant move in diemove["move"]._arr)
                {
                    if (move["dir"]._int == dir)
                    {
                        return move;
                    }
                }
            }
            return null;
        }
        public Variant GetMonDieMove(int mid, int dir)
        {
            Variant diemove = m_conf["diemove"];
            foreach (Variant move in diemove._arr)
            {
                if (move["mid"] == mid)
                {
                    return GetDieMove(move["movetp"], dir);
                }
            }
            return null;
        }
        private Variant _nomoveData;
        public Boolean IsNoMove(int mid)
        {
            if (!_nomoveData)
            {
                _nomoveData = new Variant();
                if (null != m_conf["mondiemove"] && null != m_conf["mondiemove"]["nomove"])
                {
                    Variant nomove = m_conf["mondiemove"]["nomove"][0];
                    string mids = nomove["mids"]._str;
                    Variant midArr = GameTools.split(mids, ",");
                    foreach (string id in midArr._arr)
                    {
                        _nomoveData[id] = Int32.Parse(id);
                    }
                }
            }
            return _nomoveData[mid];
        }
        //------------------------------- 点击使用物品 -------------------------------------------
        public Variant GetClkUseItemConf(uint tpid)
        {
            if (m_conf.ContainsKey("clkuseitem"))
            {
                return m_conf["clkuseitem"][tpid];
            }
            return null;
        }

        //------------------------------- 爬塔 -------------------------------------------
        public Variant GetTowerLvlMis(uint ltpid)
        {
            return m_conf["tower"][ltpid];
        }

        //------------------------------- 聊天框样式 -------------------------------------------
        public Variant GetChatLayer(int layer)
        {
            return m_conf["chatlayer"]["layer"][layer];
        }
        public int ChatMaxLayer()
        {
            return m_conf["chatlayer"]["max"];
        }
        public int GetMiniChatLayer()
        {
            Variant layerArr = m_conf["chatlayer"]["layer"];
            int tp = 0;
            foreach (Variant layer in layerArr._arr)
            {
                if (null != layer["mini"])
                {
                    tp = layer["tp"];
                    break;
                }
            }
            return tp;
        }
        //------------------------------- 死亡复活提示 -------------------------------------------
        public int GetRespawnStrengthPageLen()
        {
            int len = 0;
            foreach (Variant obj in m_conf["respawn"]["strength"]._arr)
            {
                if (null != obj["itm"])
                {
                    ++len;
                }
            }
            return len;
        }
        public Variant GetRespawnStrength(int id)
        {
            Variant strength = m_conf["respawn"]["strength"][id];
            if (null != strength)
            {
                return strength["itm"];
            }
            return null;
        }
        public Boolean GetRespawnBanLvl(int tpid)
        {
            Variant banlvl = m_conf["respawn"]["banlvl"][tpid];
            if (null != banlvl)
            {
                return true;
            }
            return false;
        }
        //------------------------------- npc头顶特效 -------------------------------------------
        public Variant GetNpcTopEff(int id)
        {
            return m_conf["npctopeff"][id];
        }
        //------------------------------- 藏宝阁展示 -------------------------------------------
        public Variant GetRandShopShow(int tpid)
        {
            foreach(string obj in m_conf["randshopshow"].Keys)
                if(m_conf["randshopshow"][obj]["tpid"]._int==tpid)
                    return m_conf["randshopshow"][obj];
            return null;
        }
        public Variant GetRandShopShowIdArr()
		{
			Variant arr = new Variant();
            foreach (Variant obj in m_conf["randshopshow"].Keys)
			{
				    arr.pushBack(m_conf["randshopshow"][obj+""]["tpid"]._int);
			}
			return arr.Length > 0 ? arr:null;
		}
        //------------------------------- 卓越、强化连锁 -------------------------------------------
        public Variant GetPosAvatar(int stid)
        {
            return m_conf["posavatar"][stid];
        }
        //------------------------------- 卓越、强化连锁 -------------------------------------------
        public Variant GetExattchainAtt(int id)
        {
            return m_conf["exattchain"]["exatt"][id];
        }
        public Variant GetExattchainFlvl(int id)
        {
            return m_conf["exattchain"]["flvl"][id];
        }
        public Variant GetExattchainAttInfo(int id)
        {
            return m_conf["exattchain"]["exattInfo"][id];
        }
        public Variant GetExattchainFlvlInfo(int id)
        {
            return m_conf["exattchain"]["flvlInfo"][id];
        }
        public Variant GetExattchainColor(int id)
        {
            return m_conf["exattchain"]["color"][id];
        }

        //------------------------------- 进入副本需要屏蔽一些选项 -------------------------------------------
        public Variant GetHideLvlTips(int tpid)
        {
            return m_conf["hidelvltips"][tpid];
        }

        public bool is_need_show_hidetips(int tpid)
        {
            Variant hideObj = GetHideLvlTips(tpid);
            if (hideObj != null)
            {
                return true;
            }
            return false;
        }

        //------------------------------- 点击任务链接所以做的动作 -------------------------------------------
        public Variant GetMisAction(int misid)
        {
            return m_conf["misaction"][misid];
        }
        //------------------------------- 用于客户端显示的物品实例  -------------------------------------------
        public Variant GetClientItem(int id)
        {
            return m_conf["clientItem"]["itm"][id];
        }
        //------------------------------- 登陆奖励  -------------------------------------------
        public Variant GetLoginTile()
        {
            return m_conf["logintile"];
        }
        /**
         * 获取vip的客户端配置奖励
         * */
        public Variant GetClientVipAwd(int id)
        {
            return m_conf["vipawd"][id.ToString()]["itm"];
        }
        public int GetBuyVipId(int id)
        {
            return m_conf["vipawd"][id.ToString()]["tpid"]._int;
        }






        private Variant _combptExatt;
        private Variant _avaCha;
        private Variant _monAtk;
        private Variant _plyAtk;
        private Variant _npcHelpData;
        private void readNpcHelp(Variant node)
        {
            _npcHelpData = new Variant();
            int npcid = node[0]["npcid"]._str == "" ? 0 : node[0]["npcid"]._int;
            if (null != _npcHelpData[npcid])
            {
                _npcHelpData[npcid]["desc"]._arr.Add(node[0]["desc"]._str);
            }
            else
            {
                Variant desc = new Variant();
                desc._arr.Add(node[0]["desc"]._str);
                _npcHelpData[npcid] = new Variant();
                _npcHelpData[npcid]["npcid"] = npcid;
                _npcHelpData[npcid]["name"] = node[0]["name"]._str;
                _npcHelpData[npcid]["desc"] = desc;
            }
        }
        public Variant get_npcHelpData(int npcid)
        {
            if (!_npcHelpData.ContainsKey(npcid))
                return null;
            return _npcHelpData[npcid];
        }

        private Variant _npcMarketData;
        private void readNpcMarket(Variant node)
        {
            _npcMarketData = new Variant();
            string temp = node[0]["npcid"]._str;
            Variant arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
            foreach (string str in arr._arr)
            {
                _npcMarketData[str] = str;
            }
        }
        public Variant get_npcMarketData(int npcid)
        {
            return _npcMarketData[npcid];
        }
        private Variant _npcRanShopData;
        private void readRanShop(Variant node)
        {
            _npcRanShopData = new Variant();
            string temp = node[0]["npcid"]._str;
            Variant arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
            foreach (string str in arr._arr)
            {
                _npcRanShopData[str] = str;
            }
        }
        public Variant get_npcRanShopData(int npcid)
        {
            return _npcRanShopData[npcid];
        }
        private Variant _npcGiftData;
        private void readGift(Variant node)
        {
            _npcGiftData = new Variant();

            string temp = node[0]["npcid"]._str;
            Variant Arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
            foreach (string str in Arr._arr)
            {
                _npcGiftData[str] = str;
            }
        }
        public Variant get_npcGiftData(int npcid)
        {
            return _npcGiftData[npcid];
        }
        //npc转职
        private Variant _npcTransferData;
        private void readTransfer(Variant node)
        {
            _npcTransferData = new Variant();
            string temp = node[0]["npcid"];
            Variant Arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
            foreach (string str in Arr._arr)
            {
                _npcTransferData[str] = str;
            }
        }
        public Variant get_npcTransferData(int npcid)
        {
            return _npcTransferData[npcid];
        }
        //npc结缘
        private Variant _npcMarryData;
        private void readMarry(Variant node)
        {
            _npcMarryData = new Variant();
            string temp = node[0]["npcid"];
            Variant Arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
            foreach (string str in Arr._arr)
            {
                _npcMarryData[str] = str;
            }
        }
        public Variant get_npcMarryData(int npcid)
        {
            return _npcMarryData[npcid];
        }
        //膜拜PK之王
        private Variant _npcPkKingData;
        private void readPkKing(Variant node)
        {
            _npcPkKingData = new Variant();
            string temp = node[0]["npcid"];
            Variant Arr = GameTools.split(temp, ",", GameConstantDef.SPLIT_TYPE_STRING);
            foreach (string str in Arr._arr)
            {
                _npcPkKingData[str] = str;
            }
        }
        public Variant get_npcPkKingData(int npcid)
        {
            return _npcPkKingData[npcid];
        }
        //副本npc
        private Variant _npcLevelData;
        private void readLevel(Variant node)
        {
            _npcLevelData = new Variant();
            foreach (Variant nodeObj in node._arr)
            {
                _npcLevelData[nodeObj["tpid"]] = nodeObj;
                if (nodeObj.ContainsKey("linknpc"))
                {
                    Variant linkArr = GameTools.split(nodeObj["linknpc"], ",", GameConstantDef.SPLIT_TYPE_INT);

                    _npcLevelData[nodeObj["tpid"]]["linknpc"] = linkArr;
                }
            }
        }
        public Variant GetNpcLevelData(uint ltpid, int npcid)
        {
            Variant nlData = _npcLevelData[ltpid.ToString()];
            if (nlData != null && nlData["level"] != null)
            {
                foreach (Variant npcObj in nlData["level"].Values)
                {
                    if (npcObj["npcid"]._int == npcid)
                    {
                        return npcObj;
                    }
                }
            }
            return null;
        }
        public Variant GetNpcLevelConf(uint ltpid)
        {
            return _npcLevelData[ltpid];
        }

        //private Variant _rotateskill;
        //private Variant _carrSkillAction;
        private Variant _mapLanguageConf;
        public Variant getmap_need_lvl
        {
            get
            {
                Variant map_need = new Variant();
                for (int i = 0; i < conf["map_need_lvl"].Count; i++)
                {
                    map_need[conf["map_need_lvl"][i]["mapid"]._int] = conf["map_need_lvl"][i]["lvl"]._int;
                }
                return map_need;
            }
        }


        //public function  GetCarrConfig( carr:uint, sex:uint ):Object
        //{
        //    if( !(carr in this._conf.carr) )
        //    {
        //        return null;
        //    }

        //    var carrObj:Object = this._conf.carr[carr];
        //    return carrObj.sex[sex];
        //}

        public Variant GetCarrConfig(uint carr, uint sex)
        {
            if (m_conf["carr"][carr.ToString()] == null)
            {
                return null;
            }
            Variant carrObj = m_conf["carr"][carr.ToString()];
            return carrObj["sex"][sex.ToString()];
        }

        private Variant _comConfs;
        public Variant GetCommonConf(string id)
        {
            if (null != _comConfs)
            {
                return _comConfs[id];
            }
            _comConfs = new Variant();
            Variant temp = conf["common"]["0"]["cfg"];
            foreach (Variant cfg in temp._arr)
            {
                _comConfs[cfg["id"]] = cfg["val"];
            }
            return _comConfs[id];
        }


        private Variant _attChangeShowArr;
        public int IsAttChangeShow(string att)
        {
            if (null == _attChangeShowArr)
            {
                _attChangeShowArr = new Variant();
                foreach (string s in conf["attchangeshow"].Keys)
                {
                    Variant linkArr = GameTools.split(conf["attchangeshow"][s]._str, ",", GameConstantDef.SPLIT_TYPE_STRING);
                    _attChangeShowArr[s] = linkArr;
                }
            }

            foreach (string ss in _attChangeShowArr.Keys)
            {
                if (_attChangeShowArr[ss]._str.IndexOf(att) > -1)
                {
                    return int.Parse(ss);
                }
            }
            return -1;
        }
        private Variant _thousandArr;
        private void initThousand()
        {
            if (null == _thousandArr)
            {
                _thousandArr = new Variant();
                Variant conf = GetCommonConf("thousand_Str");
                string str = conf._str;
                _thousandArr = GameTools.split(str, ",", GameConstantDef.SPLIT_TYPE_STRING);
            }
        }
        /**
         * 将属性字段中千分比转换为百分比
         * */
        public float changethousandtohundred(string name, float val)
        {
            initThousand();
            float temp = 0;
            foreach (string str in _thousandArr._arr)
            {
                if (name == str)
                {
                    temp = val / 10;
                    return temp;
                }
            }
            temp = val;
            return temp;
        }
        /**
         * 是否属于千分比的字段
         * */
        public Boolean IsThousand(string name)
        {
            initThousand();
            return (_thousandArr._str.IndexOf(name) > -1);
        }
        //-------------------------------------------  文字替换 相关 -----------------------------------------
        private Variant getLangtransRepText(string group, string txt)
        {
            Variant ltransConf = conf["ltrans"];
            if (ltransConf != null)
            {
                Variant gConf = ltransConf[group];
                if (gConf != null)
                {
                    Variant tConf = gConf["t"][txt];
                    if (tConf != null)
                    {
                        return tConf["rept"];
                    }
                }
            }
            return null;
        }

        public String FormatSuperTextXML(string str, string fmtStr)
        {
            string newStr = "text=\"" + str + "\" ";

            if (fmtStr != null)
            {
                Variant fmtArr = GameTools.split(fmtStr, ",");
                foreach (string f in fmtArr._arr)
                {
                    Variant fArr = GameTools.split(f, "=");
                    if (fArr.Length == 2)
                    {
                        switch (fArr[0]._str)
                        {
                            case "c":
                                {
                                    Variant rept = getLangtransRepText("c", fArr[1]._str);
                                    if (rept != null)
                                        newStr += "color=\"0x" + rept._str;
                                    else
                                        newStr += "color=\"" + fArr[1]._str;

                                } break;
                            case "u":
                                {
                                    newStr += "underline=\"" + fArr[1]._str;
                                } break;
                            case "b":
                                {
                                    newStr += "bold=\"" + fArr[1]._str;
                                } break;
                            case "fmt":
                                {
                                    newStr += "format=\"" + fArr[1]._str;
                                } break;
                            case "sz":
                                {
                                    newStr += "size=\"" + fArr[1]._str;
                                } break;
                            case "st":
                                {
                                    newStr += "style=\"" + fArr[1]._str;
                                } break;
                            case "sp":
                                {
                                    newStr += "letterSpacing=\"" + fArr[1];
                                } break;
                            case "clk":
                                {
                                    newStr += "onclick=\"" + fArr[1]._str;
                                } break;
                            case "ckp":
                                {
                                    newStr += "clickpar=\"" + fArr[1]._str;
                                } break;
                            case "ovr":
                                {
                                    newStr += "onover=\"" + fArr[1]._str;
                                } break;
                            case "vrp":
                                {
                                    newStr += "overpar=\"" + fArr[1]._str;
                                } break;
                            case "out":
                                {
                                    newStr += "onout=\"" + fArr[1]._str;
                                } break;
                            case "otp":
                                {
                                    newStr += "outpar=\"" + fArr[1]._str;
                                } break;
                            case "evp":
                                {
                                    newStr += "eventpar=\"" + fArr[1]._str;
                                } break;
                            default:
                                continue;
                        }
                        newStr += "\" ";

                    }
                    else if (fArr.Length > 2)
                    {
                        string type = fArr[0]._str;
                        if ("ckp" == type)
                        {
                            newStr += "clickpar=\"";
                            int len = fArr.Length;
                            for (int i = 1; i < len; ++i)
                            {
                                newStr += fArr[i]._str;
                                if (i < len - 1)
                                {
                                    newStr += "=";
                                }
                            }
                            newStr += "\" ";
                        }
                    }
                }
            }

            return GameConstant.SUPERTXT_PROFIX + newStr + GameConstant.SUPERTXT_END;
        }

        public String TransToSuperText(string str)
        {
            if (str == null)
                return str;
            Regex SUBS_RE = new Regex(@"{[^<>\{\}\[\]\(\)\(\)]+}");
            Match match = SUBS_RE.Match(str);

            if (match == null || match.ToString() == "")
            {
                Regex reg = new Regex(@"<(.*?)\/>");
                Match matchArr = reg.Match(str);
                if (matchArr != null && matchArr.ToString() != "")
                {
                    return str;
                }
                return FormatSuperTextXML(str, null);
            }
            string newStr = "";
            string matchTxt;
            string fmtTxt = null;
            while (match != null && match.ToString() != "")
            {
                if (match.Index > 0)
                {
                    newStr += FormatSuperTextXML(str.Substring(0, match.Index), fmtTxt);
                }

                matchTxt = match.ToString();
                // 将text指向匹配字符串后的文字
                str = str.Substring(match.Index + matchTxt.Length);
                // 去除{}
                matchTxt = matchTxt.Substring(1, matchTxt.Length - 2);

                switch (matchTxt)
                {
                    case "end"://结束  清除格式
                        fmtTxt = null;
                        break;
                    case "br"://换行  不清除格式
                        newStr += GameConstant.SUPERTXT_BR;
                        break;
                    default:
                        fmtTxt = matchTxt;
                        break;
                }
                //{[_a-zA-Z.,=:\/?-]+[ _a-zA-Z0-9.,=:\/?-]*(\([^\)]*\))?}
                SUBS_RE = new Regex(@"{[^<>\{\}\[\]\(\)\(\)]+}");
                match = SUBS_RE.Match(str, 0);
            }

            if (str.Length > 0)
            {
                newStr += FormatSuperTextXML(str, fmtTxt);
            }

            //防止格式错误
            //Regex reg = new Regex(@"<(.*?)\/>");
            //Match matchArr = reg.Match(newStr);
            //string cnewStr = "";
            //foreach (Match ms in matchArr.Groups)
            //{
            //    cnewStr += ms.ToString();
            //}
            //if(CONFIG::DEBUG)
            //{
            //    if(cnewStr.Length != newStr.Length)
            //    {
            //        DebugTrace.add(DebugTrace.DTT_ERR,"wrong supertext format");
            //    }
            //}
            return newStr;
        }

        //------------------------------------------------ 跟随物 --------------------------------------------
        public Variant GetFollowConf(uint id)
        {

            Variant followConf = conf["follow"];
            if (followConf != null)
            {
                return followConf[id.ToString()];
            }
            return null;
        }

        //-----------------------------------------------------------------------------------------------------
        public Variant getPlayerCarrStruct(uint carr, uint carrlvl)
        {
            if (!(conf["carr"].ContainsKey(carr.ToString())))
            {
                return "";
            }
            Variant carrConf = this.conf["carr"][carr.ToString()];
            if (carrConf["carrName"][(int)carrlvl] == null)
            {
                return this.conf["carr"]["name"];
            }

            return carrConf["carrName"][(int)carrlvl];
        }


        //根据职业获取头像
        public String getPlayerHeadImgPath(uint uCarr, uint uCarrlvl = 0)
        {
            if (this.conf.ContainsKey("headImg"))
            {
                Variant objHeadConf = conf["headImg"];
                for (int i = 0; i < objHeadConf[0]["img"].Count; i++)
                {
                    if (objHeadConf[0]["img"][i]["carr"]._uint == uCarr)
                    {
                        return objHeadConf[0]["img"][i]["content"];
                    }
                }

            }

            return "";

        }

        public String getNotificationImgPath(string strNotifyType)
        {
            if (this.conf.ContainsKey("notification"))
            {
                Variant objNotificationConf = conf["notification"];
                //for (int i = 0; i < objNotificationConf[0]["item"]._arr.Count; i++)
                //{
                //    if (objNotificationConf[0]["item"][i.ToString()]["type"] == strNotifyType)
                //    {
                //        return objNotificationConf[0]["item"][i.ToString()]["path"];
                //    }
                //}
                foreach (Variant obj in objNotificationConf[0]["item"]._arr)
                {
                    if (obj["type"]._str == strNotifyType)
                    {
                        return obj["path"]._str;
                    }
                }

            }
            return "";

        }

        //---------------------------------- 物品合成 start---------------------------------------------------
        public Variant get_compose_conf()
        {
            if (this.conf == null)
            {
                return null;
            }
            return conf["compose"];
        }
        public Variant GetItemComposeBytype(int recipe, int type)
        {
            foreach (Variant obj in conf["compose"]._arr)
            {
                Variant items = obj["items"];
                foreach (Variant itmobj in items._arr)
                {
                    Variant infoArr = itmobj["tp"]._arr[0]["info"];
                    foreach (Variant tpobj in infoArr._arr)
                    {
                        if (tpobj["recipe"]._int == recipe && tpobj["type"]._int == type)
                        {
                            return infoArr;
                        }
                    }
                }
            }
            return null;
        }

        public Variant GetItemComposeRecipe(uint tpid, int comp = 0)
        {

            foreach (Variant obj in conf["itemRecipe"].Values)
            {
                if (comp != 0)
                {
                    if (obj["needid"]._uint == tpid && obj.ContainsKey("comp") && obj["comp"]._int != 0)
                    {
                        return obj;
                    }
                }
                else if (comp == 0 && obj["needid"]._uint == tpid && (!obj.ContainsKey("comp") || obj["comp"]._int == 0))
                {
                    return obj;
                }
            }
            return null;
        }

        public Variant get_actinfo_conf()
        {
            if (this.conf == null)
            {
                return null;
            }
            return this.conf["actinfo"];
        }
        public Variant GetConfByTypeId(string type, uint id)
        {
            if (this.conf == null || !conf["actinfo"].ContainsKey(type))
            {
                return null;
            }
            Variant arr = this.conf["actinfo"][type]["act"];
            foreach (Variant obj in arr._arr)
            {
                if (obj["id"]._uint == id)
                {
                    return obj;
                }
            }
            return null;
        }


        public Variant get_hotsell_conf()
        {
            if (this.conf == null)
            {
                return null;
            }
            return this.conf["hots"];
        }

        public Variant get_worldChat_conf()
        {
            if (this.conf == null)
            {
                return null;
            }
            return this.conf["worldChat"];
        }

        public Variant get_chatFace_conf()
        {
            if (this.conf == null)
            {
                return null;
            }
            return this.conf["chatFace"];
        }

        //声音相关
        //地图背景音乐
        public Variant getMapMusic(uint mpid)
        {
            Variant mapMusic = conf["mapMusic"];
            if (mapMusic != null)
            {
                return mapMusic[mpid.ToString()];
            }
            return null;
        }
        //地图背景音效
        public Variant getMapSound(uint mpid)
        {
            Variant mapsound = conf["mapsound"];
            if (mapsound != null)
            {
                return mapsound[mpid.ToString()];
            }
            return null;
        }

        //怪物音效
        public Variant getMonSound(uint id)
        {
            Variant monsound = conf["monsound"];
            if (monsound != null)
            {
                return monsound[id.ToString()];
            }
            return null;
        }

        public Variant getMonAttSound(string id)
        {
            if (conf.ContainsKey("monAttsound") && conf["monAttsound"] != null)
            {
                return this.conf["monAttsound"][id.ToString()];
            }
            return null;
        }
        //普通攻击音效		
        public String getNormalAttSound(uint id)
        {
            if (conf.ContainsKey("normalAttsound") && conf["normalAttsound"] != null)
            {
                if (!(this.conf["normalAttsound"].ContainsKey(id.ToString())))
                    return "";
                return this.conf["normalAttsound"][id.ToString()]["sid"]._str;
            }
            return "";
        }
        //暴击音效		
        public String getOutAttSound(uint id)
        {
            if (conf.ContainsKey("outAttsound") && conf["outAttsound"] != null)
            {
                if (!(this.conf["outAttsound"].ContainsKey(id.ToString())))
                    return "";
                return this.conf["outAttsound"][id.ToString()]["sid"]._str;
            }
            return "";
        }
        //拾取音效		
        public String getPickSound(string id)
        {
            if (conf.ContainsKey("picksound") && conf["picksound"] != null)
            {
                if (!(this.conf["picksound"].ContainsKey(id.ToString())))
                    return "";
                return this.conf["picksound"][id.ToString()]["sid"]._str;
            }
            return "";
        }
        //掉落音效		
        public String getDropSound(string id)
        {
            if (conf.ContainsKey("dropsound") && conf["dropsound"] != null)
            {
                if (!(this.conf["dropsound"].ContainsKey(id.ToString())))
                    return "";
                return this.conf["dropsound"][id.ToString()]["sid"]._str;
            }
            return "";
        }

        //技能音效		
        public String getSkillSound(uint id)
        {
            if (conf.ContainsKey("skillsound") && conf["skillsound"] != null)
            {
                if (!(this.conf["skillsound"].ContainsKey(id.ToString())))
                    return "";
                return this.conf["skillsound"][id.ToString()]["sid"]._str;
            }
            return "";
        }

        //npc音效		
        public String getNpcSound(uint id)
        {
            if (conf.ContainsKey("npcsound") && conf["npcsound"] != null)
            {
                if (!(this.conf["npcsound"].ContainsKey(id.ToString())))
                    return "";
                return this.conf["npcsound"][id.ToString()]["sid"]._str;
            }
            return "";
        }

        //距离NPC半径	
        public float getNpcRadius(uint id)
        {
            if (conf.ContainsKey("npcsound") && conf["npcsound"] != null)
            {
                if (!(this.conf["npcsound"].ContainsKey(id.ToString())))
                    return 0;
                return this.conf["npcsound"][id.ToString()]["radius"]._float;
            }
            return 0;
        }
        //需要传送的任务配置
        public Variant getTranMission(uint id)
        {
            if (null != conf["tranmission"])
            {
                if (!(this.conf["tranmission"].ContainsKey(id.ToString())))
                    return null;
                return this.conf["tranmission"][id.ToString()];
            }
            return null;
        }
        //-------------------------------卡等级任务 -------------------------------------------
        public Variant GetMisLinks()
        {
            return m_conf["mislinks"];
        }
        public Variant GetMisLinksById(uint misid)
        {
            return m_conf["mislinks"][misid];
        }

        public Variant GetOpenEff(String id)
        {
            return m_conf["openeff"][id];
        }
        //------------------------------------- 技能  -------------------------------------------------
        //特效需要旋转的技能
        private Variant _rotateskill = new Variant();
        private Variant _rotateFlySkill = new Variant();
        private Variant _carrSkillAction = new Variant();
        private Variant _carrDefSkill = null;
        public Variant getRotateSkill(uint sid)
        {
            return _rotateskill[sid.ToString()];
        }
        public Boolean IsRotateFlySkill(uint sid)
        {
            for (int i = 0; i < _rotateFlySkill.Length; ++i)
            {
                if (_rotateFlySkill[i] == sid) return true;
            }
            return false;
        }

        public Variant get_carrSkillAction(int carr)
        {
            return _carrSkillAction[carr.ToString()];
        }
        public Variant GetManualSkill()
        {
            Variant arr = null;
            if (conf.ContainsKey("skill"))
            {
                arr = new Variant();
                Variant obj = conf["skill"][0]["manualskill"];
                string idsStr = obj[0]["ids"]._str;
                Variant ids = GameTools.split(idsStr, ",");
                for (int i = 0; i < ids.Length; ++i)
                {
                    arr._arr.Add(ids[i]._uint);
                }
                return arr;
            }
            return arr;
        }
        public Variant GetCarrDefSkill(int carr)
        {
            if (_carrDefSkill == null)
            {
                _carrDefSkill = new Variant();
                if (conf.ContainsKey("skill") && conf["skill"][0].ContainsKey("defskill"))
                {
                    foreach(Variant def in conf["skill"][0]["defskill"]._arr)
                    {
                        _carrDefSkill[def["carr"]._str] = GameTools.split(def["ids"]._str, ",", GameConstantDef.SPLIT_TYPE_INT);
                    }
                }
            }
            return _carrDefSkill.ContainsKey(carr.ToString())?_carrDefSkill[carr.ToString()]:null;
        }
        public bool IsDefSkill(int carr, uint sid)
        {//是否是默认职业技能
            Variant defSkills = GetCarrDefSkill(carr);
            if (defSkills != null)
            {
                for (int i = 0; i < defSkills.Count; i++)
                {
                    if (defSkills[i]._uint == sid)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Variant _lotterys;
        public Variant GetlotterysByIdx(int id)
        {
            if (_lotterys != null)
            {
                return _lotterys[id];
            }

            if (conf.ContainsKey("lottery"))
            {
                _lotterys = new Variant();
                Variant lotArr = conf["lottery"]["0"]["ltr"];
                foreach (Variant itemObj in lotArr._arr)
                {
                    _lotterys._arr.Add(itemObj);
                }
                return _lotterys[id];
            }
            return null;
        }

        public Variant getLotteryUseItems()
        {
            if (conf.ContainsKey("lottery"))
            {
                if (conf["lottery"]["0"].ContainsKey("showTip"))
                {
                    Variant itms = new Variant();
                    foreach (Variant showObj in conf["lottery"]["0"]["showTip"]._arr)
                    {
                        itms[showObj["id"]] = showObj["tpid"];
                    }
                    return itms;
                }
            }
            return null;
        }

        //----------------------爵位颜色---------------------------
        private Variant _nobColor;
        public Variant GetNobilityColor(int id)
        {
            if (_nobColor == null)
            {
                _nobColor = new Variant();
                if (conf!=null && conf["nobilityColor"]!=null)
                {
                    Variant tmp = conf["nobilityColor"];
                    foreach (Variant obj in tmp._arr)
                    {
                        getNobilityColor(obj);
                    }
                }
            }
            return _nobColor[id.ToString()];
        }
        private void getNobilityColor(Variant data)
        {
            if (data != null)
            {
                string lvls = data["lvl"]._str;
                if (lvls == "")
                    return;
                Variant arr = GameTools.split(lvls, ",");
                int l = arr.Length;
                int tmp;
                for (int i = 0; i < l; i++)
                {
                    tmp = arr[i];
                    if (!_nobColor.ContainsKey(tmp.ToString()) || _nobColor[tmp.ToString() == null])
                    {
                        _nobColor[tmp.ToString()] = new Variant();
                    }
                    _nobColor[tmp.ToString()]["color"] = data["color"];
                    if (data.ContainsKey("star"))
                    _nobColor[tmp.ToString()]["star"] = data["star"]._bool;
                    else
                        _nobColor[tmp.ToString()]["star"] = false;
                }
            }
        }
        //地图大小
        public Variant getMapsize()
        {
            Variant mapSizeConf = conf["mapsize"]["0"];
            int width = mapSizeConf["size"][0]["width"]._int;
            int height = mapSizeConf["size"][0]["height"]._int;
            Variant mapSize = new Variant();
            mapSize["width"] = width;
            mapSize["height"] = height;
            return mapSize;
        }

        private Variant _mapsizeinfo;
        public Variant getMapsizeinfo(uint mapId)
        {
            if (_mapsizeinfo != null)
            {
                return _mapsizeinfo[mapId.ToString()];
            }
            _mapsizeinfo = new Variant();
            Variant mapsizeArr = conf["mapsizeinfo"]["0"]["map"];
            foreach (Variant mpdata in mapsizeArr._arr)
            {
                int id = mpdata["id"]._int;
                _mapsizeinfo[id.ToString()] = mpdata;
            }

            return _mapsizeinfo[mapId.ToString()];
        }


        //------------------------爵位颜色end--------------------------------------
        //-------------------------称号-------------------------------------------------
        private Variant _achiveData;
        public Variant getAchiveData(int id)
        {
            if (_achiveData == null)
            {
                _achiveData = new Variant();
                if (conf != null && conf.ContainsKey("achiveInfo") && conf["achiveInfo"] != null)
                {
                    Variant tmp = conf["achiveInfo"];
                    foreach (Variant obj in tmp._arr)
                    {
                        _achiveData[obj["id"]] = obj;
                    }
                }
            }
            return _achiveData[id.ToString()];
        }
        ////-------------------------称号end-------------------------------------------------
        //-------------------------获得副本难度 数据-------------------------------------------------
        /**
         *获得副本难度 数据
         */
        private Variant _lvlDiffitem;
        public Variant getLvlDiff(int tpid)
        {
            if (_lvlDiffitem == null)
            {
                setlvlDiff();
            }
            return _lvlDiffitem[tpid.ToString()];
        }
        private void setlvlDiff()
        {
            _lvlDiffitem = new Variant();
            if (conf["level"]["0"].ContainsKey("lvldiff"))
            {
                _lvlDiffitem = GameTools.array2Map(conf["level"]["0"]["lvldiff"], "id");
            }
        }
        //-------------------------不需要显示排行信息的副本-------------------------------------------------
        private Variant _ltpidArr;
        public Boolean NeedShowRank(uint ltpid)
        {
            if (null == _ltpidArr)
            {
                _ltpidArr = new Variant();
                Variant conf = GetCommonConf("lvl_unshow_rank");
                string tempstr = conf._str;
                _ltpidArr = GameTools.split(tempstr, ",");
            }
            //不需要显示排行信息的副本
            foreach (string id in _ltpidArr.Values)
            {

                if (ltpid == int.Parse(id))
                    return false;
            }
            return true;
        }

        /**
         * 需要显示/隐藏一些界面的 副本
         * */
        public Boolean is_lvl_need_hideUI(uint current_lvl)
        {
            Variant conf = GetCommonConf("lvl_need_hideUI");
            string lvl_need_hideUI = conf._str;
            Variant arr = GameTools.split(lvl_need_hideUI, ",");
            foreach (Variant i in arr._arr)
            {
                if (uint.Parse(i._str) == current_lvl)
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * 获得对应副本难度是否显示累积铜币//目前只应用于副本进度显示
         * @param tpid 副本编号
         * @param diff 副本难度
         */
        public Boolean IsShowLvlDiffHeapGold(int tpid, int diff)
        {
            Variant lvl_obj = this.getLvlDiff(tpid);
            if (lvl_obj == null)
                return false;

            Variant diffobj = lvl_obj["diff"][diff.ToString()];
            if (diffobj == null)
                return false;

            if (diffobj["isSGold"]._int == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /**
         *获得副本对应难度下的物品
         * @param tpid 副本编号
         * @param diff 难度编号 
         */
        public Variant getLvlDiffItems(int tpid, int diff)
        {
            Variant lvl_obj = this.getLvlDiff(tpid);
            if (lvl_obj != null)
            {
                Variant diffobj = lvl_obj["diff"][diff];
                if (diffobj != null)
                {
                    return diffobj["items"];
                }
            }
            return null;
        }
        /**
         * 获得对应副本难度是否显示掉落物品配置//目前只应用于副本进度显示
         * @param tpid 副本编号
         * @param diff 副本难度
         */
        public Boolean IsShowLvlDiffItems(int tpid, int diff)
        {
            Variant lvl_obj = this.getLvlDiff(tpid);
            if (lvl_obj == null)
                return false;

            Variant diffobj = lvl_obj["diff"][diff];
            if (diffobj == null)
                return false;

            if (diffobj.ContainsKey("isShowItem") && diffobj["isShowItem"]._int == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Variant GetLvlAwdsConf(int tpid, int diff)
        {
            Variant lvl_obj = this.getLvlDiff(tpid);
            if (lvl_obj == null)
                return null;
            foreach (Variant obj in lvl_obj["diff"]._arr)
            {
                if (obj["id"]._int == diff)
                {
                    return obj;
                }
            }

            return null;
        }
        /**
         * 获得对应副本难度是否显示累积经验//目前只应用于副本进度显示
         * @param tpid 副本编号
         * @param diff 副本难度
         */
        public Boolean IsShowLvlDiffHeapExp(int tpid, int diff)
        {
            Variant lvl_obj = this.getLvlDiff(tpid);
            if (lvl_obj == null)
                return false;

            Variant diffobj = lvl_obj["diff"][diff];
            if (diffobj == null)
                return false;

            if (diffobj["isSExp"]._int == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * 世界boss
         * @id bossid
         */
        protected Variant _worldBossData;
        protected void initWorldBosData(int carr)
        {
            _worldBossData = new Variant();
            Variant wordBossArr = conf["worldboss"]["0"]["boss"];
            foreach (Variant bossData in wordBossArr._arr)
            {
                int bossid = bossData["mid"]._int;
                string items = bossData["desc2"]._str;
                _worldBossData[bossid.ToString()] = bossData;
                _worldBossData[bossid.ToString()]["items"] = getBossDesc2(bossid, carr);//translateStr(items);
            }
        }
        //获得boss职业掉落
        private Variant getBossDesc2(int bossid, int carr)
        {
            Variant arr = new Variant();
            if (conf["worldbossDrop"] != null)
            {
                Variant wordBossArr = conf["worldbossDrop"][0]["boss"];
                foreach (Variant boss in wordBossArr._arr)
                {
                    if (boss["mid"] == bossid)
                    {
                        foreach (Variant obj in boss["item"]._arr)
                        {
                            if (!(obj.ContainsKey("carr")) || obj["carr"]._int == carr)
                            {
                                arr._arr.Add(obj);
                            }
                        }
                        break;
                    }
                }
            }
            return arr;
        }

        private Variant translateStr(string str)
        {
            Variant tmp = GameTools.split(str, ",");
            if (tmp.Length == 0) return null;

            //Variant match;
            Variant items = new Variant();
            //Variant itemData;
            //foreach (string item in tmp.Values)
            //{
            //    itemData = new Variant();
            //    RegExp regExp = /{[_a-zA-Z\.]+[_a-zA-Z0-9\.:]*(\([^\)]*\))?}/g;
            //    match = item.match(regExp);
            //    string tmpStr =match[0];
            //    tmpStr = tmpStr.substr(1,tmpStr.Length-2);//去除{}
            //    Variant attArr = GameTools.split(tmpStr,':');//tpid.xxx:
            //    foreach (string att in attArr.Values)
            //    {
            //        Variant itm = GameTools.split(att,'.');
            //        if(itm.Length==2)
            //        {
            //            itemData[itm[0]] = itm[1];
            //        }
            //    }
            //    items._arr.Add(itemData);
            //}
            return items;
        }
        public Variant getWorldBossConfById(uint bossid, int carr)
        {
            if (_worldBossData == null)
            {
                initWorldBosData(carr);
            }

            return _worldBossData[bossid.ToString()];
        }

        public Variant getWorldBossConfByMapId(uint mpid, int carr)
        {
            Variant bossArr = new Variant();
            if (_worldBossData == null)
            {
                initWorldBosData(carr);
            }

            foreach (Variant bossData in _worldBossData.Values)
            {
                if (bossData.ContainsKey("mapid") && bossData["mapid"]._uint == mpid)
                {
                    bossArr._arr.Add(bossData);
                }
            }

            return bossArr;
        }
        public Variant getWorldBossConfByLtpid(uint mapid, uint ltpid, uint diff, int carr)
        {
            Variant bossArr = new Variant();
            if (_worldBossData == null)
            {
                initWorldBosData(carr);
            }

            foreach (Variant bossData in _worldBossData._arr)
            {
                if (bossData.ContainsKey("mapid") && bossData.ContainsKey("ltpid") && bossData.ContainsKey("diff"))
                {
                    if (bossData["mapid"]._uint == mapid && bossData["ltpid"]._uint == ltpid && bossData["diff"]._uint == diff)
                    {
                        bossArr._arr.Add(bossData);
                    }
                }
            }

            return bossArr;
        }
        //boss
        public Boolean NoteBossInfo(uint ltpid, uint mid, int carr)
        {
            Variant bossArr = new Variant();
            if (_worldBossData == null)
            {
                initWorldBosData(carr);
            }

            foreach (Variant bossData in _worldBossData._arr)
            {
                if (bossData.ContainsKey("ltpid") && bossData.ContainsKey("mid"))
                {
                    if (bossData["ltpid"]._uint == ltpid && bossData["mid"]._uint == mid)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        //-----------------------------攻城战地图的对应建筑编号配置-----------------------------------------
        private Variant build_data;
        public uint GetBuildIndex(uint mapid, uint mid)
        {
            if (build_data == null)
            {
                setBuildData();
            }
            Variant tmp = build_data[mapid.ToString()];
            if (tmp != null)
            {
                foreach (Variant obj in tmp._arr)
                {
                    if (obj["mid"]._uint == mid)
                    {
                        return obj["idx"]._uint;
                    }
                }
            }
            return 0;
        }
        private void setBuildData()
        {
            build_data = new Variant();
            if (conf.ContainsKey("buildIndex"))
            {
                if (conf["buildIndex"][0].ContainsKey("map"))
                {
                    foreach (Variant obj in conf["buildIndex"][0]["map"]._arr)
                    {
                        build_data[obj["mapid"]] = obj["mon"];
                    }
                }
            }
        }
        //--------------------------------功能物品配置-----------------------------------------------------------
        private Variant _itemsFeatures;
        private void readitemsFeatures()
        {
            _itemsFeatures = new Variant();
            if (conf.ContainsKey("features"))
            {
                foreach (Variant obj in conf["features"]._arr)
                {
                    string type = obj["type"]._str;
                    string tpid = obj["tpid"]._str;
                    string mktp = obj["mktp"]._str;
                    Variant tmp = new Variant();
                    Variant items = GameTools.split(tpid, ",");
                    Variant mktps = GameTools.split(mktp, ",");
                    tmp["type"] = type;
                    tmp["items"] = items;
                    tmp["mktps"] = mktps;
                    _itemsFeatures[type] = tmp;
                }
            }
        }
        /**获得功能类型的物品id
         */
        public uint get_itemId_byFeatures(string type)
        {
            if (_itemsFeatures == null)
            {
                readitemsFeatures();
            }
            Variant obj = _itemsFeatures[type];
            if (null == obj)
            {
                return 0;
            }
            Variant items = obj["items"];
            if (items != null && items.Length > 0)
            {
                return items[0];
            }
            return 0;
        }
        public Variant GetItemsByFeatures(string type)
        {
            if (_itemsFeatures == null)
            {
                readitemsFeatures();
            }
            Variant obj = _itemsFeatures[type];
            if (obj != null)
            {
                Variant items = obj["items"];
                if (items != null && items.Length > 0)
                {
                    return items;
                }
            }
            return null;
        }
        public Variant GetScriptItem(int tpid, int diff_lvl)
        {
            Variant diffArr = conf["level"]["0"]["lvldiff"];
            foreach (Variant o in diffArr._arr)
            {
                if (o["id"]._int == tpid)
                {
                    Variant itemArr = o["diff"];
                    foreach (Variant obj in itemArr._arr)
                    {
                        if (obj["id"]._int == diff_lvl)
                        {
                            return obj;
                        }
                    }
                }
            }
            return null;
        }


        /**
         * 获得世界地图配置
         */
        protected Variant _worldmaps;
        public Variant getAllWorldmap()
        {
            if (_worldmaps != null)
            {
                return _worldmaps;
            }

            if (conf.ContainsKey("worldmap"))
            {
                _worldmaps = conf["worldmap"]["0"]["wm"];
            }

            return _worldmaps;
        }

        public Variant getWorldMapConfByMapid(int mpid)
        {
            if (_worldmaps == null)
            {
                getAllWorldmap();
            }

            foreach (Variant data in _worldmaps._arr)
            {
                if (data["mapid"]._int == mpid)
                {
                    return data;
                }
            }
            return null;
        }

        /**
         * 获得世界地图掉落配置
         */
        protected Variant _mapDropItems;
        public Variant getWorldMapDropItems()
        {
            //			mapdropitems
            if (_mapDropItems != null)
            {
                return _mapDropItems;
            }

            if (conf.ContainsKey("worldmap"))
            {
                _mapDropItems = conf["mapdropitems"]["0"]["map"];
            }

            return _mapDropItems;
        }

        public Variant getWorldmapDropItem(int mapid)
        {
            Variant dropItems = new Variant();
            if (_mapDropItems == null)
            {
                getWorldMapDropItems();
            }

            foreach (Variant dropData in _mapDropItems._arr)
            {
                if (dropData["id"]._int == mapid)
                {
                    return dropData;
                }
            }
            return dropItems;
        }
        //获取物品不足提示配置
        public Variant getPromptConf()
        {
            if (this.conf == null)
            {
                return null;
            }
            return this.conf["lackprompt"];
        }

        //---------------------- 需要买buff的副本  start----------------------------------------------
        public Variant lvl_need_buy_buff(int ltpid)
        {
            Variant tempArr = _getBuffArr();
            foreach (Variant lvl in tempArr._arr)
            {
                if (lvl["ids"]._str.IndexOf(ltpid.ToString()) != -1)
                {
                    return lvl;
                }
            }
            return null;
        }
        private Variant _buffArr;
        private Variant _getBuffArr()
        {
            if (null == _buffArr)
            {
                _buffArr = new Variant();
                Variant tempbuff = new Variant();
                Variant a = conf["lvlbuff"][0];
                if (a != null && a["lvl"])
                {
                    tempbuff = a["lvl"];
                }
                foreach (Variant obj in tempbuff._arr)
                {
                    Variant ids = GameTools.split(obj["id"]._str, ",");
                    obj["ids"] = ids;
                    _buffArr._arr.Add(obj);
                }
            }
            return _buffArr;
        }
        //---------------------- 需要买buff的副本  end----------------------------------------------

        //---------------------- 可购买的副本buff start----------------------------------------------
        private Variant _buybuffData;
        public Variant GetBuyBuff(string type)
        {
            if (null == _buybuffData)
            {
                _buybuffData = new Variant();
                Variant buy = conf["buybuff"][0]["buy"];
                if (buy && buy.Length > 0)
                {
                    foreach (string i in buy[0].Keys)
                    {
                        Variant buffs = GameTools.split(buy[0][i]._str, ",");
                        _buybuffData[i] = buffs;
                    }
                }
            }
            if (_buybuffData)
            {
                return _buybuffData[type];
            }
            return null;
        }
        //---------------------- 可购买的副本buff end----------------------------------------------

        //---------------------- 副本打宝 start----------------------------------------------
        private Variant _scriptinfos;
        public Variant GetScriptInfo()
        {
            if (null == _scriptinfos)
            {
                Variant temp = conf["scriptdropitem"][0];
                if (temp)
                {
                    _scriptinfos = temp["lvl"];
                }
            }
            return _scriptinfos;
        }
        //---------------------- 副本打宝  end----------------------------------------------

        //-------------------一键出售的att配置Start---------------------------------
        private Variant _itemFilter = null;
        private void readFastSellAtt(Variant node)
        {
            _itemFilter = new Variant();

            foreach (Variant obj in node._arr)
            {
                Variant tdata = new Variant();
                tdata["attchk"] = readAttSubNode(obj["attchk"]);
                tdata["cfgchk"] = readAttSubNode(obj["cfgchk"]);

                _itemFilter[obj["tp"]] = tdata;
            }
        }
        private Variant readAttSubNode(Variant nodeList)
        {
            Variant attArr = new Variant();
            if(nodeList !=null)
            foreach (Variant attxml in nodeList._arr)
            {
                Variant attData = new Variant();
                string fun = attxml["fun"]._str;
                attData["fun"] = fun;
                attData["name"] = attxml["name"]._str;
                if ("match" == fun || "notmatch" == fun)
                {
                    string vals = attxml["val"]._str;
                    Variant tempArr = GameTools.split(vals, ",");
                    Variant valArr = new Variant();
                    foreach (string str in tempArr._arr)
                    {
                        valArr._arr.Add(int.Parse(str));
                    }
                    attData["val"] = valArr;
                }
                else
                {
                    attData["val"] = attxml["val"]._int;
                }
                attArr._arr.Add(attData);
            }
            return attArr;
        }
        public Variant GetFastSellAtt(uint tp = 0)
        {
            if (_itemFilter == null)
            {
                readFastSellAtt(conf["eqpattchk"][0]["itemFilter"]);
            }
            return _itemFilter[tp.ToString()];
        }
        public Variant GetItemCheck()
        {
            if (_itemFilter == null)
            {
                readFastSellAtt(conf["eqpattchk"][0]["itemFilter"]);
            }
            return _itemFilter;
        }
        //------------------一键出售的att配置End-------------------------------

        //-----------------地图挂机配置 start	-------------------------
        protected Variant _autoGameConf;
        protected void _initAutoGame()
        {
            if (_autoGameConf != null)
            {
                return;
            }
            _autoGameConf = new Variant();
            Variant mapDatas = conf["mapautogame"]["0"]["map"];
            foreach (Variant mapData in mapDatas._arr)
            {
                _autoGameConf[mapData["id"]] = mapData;
            }
        }

        public Variant getAutoGameConfByMapid(int mpid)
        {
            if (_autoGameConf == null)
            {
                _initAutoGame();
            }
            return _autoGameConf[mpid.ToString()];
        }
        public Variant getAutoGameConf()
        {
            if (_autoGameConf == null)
            {
                _initAutoGame();
            }
            return _autoGameConf;
        }
        /**获取一个随机坐标
         */
        private Variant rdArr;
        public String GetRandPos(string id)
        {
            if (_randposConf == null)
            {
                _initRandpos();
            }
            Variant posData = _randposConf[id];
            if (posData)
            {
                Variant posArr = posData["pos"];
                if (posArr.Length > 0)
                {

                    foreach (Variant data in posArr._arr)
                    {
                        if (data.ContainsKey("checks"))
                        {	//剔除有条件限制的随机坐标
                            //							Variant self_info = _.get_self_detail_info();
                            //							if(!game.global.attchk.is_attchk(data.checks, self_info))
                            //								continue;
                        }
                        if (null == rdArr)
                            rdArr = new Variant();
                        rdArr._arr.Add(data);
                    }
                    if (rdArr && rdArr.Length > 0)
                    {
                        Variant pos = new Variant();
                        int length = rdArr.Length;
                        if (1 == length)
                        {
                            pos = rdArr[0];
                        }
                        else
                        {
                            Random num = new Random();
                            int radomnum = num.Next(0, length);
                            pos = rdArr[radomnum];
                        }
                        return pos["mapid"]._int + "_" + pos["x"]._int + "_" + pos["y"]._int;
                    }
                }
            }
            return "";
        }
        //-----------------地图挂机配置 end	-------------------------
        //----------------  挂机配置start	------------------
        protected Variant _randposConf;
        protected void _initRandpos()
        {
            if (_randposConf != null)
            {
                return;
            }
            _randposConf = new Variant();
            Variant randPosData = conf["randpos"]["0"]["auto"];
            foreach (Variant data in randPosData._arr)
            {
                _randposConf[data["id"]] = data;
            }
        }

        public Variant getRandPosConfById(int id)
        {
            if (_randposConf == null)
            {
                _initRandpos();
            }
            return _randposConf[id.ToString()];
        }

        //自动加点配置
        public Variant getAutoPointDataById(int id)
        {
            if (conf != null)
            {
                return conf["autoPoint"][id.ToString()];
            }
            return null;
        }

        //vip加点相关配置
        public Variant getVipAddPointData()
        {
            if (conf != null)
            {
                return conf["vipAddPoint"];
            }
            return null;
        }

        public Variant getVipStateData(int lvl)
        {
            if (conf != null)
            {
                return conf["vipState"][lvl.ToString()];
            }
            return null;
        }
        //        //--------------------------------NPC功能按钮挂接 start-----------------------------------
        //        //npc帮助
        //private Variant _npcHelpData = new Variant();
        //private void readNpcHelp(Variant node)
        //{
        //    int npcid = node[0]["npcid"]._int;
        //    if(_npcHelpData[npcid.ToString()])
        //    {
        //        _npcHelpData[npcid.ToString()]["desc"]._arr.Add(node[0]["desc"]._str);
        //    }
        //    else
        //    {
        //        Variant desc = new Variant();
        //        desc._arr.Add(node[0]["desc"]._str);
        //        Variant v = new Variant();
        //        v["npcid"]._int = npcid;
        //        v["name"] = node[0]["name"]._str;
        //        v["desc"] = desc;
        //        _npcHelpData[npcid.ToString()] = v;
        //    }
        //}
        //public Variant get_npcHelpData(int npcid)
        //{
        //    return _npcHelpData[npcid];
        //}
        ////npc市场
        //private Variant _npcMarketData = new Variant();
        //private void readNpcMarket(Variant node)
        //{
        //    string temp = node[0].npcid;
        //    Array Arr = temp.split(",");
        //    foreach (string str in Arr)
        //    {
        //        _npcMarketData[str] = str;
        //    }
        //}
        //        public Variant get_npcMarketData(int npcid)
        //        {
        //            return _npcMarketData[npcid];
        //        }
        //        //npc藏宝阁
        //        private Variant _npcRanShopData = new Object();
        //        private void readRanShop(Variant node)
        //        {
        //            string temp = node[0].npcid;
        //            Array Arr = temp.split(",");
        //            foreach (string str in Arr)
        //            {
        //                _npcRanShopData[str] = str;
        //            }
        //        }
        //        public Variant get_npcRanShopData(int npcid)
        //        {		
        //            return _npcRanShopData[npcid];
        //        }
        //        //npc福利
        //        private Variant _npcGiftData = new Object();
        //        public void readGift(Variant node)
        //        {
        //            string temp = node[0].npcid;
        //            Array Arr = temp.split(",");
        //            foreach (string str in Arr)
        //            {
        //                _npcGiftData[str] = str;
        //            }
        //        }
        //        public Variant get_npcGiftData(int npcid)
        //        {	
        //            return _npcGiftData[npcid];
        //        }
        //        //npc转职
        //        private Variant _npcTransferData = new Object();
        //        public void readTransfer(Variant node)
        //        {
        //            string temp = node[0].npcid;
        //            Array Arr = temp.split(",");
        //            foreach (string str in Arr)
        //            {
        //                _npcTransferData[str] = str;
        //            }
        //        }
        //        public Variant get_npcTransferData(int npcid)
        //        {
        //            return _npcTransferData[npcid];
        //        }
        //        //npc结缘
        //        private Variant _npcMarryData = new Object();
        //        public void readMarry(Variant node)
        //        {
        //            string temp = node[0].npcid;
        //            Array Arr = temp.split(",");
        //            foreach (string str in Arr)
        //            {
        //                _npcMarryData[str] = str;
        //            }
        //        }
        //        public Variant get_npcMarryData(int npcid)
        //        {
        //            return _npcMarryData[npcid];
        //        }
        //        //膜拜PK之王
        //        private Variant _npcPkKingData = new Object();
        //        public void readPkKing(Variant node)
        //        {
        //            string temp = String(node[0].npcid);
        //            Array Arr = temp.split(",");
        //            foreach (string str in Arr)
        //            {
        //                _npcPkKingData[str] = str;
        //            }
        //        }
        //        public Variant get_npcPkKingData(int npcid)
        //        {
        //            return _npcPkKingData[npcid];
        //        }
        //        //副本npc
        //        private Variant _npcLevelData = new Object();
        //        public void readLevel(Variant node)
        //        {
        //            foreach (Variant nodeObj in node)
        //            {
        //                _npcLevelData[nodeObj.tpid] = nodeObj;
        //                if(nodeObj.hasOwnProperty("linknpc"))
        //                {
        //                    Array linkArr = String(nodeObj.linknpc).split(",");
        //                    if(linkArr)
        //                    {
        //                        for(int i = 0; i < linkArr.length; i++)
        //                        {
        //                            linkArr[i] = uint(linkArr[i]);
        //                        }
        //                    }
        //                    _npcLevelData[nodeObj.tpid].linknpc = linkArr;
        //                }
        //            }
        //        }
        //        public Variant GetNpcLevelData(uint ltpid, int npcid)
        //        {
        //            if(_npcLevelData[ltpid] && _npcLevelData[ltpid].level)
        //            {
        //                foreach (Variant npcObj in _npcLevelData[ltpid].level)
        //                {
        //                    if(npcObj.npcid == npcid)
        //                    {
        //                        return npcObj;
        //                    }
        //                }
        //            }
        //            return null;
        //        }
        //        public Variant GetNpcLevelConf(uint ltpid)
        //        {
        //            return _npcLevelData[ltpid];
        //        }

        //摆摊
        /**
         * 获取摆摊的npc
         * */
        public Variant GetNpcFunData(int npcid)
        {
            return conf["npcfun"][npcid.ToString()];
        }
        /// <summary>
        /// 获得npc3d显示信息
        /// </summary>
        /// <param name="npcid"></param>
        /// <returns></returns>
        public Variant GetNpc3Dshow(int npcid)
        {
            if (conf.ContainsKey("npcshow") && conf["npcshow"].ContainsKey(npcid.ToString()))
                return conf["npcshow"][npcid.ToString()];
            return null;
        }
        /// <summary>
        /// 获得爵位图片路径
        /// </summary>
        /// <returns></returns>
        public Variant Getnobfile()
        {
            if (conf.ContainsKey("nobfile"))
                return conf["nobfile"];
            return null;
        }

        //--------------------------------NPC功能按钮挂接 end-----------------------------------
        //广播
        private Variant _buffDataArr;
        private void readBroadCitywarBuybuff(Variant data)
        {
            _buffDataArr = new Variant();
            Variant buffArr = data[0]["buff"];
            foreach (Variant obj in buffArr._arr)
            {
                String desc = LanguagePack.getLanguageText("chat", obj["desc"]._str);
                obj["desc"] = desc;
                _buffDataArr._arr.Add(obj);
            }
        }
        /**
         * 获取攻城战副本 鼓舞BUFF的广播描述
         */
        public String get_buff_desc_bytpid(string id, string lvl)
        {
            if (_buffDataArr == null)
            {
                readBroadCitywarBuybuff(conf["broad_citywar_buybuff"]);
            }

            foreach (Variant obj in _buffDataArr._arr)
            {
                if ((obj["id"]._str == id) && (obj["lvl"]._str == lvl))
                {
                    return obj["desc"];
                }
            }
            return "";
        }

        /**
         * 获取boss死亡的广播
         */
        private Variant _bossDieEff;
        private void readBossDieEffFun(Variant data)
        {
            _bossDieEff = new Variant();
            Variant bossDieDataArr = data[0]["bcase"];
            foreach (Variant bData in bossDieDataArr._arr)
            {
                String msg = LanguagePack.getLanguageText("chat", bData["msg"]._str);
                bData["msg"] = msg;
                _bossDieEff[bData["id"].ToString()] = bData;
            }
        }

        public Variant GetBossDieEffById(int id)
        {
            if (_bossDieEff == null)
            {
                readBossDieEffFun(conf["boss_die_eff"]);
            }

            return _bossDieEff[id.ToString()];
        }

        /**
         * 获取接受任务的广播描述
         */
        private Variant _broad_mis_awd;
        private void readBroadRMisAwd(Variant data)
        {
            Variant rmisBroadData = data[0]["rmis"];

            foreach (Variant bData in rmisBroadData._arr)
            {
                string misid = bData["misid"]._str;
                string desc = bData["desc"]._str;
                _broad_mis_awd[misid]["misid"] = misid;
                _broad_mis_awd[misid]["desc"] = desc;
            }
        }


        public String get_acpmis_desc_bymisid(uint misid)
        {
            if (_broad_mis_awd == null)
            {
                _broad_mis_awd = new Variant();
                readBroadRMisAwd(conf["broad_rmis_desc"]);
            }

            foreach (Variant obj in _broad_mis_awd._arr)
            {
                if (obj["misid"]._uint == misid)
                {
                    return obj["desc"];
                }
            }
            return "";
        }

        /**
         * 世界boss刷新死亡播报
         */
        //-------------------------------BOSS死亡复活广播--------------------------------------------------------
        private Variant _broad_boss;
        private void readBroadBoss(Variant data)
        {
            _broad_boss = new Variant();

            Variant bossDataArr = data[0]["boss"];

            foreach (Variant bData in bossDataArr._arr)
            {
                String type = bData["type"];
                Variant arr;
                Variant obj = new Variant();
                obj["desc"] = bData["desc"];
                obj["type"] = bData["type"];
                if (bData.ContainsKey("mid"))
                {
                    arr = GameTools.split(bData["mid"]._str, ",");
                    obj["mids"] = arr;
                }
                if (bData.ContainsKey("mapid"))
                {
                    arr = GameTools.split(bData["mapid"]._str, ",");
                    obj["mapids"] = arr;
                }
                if (_broad_boss[type] == null)
                {
                    _broad_boss[type] = new Variant();
                }
                _broad_boss[type]._arr.Add(obj);
            }

        }


        private String get_boss_desc_bytype(string type, uint mapid, uint mid)
        {
            if (mapid == 0 && mid == 0)
            {
                return "";
            }
            Variant data = _broad_boss[type];
            if (data)
            {
                foreach (Variant obj in data._arr)
                {
                    if (mapid != 0 && mid != 0)
                    {//mapid  monid都存在
                        if (obj.ContainsKey("mids") && obj.ContainsKey("mapids"))
                        {
                            if (obj["mids"]._str.IndexOf(mid.ToString()) != -1 && obj["mapids"]._str.IndexOf(mapid.ToString()) != -1)
                            {
                                return obj["desc"];
                            }
                        }
                    }
                    else if (mapid != 0)
                    {//只有mapid
                        if (obj.ContainsKey("mapids") && !obj.ContainsKey("mids"))
                        {
                            if (obj["mapids"]._str.IndexOf(mapid.ToString()) != -1)
                            {
                                return obj["desc"];
                            }
                        }
                    }
                    else
                    {
                        //只有mid
                        if (obj.ContainsKey("mids") && !obj.ContainsKey("mapids"))
                        {
                            if (obj["mids"]._str.IndexOf(mid.ToString()) != -1)
                            {
                                return obj["desc"];
                            }
                        }
                    }
                }
            }
            return "";
        }
        /**
         * 获取BOSS复活的广播描述
         */
        public String get_bossrevive_desc_bytpid(uint mid = 0, uint mapid = 0)
        {
            if (_broad_boss == null)
            {
                readBroadBoss(conf["broad_boss"]);
            }

            return get_boss_desc_bytype("born", mapid, mid);
        }
        //-------------------------------BOSS死亡广播--------------------------------------------------------

        /**
         * 获取BOSS死亡的广播描述
         */
        public String get_bossdie_desc_bytpid(uint mid = 0, uint mapid = 0)
        {
            if (_broad_boss == null)
            {
                readBroadBoss(conf["broad_boss"]);
            }

            return get_boss_desc_bytype("die", mapid, mid);
        }

        //-------------------------------购买商城物品广播--------------------------------------------------------
        private Variant _broad_mall_items;
        private void readBroadMalldItems(Variant data)
        {
            _broad_mall_items = new Variant();
            Variant malldItemsArr = data[0]["itm"];
            foreach (Variant obj in malldItemsArr._arr)
            {
                String desc = LanguagePack.getLanguageText("chat", obj["desc"]._str);
                obj["desc"] = desc;
                _broad_mall_items[obj["tpid"].ToString()] = obj;
            }
        }

        /**
         * 获取商城物品的广播描述
         */
        public String get_desc_bytpid(string tpid)
        {
            if (_broad_mall_items == null)
            {
                readBroadMalldItems(conf["broad_mall_items"]);
            }
            foreach (Variant obj in _broad_mall_items._arr)
            {
                if (obj["tpid"]._str == tpid)
                {
                    return obj["desc"];
                }
            }
            return "";
        }

        //-------------------------------指定物品广播--------------------------------------------------------
        private Variant _broad_items;
        private void readBroaddItems(Variant data)
        {
            _broad_items = new Variant();
            Variant itemsArr = data[0]["itm"];

            foreach (Variant itmData in itemsArr._arr)
            {
                _broad_items[itmData["tpid"].ToString()] = itmData;
            }

            //string tpid = String(node.attribute("tpid"));
            //string desc = String(node.attribute("desc"));
            //Variant obj = new Object();
            //obj.tpid = tpid;
            //obj.desc = desc;
            //_broad_items[tpid] = obj;
        }
        /**
         * 判断是否是需要广播的物品
         */
        public Boolean is_need_broad(string tpid)
        {
            if (_broad_items == null)
            {
                readBroaddItems(conf["broad_items"]);
            }
            return (_broad_items[tpid] != null);
        }
        /**
         * 获取需要广播的物品广播描述
         */
        public String get_broadstr_bytpid(string tpid)
        {
            foreach (Variant obj in _broad_items._arr)
            {
                if (obj["tpid"]._str == tpid)
                {
                    String desc = LanguagePack.getLanguageText("chat", obj["desc"]._str);
                    return desc;
                }
            }
            return "";
        }

        //------------------------膜拜pk之王----------------------------------
        private Variant _pkKingShowInfo = new Variant();
        private void readPkKingInfo(Variant data)
        {

            //			int carr = int(node.attribute("carr"));
            //			string cid = String(node.attribute("cid"));
            //			string img = String(node.attribute("img"));
            //			_pkKingShowInfo[carr] = {carr carr,cid cid,img img};
        }
        public Variant GetPkKingInfo(int carr)
        {
            return _pkKingShowInfo[carr.ToString()];
        }

        private Variant _pkKingNPCInfo = null;

        private void readPkKingNPC(Variant data)
        {
            //			_pkKingNPCInfo = {
            //				int mapid(node.attribute("mapid")),
            //				int npcid(node.attribute("npcid")),
            //				int posx(node.attribute("posx")),
            //				int posy(node.attribute("posy")),
            //				string icon(node.attribute("icon"))
            //			};
        }
        public Variant GetPkKingNPC()
        {
            return _pkKingNPCInfo;
        }

        //--------------------------------快捷键配置start---------------------------------------
        private Variant _hotKeyData = new Variant();

        public Variant getInputKey(uint code)
        {
            return _hotKeyData[code.ToString()];
        }

        //--------------------------------快捷键配置end-----------------------------------------

        //-------------------------------线路繁忙度显示--------------------------------------------------------
        private Variant _lines_arr;
        /**
         * 根据当前线路人数百分比，获取应该显示的繁忙度
         */
        public uint get_percent_byscale(uint scale)
        {
            Variant lines_show = conf["lines_show"];
            if (null == _lines_arr)
            {
                if (lines_show != null && lines_show[0] != null)
                {
                    _lines_arr = lines_show[0]["line"];
                }
            }
            foreach (Variant temp in _lines_arr._arr)
            {
                if (scale <= temp["max"]._uint && scale >= temp["min"]._uint)
                {
                    return temp["show"]._uint;
                }
            }
            return 100;
        }

        //-------------------------------温馨提示  start--------------------------------------------------------
        //private Variant _warmhintData;
        public Variant get_warmhintData(int lvlid)
        {
            if (conf.ContainsKey("warm_hint"))
            {
                return conf["warm_hint"][lvlid.ToString()];
            }

            return null;
        }
        //-------------------------------温馨提示 end--------------------------------------------------------



        public Variant objGetOlAwardConfByAid(int iAid)
        {
            Variant arr = this.conf["ol_award"];
            for (uint i = 0; i != arr.Length; i++)
            {
                Variant obj = arr[i];
                if (obj != null)
                {
                    uint aid = obj["aid"]._uint;
                    if (aid == iAid)
                    {
                        return obj;
                    }
                }
            }

            return null;
        }

        //-------------------------------回归礼包start---------------------------------------
        public Variant GetBackGift()
        {
            return conf["backGift"][0]["gift"];
        }
        public Variant getBackItemsByTpid(uint tpid)
        {
            Variant backarr = conf["backGift"][0]["gift"];
            foreach (Variant obj in backarr._arr)
            {
                if (obj["itmid"]._uint == tpid)
                    return obj;
            }
            return null;
        }
        //-------------------------------回归礼包end-----------------------------------------

        //-------------------------------任务icon配置Start---------------------------------------------------
        public Variant GetMissionIcon()
        {
            return conf["missionIcon"];
        }
        //-------------------------------任务icon配置End---------------------------------------------------
        //-------------------------------主线任务奖励配置star-----------------------------------------------
        private Variant _mlineTipInfo = new Variant();
        public Variant GetMlineAwardInfo(int chapter)
        {
            if (chapter < 0)
            {
                return null;
            }
            if (!_mlineTipInfo[chapter.ToString()])
            {
                _mlineTipInfo[chapter.ToString()] = conf["mlineTip"][chapter.ToString()];
                string misid = conf["mlineTip"][chapter.ToString()]["misid"];
                Variant arr = GameTools.split(misid, ",");
                _mlineTipInfo[chapter.ToString()]["misid"] = arr;
            }
            return _mlineTipInfo[chapter.ToString()];
        }
        public Variant GetMlineAwardInfoByMisid(int misid, int lvl, uint lastid)
        {
            Variant mlineTip = conf["mlineTip"];
            int chapter = -1;
            for (int i = 0; i < mlineTip.Length; ++i)
            {
                if (mlineTip[i])
                {
                    int cmisid = misid;
                    int clvl = lvl;
                    if (mlineTip[i]["isclient"])//客户端显示  
                    {
                        clvl += 1;
                        cmisid += 1;
                    }
                    if ((cmisid <= mlineTip[i]["awdmis"]._int && clvl <= mlineTip[i]["level"]._int) && lastid < mlineTip[i]["awdmis"]._uint)
                    {
                        chapter = mlineTip[i]["chapter"]._int;
                        break;
                    }
                }
            }
            return GetMlineAwardInfo(chapter);
        }
        public Variant GetMlineShow3D(int tpid)
        {
            return conf["mlineshow3D"][tpid.ToString()];
        }
        /**
         * 是否是最后一个章节奖励任务
         * */
        public Boolean IsLastMis(int misid)
        {
            bool flag = false;
            int len = conf["mlineTip"].Length;
            flag = (misid >= conf["mlineTip"][len - 1]["awdmis"]._int);
            return flag;
        }
        //-------------------------------主线任务奖励配置  end-----------------------------------------------
        //-------------------------------节日活动------------------------
        public Variant GetFestivalData(uint ractid)
        {
            Variant data = conf["actinfo"]["festival"]["act"];
            foreach (Variant obj in data._arr)
            {
                if (obj["id"]._uint == ractid)
                {
                    return obj;
                }
            }
            return null;
        }

        //-------------------------------  普通 攻击 相关 -------------------------------------------------
        //private Variant _monAtk;
        // private Variant _plyAtk;
        public Variant GetMonAtk(uint monid)
        {

            return _monAtk != null ? _monAtk[monid.ToString()] : null;
        }
        public Variant GetPlyAtk(uint carrid)
        {
            return _plyAtk != null ? _plyAtk[carrid.ToString()] : null;
        }
        //--------------------------------------------------------------------------------------------------

        //-------------------------------副本Start-----------------------------------------------
        private Variant _lvlnodedata = null;
        private void _initLvlNodeData()
        {
            Variant level = conf["level"][0]["level"];
            foreach (Variant lvl in level._arr)
            {
                //副本编号
                int tpid = lvl["tpid"]._int;

                if (_lvlnodedata[tpid.ToString()] == null)
                {
                    _lvlnodedata[tpid.ToString()] = new Variant();
                }

                Variant diff = lvl["diff"];
                foreach (Variant diffObj in diff._arr)
                {
                    int diffid = diffObj["diffid"]._int;
                    Variant node = diffObj["node"];
                    if (_lvlnodedata[tpid.ToString()][diffid.ToString()] == null)
                    {
                        _lvlnodedata[tpid.ToString()][diffid.ToString()] = new Variant();
                    }

                    for (int i = 0; i < node.Length; i++)
                    {
                        int nx = node[i]["x"]._int;
                        int ny = node[i]["y"]._int;
                        Variant v = new Variant();
                        v["x"] = nx;
                        v["y"] = ny;
                        _lvlnodedata[tpid.ToString()][diffid.ToString()]._arr.Add(v);
                    }
                }
            }
        }
        /**
         *获得副本挂机寻路行走坐标点 
         * @param tpid 副本编号
         * @param diff 副本难度
         * @return
         */
        public Variant get_lvl_node(int tpid, int diff)
        {
            if (_lvlnodedata == null)
            {
                _lvlnodedata = new Variant();
                _initLvlNodeData();
            }
            Variant l_data = _lvlnodedata[tpid.ToString()];
            if (l_data == null)
                return null;

            Variant d_data = _lvlnodedata[tpid.ToString()][diff.ToString()];
            if (d_data == null)
            {
                //如果为空向下取最大的
                int diff_lvl = 0;
                int diffid = 0;
                foreach (string i in l_data.Values)
                {
                    diffid = int.Parse(i);
                    if (diffid < diff)
                    {
                        if (diff_lvl < diffid)
                            diff_lvl = diffid;
                    }
                }

                d_data = _lvlnodedata[tpid.ToString()][diff_lvl.ToString()];
            }

            return d_data;
        }

        //-------------------------------副本End-----------------------------------------------

        //------------------------------- pickcolor -----------------------------------------------
        protected Variant _chaFilters = new Variant();
        public Variant GetChaFilterConf(string tp)
        {
            return _chaFilters[tp];
        }
        //--------------------------------星魂(静脉)坐标start--------------------
        private Variant _meriPosArr;
        private Variant meriPosArr()
        {
            if (_meriPosArr == null)
            {
                _meriPosArr = new Variant();
                Variant data = conf["meri"];
                foreach (Variant obj in data._arr)
                {
                    _meriPosArr[obj["id"].ToString()] = obj;
                }
            }
            return _meriPosArr;
        }
        public Variant GetMeriPos(int idx)
        {
            Variant data = meriPosArr();
            if (data && data.ContainsKey(idx.ToString()))
            {
                return data[idx.ToString()]["acup"];
            }
            return null;
        }
        //--------------------------------星魂(静脉)坐标end--------------------

        //--------------------------- 任务追踪面板显示副本Start-------------
        public Variant GetMisTrackShowLevel()
        {
            return conf["misTrackShowLevel"];
        }
        //--------------------------- 任务追踪面板显示副本End---------------

        //----------------------------自动按钮隐藏配置Start-------------------------------------------
        /**
         *获得隐藏自动按钮任务id 
         * @return 
         * 
         */
        public int GetHideAutoGameBtnMisid()
        {
            return conf["hideAGBtnConf"][0]["finMline"][0]["id"]._int;
        }
        //---------------------------自动按钮隐藏配置End--------------------------------------------
        //----------------------------酒馆日常任务分类   Start-------------------------------------------	
        private Variant _typeRmis;
        private Variant _RmisType;
        public Variant GetDalyrepTypeRmis(uint type)
        {
            if (null == _typeRmis)
            {
                _typeRmis = new Variant();
                Variant dalyrepMis = conf["rmisDalyrep"];
                if (dalyrepMis != null && dalyrepMis[0] != null)
                {
                    foreach (Variant obj in dalyrepMis[0]["type"]._arr)
                    {
                        if (obj != null)
                        {
                            _typeRmis[obj["id"].ToString()] = GameTools.split(obj["rmis"]._str, ",");
                        }
                    }
                }
            }
            return (_typeRmis[type.ToString()]);
        }
        public uint GetDalyrepRmisType(uint rmis)
        {
            if (null == _RmisType)
            {
                _RmisType = new Variant();
                Variant dalyrepMis = conf["rmisDalyrep"];
                if (dalyrepMis != null && dalyrepMis[0] != null)
                {
                    foreach (Variant obj in dalyrepMis[0]["type"]._arr)
                    {
                        if (obj)
                        {
                            Variant arr = GameTools.split(obj["rmis"], ",");
                            foreach (string i in arr.Values)
                            {
                                _RmisType[uint.Parse(i)] = obj["id"];
                            }
                        }
                    }
                }
            }
            return _RmisType[rmis.ToString()];
        }
        //---------------------------酒馆日常任务分类      End--------------------------------------------
        //---------------------------战盟任务      start--------------------------------------------
        private Variant _clanRmis;
        public Variant GetClanLvlRmis(int id = 1)
        {
            if (null == _clanRmis)
            {
                _clanRmis = new Variant();
                Variant clanMis = conf["rmisClan"];
                if (clanMis != null)
                {
                    foreach (Variant obj in clanMis._arr)
                    {
                        if (obj != null)
                        {
                            _clanRmis[obj["id"].ToString()] = GameTools.split(obj["rmis"]._str, ",");
                        }
                    }
                }
            }
            return _clanRmis[id.ToString()];
        }
        //---------------------------战盟任务      end--------------------------------------------
        //--------------------------- 替身 装备 ------------------------------------------------------------
        // private Variant _avaCha = new Variant();
        public Variant GetAvatarApp(int avaid)
        {
            return _avaCha[avaid.ToString()];
        }

        //----------------------------状态消失提示Start-------------------------------------------
        /**
         * 是否需要状态消失提示
         * @param id状态id
         * @return 
         * 
         */
        public Boolean IsNeedStateDisTip(int id)
        {
            if (conf["stateDisTip"] == null)
            {
                return false;
            }

            return conf["stateDisTip"][id.ToString()] != null;
        }
        //不需要提示的的配置
        public Boolean IsNeedStateAddTip(int id)
        {
            if (conf["stateAddTip"] == null)
            {
                return false;
            }

            return conf["stateAddTip"][id.ToString()] != null;
        }
        //---------------------------状态消失提示End--------------------------------------------

        //---------------------------切换地图时显示的地图名字start----------------------------------
        public String get_mapname_icon(uint mapid)
        {
            if (conf["mapNameImg"].ContainsKey(mapid.ToString()))
            {
                return conf["mapNameImg"][mapid.ToString()]["icon"];
            }
            return "";
        }
        //---------------------------切换地图时显示的地图名字start----------------------------------
        //-------------------------------新手礼包提示Start-------------------------------------------------------
        private Variant _newcomer_gift = null;//新手礼包数据
        //获取自身职业新手礼包数据数组
        public Variant get_carr_gift_arr(int carr, int carrlvl)
        {
            if (_newcomer_gift == null)
            {
                _newcomer_gift = new Variant();

                Variant arr = conf["newcomer_gift"][0]["gift"];
                for (int j = 0; j < arr.Length; j++)
                {
                    Variant gift = arr[j];
                    string id = gift["id"]._str;
                    string level = gift["level"]._str;
                    string dropid = gift["dropid"]._str;
                    string tempcarr = gift["carr"]._str;
                    string tempcarrlvl = gift["carrlvl"]._str;
                    Variant items = GameTools.split(dropid, ",");
                    Variant obj = new Variant();
                    obj["id"] = id;
                    obj["level"] = level;
                    obj["dropid"] = dropid;
                    obj["carrlvl"] = tempcarrlvl;
                    obj["carr"] = tempcarr;
                    obj["items"] = items;
                    _newcomer_gift._arr.Add(obj);
                }
            }
            Variant temp = new Variant();
            for (uint i = 0; i < _newcomer_gift.Length; i++)
            {
                Variant giftobj = _newcomer_gift[i];
                if (carr == giftobj["carr"]._uint && carrlvl == giftobj["carrlvl"]._uint)
                {
                    temp._arr.Add(giftobj);
                }
            }
            return temp;
        }

        //-------------------------------新手礼包提示End-------------------------------------------------------	}

        //-------------------------------技能列表start-----------------------------------------------------------
        public Variant getCurProSkillList(int carr)
        {
            if (conf["skillList"].ContainsKey(carr.ToString()))
            {
                string skillListStr = conf["skillList"][carr.ToString()]["skid"]._str;
                Variant skillIdArr = GameTools.split(skillListStr, ",");
                Variant skidArr = new Variant();
                for (int i = 0; i < skillIdArr.Length; ++i)
                {
                    Variant v = new Variant();
                    v["skid"] = skillIdArr[i];
                    v["mark"] = "general";
                    skidArr[i] = v;//gener用于标记该技能客户端中有存
                }
                return skidArr;
            }
            return null;
        }
        //-------------------------------技能列表end-----------------------------------------------------------

        //--------------------------推荐挂机文本链接  start-------------------------------------------------------------------------
        private Variant _linkInfos;
        /**
         * 获取任务的挂机地点的数组
         * @param misid 任务id
         */
        public Variant GetAutogameLinks()
        {
            if (null == _linkInfos)
            {
                _linkInfos = new Variant();
                if (conf.ContainsKey("links") && conf["links"][0])
                {
                    _linkInfos = conf["links"][0]["link"];
                }
            }
            return _linkInfos;
        }
        //--------------------------多个文本链接  end-------------------------------------------------------------------------

        //--------------------------npc特殊属性配置  start-------------------------------------------------------------------------
        private Variant _npcPropObj;
        /**
         * 获取任务的挂机地点的数组
         * @param misid 任务id
         */
        public Variant GetNpcProp(int npcid)
        {
            if (null == _npcPropObj)
            {
                _npcPropObj = new Variant();
                if (conf.ContainsKey("npcprop") && conf["npcprop"][0])
                {
                    foreach (Variant obj in conf["npcprop"][0]["npc"]._arr)
                    {
                        _npcPropObj[obj["id"].ToString()] = obj;
                    }
                }
            }
            return _npcPropObj[npcid.ToString()];
        }
        //--------------------------npc特殊属性配置  end-------------------------------------------------------------------------

        //--------------------------客户端任务目标    start-------------------------------------------------------------------------------------------------
        protected Variant _cgoaldata;
        public Variant GetCgoals(int goalid)
        {
            if (null == _cgoaldata)
            {
                _cgoaldata = new Variant();
                if (conf.ContainsKey("clientgoal") && conf["clientgoal"][0])
                {
                    foreach (Variant obj in conf["clientgoal"][0]["goal"]._arr)
                    {
                        Variant misarr = obj["mis"];
                        foreach (Variant tmp in misarr._arr)
                        {
                            string link = LanguagePack.getLanguageText("clientgoal", tmp["link"]._str);
                            tmp["link"] = link;
                        }
                        _cgoaldata[obj["id"].ToString()] = misarr;
                    }
                }
            }
            return _cgoaldata[goalid.ToString()];
        }

        //--------------------------客户端任务目标    end-------------------------------------------------------------------------------------------------

        //--------------------------天下第一   start-------------------------------------------------------------------------------------------------
        protected Variant _bestoneArr;
        public Variant GetBestoneInfo()
        {
            if (null == _bestoneArr)
            {
                _bestoneArr = new Variant();
                if (conf.ContainsKey("bestone") && conf["bestone"][0])
                {
                    foreach (Variant obj in conf["bestone"][0]["carr"]._arr)
                    {
                        obj["name"] = LanguagePack.getLanguageText("carrchief", obj["name"]._str);
                        _bestoneArr._arr.Add(obj);
                    }
                }
            }
            return _bestoneArr;
        }

        //--------------------------天下第一    end-------------------------------------------------------------------------------------------------
        //----------------------------过期提醒start----------------------------------------------------------------
        private Variant _permanentItemsArr;
        private void setPermanentItemsArr()
        {
            if (conf.ContainsKey("permanentItems"))
            {
                _permanentItemsArr = conf["permanentItems"][0]["tp"];
            }
        }
        public Variant get_permanentItems_by_type(string type)
        {
            if (null == _permanentItemsArr)
            {
                setPermanentItemsArr();
            }
            if (_permanentItemsArr == null)
            {
                return null;
            }
            for (uint i = 0; i < _permanentItemsArr.Length; i++)
            {
                if (_permanentItemsArr[i]["type"]._str == type)
                {
                    return _permanentItemsArr[i];
                }
            }
            return null;
        }

        public Variant Get3DShow(uint tpid)
        {
            if (conf.ContainsKey("permanentAvatar"))
            {
                Variant dataArr = conf["permanentAvatar"][0]["show3D"];
                foreach (Variant obj in dataArr._arr)
                {
                    if (obj["tpid"]._uint == tpid)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        //----------------------------过期提醒end----------------------------------------------------------------

        //----------------------------形象 头顶对应称号  start----------------------------------------------------------------
        private Variant _objAchives;
        public Variant GetObjAchiveID(string type, int id)
        {
            if (!_objAchives)
            {
                _objAchives = new Variant();
                if (conf.ContainsKey("achive"))
                {
                    Variant achive = conf["achive"][0];
                    if (achive)
                    {
                        foreach (string s in achive.Keys)
                        {
                            Variant tmp = new Variant();
                            foreach (Variant achdata in achive[s]._arr)
                            {
                                Variant arr = GameTools.split(achdata["achiveid"]._str, ",");
                                tmp[(achdata["id"]._int).ToString()] = arr;
                            }
                            _objAchives[s] = tmp;
                        }
                    }
                }
            }
            if (_objAchives[type])
            {
                return _objAchives[type][id.ToString()];
            }
            return null;
        }
        //----------------------------形象 头顶对应称号   end----------------------------------------------------------------

        //---------------------------新手指南  start------------------------------------------------------
        //		private Array _playerguideArr;
        public Variant GetPlayerGuideInfo(int tp)
        {
            return conf["playerguide"][tp.ToString()];
        }
        //---------------------------新手指南  end--------------------------------------------------------

        //---------------------------更新公告  start------------------------------------------------------
        public Variant GetUpdateBoardInfo()
        {
            return conf["updateboard"];
        }
        //---------------------------更新公告  end--------------------------------------------------------

        //---------------------------手机大礼包 start --------------------------------------------------------
        public Variant GetMobilegifts()
        {
            if (conf)
            {
                Variant mgift = conf["mobilegift"];
                if (mgift && mgift[0])
                {
                    return mgift[0]["itm"];
                }
            }
            return null;
        }
        //---------------------------手机大礼包 end --------------------------------------------------------

        //---------------------------副本结束的操作 start --------------------------------------------------------
        public Variant GetLvlFinDo(uint ltpid)
        {
            if (conf)
            {
                Variant lvlfindo = conf["lvlfindo"];
                if (lvlfindo && lvlfindo[0])
                {
                    foreach (Variant obj in lvlfindo[0]["lvl"]._arr)
                    {
                        if (ltpid == obj["ltpid"]._uint)
                        {
                            return obj;
                        }
                    }
                }
            }
            return null;
        }
        //---------------------------副本结束的操作 end --------------------------------------------------------
        //----------------------装备评分start------------------
        // private Variant _combptExatt;
        public int GetExattCombpt(int att, int grade = 1)
        {
            int val = 0;
            foreach (string exatt in _combptExatt.Keys)
            {
                int i = StringUtil.FormatStringType(exatt);
                if ((att & i) == i)
                {
                    if (_combptExatt[i.ToString()][grade] != null)
                    {
                        val += _combptExatt[i.ToString()][grade]._int;
                    }
                }
            }
            return val;
        }
        private Variant combptConf;
        public Variant GetCombptConf()
        {
            if (conf.ContainsKey("combpt"))
            {
                if (combptConf == null)
                {
                    combptConf = new Variant();
                    foreach (Variant obj in conf["combpt"]._arr)
                    {
                        combptConf[obj["attname"]._str] = obj["per"];
                    }
                }
            }
            return combptConf;
        }
        //----------------------装备评分end--------------------
        //--------------------神兵start--------------------
        public Variant GetVeaponData(int carr)
        {
            if (this.conf == null)
            {
                return null;
            }
            return this.conf["veapon"][carr.ToString()];
        }
        //--------------------神兵end--------------------
        /**
         * 根据钻石充值数量--返利--客户端奖励
         * */
        public Variant getClientPvipAwdByCount()
        {
            return conf["pvipitmname"][0]["item"];
        }

        //--------------------黄钻start---------------------------------
        //充值黄钻入口配置
        public Boolean GetRechargeState()
        {
            return conf["rechargestate"][0]["flag"];
        }
        private Variant _pvipChargeData;
        public Variant GetPvipCharge()
        {
            if (_pvipChargeData == null)
            {
                _pvipChargeData = new Variant();
                Variant data = conf["pvipCharge"][0];
                if (data)
                {
                    foreach (Variant obj in data["tpid"]._arr)
                    {
                        _pvipChargeData[obj["id"].ToString()] = obj;
                    }
                }
            }
            return _pvipChargeData;
        }
        private Variant _feeds;
        public Variant GetFeedsData()
        {
            if (null == _feeds)
            {
                _feeds = new Variant();
                Variant data = conf["feed"];
                if (data != null)
                {
                    foreach (Variant obj in data._arr)
                    {
                        _feeds[obj["type"]] = new Variant();
                        foreach (string s in obj.Keys)
                        {
                            _feeds[obj["type"]][s] = obj[s];
                        }
                        _feeds[obj["type"].ToString()]["once"] = obj["once"]._bool;
                        if (obj.ContainsKey("con"))
                        {
                            Variant cdata = new Variant();
                            foreach (Variant con in obj["con"]._arr)
                            {
                                cdata[con["arg"].ToString()] = con;
                                if (con.ContainsKey("once"))
                                {
                                    cdata[con["arg"].ToString()]["once"] = con["once"]._bool;
                                }
                                else
                                {
                                    cdata[con["arg"].ToString()]["once"] = _feeds[obj["type"].ToString()]["once"];
                                }
                                if (!con.ContainsKey("desc"))
                                {
                                    cdata[con["arg"].ToString()]["desc"] = _feeds[obj["type"].ToString()]["desc"];
                                }
                            }
                            _feeds[obj["type"].ToString()]["con"] = cdata;
                        }
                    }
                }
            }
            return _feeds;
        }
        //--------------------黄钻  end---------------------------------
        //--------------------坐骑start---------------------------------
        public int GetMountAvatar(int qual)
        {
            if (conf["mount"])
            {
                if (conf["mount"][qual.ToString()])
                {
                    return conf["mount"][qual.ToString()]["avatar"];
                }
            }
            return 0;
        }
        //--------------------坐骑  end---------------------------------
        //--------------------技能攻击 start-----------------------------------------
        public Variant GetSkillAttack(int carr)
        {
            if (conf.ContainsKey("attackSkill"))
            {
                return conf["attackSkill"][carr.ToString()];
            }
            return null;
        }
        //--------------------技能攻击   end-------------------     ----------------------
        //--------------------日常任务 start------------------------

        public int GetDailyQual(int mid, int rid, int qual)
        {
            Variant data = conf["dailyMis"];
            if (data && data[mid])
            {
                foreach (Variant obj in data[mid.ToString()]["rqual"]._arr)
                {
                    if (obj["id"]._int == rid)
                    {
                        if (qual < obj["qual"]._int)
                        {
                            qual = obj["qual"]._int;
                        }
                        break;
                    }
                }
            }
            return qual;
        }
        //--------------------日常任务   end ------------------------
        //----------------------副本组队start-------------------------------------
        public Variant GetMultilevel()
        {
            return conf["multilevel"];
        }
        //----------------------副本组队  end-------------------------------------


        public String GetRepAni(int cid, string ani)
        {
            Variant repConf = conf["replaceAni"];
            if (repConf && repConf.ContainsKey(cid.ToString()) && repConf[cid.ToString()]["rep"].ContainsKey(ani.ToString()))
            {
                return repConf[cid.ToString()]["rep"][ani]["rani"];
            }
            return ani;
        }
        //-------------------------地图进去限制----------------------
        private Variant _maplimit;
        public Variant GetMapLimit(int mapid)
        {
            if (_maplimit == null)
            {
                _maplimit = new Variant();
                Variant limits = conf["mapLimit"];
                if (limits!=null)
                {
                    foreach (Variant obj in limits._arr)
                    {
                        Variant mapids = GameTools.split(obj["mapid"]._str, ",");
                        foreach (string mid in mapids._arr)
                        {
                            _maplimit[mid] = obj;
                        }
                    }
                }

            }
            return _maplimit[mapid.ToString()];
        }

        //----------------------副本变身---------------------------------------
        private Variant _levelAvatar;
        public Variant GetLevelAvatar(int ltpid, int carr)
        {
            if (null == _levelAvatar)
            {
                _levelAvatar = new Variant();
                if (conf.ContainsKey("lvlAvatar"))
                {
                    foreach (Variant lvl in conf["lvlAvatar"]._arr)
                    {
                        _levelAvatar[lvl["ltpid"].ToString()] = new Variant();
                        Variant carrobj = new Variant();
                        foreach (Variant c in lvl["carr"]._arr)
                        {
                            carrobj[c["id"].ToString()] = c;
                        }
                        //_levelAvatar[lvl["ltpid"].ToString()]["prop"] = GameTools.deepCloneSimpleObject(lvl);
                        _levelAvatar[lvl["ltpid"].ToString()]["prop"] = lvl.clone();

                        _levelAvatar[lvl["ltpid"].ToString()]["prop"].RemoveKey("carrobj");
                        _levelAvatar[lvl["ltpid"].ToString()]["carr"] = carrobj;
                    }
                }
            }
            if (_levelAvatar.ContainsKey(ltpid.ToString()))
            {
                Variant v = new Variant();
                if (_levelAvatar[ltpid.ToString()]["carr"].ContainsKey(carr.ToString()))
                {
                    v["prop"] = _levelAvatar[ltpid.ToString()]["prop"];
                    v["carr"] = _levelAvatar[ltpid.ToString()]["carr"][carr.ToString()];
                    return v;
                }
                v["prop"] = _levelAvatar[ltpid.ToString()]["prop"];
                v["carr"] = _levelAvatar[ltpid.ToString()]["carr"][0];
                return v;
            }
            return null;
        }

        public Variant GetQuizConf()
        {
            return conf["quiz"];
        }
        //---------------------积分商城-------------------------
        public Variant GetpointShop()
        {
            return conf["pointShop"];
        }

        private Variant _actfestival;
        public Variant GetActFestival()
        {
            if (null == _actfestival)
            {
                _actfestival = new Variant();
                foreach (Variant obj in conf["actFestival"]._arr)
                {
                    string str = obj["id"]._str;
                    Variant arr = GameTools.split(str, ",");
                    _actfestival[obj["tp"].ToString()] = arr;
                }
            }
            return _actfestival;
        }

        //-----------------------客户端地图物品显示----------------------------------
        public Variant GetMapObj(int type, int showtp)
        {
            Variant mapobj = conf["mapObject"];
            if (mapobj && mapobj[type.ToString()])
            {
                return mapobj[type]["show"][showtp.ToString()];
            }
            return null;

        }
        //--------------------------------转生显示-----------------------------------------------

        public Variant GetResetObj(int lvl)
        {
            if (conf.ContainsKey("resetlevel"))
            {
                return conf["resetlevel"][lvl.ToString()];
            }
            return null;
        }

        // 职业加点对应加成
        private Variant _carrAtt;
        public Variant GetCarrAtt(int carr, string att)
        {
            if (_carrAtt == null)
            {
                _carrAtt = new Variant();
                if (conf["carrAtt"] == null)
                {
                    return null;
                }
                foreach (Variant obj in conf["carrAtt"]._arr)
                {
                    //					_carrAtt[obj.id] =
                    Variant carrAtt = new Variant(); ;
                    foreach (Variant atts in obj["addAtt"]._arr)
                    {
                        string str = atts["addAtts"]._str;
                        carrAtt[atts["att"].ToString()] = GameTools.split(str, ",");
                    }
                    _carrAtt[obj["id"].ToString()] = carrAtt;
                }
            }

            if (_carrAtt[carr.ToString()] == null)
            {
                return null;
            }
            return _carrAtt[carr.ToString()][att];
        }
        //副本头顶显示
        public Variant GetlevelTitle(uint tpid)
        {
            if (conf.ContainsKey("levelTitle"))
            {
                foreach (Variant obj in conf["levelTitle"]._arr)
                {
                    if (obj["tpid"]._uint == tpid)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        public Variant GetInvestYb()
        {
            if (conf.ContainsKey("investYb"))
            {
                string str = conf["investYb"][0]["yb"]._str;
                return GameTools.split(str, ",");
            }
            return null;
        }

        //任务追踪

        public Variant GetMisTrackObj()
        {
            if (conf["mistrack"])
            {
                return conf["mistrack"][0];
            }
            return null;
        }
        //---------------单个巡航点---------------
        public Variant GetMultiPosKil(uint id)
        {
            return conf["multiPosKil"][id.ToString()];
        }
        //-----------------自动任务 -------------------------------
        private Variant _autoMis;
        public Variant GetAutoMis()
        {
            if (conf.ContainsKey("autoMis"))
            {
                if (_autoMis == null)
                {
                    _autoMis = new Variant();
                    _autoMis = conf["autoMis"][0];
                    string lvls = _autoMis["maxlvl"]._str;
                    Variant lvlArr = GameTools.split(lvls, ",");
                    int randlvl = lvlArr[0]._int;
                    if (lvlArr.Length > 1)
                    {
                        Random num = new Random();
                        int radomnum = num.Next(0, lvlArr[1]._int - randlvl);
                        randlvl = randlvl + radomnum;//随机
                    }
                    _autoMis["maxlvl"] = randlvl;
                    if (_autoMis.ContainsKey("misids"))
                    {
                        string misids = _autoMis["misids"]._str;
                        Variant misArr = GameTools.split(misids, ",");
                        Random num = new Random();
                        int radomnum = num.Next(0, misArr.Length);
                        int r = radomnum;
                        _autoMis["misid"] = misArr[r.ToString()];
                    }
                    if (_autoMis.ContainsKey("disconnectlvl"))
                    {
                        string dlvl = _autoMis["disconnectlvl"]._str;
                        Variant dlvlArr = GameTools.split(dlvl, ",");
                        _autoMis["disminlvl"] = dlvlArr[0];
                        _autoMis["dismaxlvl"] = dlvlArr[1];
                    }
                }
                return _autoMis;
            }
            return null;
        }

        //---------------------------- 颜色配置   ---------------------------
        public uint GetColorByType(string s)
        {
            if (conf.ContainsKey("showcolor") && conf["showcolor"].ContainsKey(s))
            {

                return StringUtil.FormatStringType(conf["showcolor"][s]["color"]._str)._uint;
            }
            return 0xffffff;
        }

        //------------------------- 商城转生物品限制  ---------------------------
        /**
         * 返回不满足的物品 
         */
        public Variant GetItemsByResetlvl(int lvl)
        {
            Variant arr = new Variant();
            if (conf.ContainsKey("shopFilter"))
            {
                foreach (Variant obj in conf["shopFilter"].Values)
                {
                    if (lvl < obj["lvl"]._int)
                    {
                        arr._arr.AddRange(obj["items"]._arr);
                    }
                }
            }
            return arr;
        }

        /**
         * 返回全部的合成幸运符 
         */
        public Variant GetGemComposeRateItems()
        {
            if (conf.ContainsKey("rateItem"))
            {
                return conf["rateItem"];
            }
            return null;
        }
        public Variant GetGemHasRateItems()
        {
            if (conf.ContainsKey("hasRateItm"))
            {
                return conf["hasRateItm"];
            }
            return null;
        }

        //--------------------------------坐骑技能 坐标start--------------------
        private Variant _rideSkillPosArr;
        private Variant _getRideSkillPosArr()
        {
            if (_rideSkillPosArr == null)
            {
                _rideSkillPosArr = new Variant();
                Variant data = conf["ridestarpos"];
                foreach (Variant obj in data._arr)
                {
                    _rideSkillPosArr[obj["qual"].ToString()] = obj;
                }
            }
            return _rideSkillPosArr;
        }
        public Variant GetRideSkillPos(int qual)
        {
            Variant data = _getRideSkillPosArr();
            if (data && data.ContainsKey(qual.ToString()))
            {
                return data[qual]["pos"];
            }
            return null;
        }
        //多个合成配方

        public Variant GetMulitCompose(int id)
        {
            if (conf.ContainsKey("mulitCompose"))
            {
                if (conf["mulitCompose"][id.ToString()])
                {
                    return conf["mulitCompose"][id.ToString()];
                }
            }
            return null;
        }
        //-----------------------坐骑技能界面图标配置--------------------------------------
        public Variant GetRideSkillImg()
        {
            if (conf.ContainsKey("rideskillimg"))
            {
                return conf["rideskillimg"];
            }

            return null;
        }

        //-----------------------客户端虚拟技能配置--------------------------------------
        public Variant GetVirSkillConfBySkid(uint skid)
        {
            if (conf.ContainsKey("virtualskill"))
            {
                Variant virSkills = conf["virtualskill"];
                foreach (Variant skillObj in virSkills._arr)
                {
                    if (skillObj["skid"]._uint == skid)
                    {
                        return skillObj;
                    }
                }
            }

            return null;
        }
        public Variant GetVirSkillConfByStateid(uint stateid)
        {
            if (conf.ContainsKey("virtualskill"))
            {
                Variant virSkills = conf["virtualskill"];
                foreach (Variant skillObj in virSkills._arr)
                {
                    if (skillObj["stateid"]._uint == stateid)
                    {
                        return skillObj;
                    }
                }
            }
            return null;
        }
        public Variant GetVirSkillContent(uint skid, uint sklvl)
        {
            if (conf.ContainsKey("virtualskill"))
            {
                Variant virSkills = conf["virtualskill"];
                foreach (Variant skillObj in virSkills._arr)
                {
                    if (skillObj["skid"]._uint == skid && skillObj["lvl"])
                    {
                        foreach (Variant lvlObj in skillObj["lvl"]._arr)
                        {
                            if (lvlObj["val"]._uint == sklvl)
                            {
                                return lvlObj;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public Variant GetComGroup(int id)
        {
            if (conf.ContainsKey("checkGroup"))
            {
                return conf["checkGroup"][id.ToString()];
            }
            return null;
        }
        public Variant GetCrossWarInfo()
        {
            return conf["crosswar"];
        }
        //--------------------------------------------時裝對應稱號配置 start-----------------------------------------------------------------
        public Variant GetFashionAchieves()
        {
            return conf["fashion"];
        }
        public Boolean IsFashionAchid(uint achid)
        {
            foreach (Variant fobj in conf["fashion"].Values)
            {
                if (achid == fobj["id"]._uint /*|| achid == fobj.relateid*/)
                {
                    return true;
                }
            }
            return false;
        }
        //		public uint GetFashionRelateID(uint id)
        //		{	//获取时装相关的限时称号
        //			foreach (Variant fobj in _conf.fashion)
        //			{
        //				if(id == fobj.id) 
        //				{
        //					return fobj.relateid;
        //				}
        //			}
        //			return 0;
        //		}
        public Variant GetFashionParts(uint id)
        {
            foreach (Variant fobj in conf["fashion"]._arr)
            {
                if (id == fobj["id"]._uint)
                {
                    return fobj["parts"];
                }
            }
            return null;
        }
        //--------------------------------------------時裝對應稱號配置 end-----------------------------------------------------------------

        //-------------------------------跨服战Start------------------------
        /**
         * 获得当前段位信息
         * 
         * @param int rnkv 荣誉值
         * 
         * */
        public Variant GetCrossWarAchieve(int rnkv)
        {
            foreach (Variant achieve in conf["crosswarAchieve"]["achieve"]._arr)
            {
                if (rnkv >= achieve["min"]._int && rnkv <= achieve["max"]._int)
                {
                    return achieve;
                }
            }
            return null;
        }
        /**
         * 获得所有段位信息
         * */
        public Variant GetCrossAchieves()
        {
            return conf["crosswarAchieve"]["achieve"];
        }
        //-------------------------------跨服战end------------------------
        //-------------------------------------阻挡地图特效start、------------------------------------------------
        public Variant getBlockMapEffect(int lpid)
        {
            if (conf["mapeffect"] && conf["mapeffect"].ContainsKey(lpid.ToString()))
            {
                return conf["mapeffect"][lpid.ToString()]["blockZone"];
            }
            return null;
        }
        //-------------------------------------阻挡地图特效  end、------------------------------------------------
        //-------------------------------------幸运大转盘配置 start---------------------------------------------
        public Variant GetLuckdrawConfByOid(string oid)
        {
            if (conf.ContainsKey("luckdraw"))
            {
                foreach (Variant obj in conf["luckdraw"]._arr)
                {
                    if (obj["oid"]._str == oid)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        public Variant GetLuckdrawConfById(int id)
        {
            if (conf.ContainsKey("luckdraw"))
            {
                foreach (Variant obj in conf["luckdraw"]._arr)
                {
                    if (obj["id"]._int == id)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        public Variant GetLuckydrawConf()
        {
            return conf["luckdraw"];
        }
        //-------------------------------------幸运大转盘配置 end---------------------------------------------
        //-------------------------------------跨服战中可以使用buff药品 start------------------------------------
        private Variant _bsBuffuse;
        public Variant GetBSBuffUses()
        {
            if (_bsBuffuse == null)
            {
                Variant conf = GetCommonConf("bsbuffuse");
                string buffStr = conf._str;
                _bsBuffuse = GameTools.split(buffStr, ",");
                if (_bsBuffuse)
                {
                    for (int i = 0; i < _bsBuffuse.Length; i++)
                    {
                        _bsBuffuse[i] = _bsBuffuse[i]._int;
                    }
                }
            }
            return _bsBuffuse;
        }
        //-------------------------------------跨服战中可以使用buff药品 start-----------------------------------
        //-------------------------------------多重副本 start-----------------------------------
        private Variant _lvlDirIds;
        public Variant GetLvlDirIds()
        {
            if (_lvlDirIds == null)
            {
                Variant conf = GetCommonConf("lvlDirlvlId");
                string idstr = conf._str;
                _lvlDirIds = GameTools.split(idstr, ",");
                for (int i = 0; i < _lvlDirIds.Length; i++)
                {
                    _lvlDirIds[i] = _lvlDirIds[i]._uint;
                }
            }
            return _lvlDirIds;
        }
        //-------------------------------------多重副本 end-----------------------------------
        public Boolean IsLevelNeedShow(int idx, Variant netData)
        {
            Variant arr = conf["lvlNeedHide"];
            bool show = true;
            if (arr[idx.ToString()])
            {
                Variant a = GameTools.split(arr[idx.ToString()]["hide"]._str, ",");
                switch (a[0]._str)
                {
                    case "level":
                        show = netData["level"]._int >= a[1]._int;
                        break;
                    case "resetlvl":
                        show = netData["resetlvl"]._int >= a[1]._int;
                        break;
                }
            }
            return show;
        }

        public Variant GetAttShowVal()
        {
            string attshow = GetCommonConf("attshowval")._str;
            if (attshow != null)
            {

                return GameTools.split(attshow, ",");
            }
            return null;
        }
        public Variant GetNewBroadcastWay()
        {
            String ltpidStr = (GetCommonConf("newBroadcastWay")).ToString();
            if (null != ltpidStr)
            {
                string[] ltpidArr = ltpidStr.Split(',');
                Variant var = new Variant();
                foreach (string s in ltpidArr)
                    var.pushBack(s);
                return var;
            }
            return null;
        }


        /**
         * 把common的配置转成数组形式
         */
        public Variant GetCommonConfArray(string id, bool toInt = false)
        {
            Variant conf = GetCommonConf(id);
            string confStr = conf._str;
            if (confStr != null)
            {
                Variant arr = GameTools.split(confStr, ",");
                if (toInt)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = arr[i]._int;
                    }
                }
                return arr;
            }
            return null;
        }
        public Variant GetLevelHall()
        {
            return conf["levelhall"];
        }

        public Variant GetWorshipData()
        {
            return conf["worship"];
        }


        public int GetRecommendAutoItem(int level, string value)
        {
            if (conf.ContainsKey("autoRecommend"))
            {
                foreach (Variant recommend in conf["autoRecommend"]._arr)
                {
                    if (recommend["value"]._str == value)
                    {
                        foreach (Variant item in recommend["item"]._arr)
                        {
                            if ((level >= item["min"]._int || !item.ContainsKey("min")) && (level <= item["max"]._int || !item.ContainsKey("max")))
                                return item["tpid"];
                        }
                    }
                }
            }
            return 0;
        }

        /**
		 *获得装备等级
		 */	
		public int GetItemGrade(int tpid)
		{
			if(m_conf["clientItem"][0]["item"] == null)
			{
				return 0;
			}
			foreach(Variant obj in m_conf["clientItem"][0]["item"]._arr)
			{
				if(obj["tpid"] == tpid)
				{
					return obj["grade"]._int;
				}
			}
			return 0;
		}
        public Variant GetTranscriptinfo(int tpid)
        {
            if (m_conf.ContainsKey("transcriptinfo"))
            {
                foreach (Variant obj in m_conf["transcriptinfo"].Values)
                {
                    if (obj["tpid"] == tpid)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        public Variant GetMisAcceptable(int mid)
        {
            if (conf.ContainsKey("misAcceptable"))
            {
                foreach (Variant acp in conf["misAcceptable"]._arr)
                {
                    if (acp["mid"]._int == mid)
                    {
                        return acp;
                    }
                }
            }
            return null;
        }


        //----------------------------------------成就相关-----------------------------------------
        private Variant _mapAchieveInfo = new Variant();
        /// <summary>
        /// 获取某个地图成就
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public Variant GetMapAchieve(int idx)
        {
            if (!_mapAchieveInfo[idx])
            {
                Variant info = m_conf["mapachieve"][idx];
                Variant arr = GameTools.split(info["achieves"], ",");
                for (int i = 0; i < arr.Length; ++i)
                {
                    arr[i] = arr[i]._int;
                }
                _mapAchieveInfo[idx] = GameTools.createGroup("idx", idx, "mapid",
                    info["mapid"], "achieves", arr, "special", info["special"]);
            }
            return _mapAchieveInfo[idx];
        }
        /// <summary>
        /// 获取所有的地图成就
        /// </summary>
        /// <returns></returns>
        public Variant GetAllMapAchieve()
        {
            foreach (Variant info in m_conf["mapachieve"]._arr)
            {
                int idx = info["idx"];
                if (!_mapAchieveInfo[idx])
                {
                    Variant arr = GameTools.split(info["achieves"]._str, ",");
                    for (var i = 0; i < arr.Length; ++i)
                    {
                        arr[i] = arr[i]._int;
                    }
                    _mapAchieveInfo[idx] = GameTools.createGroup("idx", idx,
                    "mapid", info["mapid"], "achieves", arr);
                }
            }
            return _mapAchieveInfo;
        }
        public int GetMapAchieveNum()
        {
            return m_conf["mapachieve"].Length;
        }
        /// <summary>
        /// 获取成就信息
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>

        public Variant GetAchieveInfo(int id)
        {
            return m_conf["achieve"][id];
        }
        //----------------------------------------百服、合服-------------------------------------
        public Variant GetBfActivity(uint tp)
        {
            return m_conf["bfactivity"][tp];
        }
        public Variant GetHfActivity()
        {
            return m_conf["hfactivity"][0];
        }
        //----------------------------------------副本奖励物品-----------------------------------------
        public Variant GetLoseAwd(int ltpid, int diff)
        {
            if (m_conf["lose_awd"] != null)
            {
                Variant lvl = m_conf["lose_awd"][ltpid];
                if (lvl)
                {
                    return lvl["diff"][diff];
                }
            }
            return null;
        }
        public Variant GetWinAwd(int ltpid, int diff)
        {
            if (m_conf.ContainsKey("win_awd"))
            {
                Variant lvl = m_conf["win_awd"][ltpid.ToString()];
                if (null != lvl)
                {
                    return lvl["diff"][diff];
                }
            }
            return null;
        }
        //---------------------------技能释放配置 start ---------------------------------------------
        public Variant GetCastSkillConf()
        {
            return (m_conf.ContainsKey("castskillconf")) ? m_conf["castskillconf"] : null;
        }
        //---------------------------技能释放配置 end ---------------------------------------------
        //---------------------------技能设置配置 start ---------------------------------------------
        public Variant GetSkillsetConf()
        {
            return (m_conf.ContainsKey("skillsetconf")) ? m_conf["skillsetconf"][0] : null;
        }

        //---------------------------模式设置 start -------------------------------------------
        public Variant GetViewUIS(uint tp)
        {
            if ((m_conf.ContainsKey("systemset")) && m_conf["systemset"].ContainsKey(tp.ToString()))
            {
                return m_conf["systemset"][tp.ToString()]["u"];
            }
            return null;
        }
        //---------------------------模式设置 end ---------------------------------------------
        //----------------------------------------自动加技能buff _start-------------------------------------
		public Variant get_buffSkill()
		{
            return m_conf["auto_buff"];
		}
		//----------------------------------------自动加技能buff _end-------------------------------------

        //根据职业获取排行榜头像
        //protected Variant _conf;
		public String getRangPlayerHeadImgPath(uint uCarr)
		{
			if(m_conf.ContainsKey ("rankheadImg"))
			{
				Variant objHeadConf = m_conf["rankheadImg"];
                //foreach(String i in objHeadConf[0]["img"].Keys)
                for (int i = 0; i < objHeadConf[0]["img"].Length; i++)
                {
                    if (objHeadConf[0]["img"][i]["carr"]._uint == uCarr)
                    {
                        return objHeadConf[0]["img"][i]["content"];
                    }
                }
				
			}
			
			return "";
			
		}


    }
}
