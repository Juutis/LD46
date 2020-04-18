using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    [SerializeField]
    public float HungerRestored = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        EdibleManager.GetInstance().Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        EdibleManager.GetInstance().Remove(this);
    }

    public void Eat()
    {
        Destroy(gameObject);
    }

    public bool isPoison()
    {
        return HungerRestored < 0.0f;
    }
}
