using UnityEngine;
using UnityEngine.Events;

public class CollisionEventHandler : MonoBehaviour
{
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionStay;
    public UnityEvent onCollisionExit;

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke();
    }

    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay?.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit?.Invoke();
    }
}
