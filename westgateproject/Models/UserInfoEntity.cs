﻿using System;
namespace westgateproject.Models
{
    public class UserInfoEntity : Microsoft.WindowsAzure.Storage.Table.TableEntity
    {
		public UserInfoEntity(string id, string shopLocation, string shopName, string phoneNumber, string addInfo)
		{
			PartitionKey = id;
			RowKey = shopLocation;

			ShopName = shopName;
			PhoneNumber = phoneNumber;
			Paid = false;
			AddInfo = addInfo;
		}

		public UserInfoEntity() { }


		public string ShopName { get; set; }
		public string PhoneNumber { get; set; }
		public bool Paid { get; set; }
		public string AddInfo { get; set; }
        public DateTime Period { get; set; }
    }
}
