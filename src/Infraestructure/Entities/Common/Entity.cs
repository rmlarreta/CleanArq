namespace Infraestructure.Entities.Common
{
    public class Entity<TIdentifier>
    {
        public required TIdentifier Id { get; set; }
    }
}
