using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.Diagnostics;
using Wpf.Util.Core.Model;

namespace Wpf.Util.Core.ViewModels
{
    public class LogMessageContainerViewModel :CoreViewModel
    {
        /// <summary>
        /// Selected log level.
        /// </summary>
        private string _selectedLogLevel;

        /// <summary>
        /// Filtered log messages collections.
        /// </summary>
        private SafeObservableCollection<LogMessage> _filteredLogMessages = new SafeObservableCollection<LogMessage>();

        public LogMessageContainerViewModel(ILogger logger)
        {
            Logger = logger;
            this.ClearCommand = new DelegateCommand(() =>
            {
                this.Logger.Clear();
                this._filteredLogMessages.Clear();
                OnPropertyChanged(()=> this.LogMessages);
            });

            this.SelectedLogLevel = this.LogLevels.First();
            this.Logger.LogMessages.CollectionChanged += (s, e) =>
            {
                if (this.SelectedLogLevel == "All")
                {
                    return;     // If ALL items then no need to maintain our filtered messages.
                }

                if (e.NewItems == null)
                {
                    this._filteredLogMessages.Clear();
                }
                else
                {
                    e.NewItems.OfType<LogMessage>()
                        .Where(l => l.Level.ToString() == this._selectedLogLevel)
                        .ToList()
                        .ForEach(l => this._filteredLogMessages.Add(l));
                }
            };
        }

        /// <summary>
        /// Gets logger instance.
        /// </summary>
        public ILogger Logger { get; }


        /// <summary>
        /// Gets or sets Clear command handler.
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        /// Gets log messages.
        /// </summary>
        public ObservableCollection<LogMessage> LogMessages
        {
            get
            {
                if (this.SelectedLogLevel == "All")
                {
                    return this.Logger.LogMessages;
                }

                return this._filteredLogMessages;
            }
        }

        /// <summary>
        /// Gets log levels as strin.
        /// </summary>
        public string[] LogLevels => new string[]
        {
            "All",
            LogLevel.Debug.ToString(),
            LogLevel.Error.ToString(),
            LogLevel.Info.ToString(),
        };

        /// <summary>
        /// Gets or sets selected log level.
        /// </summary>
        public string SelectedLogLevel
        {
            get => this._selectedLogLevel;
            set
            {
                this._selectedLogLevel = value;
                if (value == "All")
                {
                    this._filteredLogMessages.Clear();
                }
                else
                {
                    this._filteredLogMessages =
                        new SafeObservableCollection<LogMessage>(
                            this.Logger.LogMessages.Where(l => l.Level.ToString() == value).ToList());
                }

                this.OnPropertyChanged(() => this.LogMessages);
            }
        }
    }
}
