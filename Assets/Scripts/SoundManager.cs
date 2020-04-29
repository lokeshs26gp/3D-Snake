using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AudioCollection
{
    public string name;
    public AudioSource source;
    public void play()
    {
        if (source != null)
            source.Play();
        else
            Debug.LogError("AudioSource reference not set!");
    }
}


public class SoundManager : GameStateAbstract
{
    public List<AudioCollection> collectionList;

    public static System.Action<string> AudioPlayEvent;
    protected override void Start()
    {
        base.Start();
        AudioPlayEvent += OnPlayAudio;
    }
    protected override void OnGameStateChange(GAMESTATE _state)
    {
        switch (_state)
        {
            case GAMESTATE.INITILIZE: break;
            case GAMESTATE.PAUSE: break;
            case GAMESTATE.GAMEOVER:

                break;
            case GAMESTATE.GAMEPLAY:

                break;
            case GAMESTATE.GAMEEND:
                OnPlayAudio(ConstantsList.Sfx_Crash);
                break;



        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        AudioPlayEvent -= OnPlayAudio;
    }
    private void OnPlayAudio(string constString)
    {
        AudioCollection ac = collectionList.Find(x => x.name == constString);
        if (ac != null)
            ac.play();
        else
            Debug.LogError("AudioCollection not set for "+ constString);
    }

}
