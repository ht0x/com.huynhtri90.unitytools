using System;

namespace EnumCollection
{
    public enum INTERNET_CONNECTION_STATUS
    {
        NO_CONNECTION,
        CONNECTING,
        CONNECTED
    }

    public enum TimelineState
    {
        Idle,
        Playing,
        Paused,
        Resumed,
        Stopped
    }

    public enum InputState
    {
        MouseIdle,
        MouseDown,
        MouseHold,
        MouseUp
    }

    public enum PoolType
    {
        Item,
        Particle,
        Audio
    }
    
    public enum AssetType
    {
        Modsel,
        Sprite,
        Texture,
        Sound,
        Particle,
        ScriptableObject,
        Animation
    }
}