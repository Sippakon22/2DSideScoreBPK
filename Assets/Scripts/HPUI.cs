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
