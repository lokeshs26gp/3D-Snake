using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    public Transform lookAtTransform;
    public Transform moveTransform;
    [Range(0.0f,100.0f)]
    public float zoomSpeed;
    private float originalFV ;
    
    private Camera cam;

    public void ZoomIn(float zoom)
    {
        cam = GetComponent<Camera>();
        originalFV = cam.fieldOfView;
        enabled = true;
        
        StartCoroutine(CoroutineZoomIn(zoom, lookAtTransform));
    }
    IEnumerator CoroutineZoomIn(float zoom,Transform target)
    {
        while(cam.fieldOfView > zoom)
        {

            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;
            transform.position = Vector3.MoveTowards(transform.position, moveTransform.position, Time.deltaTime * zoomSpeed);
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnDisable()
    {
        StopCoroutine("CoroutineZoomIn");
        cam.fieldOfView = originalFV;
    }
}
