using UnityEngine;

public class AddPlayerOutline : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.GetComponent<PlayerOutline>() == null)
            gameObject.AddComponent<PlayerOutline>();
    }
}