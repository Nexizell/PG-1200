using UnityEngine;

public static class Controls
{
    public static KeyCode Reload = KeyCode.R;
    public static KeyCode Aim = KeyCode.Mouse1;

    public static KeyCode Grenade = KeyCode.G;
    public static KeyCode Tool = KeyCode.C;
    public static KeyCode Support = KeyCode.X;

    public static KeyCode Chat = KeyCode.T;

    public static string GetKeyName(string key)
    {
        if (key == "Grenade")
        {
            return "G";
        }
        if (key == "Tool")
        {
            return "C";
        }
        if (key == "Support")
        {
            return "X";
        }
        if (key == "Chat")
        {
            return "T";
        }
        return "?";
    }
}
