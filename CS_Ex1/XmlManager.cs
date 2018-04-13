using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CS_Ex1
{
    class XmlManager
    {
        public string Filename { get; set; }
        public XmlManager(string file)
        {
            Filename = file;
        }
        public void Read(SortedList<string,Book> bookList, List<Author> authorList)
        {

            XmlTextReader reader = new XmlTextReader(Filename);
            reader.WhitespaceHandling = WhitespaceHandling.None;

            while (reader.Name != "Book")
            {
                reader.Read();
            }

            while (reader.Name == "Book")
            {
                reader.ReadStartElement("Book");
                string bookISBN = reader.ReadElementString("ISBN");
                string bookName = reader.ReadElementString("Name");
                string bookNumberOfCopies= reader.ReadElementString("NumberOfCopies");
                string bookPrice = reader.ReadElementString("Price");

                reader.ReadStartElement("Author");
                string authorName = reader.ReadElementString("FirstName");
                string authorLastName = reader.ReadElementString("LastName");
                string authorNumberOfBooks = reader.ReadElementString("NumberOfBooks");
                reader.ReadEndElement();

                Author author = new Author(authorName, authorLastName, int.Parse(authorNumberOfBooks));

                if (authorList.Contains(author))
                {
                    author = authorList.ElementAt(authorList.IndexOf(author));
                }

                Book book = new Book(bookISBN, bookName, author, int.Parse(bookNumberOfCopies), decimal.Parse(bookPrice));
                
                bookList.Add($"{bookName} {bookISBN}", book);
                reader.ReadEndElement();
            }
            reader.Close();
        }

        public void Write(SortedList<string, Book> BooksList)
        {
            XmlTextWriter writer = new XmlTextWriter(Filename, Encoding.Unicode);
            writer.Formatting = Formatting.Indented;

            // start the document
            writer.WriteStartDocument();
            

            // write the data
            foreach (Book book in BooksList.Values)
            {
                writer.WriteStartElement("Book");
                writer.WriteElementString("ISBN", book.ISBN);
                writer.WriteElementString("Name", book.Name);
                writer.WriteElementString("NumberOfCopies", book.NumberOfBooks.ToString());
                writer.WriteElementString("Price", book.Price.ToString());
                writer.WriteStartElement("Author");
                writer.WriteElementString("FirstName", book.Author.FirstName);
                writer.WriteElementString("LastName", book.Author.LastName);
                writer.WriteElementString("NumberOfBooks", book.Author.NumberOfBooks.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            // end the document
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}