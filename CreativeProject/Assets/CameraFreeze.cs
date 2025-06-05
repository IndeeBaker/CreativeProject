using UnityEngine;

public class CameraFreeze : MonoBehaviour
{
    private Vector3 frozenPosition;
    private bool isFrozen = false;

    public bool IsFrozen => isFrozen;  // Public getter for freeze state

    void LateUpdate()
    {
        if (isFrozen)
        {
            transform.position = frozenPosition;
        }
    }

    public void Freeze()
    {
        frozenPosition = transform.position;
        isFrozen = true;
    }

    public void Unfreeze()
    {
        isFrozen = false;
    }
}
