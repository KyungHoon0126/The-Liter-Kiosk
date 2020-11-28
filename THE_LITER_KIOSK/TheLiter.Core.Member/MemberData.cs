using System.Threading.Tasks;
using TheLiter.Core.Member.ViewModel;

namespace TheLiter.Core.Member
{
    public class MemberData
    {
        public MemberViewModel memberViewModel = new MemberViewModel();

        public void Login()
        {
            memberViewModel.OnLogin();
        }

        public async Task<bool> AutoLogin()
        {
            return await memberViewModel.IsValidAutoLogin();
        }

        public void GetMemberData()
        {
            memberViewModel.GetMemberData();
        }

        public void GetAllMemberData()
        {
            memberViewModel.GetAllMemberData();
        }
    }
}
