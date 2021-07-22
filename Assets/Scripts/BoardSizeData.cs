using UnityEngine;

public class BoardSizeData : MonoBehaviour
{
    public int rowValue = 2;
    public int columnValue = 2;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
