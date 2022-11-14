using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Joystic : MonoBehaviour
{
    void Start()
    {
        if(YG.YandexGame.EnvironmentData.isDesktop)
        {
            gameObject.SetActive(false);
        }
    }
}
