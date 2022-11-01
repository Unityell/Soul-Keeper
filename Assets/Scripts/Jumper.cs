using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] float Power;
    Rigidbody2D Player;

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.CompareTag("Player"))
        {
            Player = Other.GetComponent<Rigidbody2D>();
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if(Other.CompareTag("Player"))
        {
            Player = null;
        }
    }

    void Update()
    {
        if(Player)
        {
            Player.AddForce(Vector2.up * Power);
        }
    }
}
