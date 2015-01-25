using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour {

    public Text highest_text;
    public Text height_text;
    public Transform player;
    float highest;

    float offset;

    private void Start()
    {
        offset = player.position.y;

        highest = offset;
    }

    private void Update()
    {
        float height = player.transform.position.y;

        highest = Mathf.Max(highest, height);

        height_text.text = "Height: " + (int)(height - offset);
        highest_text.text = "Highest: " + (int)(highest - offset);

    }

}
