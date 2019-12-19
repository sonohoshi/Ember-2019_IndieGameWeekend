    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTitle : MonoBehaviour
{
    public GameObject settingPanel;
    public Slider masterSlider;
    public Slider backgroundSlider;
    public Slider effectSlider;
    public GameObject slot;

    private void Awake()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayBackground("MainTitle");
        masterSlider.value = SoundManager.Instance.GetMasterVolume();
        backgroundSlider.value = SoundManager.Instance.GetBackgroundVolume();
        effectSlider.value = SoundManager.Instance.GetEffectVolume();
    }
    public void GameReady(bool isShow)
    {
        slot.SetActive(isShow);
    }
    public void GameStart()
    {
        SoundManager.Instance.PlayBackground("GameBackground");
        SceneManager.LoadScene(1);
    }
    public void GameSetting(bool isShow)
    {
        settingPanel.SetActive(isShow);
    }
    public void GameQuit()
    {
        Application.Quit();
    }

    public void SetMasterVolume()
    {
        SoundManager.Instance.SetMasterVolume(masterSlider.value);
    }
    public void SetBackgroundVolume()
    {
        SoundManager.Instance.SetBackgroundVolume(backgroundSlider.value);
    }
    public void SetEffectVolume()
    {
        SoundManager.Instance.SetEffectVolume(effectSlider.value);

    }

   public void ExitGame()
    {
        Application.Quit(0);
    }
}
