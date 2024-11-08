using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.UI;

public partial class TutorialMenu : UIMenu
{
    [Header("TutorialMenu settings")]
    [SerializeField] private CutoutImage _cutourImage;
    [SerializeField] private Image _blockImage;

    public override void Display(bool playAnimation = false)
    {
        base.Display(playAnimation);

        Time.timeScale = 0;

        _blockImage.enabled = true;
    }

    public override void Hide(bool playAnimation = false)
    {
        base.Hide(playAnimation);

        Time.timeScale = 1;

        _blockImage.enabled = false;
    }
}
