using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{

	private Transform player;		// Reference to the player's transform.
	public Vector2 min;
	public Vector2 max;

    void Start() {
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
		float x = Mathf.Clamp (player.transform.position.x, min.x, max.x);
		float y = Mathf.Clamp (player.transform.position.y, min.y, max.y);
		// Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(x, y, -5f);
	}
}
