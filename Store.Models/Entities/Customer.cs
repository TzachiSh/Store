using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Store.Models.Entities.Base;

namespace Store.Models.Entities
{
    [Table("Customers", Schema = "Store")]
    public class Customer : EntityBase
    {

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }
        [DataType(DataType.Text), MaxLength(50), Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [InverseProperty(nameof(Order.Customer))]
        public List<Order> Orders { get; set; }= new List<Order>();
        [InverseProperty(nameof(ShoppingCartRecord.Customer))]
        public virtual List<ShoppingCartRecord> ShoppingCartRecords { get; set; }
        = new List<ShoppingCartRecord>();
        



    }
}