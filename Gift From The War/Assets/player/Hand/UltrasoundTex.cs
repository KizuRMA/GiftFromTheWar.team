using UnityEngine;
using UnityEngine.UI;

public class UltrasoundTex : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float vanishSpeed;
    float alpha;

    private void Awake()
    {
        alpha = 0.0f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    private void Update()
    {
        if (alpha <= 0.0f) return;

        alpha -= vanishSpeed * Time.deltaTime;
        alpha = Mathf.Max(alpha,0.0f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    public void OnDisplay()
    {
        alpha = 1.0f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

}
