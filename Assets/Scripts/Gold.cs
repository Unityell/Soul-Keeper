using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] int GoldCount;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.CompareTag("Player"))
        {
            GameManager.GameScore += GoldCount;
            AudioManager.PlaySound("GoldSound");
            VFXManager.SpawnFloatingText(transform.position + Vector3.up, "+" + GoldCount, 0.5f);
            Destroy(gameObject);
        }
    }
}
