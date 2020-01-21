using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEFMVVMSample.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public List<product> products { get; set; }


        //public product selectedProduct { get; set; }

        private product _selectedProduct;
        public product SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                _selectedProduct = value;
                NotifyPropertyChanged("SelectedProduct");
            }
        }

        public ProductViewModel()
        {

            products = new List<product>()
            {
                new product(){ pid = 1, pdtname = "Product1", unitprice = 2},
                new product(){ pid = 2, pdtname = "Product2", unitprice = 3},
                new product(){ pid = 3, pdtname = "Product3", unitprice = 4},
                new product(){ pid = 4, pdtname = "Product4", unitprice = 5},
                new product(){ pid = 5, pdtname = "Product5", unitprice = 6},
                new product(){ pid = 6, pdtname = "Product6", unitprice = 7},
                new product(){ pid = 7, pdtname = "Product7", unitprice = 8},
                new product(){ pid = 8, pdtname = "Product8", unitprice = 9},
                new product(){ pid = 9, pdtname = "Product9", unitprice = 10},
            };
            //using (var db = new PTContext())
            //{
            //   products = db.products.ToList();

            //    SelectedProduct = db.products.Find(1);
            //}


        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
