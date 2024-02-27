using System.Windows;
using System.Windows.Controls;
using Game_Of_Life.Models;
using Game_Of_Life.Utils;

namespace Game_Of_Life;

public partial class SaveGridSizeDialog : Window
{
    public SaveGridSizeDialog()
    {
        InitializeComponent();
    }
    
    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        var gridSize = int.Parse(InputTextBox.Text);
        SaveUtil.Save(new Save(gridSize));
        Close();
    }

    private void CheckInput(object sender, TextChangedEventArgs e)
    {
        if (int.TryParse(InputTextBox.Text, out int result))
        {
            OkButton.IsEnabled = result > 0;
        }
        else
        {
            OkButton.IsEnabled = false;
        }
    }
}