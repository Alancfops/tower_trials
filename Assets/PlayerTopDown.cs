using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerTopDown : MonoBehaviour
{
    public float moveSpeed = 4f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D, ←/→
        float v = Input.GetAxisRaw("Vertical");   // W/S, ↑/↓
        input = new Vector2(h, v);

        if (input.sqrMagnitude > 1f)
            input = input.normalized; // mesma velocidade na diagonal

        bool isMoving = input.sqrMagnitude > 0.01f;
        anim.SetBool("IsMoving", isMoving);

        // virar sprite para a esquerda/direita
        if (isMoving && Mathf.Abs(input.x) > 0.01f)
            sr.flipX = input.x < 0f;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}
