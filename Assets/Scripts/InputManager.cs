using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DIRECTION
{
    NONE = 0,
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4
}
public class InputManager : GameStateAbstract
{
    public delegate void GetInputSwipDelegate(DIRECTION dir,Vector3 start,Vector3 end);
    public static GetInputSwipDelegate OnInputSwipEvent;

    public Camera cam;
    [Header("KEYBOARD INPUT --> LEFT,RIGHT,UP,DOWN ARROWS")]
    public bool keyboardInput = false;
    public bool mouseInput   =  false;
    [Range(0.0f,10.0f)]
    public float minTouchdis = 0.5f;

    private bool isTouchDown = false;
    private Vector3 touchStart, touchEnd;

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }
    protected override void OnGameStateChange(GAMESTATE _state)
    {
        switch (_state)
        {
            
            case GAMESTATE.INITILIZECOMPLETE:
            case GAMESTATE.GAMEPLAY:
                gameObject.SetActive(true);
                break;
            case GAMESTATE.INITILIZE:
            case GAMESTATE.PAUSE:
            case GAMESTATE.GAMEOVER:
            case GAMESTATE.GAMEEND:
                gameObject.SetActive(false);
                break;

        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    void Update () 
	{
#if UNITY_ANDROID || UNITY_IOS || UNITY_WP8
        TouchProcess();
#else
        if (keyboardInput)
        {
            Keyboard();
        }
        if(mouseInput)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButtonDown(0))
            {
                isTouchDown = true;
                touchStart = Input.mousePosition;
            }
            else if (!Input.GetMouseButtonUp(0) && isTouchDown)
            {
                touchEnd = Input.mousePosition;
                OnInputSwip(GetGesture(touchStart, touchEnd), worldPosition(touchStart), worldPosition(touchEnd));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isTouchDown = false;
                touchEnd = Input.mousePosition;
                OnInputSwip(GetGesture(touchStart, touchEnd), worldPosition(touchStart), worldPosition(touchEnd));
                touchStart = touchEnd = Vector3.zero;
            }
        }
#endif
    }

    void TouchProcess()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        foreach (Touch touch in Input.touches)
        {
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    touchStart = touch.position;
                    break;
                case TouchPhase.Moved:
                    touchEnd = touch.position;
                    OnInputSwip(GetGesture(touchStart, touchEnd), worldPosition(touchStart), worldPosition(touchEnd));
                    break;
                case TouchPhase.Ended:
                    touchEnd = touch.position;
                    OnInputSwip(GetGesture(touchStart, touchEnd), worldPosition(touchStart), worldPosition(touchEnd));
                    break;
                case TouchPhase.Canceled:
                    touchStart = touchEnd = Vector3.zero;
                    break;
                case TouchPhase.Stationary:
                    touchStart = touch.position;
                    break;
            }
        }
    }
    Vector3 worldPosition(Vector3 position)
    {
       return  cam.ScreenPointToRay(position).GetPoint(5.0f);
    }
    DIRECTION GetGesture(Vector2 start,Vector2 end)
    {
        Vector2 delta = end - start;
        if(delta.magnitude >= minTouchdis)
        {
           // if(Debug.isDebugBuild) Debug.Log("Touch recognised");
            float deltaX = end.x - start.x;
            float deltaY = end.y - start.y;
            if(Mathf.Abs(deltaX)> Mathf.Abs(deltaY))//Horizontal
            {
                return deltaX > 0 ? DIRECTION.RIGHT : DIRECTION.LEFT;
            }
            else//Vertical
            {
                return deltaY > 0 ? DIRECTION.UP : DIRECTION.DOWN;
            }
        }
        return DIRECTION.NONE;
    }
    void Keyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnInputSwip(DIRECTION.LEFT,Vector3.zero,Vector3.zero);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnInputSwip(DIRECTION.RIGHT, Vector3.zero, Vector3.zero);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnInputSwip(DIRECTION.UP, Vector3.zero, Vector3.zero);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnInputSwip(DIRECTION.DOWN, Vector3.zero, Vector3.zero);
        }
    }
    public static void OnInputSwip(DIRECTION dir, Vector3 start, Vector3 end)
    {
        if (OnInputSwipEvent != null) OnInputSwipEvent(dir, start,end);
    }
	
}
