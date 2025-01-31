using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private Transform transitionObj;

    private int transitionLevel;

    [Header("Start Variables")]
    [SerializeField] private Animator animator;

    public void StartTransition(int level)
    {
        animator.Play("Base Layer.TransitionStart");
        transitionLevel = level;
    }

    public void EndTransition()
    {
        animator.Play("Base Layer.TransitionEnd");
    }

    public void AnimationComplete()
    {
        transitionObj.transform.position = Vector3.zero;
        PlayerManager.playerManager.levelManager.AnimationComplete(transitionLevel);
        transitionLevel = 0;
    }

    public void EndTransitionFinished()
    {
        PlayerManager.playerManager.StartGameplay();
    }
}
