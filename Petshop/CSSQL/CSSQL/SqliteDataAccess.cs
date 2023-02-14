using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSSQL
{
    public class SqliteDataAccess
    {
        private static List<ProductModel> productList;

        public static List<ProductModel> LoadProduct()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                List<ProductModel> output = cnn.Query<ProductModel>("SELECT * FROM Products", new DynamicParameters()).ToList();
                return output;
            }
        }
        public static List<ShopModel> LoadShops()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                List<ShopModel> output = cnn.Query<ShopModel>("SELECT * FROM Shops", new DynamicParameters()).ToList();
                Console.WriteLine(output.Count);
                return output;
            }
        }
        

        public static void AddCustomer(CustomerModel cs)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO Customers (Name, Address, Zipcode, Password) VALUES (@Name, @Address, @Zipcode, @Password)", cs);
            }
        }
        public static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }



        public static void BuyProdukt(int customerID, int id, int amount)
        {
            bool amountLimit = true;

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                productList = LoadProduct();
                int shopAmount = productList[id - 1].Amount;
                int productPrice = productList[id - 1].Price;
                if (shopAmount >= amount)
                {
                    amountLimit = false;
                }

                while (amountLimit == true)
                {
                    Console.WriteLine("There are only " + shopAmount + " of this product left. \nPlease rewrite your answer");
                    try
                    {
                        amount = int.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nPlease write a number");
                        continue;
                    }
                    amount = int.Parse(Console.ReadLine());
                }
                int totalPrice = amount * productPrice;
                int storeChange = shopAmount - amount;
                cnn.Execute("INSERT INTO Transactions " +
                    "(ProductID, CustomerID, Amount, totalPrice, productName) " +
                    "VALUES ("+ id + ", " + Global.userID + ", " + amount + ", " + totalPrice + "," + "'" + Global.productName + "'" + ")");
                
                cnn.Execute("UPDATE Products" + " SET Amount = " + storeChange +" WHERE ID = " + id + ";");
                
                
            }
        }
        public static List<TransactionModel> ViewHistory()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                Console.Clear();
                List<TransactionModel> output = cnn.Query<TransactionModel>(
                    "SELECT * FROM Transactions WHERE customerID=" + Global.userID +";",new DynamicParameters()).ToList();
                for (int i = 0; i < output.Count; i++)
                {
                    int number = i + 1;

                    Console.WriteLine("[" + number + "] " + "Product: " + output[i].productName + " | Amount: " + output[i].amount + " | Total price: " + output[i].totalPrice);
                    Console.WriteLine("------------------------------------------------------------------------------------------");
                }
                Console.WriteLine("Type anything to return to the menu");
                Console.ReadLine();
                Console.Clear();

                return output;
            }
        }


    }
}
