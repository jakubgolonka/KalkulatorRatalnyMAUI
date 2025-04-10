using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace KalkulatorRatalnyMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Obsługa przełącznika z wyświetlaniem pola tekstowego z wartością nadpłaty kredytu
        private void OnNadplataToggled(object sender, ToggledEventArgs e)
        {
            bool isToggled = e.Value;

            nadplataKredytuLabel.IsVisible = isToggled;
            nadplataKredytuInput.IsVisible = isToggled;
        }

        // Obsługa przycisku odpowiadającego za otwieranie nowego okna z harmonogramem
        private async void OnObliczRatyClicked(object sender, EventArgs e)
        {
            // Pobranie i walidacja danych z wartością kwoty kredytu
            if (!decimal.TryParse(kwotaKredytuInput.Text, out decimal kwotaKredytu) || kwotaKredytu <= 0 || kwotaKredytu > 10000000)
            {
                await DisplayAlert("Błąd", "Wprowadź poprawną kwotę kredytu (od 1 do 10 000 000)!", "Rozumiem");

                return;
            }

            // Pobranie i walidacja danych z wartością liczbą miesięcy kredytu
            if (!int.TryParse(liczbaMiesiecyInput.Text, out int liczbaMiesiecy) || liczbaMiesiecy <= 0 || liczbaMiesiecy > 420)
            {
                await DisplayAlert("Błąd", "Wprowadź poprawną liczbę miesięcy (maksymalnie 420 miesięcy)!", "Rozumiem");

                return;
            }

            // Pobranie i walidacja danych z wartością oprocentowania kredytu
            if (!decimal.TryParse(oprocentowanieKredytuInput.Text, out decimal oprocentowanie) || oprocentowanie < 0 || oprocentowanie > 50)
            {
                await DisplayAlert("Błąd", "Wprowadź poprawne oprocentowanie (od 0 do 50%)!", "Rozumiem");

                return;
            }

            // Pobranie i walidacja danych z wartością nadpłaty kredytu
            decimal nadplata = 0;

            if (czyNadplataKredytuSwitch.IsToggled)
            {
                if (!decimal.TryParse(nadplataKredytuInput.Text, out nadplata) || nadplata < 0)
                {
                    await DisplayAlert("Błąd", "Wprowadź poprawną wartość nadpłaty!", "Rozumiem");

                    return;
                }
            }

            // Obliczenie harmonogramu spłat kredytu
            List<HarmonogramItem> harmonogram = ObliczHarmonogram(kwotaKredytu, liczbaMiesiecy, oprocentowanie, nadplata);

            // Otworzenie i przejście do nowej strony zawierającej harmonogram spłat kredytu
            await Navigation.PushAsync(new HarmonogramPage(harmonogram));
        }

        // Metoda / Funkcja do obliczania harmonogramu spłat kredytu
        private List<HarmonogramItem> ObliczHarmonogram(decimal kwota, int miesiace, decimal oprocentowanie, decimal nadplata)
        {
            // Obsługa błędów: Sprawdzamy, czy kwota i oprocentowanie są prawidłowe
            if (kwota <= 0 || kwota > 10000000)
            {
                throw new ArgumentException("Kwota pożyczki musi być większa niż 0 i mniejsza lub równa 10 000 000.");
            }

            if (miesiace <= 0 || miesiace > 420)
            {
                throw new ArgumentException("Liczba miesięcy musi być większa niż 0 i mniejsza lub równa 420.");
            }

            if (oprocentowanie < 0 || oprocentowanie > 50)
            {
                throw new ArgumentException("Oprocentowanie nie może być ujemne i musi wynosić maksymalnie 50%.");
            }

            List<HarmonogramItem> harmonogram = new List<HarmonogramItem>();

            decimal saldo = kwota;
            decimal miesieczneOprocentowanie = (oprocentowanie / 100) / 12;

            // Sprawdzamy, czy rata nie jest większa niż kwota pożyczki
            decimal rata = kwota * (miesieczneOprocentowanie * (decimal)Math.Pow(1 + (double)miesieczneOprocentowanie, miesiace)) / ((decimal)Math.Pow(1 + (double)miesieczneOprocentowanie, miesiace) - 1);

            if (rata > kwota)
            {
                throw new InvalidOperationException("Rata miesięczna jest wyższa niż kwota pożyczki, co jest nierealne.");
            }

            for (int i = 1; i <= miesiace; i++)
            {
                decimal odsetki = saldo * miesieczneOprocentowanie;
                decimal kapital = rata - odsetki;

                // Uwzględnienie i sprawdzenie warunku odpowiadającego na nadpłatę kredytu
                if (nadplata > 0)
                {
                    saldo -= nadplata;
                }

                saldo -= kapital;

                if (saldo < 0)
                {
                    saldo = 0;
                }

                harmonogram.Add(new HarmonogramItem
                {
                    NumerRaty = i,  
                    KwotaRaty = rata,
                    Kapital = kapital,
                    Odsetki = odsetki,
                    PozostaleSaldo = saldo
                });

                if (saldo == 0)
                {
                    break;
                }
            }

            return harmonogram;
        }
    }

    public class HarmonogramItem
    {
        public int NumerRaty { get; set; }
        public decimal KwotaRaty { get; set; }
        public decimal Kapital { get; set; }
        public decimal Odsetki { get; set; }
        public decimal PozostaleSaldo { get; set; }
    }
}
