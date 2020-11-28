using Newtonsoft.Json;
using TheLiter.Core.Network;

namespace TheLiter.Core.Network.Model
{
    public class MessageModel : TcpModel
    {
        public MessageModel() : base()
        {

        }

        private bool _group;
        [JsonProperty("Group")]
        public bool Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }
    }
}
