using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class EditGroup : Page
    {
        GroupKeys gk;

        public EditGroup()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.gk = (GroupKeys)e.Parameter;

            if (gk != null) {
                previewIcon.Text = gk.Icon ?? "";
                name.Text = gk.Name ?? "";
                desc.Text = gk.Description ?? "";
                note.Text = gk.Note ?? "";

                DateTime datetime;
                if (DateTime.TryParse(gk.CreateAt, out datetime))
                {
                    createdAt.Text = datetime.ToString();
                }
                if (DateTime.TryParse(gk.UpdateAt, out datetime))
                {
                    updateAt.Text = datetime.ToString();
                }
            }

            Utils.SetTitlepage("Edit Group");
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (App.currentDb.Groups == null) {
                App.currentDb.Groups = new ObservableCollection<GroupKeys>();
            }

            if (this.gk == null) {

                //new
                App.currentDb.Groups.Add(new GroupKeys {
                    Icon = previewIcon.Text,
                    Name = name.Text,
                    Keys = new ObservableCollection<Key>(),
                    Description = desc.Text,
                    Note = note.Text,
                    CreateAt = DateTime.Now.ToUniversalTime().ToString(),
                    UpdateAt = DateTime.Now.ToUniversalTime().ToString()
                });
            }
            else {
                //update
                gk.Icon = previewIcon.Text;
                gk.Name = name.Text;
                gk.Description = desc.Text;
                gk.Note = note.Text;
                gk.UpdateAt = DateTime.Now.ToUniversalTime().ToString();
            }
           
            this.Frame.GoBack();
        }

        private async void icon_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogs.IconsGrid dialog = new KeePassWin.ContentDialogs.IconsGrid();
            await dialog.ShowAsync();
            previewIcon.Text = dialog.SelectedEmoji.GetIcon();
        }
    }
}
