using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SCR_Shooting_Scoped : MonoBehaviour 
{
    [Header("Scope Zoom")] 
    [SerializeField] private GameObject crossHairs;
    [SerializeField] private GameObject defaultCrosshair;
    [SerializeField] private float baseFOV;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float velocity = 0f;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private SCR_HeadsUpDisplay hud;
    public Camera _cam;

    void Start()
    {
        hud = SCR_HeadsUpDisplay.Instance;
        crossHairs = hud.scopeReticle;
        defaultCrosshair = hud.pointReticle;
        _cam = Camera.main;
        baseFOV = _cam.fieldOfView;
    }
    void Update() {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _cam.fieldOfView = Mathf.SmoothDamp(_cam.fieldOfView, zoomAmount, ref velocity, smoothTime);
        }
        else
        {
            _cam.fieldOfView = Mathf.SmoothDamp(_cam.fieldOfView, baseFOV, ref velocity, smoothTime);

        }

        if (Mathf.Abs(_cam.fieldOfView - zoomAmount) < 10.0f)
        {
            crossHairs.SetActive(true);
            defaultCrosshair.SetActive(false);
        }
        else
        {
            crossHairs.SetActive(false);
            defaultCrosshair.SetActive(true);
        }


    }


}
