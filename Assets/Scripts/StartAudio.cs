using UnityEngine;

public class StartAudio : MonoBehaviour
{
    public void Play(string Name)
    {
        AudioManager.PlaySound(Name);
    }
}
