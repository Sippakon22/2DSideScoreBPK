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
