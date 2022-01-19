using Microsoft.Extensions.Configuration;
using ServiceBusTrigger.Models;
using System;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace ServiceBusTrigger.Data
{
    public class ReceiveNotification : IReceiveNotification
    {

        private readonly IConfiguration _configuration;
        private readonly ISendNotification _sendNotification;

        public event EventHandler ItemReceived;

        public ReceiveNotification(IConfiguration configuration, ISendNotification sendNotification)
        {
            _configuration = configuration;
            _sendNotification = sendNotification;
        }


        public void ReceiveNotificationFromSQL()
        {
            try
            {
                var mapper = new ModelToTableMapper<Product>();
                mapper.AddMapping(c => c.Name, "Name");
                mapper.AddMapping(c => c.ProductNumber, "ProductNumber");

                // Here - as second parameter - we pass table name: 
                // this is necessary only if the model name is different from table name 
                // (in our case we have Customer vs Customers). 
                // If needed, you can also specifiy schema name.

                using (var dep = new SqlTableDependency<Product>(_configuration.GetConnectionString("sqlConnection"),
                                                                    tableName: "Product",
                                                                    schemaName: "SalesLT",
                                                                    mapper: mapper))
                {
                    dep.OnChanged += ChangedAsync;
                    dep.Start();

                    Console.WriteLine();
                    Console.ReadKey();

                    dep.Stop();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void ChangedAsync(object sender, RecordChangedEventArgs<Product> e)
        {
            var changedEntity = e.Entity;
            Product product;

            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert || e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update)
            {
                Console.WriteLine("DML operation: " + e.ChangeType);
                product = new Product()
                {
                    Color = changedEntity.Color,
                    DiscontinuedDate = changedEntity.DiscontinuedDate,
                    ListPrice = changedEntity.ListPrice,
                    Name = changedEntity.Name,
                    ProductCategory = changedEntity.ProductCategory,
                    ProductModel = changedEntity.ProductModel
                };
                Task.Run(async () => await _sendNotification.SendNotificationToServiceBus(product));

            }
        }


        #region CommentedCode

        //public List<Address> GetAddressDetails()
        //{
        //    var AddressDetails = new List<Address>();
        //    Address _Address = null;

        //    connection = _configuration.GetConnectionString("sqlConnection");

        //    try
        //    {
        //        if (isFirst)
        //        {
        //            SqlDependency.Stop(connection);
        //            SqlDependency.Start(connection);
        //        }
        //        Console.WriteLine("Listening..");
        //        using (SqlConnection con = new SqlConnection(connection))
        //        {
        //            con.Open();

        //            SqlCommand cmd = new SqlCommand($@"SELECT [AddressID]
        //                                          ,[AddressLine1]
        //                                          ,[AddressLine2]
        //                                          ,[City]
        //                                          ,[StateProvince]
        //                                          ,[CountryRegion]
        //                                          ,[PostalCode]
        //                                          ,[ModifiedDate]
        //                                      FROM[AdventureWorksLT2014].[SalesLT].[Address]", con);

        //            SqlDependency dependency = new SqlDependency(cmd);
        //            // Subscribe to the SqlDependency event.
        //            dependency.OnChange += new
        //               OnChangeEventHandler(OnDependencyChange);


        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                _Address = new Address();
        //                _Address.AddressId = Convert.ToInt32(reader["AddressId"]);
        //                _Address.AddressLine1 = Convert.ToString(reader["AddressLine1"]);
        //                _Address.City = Convert.ToString(reader["City"]);
        //                _Address.StateProvince = Convert.ToString(reader["StateProvince"]);
        //                _Address.PostalCode = Convert.ToString(reader["PostalCode"]);
        //                AddressDetails.Add(_Address);
        //            }
        //            con.Close();

        //        }

        //        Console.ReadLine();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return AddressDetails;
        //}
        //private void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        //{
        //    isFirst = false;
        //    var sometype = e.Info;
        //    //call notify item again 
        //    //   NotifyNewItem();
        //    //if it s an insert notify the calling class 
        //    if (sometype == SqlNotificationInfo.Insert || sometype == SqlNotificationInfo.Update)
        //        onItemReceived(e);
        //    SqlDependency dep = sender as SqlDependency;
        //    //unsubscribe 
        //    dep.OnChange -= new OnChangeEventHandler(OnDependencyChange);
        //}
        //private void onItemReceived(SqlNotificationEventArgs eventArgs)
        //{
        //    EventHandler handler = ItemReceived;
        //    if (handler != null)
        //        handler(this, eventArgs);
        //}
        #endregion CommentedCode
    }
}
