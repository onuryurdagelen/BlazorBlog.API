using BlazorBlog.API.Dtos;
using BlazorBlog.API.Response;

namespace BlazorBlog.API.BusinessRules
{
    public class ArticleManager
    {

        public static ServerDataResponse<AddArticleDto> CheckFileExtension(string fileExtension)
        {
            ServerDataResponse<AddArticleDto> response = new ServerDataResponse<AddArticleDto>();

            if(fileExtension.Contains(".jpg") || fileExtension.Contains(".jpeg") || fileExtension.Contains(".png"))
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Invalid file extension.Please choose .jpgh , .jpeg or .png file.";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            return response;
        }

        public static ServerDataResponse<AddArticleDto> CheckFileSize(long fileSize)
        {
            ServerDataResponse<AddArticleDto> response = new ServerDataResponse<AddArticleDto>();

            if (fileSize <=2)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "File Size must be less than 2MB.";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            return response;
        }
    }
}
