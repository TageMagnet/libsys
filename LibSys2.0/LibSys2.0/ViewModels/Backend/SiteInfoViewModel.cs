using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.ViewModels.Backend
{
    public class SiteInfoViewModel : BaseViewModel
    {
        /// <summary>
        /// Properties
        /// </summary>
        #region Properties
        public string BookInfo { get; set; }
        public string RegisterInfo { get; set; }
        public string GeneralInfo { get; set; }
        #endregion

        public SiteInfoViewModel()
        {
            GetAllInfo();
        }

        public async void GetAllInfo()
        {
            await GetBookInfo();
            await GetRegisterInfo();
            await GetGeneralInfo();
        }

        public async Task GetBookInfo()
        {
            BookInfo = "För att låna bok behöver du registrera ett konto. \nFörsent inlämnad bok beläggs med en avgift på 75 kronor.";
        }

        public async Task GetRegisterInfo()
        {
            RegisterInfo = "För att registrera dig behöver du ange din mailadress";
        }

        public async Task GetGeneralInfo()
        {
            GeneralInfo = "Har du frågor om registrering eller boklån, hör av dig till närmsta bibliotekarie.";
        }
    }
}
