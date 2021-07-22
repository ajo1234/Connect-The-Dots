using UnityEngine;

public class BoardSizeData : MonoBehaviour
{
    public int rowValue;
    public int columnValue;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
