using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    void Start()
    {
        Player.Signal += ShakeCamera;
    }
    void OnDestroy()
    {
        Player.Signal -= ShakeCamera;
    }
    void ShakeCamera(string NewMessage)
    {
        if(NewMessage == "Kick")
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Camera Cam = Camera.main;
        for (int i = 0; i < 3; i++)
        {
            transform.position += Vector3.up * 0.05f;
            yield return new WaitForSeconds(0.02f);
            transform.position += Vector3.down * 0.05f;
            yield return new WaitForSeconds(0.02f);  
        }
    }
}
