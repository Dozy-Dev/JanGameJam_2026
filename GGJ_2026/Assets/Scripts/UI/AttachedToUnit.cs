using UnityEngine;

public class AttachedToUnit : MonoBehaviour
{
    [SerializeField] private GameObject _attachedToUnit;
    private Vector3 _prevPosition;

    private void Start()
    {
        _prevPosition = _attachedToUnit.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = _attachedToUnit.transform.position - _prevPosition;
        transform.position += delta;

        _prevPosition = _attachedToUnit.transform.position;
    }
}
