using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public float CameraSpeed;

	
	void Update () {

        Vector3 position = new Vector3(Target.position.x, Target.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, position, CameraSpeed * Time.deltaTime);

	}
}
