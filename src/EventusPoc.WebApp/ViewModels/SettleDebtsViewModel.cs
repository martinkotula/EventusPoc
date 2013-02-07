using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventusPoc.WebApp.ViewModels
{
    public class SettleDebtsViewModel
    {
        public int EventId { get; set; }
        public string EventDate { get; set; }
        public List<SettleDebtsPlayerViewModel> Participants { get; set; }
        public int AddedUserId { get; set; }

        public IEnumerable<SelectListItem> AvailableUsers { get; set; }

        // submit buttons
        public string SaveButton { get; set; }
        public string AddPlayerButton { get; set; }

        public bool IsSaveButtonClicked { get { return !string.IsNullOrEmpty(this.SaveButton); } }
        public bool IsAddPlayerButtonClicked { get { return !string.IsNullOrEmpty(this.AddPlayerButton); } }
    }

    public class SettleDebtsPlayerViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public bool WasPresent { get; set; }
        public bool HasPaid { get; set; }
    }
}