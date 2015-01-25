using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

    public string LevelToLoad;

    void OnTriggerEnter2D(Collider2D other)
    {
        Application.LoadLevel(LevelToLoad);
    }

}
