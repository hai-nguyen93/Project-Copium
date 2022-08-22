using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    private GameController gc;

    public MenuStatusPanel statusPanel;
    public MenuAbilityPanel menuAbilityPanel;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
        ClearAllPanels();
    }

    public void ShowStatusPanel()
    {
        ClearAllPanels();
        statusPanel.gameObject.SetActive(true);
    }

    public void ShowAbilityPanel()
    {
        ClearAllPanels();
        menuAbilityPanel.gameObject.SetActive(true);
    }

    public void ClearAllPanels()
    {
        statusPanel.gameObject.SetActive(false);
        menuAbilityPanel.gameObject.SetActive(false);
    }

    public void Resume()
    {
        gc.ResumeGame();
    }

    public void Quit()
    {
        gc.QuitGame();
    }
}
