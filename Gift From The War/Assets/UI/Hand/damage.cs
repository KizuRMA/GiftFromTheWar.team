
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class damage : MonoBehaviour
{
    [SerializeField] playerAbnormalcondition abnormalcondition;
    [SerializeField] Image redImage;
    [SerializeField] PostProcessVolume volume;
    Image image;
    float ang;
    float pastPlayerLife;
    float playerlifeMax;

    // Start is called before the first frame update
    void Start()
    {
        ang = 0;
        pastPlayerLife = abnormalcondition.life;
        playerlifeMax = pastPlayerLife;
        image = this.GetComponent<Image>();
        image.enabled = true;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        redImage.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (abnormalcondition.life != pastPlayerLife)
        {
            Damage();
            pastPlayerLife = abnormalcondition.life;
        }

        //Ô‚¢‰æ‘œ‚Ìˆ—
        if (redImage.color.a > 0)
        {
            float _redAlpha = redImage.color.a - 1.0f * Time.deltaTime;
            _redAlpha = Mathf.Max(_redAlpha, 0.0f);
            redImage.color = new Color(redImage.color.r, redImage.color.g, redImage.color.b, _redAlpha);
        }

        {
            float _alpha = 1 - (pastPlayerLife / playerlifeMax);
            image.color = new Color(image.color.r, image.color.g, image.color.b, _alpha);
        }

        if (pastPlayerLife == 1.0f)
        {
            ang += Time.deltaTime * 3.0f;
            if (ang > 360.0f)
            {
                ang -= 360.0f;
            }

            float _weight = (Mathf.Sin(ang) + 1.0f) / 2;
            volume.weight = _weight + 0.1f;
        }
    }

    void Damage()
    {
        redImage.color = new Color(0.5f, 0, 0, 0.8f);
    }
}
