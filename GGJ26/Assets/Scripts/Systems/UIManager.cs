using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
public class UIManager : MonoBehaviour
{
    private const string TIMER_LABEL = "timer-label";
    private const string FIRST_COLOUR_WINDOW = "colour-top";
    private const string SECOND_COLOUR_WINDOW = "colour-right";
    private const string THIRD_COLOUR_WINDOW = "colour-left";
    private const string FOURTH_COLOUR_WINDOW = "colour-bottom";

    private const string COLLECTIBLE_ICON = "bug-";

    private VisualElement _col1;
    private VisualElement _col2;
    private VisualElement _col3;
    private VisualElement _col4;

    private Label _timer;

    private List<VisualElement> _collectibles;
    int _collectiblesCollected = 0;
    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        _col1 = root.Q<VisualElement>(FIRST_COLOUR_WINDOW);
        _col2 = root.Q< VisualElement>(SECOND_COLOUR_WINDOW);
        _col3 = root.Q< VisualElement>(THIRD_COLOUR_WINDOW);
        _col4 = root.Q<VisualElement>(FOURTH_COLOUR_WINDOW);
        _timer = root.Q<Label>(TIMER_LABEL);
        
                _collectibles = new List<VisualElement>();
        for (int i = 0; i < 3; i++)
        {
            VisualElement icon = root.Q<VisualElement>(COLLECTIBLE_ICON + (i + 1).ToString());
            _collectibles.Add(icon);
        }


        PaletteManager.Instance.OnPaletteChanged += UpdateUIColours;
        UpdateUIColours(PaletteManager.Instance.CurrentLevelPalette.palette);

        GameManager.Instance.OnTimerUpdate += UpdateTimer;
        GameManager.Instance.OnPickup += () => 
        {
            _collectiblesCollected++;
            OnCollectiblePickup();
        };
    }
    private void OnCollectiblePickup()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i <= _collectiblesCollected)
            {
                _collectibles[i].SetEnabled(true);
            }
            else
            {
                _collectibles[i].SetEnabled(false);
            }
        }
    }
    private void UpdateTimer(int timeRemaining)
    {
        _timer.text = timeRemaining.ToString() + " s";
        if(timeRemaining < 10)
        {
            _timer.style.color = Color.red;
        }
        _timer.AddToClassList("highlight");
        Timers.After(0.1f, () => _timer.RemoveFromClassList("highlight"));
    }
    private void UpdateUIColours(LevelPaletteStruct palette)
    {
        Color col = Colourable.GetColourFromPalette(palette, Colourable.TileColour.First);
        col.a = 1f;
        _col1.style.backgroundColor = col;
        col = Colourable.GetColourFromPalette(palette, Colourable.TileColour.Second);
        col.a = 1f;
        _col2.style.backgroundColor = col;
        col = Colourable.GetColourFromPalette(palette, Colourable.TileColour.Third);
        col.a = 1f;
        _col3.style.backgroundColor = col;
        col = Colourable.GetColourFromPalette(palette, Colourable.TileColour.Fourth);
        col.a = 1f;
        _col4.style.backgroundColor = col;

        _col1.AddToClassList("highlight");
        _col2.AddToClassList("highlight");
        _col3.AddToClassList("highlight");
        _col4.AddToClassList("highlight");


        Debug.Log(_collectibles.Count);
            col = Colourable.GetColourFromPalette(palette, Colourable.TileColour.Special);
        col.a = 1f;
        foreach (var collectible in _collectibles)
        {
            collectible.style.unityBackgroundImageTintColor = col;
        }

        Timers.After(0.1f, () => 
        {
            _col1.RemoveFromClassList("highlight");
            _col2.RemoveFromClassList("highlight");
            _col3.RemoveFromClassList("highlight");
            _col4.RemoveFromClassList("highlight");
        });
    }
}
