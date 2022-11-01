using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float MoveSpeed;
    public float GameSpeed;
    bool Init;

    public void Initialization(float GameSpeed, float MoveSpeed)
    {
        this.GameSpeed = GameSpeed;
        this.MoveSpeed = MoveSpeed;
        Init = true;
    }

    void Update()
    {
        if(Init)
        {
            transform.position -= Vector3.right * GameSpeed* MoveSpeed * Time.deltaTime;
        }
    }
}
