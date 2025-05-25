using UnityEngine;

public class FontChanger : MonoBehaviour
{
    public static FontChanger Instance;

    public UIFont pixelFont;

    public UIFont russoFont;

    public Font russoTTF;

    public Font pixelTTF;

    public UIAtlas pixelAtlas;

    public UIAtlas russoAtlas;

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
            return TTFFontForNumber();
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
        UIFont _font;
        switch (FontNum)
        {
            case 0:
                {
                    _font = russoFont;
                    break;
                }
            case 1:
                {
                    _font = pixelFont;
                    break;
                }
            default:
                {
                    _font = russoFont;
                    break;
                }
        }
        return _font;
    }

    private Font TTFFontForNumber()
    {
        Font _font;
        switch (FontNum)
        {
            case 0:
                {
                    _font = russoTTF;
                    break;
                }
            case 1:
                {
                    _font = pixelTTF;
                    break;
                }
            default:
                {
                    _font = russoTTF;
                    break;
                }
        }
        return _font;
    }

    private UIAtlas AtlasForNumber()
    {
        UIAtlas _atlas;
        switch (FontNum)
        {
            case 0:
                {
                    _atlas = russoAtlas;
                    break;
                }
            case 1:
                {
                    _atlas = pixelAtlas;
                    break;
                }
            default:
                {
                    _atlas = russoAtlas;
                    break;
                }
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
                label.trueTypeFont = TTFFontForNumber();
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
