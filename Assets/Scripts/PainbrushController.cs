using UnityEngine;
using System.Collections;

public class PainbrushController : MonoBehaviour {

    public GameObject trail;

	void Start () {
        if (trail == null) trail = GameObject.Find("Trail");
	}
	

    void OnTriggerEnter2D(Collider2D other)
    {
        trail.SetActive(true);
        gameObject.SetActive(false);
    }
}
