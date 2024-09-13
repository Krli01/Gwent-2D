using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public TextMeshProUGUI Instructions;
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI ResultTextMini;
    public Image Panel;
    public Image Banner;
    public Image Result;
    public Button Replay;
    public Button BackToMenu;

    static CanvasGroup cg;
    public static InfoPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = FindObjectOfType<InfoPanel>();
    }
    
    void Start()
    {
        Result.enabled = false;
        ResultText.enabled = false;
        ResultTextMini.enabled = false;
        Replay.gameObject.SetActive(false);
        BackToMenu.gameObject.SetActive(false);
        
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;
    }

    public void Show(string message, float time, float delay,  bool endScene, string instructions = "")
    {
        Text.text = message;
        Instructions.text = instructions;
        StartCoroutine(DisplayFor(time, delay, endScene));
    }

    public void End(string winner)
    {
        //Panel.color = Panel.color.WithAlpha(230);
        if (winner == "Tie") 
        {
            Result.sprite = Resources.Load<Sprite>("end_draw");
            ResultText.colorGradientPreset = Resources.Load<TMP_ColorGradient>("TieGradient");
            ResultTextMini.colorGradientPreset = Resources.Load<TMP_ColorGradient>("TieGradient");
            ResultText.text = "empate";
            ResultTextMini.text = "es un";
        }
        else
        {
            Result.sprite = Resources.Load<Sprite>("end_win");
            ResultText.colorGradientPreset = Resources.Load<TMP_ColorGradient>("WinGradient");
            ResultTextMini.colorGradientPreset = Resources.Load<TMP_ColorGradient>("WinGradient");
            ResultText.text = winner;
            ResultTextMini.text = "victoria";
        }
        Show("", 0, 1f, true);
    }

    IEnumerator DisplayFor(float time, float delay,  bool endScene) 
    {
        yield return new WaitForSeconds(delay);
        EnableComponents(true, endScene);
        yield return FadeCanvasGroup(cg, 0f, 1f, 0.5f);
        if (!endScene)
        {
            yield return new WaitForSeconds(time);
            yield return FadeCanvasGroup(cg, 1f, 0f, 0.5f);
            EnableComponents(false, endScene);
        }
    }

    void EnableComponents(bool enabled, bool endScene)
    {
        Panel.enabled = enabled;
        if(endScene)
        {
            Result.enabled = enabled;
            ResultText.enabled = enabled;
            ResultTextMini.enabled = enabled;
            Banner.enabled = enabled;
            //Replay.gameObject.SetActive(enabled);
            BackToMenu.gameObject.SetActive(enabled);
        }
        else
        {
            Banner.enabled = enabled;
            Text.enabled = enabled;
            Instructions.enabled = enabled;
            //Replay.gameObject.SetActive(false);
            BackToMenu.gameObject.SetActive(false);
        }
    }

    private static IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}
