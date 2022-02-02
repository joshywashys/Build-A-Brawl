using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableObject : MonoBehaviour
{
    [HideInInspector] 
    public new Rigidbody rigidbody;
    public new Collider collider;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void IsGrabbed(bool pickedUp)
    {
        collider.enabled = !pickedUp;
    }
}
