using System;
using System.Collections.Generic;
using NezarkaBookstore;


namespace htmlviews
{
    class View
    {
        public static void Header(string name, int number)
        {
            Console.WriteLine("<!DOCTYPE html>");
            Console.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Console.WriteLine("<head>");
            Console.WriteLine("	<meta charset=\"utf-8\" />");
            Console.WriteLine(" <title>Nezarka.net: Online Shopping for Books</title>");
            Console.WriteLine("</head>");
            Console.WriteLine("<body>");
            Console.WriteLine(" <style type=\"text/css\">");
            Console.WriteLine("     table, th, td {");
            Console.WriteLine("         border: 1px solid black;");
            Console.WriteLine("         border-collapse: collapse;");
            Console.WriteLine("     }");
            Console.WriteLine("     table {");
            Console.WriteLine("         margin-bottom: 10px;");
            Console.WriteLine("     }");
            Console.WriteLine("     pre {");
            Console.WriteLine("         line-height: 70%;");
            Console.WriteLine("     }");
            Console.WriteLine(" </style>");
            Console.WriteLine(" <h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            Console.WriteLine(" "+name+", here is your menu:");
            Console.WriteLine(" <table>");
            Console.WriteLine("     <tr>");
            Console.WriteLine("         <td><a href=\"/Books\">Books</a></td>");
            Console.WriteLine("         <td><a href=\"/ShoppingCart\">Cart ("+number+")</a></td>");
            Console.WriteLine("     </tr>");
            Console.WriteLine(" </table>");
        }

        public static void End()
        {
            Console.WriteLine("</body>");
            Console.WriteLine("</html>");
        }

        public static void InvalidRequest()
        {
            Console.WriteLine("<!DOCTYPE html>");
            Console.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            Console.WriteLine("<head>");
            Console.WriteLine(" <meta charset=\"utf-8\" />");
            Console.WriteLine(" <title>Nezarka.net: Online Shopping for Books</title>");
            Console.WriteLine("</head>");
            Console.WriteLine("<body>");
            Console.WriteLine("<p>Invalid request.</p>");
            End();
        }

        public static void ViewAllBooks(List<Book> books, Customer customer)
        {
            Header(customer.FirstName, customer.ShoppingCart.Items.Count);
            Console.WriteLine(" Our books for you:");
            Console.WriteLine(" <table>");
            int counter = 0;
            foreach (Book book in books)
            {
                if (counter % 3 == 0)
                    Console.WriteLine("     <tr>");
                Console.WriteLine("         <td style=\"padding: 10px;\">");
                Console.WriteLine("             <a href=\"/Books/Detail/"+book.Id+"\">"+book.Title+"</a><br />");
                Console.WriteLine("             Author: "+book.Author+"<br />");
                Console.WriteLine("             Price: "+book.Price+" EUR &lt;<a href=\"/ShoppingCart/Add/"+book.Id+"\">Buy</a>&gt;");
                Console.WriteLine("         </td>");
                if (counter % 3 == 2)
                    Console.WriteLine("     </tr>");
                counter++;
            }
            if (!(counter - 1 % 3 == 2) && counter != 0)
                Console.WriteLine("     </tr>");
            Console.WriteLine(" </table>");
            End();
        }

        public static void ViewOneBook(Book book, Customer customer)
        {
            Header(customer.FirstName, customer.ShoppingCart.Items.Count);
            Console.WriteLine(" Book details:");
            Console.WriteLine(" <h2>"+book.Title+"</h2>");
            Console.WriteLine(" <p style=\"margin-left: 20px\">");
            Console.WriteLine(" Author: "+book.Author+"<br />");
            Console.WriteLine(" Price: "+book.Price+" EUR<br />");
            Console.WriteLine(" </p>");
            Console.WriteLine(" <h3>&lt;<a href=\"/ShoppingCart/Add/"+book.Id+"\">Buy this book</a>&gt;</h3>");
            End();
        }

        public static void ViewCartContent(Customer customer, ModelStore model_store)
        {
            Header(customer.FirstName, customer.ShoppingCart.Items.Count);
            if (customer.ShoppingCart.Items.Count == 0)
                Console.WriteLine(" Your shopping cart is EMPTY.");
            else
            {
                Console.WriteLine(" Your shopping cart:");
                Console.WriteLine(" <table>");
                Console.WriteLine("     <tr>");
                Console.WriteLine("         <th>Title</th>");
                Console.WriteLine("         <th>Count</th>");
                Console.WriteLine("         <th>Price</th>");
                Console.WriteLine("         <th>Actions</th>");
                Console.WriteLine("     </tr>");
                decimal totalPrice = 0;
                foreach (ShoppingCartItem item in customer.ShoppingCart.Items)
                {
                    decimal priceOfThisItem = item.Count * model_store.GetBook(item.BookId).Price;
                    totalPrice += priceOfThisItem;
                    Console.WriteLine("     <tr>");
                    Console.WriteLine("         <td><a href=\"/Books/Detail/"+item.BookId+"\">"+ model_store.GetBook(item.BookId).Title + "</a></td>");
                    Console.WriteLine("         <td>{0}</td>", item.Count);
                    if (item.Count > 1)
                        Console.WriteLine("         <td>"+item.Count+" * "+ model_store.GetBook(item.BookId).Price + " = "+priceOfThisItem+" EUR</td>");
                    else Console.WriteLine("            <td>"+ model_store.GetBook(item.BookId).Price + " EUR</td>");
                    Console.WriteLine("         <td>&lt;<a href=\"/ShoppingCart/Remove/"+item.BookId+"\">Remove</a>&gt;</td>");
                    Console.WriteLine("     </tr>");
                }
                Console.WriteLine(" </table>");
                Console.WriteLine(" Total price of all items: "+totalPrice+" EUR");
            }
            End();
        }

        public static void AddBook(int customerId, int bookId, ModelStore modelStore)
        {
            Customer customer = modelStore.GetCustomer(customerId);
            bool contains = false;
            foreach (ShoppingCartItem item in customer.ShoppingCart.Items)
                if (item.BookId == bookId)
                {
                    item.Count++;
                    contains = true;
                }
            if (!contains)
                customer.ShoppingCart.Items.Add(new ShoppingCartItem { BookId = bookId, Count = 1 });
            ViewCartContent(customer, modelStore);

        }

        public static void RemoveBook(int customerId, int bookId, ModelStore modelStore)
        {
            Customer customer = modelStore.GetCustomer(customerId);
            try
            {
                ShoppingCartItem item = customer.ShoppingCart.Items.Find(x => x.BookId == bookId);
                if (item.Count == 1)
                    customer.ShoppingCart.Items.Remove(item);
                else item.Count--;
                ViewCartContent(customer, modelStore);
            }
            catch
            {
                InvalidRequest();
            }
        }
    }
}
