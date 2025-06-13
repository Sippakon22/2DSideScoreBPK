using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Values")]
    [Tooltip("ความเร็วในการเคลื่อนที่ด้านข้าง")]
    public float speed;

    [Tooltip("ความแรงของการกระโดด")]
    public float jumpPower;

    [Tooltip("ระยะตรวจสอบพื้นที่ด้านล่างตัวละคร")]
    public float groundCheckDistance = 0.05f;

    [Tooltip("สถานะว่าติดพื้นหรือไม่")]
    public bool isGround;

    [Header("Components")]
    [Tooltip("Rigidbody2D ของตัวละคร")]
    public Rigidbody2D rb;

    // อินพุตที่รับจากผู้เล่น
    private bool jumpInput;
    private float moveInput;

    // อ้างอิง Transform ของตัวละคร
    private Transform player;

    void Awake()
    {
        player = transform;
    }

    void Update()
    {
        // ตรวจสอบว่าอยู่บนพื้น และมีการกดปุ่มกระโดด
        if (IsGround() && jumpInput)
        {
            Jump(jumpPower);
        }

        // อัปเดตสถานะติดพื้น
        isGround = IsGround();

        // รับอินพุตจากผู้เล่น
        HandleInput();
    }

    void FixedUpdate()
    {
        // เคลื่อนที่ตัวละคร
        Walk(moveInput);
    }

    private void Walk(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    private void Jump(float jumpPower)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private bool IsGround()
    {
        float playerHeight = player.GetComponent<BoxCollider2D>().size.y;
        Vector2 origin = player.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance + playerHeight / 2, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    private void HandleInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
    }

    // วาด Gizmo แสดงระยะตรวจสอบพื้นใน Scene View
    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.red;
        Vector2 origin = player.position;
        Vector2 direction = Vector2.down;
        float playerHeight = GetComponent<BoxCollider2D>() != null ? GetComponent<BoxCollider2D>().size.y : 1f;

        Gizmos.DrawLine(origin, origin + direction * (groundCheckDistance + playerHeight / 2));
    }
}
