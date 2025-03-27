using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace KalkulatorRatalnyMAUI
{
    public partial class HarmonogramPage : ContentPage
    {
        public HarmonogramPage(List<HarmonogramItem> harmonogram)
        {
            InitializeComponent();
            
            HarmonogramListView.ItemsSource = harmonogram;
        }

        // Metoda do obsługi przycisku odpowiadającego za powrót do głównej strony
        private async void OnWrocClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
