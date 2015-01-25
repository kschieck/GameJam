using UnityEngine;
using System.Collections;

public class ClickLoad : MonoBehaviour {

    public string NextLevel;
    public float delay = 0f;

	void Start () {

        StartCoroutine(getReady());
	}

    private IEnumerator getReady()
    {
        yield return new WaitForSeconds(delay);
        ready = true;
    }

    private bool ready = false;
	void Update () {

        if (ready && Input.GetMouseButtonDown(0))
        {
            Application.LoadLevel(NextLevel);
        }

	}


}
