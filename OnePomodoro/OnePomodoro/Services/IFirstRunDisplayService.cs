using System.Threading.Tasks;

namespace OnePomodoro.Services
{
    public interface IFirstRunDisplayService
    {
        Task ShowIfAppropriateAsync();
    }
}
