using System;
using System.Text.RegularExpressions;

namespace CS_Ex1
{
    public class Author : IEquatable<Author>
    {
        private string _firstName;
        private string _lastName;
        private int _numberOfBooks;

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (!Regex.Match(value, @"[A-Z][a-zA-Z]{0,12}").Success)
                {
                    throw new InvalidValueException("Invalid author first name");
                }
                _firstName = value;
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                if (!Regex.Match(value,@"[A-Z][a-zA-Z]{0,12}").Success)
                {
                    throw new InvalidValueException("Invalid author last name");
                }
                _lastName = value;
            }
        }
        public int NumberOfBooks
        {
            get => _numberOfBooks;
            set
            {
                if (value < 0)
                {
                    throw new InvalidValueException("Invalid number of books per author");
                }
                _numberOfBooks = value;
            }
        }

        public Author(string firstName, string lastName, int numberOfBooks)
        {
            FirstName = firstName;
            LastName = lastName;
            NumberOfBooks = numberOfBooks;
        }

        public bool Equals(Author other)
        {
            return other.FirstName == _firstName && other.LastName == _lastName ? true : false;
        }
    }
}