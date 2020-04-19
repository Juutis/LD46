using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EdibleManager : MonoBehaviour
{
    List<Edible> Edibles = new List<Edible>();

    private static EdibleManager INSTANCE;

    public int Cakes = 0, CakesEaten;
    public int Sausages = 0, SausagesEaten;

    [SerializeField]
    Text CakesText;

    [SerializeField]
    Text SausagesText;

    public void AddCake()
    {
        Cakes++;
        UpdateHud();
    }

    public void AddCakeEaten()
    {
        CakesEaten++;
        UpdateHud();
    }

    public void AddSausage()
    {
        Sausages++;
        UpdateHud();
    }

    public void AddSausageEaten()
    {
        SausagesEaten++;
        UpdateHud();
    }
    
    private void UpdateHud()
    {
        CakesText.text = CakesEaten + " / " + Cakes;
        SausagesText.text = SausagesEaten + " / " + Sausages;
    }

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
