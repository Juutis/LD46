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
    Animator anim;

    Quaternion origRootRotation;

    private float munchTimer = 0.0f;

    float eatTimer = 0.0f;

    [SerializeField]
    AudioClip[] munches;

    // Start is called before the first frame update
    void Start()
    {
        dog = GetComponent<Dog>();
        anim = GetComponentInChildren<Animator>();
        origRootRotation = RootBone.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (dog.Alive)
        {
            Edible closestEdible = GetClosestEdible();


            if (closestEdible != null)
            {
                if (DistanceTo(closestEdible) < 20f)
                {
                    if (munchTimer < Time.time)
                    {
                        dog.PlayRandomSound(munches);
                        munchTimer = Time.time + 0.2f + Random.Range(0.0f, 0.1f);
                    }

                    anim.SetLayerWeight(anim.GetLayerIndex("Chomp"), 1.0f);
                    anim.SetLayerWeight(anim.GetLayerIndex("Sniff"), 0.0f);

                    var targetDir = closestEdible.transform.position - RootBone.position;

                    float targetAngle = Vector3.Angle(targetDir, Limiter.forward);
                    if (targetAngle > MaxAngle)
                    {
                        targetDir = Vector3.RotateTowards(Limiter.forward, targetDir, Mathf.Deg2Rad * MaxAngle, 100.0f);
                    }

                    var currentDir = -RootBone.right;

                    var diffHoriz =- Vector2.SignedAngle(new Vector2(currentDir.x, currentDir.z), new Vector2(targetDir.x, targetDir.z));
            
                    if (Mathf.Abs(diffHoriz) > 0.1f)
                    {
                        RootBone.Rotate(Vector3.up, diffHoriz, Space.World);
                    }

                    var diffVert = Vector3.SignedAngle(-RootBone.right, targetDir, RootBone.up);

                    if (Mathf.Abs(diffVert) > 0.1f)
                    {
                        RootBone.Rotate(Vector3.up, diffVert, Space.Self);
                    }

                    var eatRange = closestEdible.UseExtendedEatRange ? 6.0f : EatRange;

                    if (Vector3.Distance(MouthBone.position, closestEdible.transform.position) < eatRange)
                    {
                        if (eatTimer < Time.time)
                        {
                            dog.Eat(closestEdible);
                            eatTimer = Time.time + 0.2f;
                            munchTimer = Time.time + 0.2f;
                        }
                    }
                }
                else
                {
                    anim.SetLayerWeight(anim.GetLayerIndex("Chomp"), 0.0f);
                    anim.SetLayerWeight(anim.GetLayerIndex("Sniff"), 1.0f);
                    RootBone.localRotation = origRootRotation;
                }
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
