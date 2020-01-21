namespace WpfEFMVVMSample
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("product")]
    public partial class product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int pid { get; set; }

        [StringLength(50)]
        public string pdtname { get; set; }

        [Column(TypeName = "money")]
        public decimal? unitprice { get; set; }
    }
}
