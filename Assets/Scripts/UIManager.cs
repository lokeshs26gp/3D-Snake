using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : GameStateAbstract
{
    public GameObject inilitizeObj;
    public GameObject inilitizecompletObj;
    public GameObject gameplayObj;
    public GameObject gameoverObj;
    public GameObject pauseObj;


    private GAMESTATE cachedState;
    protected override void Start()
    {
        base.Start();

    }
    protected override void OnGameStateChange(GAMESTATE _state)
    {
        switch (cachedState)
        {
            case GAMESTATE.INITILIZE:
                inilitizeObj.SetActive(false);
                break;
            case GAMESTATE.INITILIZECOMPLETE:
                inilitizecompletObj.SetActive(false);
                break;
            case GAMESTATE.PAUSE:
                pauseObj.SetActive(false);
                break;
            case GAMESTATE.GAMEOVER:
                gameoverObj.SetActive(false);
                break;
            case GAMESTATE.GAMEPLAY:
                gameplayObj.SetActive(false);
                break;

        }
        switch (_state)
        {
            case GAMESTATE.INITILIZE:
                inilitizeObj.SetActive(true);
                break;
            case GAMESTATE.INITILIZECOMPLETE:
                inilitizecompletObj.SetActive(true);
                break;
            case GAMESTATE.PAUSE:
                pauseObj.SetActive(true);
                break;
            case GAMESTATE.GAMEOVER:
                gameoverObj.SetActive(true);
                break;
            case GAMESTATE.GAMEPLAY:
                gameplayObj.SetActive(true);
                break;

        }
        cachedState = _state;
    }
    public void ChangeState(int state)
    {
        SoundManager.AudioPlayEvent(ConstantsList.Sfx_Click);
        GameManager.OnGameStateChangeByOther((GAMESTATE)state);
    }
    public void Restart()
    {
        SoundManager.AudioPlayEvent(ConstantsList.Sfx_Click);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
   
}
