using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.ViewModels
{
    public class SearchViewModel
    {
        public List<SearchVideosViewModel> Videos { get; set; }
        public List<SearchUsersViewModel> Users { get; set; }
        public double TotalPagesVideos { get; set; }
        public double TotalPagesUsers { get; set; }
        public string SearchQuery { get; set; }

        public SearchViewModel()
        {
        }

        public SearchViewModel(List<SearchVideosViewModel> videos, List<SearchUsersViewModel> users,
            double totalPagesVideos, double totalPagesUsers, string searchQuery)
        {
            Videos = videos;
            Users = users;
            TotalPagesVideos = totalPagesVideos;
            TotalPagesUsers = totalPagesUsers;
            SearchQuery = searchQuery;
        }
    }
}
