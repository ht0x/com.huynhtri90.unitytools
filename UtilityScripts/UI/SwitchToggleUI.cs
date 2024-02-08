using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchToggleUI : MonoBehaviour
{
    [SerializeField] Image on;
    [SerializeField] Image off;
    [SerializeField] bool _isOn;
    [SerializeField] bool autoToggle = true;
    [SerializeField] bool _interacable;
    private Button button;
 
    public readonly UnityEvent<bool> onValueChanged = new UnityEvent<bool>();
    public bool IsOn { get => _isOn; set => SetState(value); }
    public bool Interactable { get => _interacable; set => SetInteracable(value); }
    
    #region ----- Unity Messsages -----
    private void Awake()
    {
        if (on == null)
        {
            var onObj = MyUtilities.FindObjectStartWith(gameObject,"ON_");
            on = onObj ? onObj.GetComponent<Image>() : null;
        }

        if (off == null)
        {
            var offObj = MyUtilities.FindObjectStartWith(gameObject, "OFF_");
            off = offObj ? offObj.GetComponent<Image>() : null;
        }

        if (on)
            on.raycastTarget = false;
        if (off)
            off.raycastTarget = false;

        button = GetComponent<Button>();
        if (ReferenceEquals(button, null))
            throw new Exception("Missing Required Component: Button");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        button.onClick?.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick?.RemoveListener(OnClick);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        SetState(IsOn,false);
#endif
    }

    #endregion

    #region ----- Methods -----

    private void SetInteracable(bool value)
    {
        button.interactable = value;
    }
    
    private bool SetState(bool value,bool inVoke=false)
    {
        if(!(ReferenceEquals(on, null) && !ReferenceEquals(off, null)))
        {
            on.gameObject.SetActive(value);
            off.gameObject.SetActive(!value);
        }

        _isOn = value;

        if (inVoke)
            onValueChanged?.Invoke(value);
        return _isOn;
    }

    private void OnClick()
    {
        if (autoToggle)
            SetState(!_isOn);

        onValueChanged?.Invoke(_isOn);
    }

    #endregion

}