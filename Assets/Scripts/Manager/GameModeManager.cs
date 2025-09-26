using UnityEngine;

public static class GameModeManager
{
    public static int selectedMode = 0;

    public static void SetMode(int mode)
    {
        selectedMode = mode;
    }
}
