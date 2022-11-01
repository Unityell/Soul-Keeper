using UnityEngine;

public class SaveHouse : MonoBehaviour
{
    [SerializeField] GameManager GM;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.CompareTag("Player"))
        {
            GM.IntoSaveHouse();
        }
    }
}
