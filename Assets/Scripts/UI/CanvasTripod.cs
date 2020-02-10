using UnityEngine;
using UnityEngine.UI;

public class CanvasTripod : MonoBehaviour
{
    public Health health;

    [Tooltip("Image component displaying health left")]
    public Image healthBarImage;
    [Tooltip("The floating healthbar pivot transform")]
    public Transform healthBarPivot;
    [Tooltip("Whether the health bar is visible when at full health or not")]
    public bool hideFullHealthBar = false;

    private GameObject cam;
    private float objectScale = 0.1f;
    private bool orientate = true;
    private bool scale = false;
    private Vector3 initialScale;

    void Start()
    {
        cam = Camera.main.gameObject;
        initialScale = transform.localScale;
    }
    void Update()
    {
        UpdateRatation();
        UpdateValues();
        if (health.currentHealth == 0)
        {
            healthBarPivot.gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void UpdateValues()
    {
        if (health != null)
        {
            healthBarImage.fillAmount = health.CurrentHealth / health.maxHealth;
            if (hideFullHealthBar)
            {
                healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1);
            }
        }
    }

    private void UpdateRatation()
    {
        //billboarding the canvas
        if (orientate)
        {
            healthBarPivot.transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform.rotation * Vector3.up);
            healthBarPivot.transform.Rotate(0, 180, 0);
        }
        //making it properly scaled
        if (scale)
        {
            Plane plane = new Plane(cam.transform.forward, cam.transform.position);
            float dist = plane.GetDistanceToPoint(transform.position);
            healthBarPivot.transform.localScale = initialScale * dist * objectScale;
        }
    }
}
