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

    float InputX, InputY;

    Rigidbody rb;
    Animator anim;

    float Hunger = 90;

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
    }

    // Update is called once per frame
    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

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

        if (rb.velocity.magnitude > 0.1f)
        {
            var horizDiff = Vector3.SignedAngle(Model.forward, rb.velocity, Vector3.up);
            Model.Rotate(Vector3.up, horizDiff);
            anim.SetFloat("LegSpeed", rb.velocity.magnitude / MoveSpeed);
        } else
        {
            anim.SetFloat("LegSpeed", 0.0f);
        }

        Debug.DrawLine(Model.transform.position, Model.transform.position + Model.transform.forward);

    }

    void FixedUpdate()
    {
        rb.velocity = MoveSpeed * new Vector3(InputX, 0.0f, InputY);
    }

    public void Die()
    {

    }

    public void Eat(Edible edible)
    {
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
        Hunger -= 20f;
        Instantiate(VomitEffect, VomitRoot);
    }
}
