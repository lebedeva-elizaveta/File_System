using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace task2_c
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<FileSystemViewModel> FileSystemItems { get; } = new ObservableCollection<FileSystemViewModel>();

        public ICommand CopyCommand { get; }
        public ICommand MoveCommand { get; }
        public FileSystemViewModel FirstSelectedItem { get; set; }
        public FolderViewModel SecondSelectedItem { get; set; }

        private long _selectedItemSize;
        public long SelectedItemSize
        {
            get { return _selectedItemSize; }
            set
            {
                _selectedItemSize = value;
                OnPropertyChanged(nameof(SelectedItemSize));
            }
        }

        public MainViewModel()
        {
            CopyCommand = new RelayCommand(Copy, CanCopy);
            MoveCommand = new RelayCommand(Move, CanMove);

            var rootFolder = new FolderViewModel { Name = "Root" };
            var subFolder = new FolderViewModel { Name = "Subfolder" };
            var file1 = new FileViewModel("File1.txt", 1024);
            var file2 = new FileViewModel("File2.txt", 2048);
            subFolder.AddItem(file1);
            subFolder.AddItem(file2);
            rootFolder.AddItem(subFolder);

            FileSystemItems.Add(rootFolder);
        }

        private void Copy(object parameter)
        {
            Debug.WriteLine("Copy command executed!");
            Debug.WriteLine($"First selected item: {FirstSelectedItem?.Name}");
            Debug.WriteLine($"Second selected folder: {SecondSelectedItem?.Name}");

            if (FirstSelectedItem != null && SecondSelectedItem != null)
            {
                if (FirstSelectedItem is FolderViewModel folder)
                {
                    CopyFolder(folder, SecondSelectedItem);
                    MessageBox.Show($"Folder '{folder.Name}' has been copied to '{SecondSelectedItem.Name}'.");
                    RefreshFileSystemItems();
                }
                else if (FirstSelectedItem is FileViewModel file)
                {
                    CopyFile(file, SecondSelectedItem);
                    MessageBox.Show($"File '{file.Name}' has been copied to '{SecondSelectedItem.Name}'.");
                    RefreshFileSystemItems();
                }
            FirstSelectedItem = null;
            SecondSelectedItem = null;
            }
            else
            {
                MessageBox.Show("Please select a source item and a destination folder.");
            }
        }

        private void CopyFolder(FolderViewModel folder, FolderViewModel destinationFolder)
        {
            var copyFolder = new FolderViewModel { Name = folder.Name };

            foreach (var item in folder.Items)
            {
                if (item is FolderViewModel subfolder)
                {
                    CopyFolder(subfolder, copyFolder);
                }
                else if (item is FileViewModel file)
                {
                    var copyFile = new FileViewModel(file.Name, file.Size);
                    copyFolder.AddItem(copyFile);
                }
            }

            destinationFolder.AddItem(copyFolder);
            OnPropertyChanged(nameof(FileSystemItems));
        }

        private void CopyFile(FileViewModel file, FolderViewModel destinationFolder)
        {
            if (!destinationFolder.Items.Any(item => item.Name == file.Name))
            {
                var copyFile = new FileViewModel(file.Name, file.Size);
                destinationFolder.AddItem(copyFile);
                OnPropertyChanged(nameof(FileSystemItems));
            }
            else
            {
                Console.WriteLine($"Failed to copy {file.Name}: Destination folder already contains a file with the same name.");
            }
        }

        private bool CanCopy(object parameter)
        {
            return FirstSelectedItem != null && SecondSelectedItem != null;
        }

        private bool CanMove(object parameter)
        {
            return FirstSelectedItem != null && SecondSelectedItem != null;
        }

        private void Move(object parameter)
        {
            Debug.WriteLine("Move command executed!");
            Debug.WriteLine($"First selected item: {FirstSelectedItem?.Name}");
            Debug.WriteLine($"Second selected folder: {SecondSelectedItem?.Name}");

            if (FirstSelectedItem != null && SecondSelectedItem != null)
            {
                if (FirstSelectedItem is FolderViewModel folder)
                {
                    MoveFolder(folder, SecondSelectedItem);
                    MessageBox.Show($"Folder '{folder.Name}' has been moved to '{SecondSelectedItem.Name}'.");
                    RefreshFileSystemItems();
                }
                else if (FirstSelectedItem is FileViewModel file)
                {
                    MoveFile(file, SecondSelectedItem);
                    MessageBox.Show($"File '{file.Name}' has been moved to '{SecondSelectedItem.Name}'.");
                    RefreshFileSystemItems();
                }
            FirstSelectedItem = null;
            SecondSelectedItem = null;
            }           
            else
            {
                MessageBox.Show("Please select a source item and a destination folder.");
            }
        }

        private void MoveFolder(FolderViewModel folder, FolderViewModel destinationFolder)
        {
            if (folder != destinationFolder && !folder.IsAncestorOf(destinationFolder))
            {
                folder.ParentFolder?.RemoveItem(folder);
                destinationFolder.AddItem(folder);
                OnPropertyChanged(nameof(FileSystemItems));
            }
            else
            {
                Console.WriteLine($"Failed to move {folder.Name}: Destination folder is a subfolder of the source folder.");
            }
        }

        private void MoveFile(FileViewModel file, FolderViewModel destinationFolder)
        {
            if (!destinationFolder.Items.Any(item => item.Name == file.Name))
            {
                file.ParentFolder?.RemoveItem(file);
                destinationFolder.AddItem(file);
                OnPropertyChanged(nameof(FileSystemItems));
            }
            else
            {
                Console.WriteLine($"Failed to move {file.Name}: Destination folder already contains a file with the same name.");
            }
        }

        private void RefreshFileSystemItems()
        {
            OnPropertyChanged(nameof(FileSystemItems));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
