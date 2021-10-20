using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text levelNumberText;

    private LevelSelectionUI _lsu;

    public void SetText(string text)
    {
        levelNumberText.text = text;
    }

    public void SetAvailable(bool isAvailable)
    {
        Button button = GetComponent<Button>();
        if (button == null) return;

        button.interactable = isAvailable;
    }

    public void SetDone(bool isDone)
    {
        if (isDone)
        {
            Debug.Log("Coucou je suis un niveau fini, peut-être faudrait-il me passer en vert ou quelque chose. Enfin je dis ça je sais pas");
        }
    }

    public void SetUI(LevelSelectionUI lsu)
    {
        Button b = GetComponent<Button>();

        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(delegate {lsu.ChangeSceneOnButtonPressedServerRpc(int.Parse(levelNumberText.text));});
    }
}
