using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAScript : MonoBehaviour {

    [SerializeField] float speed = 1f;
    [SerializeField] Transform leftSensor, rightSensor;

    Ray rayon;
    RaycastHit hit;

	void FixedUpdate () {

        rayon = new Ray(leftSensor.position, transform.TransformDirection(Vector3.forward));

        if(Physics.Raycast(rayon, out hit, Mathf.Infinity))
        {
            Debug.Log("left sensor object : " + hit.collider.name + " distance : " + hit.distance);
            if(hit.distance < 0.4)
            {
                float angle = Random.Range(100f, 300f);
                transform.Rotate(Vector3.up * angle);
            }
        }

        Debug.DrawRay(leftSensor.position, transform.TransformDirection(Vector3.forward) * 10f, Color.black);


        rayon = new Ray(rightSensor.position, transform.TransformDirection(Vector3.forward));

        if (Physics.Raycast(rayon, out hit, Mathf.Infinity))
        {
            Debug.Log("right sensor object : " + hit.collider.name + " distance : " + hit.distance);
            if (hit.distance < 0.4)
            {
                float angle = Random.Range(100f, 300f);
                transform.Rotate(Vector3.up * angle);
            }
        }

        Debug.DrawRay(rightSensor.position, transform.TransformDirection(Vector3.forward) * 10f, Color.black);




        transform.Translate(Vector3.forward * speed * Time.deltaTime);

	}
}
