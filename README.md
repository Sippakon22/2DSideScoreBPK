# Movement.cs
สคริปต์ควบคุมการเดินของผู้เล่น
```csharp
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
```


# FollowCamera.cs
สคริปต์ควบคุมกล้องให้ตามตัวผู้เล่น
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Target Settings")]
    // ผู้เล่นที่กล้องจะติดตาม
    public Transform player;

    [Header("Camera Offset")]
    // ระยะเลื่อนกล้องในแนวแกน X
    public float xOffset = 0.1f;

    // ระยะเลื่อนกล้องในแนวแกน Y
    public float yOffset = 0.1f;

    void Update()
    {
        Follow(player);
    }

    private void Follow(Transform player)
    {
        if (player == null) return;
        // อัปเดตตำแหน่งกล้องให้ตามตำแหน่งของ player โดยเพิ่ม offset
        transform.position = new Vector3(
            player.position.x + xOffset,
            player.position.y + yOffset,
            transform.position.z // รักษาค่าความลึก (Z) เดิมไว้
        );
    }
}
```


# CoinManager.cs
สคริปต์เก็บเหรียญ
```csharp
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [Header("Coin Settings")]
    // Tag ที่ใช้ตรวจจับเหรียญ
    public string coinTag = "Coin";

    [Header("Score Tracking")]
    // คะแนนปัจจุบันของผู้เล่น
    public int score = 0;

    // UI ที่จะแสดงคะแนน (ใช้ TextMeshPro)
    public TextMeshProUGUI scoreUI;

    // ฟังก์ชันจะถูกเรียกเมื่อมีวัตถุอื่นเข้ามาใน Trigger ของวัตถุนี้
    void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่า Tag ของวัตถุที่ชนคือ "Coin"
        if (collision.tag == coinTag)
        {
            // ทำลายวัตถุเหรียญ
            Destroy(collision.gameObject);

            // เพิ่มคะแนน
            score += 1;

            // อัปเดตข้อความบนหน้าจอ
            scoreUI.text = "Score: " + score;
        }
    }
}
```


# Enemy.cs
สคริปต์ควบคุมการเดินของศัตรู
```csharp
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
```


# PlayerManager.cs
สคริปต์ควบคุมเลือด การตายและสถานะอื่นๆของผู้เล่น
```csharp
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("ค่าพลังชีวิตสูงสุด")]
    public int maxHp = 3;

    [Tooltip("ค่าพลังชีวิตปัจจุบัน")]
    public int hp = 3;

    [Header("Invincibility")]
    [Tooltip("ระยะเวลาอมตะหลังจากได้รับความเสียหาย")]
    public float invincibilityTime = 1f;
    private bool isInvincible = false;

    [Header("UI")]
    [Tooltip("UI ที่จะแสดงเมื่อผู้เล่นตาย")]
    public GameObject deadUi;

    // เพิ่มพลังชีวิต
    public void Heal(int healAmount)
    {
        hp += healAmount;
        if (hp > maxHp) hp = maxHp;
    }

    // รับความเสียหาย ถ้าไม่อยู่ในช่วงอมตะ
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        hp -= damage;
        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    // ทำให้ตัวละครอมตะชั่วคราว
    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    // เมื่อตายให้ทำลายตัวละครและเปิด UI แสดงความตาย
    public void Dead()
    {
        Debug.Log("You dead");
        Destroy(gameObject);
        deadUi.SetActive(true);
    }
}
```


# HPUI.cs
สคริปต์แสดงผลเลือดผู้เล่น
```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [Header("UI Prefab Settings")]
    // พรีแฟบของไอคอน HP ที่จะใช้แสดงผล
    public GameObject hpPrefab;

    [Header("Player Reference")]
    // อ้างอิงไปยัง PlayerManager เพื่อตรวจสอบค่า hp ปัจจุบัน
    public PlayerManager player;

    // รายการไอคอน HP ที่ถูกสร้างขึ้น
    private List<GameObject> hpIcons = new List<GameObject>();

    void Start()
    {
        UpdateHPUI(); // แสดง HP ตอนเริ่มเกม
    }

    void Update()
    {
        // อัปเดต UI เฉพาะเมื่อจำนวนไอคอนไม่เท่ากับ HP ปัจจุบัน
        if (hpIcons.Count != player.hp)
        {
            UpdateHPUI();
        }
    }

    void UpdateHPUI()
    {
        // ลบไอคอนทั้งหมดก่อนสร้างใหม่
        foreach (GameObject icon in hpIcons)
        {
            Destroy(icon);
        }
        hpIcons.Clear();

        // สร้างไอคอนตามจำนวน HP ปัจจุบัน
        for (int i = 0; i < player.hp; i++)
        {
            GameObject icon = Instantiate(hpPrefab, transform);
            hpIcons.Add(icon);
        }
    }
}
```


# BoxHitting.cs
สคริปต์จัดการ Hitbox
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHitting : MonoBehaviour
{
    [Header("Spawn Object Settings")]
    // กำหนดวัตถุที่จะ spawn เมื่อถูกผู้เล่นชน
    public GameObject flower;

    // ตัวแปรป้องกันไม่ให้ spawn ซ้ำหลายครั้ง
    private bool isSpawn = false;

    // เรียกเมื่อมีวัตถุอื่นชนกับวัตถุนี้
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ถ้าชนกับวัตถุที่มี tag เป็น "Player" ให้ทำการ Spawn
        if (collision.gameObject.tag == "Player")
        {
            Spawn(flower);
        }
    }

    // ฟังก์ชันสำหรับสร้างวัตถุที่กำหนดขึ้นมา
    void Spawn(GameObject spawnObject)
    {
        // ถ้าเคย spawn ไปแล้ว จะไม่ทำซ้ำ
        if (isSpawn) return;

        // ตำแหน่งการเกิดของวัตถุจะอยู่เหนือกล่องขึ้นไปเล็กน้อย
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y + 1f);

        // สร้างวัตถุขึ้นมาในตำแหน่งที่กำหนด
        Instantiate(spawnObject, spawnPos, transform.rotation);

        // ตั้งค่าว่าได้ spawn ไปแล้ว
        isSpawn = true;
    }
}
```


# Weakness.cs
สคริปต์จุดอ่อนของศัตรู
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : MonoBehaviour
{
    // ตัวละครที่จะถูกทำลายเมื่อชนกับ Player
    public GameObject character;

    // ฟังก์ชันจะถูกเรียกเมื่อเกิดการชนกัน (collision) แบบ 2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ตรวจสอบว่าชนกับวัตถุที่มี tag เป็น "Player" หรือไม่
        if (collision.gameObject.tag == "Player")
        {
            print("Player!!");  // แสดงข้อความใน Console ว่า Player ชนเข้ามา
            Destroy(character);  // ทำลายตัวละครที่กำหนดไว้
        }
    }
}
```


# HealItem.cs
สคริปต์ไอเทมเพื่มเลือด
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    [Header("Healing Settings")]
    // ปริมาณ HP ที่จะฟื้นฟูให้ผู้เล่นเมื่อเก็บไอเท็มนี้
    public int healAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // ถ้า object ที่ชนเป็น Player
        if (other.gameObject.tag == "Player")
        {
            // เรียกใช้ฟังก์ชัน Heal จาก PlayerManager เพื่อเพิ่ม HP
            other.gameObject.GetComponent<PlayerManager>().Heal(healAmount);

            // ทำลายไอเท็มหลังจากถูกเก็บ
            Destroy(gameObject);
        }
    }
}
```


# Scene.cs
สคริปต์เปลี่ยนด่านและการเริ่มเกมใหม่
```csharp
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scene : MonoBehaviour
{
    // ฟังก์ชันสำหรับเริ่มฉากใหม่อีกครั้ง (รีสตาร์ทเกม)
    public void Restart()
    {
        // ดึงชื่อฉากปัจจุบัน
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();

        // โหลดฉากปัจจุบันใหม่
        SceneManager.LoadScene(currentScene.name);
    }
}
```
