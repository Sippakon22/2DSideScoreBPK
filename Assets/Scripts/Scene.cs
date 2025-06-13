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
