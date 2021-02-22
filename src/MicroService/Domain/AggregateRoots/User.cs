using System;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.AggregateRoots
{
    public class User : AggregateRoot
    {
        public Guid Id { get; }

        private string _name;

        public string Name
        {
            get => _name;
            protected set => _name = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException(nameof(Name))
                : value.Trim();
        }

        private int _age;

        public int Age
        {
            get => _age;
            protected set => _age = value < 0
                ? throw new ArgumentOutOfRangeException(nameof(Age), "Age can not less than 0.")
                : value;
        }

        public User(Guid id, string name, int age)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            (Id, Name, Age) = (id, name, age);
        }

        public void ChangeName(string name) => Name = name;

        public void CelebrateBirthday() => Age++;
    }
}