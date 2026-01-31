using UnityEngine;

public class AttachedToUnit : MonoBehaviour
{
    [SerializeField] private GameObject _attachedToUnit;
    private Vector3 _offsetPosition;

    private void Start()
    {
        _offsetPosition = _attachedToUnit.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if( _attachedToUnit.transform.localScale.x < 0f )
        {
            transform.localScale = new Vector3(-1f,1,1);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
