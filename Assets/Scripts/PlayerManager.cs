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
