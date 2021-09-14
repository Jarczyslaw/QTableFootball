using JToolbox.Core;
using JToolbox.Core.Extensions;
using JToolbox.Desktop.Dialogs;
using JToolbox.WPF.Core.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace QTableFootball.App.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly IDialogsService dialogs = new DialogsService();
        private string activePlayersHeader;
        private string playersHeader;
        private PlayerViewModel selectedPlayer;
        private string selectedPlayerName;

        public MainViewModel()
        {
            UpdateHeaders();
        }

        public RelayCommand ActivateAllPlayersCommand => new RelayCommand(() =>
        {
            var players = Players.ToList();
            players.ForEach(x => MovePlayer(x, Players, ActivePlayers));
            UpdateHeaders();
        });

        public RelayCommand ActivatePlayersCommand => new RelayCommand(() =>
        {
            if (CheckSelectedPlayers(SelectedPlayers))
            {
                SelectedPlayers.ForEach(x => MovePlayer(x, Players, ActivePlayers));
                UpdateHeaders();
            }
        });

        public ObservableCollection<PlayerViewModel> ActivePlayers { get; } = new ObservableCollection<PlayerViewModel>();

        public string ActivePlayersHeader
        {
            get => activePlayersHeader;
            set => Set(ref activePlayersHeader, value);
        }

        public RelayCommand AddPlayerCommand => new RelayCommand(() =>
        {
            if (CheckPlayerName(SelectedPlayerName))
            {
                var newPlayer = new PlayerViewModel
                {
                    Name = SelectedPlayerName
                };
                Players.Add(newPlayer);
                Players.OrderBy(x => x.Name);
                UpdateHeaders();

                SelectedPlayer = newPlayer;
                SelectedPlayerName = string.Empty;
            }
        });

        public RelayCommand DeactivateAllPlayersCommand => new RelayCommand(() =>
        {
            var activePlayers = ActivePlayers.ToList();
            activePlayers.ForEach(x => MovePlayer(x, ActivePlayers, Players));
            UpdateHeaders();
        });

        public RelayCommand DeactivatePlayersCommand => new RelayCommand(() =>
        {
            if (CheckSelectedPlayers(SelectedActivePlayers))
            {
                SelectedActivePlayers.ForEach(x => MovePlayer(x, ActivePlayers, Players));
                UpdateHeaders();
            }
        });

        public RelayCommand EditPlayerCommand => new RelayCommand(() =>
        {
            if (SelectedPlayer == null)
            {
                dialogs.ShowError("No player selected");
                return;
            }

            if (CheckPlayerName(SelectedPlayerName))
            {
                SelectedPlayer.Name = SelectedPlayerName;
                Players.OrderBy(x => x.Name);
            }
        });

        public ObservableCollection<PlayerViewModel> Players { get; } = new ObservableCollection<PlayerViewModel>();

        public string PlayersHeader
        {
            get => playersHeader;
            set => Set(ref playersHeader, value);
        }

        public RelayCommand RemovePlayersCommand => new RelayCommand(() =>
        {
            if (CheckSelectedPlayers(SelectedPlayers)
                && dialogs.ShowYesNoQuestion("Do you want to remove selected players?"))
            {
                SelectedPlayers.ForEach(x => Players.Remove(x));
                UpdateHeaders();
            }
        });

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

        public RelayCommand ShuffleCommand => new RelayCommand(() =>
        {
            if (ActivePlayers.Count == 0)
            {
                dialogs.ShowError("No active players");
                return;
            }

            if (ActivePlayers.Count % 2 == 1)
            {
                dialogs.ShowError("Active players count must be even");
                return;
            }

            var activePlayers = ActivePlayers.ToList();
            activePlayers.StrongShuffle();
            var squadsCount = activePlayers.Count / 2;

            Squads.Clear();
            for (int i = 0; i < squadsCount; i++)
            {
                var index1 = 2 * i;
                var index2 = index1 + 1;

                Squads.Add(new SquadViewModel
                {
                    Number = i + 1,
                    Player1 = activePlayers[index1],
                    Player2 = activePlayers[index2]
                });
            }
        });

        public ObservableCollection<SquadViewModel> Squads { get; } = new ObservableCollection<SquadViewModel>();

        private bool CheckPlayerName(string name)
        {
            if (!ValidatePlayerName(name))
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

        private void MovePlayer(PlayerViewModel player, ObservableCollection<PlayerViewModel> from, ObservableCollection<PlayerViewModel> to)
        {
            from.Remove(player);
            to.Add(player);
        }

        private void UpdateHeaders()
        {
            PlayersHeader = $"All players - {Players.Count}";
            ActivePlayersHeader = $"Active players - {ActivePlayers.Count}";
        }

        private bool ValidatePlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (name.Length < 3)
            {
                return false;
            }

            var allNames = Players.Select(x => x.Name).ToList();
            allNames.AddRange(ActivePlayers.Select(x => x.Name));
            return allNames.Count(x => x == name) <= 1;
        }
    }
}