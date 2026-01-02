using UnityEngine;
using UnityEngine.UI;

public class UIHealthbarPlayer : MonoBehaviour
{
    // dependency
    [SerializeField] private PlayerStateMachine _ctx;
    [SerializeField] private Image _healthbar;

    // rectransform
    private RectTransform _healthbarRT;

    // width
    private float _healthbarWidth;

    void Awake()
    {
        _healthbarRT = _healthbar.rectTransform;
    }

    void LateUpdate()
    {
        _healthbarWidth = _ctx.Stat.CurrentHealthPercentage * 100f;
        _healthbarRT.sizeDelta = new Vector2(_healthbarWidth, _healthbarRT.sizeDelta.y);
    }
}