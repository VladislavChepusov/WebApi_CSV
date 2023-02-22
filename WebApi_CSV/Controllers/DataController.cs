using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using WebApi_CSV.Services;
using static System.Net.WebRequestMethods;
using static WebApi_CSV.Exceptions.CustomExceptions;

namespace WebApi_CSV.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        private readonly DataService _dataService;

        public DataController(DataService dataService)
        {
            _dataService = dataService;
        }


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
                    await _dataService.DataProcessing(files);

                    /*
                    using (var reader = new StreamReader(files.OpenReadStream()))
                    {
                        
                        Console.WriteLine(await reader.ReadToEndAsync());
                    }
                    */
                }
                else
                    throw new testNotFoundException();
            }   
        }
    }
}
