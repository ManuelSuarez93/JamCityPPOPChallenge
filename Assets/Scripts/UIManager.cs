using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{   
    public static UIManager Instance {get; set;}

    [SerializeField] private TextMeshProUGUI _widthTextAmt,_heightTextAmt,  _infoText; 
    [SerializeField] private Slider _widthSlider, _heightSlider;
    [SerializeField] private Button _createButton, _deleteButton, _resetPath, _newPath;
    
    public int WidthAmount  => _widthSlider != null ? (int)_widthSlider.value : 0; 
    public int HeightAmount   => _heightSlider != null ?(int)_heightSlider.value : 0;

    private void Awake()
    {
         if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
    }

    private void Start()
    {
        ChangeHeightAmt(HeightAmount);
        ChangeWidthAmt(WidthAmount);
    }
    
    public void EnableCreate(bool enable)
    {
        if(_createButton)_createButton.interactable = enable;
    }
    public void ChangeWidthAmt(float amt)
    {
        if(_widthTextAmt) _widthTextAmt.text = amt.ToString(); 
    }
    public void ChangeHeightAmt(float amt)
    {
        if(_heightTextAmt)_heightTextAmt.text = amt.ToString(); 
    }
    public void SetInfoText(string text)
    {
        if(_infoText) _infoText.text = text;
    } 
}
