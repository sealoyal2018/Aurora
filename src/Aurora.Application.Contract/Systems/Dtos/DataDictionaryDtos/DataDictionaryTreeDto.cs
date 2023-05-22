using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.DataDictionaryDtos; 

public class DataDictionaryTreeDto : DataDictionary, ITreeModel<string, DataDictionaryTreeDto> {
    public IEnumerable<DataDictionaryTreeDto> Children { get; set; } = new List<DataDictionaryTreeDto>();
    public string CheckArr { get; set; } = "0";
}