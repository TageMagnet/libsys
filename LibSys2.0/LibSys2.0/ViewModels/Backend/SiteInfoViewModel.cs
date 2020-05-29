using LibrarySystem.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.ViewModels.Backend
{
    /// <summary>
    /// Info bubble available from the HomeView.
    /// Always accessible, whetever logged in or not
    /// </summary>
    public class SiteInfoViewModel : BaseViewModel
    {
        #region Properties
        public string BookInfo { get; set; }
        public string RegisterInfo { get; set; }
        public string GeneralInfo { get; set; }
        // todo; dynamically load actual fee
        private decimal Fee = 75.0M;
        #endregion

        public RelayCommand CloseWindowCommand { get; set; }

        public SiteInfoViewModel()
        {
            GetAllInfo();
            CloseWindowCommand = new RelayCommand(async () => await CloseWindowMethod());
        }

        public async void GetAllInfo()
        {
            BookInfo        = await GetBookInfo();
            RegisterInfo    = await GetRegisterInfo();
            GeneralInfo     = await GetGeneralInfo();
        }

        public async Task<string> GetBookInfo() => string.Format("För att låna bok behöver du registrera ett konto. \nFörsent inlämnad bok beläggs med en avgift på {0} kronor.", Fee.ToString("F0"));

        public async Task<string> GetRegisterInfo() => "För att registrera dig behöver du ange din mailadress";

        public async Task<string> GetGeneralInfo() => "Har du frågor om registrering eller boklån, hör av dig till närmsta bibliotekarie.";

        private async Task CloseWindowMethod()
        {
            var infoView = new SiteInfoView();
            infoView.Close();
        }
    }
}
