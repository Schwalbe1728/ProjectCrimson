using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    private Transform lookAtPosition;

    [SerializeField]
    private float MinDistance = 5;

    [SerializeField]
    private float MaxDistance = 7;

    private Vector3 CameraPosition;

	// Use this for initialization
	void Start () {
        CameraPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float dist = Vector3.Distance(lookAtPosition.position, transform.position);
        bool smallerThanMin = dist < MinDistance;
        bool greaterThanMax = dist > MaxDistance;

        CameraPosition.x = lookAtPosition.position.x;

        if(smallerThanMin)
        {
            float dx = Mathf.Pow(lookAtPosition.position.x - transform.position.x, 2);
            float dy = Mathf.Pow(lookAtPosition.position.y - transform.position.y, 2);

            float newCamZ = lookAtPosition.position.z - Mathf.Sqrt(MinDistance * MinDistance - (dx + dy));

            if (!float.IsNaN(newCamZ))
                CameraPosition.z = newCamZ;
                    //transform.SetPositionAndRotation(new Vector3(lookAtPosition.position.x, transform.position.y, newCamZ), new Quaternion());
        }

        if (greaterThanMax)
        {
            float dx = Mathf.Pow(lookAtPosition.position.x - transform.position.x, 2);
            float dy = Mathf.Pow(lookAtPosition.position.y - transform.position.y, 2);

            float newCamZ = -Mathf.Sqrt(MaxDistance * MaxDistance - (dx + dy)) + lookAtPosition.position.z;

            if (!float.IsNaN(newCamZ))
                CameraPosition.z = newCamZ;
                    //transform.SetPositionAndRotation(new Vector3(lookAtPosition.position.x, transform.position.y, newCamZ), new Quaternion());
        }

        transform.SetPositionAndRotation(CameraPosition, new Quaternion());
        transform.LookAt(lookAtPosition);
	}
}
