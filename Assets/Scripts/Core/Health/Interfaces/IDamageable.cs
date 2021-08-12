namespace Core.Health.Interfaces
{
    public interface IDamageable
    {
        void TryToDamage(float amount);

        void Reduce(float amount);
    }
}