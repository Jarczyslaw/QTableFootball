using JToolbox.Core;
using JToolbox.Core.Extensions;
using JToolbox.Desktop.Dialogs;
using JToolbox.WPF.Core.Base;
using QTableFootball.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace QTableFootball.App.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly List<PlayerViewModel> allPlayers = new List<PlayerViewModel>();
        private readonly IDialogsService dialogs = new DialogsService();
        private readonly LocalStorage localStorage = new LocalStorage();
        private ObservableCollection<PlayerViewModel> activePlayers = new ObservableCollection<PlayerViewModel>();
        private string activePlayersHeader;
        private ObservableCollection<PlayerViewModel> players = new ObservableCollection<PlayerViewModel>();
        private string playersHeader;
        private PlayerViewModel selectedPlayer;
        private string selectedPlayerName;
        private StorageContent storageContent;
        private string windowHeader = "QTable Football";

        public MainViewModel()
        {
            LoadPlayers();
            UpdateHeaders();
        }

        public RelayCommand ActivateAllPlayersCommand => new RelayCommand(ActivateAllPlayers);

        public RelayCommand ActivatePlayersCommand => new RelayCommand(ActivatePlayers);

        public ObservableCollection<PlayerViewModel> ActivePlayers
        {
            get => activePlayers;
            set => Set(ref activePlayers, value);
        }

        public string ActivePlayersHeader
        {
            get => activePlayersHeader;
            set => Set(ref activePlayersHeader, value);
        }

        public RelayCommand AddPlayerCommand => new RelayCommand(AddPlayer);

        public RelayCommand DeactivateAllPlayersCommand => new RelayCommand(DeactivateAllPlayers);

        public RelayCommand DeactivatePlayersCommand => new RelayCommand(DeactivatePlayers);

        public RelayCommand EditPlayerCommand => new RelayCommand(EditPlayer);

        public ObservableCollection<PlayerViewModel> Players
        {
            get => players;
            set => Set(ref players, value);
        }

        public string PlayersHeader
        {
            get => playersHeader;
            set => Set(ref playersHeader, value);
        }

        public RelayCommand RemovePlayersCommand => new RelayCommand(RemovePlayers);

        public List<PlayerViewModel> SelectedActivePlayers => ActivePlayers.Where(x => x.IsSelected)
            .ToList();

        public PlayerViewModel SelectedPlayer
        {
            get => selectedPlayer;
            set
            {
                Set(ref selectedPlayer, value);
                SelectedPlayerName = SelectedPlayer?.Name;
            }
        }

        public string SelectedPlayerName
        {
            get => selectedPlayerName;
            set => Set(ref selectedPlayerName, value);
        }

        public List<PlayerViewModel> SelectedPlayers => Players.Where(x => x.IsSelected)
            .ToList();

        public RelayCommand ShuffleCommand => new RelayCommand(Shuffle);

        public ObservableCollection<SquadViewModel> Squads { get; } = new ObservableCollection<SquadViewModel>();

        public string WindowHeader
        {
            get => windowHeader;
            set => Set(ref windowHeader, value);
        }

        private void ActivateAllPlayers()
        {
            var players = Players.ToList();

            players.ForEach(x => MovePlayer(x, Players, ActivePlayers));
            ActivePlayers.ForEach(x => x.IsSelected = true);
            AfterCollectionsChanged();
        }

        private void ActivatePlayers()
        {
            if (CheckSelectedPlayers(SelectedPlayers))
            {
                ActivePlayers.ForEach(x => x.IsSelected = false);
                SelectedPlayers.ForEach(x => MovePlayer(x, Players, ActivePlayers));
                AfterCollectionsChanged();
            }
        }

        private void AddPlayer()
        {
            if (CheckPlayerName(SelectedPlayerName, true))
            {
                var newPlayer = new PlayerViewModel
                {
                    Name = SelectedPlayerName
                };

                allPlayers.Add(newPlayer);
                SavePlayers();

                Players.Add(newPlayer);
                AfterCollectionsChanged();

                SelectedPlayer = newPlayer;
                SelectedPlayerName = string.Empty;
            }
        }

        private void AfterCollectionsChanged()
        {
            SortCollections();
            UpdateHeaders();
        }

        private bool CheckPlayerName(string name, bool add)
        {
            if (!ValidatePlayerName(name, add))
            {
                dialogs.ShowError("Player name should be unique and contain minimum 3 characters");
                return false;
            }
            return true;
        }

        private bool CheckSelectedPlayers(IEnumerable<PlayerViewModel> players)
        {
            if (!players.Any())
            {
                dialogs.ShowError("No players selected");
                return false;
            }

            return true;
        }

        private void DeactivateAllPlayers()
        {
            var activePlayers = ActivePlayers.ToList();
            activePlayers.ForEach(x => MovePlayer(x, ActivePlayers, Players));
            Players.ForEach(x => x.IsSelected = true);
            AfterCollectionsChanged();
        }

        private void DeactivatePlayers()
        {
            if (CheckSelectedPlayers(SelectedActivePlayers))
            {
                Players.ForEach(x => x.IsSelected = false);
                SelectedActivePlayers.ForEach(x => MovePlayer(x, ActivePlayers, Players));
                AfterCollectionsChanged();
            }
        }

        private void EditPlayer()
        {
            if (SelectedPlayer == null)
            {
                dialogs.ShowError("No player selected");
                return;
            }

            if (CheckPlayerName(SelectedPlayerName, false))
            {
                SelectedPlayer.Name = SelectedPlayerName;
                AfterCollectionsChanged();

                SavePlayers();
            }
        }

        private void LoadPlayers()
        {
            try
            {
                storageContent = localStorage.Load();
                storageContent.SquadSize = Math.Max(2, storageContent.SquadSize);

                WindowHeader += $" (squad size: {storageContent.SquadSize})";

                var loadedPlayers = storageContent
                    .Players
                    .Select(x => new PlayerViewModel
                    {
                        Name = x
                    })
                    .OrderBy(x => x.Name);

                allPlayers.AddRange(loadedPlayers);
                allPlayers.ForEach(x => x.OnSelectedChanged += UpdateHeaders);

                Players.AddRange(allPlayers);
            }
            catch (Exception exc)
            {
                dialogs.ShowException(exc, "Exception occured during loading players");
            }
        }

        private void MovePlayer(PlayerViewModel player, ObservableCollection<PlayerViewModel> from, ObservableCollection<PlayerViewModel> to)
        {
            from.Remove(player);
            to.Add(player);
        }

        private void RemovePlayers()
        {
            if (CheckSelectedPlayers(SelectedPlayers)
                              && dialogs.ShowYesNoQuestion("Do you want to remove selected players?"))
            {
                SelectedPlayers.ForEach(x =>
                {
                    allPlayers.Remove(x);
                    Players.Remove(x);
                });
                SavePlayers();
                AfterCollectionsChanged();
            }
        }

        private void SavePlayers()
        {
            try
            {
                storageContent.Players = allPlayers
                    .Select(x => x.Name)
                    .OrderBy(x => x)
                    .ToList();
                localStorage.Save(storageContent);
            }
            catch (Exception exc)
            {
                dialogs.ShowException(exc, "Exception occured during saving players");
            }
        }

        private void Shuffle()
        {
            if (ActivePlayers.Count == 0)
            {
                dialogs.ShowError("No active players");
                return;
            }

            if (ActivePlayers.Count % storageContent.SquadSize != 0)
            {
                dialogs.ShowError($"Active players count must be divisible by squad size ({storageContent.SquadSize})");
                return;
            }

            var activePlayers = ActivePlayers.ToList();
            activePlayers.StrongShuffle();
            var squadsCount = activePlayers.Count / storageContent.SquadSize;

            Squads.Clear();
            for (int i = 0; i < squadsCount; i++)
            {
                var index = i * storageContent.SquadSize;
                var squadPlayers = activePlayers.GetRange(index, storageContent.SquadSize);

                Squads.Add(new SquadViewModel
                {
                    Number = i + 1,
                    Players = string.Join(Environment.NewLine, squadPlayers.Select(x => x.Name))
                });
            }
        }

        private void SortCollections()
        {
            ActivePlayers = new ObservableCollection<PlayerViewModel>(ActivePlayers.OrderBy(x => x.Name));
            Players = new ObservableCollection<PlayerViewModel>(Players.OrderBy(x => x.Name));
        }

        private void UpdateHeaders()
        {
            PlayersHeader = $"All players - {Players.Count}";
            if (SelectedPlayers.Count > 0)
            {
                PlayersHeader += $" ({SelectedPlayers.Count})";
            }

            ActivePlayersHeader = $"Active players - {ActivePlayers.Count}";
            if (SelectedActivePlayers.Count > 0)
            {
                ActivePlayersHeader += $" ({SelectedActivePlayers.Count})";
            }
        }

        private bool ValidatePlayerName(string name, bool add)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (name.Length < 3)
            {
                return false;
            }

            var allNames = allPlayers.Select(x => x.Name);
            return allNames.Count(x => x == name) <= (add ? 0 : 1);
        }
    }
}