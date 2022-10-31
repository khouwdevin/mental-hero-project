using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    public GameObject position_a;
    public GameObject position_b;

    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private float horizontal_move;
    private float move_toward;
    const float movement_smooth = 0.05f;

    private bool isStop;
    private bool isA;

    private Attributes attributes;

    // Start is called before the first frame update
    void Start()
    {
        attributes = GetComponent<Attributes>();
        rb = GetComponent<Rigidbody2D>();

        if (isA) move_toward = -1;
        else move_toward = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isA) move_toward = -1;
        else move_toward = 1;

        if (transform.position.x <= position_a.transform.position.x && isA)
        {
            StartCoroutine(between_position());
        }
        else if (transform.position.x >= position_b.transform.position.x && !isA)
        {
            StartCoroutine(between_position());
        }

        horizontal_move = move_toward * attributes.speed;
    }

    private void FixedUpdate()
    {
        move(horizontal_move * Time.fixedDeltaTime);
    }

    private IEnumerator between_position()
    {
        if (!isStop)
        {         
            isStop = true;

            horizontal_move = 0f;

            if (isA == true) isA = false;
            else isA = true;

            rb.velocity = Vector3.zero;

            yield return new WaitForSeconds(2f);

            isStop = false;
        }
    }

    private void move(float direction)
    {
        if (!isStop)
        {
            Vector3 targetVelocity = new Vector2(direction * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movement_smooth);
        }
    }
}
