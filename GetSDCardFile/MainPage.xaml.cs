using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GetSDCardFile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Items = new ObservableCollection<Model>();
            this.DataContext = this;
        }

        public ObservableCollection<Model> Items { get; set; }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the first child folder, which represents the SD card.
            //picker.SuggestedStartLocation = PickerLocationId.Desktop;
            FolderPicker picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            StorageFolder sdCard = await picker.PickSingleFolderAsync();
            //StorageFolder externalDevices = Windows.Storage.KnownFolders.RemovableDevices;
            //StorageFolder sdCard = (await externalDevices.GetFoldersAsync()).FirstOrDefault();
            if (sdCard != null)
            {
                var storageFolders = await sdCard.GetFoldersAsync();
                Debug.WriteLine(storageFolders.Count);
                foreach (var folder in storageFolders)
                {
                    var FolderThumb = await folder.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(FolderThumb);
                    Model model = new Model { ImSource = bitmap };
                    Items.Add(model);
                }

                var storageFiles = await sdCard.GetFilesAsync();
                Debug.WriteLine(storageFiles.Count);
                foreach(var file in storageFiles)
                {
                    var FileThumb = await file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(FileThumb);
                    Model model = new Model { ImSource = bitmap };
                    Items.Add(model);
                }
            }
            else
            {
                // No SD card is present.
            }
        }
    }

    public class Model
    {
        public BitmapImage ImSource { get; set; }
    }


}
