using UnityEngine;

public class ScaleObject : AbstractAttack
{
    [Header("ScaleObject")]
    public GameObject objectToBeScaled;
    public float scaleFactor;
    public float totalTime;
    private Vector3 initialScale;
    private Vector3 finalScale;
    private float timeElapsed;
    protected void Update()
    {
        timeElapsed += Time.deltaTime;
        objectToBeScaled.transform.localScale = Vector3.Lerp(initialScale, finalScale, Mathf.Clamp(timeElapsed / totalTime, 0, 1));
        if (timeElapsed >= totalTime)
        {
            OnFinished?.Invoke(new IAttack.AttackContext(finishedCount, this));
        }
    }

    public override void RestartAttack()
    {
        initialScale = objectToBeScaled.transform.localScale;
        finalScale = objectToBeScaled.transform.localScale * scaleFactor;
        timeElapsed = 0;
    }
}