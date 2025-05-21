using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace KalkulatorRatalnyMAUI
{
    // Główna strona aplikacji (ContentPage) w .NET MAUI, gdzie użytkownik wprowadza dane do kalkulacji rat kredytu.
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Obsługa przełącznika (Switch) dla opcji nadpłaty kredytu.
        // Pokazuje lub ukrywa pole do wpisania kwoty nadpłaty w zależności od stanu przełącznika.
        private void OnNadplataToggled(object sender, ToggledEventArgs e)
        {
            bool isToggled = e.Value;

            nadplataKredytuInput.IsVisible = isToggled;
        }

        // Metoda wywoływana po kliknięciu przycisku „Oblicz raty”.
        // Zawiera walidację wszystkich pól wejściowych, obsługę błędów i wywołanie funkcji obliczającej harmonogram rat.
        private async void OnObliczRatyClicked(object sender, EventArgs e)
        {
            // Walidacja pola "Kwota kredytu"
            if (string.IsNullOrWhiteSpace(kwotaKredytuInput.Text))
            {
                await DisplayAlert("Błąd", "Pole \"Kwota kredytu\" nie może być puste.", "Rozumiem");
                return;
            }

            if (kwotaKredytuInput.Text.Contains('.'))
            {
                // Informacja o poprawnym formacie dziesiętnym (używamy przecinka zamiast kropki)
                await DisplayAlert("Uwaga", "W polu \"Kwota kredytu\" użyj przecinka ',' jako separatora dziesiętnego zamiast kropki '.'", "Rozumiem");
                return;
            }

            if (!decimal.TryParse(kwotaKredytuInput.Text, out decimal kwotaKredytu))
            {
                await DisplayAlert("Błąd", "\"Kwota kredytu\" musi być liczbą.", "Rozumiem");
                return;
            }

            if (kwotaKredytu <= 0)
            {
                await DisplayAlert("Błąd", "\"Kwota kredytu\" musi być większa niż 0.", "Rozumiem");
                return;
            }

            if (kwotaKredytu > 10000000)
            {
                await DisplayAlert("Błąd", "\"Kwota kredytu\" nie może przekraczać 10 000 000.", "Rozumiem");
                return;
            }

            // Walidacja pola "Liczba miesięcy"
            if (string.IsNullOrWhiteSpace(liczbaMiesiecyInput.Text))
            {
                await DisplayAlert("Błąd", "Pole \"Liczba miesięcy\" nie może być puste.", "Rozumiem");
                return;
            }

            if (liczbaMiesiecyInput.Text.Contains('.'))
            {
                await DisplayAlert("Uwaga", "W polu \"Liczba miesięcy\" nie używaj kropki '.'", "Rozumiem");
                return;
            }

            if (!int.TryParse(liczbaMiesiecyInput.Text, out int liczbaMiesiecy))
            {
                await DisplayAlert("Błąd", "\"Liczba miesięcy\" musi być liczbą całkowitą.", "Rozumiem");
                return;
            }

            if (liczbaMiesiecy <= 0)
            {
                await DisplayAlert("Błąd", "\"Liczba miesięcy\" musi być większa niż 0.", "Rozumiem");
                return;
            }

            if (liczbaMiesiecy > 420)
            {
                await DisplayAlert("Błąd", "\"Liczba miesięcy\" nie może przekraczać 420.", "Rozumiem");
                return;
            }

            // Walidacja pola "Oprocentowanie"
            if (string.IsNullOrWhiteSpace(oprocentowanieKredytuInput.Text))
            {
                await DisplayAlert("Błąd", "Pole \"Oprocentowanie\" nie może być puste.", "Rozumiem");
                return;
            }

            if (oprocentowanieKredytuInput.Text.Contains('.'))
            {
                await DisplayAlert("Uwaga", "W polu \"Oprocentowanie\" użyj przecinka ',' jako separatora dziesiętnego zamiast kropki '.'", "Rozumiem");
                return;
            }

            if (!decimal.TryParse(oprocentowanieKredytuInput.Text, out decimal oprocentowanie))
            {
                await DisplayAlert("Błąd", "\"Oprocentowanie\" musi być liczbą.", "Rozumiem");
                return;
            }

            if (oprocentowanie < 0)
            {
                await DisplayAlert("Błąd", "\"Oprocentowanie\" nie może być ujemne.", "Rozumiem");
                return;
            }

            if (oprocentowanie > 50)
            {
                await DisplayAlert("Błąd", "\"Oprocentowanie\" nie może być większe niż 50%.", "Rozumiem");
                return;
            }

            decimal nadplata = 0;

            // Walidacja pola "Nadpłata", tylko jeśli opcja jest włączona (przełącznik jest aktywny)
            if (czyNadplataKredytuSwitch.IsToggled)
            {
                if (string.IsNullOrWhiteSpace(nadplataKredytuInput.Text))
                {
                    await DisplayAlert("Błąd", "Pole \"Nadpłata\" nie może być puste, jeśli opcja jest włączona.", "Rozumiem");
                    return;
                }

                if (nadplataKredytuInput.Text.Contains('.'))
                {
                    await DisplayAlert("Uwaga", "W polu \"Nadpłata\" użyj przecinka ',' jako separatora dziesiętnego zamiast kropki '.'", "Rozumiem");
                    return;
                }

                if (!decimal.TryParse(nadplataKredytuInput.Text, out nadplata))
                {
                    await DisplayAlert("Błąd", "\"Nadpłata\" musi być liczbą.", "Rozumiem");
                    return;
                }

                if (nadplata < 0)
                {
                    await DisplayAlert("Błąd", "\"Nadpłata\" nie może być ujemna.", "Rozumiem");
                    return;
                }

                if (nadplata > kwotaKredytu)
                {
                    await DisplayAlert("Błąd", "\"Nadpłata\" nie może być większa niż kwota kredytu.", "Rozumiem");
                    return;
                }
            }

            try
            {
                // Wywołanie metody obliczającej harmonogram rat kredytu na podstawie wprowadzonych danych
                List<HarmonogramItem> harmonogram = ObliczHarmonogram(kwotaKredytu, liczbaMiesiecy, oprocentowanie, nadplata);

                // Przejście do nowej strony wyświetlającej harmonogram rat
                await Navigation.PushAsync(new HarmonogramPage(harmonogram));
            }
            catch (Exception ex)
            {
                // Obsługa potencjalnych błędów podczas obliczeń
                await DisplayAlert("Błąd", $"Wystąpił błąd podczas obliczeń: {ex.Message}", "Rozumiem");
            }
        }

        private static List<HarmonogramItem> ObliczHarmonogram(decimal kwota, int miesiace, decimal oprocentowanie, decimal nadplata)
        {
            var harmonogram = new List<HarmonogramItem>();

            // Saldo do spłaty pomniejszone o nadpłatę
            decimal saldo = kwota - nadplata;

            if (saldo < 0)
            {
                saldo = 0; // Jeśli nadpłata przewyższa kwotę kredytu, saldo jest 0 (brak długu)
            }

            // Obliczenie miesięcznej stopy procentowej z rocznej (oprocentowanie podane w %)
            decimal miesieczneOprocentowanie = (oprocentowanie / 100) / 12;
            decimal rata;

            // Obliczenie wysokości raty
            if (oprocentowanie == 0)
            {
                // Jeśli brak oprocentowania, rata jest po prostu podziałem kwoty na liczbę miesięcy
                rata = saldo / miesiace;
            }
            else
            {
                // Standardowy wzór na ratę równą (annuitetową)
                double temp = Math.Pow(1 + (double)miesieczneOprocentowanie, miesiace);
                rata = saldo * (miesieczneOprocentowanie * (decimal)temp) / ((decimal)temp - 1);
            }

            // Pętla tworząca harmonogram rat
            for (int i = 1; i <= miesiace; i++)
            {
                // Obliczanie części odsetkowej raty od aktualnego salda
                decimal odsetki = saldo * miesieczneOprocentowanie;

                // Część kapitałowa to rata pomniejszona o odsetki
                decimal kapital = rata - odsetki;

                // Aktualizacja salda po spłacie kapitału
                saldo -= kapital;

                // Jeśli saldo spadło poniżej zera (zaokrąglenia), ustawiamy na zero
                if (saldo < 0)
                {
                    saldo = 0;
                }

                // Dodanie obliczonej raty do harmonogramu
                harmonogram.Add(new HarmonogramItem
                {
                    NumerRaty = i,
                    KwotaRaty = rata,
                    Kapital = kapital,
                    Odsetki = odsetki,
                    PozostaleSaldo = saldo
                });

                // Przerwanie pętli, jeśli kredyt został całkowicie spłacony wcześniej
                if (saldo == 0)
                {
                    break;
                }
            }

            return harmonogram;
        }
    }

    // Klasa reprezentująca pojedynczą ratę w harmonogramie spłat kredytu
    public class HarmonogramItem
    {
        public int NumerRaty { get; set; } // Numer kolejnej raty
        public decimal KwotaRaty { get; set; } // Całkowita kwota raty
        public decimal Kapital { get; set; } // Część raty spłacająca kapitał
        public decimal Odsetki { get; set; } // Część raty stanowiąca odsetki
        public decimal PozostaleSaldo { get; set; } // Pozostałe saldo kredytu po spłacie danej raty
    }
}
