using Aurora.Application.Contract.Systems.Dtos.DataDictionaryDtos;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IDataDictionaryAppService : IAppService<DataDictionary> {

    Task<IEnumerable<DataDictionaryTreeDto>> GetTreeAsync();
    
    Task RemoveAsync(params string[] ids);
    
    Task SaveOrUpdateAsync(DataDictionary input);
    Task<DataDictionary> GetData(string dataId);
    
    /// <summary>
    /// 获取指定编码字典数, 不包含当前
    /// </summary>
    /// <param name="code">指定编码</param>
    /// <returns></returns>
    Task<IEnumerable<DataDictionaryTreeDto>> GetTreeAsync(string code);

    Task<IEnumerable<DataDictionary>> GetChildrenAsync(string parentCode);
    Task<IEnumerable<DataDictionaryTreeDto>> GetTreeByTagAsync(string tag);
}