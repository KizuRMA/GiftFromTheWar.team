using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damage : MonoBehaviour
{
    [SerializeField] playerAbnormalcondition abnormalcondition;
    [SerializeField] Image redImage;
    Image image;
    float alpha;
    float pastPlayerLife;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 0;
        pastPlayerLife = abnormalcondition.life;
        image = this.GetComponent<Image>();
        image.enabled = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
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

        if (redImage.color.a > 0)
        {
            float _redAlpha = redImage.color.a - 1.0f * Time.deltaTime;
            _redAlpha = Mathf.Max(_redAlpha, 0.0f);
            redImage.color = new Color(redImage.color.r, redImage.color.g, redImage.color.b, _redAlpha);
        }

        if (abnormalcondition.life <= 1)
        {
            alpha += Time.deltaTime * 80.0f;
            alpha = Mathf.Min(alpha,90.0f);
            float _alpha = (Mathf.Sin(alpha * Mathf.Deg2Rad) + 1.0f) / 2;
            image.enabled = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, _alpha);
        }
    }

    void Damage()
    {
        redImage.color = new Color(0.5f,0,0,0.5f);
    }
}
