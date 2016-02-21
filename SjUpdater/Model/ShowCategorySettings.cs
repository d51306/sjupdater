﻿using SjUpdater.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SjUpdater.Utils;

namespace SjUpdater.Model
{
    public enum CategoryOrderingType
    {
        Date,
        Alphabetical
    }

    public class ShowCategorySettings : PropertyChangedImpl
    {

        private static readonly Comparer<ShowTileViewModel> AlphabeticalShowComparer = Comparer<ShowTileViewModel>.Create((m1, m2) => String.CompareOrdinal(m1.Title, m2.Title));
        private static readonly Comparer<ShowTileViewModel> DateShowComparer = Comparer<ShowTileViewModel>.Create(
            (vm1, vm2) =>
            {
                if (vm1.Show.NextEpisodeDate.HasValue && vm2.Show.NextEpisodeDate.HasValue) //both have next ep date
                {
                    if (vm1.Show.NextEpisodeDate.Value.Date != vm2.Show.NextEpisodeDate.Value.Date)
                    {
                        if (vm1.Show.NextEpisodeDate.Value.Date < vm2.Show.NextEpisodeDate.Value.Date)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }

                }
                else if (vm1.Show.NextEpisodeDate.HasValue)
                {
                    return -1;
                }
                else if (vm2.Show.NextEpisodeDate.HasValue)
                {
                    return 1;
                }


                if (vm1.Show.PreviousEpisodeDate.HasValue && vm2.Show.PreviousEpisodeDate.HasValue) //both have prev ep date
                {
                    if (vm1.Show.PreviousEpisodeDate.Value.Date != vm2.Show.PreviousEpisodeDate.Value.Date)
                    {
                        if (vm1.Show.PreviousEpisodeDate.Value.Date > vm2.Show.PreviousEpisodeDate.Value.Date)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }

                }
                else if (vm1.Show.PreviousEpisodeDate.HasValue)
                {
                    return -1;
                }
                else if (vm2.Show.PreviousEpisodeDate.HasValue)
                {
                    return 1;
                }

                return AlphabeticalShowComparer.Compare(vm1, vm2);

            });


        private static readonly Dictionary<CategoryOrderingType, Comparer<ShowTileViewModel>> Comparers = new Dictionary<CategoryOrderingType, Comparer<ShowTileViewModel>>()
        {
            {CategoryOrderingType.Alphabetical, AlphabeticalShowComparer },
            {CategoryOrderingType.Date, DateShowComparer }
        };

        private bool _enabled;
        private CategoryOrderingType _orderingType;
        private string _title;
        private string _description;

        private static readonly Dictionary<String, String> CategoryDescriptions = new Dictionary<string, string>()
        {
            {"new","Shows with new Episodes"},
            {"update", "Shows with updated Episodes" },
            {"active","Active Shows"},
            {"ended","Shows that have ended"},
            {"unknown","Shows with unknown state" },
            {"all","All your Tv Shows"}
        };


        public ShowCategorySettings(String title = null, CategoryOrderingType ordering=CategoryOrderingType.Alphabetical, bool enabled=true)
        {
            Title = title;
            OrderingType = ordering;
            Enabled = enabled;
        }

        public ShowCategorySettings()
        {
            Enabled = true;
        }


        public String Title
        {
            get { return _title; }
            internal set
            {
                if (value == _title) return;
                _title = value;
                if (_description == null && CategoryDescriptions.ContainsKey(value))
                {
                    Description = CategoryDescriptions[value];
                }
                OnPropertyChanged();
            }
        }

        public String Description
        {
            get { return _description; }
            internal set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged();
            }
        }


        public CategoryOrderingType OrderingType
        {
            get { return _orderingType; }
            set
            {
                if (value == _orderingType) return;
                _orderingType = value;
                OnPropertyChanged();
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value == _enabled) return;
                _enabled = value;
                OnPropertyChanged();
            }
        }

        public void Sort(ObservableCollection<ShowTileViewModel> shows)
        {
            shows.Sort(Comparers[OrderingType]);
        }

    }
}
