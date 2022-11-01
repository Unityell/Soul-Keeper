using UnityEngine;

public class EvilHand : MonoBehaviour
{
    [SerializeField] private    Transform Player;
    private                     AudioSource ZombieSounds;
    void Start()
    {
        ZombieSounds = AudioManager.GetAudioPlayer("ZombieSound");
    }
    void FixedUpdate()
    {
        if(ZombieSounds)
        {
            if(!AudioListener.pause)
            {
                float Procent = 1 - ((Vector2.Distance(Player.position, transform.position) / (4.7f / 100))  / 100);

                if(Procent < 0)
                {
                    Procent = 0;
                }

                ZombieSounds.volume = Procent;
            }
        }
    }
}
