using System;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.AggregateRoots
{
    public class User : AggregateRoot
    {
        public Guid Id { get; }
        public string Name { get; protected set; }
        public int Age { get; protected set; }

        public User(Guid id, string name, int age)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (age < 0) throw new ArgumentOutOfRangeException(nameof(age), "Age can not less than 0.");
            Id = id;
            Name = name.Trim();
            Age = age;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name.Trim();
        }

        public void CelebrateBirthday() => Age++;
    }
}