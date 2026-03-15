using UnityEngine;

public class AlignToObject : MonoBehaviour
{
    [SerializeField] private bool alignX = false;
    [SerializeField] private bool alignY = false;
    [SerializeField] private bool alignZ = false;

    [SerializeField] private Transform alignTo;

    [SerializeField] private Vector3 offset = Vector3.zero;
    
    private void OnEnable()
    {
        Align();
    }

    public void Align()
    {
        if (!alignTo) return;

        transform.position = new Vector3(
            alignX ? alignTo.position.x + offset.x : transform.position.x,
            alignY ? alignTo.position.y + offset.y : transform.position.y,
            alignZ ? alignTo.position.z + offset.z : transform.position.z
        );
    }
}