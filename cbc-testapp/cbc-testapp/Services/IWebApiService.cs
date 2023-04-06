using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cbc_testapp.Services
{
    public interface IWebApiService
    {
        Task<Trs> Post<TRq, Trs>(TRq request, string relativeUrl) where Trs : class;
        Task<Trs> Get<Trs>(string request) where Trs : class;

    }
}
