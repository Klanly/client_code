public enum PK_TYPE //PK的模式
{
    PK_PEACE = 0,  //和平模式，只打怪
    PK_PKALL = 1,  //PK模式，除了自己，其它人都打
    PK_TEAM = 2,   //组队模式，同队伍的不打
    PK_LEGION = 3, //军团模式，同军团的不打
    PK_HERO = 4,   //英雄模式，只能杀红名
    Pk_SPOET = 5,  // 战场竞技模式
}
public enum REDNAME_TYPE//红名
{
    RNT_NORMAL=0,  //0-50     #FFFFFF 
    RNT_RASCAL=1,  //16-90    #FFBBE8
    RNT_EVIL=2,    //91-150   #FF0000
    RNT_DEVIL=3    //151-1023 #840000  
}

public enum SPOSTNAME_TYPE {
      //MAIN = 0 ,  //自己
      //TEAMMATE =1, //友方
      //ENEMY = 2 //敌方
        BULE = 0,
        RED = 1

}
public enum LOGION_DEF //默认军团
{
    LNDF_MONSTER = 1,  //怪物
    LNDF_PLAYER = 2,   //没有军团的散玩家
}
