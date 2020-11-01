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
    }
}
