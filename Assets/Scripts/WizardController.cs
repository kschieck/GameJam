using UnityEngine;
using System.Collections;

public class WizardController : MonoBehaviour {

    private bool triggered = false;
    public GameObject trail;
    public GameObject text;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") return;
        StartCoroutine(Trigger());
    }

    private IEnumerator Trigger()
    {
        triggered = true;
        text.SetActive(true);

        yield return new WaitForSeconds(3f);
        trail.SetActive(true);

        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }

}
