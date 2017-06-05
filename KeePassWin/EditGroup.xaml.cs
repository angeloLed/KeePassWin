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
                note.Document.SetText( Windows.UI.Text.TextSetOptions.None, gk.Note ?? "");

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
            if (App.CurrentDb.Groups == null) {
                App.CurrentDb.Groups = new ObservableCollection<GroupKeys>();
            }

            if (this.gk == null) {

                //new
                string richText;
                note.Document.GetText(Windows.UI.Text.TextGetOptions.None, out richText);
                App.CurrentDb.Groups.Add(new GroupKeys {
                    Icon = previewIcon.Text,
                    Name = name.Text,
                    Keys = new ObservableCollection<Key>(),
                    Description = desc.Text,
                    Note = richText,
                    CreateAt = DateTime.Now.ToString(),
                    UpdateAt = DateTime.Now.ToString()
                });
            }
            else
            {
                string richText;
                note.Document.GetText(Windows.UI.Text.TextGetOptions.None, out richText);
                //update
                gk.Icon = previewIcon.Text;
                gk.Name = name.Text;
                gk.Description = desc.Text;
                gk.Note = richText;
                gk.UpdateAt = DateTime.Now.ToString();
            }
           
            this.Frame.GoBack();
        }

        private async void icon_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogs.IconsGrid dialog = new KeePassWin.ContentDialogs.IconsGrid();
            await dialog.ShowAsync();
            if (dialog.SelectedEmoji != null) {
                previewIcon.Text = dialog.SelectedEmoji.GetIcon();
            }
            
        }

        private void panel_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                if (FocusManager.GetFocusedElement() != buttonSave)
                {
                    FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
                }
                else
                {
                    buttonSave_Click(sender, e);
                }
            }
        }
    }
}
