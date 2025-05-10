using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Class_Lib
{
    public class Person : IHasIdentification
    {
        public uint ID { get; private set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get => FirstName + " " + Surname; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        protected internal Person()
        {
            RowVersion = Array.Empty<byte>();
        }

        public Person(string name, string surname, string phoneNumber, string email)
        {
            var exceptions = new List<Exception>();

            if (string.IsNullOrWhiteSpace(name))
                exceptions.Add(new ArgumentException("Поле \"Ім'я\" не може бути пустим."));

            if (string.IsNullOrWhiteSpace(surname))
                exceptions.Add(new ArgumentException("Поле \"Прізвище\" не може бути пустим."));

            if (string.IsNullOrWhiteSpace(phoneNumber))
                exceptions.Add(new ArgumentException("Поле \"Телефон\" не може бути пустим."));

            if (string.IsNullOrWhiteSpace(email))
                exceptions.Add(new ArgumentException("Поле \"Електронна пошта\" не може бути пустим."));

            if(!Regex.IsMatch(phoneNumber, @"^\+?\d{1,3}?[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$"))
                exceptions.Add(new ArgumentException("Поле \"Телефон\" повинно містити лише цифри та дозволені спеціальні символи."));
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                exceptions.Add(new ArgumentException("Поле \"Електронна пошта\" повинна бути в форматі \"[текст]@[текст].[текст]\" ."));
            
            if (exceptions.Count > 0)
                throw new AggregateException("Помилки при створенні особи.", exceptions);

            FirstName = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Email = email;

            RowVersion = Array.Empty<byte>();
        }

        // regex for future use (Email validation) @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
        // follows a format of *@*.* where * is any character except for whitespace or @

        [Timestamp] // concurrency token property
        public byte[] RowVersion { get; set; }
    }
}
