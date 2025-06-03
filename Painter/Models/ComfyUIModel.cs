using System.Net.Http;
using System.Threading.Tasks;
using System;
using Painter.Interfaces;
using Painter.Models;

namespace Painter.Models
{
    public class ComfyUIModel : IComfyUIModel
    {
        private HttpClient _httpClient;
        private string _apiEndpoint;

        public ComfyUIModel(HttpClient httpClient, string apiEndpoint)
        {
            _httpClient = httpClient;
            _apiEndpoint = apiEndpoint;
        }

        public Task<string> GenerateImage(string prompt) 
        { 
            throw new NotImplementedException(); 
        }

        public Task<ImageInfo> GetImageInfo(string imageId) 
        { 
            throw new NotImplementedException(); 
        }

        public Task<Workflow> GetWorkflow(string workflowId) 
        { 
            throw new NotImplementedException(); 
        }
    }
}