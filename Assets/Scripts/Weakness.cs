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
