using Mecalux.Domain.Enums;
using Mecalux.Domain.Models;
using Mecalux.Wpf.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace Mecalux.Wpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string? _inputText;
        private string? _processedText;
        private int _wordCount;
        private int _hyphenCount;
        private int _spaceCount;
        private string? _selectedOrder;

        private ObservableCollection<string>? _orderOptions;

        private readonly IDataService _dataService;
        public MainWindowViewModel(IDataService dataService)
        {
            _dataService = dataService;

            // Populate dropdown with order options
            _ = GetOrderOptionsAsync();
        }

        public string InputText
        {
            get => _inputText ?? "";
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }

        public string ProcessedText
        {
            get => _processedText ?? "";
            set
            {
                _processedText = value;
                OnPropertyChanged();
            }
        }

        public int WordCount
        {
            get => _wordCount;
            set
            {
                _wordCount = value;
                OnPropertyChanged();
            }
        }

        public int HyphenCount
        {
            get => _hyphenCount;
            set
            {
                _hyphenCount = value;
                OnPropertyChanged();
            }
        }

        public int SpaceCount
        {
            get => _spaceCount;
            set
            {
                _spaceCount = value;
                OnPropertyChanged();
            }
        }

        public string SelectedOrder
        {
            get => _selectedOrder ?? "";
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
                _ = ProcessTextAsync();
            }
        }

        public ObservableCollection<string> OrderOptions
        {
            get => _orderOptions ?? [];
            set
            {
                _orderOptions = value;
                OnPropertyChanged();
            }
        }

        private async Task ProcessTextAsync()
        {
            if (string.IsNullOrEmpty(InputText)) return;

            try
            {
                var response = await _dataService.OrderedText(new OrderTextRequest
                {
                    TextToOrder = InputText,
                    OrderOption = Enum.Parse<OrderOption>(SelectedOrder)
                });

                if (response != null)
                {
                    ProcessedText = string.Join('\n', response);

                    // Update statics
                    var responseStatistics = await _dataService.GetStatistics(InputText);

                    WordCount = responseStatistics.WordCount;
                    HyphenCount = responseStatistics.HyphenCount;
                    SpaceCount = responseStatistics.SpaceCount;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task GetOrderOptionsAsync()
        {
            try
            {
                var result = JsonSerializer.Deserialize<List<string>>(await _dataService.GetOrderOptions());
                if (result != null)
                {
                    OrderOptions = new ObservableCollection<string>(result);
                    SelectedOrder = OrderOptions.FirstOrDefault()!;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}