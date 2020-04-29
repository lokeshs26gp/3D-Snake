using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateAbstract : MonoBehaviour
{
	protected virtual void Start ()
    {
        GameManager.OnGameStateChangeEvent += OnGameStateChange;
    }

    protected virtual void OnGameStateChange(GAMESTATE _state){ }
    protected virtual void OnDestroy()
    {
        GameManager.OnGameStateChangeEvent -= OnGameStateChange;
    }
}
