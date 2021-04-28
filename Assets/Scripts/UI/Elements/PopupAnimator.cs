using System.Collections;
using UnityEngine;

public class PopupAnimator : MonoBehaviour
{
    public float AnimationSpeed = 30f;

    void Awake()
    {
        transform.localScale = new Vector3(.0f, 1f, 1f);
    }

    public void Animate(bool up)
    {
        if(up)
        {
            StopCoroutine("AnimateDownCoroutine");
            StartCoroutine("AnimateUpCoroutine");
        }
        else
        {
            StartCoroutine("AnimateDownCoroutine");
            StopCoroutine("AnimateUpCoroutine");
        }
    }

    IEnumerator AnimateDownCoroutine()
    {
        Vector3 animatedScale = Vector3.one;
        float startX = transform.localScale.x;
        float endX = 0f;

        while(startX != endX)
        {
            startX = Mathf.Lerp(startX, endX, Time.deltaTime * AnimationSpeed);
            startX = Mathf.Clamp01(startX);

            animatedScale.x = startX;
            transform.localScale = animatedScale;
            yield return null;
        }
    }

    IEnumerator AnimateUpCoroutine()
    {
        Vector3 animatedScale = Vector3.one;
        float startX = transform.localScale.x;
        float endX = 1f;

        while(startX != endX)
        {
            startX = Mathf.Lerp(startX, endX, Time.deltaTime * AnimationSpeed);
            startX = Mathf.Clamp01(startX);

            animatedScale.x = startX;
            transform.localScale = animatedScale;
            yield return null;
        }
    }
}
