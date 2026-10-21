using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private SoundMaker soundMaker;
    private TagStatesAndVolumes tagSaV;

    private Dictionary<States, float> SaV
    {
        get
        {
            return tagSaV.StatesAndVolumes;
        }
    }

    public SoundManager(TagStatesAndVolumes tag, StateManager sM, SoundMaker soundMaker)
    {
        tagSaV = tag;
        sM.StateChanged += StateChanged;
        this.soundMaker = soundMaker;
    }

    private void StateChanged(object o, ValueChangedEventArgs<States> e)
    {
        if (SaV.TryGetValue(e.Value, out float length))
        {
            soundMaker.MakeSound(length);
        }
        else
        {
            soundMaker.MakeSound(0);
        }
    }
}
