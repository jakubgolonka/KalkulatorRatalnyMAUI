# MAUI Loan Calculator

A mobile application built with **.NET MAUI**, allowing quick and accurate calculation of loan installments and generating a detailed repayment schedule with PDF export capability (Windows only).

---

## Table of Contents

* [Project Description](#project-description)
* [Features](#features)
* [Technologies and Libraries](#technologies-and-libraries)
* [Architecture and Code Structure](#architecture-and-code-structure)
* [Installation and Launch Guide](#installation-and-launch-guide)
* [Usage Example](#usage-example)
* [PDF Export of Repayment Schedule](#pdf-export-of-repayment-schedule)
* [Future Development](#future-development)
* [Project Authors](#project-authors)

---

## Project Description

**MAUI Loan Calculator** is a tool dedicated to individuals who want to accurately calculate a loan repayment schedule. The application allows:

* Calculation of loan installments divided into principal and interest components.
* Visualization of the repayment schedule in a clear table.
* Export of the schedule to a PDF file (feature available on Windows only).
* Intuitive navigation between the loan settings screen and the schedule view.

---

## Features

* **Installment Calculation:** dynamic calculation of installment amounts based on input parameters (amount, interest rate, repayment period).
* **Repayment Schedule:** detailed list of installments where each entry includes:

  * installment number,
  * total installment amount,
  * principal part,
  * interest part,
  * remaining balance.
* **PDF Export:** generate a well-designed PDF report with the schedule, ready for print or email.
* **User-Friendly UI:** simple and clear interface based on XAML with MAUI controls.

---

## Technologies and Libraries

* **.NET MAUI** — Microsoft's framework for building mobile and desktop apps.
* **XAML** — declarative language for creating UI.
* **PdfSharpCore** — library for creating and editing PDF files in .NET.
* **C#** — programming language used for application logic.

---

## Architecture and Code Structure

* **Data Model:**
  `HarmonogramItem` — class representing a single loan installment with the following properties:

  * `InstallmentNumber` (int)
  * `InstallmentAmount` (decimal)
  * `Principal` (decimal)
  * `Interest` (decimal)
  * `RemainingBalance` (decimal)

* **View:**
  `HarmonogramPage.xaml` — contains the table layout and definition of the list view with headers and rows.

* **View Logic:**
  `HarmonogramPage.xaml.cs` — handles user interactions, e.g. navigating back, exporting to PDF, and data initialization.

---

## Installation and Launch Guide

### Requirements

* Visual Studio 2022+ with .NET MAUI support installed
* .NET SDK 7.0 or newer
* Windows OS (recommended for PDF generation)

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/jakubgolonka/KalkulatorRatalnyMAUI.git
   cd KalkulatorRatalnyMAUI
   ```
2. Open the project in Visual Studio.
3. Select the target platform (Android, iOS, Windows) from the top menu.
4. Run the project by pressing F5 or clicking Run.

---

## Usage Example

After launching the application:

1. Enter loan data: amount, interest rate, and duration.
2. Press **"Calculate Installments"**.
3. Navigate to the repayment schedule tab.
4. View installment details in the table.
5. If on Windows, click **"Export to PDF"** to save the report in the **Documents** folder.

---

## PDF Export of Repayment Schedule

The PDF generation feature uses the **PdfSharpCore** library and currently works only on **Windows**.

The PDF includes:

* Title and generation date
* Table with columns: installment number, amount, principal, interest, balance
* Automatic pagination
* Elegant headers and currency formatting

**Save location:** user's `Documents` folder, file named:
`Schedule_yyyyMMdd_HHmmss.pdf`

---

## Future Development

* PDF export for Android and iOS
* Calculation history and local database support
* Addition of capital and interest repayment charts

---

## Project Authors

* Jakub Golonka
* Jakub Matras
* Krystian Frączek
* Łukasz Szewczyk
