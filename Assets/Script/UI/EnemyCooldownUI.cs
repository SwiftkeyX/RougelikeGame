using UnityEngine;
using UnityEngine.UI;

public class EnemyCooldownUI : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine _target;
    [SerializeField] private Text _name;

    // put light, heavy, etc.. 's background here
    [SerializeField] private Image _light;
    [SerializeField] private Image _heavy;
    [SerializeField] private Image _special;

    // width
    private float _lightWidth;
    private float _heavyWidth;
    private float _specialWidth;

    // rectTransform
    RectTransform rt_light;
    RectTransform rt_heavy;
    RectTransform rt_special;

    // getter and setter
    public float LightWidth { get { return _lightWidth; } set { _lightWidth = value; } }

    void Awake()
    {
        rt_light = _light.rectTransform;
        rt_heavy = _heavy.rectTransform;
        rt_special = _special.rectTransform;
    }

    void Update()
    {
        _lightWidth = _target.EnemyStat.GetCurrentCooldownPercentage(ENEMYATTACKTYPE.LIGHT) * 100;
        rt_light.sizeDelta = new Vector2(_lightWidth, rt_light.sizeDelta.y);

        _heavyWidth = _target.EnemyStat.GetCurrentCooldownPercentage(ENEMYATTACKTYPE.HEAVY) * 100;
        rt_heavy.sizeDelta = new Vector2(_heavyWidth, rt_heavy.sizeDelta.y);

        _specialWidth = _target.EnemyStat.GetCurrentCooldownPercentage(ENEMYATTACKTYPE.SPECIAL) * 100;
        rt_special.sizeDelta = new Vector2(_specialWidth, rt_special.sizeDelta.y);
    }
}