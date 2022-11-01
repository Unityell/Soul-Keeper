using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("--------------------------------")]
    public                      TextMeshProUGUI GameScoreText;
    [Header("--------------------------------")]
    public                      TextMeshProUGUI Score;
    public                      TextMeshProUGUI BestScore;
    [Header("--------------------------------")]
    public                      RectTransform HP;
    public                      RectTransform Energy;
    [Header("--------------------------------")]
    public                      GameObject Pannel;
    public                      GameObject DeathPannel;
    public                      GameObject SaveHousePannel;
    [Header("--------------------------------")]
    public                      GameObject PausePannel;
    [Header("--------------------------------")]
    [SerializeField] private    Ability SoulStormAbility;
    [SerializeField] private    Animator RedShine;

    public void Initialization()
    {
        Subscribe();
    }

    void Subscribe()
    {
        InputManager.Signal += HUDShowHidePausePannel;
        Player.ValueSignal += HUDRefreshValue;
        Player.Signal += HUDScoreAndBestScore;
        Player.Signal += HUDShowPannel;
        Player.Signal += HUDRedDetect;
        Player.Signal += HUDStartSoulStorm;
        Player.Signal += HUDRefreshScore;
        GameManager.Signal += HUDScoreAndBestScore;
        GameManager.Signal += HUDShowPannel;
        GameManager.Signal += HUDHidePannel;
        GameManager.Signal += HUDRefreshScore;
    }
    void OnDestroy()
    {
        InputManager.Signal -= HUDShowHidePausePannel;
        Player.ValueSignal -= HUDRefreshValue;
        Player.Signal -= HUDScoreAndBestScore;
        Player.Signal -= HUDShowPannel;
        Player.Signal -= HUDRedDetect;
        Player.Signal -= HUDStartSoulStorm;
        Player.Signal -= HUDRefreshScore;
        GameManager.Signal -= HUDScoreAndBestScore;
        GameManager.Signal -= HUDShowPannel;
        GameManager.Signal -= HUDHidePannel;
        GameManager.Signal -= HUDRefreshScore;
    }

    void HUDRefreshValue(string NewMessage, float NewValue)
    {
        switch (NewMessage)
        {
            case "RefreshHp" : HP.sizeDelta = new Vector2(NewValue, 40); break;
            case "RefreshEnergy" : Energy.sizeDelta  = new Vector2(NewValue, 40); break;
            default: break;
        }
    }

    void HUDScoreAndBestScore(string NewMessage)
    {
        if(NewMessage == "RefreshScore")
        {
            Score.text = GameManager.GameScore.ToString();
            BestScore.text = PlayerPrefs.GetInt("BestScore").ToString();
            if(PlayerPrefs.GetInt("BestScore") < GameManager.GameScore)
            {
                PlayerPrefs.SetInt("BestScore", GameManager.GameScore);
                PlayerPrefs.Save();
                BestScore.text = PlayerPrefs.GetInt("BestScore").ToString();
            } 
        }
    }

    void HUDShowPannel(string NewMessage)
    {
        switch (NewMessage)
        {
            case "Death" : 
                InputManager.IsEnabled = false;
                Pannel.SetActive(true); 
                DeathPannel.SetActive(true); 
                SaveHousePannel.SetActive(false);
                HUDScoreAndBestScore("RefreshScore");
                Time.timeScale = 0;
                break;
            case "Save" : 
                InputManager.IsEnabled = false; 
                Pannel.SetActive(true); 
                DeathPannel.SetActive(false);
                SaveHousePannel.SetActive(true);
                HUDScoreAndBestScore("RefreshScore");
                Time.timeScale = 0;
                break;
            default: break;
        }
    }
    
    void HUDHidePannel(string NewMessage)
    {
        if(NewMessage == "HidePannel")
        {
            Pannel.SetActive(false);
            AudioListener.pause = AudioManager.CheckSoundState;
            InputManager.IsEnabled = true;
        }
    }

    void HUDRefreshScore(string NewMessage)
    {
        if(NewMessage == "RefreshScore")
        {
            GameScoreText.text = "Score:  " + GameManager.GameScore;
        }
    }

    void HUDShowHidePausePannel(string NewMessage)
    {
        if(NewMessage == "ShowPausePannel")
        {
            InputManager.IsEnabled = false;
            PausePannel.SetActive(true);
            AudioListener.pause = true;
            Time.timeScale = 0;
        }
        if(NewMessage == "HidePausePannel")
        {
            InputManager.IsEnabled = true;
            PausePannel.SetActive(false);
            AudioListener.pause = AudioManager.CheckSoundState;
            Time.timeScale = 1;
        }
    }
    void HUDStartSoulStorm(string NewMessage)
    {
        if(NewMessage == "Ability")
        {
            SoulStormAbility.StartSoulStorm();
        }
    }
    void HUDRedDetect(string NewMessage)
    {
        if(NewMessage == "RedDetect")
        {
            RedShine.Play("RedShine", 0, 0);
        }
    }
}
