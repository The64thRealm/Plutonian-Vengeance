using UnityEngine;

public class AlignToObject : MonoBehaviour
{
    [SerializeField] private bool alignX = false;
    [SerializeField] private bool alignY = false;
    [SerializeField] private bool alignZ = false;

    [SerializeField] private Transform alignTo;
    
    private void OnEnable()
    {
        Align();
    }

    public void Align()
    {
        if (!alignTo) return;

        transform.position = new Vector3(
            alignX ? alignTo.position.x : transform.position.x,
            alignY ? alignTo.position.y : transform.position.y,
            alignZ ? alignTo.position.z : transform.position.z
        );
    }
}