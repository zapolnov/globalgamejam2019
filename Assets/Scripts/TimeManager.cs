
using UnityEngine;

public static class TimeManager
{
    public static bool IsPaused;

    public static float deltaTime => IsPaused ? 0.0f : Time.deltaTime;
    public static float unscaledDeltaTime => IsPaused ? 0.0f : Time.unscaledDeltaTime;
    public static float timeSinceLevelLoad => Time.timeSinceLevelLoad;
}
