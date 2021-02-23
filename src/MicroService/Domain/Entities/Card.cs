using System;

namespace Domain.Entities
{
    public class Card
    {
        public Guid Id { get;}
        public string Name { get; protected set; }

        public Card(Guid id, string name)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            (Id, Name) = (id, name);
        }
    }
}