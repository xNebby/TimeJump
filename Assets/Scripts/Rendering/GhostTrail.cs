using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    private SpriteRenderer sr;
    public Transform ghostsParent;
    public Color trailColor;
    public Color fadeColor;
    public float ghostInterval;
    public float fadeTime;
    public GameObject Player;
    private SpriteRenderer playerSR;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        EventManager.StartListening("CAM_UpdateFollow", BindPlayer);
    }
    void OnDisable()
    {
        EventManager.StopListening("CAM_UpdateFollow", BindPlayer);
    }
    public void BindPlayer()
    {
        //Debug.Log("Bound");
        Player = GameObject.FindWithTag("PlayerRenderer");
        playerSR = Player.GetComponent<SpriteRenderer>();
    }

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++)
        {
            Transform currentGhost = ghostsParent.GetChild(i);
            s.AppendCallback(() => currentGhost.position = Player.transform.position);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = playerSR.flipX);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().sprite = playerSR.sprite);
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
