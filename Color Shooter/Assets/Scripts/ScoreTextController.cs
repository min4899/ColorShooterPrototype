using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextController : MonoBehaviour {
    public Animator animator;
    private TextMeshProUGUI damageText;

    void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        damageText = animator.GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        damageText.text = "+" + text;
    }
}
