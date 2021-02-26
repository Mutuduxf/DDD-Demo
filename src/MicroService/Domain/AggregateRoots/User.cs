using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.ValueObjects;
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

        public Address Address { get; protected set; }

        public Gender Gender { get; protected set; }

        private List<string> _tags = new();
        public IReadOnlyList<string> Tags => _tags;

        private List<Card> _cards = new();
        public IReadOnlyList<Card> Cards => _cards;

        private User()
        {
        }

        public User(Guid id, string name, int age, Gender gender, string country, string state, string city,
            string street)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            (Id, Name, Age, Gender, Address) = (id, name, age, gender, new Address(country, state, city, street));
        }

        public void ChangeName(string name) => Name = name;

        public void CelebrateBirthday() => Age++;

        public void SetTags(params string[] tags)
        {
            _tags = tags.Distinct().ToList();
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
    }
}