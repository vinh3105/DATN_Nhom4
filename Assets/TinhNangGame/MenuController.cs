using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene"; // Tên scene chính của game
    [SerializeField] private AudioSource backgroundMusic; // Nhạc nền
    [SerializeField] private Button startButton, muteButton; // Các nút UI
    [SerializeField] private Slider volumeSlider; // Thanh trượt âm lượng

    private bool isMuted = false;

    void Start()
    {
        // Xóa Player nếu tồn tại
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }

        // Đảm bảo các nút và thanh trượt không phải null trước khi gán sự kiện
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start Button is not assigned!");
        }

        if (muteButton != null)
        {
            muteButton.onClick.AddListener(ToggleMute);
        }
        else
        {
            Debug.LogError("Mute Button is not assigned!");
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
            volumeSlider.value = backgroundMusic.volume; // Đặt giá trị mặc định
        }
        else
        {
            Debug.LogError("Volume Slider is not assigned!");
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void ToggleMute()
    {
        isMuted = !isMuted;
        backgroundMusic.mute = isMuted;
    }

    void ChangeVolume(float value)
    {
        backgroundMusic.volume = value; // Cập nhật âm lượng theo slider
    }
}