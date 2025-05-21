using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Threading.Tasks;

namespace KalkulatorRatalnyMAUI
{
    // Strona wy�wietlaj�ca harmonogram sp�at kredytu i umo�liwiaj�ca zapis do PDF
    public partial class HarmonogramPage : ContentPage
    {
        // Lista pozycji harmonogramu przekazywana z poprzedniej strony
        private readonly List<HarmonogramItem> harmonogram;

        public HarmonogramPage(List<HarmonogramItem> harmonogram)
        {
            InitializeComponent();

            // Przypisanie danych do widoku listy (ListView)
            this.harmonogram = harmonogram;
            HarmonogramListView.ItemsSource = harmonogram;
        }

        // Obs�uga powrotu do poprzedniej strony
        private async void OnWrocClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Obs�uga zapisu harmonogramu do pliku PDF (wywo�uje metod� generuj�c� PDF asynchronicznie)
        private async void OnSavePdfClicked(object sender, EventArgs e)
        {
            try
            {
                var filePath = await GeneratePdfAsync(harmonogram);
                await DisplayAlert("Sukces", $"Plik PDF zosta� zapisany:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("B��d", $"Nie uda�o si� wygenerowa� PDF:\n{ex.Message}", "OK");
            }
        }

        // Kluczowa metoda generuj�ca PDF z harmonogramem
        private static Task<string> GeneratePdfAsync(List<HarmonogramItem> data)
        {
            return Task.Run(() =>
            {
                // Przygotowanie pliku PDF z nazw� zawieraj�c� dat� i zapis w folderze Dokumenty
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filename = $"Harmonogram_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string fullPath = Path.Combine(documentsPath, filename);

                using var document = new PdfDocument();
                var page = document.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;

                // Obiekt do rysowania grafiki na stronie PDF
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Czcionki do r�nych element�w PDF
                var fontTitleBig = new XFont("Arial", 20, XFontStyle.Bold);
                var fontTitleSmall = new XFont("Arial", 14, XFontStyle.Regular);
                var fontRegular = new XFont("Arial", 12, XFontStyle.Regular);
                var fontBold = new XFont("Arial", 12, XFontStyle.Bold);

                // Marginesy i szeroko�� do rysowania tabeli
                double marginLeft = 40;
                double marginRight = 40;
                double usableWidth = page.Width - marginLeft - marginRight;

                double yPoint = 40; // pozycja pionowa startowa

                // Rysujemy tytu� i podtytu�y (m.in. aktualn� dat�)
                gfx.DrawString("Kalkulator Ratalny", fontTitleBig, XBrushes.Black,
                    new XRect(marginLeft, yPoint, usableWidth, 25), XStringFormats.Center);
                yPoint += 40;
                gfx.DrawString("Harmonogram sp�at kredytu", fontTitleSmall, XBrushes.Black,
                    new XRect(marginLeft, yPoint, usableWidth / 2, 25), XStringFormats.TopLeft);
                gfx.DrawString(DateTime.Now.ToString("dd.MM.yyyy HH:mm"), fontTitleSmall, XBrushes.Black,
                    new XRect(marginLeft + usableWidth / 2, yPoint, usableWidth / 2, 25), XStringFormats.TopRight);
                yPoint += 40;

                // Nag��wki tabeli i szeroko�ci kolumn (5 kolumn)
                int columnsCount = 5;
                double columnWidth = usableWidth / columnsCount;
                var headers = new[] { "Numer", "Kwota raty", "Kapita�", "Odsetki", "Pozosta�e saldo" };

                // Rysujemy nag��wki z szarym t�em
                double xPos = marginLeft;
                var headerBackground = new XSolidBrush(XColor.FromArgb(220, 220, 220));

                for (int i = 0; i < columnsCount; i++)
                {
                    var rect = new XRect(xPos, yPoint, columnWidth, 25);

                    gfx.DrawRectangle(headerBackground, rect);
                    gfx.DrawRectangle(XPens.Black, rect);
                    gfx.DrawString(headers[i], fontBold, XBrushes.Black, rect, XStringFormats.Center);

                    xPos += columnWidth;
                }

                yPoint += 25;

                // Rysowanie danych - wiersze tabeli z informacjami o ratach
                foreach (var item in data)
                {
                    xPos = marginLeft;

                    // W ka�dej kolumnie rysujemy odpowiedni� warto��
                    void DrawCell(string text)
                    {
                        var rect = new XRect(xPos, yPoint, columnWidth, 25);

                        gfx.DrawRectangle(XPens.Black, rect);
                        gfx.DrawString(text, fontRegular, XBrushes.Black, rect, XStringFormats.Center);

                        xPos += columnWidth;
                    }

                    DrawCell(item.NumerRaty.ToString());
                    DrawCell(item.KwotaRaty.ToString("C"));
                    DrawCell(item.Kapital.ToString("C"));
                    DrawCell(item.Odsetki.ToString("C"));
                    DrawCell(item.PozostaleSaldo.ToString("C"));

                    yPoint += 25;

                    // Je�li zabraknie miejsca na stronie, dodajemy now� stron� i powtarzamy nag��wki
                    if (yPoint + 25 > page.Height - 40)
                    {
                        gfx.Dispose();

                        page = document.AddPage();
                        page.Size = PdfSharpCore.PageSize.A4;
                        gfx = XGraphics.FromPdfPage(page);

                        yPoint = 40;

                        // Rysujemy tytu� i nag��wki ponownie na nowej stronie
                        gfx.DrawString("Kalkulator Ratalny", fontTitleBig, XBrushes.Black,
                            new XRect(marginLeft, yPoint, usableWidth, 25), XStringFormats.Center);
                        yPoint += 40;
                        gfx.DrawString("Harmonogram sp�at kredytu", fontTitleSmall, XBrushes.Black,
                            new XRect(marginLeft, yPoint, usableWidth / 2, 25), XStringFormats.TopLeft);
                        gfx.DrawString(DateTime.Now.ToString("dd.MM.yyyy HH:mm"), fontTitleSmall, XBrushes.Black,
                            new XRect(marginLeft + usableWidth / 2, yPoint, usableWidth / 2, 25), XStringFormats.TopRight);
                        yPoint += 40;

                        xPos = marginLeft;

                        for (int i = 0; i < columnsCount; i++)
                        {
                            var rect = new XRect(xPos, yPoint, columnWidth, 25);

                            gfx.DrawRectangle(headerBackground, rect);
                            gfx.DrawRectangle(XPens.Black, rect);
                            gfx.DrawString(headers[i], fontBold, XBrushes.Black, rect, XStringFormats.Center);
                            
                            xPos += columnWidth;
                        }

                        yPoint += 25;
                    }
                }

                gfx.Dispose();

                // Zapisujemy plik PDF do podanej lokalizacji i zwracamy t� �cie�k�
                document.Save(fullPath);
                return fullPath;
            });
        }
    }
}
