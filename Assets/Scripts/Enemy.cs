using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Waypoints")]
    // จุดที่ศัตรูจะเดินไปตามลำดับ
    public Transform[] wayPoints;

    [Header("Movement Settings")]
    // ความเร็วในการเคลื่อนที่
    public float speed = 2f;
    
    // เวลาหยุดพักเมื่อถึงจุด
    public float walkCD = 3f;

    // ดัชนีของ waypoint ปัจจุบัน
    private int currentPointIndex = 0;

    // สถานะรอ
    private bool isWaiting = false;

    // ตัวจับเวลาการรอ
    private float waitTimer = 0f;

    void FixedUpdate()
    {
        MoveToWaypoint(wayPoints);
    }

    // เคลื่อนที่ไปยัง waypoint ตามลำดับ
    private void MoveToWaypoint(Transform[] wayPoints)
    {
        if (wayPoints.Length == 0) return;

        // ถ้ากำลังรออยู่ ให้ลดเวลารอ
        if (isWaiting)
        {
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                // ไปยัง waypoint ถัดไป
                currentPointIndex = (currentPointIndex + 1) % wayPoints.Length;
            }
            return;
        }

        // กำหนดเป้าหมายเป็นตำแหน่งของ waypoint (เฉพาะแกน X)
        Vector2 target = new Vector2(wayPoints[currentPointIndex].position.x, transform.position.y);

        // เคลื่อนที่ไปยังตำแหน่งเป้าหมาย
        Vector2 newPos = Vector2.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        transform.position = newPos;

        // ถ้าถึงเป้าหมายแล้วให้รอ
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            isWaiting = true;
            waitTimer = walkCD;
        }
    }

    // ตรวจจับการชนกับผู้เล่น
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Player dead!");
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(1);
        }
    }
}
