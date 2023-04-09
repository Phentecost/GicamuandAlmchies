using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    bool vsync;

    void Start()
    {
        vsync = false;
    }

    void Update()
    {
        if (vsync)
            QualitySettings.vSyncCount = 1; //Si VSync
        else
            QualitySettings.vSyncCount = 0; //No VSync

        //Fijar fps
        //Application.targetFrameRate = 60;
        
    }

    void VsyncActivated() //Se activa mediante opciones
    {
        vsync = true;
    }

    void VsyncDisabled()  //Se desactiva mediante opciones
    { 
        vsync = false; 
    }
}