using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WizardController : MonoBehaviour {

    private bool triggered = false;
    public GameObject brush;

    public List<string> statements;
    public Text text;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player" || 
            triggered) return;
        StartCoroutine(Trigger());
        StartCoroutine(Words());
    }

    private IEnumerator Trigger()
    {
        triggered = true;
		audio.Play();

        yield return new WaitForSeconds(1f);
        brush.SetActive(true);

    }

    private IEnumerator Words()
    {
        for (int i = 0; i < statements.Count; i++)
        {
            text.text = statements[i];
            yield return new WaitForSeconds(2f);
        }

        text.text = "";
    }

}
