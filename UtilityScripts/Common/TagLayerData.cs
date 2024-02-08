using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagLayerData 
{
    #region ----- Tag -----

    public const string UNTAGGED_TAG = "Untagged";
    public const string MAIN_CAMERA_TAG = "Camera";
    public const string PLAYER_TAG = "Player";
    public const string BG_MUSIC = "BGMusic";
    public const string SFX = "SFX";

    #endregion

    #region ----- Sorting Layer -----

    public const string DEFAULT_SORTING_LAYER = "Default";   

    public static readonly int DEFAULT_SORTING_LAYER_ID = SortingLayer.NameToID(DEFAULT_SORTING_LAYER);

    #endregion

    #region ----- Layer -----

    public const int DEFAULT_LAYER_BS = 1 << 0;
    public const int TRANSPARENT_FX_LAYER_BS = 1 << 1;
    public const int IGNORE_RAYCAST_LAYER_BS = 1 << 2;
    public const int WATER_LAYER_BS = 1 << 4;
    public const int UI_LAYER_BS = 1 << 5;
    public const int PLAYER_LAYER_BS = 1 << 6;

    public const int DEFAULT_LAYER_VALUE = 0;
    public const int TRANSPARENT_FX_LAYER_VALUE = 1;
    public const int IGNORE_RAYCAST_LAYER_VALUE = 2;
    public const int WATER_LAYER_VALUE = 4;
    public const int UI_LAYER_VALUE = 5;
    public const int PLAYER_LAYER_VALUE = 6;

    public const string DEFAULT_LAYER_STR = "Default";
    public const string TRANSPARENT_FX_LAYER_STR = "TransparentFX";
    public const string IGNORE_RAYCAST_LAYER_STR = "Ignore Raycast";
    public const string WATER_LAYER_STR = "Water";
    public const string UI_LAYER_STR = "UI";
    public const string PLAYER_LAYER_STR = "Player";

    #endregion

    #region ----- MEC Tag -----

    public const string MEC_CAN_BE_KILLED_TAG = "CAN_BE_KILLED";
    public const string MEC_DEFAULT_TAG = "DEFAULT";

    #endregion
}
