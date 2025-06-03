using System.Net.Http;
using System.Threading.Tasks;
using System;
using Painter.Models; // ImageInfo, Workflow 클래스를 사용하기 위해 추가

namespace Painter.Models
{
    // Models
    public class ComfyUIModel
    {
        private HttpClient _httpClient; // ComfyUI API 통신
        private string _apiEndpoint; // ComfyUI API 엔드포인트

        /// <summary>
        /// ComfyUIModel 생성자
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        /// <param name="apiEndpoint">ComfyUI API 엔드포인트</param>
        public ComfyUIModel(HttpClient httpClient, string apiEndpoint)
        {
            _httpClient = httpClient;
            _apiEndpoint = apiEndpoint;
        }

        /// <summary>
        /// ComfyUI API를 사용하여 이미지 생성
        /// </summary>
        /// <param name="prompt">프롬프트</param>
        /// <returns>이미지 생성 결과</returns>
        public Task<string> GenerateImage(string prompt) { throw new NotImplementedException(); }

        /// <summary>
        /// 이미지 정보 가져오기
        /// </summary>
        /// <param name="imageId">이미지 ID</param>
        /// <returns>이미지 정보</returns>
        public Task<ImageInfo> GetImageInfo(string imageId) { throw new NotImplementedException(); }

        /// <summary>
        /// 워크플로우 정보 가져오기
        /// </summary>
        /// <param name="workflowId">워크플로우 ID</param>
        /// <returns>워크플로우 정보</returns>
        public Task<Workflow> GetWorkflow(string workflowId) { throw new NotImplementedException(); }
    }
}