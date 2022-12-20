using System;
using System.Windows;

namespace Mitya_plus_Kashkin_ravno_Saper;

public partial class Settings : Window
{
    public Settings()
    {
        InitializeComponent();
    }

    private void Ready_OnClick(object sender, RoutedEventArgs e)
    {
        int width;
        int height;
        int count;
        try
        {
            width = Convert.ToInt32(widht_Field.Text);
            height = Convert.ToInt32(height_Field.Text);
            count = Convert.ToInt32(mines_count.Text);
            if (width * height <= count || width * height * 0.5 < count || width < 5 || width > 20 || height < 5 || height > 20 || count < 1)
            {
                throw new Exception();
            }
        }
        catch (Exception exception)

        {
            MessageBox.Show("Введены неверные данные, бездарь! \n Границы поля от 5 до 20 \n Количество мин - половина от числа клеток.");
            return;
        }

        Input_info.height_field = height;
        Input_info.width_field = width;
        Input_info.count_mines = count;
        DialogResult = true;
    }
}