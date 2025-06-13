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
