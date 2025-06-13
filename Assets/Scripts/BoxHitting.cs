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
