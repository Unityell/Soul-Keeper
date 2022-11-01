using UnityEngine;

public class WheatherGen : MonoBehaviour
{
    [SerializeField] private    ParticleSystem[] SkyEffects;
    [SerializeField] private    Animator RedMoon;
    [SerializeField] private    float MinRandomTime;
    [SerializeField] private    float MaxRandomTime;

    void Start()
    {
        Invoke(nameof(WheaterGeneration), 1);
    }

    void WheaterGeneration()
    {
        int Rand = Random.Range(0,6);
        switch (Rand)
        {
            case 0 : ClearMoonInAnim(); ClearSkyEffect(); Rain(); break;
            case 1 : ClearSkyEffect(); BloodRain(); break;
            case 2 : ClearSkyEffect(); BloodRain(); RedMoonEffect(); break;
            case 3 : ClearSkyEffect(); break;
            case 4 : ClearMoonInAnim(); break;
            case 5 : RedMoonEffect(); break;
            default: break;
        }
        Invoke(nameof(WheaterGeneration), Random.Range(MinRandomTime, MaxRandomTime));
    }

    void ClearSkyEffect()
    {
        for (int i = 0; i < SkyEffects.Length; i++)
        {
            SkyEffects[i].Stop();
        }
        AudioManager.PlayEnvironment("RainSound" , false);
    }

    void ClearMoonInAnim()
    {
        if(RedMoon.GetComponent<SpriteRenderer>().color.a != 0)
        {
            RedMoon.Play("RedMoonOff", 0 ,0);
        }
    }

    void ClearMoon()
    {
        RedMoon.Play("RedMoonClear", 0 ,0);
    }

    void Rain()
    {
        SkyEffects[0].Play();
        AudioManager.PlayEnvironment("RainSound" , true);
    }

    void BloodRain()
    {
        SkyEffects[1].Play();
        AudioManager.PlayEnvironment("RainSound" , true);
    }
    void RedMoonEffect()
    {
        if(RedMoon.GetComponent<SpriteRenderer>().color.a <= 0)
        {
            RedMoon.Play("RedMoonOn", 0 ,0);
            AudioManager.PlaySound("WolfSound");
        }
    }
}
