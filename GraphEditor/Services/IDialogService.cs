using System.Threading.Tasks;

namespace GraphEditor.Services;

public interface IDialogService
{
    Task<string?> ShowSaveFileDialogAsync();
}