using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * NOTES
 * Make sure that nearByObjects has objects in it.  Check in the Vehicle.cs to make sure that it is adding objects to the list upon Trigger.
 * This is currently based on it not being food or poison 
 */

[RequireComponent(typeof(Rigidbody2D))]
public class Separation : MonoBehaviour {

    public float sepMaxAcc = 25f;
    public float maxSepDist = 1f;
    // Use this for initialization
    void Start () {
		
	}

    public Vector3 Separate(List<GameObject> nearByObjects)
    {
        //gets the radius of the collider
        //float boundRadius = Mathf.Max(sphereCollider.transform.localScale.x, sphereCollider.transform.localScale.y, sphereCollider.transform.localScale.z) * sphereCollider.radius;
        //Debug.Log(boundRadius);
        //Debug.Log(nearByObjects.Count);
        Vector3 sepAcceleration = Vector3.zero; //zero it out just in case
        int targetCount = 0;

        nearByObjects = checkForNulls(nearByObjects);

        //adds all th nearby objects accelerations together and returns them
        foreach (GameObject obj in nearByObjects)
        {

            Vector3 direction = transform.position - obj.transform.position;

            //float dist = direction.magnitude;
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            //Debug.Log(dist);

            //check if inside the personal bubble
            if (dist > 0 && dist < maxSepDist)
            {
                //SphereCollider objSphCollider = obj.GetComponent<SphereCollider>();
                //float targetObjRadius = Mathf.Max(objSphCollider.transform.localScale.x, objSphCollider.transform.localScale.y, objSphCollider.transform.localScale.z) * objSphCollider.radius;

                //how much force is needed to separate the two objects
                //float sepStrength = sepMaxAcc * (maxSepDist - dist) / (maxSepDist);
                //float sepStrength = Mathf.Min(10f / (dist * dist), sepMaxAcc);
                targetCount++;
                direction.Normalize();
                direction /= dist;
                sepAcceleration += direction;// * sepStrength;
            }
        }

        if(targetCount > 0)
        {
            sepAcceleration /= targetCount;
            sepAcceleration.Normalize();
            sepAcceleration *= sepMaxAcc;
        }
        return sepAcceleration;

    }

    public List<GameObject> checkForNulls(List<GameObject> nearByObjects)
    {
        foreach (GameObject obj in nearByObjects)
        {
            if (obj == null)
            {
                nearByObjects.Remove(obj);
            }
        }

        return nearByObjects;
    }
}
