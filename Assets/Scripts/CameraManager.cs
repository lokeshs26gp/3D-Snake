using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class CameraManager : GameStateAbstract
{

    private BlurOptimized _blurComp;
    private CameraShake _cameraShake;
    private CameraZoom _cameraZoom;

    private Vector3 _originalPos;
    private Quaternion _originalRotation;
    protected override void Start()
    {
        base.Start();
        _originalPos = transform.localPosition;
        _originalRotation = transform.rotation;

        _blurComp    = GetComponent<BlurOptimized>();
        _cameraShake = GetComponent<CameraShake>();
        _cameraZoom  = GetComponent<CameraZoom>();
    }

    protected override void OnGameStateChange(GAMESTATE _state)
    {
        //base.OnGameStateChange(_state);
        switch (_state)
        {
            case GAMESTATE.INITILIZE:
                _blurComp.enabled = true;
                Reset();
                break;
            case GAMESTATE.GAMEPLAY:
                _blurComp.enabled = false;
                break;
            case GAMESTATE.PAUSE:
                _blurComp.enabled = true;
                break;
            case GAMESTATE.GAMEEND:
                _blurComp.enabled = false;
                StartCoroutine(CameraEffect());
                break;
            case GAMESTATE.GAMEOVER:
                 StopCoroutine("CameraEffect");
                //_cameraZoom.enabled = false;
                _blurComp.enabled = true;
                Reset();
                break;
            
        }
    }
    IEnumerator CameraEffect()
    {
        _cameraShake.OnCameraShake(0.25f);
        yield return new WaitForSeconds(0.3f);
        _cameraShake.enabled = false;
        Reset();
       // yield return new WaitForSeconds(0.1f);
        //_cameraZoom.ZoomIn(50);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    private void Reset()
    {
        transform.LookAt(null);
        transform.localPosition = _originalPos;
        transform.rotation  = _originalRotation;
        
    }
}
