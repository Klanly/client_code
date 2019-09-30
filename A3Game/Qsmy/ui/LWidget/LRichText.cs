using UnityEngine;
using System.Collections.Generic;
using System.Security;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lui
{

    public enum RichType
    {
        TEXT,
        IMAGE,
        ANIM,
        NEWLINE,
    }

    public enum RichAlignType
    {
        DESIGN_CENTER,
        LEFT_TOP,
        LEFT_BOTTOM,
        RIGHT_BOTTOM,
        RIGHT_TOP
    }

    public enum RichPivotType
    {
        LEFT_TOP,
        LEFT_BOTTOM,
        RIGHT_BOTTOM,
        RIGHT_TOP
    }

    class LRichElement : Object
    {

        public RichType type { get; protected set; }
        public Color color { get; protected set; }
        public string data { get; protected set; }
    }
    /// <summary>
    /// 文本元素
    /// </summary>
    class LRichElementText : LRichElement
    {
        public string txt { get; protected set; }
        public bool isUnderLine { get; protected set; }
        public bool isOutLine { get; protected set; }
        public int fontSize { get; protected set; }

        public LRichElementText(Color color, string txt, int fontSize, bool isUnderLine, bool isOutLine, string data)
        {
            this.type = RichType.TEXT;
            this.color = color;
            this.txt = txt;
            this.fontSize = fontSize;
            this.isUnderLine = isUnderLine;
            this.isOutLine = isOutLine;
            this.data = data;
        }
    }

    /// <summary>
    /// 图片元素
    /// </summary>
    class LRichElementImage : LRichElement
    {
        public string path { get; protected set; }

        public LRichElementImage(string path, string data)
        {
            this.type = RichType.IMAGE;
            this.path = path;
            this.data = data;
        }
    }

    /// <summary>
    /// 动画元素
    /// </summary>
    class LRichElementAnim : LRichElement
    {
        public string path { get; protected set; }
        public float fs { get; protected set; }

        public LRichElementAnim(string path, float fs, string data)
        {
            this.type = RichType.ANIM;
            this.path = path;
            this.data = data;
            this.fs = fs;
        }
    }

    /// <summary>
    /// 换行元素
    /// </summary>
    class LRichElementNewline : LRichElement
    {
        public LRichElementNewline()
        {
            this.type = RichType.NEWLINE;
        }
    }

    /// <summary>
    /// 缓存结构
    /// </summary>
    class LRichCacheElement : Object
    {
        public bool isUse;
        public GameObject node;
        public LRichCacheElement(GameObject node)
        {
            this.node = node;
        }
    }

    /// <summary>
    /// 渲染结构
    /// </summary>
    class LRenderElement
    {
        public RichType type;
        public string strChar;
        public int width;
        public int height;
        public bool isOutLine;
        public bool isUnderLine;
        public Font font;
        public int fontSize;
        public Color color;
        public string data;
        public string path;
        public float fs;
        public bool isNewLine;
        public Vector2 pos;
        public RectTransform rect;

        public LRenderElement Clone()
        {
            LRenderElement cloneOjb = new LRenderElement();
            cloneOjb.type = this.type;
            cloneOjb.strChar = this.strChar;
            cloneOjb.width = this.width;
            cloneOjb.height = this.height;
            cloneOjb.isOutLine = this.isOutLine;
            cloneOjb.isUnderLine = this.isUnderLine;
            cloneOjb.font = this.font;
            cloneOjb.fontSize = this.fontSize;
            cloneOjb.color = this.color;
            cloneOjb.data = this.data;
            cloneOjb.path = this.path;
            cloneOjb.fs = this.fs;
            cloneOjb.isNewLine = this.isNewLine;
            cloneOjb.pos = this.pos;
            cloneOjb.rect = this.rect;
            return cloneOjb;
        }
    }

    /// <summary>
    /// 富文本
    /// </summary>
    public class LRichText : MonoBehaviour, IPointerClickHandler
    {
        public RichAlignType alignType;
        public RichPivotType pivotType;
        public int verticalOffset,uVerticalOffset = 0;
        public int maxLineWidth;
        public Font font;

        public UnityAction<string> onClickHandler;
        public int realLineHeight { get; protected set; }
        public int realLineWidth { get; protected set; }

        List<LRichElement> _richElements;
        List<LRenderElement> _elemRenderArr;
        List<LRichCacheElement> _cacheLabElements;
        List<LRichCacheElement> _cacheImgElements;
        List<LRichCacheElement> _cacheFramAnimElements;
        Dictionary<GameObject, string> _objectDataMap;

        public void removeAllElements()
        {
            foreach (LRichCacheElement lab in _cacheLabElements)
            {
                lab.isUse = false;
                lab.node.transform.SetParent(null);
            }
            foreach (LRichCacheElement img in _cacheImgElements)
            {
                img.isUse = false;
                img.node.transform.SetParent(null);
            }

            foreach (LRichCacheElement anim in _cacheFramAnimElements)
            {
                anim.isUse = false;
                anim.node.transform.SetParent(null);
            }
            _elemRenderArr.Clear();
            _objectDataMap.Clear();
        }

        public void insertElement(string txt, Color color, int fontSize, bool isUnderLine, bool isOutLine, Color outLineColor, string data)
        {
            _richElements.Add(new LRichElementText(color, txt, fontSize, isUnderLine, isOutLine, data));
        }

        public void insertElement(string path, float fp, string data)
        {
            _richElements.Add(new LRichElementAnim(path, fp, data));
        }

        public void insertElement(string path, string data)
        {
            _richElements.Add(new LRichElementImage(path, data));
        }

        public void insertElement(int newline)
        {
            _richElements.Add(new LRichElementNewline());
        }

        public LRichText()
        {
            this.alignType = RichAlignType.LEFT_TOP;
            this.maxLineWidth = 200;

            _richElements = new List<LRichElement>();
            _elemRenderArr = new List<LRenderElement>();
            _cacheLabElements = new List<LRichCacheElement>();
            _cacheImgElements = new List<LRichCacheElement>();
            _cacheFramAnimElements = new List<LRichCacheElement>();
            _objectDataMap = new Dictionary<GameObject, string>();
        }

        public void reloadData(float newElemOffset = 3.5f)
        {
            this.removeAllElements();
            RectTransform rtran = this.GetComponent<RectTransform>();
            //align
            switch (alignType)
            {
                default:
                case RichAlignType.LEFT_TOP:
                    rtran.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
                    break;
                case RichAlignType.DESIGN_CENTER:
                    rtran.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                    break;
                case RichAlignType.RIGHT_BOTTOM:
                    rtran.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
                    break;
                case RichAlignType.LEFT_BOTTOM:
                    rtran.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
                    break;
            }
            for (int i = 0; i < _richElements.Count; i++)
            {
                LRichElement elem = _richElements[i];
                if (elem.type == RichType.TEXT)
                {
                    LRichElementText elemText = elem as LRichElementText;
                    char[] _charArr = elemText.txt.ToCharArray();
                    TextGenerator gen = new TextGenerator();
                    for (int j = 0; j < _charArr.Length; j++)
                    {
                        char strChar = _charArr[j];
                        LRenderElement rendElem = new LRenderElement();
                        rendElem.type = RichType.TEXT;
                        rendElem.strChar = strChar.ToString();
                        rendElem.isOutLine = elemText.isOutLine;
                        rendElem.isUnderLine = elemText.isUnderLine;
                        rendElem.font = this.font;
                        rendElem.fontSize = elemText.fontSize;
                        rendElem.data = elemText.data;
                        float r = elemText.color.r,
                            g = elemText.color.g,
                            b = elemText.color.b;
                        if (r > 1) r = r / 255f;
                        if (g > 1) g = g / 255f;
                        if (b > 1) b = b / 255f;
                        rendElem.color = new Color(r, g, b);

                        TextGenerationSettings setting = new TextGenerationSettings();
                        setting.font = this.font;
                        setting.fontSize = elemText.fontSize;
                        setting.lineSpacing = 1;
                        //setting.scaleFactor = 1;
                        setting.verticalOverflow = VerticalWrapMode.Overflow;
                        setting.horizontalOverflow = HorizontalWrapMode.Overflow;

                        rendElem.width = (int)gen.GetPreferredWidth(rendElem.strChar, setting);
                        rendElem.height = (int)gen.GetPreferredHeight(rendElem.strChar, setting);
                        _elemRenderArr.Add(rendElem);
                    }
                }
                else if (elem.type == RichType.IMAGE)
                {
                    LRichElementImage elemImg = elem as LRichElementImage;
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = RichType.IMAGE;
                    rendElem.path = elemImg.path;
                    rendElem.data = elemImg.data;

                    Sprite sp = GAMEAPI.ABUI_LoadSprite(rendElem.path);
                    rendElem.width = (int)sp.rect.size.x;
                    rendElem.height = (int)sp.rect.size.y;
                    _elemRenderArr.Add(rendElem);
                }
                else if (elem.type == RichType.ANIM)
                {
                    LRichElementAnim elemAnim = elem as LRichElementAnim;
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.type = RichType.ANIM;
                    rendElem.path = elemAnim.path;
                    rendElem.data = elemAnim.data;
                    rendElem.fs = elemAnim.fs;

                    Sprite sp = GAMEAPI.ABUI_LoadSprite(rendElem.path + "/1");
                    rendElem.width = (int)sp.rect.size.x;
                    rendElem.height = (int)sp.rect.size.y;
                    _elemRenderArr.Add(rendElem);
                }
                else if (elem.type == RichType.NEWLINE)
                {
                    LRenderElement rendElem = new LRenderElement();
                    rendElem.isNewLine = true;
                    _elemRenderArr.Add(rendElem);
                }
            }

            _richElements.Clear();

            formarRenderers( newElemOffset );
        }

        protected void formarRenderers(float newElemOffset)
        {
            int oneLine = 0;
            int lines = 1;
            int height = 27;
            bool isReplaceInSpace = false;
            int len = _elemRenderArr.Count;

            for (int i = 0; i < len; i++)
            {
                isReplaceInSpace = false;
                LRenderElement elem = _elemRenderArr[i];
                if (elem.isNewLine) // new line
                {
                    oneLine = 0;
                    lines++;
                    elem.width = 10;
                    elem.height = height;
                    elem.pos = new Vector2(oneLine, -lines * height);

                }
                else //other elements
                {
                    if (oneLine + elem.width > maxLineWidth)
                    {
                        if (elem.type == RichType.TEXT)
                        {
                            if (isChinese(elem.strChar) || elem.strChar == " ")
                            {
                                oneLine = 0;
                                lines++;

                                elem.pos = new Vector2(oneLine, -lines * height);
                                oneLine = elem.width;
                            }
                            else // en
                            {
                                int spaceIdx = 0;
                                int idx = i;
                                while (idx > 0)
                                {
                                    idx--;
                                    if (_elemRenderArr[idx].strChar == " " &&
                                        _elemRenderArr[idx].pos.y == _elemRenderArr[i - 1].pos.y) // just for the same line//
                                    {
                                        spaceIdx = idx;
                                        break;
                                    }
                                }
                                // can't find space , force new line
                                if (spaceIdx == 0)
                                {
                                    oneLine = 0;
                                    lines++;
                                    elem.pos = new Vector2(oneLine, -lines * height);
                                    oneLine = elem.width;
                                }
                                else
                                {
                                    oneLine = 0;
                                    lines++;
                                    isReplaceInSpace = true; //reset cuting words position

                                    for (int _i = spaceIdx + 1; _i <= i; ++_i)
                                    {
                                        LRenderElement _elem = _elemRenderArr[_i];
                                        _elem.pos = new Vector2(oneLine, -lines * height);
                                        oneLine += _elem.width;

                                        _elemRenderArr[_i] = _elem;
                                    }
                                }
                            }
                        }
                        else if (elem.type == RichType.ANIM || elem.type == RichType.IMAGE)
                        {
                            lines++;
                            elem.pos = new Vector2(0, -lines * height);
                            oneLine = elem.width;
                        }
                    }
                    else
                    {
                        elem.pos = new Vector2(oneLine, -lines * height);
                        oneLine += (elem.width + 1 + countNumber(elem.strChar));//元素间距
                    }
                }
                if (isReplaceInSpace == false)
                {
                    _elemRenderArr[i] = elem;
                }
            }
            //sort all lines
            Dictionary<int, List<LRenderElement>> rendElemLineMap = new Dictionary<int, List<LRenderElement>>();
            List<int> lineKeyList = new List<int>();
            len = _elemRenderArr.Count;
            for (int i = 0; i < len; i++)
            {
                LRenderElement elem = _elemRenderArr[i];
                List<LRenderElement> lineList;

                if (!rendElemLineMap.ContainsKey((int)elem.pos.y))
                {
                    lineList = new List<LRenderElement>();
                    rendElemLineMap[(int)elem.pos.y] = lineList;
                }
                rendElemLineMap[(int)elem.pos.y].Add(elem);
            }
            // all lines in arr
            List<List<LRenderElement>> rendLineArrs = new List<List<LRenderElement>>();
            foreach (var item in rendElemLineMap)
            {
                lineKeyList.Add(-1 * item.Key);
            }
            lineKeyList.Sort();
            len = lineKeyList.Count;

            for (int i = 0; i < len; i++)
            {
                int posY = -1 * lineKeyList[i];
                string lineString = "";
                LRenderElement _lastEleme = rendElemLineMap[posY][0];
                LRenderElement _lastDiffStartEleme = rendElemLineMap[posY][0];
                Vector2 counter = Vector2.zero;
                if (rendElemLineMap[posY].Count > 0)
                {
                    List<LRenderElement> lineElemArr = new List<LRenderElement>();
                    foreach (LRenderElement elem in rendElemLineMap[posY])
                    {
                        if (_lastEleme.type == RichType.TEXT && elem.type == RichType.TEXT)
                        {
                            if (_lastEleme.color == elem.color && (_lastEleme.data == elem.data || elem.data == ""))//fixed：相同颜色，不同data触发结果相同的BUG
                            {
                                // the same color and same data can mergin one string
                                lineString += elem.strChar;
                            }
                            else // diff color
                            {
                                if (_lastDiffStartEleme.type == RichType.TEXT)
                                {   
                                    LRenderElement _newElem = _lastDiffStartEleme.Clone();
                                    _newElem.strChar = lineString;
                                    //_newElem.pos += new Vector2(-5, 0);
                                    lineElemArr.Add(_newElem);

                                    _lastDiffStartEleme = elem;

                                    counter = counter + new Vector2( newElemOffset , 0 );
                                    _lastDiffStartEleme.pos = _lastDiffStartEleme.pos + counter;
                                    lineString = elem.strChar;
                                }
                            }
                        }
                        else if (elem.type == RichType.IMAGE || elem.type == RichType.ANIM || elem.type == RichType.NEWLINE)
                        {
                            //interrupt
                            if (_lastDiffStartEleme.type == RichType.TEXT)
                            {
                                LRenderElement _newEleme = _lastDiffStartEleme.Clone();
                                _newEleme.strChar = lineString;
                                lineString = "";
                                lineElemArr.Add(_newEleme);
                            }
                            lineElemArr.Add(elem);                        }
                        else if (_lastEleme.type != RichType.TEXT)
                        {
                            //interrupt
                            _lastDiffStartEleme = elem;
                            if (elem.type == RichType.TEXT)
                            {
                                lineString = elem.strChar;
                            }
                        }
                        _lastEleme = elem;
                    }
                    // the last elementText
                    if (_lastDiffStartEleme.type == RichType.TEXT)
                    {
                        LRenderElement _newElem = _lastDiffStartEleme.Clone();
                        _newElem.strChar = lineString;
                        lineElemArr.Add(_newElem);
                    }
                    rendLineArrs.Add(lineElemArr);
                }
            }

            // offset position
            int _offsetLineY = 0;
            int oneLineHeight = font.fontSize;
            realLineHeight = 0;
            len = rendLineArrs.Count;
            for (int i = 0; i < len; i++)
            {
                List<LRenderElement> _lines = rendLineArrs[i];
                int _lineHeight = 0;
                foreach (LRenderElement elem in _lines)
                {
                    _lineHeight = Mathf.Max(_lineHeight, elem.height);
                }
                oneLineHeight = _lineHeight;
                realLineHeight += _lineHeight;
                _offsetLineY += (_lineHeight - height);

                int _len = _lines.Count;
                for (int j = 0; j < _len; j++)
                {
                    LRenderElement elem = _lines[j];
                    elem.pos = new Vector2(elem.pos.x, elem.pos.y - _offsetLineY);
                    realLineHeight = Mathf.Max(realLineHeight, (int)Mathf.Abs(elem.pos.y));
                    _lines[j] = elem;
                }
                rendLineArrs[i] = _lines;
            }

            // place all position
            realLineWidth = 0;
            GameObject obj = null;
            GameObject _lastGo=null;
            float _lastGoWidth=0;
            foreach (List<LRenderElement> _lines in rendLineArrs)
            {
                int _lineWidth = 0;
                _lastGo=null;
                foreach (LRenderElement elem in _lines)
                {
                    if ( elem.type != RichType.NEWLINE )
                    {
                        if ( elem.type == RichType.TEXT )
                        {
                            obj = getCacheLabel();
                            makeLabel( obj , elem );
                            _lineWidth += ( int ) obj.GetComponent<Text>().preferredWidth;
                        }
                        else if ( elem.type == RichType.IMAGE )
                        {
                            obj = getCacheImage( true );
                            makeImage( obj , elem );
                            _lineWidth += ( int ) obj.GetComponent<Image>().preferredWidth;
                        }
                        else if ( elem.type == RichType.ANIM )
                        {
                            obj = getCacheFramAnim();
                            makeFramAnim( obj , elem );
                            _lineWidth += elem.width;
                        }
                        obj.transform.SetParent( transform );
                        float newPosY = elem.pos.y;
                        if ( Mathf.Abs( elem.pos.y ) != oneLineHeight )
                        {
                            uVerticalOffset = verticalOffset + uVerticalOffset;
                            newPosY = newPosY + uVerticalOffset;
                        }
                        obj.transform.localPosition = new Vector2(
                            x: _lastGo == null ? elem.pos.x : _lastGo.transform.localPosition.x + _lastGoWidth ,
                            //y: newPosY
                            y: _lastGo == null ? newPosY : _lastGo.transform.localPosition.y
                        );

                        _objectDataMap[ obj ] = elem.data;
                        _lastGo=obj;

                        if ( elem.type  == RichType.ANIM ) { _lastGoWidth=elem.width; continue; }

                        _lastGoWidth =  elem.type  == RichType.TEXT ? obj.GetComponent<Text>().preferredWidth : obj.GetComponent<Image>().preferredWidth;

                    }
                }

                realLineWidth = Mathf.Max(_lineWidth, realLineWidth);
            }
            RectTransform rtran = this.GetComponent<RectTransform>();
            //align
            if (alignType == RichAlignType.DESIGN_CENTER)
            {
                rtran.sizeDelta = new Vector2(maxLineWidth, realLineHeight);
            }
            else if (alignType == RichAlignType.LEFT_TOP)
            {
                rtran.sizeDelta = new Vector2(realLineWidth, realLineHeight);
            }
        }

        void makeLabel(GameObject lab, LRenderElement elem)
        {
            Text comText = lab.GetComponent<Text>();
            if (comText != null)
            {
                comText.text = elem.strChar;
                comText.font = elem.font;
                comText.fontSize = elem.fontSize;
                comText.fontStyle = FontStyle.Normal;
                comText.color = elem.color;

            }

            Outline outline = lab.GetComponent<Outline>();
            // Outline outline1 = lab.GetComponent<Outline>();
            if (elem.isOutLine)
            {
                if (outline == null)
                {
                    outline = lab.AddComponent<Outline>();
                    //  outline1 = lab.AddComponent<Outline>(); 
                }

            }
            else
            {
                if (outline)
                {
                    Destroy(outline);
                }
                //if (outline1)
                //{
                //    Destroy(outline1);
                //}

            }

            if (elem.isUnderLine)
            {
                GameObject underLine = getCacheImage(false);
                Image underImg = underLine.GetComponent<Image>();
                underImg.color = elem.color;
                underImg.GetComponent<RectTransform>().sizeDelta = new Vector2(comText.preferredWidth, 1);
                underLine.transform.SetParent(transform);
                underLine.transform.localPosition = new Vector2(elem.pos.x, elem.pos.y);
            }
            if (elem.isNewLine)
            {
                comText.text = elem.strChar;
                comText.font = this.font;
                comText.fontSize = elem.fontSize;
                comText.fontStyle = FontStyle.Normal;
                comText.color = elem.color;
            }
        }

        void makeImage(GameObject img, LRenderElement elem)
        {
            Image comImage = img.GetComponent<Image>();
            if (comImage != null)
            {
                Sprite sp = GAMEAPI.ABUI_LoadSprite(elem.path);
                comImage.sprite = sp;
            }
        }

        void makeFramAnim(GameObject anim, LRenderElement elem)
        {
            LMovieClip comFram = anim.GetComponent<LMovieClip>();
            if (comFram != null)
            {
                comFram.path = elem.path;
                comFram.fps = elem.fs;
                comFram.loadTexture();
                comFram.play();
            }
        }

        protected GameObject getCacheLabel()
        {
            GameObject ret = null;
            int len = _cacheLabElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheLabElements[i];
                if (cacheElem.isUse == false)
                {
                    cacheElem.isUse = true;
                    ret = cacheElem.node;
                    break;
                }
            }
            if (ret == null)
            {
                ret = new GameObject();
                ret.AddComponent<Text>();
                ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
                fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                RectTransform rtran = ret.GetComponent<RectTransform>();
                rtran.pivot = Vector2.zero;
                switch (pivotType)
                {
                    default:
                    case RichPivotType.LEFT_TOP:
                        rtran.anchorMax = new Vector2(0, 1);
                        rtran.anchorMin = new Vector2(0, 1);
                        break;
                    case RichPivotType.LEFT_BOTTOM:
                        rtran.anchorMax = new Vector2(0, 0);
                        rtran.anchorMin = new Vector2(0, 0);
                        break;
                    case RichPivotType.RIGHT_TOP:
                        rtran.anchorMax = new Vector2(1, 1);
                        rtran.anchorMin = new Vector2(1, 1);
                        break;
                    case RichPivotType.RIGHT_BOTTOM:
                        rtran.anchorMax = new Vector2(1, 0);
                        rtran.anchorMin = new Vector2(1, 0);
                        break;
                }


                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheLabElements.Add(cacheElem);
            }
            return ret;
        }

        protected GameObject getCacheImage(bool isFitSize)
        {
            GameObject ret = null;
            int len = _cacheLabElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheLabElements[i];
                if (cacheElem.isUse == false)
                {
                    cacheElem.isUse = true;
                    ret = cacheElem.node;
                    break;
                }
            }
            if (ret == null)
            {
                ret = new GameObject();
                ret.AddComponent<Image>();
                ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
                fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                RectTransform rtran = ret.GetComponent<RectTransform>();
                rtran.pivot = Vector2.zero;
                switch (pivotType)
                {
                    default:
                    case RichPivotType.LEFT_TOP:
                        rtran.anchorMax = new Vector2(0, 1);
                        rtran.anchorMin = new Vector2(0, 1);
                        break;
                    case RichPivotType.LEFT_BOTTOM:
                        rtran.anchorMax = new Vector2(0, 0);
                        rtran.anchorMin = new Vector2(0, 0);
                        break;
                    case RichPivotType.RIGHT_TOP:
                        rtran.anchorMax = new Vector2(1, 1);
                        rtran.anchorMin = new Vector2(1, 1);
                        break;
                    case RichPivotType.RIGHT_BOTTOM:
                        rtran.anchorMax = new Vector2(1, 0);
                        rtran.anchorMin = new Vector2(1, 0);
                        break;
                }

                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheLabElements.Add(cacheElem);
            }
            ContentSizeFitter fitCom = ret.GetComponent<ContentSizeFitter>();
            fitCom.enabled = isFitSize;
            return ret;
        }

        protected GameObject getCacheFramAnim()
        {
            GameObject ret = null;
            int len = _cacheFramAnimElements.Count;
            for (int i = 0; i < len; i++)
            {
                LRichCacheElement cacheElem = _cacheFramAnimElements[i];
                if (cacheElem.isUse == false)
                {
                    cacheElem.isUse = true;
                    ret = cacheElem.node;
                    break;
                }
            }
            if (ret == null)
            {
                ret = new GameObject();
                ret.AddComponent<Image>();
                ContentSizeFitter fit = ret.AddComponent<ContentSizeFitter>();
                fit.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fit.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                RectTransform rtran = ret.GetComponent<RectTransform>();
                rtran.pivot = Vector2.zero;
                switch (pivotType)
                {
                    default:
                    case RichPivotType.LEFT_TOP:
                        rtran.anchorMax = new Vector2(0, 1);
                        rtran.anchorMin = new Vector2(0, 1);
                        break;
                    case RichPivotType.LEFT_BOTTOM:
                        rtran.anchorMax = new Vector2(0, 0);
                        rtran.anchorMin = new Vector2(0, 0);
                        break;
                    case RichPivotType.RIGHT_TOP:
                        rtran.anchorMax = new Vector2(1, 1);
                        rtran.anchorMin = new Vector2(1, 1);
                        break;
                    case RichPivotType.RIGHT_BOTTOM:
                        rtran.anchorMax = new Vector2(1, 0);
                        rtran.anchorMin = new Vector2(1, 0);
                        break;
                }

                ret.AddComponent<LMovieClip>();

                LRichCacheElement cacheElem = new LRichCacheElement(ret);
                cacheElem.isUse = true;
                _cacheFramAnimElements.Add(cacheElem);
            }
            return ret;
        }

        protected bool isChinese(string text)
        {
            bool hasChinese = false;
            char[] c = text.ToCharArray();
            int len = c.Length;
            for (int i = 0; i < len; i++)
            {
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    hasChinese = true;
                    break;
                }
            }
            return hasChinese;
        }

        protected int countNumber(string text)
        {
            int countNumber = 0;
            char[] c = text.ToCharArray();
            int len = c.Length;
            for (int i = 0; i < len; i++)
            {
                if (c[i] >= 0x30 && c[i] <= 0x39)
                {
                    countNumber = countNumber + 1;
                }
            }
            return countNumber;
        }


        public void OnPointerClick(PointerEventData data)
        {
            if (_objectDataMap.ContainsKey(data.pointerEnter))
            {
                if ((onClickHandler != null) && (_objectDataMap[data.pointerEnter] != ""))
                {
                    onClickHandler.Invoke(_objectDataMap[data.pointerEnter]);
                }
            }

        }
    }

}