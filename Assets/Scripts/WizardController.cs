using UnityEngine;
using System.Collections;

public class WizardController : MonoBehaviour {

    private bool triggered = false;
    public GameObject brush;
    public GameObject text;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player" || 
            triggered) return;
        StartCoroutine(Trigger());
    }

    private IEnumerator Trigger()
    {

        triggered = true;
		audio.Play();
        text.SetActive(true);

        yield return new WaitForSeconds(3f);
        brush.SetActive(true);

        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }

}
