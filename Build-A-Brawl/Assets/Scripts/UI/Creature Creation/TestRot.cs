using UnityEngine;

public class TestRot : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 12.0f);
    }
}
