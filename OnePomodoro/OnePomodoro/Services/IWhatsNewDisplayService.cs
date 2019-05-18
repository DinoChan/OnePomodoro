using System.Threading.Tasks;

namespace OnePomodoro.Services
{
    public interface IWhatsNewDisplayService
    {
        Task ShowIfAppropriateAsync();
    }
}
