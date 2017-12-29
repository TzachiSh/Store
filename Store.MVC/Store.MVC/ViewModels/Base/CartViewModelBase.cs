﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Store.Models.ViewModels.Base;

namespace Store.MVC.ViewModels.Base
{
    public class CartViewModelBase : ProductAndCategoryBase
    {
        public int? CustomerId { get; set; }

        [DataType(DataType.Currency), Display(Name = "Total")]
        public decimal LineItemTotal { get; set; }
        public string TimeStampString => 
           TimeStamp != null ? JsonConvert.SerializeObject(TimeStamp).Replace("\"",""):string.Empty;
    }
}