using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdibleManager : MonoBehaviour
{
    List<Edible> Edibles = new List<Edible>();

    private static EdibleManager INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(Edible edible)
    {
        Edibles.Add(edible);
    }

    public List<Edible> GetAll()
    {
        return Edibles;
    }

    public static EdibleManager GetInstance()
    {
        return INSTANCE;
    }

    public void Remove(Edible edible)
    {
        Edibles.Remove(edible);
    }
}
