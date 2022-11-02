using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class ScenaLoader : MonoBehaviour
{
    [SerializeField] private GameObject ButtonOn;
    [SerializeField] private GameObject ButtonOff;

    public void Start()
    {
        YandexGame.FullscreenShow();
        if(AudioListener.pause)
        {
            ButtonOn.SetActive(false);
            ButtonOff.SetActive(true);
        }
        else
        {
            ButtonOn.SetActive(true);
            ButtonOff.SetActive(false);
        }
    }
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void SoundOn()
    {
        AudioListener.pause = false;
    }   

    public void SoundOff()
    {
        AudioListener.pause = true;
    }
}
