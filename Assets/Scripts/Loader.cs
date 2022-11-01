using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    void Start()
    {
        InputManager.Signal += LoadLevel;
    }
    void OnDestroy()
    {
        InputManager.Signal -= LoadLevel;
    }
    void LoadLevel(string NewMessage)
    {
        switch (NewMessage)
        {
            case "Menu" : 
                Time.timeScale = 1;
                AudioListener.pause = AudioManager.CheckSoundState;
                GameManager.GameScore = 0;
                SceneManager.LoadScene(NewMessage); 
                break;
            case "Restart" :
                Time.timeScale = 1;
                GameManager.GameScore = 0;
                AudioListener.pause = AudioManager.CheckSoundState;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            default: break;
        }
    }
}
