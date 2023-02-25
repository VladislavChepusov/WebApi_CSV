using Microsoft.AspNetCore.Mvc;
using WebApi_CSV.Models;
using WebApi_CSV.Services;
using static WebApi_CSV.Exceptions.CustomExceptions;

namespace WebApi_CSV.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        private readonly DataServiceCreate _dataServiceCreate;
        private readonly DataServiceGet _dataServiceGet;

        public DataController(DataServiceCreate dataServiceCreate, DataServiceGet dataServiceGet)
        {
            _dataServiceCreate = dataServiceCreate;
            _dataServiceGet = dataServiceGet;
        }


        // Метод 1
        [HttpPost]
        public async Task UploadFile(IFormFile files)
        {
            if (files.ContentType != "text/csv")
            {
                throw new CSVException();
            }
            else
            {
                if (files != null)
                {
                    await _dataServiceCreate.DataProcessing(files);
                }
                else
                    throw new CSVException();
            }
        }

        // Метод 2
        [HttpGet]
        public async Task<IEnumerable<ResultModel>> GetResults([FromQuery] FilterModel filter)
             => await _dataServiceGet.GetResults(filter);

        // Метод 3
        [HttpGet]
        public async Task<IEnumerable<ResponseValuesModel>> GetValues(String FileName)
             => await _dataServiceGet.GetValues(FileName);

    }
}
