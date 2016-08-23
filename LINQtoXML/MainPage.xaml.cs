using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Controls;

namespace LINQtoXML
{
    public class Censo_Camas
    {
        public string Fecha { get; set; }
        public string Unidad_Funcional { get; set; }
        public int Censo { get; set; }
        //public string IsMale { get; set; }
    }
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += wc_OpenReadCompleted;
            wc.OpenReadAsync(new Uri("http://valme_15:8080/oradb/ALMAGESTO/DAE_ULTIMO_CENSO_CAMAS"));

        }

        private void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            using (Stream s = e.Result)
            {
                XDocument doc = XDocument.Load(s);
                var PlaylistItems = from row in
                                        doc.Descendants("ROW")
                                    select new Censo_Camas
                                    {
                                        Fecha = Convert.ToString(row.Element("FECHA").Value),
                                        Unidad_Funcional = Convert.ToString(row.Element("COD_UF").Value),
                                        Censo = Convert.ToInt32(row.Element("CENSO").Value),
                                    };
                PlayListDataGrid.SelectedIndex = -1;
                PlayListDataGrid.ItemsSource = PlaylistItems;
            }
        }

    }
    
}
