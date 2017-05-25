using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace KeePassWin
{

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

    }

    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Person> persons;
        private Db db;

        public MainPage()
        {
            this.InitializeComponent();

            this.persons = new List<Person>();
            this.persons.Add(new Person { Name = "asd2", Age = 45 });
            this.persons.Add(new Person { Name = "asd3", Age = 47 });

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //this.db = (Db)e.Parameter;
            
        }

        private async void Convert_Click(object sender, RoutedEventArgs e)
        {
            /*
            //esempio di serializzazione e deserializzazione
            Person person = new Person() { Name = "Asd", Age = 23 };
            serialize.Text = JsonConvert.SerializeObject(person);
            deserialize.Text = JsonConvert.DeserializeObject<Person>(serialize.Text).ToString();

            // da cambiare
            this.persons.Add(JsonConvert.DeserializeObject<Person>(serialize.Text));
            gggrid.ItemsSource = null;
            gggrid.ItemsSource = this.persons;
            */

            OnedriveArbiter test = new OnedriveArbiter();
            test.Connect();
        }

        /*
        private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
        {
            Uri bUrl = new Uri("http://it-ebooks-api.info/v1/search/" + textBox.Text);
            HttpClient cli = new HttpClient();
            string content = await cli.GetStringAsync(bUrl);
            RootObject roj = JsonConvert.DeserializeObject<RootObject>(content);
            reslist.ItemsSource = roj.Books;
        }*/
    }
}
