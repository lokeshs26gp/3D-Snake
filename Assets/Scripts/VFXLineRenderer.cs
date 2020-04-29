using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class VFXLineRenderer : MonoBehaviour
{
    
    private LineRenderer _lineRenderer;
    private float showLinetimeinSec;

    void Start ()
    {
       _lineRenderer =  GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        VFXManager.RenderLineVFXEvent += ShowLineVFX;
    }
    public void Enable(bool enable,float OnScreentime)
    {
        gameObject.SetActive(enable);
        showLinetimeinSec = OnScreentime;
    }
    private void ShowLineVFX(Vector3 start, Vector3 end)
    {
         
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
        CancelInvoke("HideLine");
        Invoke("HideLine", showLinetimeinSec);
    }
    private void HideLine()
    {
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, Vector3.zero);
    }
    private void OnDisable()
    {
       VFXManager.RenderLineVFXEvent -= ShowLineVFX;
    }
   
}
