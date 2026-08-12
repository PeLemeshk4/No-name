using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager
{
    private Animator animator;
    private StateManager stateManager;

    public AnimationManager(Animator anim, StateManager sM)
    {
        animator = anim;
        stateManager = sM;

        stateManager.StateChanged += UpdateAnimator;
    }

    private void UpdateAnimator(object o, ValueChangedEventArgs<States> e)
    {
        animator.SetBool(e.PreviosValue.ToString(), false);
        animator.SetBool(e.Value.ToString(), true);
    }
}
