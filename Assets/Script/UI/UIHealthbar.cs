using UnityEngine;
using UnityEngine.UI;

// put this on UIHealthbar
public class UIHealthbar : MonoBehaviour
{
    // dependency
    [SerializeField] private EnemyStateMachine _ctx;
    [SerializeField] private Vector3 _offset;

    // put health's background here
    [SerializeField] private Image _healthbar;

    // bar width
    private float _currentHealthbarWidth;

    // rectransform
    private RectTransform _currentHealthbar;

    void Awake()
    {
        _currentHealthbar = _healthbar.rectTransform;
    }

    void LateUpdate()
    {
        // move healthbar to above Enemy's head
        this.transform.position = Camera.main.WorldToScreenPoint(_ctx.transform.position + _offset);

        _currentHealthbarWidth = _ctx.EnemyStat.GetHealthPercentage() * 100;
        _currentHealthbar.sizeDelta = new Vector2(_currentHealthbarWidth, _currentHealthbar.sizeDelta.y);
    }
}
