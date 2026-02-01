using UnityEngine;

public static class Constants
{
    public static string TAG_Player = "Player";

    public static int LAYER_MovingPlatform = LayerMask.GetMask("MovingPlatform");
    public static int LAYER_Tongueable = LayerMask.GetMask("Tongueable");
    public static int LAYER_Default = LayerMask.GetMask("Default");

    // Game Specific
    public static float TONGUE_Thickness = 1f;
    // Will half this a lot - so extend 0.2f and retract 0.2f
    public const float TONGUE_Speed = 0.4f;
    
    public static float EAGLE_DetectionTime = 0.5f;

    
}
