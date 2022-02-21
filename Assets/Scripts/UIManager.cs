using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{   
    #region Fields

    [SerializeField] private TextMeshProUGUI _widthTextAmt,_heightTextAmt,  _infoText; 
    [SerializeField] private Slider _widthSlider, _heightSlider;
    [SerializeField] private Button _createButton, _deleteButton, _resetPath, _newPath;
    #endregion
    #region Properties
    public static UIManager Instance {get; set;}
    public int WidthAmount  => _widthSlider != null ? (int)_widthSlider.value : 0; 
    public int HeightAmount   => _heightSlider != null ?(int)_heightSlider.value : 0;
    #endregion

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
    
    #region UI methods
    public void EnableButtons(bool enable)
    {
        if(_createButton)_createButton.interactable = enable;
        if(_deleteButton)_deleteButton.interactable = enable;
        if(_newPath)_newPath.interactable = enable;
        if(_resetPath)_resetPath.interactable = enable;
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
    public void EnableCreatePath(bool enable)
    { 
        _newPath.gameObject.SetActive(enable); 
        _resetPath.gameObject.SetActive(!enable);
    }

    #endregion
}
