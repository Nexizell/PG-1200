using UnityEngine;

public class FontChanger : MonoBehaviour
{
    public static FontChanger Instance;

    public UIFont[] bitmapFonts;

    public Font[] trueTypeFonts;

    public UIAtlas[] atlases;

    private int _fontNum;

    public int FontNum
    {
        get
        {
            return _fontNum;
        }
        set
        {
            _fontNum = value;
            if (FontForNumber() != null)
            {
                ChangeAllFonts(FontForNumber());
            }
            Storager.setInt("currentFont", value);
        }
    }

    public UIFont CurrentFont
    {
        get
        {
            return FontForNumber();
        }
    }

    public Font CurrentTTFont
    {
        get
        {
            return TTFontForNumber();
        }
    }

    public UIAtlas CurrentAtlas
    {
        get
        {
            return AtlasForNumber();
        }
    }

    private void Awake()
    {
        FontNum = Storager.getInt("currentFont", 0);
        Instance = this;
        ChangeAllFonts(FontForNumber());
    }

    private UIFont FontForNumber()
    {
        UIFont _font = bitmapFonts[0];
        if (FontNum < bitmapFonts.Length && FontNum > 0)
        {
            _font = bitmapFonts[FontNum];
        }
        return _font;
    }

    private Font TTFontForNumber()
    {
        Font _font = trueTypeFonts[0];
        if (FontNum < trueTypeFonts.Length && FontNum > 0)
        {
            _font = trueTypeFonts[FontNum];
        }
        return _font;
    }

    private UIAtlas AtlasForNumber()
    {
        UIAtlas _atlas = atlases[0];
        if (FontNum < atlases.Length && FontNum > 0)
        {
            _atlas = atlases[FontNum];
        }
        return _atlas;
    }

    private void ChangeAllFonts(UIFont font)
    {
        if (!Application.isPlaying)
        {
            return;
        }
        foreach (UILabel label in FindObjectsByType<UILabel>(FindObjectsSortMode.None))
        {
            if (!label.useUnityFont)
            {
                label.bitmapFont = font;
            }
            else
            {
                label.trueTypeFont = TTFontForNumber();
            }
            if (FontNum == 1 && !label.dontUseUpper)
            {
                label.modifier = UILabel.Modifier.ToUppercase;
            }
            else
            {
                label.modifier = UILabel.Modifier.None;
            }
            label.UpdateNGUIText();
        }
        foreach (UISprite sprite in FindObjectsByType<UISprite>(FindObjectsSortMode.None))
        {
            if (sprite.spriteName.Contains("Rank_"))
            {
                sprite.atlas = AtlasForNumber();
            }
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (FontNum == 0)
            {
                FontNum = 1;
            }
            else
            {
                FontNum = 0;
            }
        }
    }
#endif
}
