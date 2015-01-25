using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrailController : MonoBehaviour
{

    public LineRenderer lr;
    public EdgeCollider2D trailcol;

    public float line_duration;
    public List<PointMap> positions;

    public GameObject player;

    void Start()
    {
        GameObject parent = new GameObject("Spheres");
        positions = new List<PointMap>();
        

        StartCoroutine(line_follow());
    }


    public class PointMap
    {
        public Vector2 position;
        public float time;

        public PointMap(Vector2 pos, float t)
        {
            position = pos;
            time = t;
        }
    }

    private IEnumerator line_follow()
    {
        //Garuntee first place
        float t = 0f;
        float last_t = -10f;
        Vector2 player_pos = new Vector2(0f, 1000f);
        int count = 0;

        for (; ; )
        {
            t += Time.deltaTime;
            
            // Add a point if
            float d = Vector3.Distance(player_pos, player.transform.position);
            if ((t > last_t + 0.1f && d > 1f) || d > 2f)
            {
                last_t = t;
                player_pos = player.transform.position;
                positions.Add(new PointMap(player.transform.position, t));
            }

            // Remove expired points
            while (positions.Count > 0 && positions[0].time < t - line_duration)
            {
                positions.RemoveAt(0);
                count--;
            }
            count = positions.Count;

            lr.SetVertexCount(count);
            for (int i = 0; i < count; i++)
            {
                lr.SetPosition(i, new Vector3(positions[i].position.x, positions[i].position.y, -1));
            }

            // Ignore previous 6 on line collider
            if (count > 4)
            {
                trailcol.points = positions.GetRange(0, count - 3).Select(p => p.position).ToArray();
            }


            yield return 0;

        }
    }

    public float bounce_speed = 1f;
    Vector2 down = new Vector2(0f, 1f);

    void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 playerpos = other.gameObject.transform.position;

        int i=0;
        for (; i < trailcol.pointCount - 1; i++)
        {
            Vector2 p1 = trailcol.points[i];
            Vector2 p2 = trailcol.points[i + 1];

            if (Vector2.Distance(playerpos, p1) < Vector2.Distance(playerpos, p2))
            {
                //Do bounce
                Vector2 angle = (p2 - p1).normalized;
                Vector2 perp = new Vector2(-angle.y, angle.x) * bounce_speed;

                if (Vector2.Dot(down, perp) < 0)
                {
                    perp = -perp;
                }
                
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(perp);
                

                break;
            }
        }

        Debug.Log("Trigger Enter");
    }


}


