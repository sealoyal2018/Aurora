using Aurora.Application.Contract.Systems.Dtos.DataFileDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IDataFileAppService : IAppService<DataFile> {
    
    Task<PageResult<DataFile>> GetPage(DataFilePageInput input);
    
}