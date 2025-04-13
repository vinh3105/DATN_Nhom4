using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private string gameOverScene;  // Sử dụng string thay vì SceneAsset

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ GameManager không bị xóa khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        // Chuyển cảnh với tên scene đã được thiết lập trong Inspector
        if (!string.IsNullOrEmpty(gameOverScene))
        {
            SceneManager.LoadScene(gameOverScene);
        }
        else
        {
            Debug.LogError("Game Over scene is not assigned in the inspector!");
        }
    }

    public void LoadMenuScene()
    {
        // Trước khi chuyển cảnh, kiểm tra nếu player đang tồn tại và xóa nó
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }

        // Sau đó chuyển cảnh đến menu
        SceneManager.LoadScene("Menu");
    }
}
