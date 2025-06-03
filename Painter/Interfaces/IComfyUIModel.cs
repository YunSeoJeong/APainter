using System.Threading.Tasks;
using Painter.Models;

namespace Painter.Interfaces
{
    public interface IComfyUIModel
    {
        Task<string> GenerateImage(string prompt);
        Task<ImageInfo> GetImageInfo(string imageId);
        Task<Workflow> GetWorkflow(string workflowId);
    }
}