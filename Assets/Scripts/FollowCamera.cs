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
