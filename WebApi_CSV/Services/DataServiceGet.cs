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
                    .Select(x => _mapper.Map<ResponseValuesModel>(x))
                    .ToListAsync();
    }
}
