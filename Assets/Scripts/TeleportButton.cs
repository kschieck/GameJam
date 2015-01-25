using UnityEngine;
using System.Collections;

public class TeleportButton : MonoBehaviour {

    public Transform player;

	void Update () {
        if (Input.GetKey(KeyCode.P))
        {
            Vector3 v = new Vector3(transform.position.x, transform.position.y, player.position.z);
            player.position = v;
        }
	}
}
