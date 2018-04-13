using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CS_Ex1
{
    class Book : IEquatable<Book>
    {
        private int _numberOfBooks;
        private decimal _price;
        private string _name;
        private string _iSBN;

        public string ISBN
        {
            get => _iSBN;
            set
            {
                if (!Regex.Match(value, @"^(\d|[A-Z]){0,10}$").Success){
                    throw new InvalidValueException("Invalid ISBN value");
                }

                _iSBN = value;
            } 
        }
        public string Name
        {
            get => _name;
            set
            {
                if (!Regex.Match(value, @"^([A-Za-z0-9]*\s?){0,15}$").Success)
                {
                    throw new InvalidValueException("Invalid book name");
                }
                _name = value;
            }
        }
        public Author Author { get; set; }
        public int NumberOfBooks
        {
            get => _numberOfBooks;
            set
            {
                if (value < 0)
                {
                    throw new InvalidValueException("Invalid number of books");
                }
                _numberOfBooks = value;
            }
        }
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                {
                    throw new InvalidValueException("Invalid price");
                }

                _price = value;
            }
        }

        public Book(string iSBN, string name, Author author, int numberOfBooks, decimal price)
        {
            ISBN = iSBN;
            Name = name;
            Author = author;
            NumberOfBooks = numberOfBooks;
            Price = price;
        }

        public bool Equals(Book other)
        {
            return other.ISBN == _iSBN && other.Name == _name && other.Author == Author ? true : false;
        }

        public static implicit operator ListBoxItem(Book v)
        {
            throw new NotImplementedException();
        }
    }
}
