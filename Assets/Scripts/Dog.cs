using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField]
    Transform Body;

    [SerializeField]
    Transform Model;

    [SerializeField]
    float MoveSpeed;

    [SerializeField]
    float HungerDecayRate = 1.0f;

    [SerializeField]
    float MaxHunger = 150.0f;

    [SerializeField]
    GameObject VomitEffect;

    [SerializeField]
    Transform VomitRoot;

    [SerializeField]
    DogUI dogUI;
    
    AudioSource audioSrc;

    [SerializeField]
    AudioClip[] eats;

    [SerializeField]
    AudioClip[] pukes;

    [SerializeField]
    AudioClip[] deaths;

    [SerializeField]
    AudioClip[] footSteps;

    [SerializeField]
    bool ShowTutorial = true;

    float InputX, InputY;

    Rigidbody rb;
    Animator anim;

    float Hunger = 90;

    public bool Alive = true;

    Head head;

    float footStepTimer = 0f;

    float TurnSpeed = 720f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        anim.Play("Idle", anim.GetLayerIndex("Base Layer"));
        anim.Play("Leg1", anim.GetLayerIndex("Leg1"));
        anim.Play("Leg2", anim.GetLayerIndex("Leg2"));
        anim.Play("Leg3", anim.GetLayerIndex("Leg3"));
        anim.Play("Leg4", anim.GetLayerIndex("Leg4"));

        if (ShowTutorial)
        {
            Invoke("LoveSausages", 3.0f);
            Invoke("LoveCakes", 10.0f);
        }

        audioSrc = GetComponent<AudioSource>();
    }

    public void LoveSausages()
    {
        dogUI.ShowSausageLove();
    }

    public void LoveCakes()
    {
        dogUI.ShowCakeLove();
    }

    // Update is called once per frame
    void Update()
    {
        if (Alive)
        {
            InputX = HybridInput.GetAxis("Horizontal");
            InputY = HybridInput.GetAxis("Vertical");

            var input = new Vector3(InputX, 0.0f, InputY);

            if (Hunger > 0.0f)
            {
                Hunger -= Time.deltaTime * HungerDecayRate;
            }
            else
            {
                Die();
            }
            float bodyScale = Hunger / 100f + 0.1f;
            Body.localScale = new Vector3(bodyScale, bodyScale, bodyScale);

            var horizSpeed = input;
            horizSpeed.y = 0.0f;

            if (horizSpeed.magnitude > 0.1f)
            {
                var horizDiff = Vector3.SignedAngle(Model.forward, horizSpeed, Vector3.up);
                float amount = Mathf.Sign(horizDiff) * Mathf.Min(Mathf.Abs(horizDiff), TurnSpeed * Time.deltaTime);
                Model.Rotate(Vector3.up, amount);
                anim.SetFloat("LegSpeed", Mathf.Clamp(horizSpeed.magnitude, 0.0f, 1.0f));

                if (footStepTimer < Time.time)
                {
                    PlayRandomSound(footSteps);
                    footStepTimer = Time.time + 0.15f + Random.Range(0.0f, 0.1f);
                }
            }
            else
            {
                anim.SetFloat("LegSpeed", 0.0f);
            }
        }

    }

    void FixedUpdate()
    {
        var vertVelocity = rb.velocity.y;
        var moveDir = new Vector3(InputX, 0.0f, InputY);
        if (moveDir.magnitude > 1.0f)
        {
            moveDir.Normalize();
        }
        rb.velocity = MoveSpeed * moveDir + new Vector3(0f, vertVelocity, 0f);
    }

    public void Die()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Leg1"), 0.0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Leg2"), 0.0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Leg3"), 0.0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Leg4"), 0.0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Sniff"), 0.0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Chomp"), 0.0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Root"), 0.0f);
        anim.SetBool("Alive", false);
        InputX = 0.0f;
        InputY = 0.0f;
        Alive = false;
        PlayRandomSound(deaths);
        Invoke("ShowDeath", 1.0f);
    }

    public void ShowWin()
    {
        dogUI.ShowWin();
    }

    public void ShowDeath()
    {
        dogUI.ShowDead();
    }

    public void Eat(Edible edible)
    {
        if (!edible.DestroyOnEat)
        {
            ShowWin();
        }

        PlayRandomSound(eats);
        edible.Eat();

        if (!edible.isPoison())
        {
            Hunger += edible.HungerRestored;
        }
        else
        {
            Invoke("Poisoned", 0.5f);
            Invoke("Poisoned", 0.9f);
        }

        if (Hunger > MaxHunger)
        {
            Hunger = MaxHunger;
        }
    }

    public void Poisoned()
    {
        Hunger -= 15f;
        Instantiate(VomitEffect, VomitRoot);
        PlayRandomSound(pukes);
        GameObject puke = Instantiate(PukePrefab);
        var pos = VomitRoot.position;
        pos.y = 0.3f;
        puke.transform.position = pos;
    }

    public void PlayRandomSound(AudioClip[] clips)
    {
        audioSrc.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    
    [SerializeField]
    GameObject PukePrefab;
}
