using UnityEngine;

static public class EnumShader
{
    static public int SPI_COLOR = Shader.PropertyToID("_Color");
    static public int SPI_RIMCOLOR = Shader.PropertyToID("_RimColor");
    static public int SPI_RIMWIDTH = Shader.PropertyToID("_RimWidth");

    static public int SPI_TINT_COLOR = Shader.PropertyToID("_TintColor");

    static public int SPI_MAINTEX = Shader.PropertyToID("_MainTex");
    static public int SPI_SUBTEX = Shader.PropertyToID("_SubTex");
    static public int SPI_SPLCOLOR = Shader.PropertyToID("_SplColor");
    static public int SPI_STRCOLOR = Shader.PropertyToID("_StrColor");
    static public int SPI_CHANGECOLOR = Shader.PropertyToID("_ChangeColor");
    static public int SPI_SPLRIM = Shader.PropertyToID("_SplRim");
    static public int SPI_STRANIM = Shader.PropertyToID("_StrAnim");
}
