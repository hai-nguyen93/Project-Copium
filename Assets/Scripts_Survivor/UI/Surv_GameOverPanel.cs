using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Surv_GameOverPanel : MonoBehaviour
{
    [Header("Result elements")]
    public GameObject lossPrompt;
    public GameObject winPrompt;
    public ParticleSystem winPS;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killCountText;

    [Header("Animations")]
    public Animator imageAnim;

    public void SetupResult(float time, int killCount, bool win = false)
    {
        SetupInfo(time, killCount);
        if (win)
        {
            lossPrompt.SetActive(false);
            winPrompt.SetActive(true);
            imageAnim.Play("game_win");

            if (winPS)
            {
                winPS.gameObject.SetActive(true);
                winPS.Play();
            }
            
        }
        else
        {
            lossPrompt.SetActive(true);
            winPrompt.SetActive(false);
            imageAnim.Play("game_loss");
        }
    }

    public void SetupInfo(float time, int killCount)
    {
        timeText.text = string.Format("Time: {0}:{1:D2}", (int)(time / 60), (int)(time % 60));
        killCountText.text = "Kill Count: " + killCount;
    }
}
