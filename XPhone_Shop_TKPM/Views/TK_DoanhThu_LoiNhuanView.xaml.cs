﻿using InteractiveDataDisplay.WPF;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for TK_DoanhThu_LoiNhuanView.xaml
    /// </summary>
    public partial class TK_DoanhThu_LoiNhuanView : UserControl
    {
        int[] listYear = { 2024, 2023, 2022, 2021, 2020, 2019 };
        int[] listMonth = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int[] listWeek = { 1, 2, 3, 4, 5, 6 };
        TK_DoanhThu_LoiNhuanViewModel _viewModel = new TK_DoanhThu_LoiNhuanViewModel();
        DateTime _start;
        DateTime _end;
        int _year = 0;
        int _month = 0;
        int _week = 0;

        LineGraph line1 = new LineGraph();
        LineGraph line2 = new LineGraph();
        LineGraph line3 = new LineGraph();
        private void refresh(DateTime start, DateTime end)
        {
            Lines.Children.Clear();
            var tempTuple = _viewModel.getListRevenueAndProfit(start, end);
            if (tempTuple == null)
                return;
            // Đường doanh thu
            Lines.Children.Add(line1);
            line1.Stroke = new SolidColorBrush(Colors.SaddleBrown);
            line1.Description = "Doanh thu";
            line1.StrokeThickness = 2;

            // Đường vốn
            Lines.Children.Add(line2);
            line2.Stroke = new SolidColorBrush(Colors.Red);
            line2.Description = "Vốn";
            line2.StrokeThickness = 2;

            // Đường vốn
            Lines.Children.Add(line3);
            line3.Stroke = new SolidColorBrush(Colors.Green);
            line3.Description = "Lợi nhuận";
            line3.StrokeThickness = 2;

            line1.Plot(tempTuple.Item1, tempTuple.Item2);
            line2.Plot(tempTuple.Item1, tempTuple.Item3);
            line3.Plot(tempTuple.Item1, tempTuple.Item4);
        }

        private void updateDuration(DateTime start, DateTime end)
        {
            string s = start.ToString("dd/MM/yyyy");
            string e = end.ToString("dd/MM/yyyy");

            txtDuration.Content = "Từ ngày " + s + " đến " + e;
        }

        private void refreshCombobox()
        {
            chooseYear.SelectedIndex = -1;
            chooseMonth.SelectedIndex = -1;
            chooseWeek.SelectedIndex = -1;

            _year = 0;
            _month = 0;
            _week = 0;
        }

        public TK_DoanhThu_LoiNhuanView()
        {
            InitializeComponent();

            for (int i = 0; i < Dashboard_Admin_Sale.menuBTN.Children.Count; i++)
            {
                if (Dashboard_Admin_Sale.menuBTN.Children[i] is MenuButton)
                {
                    var select = Dashboard_Admin_Sale.menuBTN.Children[i] as MenuButton;
                    if (select.btn.IsFocused == true)
                        select.isActive = true;
                    else
                        select.isActive = false;
                }
            }

            for (int i = 0; i < Dashboard_Admin_Sale.subMenuBTN.Children.Count; i++)
            {
                if (Dashboard_Admin_Sale.subMenuBTN.Children[i] is MenuButton)
                {
                    var select_ = Dashboard_Admin_Sale.subMenuBTN.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }

            chooseYear.ItemsSource = listYear;
            chooseMonth.ItemsSource = listMonth;
            chooseWeek.ItemsSource = listWeek;

            DateTime _start = new DateTime(2000, 1, 1);
            DateTime _end = DateTime.Now;
            updateDuration(_start, _end);
            refresh(_start, _end);

            Global.SaveScreen("TKDTVLN");
        }

        private void filterDateButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Parse(startDatePicker.Text + " 12:00");
            DateTime end = DateTime.Parse(endDatePicker.Text + " 12:00");
            _start = start;
            _end = end;

            updateDuration(start, end);
            refresh(start, end);
        }

        public static Tuple<DateTime, DateTime> GetStartAndEndDays(int year, int month, int week)
        {
            DateTime startDay;
            DateTime endDay;
            if (year > 0 && month == 0 && week == 0)
            {
                startDay = new DateTime(year, 1, 1);
                endDay = new DateTime(year, 12, 31);
            }
            else if (year == 0 && month > 0 && week == 0)
            {
                startDay = new DateTime(DateTime.Now.Year, month, 1);
                endDay = new DateTime(DateTime.Now.Year, month, 1).AddMonths(1).AddDays(-1);
            }
            else if (year == 0 && month == 0 && week > 0)
            {
                int StartDayFirstWeek = (int)(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).DayOfWeek);
                Debug.WriteLine(StartDayFirstWeek);
                if (StartDayFirstWeek == 0)
                {
                    StartDayFirstWeek = 7;
                }
                int rangeFirstWeek = 7 - StartDayFirstWeek;
                DateTime EndDateFirstWeek = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(rangeFirstWeek);


                if (week == 1)
                {
                    startDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    endDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(rangeFirstWeek);
                }
                else
                {
                    DateTime endDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                    int dayStart = (EndDateFirstWeek.Day + 1) + (week - 2) * 7;
                    int endStart = (EndDateFirstWeek.Day + 1) + (week - 2) * 7 + 6;

                    if (dayStart > endDayOfMonth.Day)
                    {
                        startDay = endDayOfMonth.AddDays(-6);
                        endDay = endDayOfMonth;
                    }
                    else if (endStart > endDayOfMonth.Day)
                    {
                        startDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7);
                        endDay = endDayOfMonth;
                    }
                    else
                    {
                        startDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7);
                        endDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7).AddDays(6);
                    }
                }
            }
            else if (year > 0 && month > 0 && week == 0)
            {
                startDay = new DateTime(year, month, 1);
                endDay = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            }
            else if (year == 0 && month > 0 && week > 0)
            {
                int StartDayFirstWeek = (int)(new DateTime(DateTime.Now.Year, month, 1).DayOfWeek);
                if (StartDayFirstWeek == 0)
                {
                    StartDayFirstWeek = 7;
                }
                int rangeFirstWeek = 7 - StartDayFirstWeek;
                DateTime EndDateFirstWeek = new DateTime(DateTime.Now.Year, month, 1).AddDays(rangeFirstWeek);


                if (week == 1)
                {
                    startDay = new DateTime(DateTime.Now.Year, month, 1);
                    endDay = new DateTime(DateTime.Now.Year, month, 1).AddDays(rangeFirstWeek);
                }
                else
                {
                    DateTime endDayOfMonth = new DateTime(DateTime.Now.Year, month, 1).AddMonths(1).AddDays(-1);

                    int dayStart = (EndDateFirstWeek.Day + 1) + (week - 2) * 7;
                    int endStart = (EndDateFirstWeek.Day + 1) + (week - 2) * 7 + 6;

                    if (dayStart > endDayOfMonth.Day)
                    {
                        startDay = endDayOfMonth.AddDays(-6);
                        endDay = endDayOfMonth;
                    }
                    else if (endStart > endDayOfMonth.Day)
                    {
                        startDay = new DateTime(DateTime.Now.Year, month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7);
                        endDay = endDayOfMonth;
                    }
                    else
                    {
                        startDay = new DateTime(DateTime.Now.Year, month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7);
                        endDay = new DateTime(DateTime.Now.Year, month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7).AddDays(6);
                    }
                }
            }
            else if (year > 0 && month == 0 && week > 0)
            {
                startDay = new DateTime(year, month, 1);
                endDay = new DateTime(year, month, 1).AddDays((week * 7) - 1);
            }
            else
            {
                int StartDayFirstWeek = (int)(new DateTime(year, month, 1).DayOfWeek);
                if (StartDayFirstWeek == 0)
                {
                    StartDayFirstWeek = 7;
                }
                int rangeFirstWeek = 7 - StartDayFirstWeek;
                DateTime EndDateFirstWeek = new DateTime(year, month, 1).AddDays(rangeFirstWeek);


                if (week == 1)
                {
                    startDay = new DateTime(year, month, 1);
                    endDay = new DateTime(year, month, 1).AddDays(rangeFirstWeek);
                }
                else
                {
                    DateTime endDayOfMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

                    int dayStart = (EndDateFirstWeek.Day + 1) + (week - 2) * 7;
                    int endStart = (EndDateFirstWeek.Day + 1) + (week - 2) * 7 + 6;

                    if (dayStart > endDayOfMonth.Day)
                    {
                        startDay = endDayOfMonth.AddDays(-6);
                        endDay = endDayOfMonth;
                    }
                    else if (endStart > endDayOfMonth.Day)
                    {
                        startDay = new DateTime(year, month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7);
                        endDay = endDayOfMonth;
                    }
                    else
                    {
                        startDay = new DateTime(year, month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7);
                        endDay = new DateTime(year, month, (EndDateFirstWeek.Day + 1) + (week - 2) * 7).AddDays(6);
                    }
                }
            }

            return Tuple.Create(startDay, endDay);
        }

        private void chooseYearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DateTime start = new DateTime(listYear[chooseYear.SelectedIndex], 1, 1);
            //DateTime end = new DateTime(listYear[chooseYear.SelectedIndex], 12, 31);
            if (chooseYear.SelectedIndex < 0)
                return;
            _year = listYear[chooseYear.SelectedIndex];
            Tuple<DateTime, DateTime> date = GetStartAndEndDays(_year, _month, _week);
            _start = date.Item1;
            _end = date.Item2;

            updateDuration(_start, _end);
            refresh(_start, _end);
        }

        private void chooseMonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DateTime end = DateTime.Now;
            //DateTime start = end.AddMonths(-listMonth[chooseMonth.SelectedIndex]);
            if (chooseMonth.SelectedIndex < 0)
                return;
            _month = listMonth[chooseMonth.SelectedIndex];
            Tuple<DateTime, DateTime> date = GetStartAndEndDays(_year, _month, _week);
            _start = date.Item1;
            _end = date.Item2;

            updateDuration(_start, _end);
            refresh(_start, _end);
        }

        private void chooseWeekComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DateTime end = DateTime.Now;
            //DateTime start = end.AddDays(- listWeek[chooseWeek.SelectedIndex] * 7);
            if (chooseWeek.SelectedIndex < 0)
                return;
            _week = listWeek[chooseWeek.SelectedIndex];
            Tuple<DateTime, DateTime> date = GetStartAndEndDays(_year, _month, _week);
            _start = date.Item1;
            _end = date.Item2;

            updateDuration(_start, _end);
            refresh(_start, _end);
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            refreshCombobox();

            _start = new DateTime(2000, 1, 1);
            _end = DateTime.Now;
            updateDuration(_start, _end);
            refresh(_start, _end);
        }
    }
}
