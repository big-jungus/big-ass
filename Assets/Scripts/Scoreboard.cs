using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text bigCoin;
    [SerializeField] private TMP_Text smallCoin;

    [SerializeField] private Animator animator;

    private int popCounter = 0;

    public void Show()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(PlayerManager.playerManager.playerStats.currentLevelTimer);
        timer.text = timeSpan.ToString(format: @"mm\:ss\:ff");

        bigCoin.text = "x " + PlayerManager.playerManager.playerStats.bigCoinCount.ToString();
        smallCoin.text = "x " + PlayerManager.playerManager.playerStats.smallCoinCount.ToString();

        animator.Play("Base Layer.Scoreboard");
    }

    public void ScoreboardAnimationFinished()
    {
        Time.timeScale = 0f;
        animator.enabled = false;
    }

    public void PlayPopSound()
    {
        PlayerManager.playerManager.soundManager.ScoreboardSound(popCounter);
        popCounter++;
    }
}
