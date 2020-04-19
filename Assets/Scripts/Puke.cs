using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puke : MonoBehaviour
{
    [SerializeField]
    GameObject[] Pukes;


    float Radius = 1.0f;
    float ScaleVariance = 0.5f;

    Vector3 origScale;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        origScale = transform.localScale;

        foreach (var puke in Pukes)
        {
            puke.transform.position = transform.position + new Vector3(Random.Range(-Radius, Radius), 0.0f, Random.Range(-Radius, Radius));
            var scale = Random.Range(1.0f - ScaleVariance, 1.0f + ScaleVariance);
            puke.transform.localScale = new Vector3(scale, scale, scale);
        }

        transform.localScale = Vector3.zero;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.magnitude < origScale.magnitude)
        {
            var scaleFactor = Mathf.Clamp((Time.time - startTime) / 0.5f, 0.0f, 1.0f);
            transform.localScale = origScale * scaleFactor;
        }
    }
}
