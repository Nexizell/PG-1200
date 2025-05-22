using UnityEngine;
using Rilisoft;

public class GetKeybindText : MonoBehaviour
{
    public UILabel keyLabel;

    public string keyID;

    private void Start()
    {
        string keyName = null;
        if (keyID != null)
        {
            keyName = Controls.GetKeyName(keyID);
        }
        if (!keyName.IsNullOrEmpty())
        {
            if (keyID == "Respawn")
            {
                keyLabel.text = string.Format("PRESS {0} TO RESPAWN", GetShortenedKeyName(keyName));
            }
            else
            {
                keyLabel.text = GetShortenedKeyName(keyName);
            }
        }
        if (GlobalControls.DoMobile/* || !UIOptions.keybindIndicators*/)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void SetKeybind()
    {
        string keyName = null;
        if (keyID != null)
        {
            keyName = Controls.GetKeyName(keyID);
        }
        if (!keyName.IsNullOrEmpty())
        {
            if (keyID == "Respawn")
            {
                keyLabel.text = string.Format("PRESS {0} TO RESPAWN", GetShortenedKeyName(keyName));
            }
            else
            {
                keyLabel.text = GetShortenedKeyName(keyName);
            }
        }
    }

    private string GetShortenedKeyName(string keyName)
    {
        if (keyName == "Mouse0") return "M0";
        if (keyName == "Mouse1") return "M1";
        if (keyName == "Mouse2") return "M2";
        if (keyName == "Mouse3") return "M3";
        if (keyName == "Mouse4") return "M4";
        if (keyName == "Mouse5") return "M5";
        if (keyName == "Mouse6") return "M6";
        if (keyName == "Escape") return "Esc";

        if (keyName == "Alpha1") return "1";
        if (keyName == "Alpha2") return "2";
        if (keyName == "Alpha3") return "3";
        if (keyName == "Alpha4") return "4";
        if (keyName == "Alpha5") return "5";
        if (keyName == "Alpha6") return "6";
        if (keyName == "Alpha7") return "7";
        if (keyName == "Alpha8") return "8";
        if (keyName == "Alpha9") return "9";
        if (keyName == "Alpha0") return "0";

        if (keyName == "LeftShift") return "LShift";
        if (keyName == "RightShift") return "RShift";
        if (keyName == "LeftControl") return "LCtrl";
        if (keyName == "RightControl") return "RCtrl";

        if (keyName == "UpArrow") return "Up";
        if (keyName == "DownArrow") return "Down";
        if (keyName == "RightArrow") return "Right";
        if (keyName == "LeftArrow") return "Left";

        return keyName;
    }
}
