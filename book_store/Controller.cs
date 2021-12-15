using System;
using NezarkaBookstore;
using System.IO;
using System.Collections.Generic;
using htmlviews;

namespace book_store
{
    
    class Controller
    {
        public static void Command(string line, ModelStore model_store)
        {
            
                try
                {
                    string[] input = line.Split(' ');
                    int Id = int.Parse(input[1]);
                    var customer = model_store.GetCustomer(Id);
                    if (input[0] == "GET" && input[2].Substring(0, 23) == "http://www.nezarka.net/" && customer != null)
                    {
                        string[] parsedSubstring = input[2].Substring(23).Split('/');
                    if (parsedSubstring.Length < 4)
                    {
                        if (parsedSubstring[0] == "Books")
                        {
                            if (parsedSubstring.Length == 1)
                                View.ViewAllBooks(model_store.GetBooks(), customer);
                            else if (parsedSubstring[1] == "Detail" && parsedSubstring.Length == 3)
                                if (model_store.GetBook(int.Parse(parsedSubstring[2])) != null)
                                    View.ViewOneBook(model_store.GetBook(int.Parse(parsedSubstring[2])), customer);
                                else View.InvalidRequest();
                            else View.InvalidRequest();

                        }

                        else if (parsedSubstring[0] == "ShoppingCart")
                        {
                            if (parsedSubstring.Length == 1)
                                View.ViewCartContent(customer, model_store);
                            else if (parsedSubstring[1] == "Add" && parsedSubstring.Length == 3)
                                if (model_store.GetBook(int.Parse(parsedSubstring[2])) != null)
                                    View.AddBook(Id, int.Parse(parsedSubstring[2]), model_store);
                                else View.InvalidRequest();
                            else if (parsedSubstring[1] == "Remove" && parsedSubstring.Length == 3)
                                if (model_store.GetBook(int.Parse(parsedSubstring[2])) != null)
                                    View.RemoveBook(Id, int.Parse(parsedSubstring[2]), model_store);
                                else View.InvalidRequest();
                            else
                                View.InvalidRequest();
                        }
                        else View.InvalidRequest();

                    }
                    else View.InvalidRequest();
                    }

                    else View.InvalidRequest();
                }
                catch
                {
                View.InvalidRequest();
                }
            
        }

        static void Main(string[] args)
        {
            var input = Console.In;
            ModelStore model_store = ModelStore.LoadFrom(input);
            if (model_store == null)
            {
                Console.WriteLine("Data error.");
                return;
            }

            string line;
            while ((line = input.ReadLine()) != null)
            {
                Command(line, model_store);
                Console.WriteLine("====");
            }
        }
    }
}
