using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Tạo một biến static để lưu trữ instance (thể hiện) duy nhất
    public static DontDestroy instance;

    void Awake()
    {
        // Nếu chưa có instance nào tồn tại
        if (instance == null)
        {
            // Thì gán instance là đối tượng này
            instance = this;

            // Và không phá hủy nó khi chuyển scene
            DontDestroyOnLoad(gameObject);
        }
        // Ngược lại, nếu đã có một instance khác tồn tại rồi
        else
        {
            // Thì phá hủy đối tượng "mới" này đi để tránh trùng lặp
            Destroy(gameObject);
        }
    }
}