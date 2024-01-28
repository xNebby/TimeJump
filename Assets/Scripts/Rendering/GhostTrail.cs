using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    private SpriteRenderer sr;
    public Transform ghostsParent;
    private SpriteRenderer GPsr;
    public Color trailColor;
    public Color fadeColor;
    public float ghostInterval;
    public float fadeTime;

    private void Start()
    {
        GPsr = ghostsParent.gameObject.GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("PD_DashStarted", ShowGhost);
    }
    private void OnDisable()
    {
        EventManager.StopListening("PD_DashStarted", ShowGhost);
    }

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++)
        {
            Transform currentGhost = ghostsParent.GetChild(i);
            s.AppendCallback(() => currentGhost.position = ghostsParent.position);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = GPsr.flipX);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().sprite = GPsr.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(ghostInterval);
        }
    }

    public void FadeSprite(Transform current)
    {
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }

}