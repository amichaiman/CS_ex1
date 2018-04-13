using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;

namespace CS_Ex1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SortedList<string,Book> books = new SortedList<string,Book>();
        List<Author> authors = new List<Author>();

        public MainWindow()
        {
            int a;
        }

        
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Book bookToAdd;
            Author authorToAdd;

            try
            { 
                authorToAdd = new Author(authorFirstNameTextBox.Text, authorLastNameTextBox.Text, 0);
            }
            catch (InvalidValueException exception) //invalid first or last name
            {
                MessageBox.Show(exception.Message,"Notice");
                return;
            }


            //check if author exists in DB
            if (authors.Contains(authorToAdd))
            {
                authorToAdd = authors.ElementAt(authors.IndexOf(authorToAdd));
            }
            
            try
            {
                bookToAdd = new Book(bookISBNTextBox.Text, bookTitleTextBox.Text, authorToAdd, int.Parse(bookNumberOfCopiesTextBox.Text), decimal.Parse(bookPriceTextBox.Text));
            }
            catch (InvalidValueException exception) //Failed parse, or invalid book fields
            {
                MessageBox.Show(exception.Message,"Notice");
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid price / number of copies", "Notice");
                return;
            }
            
            if (books.ContainsKey($"{bookToAdd.Name} {bookToAdd.ISBN}"))
            {
                MessageBox.Show("Book Already Exists","Notice");
                return;
            }

            if (authorToAdd.NumberOfBooks == 0) //first appearance of author
            {
                authors.Add(authorToAdd);
            }
            authorToAdd.NumberOfBooks++;

    

            AppendText(historyRichTextBox,$"Added {bookToAdd.Name}\n","Green");     //add action to history
            
            books.Add($"{bookToAdd.Name} {bookToAdd.ISBN}", bookToAdd);


            try
            {
                if (bookToAdd.Author == GetSelectedBookFromListBox().Author)
                {
                    showAuthorNumberOfBooksTextBox.Text = bookToAdd.Author.NumberOfBooks.ToString();
                }
            }
            catch (NoBookSelectedException) { }

            UpdateListBox();
            ClearTextBoxs();
        }


        public void AppendText(RichTextBox box, string text, string color)
        {
            BrushConverter bc = new BrushConverter();
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                    bc.ConvertFromString(color));
            }
            catch (FormatException) { }
        }
        private void ClearTextBoxs()
        {
            bookISBNTextBox.Text = bookTitleTextBox.Text = bookNumberOfCopiesTextBox.Text = 
                    bookPriceTextBox.Text = authorFirstNameTextBox.Text = authorLastNameTextBox.Text = "";
        }

        private void UpdateListBox()
        {
            bookListBox.Items.Clear();
            foreach (Book b in books.Values)
            {
                bookListBox.Items.Add($"{b.Name} {b.ISBN}");
            }
        }

        private void bookListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Book bookSelected = GetSelectedBookFromListBox();
                showAuthorNameTextBox.Text = bookSelected.Author.FirstName + " " + bookSelected.Author.LastName;
                showAuthorNumberOfBooksTextBox.Text = bookSelected.Author.NumberOfBooks.ToString();
                showNumberOfCopiesTextBox.Text = bookSelected.NumberOfBooks.ToString();
                showBookPriceTextBox.Text = bookSelected.Price.ToString();
            } 
            catch (NoBookSelectedException)
            { 
            }

        }

        private Book GetSelectedBookFromListBox()
        {
            string selectedBook;
            try
            {
                selectedBook = bookListBox.SelectedItem.ToString();
            }
            catch (Exception)
            {
                throw new NoBookSelectedException();
            }
            return books.ElementAt(books.IndexOfKey(selectedBook)).Value;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book selectedBook = GetSelectedBookFromListBox();
                
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedBook.Name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    selectedBook.Author.NumberOfBooks--;
                    books.Remove($"{selectedBook.Name} {selectedBook.ISBN}");
                    UpdateListBox();
                    ClearBookDetailTextBoxs();

                    AppendText(historyRichTextBox, $"Deleted {selectedBook.Name}\n", "Red");     //add action to history

                }
            } 
            catch (NoBookSelectedException exception)
            {
                MessageBox.Show(exception.Message,"Notice");
            }
        }

        private void ClearBookDetailTextBoxs()
        {
            showAuthorNameTextBox.Text = showAuthorNumberOfBooksTextBox.Text = showNumberOfCopiesTextBox.Text = 
                showBookPriceTextBox.Text = "";
        }

        private void updateAmountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book selectedBook = GetSelectedBookFromListBox();
                int oldNumberOfBooks = selectedBook.NumberOfBooks;
                selectedBook.NumberOfBooks = int.Parse(bookNumberOfCopiesTextBox.Text);

                AppendText(historyRichTextBox,$"Change amount of {selectedBook.Name} from {oldNumberOfBooks} to {bookNumberOfCopiesTextBox.Text}\n","Blue");
                showNumberOfCopiesTextBox.Text = selectedBook.NumberOfBooks.ToString();
            }
            catch (NoBookSelectedException exception)
            {
                MessageBox.Show(exception.Message, "Notice");
            }
            catch(FormatException)
            {
                MessageBox.Show("Number of books must be a number", "Notice");
            }
        }

        private void updatePriceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Book selectedBook = GetSelectedBookFromListBox();
                decimal oldPrice = selectedBook.Price;

                selectedBook.Price = decimal.Parse(bookPriceTextBox.Text);

                if (oldPrice < selectedBook.Price)
                {
                    MessageBox.Show("New price cannot exceed previous price", "Notice");
                    return;
                }

                AppendText(historyRichTextBox, $"Change price of {selectedBook.Name} from {oldPrice} to {bookPriceTextBox.Text}\n", "Blue");
                showBookPriceTextBox.Text = selectedBook.Price.ToString();
            }
            catch (NoBookSelectedException exception)
            {
                MessageBox.Show(exception.Message, "Notice");
            }
            catch (FormatException)
            {
                MessageBox.Show("Price must be a number", "Notice");
            }
        }


        string path = @"..\..\library.xml";
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (books.Count > 0)
            {
                XmlManager xmlConnection = new XmlManager(path);
                xmlConnection.Write(books);
            }
            else
            {
                File.Delete(path);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path))
            {
                XmlManager xmlConnection = new XmlManager(path);
                xmlConnection.Read(books, authors);
                UpdateListBox();
            }
        }



    }
}
