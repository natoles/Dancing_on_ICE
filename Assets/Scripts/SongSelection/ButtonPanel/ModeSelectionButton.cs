using UnityEngine;
using UnityEngine.UI;
using DancingICE.Modes;

public class ModeSelectionButton : Button
{
    [SerializeField]
    private Text TextComponent = null;

    [SerializeField]
    private GameObject DifficultySlider = null;

    [SerializeField]
    private ModeManager ModeManager = null;
    
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(NextMode);
    }

    protected override void Start()
    {
        base.Start();
        UpdateText(ModeManager.Current);
    }

    private void NextMode()
    {
        UpdateModeDisplay(ModeManager.Current, ModeManager.NextMode());
    }

    public void UpdateModeDisplay(Mode previous, Mode current)
    {
        UpdateButtonsVisibility(previous, current);
        SetSliderVisibility(current);
        UpdateText(current);
    }

    private void UpdateText(Mode current)
    {
        if (TextComponent != null)
            TextComponent.text = current?.name + " Mode";
    }

    private void UpdateButtonsVisibility(Mode previous, Mode current)
    {
        if (previous.buttonsToShow != null)
            previous.buttonsToShow.SetActive(false);

        if (current.buttonsToShow != null)
            current.buttonsToShow.SetActive(true);
    }

    private void SetSliderVisibility(Mode current)
    {
        DifficultySlider.SetActive(current.showDifficultySlider);
    }
}
