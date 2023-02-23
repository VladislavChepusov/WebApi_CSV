using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi_CSV.Models;

namespace WebApi_CSV.Services
{
    public class DataServiceGet
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DataServiceGet(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseValuesModel>> GetValues(String FileName) =>
                  await _context.Values.AsNoTracking()
                    .Where(x => x.FileName == FileName) 
                    .Select(x => _mapper.Map<ResponseValuesModel>(x))
                    .ToListAsync();



        // Закончмть
        public async Task<IEnumerable<ResultModel>> GetResult(FilterModel filter)
        {


            IQueryable<DAL.Entities.Results> dbResult =  _context.Results;

            if (!String.IsNullOrEmpty(filter.FileName))
            {
                dbResult = dbResult.Where(x => x.FileName == filter.FileName);
            }

            Console.WriteLine(@$"Тут {filter.CreationDate_To}  | {filter.AverageTimeWork_From}");




            return await dbResult
                .AsNoTracking()
                .Select(x => _mapper.Map<ResultModel>(x))
                .ToListAsync();
                   
        }
                 
    }
}
