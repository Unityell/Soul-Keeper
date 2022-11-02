using UnityEngine;
using YG;

public class Player : MonoBehaviour
{
    /////////////////////////////////////////////////////////////
    public delegate             void InputMessage(string Message);
    public static               event InputMessage Signal;
    /////////////////////////////////////////////////////////////
    public delegate             void ValueMessage(string Message, float Value);
    public static               event ValueMessage ValueSignal;
    /////////////////////////////////////////////////////////////
    private                     int Health = 100;
    private                     float Energy = 0;
    [SerializeField] private    float EnergyPerSecond = 1f;

    public                      bool GodMode;
    private                     bool EvilHandNear;
    private                     bool CheckKick;
    private                     bool Drop;
    private                     Rigidbody2D RB;
    [SerializeField] private    float JumpForce;
    private                     Animator Anim;
    [SerializeField] private    Transform LegOfDeath;
    [SerializeField] private    LayerMask WhatIsGround;
    [SerializeField] private    Transform GroundCheckPoint;

    void Start()
    {
        FindComponent();
        SubscribeOnInputManager();
        EnergyRecover();
    }

    public void EmitSignal(string NewMessage)
    {
        Signal?.Invoke(NewMessage);
    }

    public void EmitValueSignal(string NewMessage, float Value)
    {
        ValueSignal?.Invoke(NewMessage, Value);
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.CompareTag("EvilHand"))
        {
            EvilHandNear = true;
            AudioManager.PlayEnvironment("ScreamSound", true);
            MoreBlood();
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if(Other.CompareTag("EvilHand"))
        {
            EvilHandNear = false;
            AudioManager.PlayEnvironment("ScreamSound", false);
        }
    }

    void FindComponent()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    void SubscribeOnInputManager()
    {
        InputManager.Signal += Jump;
        InputManager.Signal += Kick;
        InputManager.Signal += SoulStorm;
        InputManager.Signal += AddHp;
        InputManager.Signal += AddEnergy;
    }
    void OnDestroy()
    {
        InputManager.Signal -= Jump;
        InputManager.Signal -= Kick;
        InputManager.Signal -= SoulStorm;
        InputManager.Signal -= AddHp;
        InputManager.Signal -= AddEnergy;
    }
    public void AddHp(string NewMessage)
    {
        if(NewMessage == "AddHp")
        {
            if(Health < 100 && GameManager.GameScore >= 100)
            {
                GameManager.GameScore -= 100;
                Health += 20;
                if(Health > 100) Health = 100;
                EmitValueSignal("RefreshHp", Health);
                EmitSignal("RefreshScore");
            }
        }
    }
    void AddEnergy(string NewMessage)
    {
        if(NewMessage == "AddEnergy")
        {
            if(GameManager.GameScore >= 200 && EnergyPerSecond < 5)
            {
                GameManager.GameScore -= 200;
                EnergyPerSecond += 0.5f;
                EmitSignal("RefreshScore");
            }
        }
    }

    void Jump(string Message)
    {
        if(Message == "Jump" && IsGroundedCheck())
        {
            AudioManager.PlaySound("JumpSound");
            RB.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); 
        }
    }

    public void SoulStorm(string NewMessage)
    {
        if(NewMessage == "Ability")
        {
            if(Energy >= 100)
            {
                EmitSignal("Ability");
                AudioManager.PlayEnvironment("SpellSound", true);
                Energy -= 100;
            }
            else
            {
                EmitSignal("RedDetect");
            }
        }
    }

    void Kick(string Message)
    {
        if(Message == "Kick" && !CheckKick)
        {
            Anim.SetInteger("State", 2);
            Anim.Play("Kick", 0, 0);
            CheckKick = true;
            AudioManager.PlaySound("KickSound"); 
        }
    }

    void KickOff()
    {
        CheckKick = false;
    }

    void MoreBlood()
    {
        if(!GodMode && EvilHandNear)
        {
            Health -= 5;
            EmitValueSignal("RefreshHp", Health);
            VFXManager.SpawnVFX("BloodVFX", transform.position);
            AudioManager.PlaySound("FleshSound");
            if(Health <= 0)
            {
                Death();
            }
            Invoke(nameof(MoreBlood), 0.45f);
        }
    }

    void Leaderboard()
    {
        
    }
    void Death()
    {
        Health = 0;
        Time.timeScale = 0;
        AudioListener.pause = true;
        YandexGame.FullscreenShow();
        EmitSignal("Death");
    }   
    void EnergyRecover()
    {
        if(Energy < 100)
        {
            Energy += EnergyPerSecond;
            EmitValueSignal("RefreshEnergy", Energy);
        }
        Invoke(nameof(EnergyRecover), 1f);
    }

    public void SetSpeed(float NewSpeed)
    {
        Anim.SetFloat("Speed", NewSpeed/6);
    }

    public void RigidKick(Rigidbody2D RB)
    {
        RB.bodyType = RigidbodyType2D.Dynamic;
        RB.AddForce((Vector2.right + Vector2.up) * 1000);
        RB.AddTorque(-1000);
    }

    public void KickEnemy(GameObject NewEnemy)
    {
        if(NewEnemy.CompareTag("Enemy"))
        {
            var Collide = NewEnemy.GetComponent<Collider2D>();
            if(Collide.enabled)
            {
                RigidKick(NewEnemy.GetComponent<Rigidbody2D>());
                GameManager.GameScore += 10;
                VFXManager.SpawnFloatingText(NewEnemy.transform.position + Vector3.up * 2f, "+10", 0.5f);
                VFXManager.SpawnVFX("KickVFX", Collide.ClosestPoint(LegOfDeath.position));
                Collide.enabled = false;
                AudioManager.PlaySound("GoldSound");
            }
        }
    }
    void Bash()
    {
        EmitSignal("Kick");
        Collider2D[] AllCollider = Physics2D.OverlapCircleAll(LegOfDeath.position, 0.05f);
        if(AllCollider != null)
        {
            for (int i = 0; i < AllCollider.Length; i++)
            {
                KickEnemy(AllCollider[i].gameObject);
            }
        }
    }

    bool IsGroundedCheck()
    {
        return Physics2D.OverlapCircle(GroundCheckPoint.position, 0.02f, WhatIsGround);
    }

    void Update()
    {
        if(IsGroundedCheck())
        {
            Drop = true;
            if(transform.position.x > -3.75f)
            {
                RB.velocity = new Vector2(0, RB.velocity.y);
            }
            else
            {
                RB.AddForce(Vector2.right * 550 * Time.deltaTime);
            }
            if(!CheckKick)
            {
                Anim.SetInteger("State", 0);
            }
        }
        else
        {
            if(!CheckKick)
            {
                Anim.SetInteger("State", 1);
            }
            if(Drop && RB.velocity.y < 0)
            {
                AudioManager.PlaySound("FallSound");
                Drop = false;
            }
        }
    }
}
