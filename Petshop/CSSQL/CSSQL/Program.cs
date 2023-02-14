using System;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using Dapper;
using System.Xml.Linq;

namespace CSSQL
{

    internal class Program
    {
        public static List<ProductModel> productList = new List<ProductModel>();
        public static List<ShopModel> shopList = new List<ShopModel>();
        public bool passwordReal = true;
        static void Main(string[] args)
        {
            bool closeApp = false;
            bool closeApp2 = false;
            while (closeApp == false)
            {
                Console.WriteLine("Welcome to the greatest petshop in the WORLD!!!!!\n\n");
                //Registre og login
                {
                    Console.WriteLine("Please choose an option\n");
                    Console.WriteLine("Type 0 to close app");
                    Console.WriteLine("Type 1 to register new user");
                    Console.WriteLine("Type 2 to login with existing user");
                    string Option1 = Console.ReadLine();
                    switch (Option1)
                    {
                        case "0":
                            closeApp = true;
                            Environment.Exit(0);
                            break;


                        case "1":
                            //registre kunde
                            int zipcodeInt = 0;

                            Console.WriteLine("Enter your name: ");
                            string userName = Console.ReadLine();

                            Console.WriteLine("Enter your address: ");
                            string userAddress = Console.ReadLine();

                            bool zipcodeCheck = true;
                            while (zipcodeCheck == true)
                            {
                                Console.WriteLine("Enter your zipcode: ");
                                string userZipcode = Console.ReadLine();
                                try
                                {
                                    int Option5Input = int.Parse(userZipcode);

                                }
                                catch
                                {
                                    Console.WriteLine("\nPlease write a valid option\n");
                                    continue;
                                }
                                zipcodeInt = int.Parse(userZipcode);
                                zipcodeCheck = false;
                            }
                            

                            Console.WriteLine("Enter your Password: ");
                            string userPassword = Console.ReadLine();

                            CustomerModel cs = new CustomerModel();

                            cs.Name = userName;
                            cs.Address = userAddress;
                            cs.Zipcode = zipcodeInt;
                            cs.Password = userPassword;
                            
                            SqliteDataAccess.AddCustomer(cs);
                            using (IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString()))
                            {
                                List<CustomerModel> output = cnn.Query<CustomerModel>("SELECT * FROM Customers WHERE Name = '" + userName + "'", new DynamicParameters()).ToList();
                                Global.userID = output[0].ID;


                            }
                            break;
                        case "2":
                            //kunde login
                            Console.Clear();
                            while (true)
                            {
                                Console.WriteLine("Please write your name");
                                string customerName = Console.ReadLine();

                                Console.WriteLine("Please write your password");
                                string customerPassword = Console.ReadLine();
                                using (IDbConnection cnn = new SQLiteConnection(SqliteDataAccess.LoadConnectionString()))
                                {
                                    List<CustomerModel> output = cnn.Query<CustomerModel>("SELECT * FROM Customers WHERE Name = '" + customerName + "'", new DynamicParameters()).ToList();
                                    string passwordCheck = output[0].Password;

                                    
                                    
                                    if (customerPassword == passwordCheck)
                                    {
                                        Global.userID = output[0].ID;
                                        break;
                                    }
                                    else
                                    {
                                       Console.Clear();
                                       Console.WriteLine("Wrong password or username, please try again\n");
                                    }
                                    
                                }
                            
                                
                            }
                            


                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Please write a valid option\n");
                            continue;
                    }

                    Console.Clear();
                    while(closeApp2 == false)
                    {
                        Console.WriteLine("Please choose an option");

                        Console.WriteLine("\nType 0 to close app");
                        Console.WriteLine("Type 1 to view different store locations");
                        Console.WriteLine("Type 2 to view Products");
                        Console.WriteLine("Type 3 to view your previous purchases");
                        
                        string Option2 = Console.ReadLine();

                        switch (Option2)
                        {
                            case "0":
                                closeApp = true;
                                Environment.Exit(0);
                                break;
                                 

                            case "1":
                                //Vis buttikker
                                Console.Clear();
                                shopList = SqliteDataAccess.LoadShops();
                                
                                for (int u = 0; u < shopList.Count; u++)
                                {
                                    Console.WriteLine("[" + shopList[u].ID + "] - Shop Address:" + shopList[u].address + "| City: " + shopList[u].cityName + " - " + shopList[u].zipcode);
                                    
                                }
                                Console.WriteLine("\nType anything to to menu");
                                Console.ReadLine();
                                Console.Clear();

                                break;
                            case "2":
                                //Vis produkter
                                productList = SqliteDataAccess.LoadProduct();
                                
                                for (int i = 0; i < productList.Count; i++)
                                {
                                    Console.WriteLine("[" + productList[i].ID + "] - " + productList[i].Name + "| Price = " + productList[i].Price + " USD | Amount Availible = " + productList[i].Amount );
                                }
                                int buyID;
                                string productName;
                                while (true)
                                {
                                    Console.WriteLine("\nType the ID on the Produkt you would like to buy, or type 0 to exit to menu");
                                    string buyInput = Console.ReadLine();
                                    try
                                    {
                                        buyID = Convert.ToInt32(buyInput);
                                        productName = productList[buyID - 1].Name;
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Please type a valid option");
                                        continue;
                                    }
                                    buyID = Convert.ToInt32(buyInput);
                                    productName = productList[buyID - 1].Name;
                                    break;
                                }
                                

                                Console.Clear();
                                Global.productName = productName;
                                Console.WriteLine("Are you sure you want to buy " + productName + "? Y/N");
                                string Option4 = Console.ReadLine();
                                switch (Option4)
                                {
                                    case "Y":
                                        Console.WriteLine("How many would you like to buy");
                                        string buyAmount = Console.ReadLine();
                                        try
                                        {
                                            int Option5Input = int.Parse(buyAmount);

                                        }
                                        catch
                                        {
                                            Console.WriteLine("\nPlease write a valid option\n");
                                            continue;
                                        }
                                        int buyAmountInt = int.Parse(buyAmount);
                                        SqliteDataAccess.BuyProdukt(Global.userID, buyID, buyAmountInt);

                                        break;
                                    case "N":
                                        Console.Clear();
                                        break;
                                    default:
                                        Console.Clear();
                                        Console.WriteLine("Please write a valid option\n");
                                        break;


                                }
                                break;
                            case "3":
                                SqliteDataAccess.ViewHistory();
                                break;

                                
                                
                            default:
                                Console.Clear();
                                Console.WriteLine("Please write a valid option\n");
                                break;
                        }
                        
                    
                    }

                }


            }
        }
    }
}