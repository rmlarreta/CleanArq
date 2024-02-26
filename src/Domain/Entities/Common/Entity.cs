namespace Domain.Entities.Common
{
    public class Entity<TIdentifier>
    {
        public TIdentifier? Id { get; set; }
    }
}
