using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContracts.Models;

namespace DataContracts.Services
{
    public interface IVideoGameService
    {
        Task<IEnumerable<VideoGame>> GetAllAsync();
    }
}
