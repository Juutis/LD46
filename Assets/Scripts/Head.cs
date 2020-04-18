using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{

    [SerializeField]
    Transform RootBone;

    [SerializeField]
    Transform MouthBone;

    [SerializeField]
    Transform Limiter;

    [SerializeField]
    float MaxAngle = 30.0f;

    [SerializeField]
    float EatRange = 0.5f;

    Dog dog;

    // Start is called before the first frame update
    void Start()
    {
        dog = GetComponent<Dog>();
    }

    // Update is called once per frame
    void Update()
    {
        Edible closestEdible = GetClosestEdible();

        if (closestEdible != null)
        {
            var targetDir = closestEdible.transform.position - RootBone.position;

            float targetAngle = Vector3.Angle(targetDir, Limiter.forward);
            Debug.DrawLine(Limiter.position, Limiter.position + Limiter.forward * 10);
            if (targetAngle > MaxAngle)
            {
                targetDir = Vector3.RotateTowards(Limiter.forward, targetDir, Mathf.Deg2Rad * MaxAngle, 100.0f);
                Debug.Log(targetAngle);
            }

            Debug.Log(targetDir);

            var currentDir = -RootBone.right;

            var diffHoriz =- Vector2.SignedAngle(new Vector2(currentDir.x, currentDir.z), new Vector2(targetDir.x, targetDir.z));
            
            RootBone.Rotate(Vector3.up, diffHoriz, Space.World);

            var diffVert = Vector3.SignedAngle(-RootBone.right, targetDir, RootBone.up);
        
            RootBone.Rotate(Vector3.up, diffVert, Space.Self);
        
            if (Vector3.Distance(MouthBone.position, closestEdible.transform.position) < EatRange)
            {
                dog.Eat(closestEdible);
            }
        }
    }

    private Edible GetClosestEdible()
    {
        List<Edible> edibles = EdibleManager.GetInstance().GetAll();
        Edible closestEdible = null;
        foreach (var edible in edibles)
        {
            if (closestEdible == null || DistanceTo(edible) < DistanceTo(closestEdible))
            {
                closestEdible = edible;
            }
        }
        return closestEdible;
    }

    private float DistanceTo(Edible edible)
    {
        return Vector3.Distance(RootBone.position, edible.transform.position);
    }
}
