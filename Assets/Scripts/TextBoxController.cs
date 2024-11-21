using System.Collections.Generic;
using Dialog;
using UnityEngine;

public class TextBoxController : MonoBehaviour
{
    [SerializeField] private List<Dialogo> duckIntroduction;
    
    private int dialogCount = 0;

    private void Awake()
    {
        TextBox.OnTextEnded += NextDialog;
    }

    private void Start()
    {
        TextBox.Show(duckIntroduction[dialogCount]);
    }
    
    private void NextDialog()
    {
        if (dialogCount < duckIntroduction.Count-1)
        {
            dialogCount++;
            TextBox.Show(duckIntroduction[dialogCount]);
            return;
        }
        
        TextBox.OnTextEnded -= NextDialog;
        GameManager.LoadTutorial();
    }
}