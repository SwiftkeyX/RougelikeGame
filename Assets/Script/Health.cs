public class Health
{
    private float _healthAmount;

    // getter and setter
    public float HealthAmount { get { return _healthAmount; } set { _healthAmount = value; } }

    public Health(float health)
    {
        this._healthAmount = health;
    }

    public void TakeDamge(float damage)
    {
        this._healthAmount -= damage;
    }
}