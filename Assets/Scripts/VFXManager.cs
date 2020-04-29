using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : GameStateAbstract
{
   
    public VFXLineRenderer _vfxInputline;
    public GameObject   _fadeoutObject;

    public static System.Action<Vector3, Vector3> RenderLineVFXEvent;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnGameStateChange(GAMESTATE _state)
    {
        switch (_state)
        {
            case GAMESTATE.INITILIZE:
            case GAMESTATE.PAUSE:
            case GAMESTATE.GAMEOVER:
                _fadeoutObject.SetActive(false);
                goto default;
            case GAMESTATE.GAMEPLAY:
                _vfxInputline.Enable(true,0.1f);
                break;
            case GAMESTATE.GAMEEND:
                _fadeoutObject.SetActive(true);
                goto default;
            default:
                _vfxInputline.Enable(false, 0.0f);
                break;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public static void OnInputLineVFX(Vector3 start, Vector3 end)
    {
        if (RenderLineVFXEvent != null) RenderLineVFXEvent(start, end);
    }
}
