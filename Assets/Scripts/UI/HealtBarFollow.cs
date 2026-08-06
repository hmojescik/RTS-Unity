using UnityEngine;
using UnityEngine.UI;

public class HealthBarFollow : MonoBehaviour
{
    public UnitHealth target;
    public Image fill;

    private Camera cam;
    private RectTransform fillRect;

    private void Start()
    {
        cam = Camera.main;
        fillRect = fill.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 offset = Vector3.up * target.healthBarHeight;

        transform.position = cam.WorldToScreenPoint(target.transform.position + offset);

        float percent = (float)target.currentHealth / target.maxHealth;
        fillRect.localScale = new Vector3(percent, 1f, 1f);
    }
}