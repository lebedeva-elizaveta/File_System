using System.Windows;

namespace task2_c
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        public void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                if (e.NewValue != null)
                {
                    viewModel.FirstSelectedItem = e.NewValue as FileSystemViewModel;
                    viewModel.SecondSelectedItem = FindSelectedFolder(viewModel.FirstSelectedItem);

                    if (viewModel.FirstSelectedItem is FileViewModel file)
                    {
                        viewModel.SelectedItemSize = file.Size;
                    }
                    else if (viewModel.FirstSelectedItem is FolderViewModel folder)
                    {
                        viewModel.SelectedItemSize = GetFolderSize(folder);
                    }
                }
            }
        }

        private long GetFolderSize(FolderViewModel folder)
        {
            long size = 0;
            foreach (var item in folder.Items)
            {
                if (item is FileViewModel file)
                {
                    size += file.Size;
                }
                else if (item is FolderViewModel subfolder)
                {
                    size += GetFolderSize(subfolder);
                }
            }
            return size;
        }

        private FolderViewModel FindSelectedFolder(object selectedItem)
        {
            if (selectedItem is FolderViewModel folder)
            {
                return folder;
            }
            else return null;
        }
    }
}
