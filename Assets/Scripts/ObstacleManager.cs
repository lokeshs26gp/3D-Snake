using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : GameStateAbstract
{
    public GridSystemManager gridManager;
    public GameObject staticObstacleprefab_0;
    public GameObject staticObstacleprefab_1;
    public int staticObstaclecount;
    public GameObject movingObstacleprefab;
    [Range(0.0f, 100.0f)]
    public float movingObstaclespeed;

    private GAMESTATE _cacheState;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnGameStateChange(GAMESTATE _state)
    {
        switch (_state)
        {
            case GAMESTATE.INITILIZE:
                gridManager.BuildEdgeWalls(staticObstacleprefab_0, PoolManager.GetInstance().GetPoolContainer(true));
                gridManager.SetRandomObstacles(staticObstacleprefab_1, PoolManager.GetInstance().GetPoolContainer(false), staticObstaclecount);
                PoolManager.GetInstance().MergeMesh();
                break;
            case GAMESTATE.INITILIZECOMPLETE:
                 break;
            case GAMESTATE.PAUSE:

                break;
            case GAMESTATE.GAMEPLAY:
                MovingObstacle();
                break;
            case GAMESTATE.GAMEEND:
            case GAMESTATE.GAMEOVER:
               break;

        }
        _cacheState = _state;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private GridNode CheckforScopeToMove(TraversalDirection dir,int count)
    {
        GridNode node =   gridManager.GetRandomPosition();
        GridNode n = node;
        for(int i = 0;i< count;i++)
        {
            node = gridManager.GetNextNode(node, dir);
            if (node.isFilled) return null;
        }
        return n;
    }
    private void MovingObstacle()
    {
        int count = 20;
        TraversalDirection direction = TraversalDirection.LEFT;
        GridNode node = CheckforScopeToMove(direction, count);
        if (node == null)
        {
            direction = TraversalDirection.RIGHT;
            node = CheckforScopeToMove(direction, count);
        }
        if (node == null)
        {
            direction = TraversalDirection.TOP;
            node = CheckforScopeToMove(direction, count);
        }
        if (node == null)
        {
            direction = TraversalDirection.BOTTOM;
            node = CheckforScopeToMove(direction, count);
        }
        if (node != null)
        {
           GameObject movingObs =  gridManager.InstantiateObstacle(staticObstacleprefab_0, node, PoolManager.GetInstance().GetPoolContainer(false));
           SnakeBlock block =  movingObs.AddComponent<SnakeBlock>();
           StartCoroutine(MoveObstacle(block, node, direction, count));
        }

    }

    IEnumerator MoveObstacle(SnakeBlock block, GridNode node, TraversalDirection dir,int moveCount)
    {
        int count = 0;
        while (_cacheState == GAMESTATE.INITILIZECOMPLETE || _cacheState == GAMESTATE.GAMEPLAY)
        {
            if (count >= moveCount)
            {
                count = 0;
                dir = GetReverseDirection(dir);
            }
            node = gridManager.GetNextNode(node, dir);
            if (node.isFilled) GameManager.OnGameStateChangeByOther(GAMESTATE.GAMEEND);
            block.SetPosition(node);
            count++;
            yield return new WaitForSeconds((1/ movingObstaclespeed));
        }
    }
    TraversalDirection GetReverseDirection(TraversalDirection dir)
    {
        switch (dir)
        {
            case TraversalDirection.LEFT:return TraversalDirection.RIGHT;
            case TraversalDirection.RIGHT: return TraversalDirection.LEFT;
            case TraversalDirection.TOP: return TraversalDirection.BOTTOM;
            case TraversalDirection.BOTTOM: return TraversalDirection.TOP;
        }
        return TraversalDirection.NONE;
    }
}
