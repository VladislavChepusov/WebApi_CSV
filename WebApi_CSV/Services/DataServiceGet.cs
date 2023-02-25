using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
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
                    .OrderByDescending(x => x.CreationDate)
                    .Select(x => _mapper.Map<ResponseValuesModel>(x))
                    .ToListAsync();


        public async Task<IEnumerable<ResultModel>> GetResults(FilterModel filter)
        {
            IQueryable<DAL.Entities.Results> dbResult = _context.Results;

            if (!String.IsNullOrEmpty(filter.FileName))
                dbResult = dbResult.Where(x => x.FileName == filter.FileName);

            if (filter.CreationDate_From != default)
                dbResult = dbResult.Where(x => x.MinDateTime >= filter.CreationDate_From);

            if (filter.CreationDate_To != default)
                dbResult = dbResult.Where(x => x.MinDateTime <= filter.CreationDate_To);

            if (filter.AverageValue_From >= 0)
                dbResult = dbResult.Where(x => x.AverageValue >= filter.AverageValue_From);

            if (filter.AverageValue_To >= 0)
                dbResult = dbResult.Where(x => x.AverageValue <= filter.AverageValue_To);

            if (filter.AverageTimeWork_From >= 0)
                dbResult = dbResult.Where(x => x.AverageTimeWork >= filter.AverageTimeWork_From);

            if (filter.AverageTimeWork_To >= 0)
                dbResult = dbResult.Where(x => x.AverageTimeWork <= filter.AverageTimeWork_To);


            return await dbResult
                .AsNoTracking()
                .OrderByDescending(x => x.MinDateTime)
                .Select(x => _mapper.Map<ResultModel>(x))
                .ToListAsync();
        }
    }
}
