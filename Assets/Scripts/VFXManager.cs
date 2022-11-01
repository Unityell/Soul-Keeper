using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VFXManager : MonoBehaviour
{
    public      List<FX> VFX;
    public      TextMeshPro  FloatingText;
    static      TextMeshPro ShadowFloatingText;
    static      List<FX> ShadowVFX;

    void Start()
    {
        ShadowVFX = VFX;
        ShadowFloatingText = FloatingText;
    }

    public static void SpawnFloatingText(Vector3 Position, string Text, float DestroyTimer)
    {
        var NewFloatingText = Instantiate(ShadowFloatingText, Position, Quaternion.identity);
        NewFloatingText.text = Text;
        Destroy(NewFloatingText.gameObject, DestroyTimer);
    }

    public static void SpawnVFX(string Name, Vector3 Position)
    {
        for (int i = 0; i < ShadowVFX.Count; i++)
        {
            if(ShadowVFX[i].Name == Name)
            {
                var NewVFX = Instantiate(ShadowVFX[i].VFX, Position, Quaternion.identity);
            }
        }
    }
}

[System.Serializable]
public class FX
{
    public string Name;
    public Animator VFX;
}
