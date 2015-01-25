using UnityEngine;
using System.Collections;
using LevelSelect;

public class SlideCameraMover : MonoBehaviour {

    public float delay;

    public float duration;
    public float start_y, end_y;
    SlideCamera slideCamera;

    Vector2 StartPosition;
    Vector2 EndPosition;

	void Start () {
        slideCamera = GetComponent<SlideCamera>();
        StartPosition = new Vector2(transform.position.x, start_y);
        EndPosition = new Vector2(transform.position.x, end_y);

        slideCamera.RequestPosition(StartPosition);
        StartCoroutine(go());
	}

    private IEnumerator go()
    {
        float t = 0f;
        bool done = false;

        yield return new WaitForSeconds(delay);

        for (; duration > 0 && !done; )
        {
            t += Time.deltaTime;

            Vector2 NewPos = Vector2.Lerp(StartPosition, EndPosition, t / duration);
            slideCamera.RequestPosition(NewPos);

            done = t / duration > 1f;

            yield return 0;
        }

    }


}
