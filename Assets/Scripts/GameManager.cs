using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GAMESTATE
{
    NONE              = 0,
    INITILIZE         = 1,
    INITILIZECOMPLETE = 2,
    GAMEPLAY          = 3,
    PAUSE             = 4,
    GAMEEND           = 5,//For VFX Gap
    GAMEOVER          = 6
}

public class GameManager : SingletonMono<GameManager>
{
    [Header("------------------------GRID SYSTEM----------------------")]
    public GridSystemManager gridManager;
    [Header("                                    Rows            Colums")]
    public Vector2 GridSize;
    [Header("---------------------------------------------------------------")]
    [Space]
    public SnakeController   snakeController;
    public GameObject        CollectPrefab;
    
    [Range(0, 10)]public float snakeIntialSpeed ;
    [Range(0,1)]public float snakespeedIncreaserate;
    
    public delegate void GameStateChangeDelegate(GAMESTATE _state);
    public static event GameStateChangeDelegate OnGameStateChangeEvent;
    public static event GameStateChangeDelegate OnGameStateChangeByOtherEvent;
    public static System.Action<int> OnScoreChangeEvent;

    private GridNode    currentNode;
    private GameObject  fruitObj;
    private GridNode    fruitNode;
    private TraversalDirection currentDirection;

    private GAMESTATE _gameState = GAMESTATE.NONE;
    private GAMESTATE GameState { get { return _gameState; } set { ChangeState(value); } }

    private int _score;
    private int Score { get { return _score; } set { _score = value; if (OnScoreChangeEvent != null) OnScoreChangeEvent(_score); } }
    public override void Awake()
    {
        base.Awake();
    }
    private IEnumerator Start ()
    {
        InputManager.OnInputSwipEvent += GetInputFromPlayer;
        OnGameStateChangeByOtherEvent += ChangeStateByOtherModule;
        GameState = GAMESTATE.INITILIZE;
        yield return new WaitForSeconds(0.1f);
        GameState = GAMESTATE.INITILIZECOMPLETE;
    }
    private void OnDestroy()
    {
        InputManager.OnInputSwipEvent -= GetInputFromPlayer;
        OnGameStateChangeByOtherEvent -= ChangeStateByOtherModule;
    }
    private void ChangeState(GAMESTATE state)
    {
        switch(_gameState)
        {
            case GAMESTATE.INITILIZE:
                break;
            case GAMESTATE.GAMEPLAY:
                break;
            case GAMESTATE.PAUSE:
                if (state == GAMESTATE.GAMEPLAY)
                {
                    StartCoroutine(StartGame());
                }
                break;
            case GAMESTATE.GAMEOVER:
                break;

        }
        _gameState = state;
        switch (state)
        {
            case GAMESTATE.INITILIZE:
                gridManager.Initilize(GridSize);
                goto default;
            case GAMESTATE.INITILIZECOMPLETE:
                fruitObj = gridManager.SetCollectObject(CollectPrefab, PoolManager.GetInstance().GetPoolContainer(false), true, out fruitNode);
                currentNode = gridManager.GetRandomPosition(10,10);
                currentDirection = (TraversalDirection)(Random.Range(1, 4));
                SetInitilialPositions(currentNode, currentDirection);
                if (currentDirection == TraversalDirection.LEFT)
                    currentDirection = TraversalDirection.RIGHT;
                else if (currentDirection == TraversalDirection.RIGHT)
                    currentDirection = TraversalDirection.LEFT;
                else if (currentDirection == TraversalDirection.BOTTOM)
                    currentDirection = TraversalDirection.TOP;
                else if (currentDirection == TraversalDirection.TOP)
                    currentDirection = TraversalDirection.BOTTOM;
                goto default;
            case GAMESTATE.GAMEPLAY:
                StartCoroutine(StartGame());
                goto default;
            case GAMESTATE.PAUSE:
                goto default;
            case GAMESTATE.GAMEEND:
                StartCoroutine(ChangeState(GAMESTATE.GAMEOVER, 1.0f));
                goto default;
            case GAMESTATE.GAMEOVER:
                if (OnGameStateChangeEvent != null) OnGameStateChangeEvent(_gameState);
                SoundManager.AudioPlayEvent(ConstantsList.Sfx_GameOver);
                Score = _score;
                break; 
            default:
                if (OnGameStateChangeEvent != null) OnGameStateChangeEvent(_gameState);
                break;
        }
        
    }
    private void ChangeStateByOtherModule(GAMESTATE _state)
    {
        GameState = _state;
    }
    private IEnumerator ChangeState(GAMESTATE state,float gap)
    {
        yield return new WaitForSeconds(gap);
        GameState = state;
    }
    private IEnumerator StartGame()
    {
       
        while (_gameState == GAMESTATE.GAMEPLAY)
        {
            if (!IsValideMove(currentDirection))
            {
                GameState = GAMESTATE.GAMEEND;
            }

            yield return new WaitForSeconds(snakeIntialSpeed);
            
        }
        
    }
    private void GetInputFromPlayer(DIRECTION dir, Vector3 start, Vector3 end)
    {
        
        switch (dir)
        {
            case DIRECTION.UP   :
                if (currentDirection != TraversalDirection.BOTTOM)
                {
                    currentDirection = TraversalDirection.TOP;
                }
                goto default;
            case DIRECTION.DOWN :
                if (currentDirection != TraversalDirection.TOP)
                {
                    currentDirection = TraversalDirection.BOTTOM;
                }
                goto default;
            case DIRECTION.LEFT :
                if (currentDirection != TraversalDirection.RIGHT)
                {
                    currentDirection = TraversalDirection.LEFT; 
                }
                goto default;
            case DIRECTION.RIGHT:
                if (currentDirection != TraversalDirection.LEFT)
                {
                    currentDirection = TraversalDirection.RIGHT; 
                }
                goto default;
            default:
                //VFX 
                if (GameState == GAMESTATE.INITILIZECOMPLETE)
                {
                    snakeController.SetPosition(currentNode);
                    GameState = GAMESTATE.GAMEPLAY;
                }
                VFXManager.OnInputLineVFX(start, end);
                break;
        }
    }
    private bool IsValideMove(TraversalDirection dir)
    {
        currentNode = gridManager.GetNextNode(currentNode, dir);
        if (currentNode == null) return false;
        if (currentNode.isFilled) return false;
        snakeController.SetPosition(currentNode);
        if(fruitNode == currentNode)
        {
            SoundManager.AudioPlayEvent(ConstantsList.Sfx_Collect);
            Score++;
            snakeIntialSpeed -= Time.deltaTime * snakespeedIncreaserate;
            snakeController.GenerateTail();
            gridManager.SetCollectObject(fruitObj, PoolManager.GetInstance().GetPoolContainer(false), false,out fruitNode);
        }
        return true;
    }
    private void SetInitilialPositions(GridNode node, TraversalDirection dir)
    {
        snakeController.SetPosition(node, gridManager, dir);
    }
    public static void OnGameStateChangeByOther(GAMESTATE _state)
    {
        if (OnGameStateChangeByOtherEvent != null) OnGameStateChangeByOtherEvent(_state);
    }

}
