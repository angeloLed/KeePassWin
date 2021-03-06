﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeePassWin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditKey : Page
    {
        private Key key = null;
        private ObservableCollection<Key> keys = null;
        public EditKey()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter.GetType() == typeof(Key)) {
                this.key = (Key)e.Parameter;

                previewIcon.Text = key.Icon ?? "&#xE192;";
                title.Text = key.Title ?? "";
                note.Document.SetText(Windows.UI.Text.TextSetOptions.None, key.Note ?? "");
                password.Password = key.Password ?? "";
                url.Text = key.Url ?? "";
                username.Text = key.Username ?? "";

                DateTime datetime;
                if (DateTime.TryParse(key.CreateAt, out datetime)) {
                    createdAt.Text = datetime.ToString();
                }
                if (DateTime.TryParse(key.UpdateAt, out datetime))
                {
                    updateAt.Text = datetime.ToString();
                }


            } else if (e.Parameter.GetType() == typeof(ObservableCollection<Key>)) {
                this.key = new Key();
                this.keys = (ObservableCollection<Key>)e.Parameter;
            }

            Utils.SetTitlepage("Edit Key");
        }

        private bool checkPassword()
        {
            bool ok = true;

            if (password.Password != key.Password) {
                if (passwordR.Password != password.Password) {
                    ok = false;
                }
            }

            return ok;
        }

        private async void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!checkPassword()) {
                ContentDialogs.WrongPassword dialog = new KeePassWin.ContentDialogs.WrongPassword();
                await dialog.ShowAsync();

                return;
            }

            string richText;
            note.Document.GetText(Windows.UI.Text.TextGetOptions.None, out richText);
            key.Title = title.Text;
            key.Note = richText;
            key.Password = password.Password;
            key.Url = url.Text;
            key.Username = username.Text;
            key.Icon = previewIcon.Text;
            key.UpdateAt = DateTime.Now.ToString();

            if (this.keys != null) {

                this.key.CreateAt = DateTime.Now.ToString();
                this.keys.Add(this.key);
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

        private void showPassword_Click(object sender, RoutedEventArgs e)
        {
            password.PasswordRevealMode = (password.PasswordRevealMode == PasswordRevealMode.Visible) ? PasswordRevealMode.Hidden : PasswordRevealMode.Visible;
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
