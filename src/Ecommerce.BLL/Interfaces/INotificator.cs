using Ecommerce.BLL.Notifications;

namespace Ecommerce.BLL.Interfaces
{
    public interface INotificator
    {
        bool HasNotification();

        List<Notification> GetNotifications();

        void Handle(Notification notification);
    }
}
