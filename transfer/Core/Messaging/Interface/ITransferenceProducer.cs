using transfer.Core.Entities;

namespace transfer.Core.Messaging.Interface
{
    public interface ITransferenceProducer
    {
        public void Publish(TransferEntity transferEntity);
    }
}
