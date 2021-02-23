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

        public IReadOnlyList<string> Tags { get; protected set; } = new List<string>();

        public IReadOnlyList<Card> Cards { get; protected set; } = new List<Card>();

        private User()
        {
        }

        public User(Guid id, string name, int age, Gender gender, string country, string state, string city,
            string street)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            (Id, Name, Age, Gender, Address) = (id, name, age, gender, new Address(country, state, city, street));
            SetTags(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            SetCards(new Card(Guid.NewGuid(), "apple"), new Card(Guid.NewGuid(), "pear"));
        }

        public void ChangeName(string name) => Name = name;

        public void CelebrateBirthday() => Age++;

        public void SetTags(params string[] tags)
        {
            Tags = tags?.ToList() ?? new List<string>();
        }

        public void SetCards(params Card[] cards)
        {
            Cards = cards?.ToList() ?? new List<Card>();
        }
    }
}