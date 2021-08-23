using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 自己实现的按钮脚本【给UGUI用】
/// </summary>
public class BtnScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    private Image thisBtnImg;
    /// <summary>
    /// 常规状态的sprite【如果是预制体生成，改sprite的时候记得把这个参数改了】
    /// </summary>
    public Sprite NormalStateSprite { get; set; }
    /// <summary>
    /// 给编辑器使用，代码修改无效
    /// </summary>
    [Tooltip("设置按钮是否可用")]
    public bool Interactable = true;
    public Sprite HighLightStateSprite;
    public Sprite PressStateSprite;
    public Sprite DisableStateSprite;
    /// <summary>
    /// 长按[反复调用]
    /// </summary>
    public event Action OnBtuuonPress;
    /// <summary>
    /// 按下<按下时的屏幕坐标>
    /// </summary>
    public event Action<Vector2> OnButtonDown;
    /// <summary>
    /// 进入<进入时的屏幕坐标>
    /// </summary>
    public event Action<Vector2> OnButtonEnter;
    /// <summary>
    /// 离开按钮范围
    /// </summary>
    public event Action OnButtonExit;
    /// <summary>
    /// 按钮抬起
    /// </summary>
    public event Action OnButtonUp;
    /// <summary>
    /// 拖动按钮【未开发】
    /// </summary>
    public event Action OnButtonDrag;
    /// <summary>
    /// 点击按钮[按下后在范围内抬起 ]
    /// </summary>
    public event Action OnClick;
    private bool _enable;
    public bool Enable {
        get => _enable;
        set
        {
            _enable = value;
            if (!_enable && DisableStateSprite!=null)
            {
                thisBtnImg.sprite = DisableStateSprite;
                IsPress = true;
                IsArea = true;
            }
            else if (_enable && NormalStateSprite!=null)
            {
                thisBtnImg.sprite = NormalStateSprite;
            }
        }
      }
    public bool IsPress { get; private set; }//是否按下状态
    public bool IsArea { get; private set; }//是否在范围内
    private void Awake()
    {
        thisBtnImg = GetComponent<Image>();
        NormalStateSprite = thisBtnImg.sprite;
        Enable = Interactable;
        IsPress = false;
        IsArea = false;
        
    }
    private void Update()
    {
        if (IsPress && IsArea)
        {
            OnBtuuonPress?.Invoke();
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (Enable)
        {
            if (PressStateSprite != null)
            {
                thisBtnImg.sprite = PressStateSprite;
            } 
            IsPress = true;
            IsArea = true;
            OnButtonDown?.Invoke(eventData.position);
        } 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Enable)
        {
            if (HighLightStateSprite != null)
            {
                thisBtnImg.sprite = HighLightStateSprite;
            }
            IsArea = true;
            OnButtonEnter?.Invoke(eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Enable /*&& IsPress*/)
        {
            if (NormalStateSprite != null)
            {
                thisBtnImg.sprite = NormalStateSprite;
            }
            IsPress = false;
            IsArea = false;
            OnButtonExit?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Enable && IsPress)
        {
            IsPress = false;
            IsArea = false;
            if (NormalStateSprite != null)
            {
                thisBtnImg.sprite = NormalStateSprite;
            }
            OnButtonUp?.Invoke();
            if (IsPress && IsArea)
            {
                OnClick?.Invoke();
            }
        }
    }


}
