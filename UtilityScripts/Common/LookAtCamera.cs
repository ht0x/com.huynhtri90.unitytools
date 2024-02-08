using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    #region ----- Variables -----

    private GameObject cameraObject;
    private Transform cacheTrans;
    
    #endregion
    
    #region ----- Unity Methods -----

    private void Awake()
    {
        cacheTrans = transform;
        var mainCamera = Camera.main;
        cameraObject = mainCamera ? mainCamera.gameObject : null;
    }

    void Update()
    {
        if (IsMissingCameraObject()) return;
        var cameraPos = cameraObject.transform.position;
        cameraPos.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation(cameraPos - cacheTrans.position);
    }
    
    #endregion
    
    #region ----- Utilities -----

    private bool IsMissingCameraObject()
    {
        return ReferenceEquals(cameraObject, null);
    }
    
    #endregion
}
