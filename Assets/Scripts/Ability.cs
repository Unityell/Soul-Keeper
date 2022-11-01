using UnityEngine;

public class Ability : MonoBehaviour
{
    private                     ParticleSystem AnimParticle;
    private                     Animator Animator;
    [SerializeField] private    Player Player;
    [SerializeField] private    LayerMask Enemy;
    private                     bool Check;
    
    void Start()
    {
        AnimParticle = GetComponentInChildren<ParticleSystem>();
        Animator = GetComponent<Animator>();
    }

    public void StartSoulStorm()
    {
        AudioManager.PlayEnvironment("SpellSound", true);
        Player.GodMode = true;
        Animator.Play("SoulsStorm", 0, 0);
        Check = true;
    }

    public void Play()
    {
        AnimParticle.Play();
    }

    public void Stop()
    {
        AudioManager.PlayEnvironment("SpellSound", false);
        Check = false;
        AnimParticle.Stop();
        Player.GodMode = false;
        Animator.Play("Stand");
    }

    void Update()
    {
        if(Check)
        {
            RaycastHit2D Hit = Physics2D.Raycast(transform.position, Vector2.right, 15, Enemy);
            if(Hit.collider)
            {
                Player.KickEnemy(Hit.collider.gameObject);
            }
        }
    }
}
