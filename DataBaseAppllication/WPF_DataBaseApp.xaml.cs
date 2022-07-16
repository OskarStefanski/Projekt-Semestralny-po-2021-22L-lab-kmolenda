using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataBaseAppllication
{
    /// <summary>
    /// Logika interakcji dla klasy WPF_DataBaseApp.xaml
    /// </summary>
    public partial class WPF_DataBaseApp : Window

    {
        public WPF_DataBaseApp()
        {
            InitializeComponent();

            CarPartsEntities db = new CarPartsEntities();
            var docs = from d in db.Tables
                       select new
                       {
                           PartName = d.Name,
                           Manufacturer = d.Manufacturer,
                           Index = d.Index,
                       };

            foreach (var item in docs)
            {
                Console.WriteLine(item.PartName);
                Console.WriteLine(item.Manufacturer);
                Console.WriteLine(item.Index);

            }

            this.CarpartsGrid.ItemsSource = docs.ToList();
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            CarPartsEntities db = new CarPartsEntities();

            Table CarpartObject = new Table()
            {
                Name = txtName.Text,
                Manufacturer = txtManufacturer.Text,
                Index = txtIndex.Text,
            };

            db.Tables.Add(CarpartObject);
            db.SaveChanges();

        }

        private void btnLoadParts_Click(object sender, RoutedEventArgs e)
        {
            CarPartsEntities db = new CarPartsEntities();
          

            this.CarpartsGrid.ItemsSource = db.Tables.ToList();
        }

        private int updatingCarPartID = 0;
        private void CarpartsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CarpartsGrid.SelectedIndex >= 0)
            if (this.CarpartsGrid.SelectedItems.Count >= 0)
            {
                if (this.CarpartsGrid.SelectedItems[0].GetType() == typeof(Table))
                {
                    Table d = (Table)this.CarpartsGrid.SelectedItems[0];
                    this.txtName2.Text = d.Name;
                    this.txtManufacturer2.Text = d.Manufacturer;
                    this.txtIndex2.Text = d.Index;
                    this.updatingCarPartID = d.Id;
                }
            }
        }

        private void btnUpdateParts_Click(object sender, RoutedEventArgs e)
        {
            CarPartsEntities db = new CarPartsEntities();

            var r = from d in db.Tables
                    where d.Id == this.updatingCarPartID
                    select d;

            Table obj = r.SingleOrDefault();

            if (obj != null)
            {
                obj.Name = this.txtName2.Text;
                obj.Manufacturer = this.txtManufacturer2.Text;
                obj.Index = this.txtIndex2.Text;

                db.SaveChanges();
            }
            
        }

        private void btnDeleteParts_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this part?", "Delete Part?",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning,
               MessageBoxResult.No);

            if (messageBoxResult == MessageBoxResult.Yes);

            CarPartsEntities db = new CarPartsEntities();

            var r = from d in db.Tables
                    where d.Id == this.updatingCarPartID
                    select d;

            Table obj = r.SingleOrDefault();
           

            if (obj != null)
            {
                db.Tables.Remove(obj);

                db.SaveChanges();
            }

          
            
        }
    }
}
