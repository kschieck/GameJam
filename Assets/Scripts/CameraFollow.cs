using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{

	private Transform player;		// Reference to the player's transform.
    private float min_y;

    void Start() {
        min_y = transform.position.y;
    }


	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	


	void FixedUpdate ()
	{
		TrackPlayer();
	}
	
	
	void TrackPlayer ()
	{

		// Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(transform.position.x, Mathf.Max(min_y, player.transform.position.y), -5f);
	}
}
