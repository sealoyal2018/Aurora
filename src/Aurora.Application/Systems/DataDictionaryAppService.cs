using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.DataDictionaryDtos;
using Aurora.Application.Contract.Systems.Dtos.OrganizeDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Aurora.Application.Systems; 

public class DataDictionaryAppService : AppServiceBase<DataDictionary>, IDataDictionaryAppService, ITransientDependency {

    private readonly ICurrentUser _currentUser;
    
    public DataDictionaryAppService(IRepository repository, ICurrentUser currentUser) : base(repository) {
        this._currentUser = currentUser;
    }

    public async Task<IEnumerable<DataDictionaryTreeDto>> GetTreeAsync() {
        Expression<Func<DataDictionary, DataDictionaryTreeDto>> selectExpr = o => new DataDictionaryTreeDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();
        var data = await GetQueryable().Select(selectExpr).ToListAsync();
        return data.Build<string, DataDictionaryTreeDto>();
    }

    public async Task RemoveAsync(params string[] ids) {
        var list = await this.GetQueryable().Select(p => new { Id = p.Id, p.ParentNum }).ToListAsync();
        var deletingIds = new List<string>();
        deletingIds.AddRange(ids);
        foreach (var item in list) {
            var arr = item.ParentNum.Split(',');
            var ret = arr.Any(ids.Contains);
            if (ret) {
                deletingIds.Add(item.Id);
            }
        }
        await this.DeleteAsync(t => deletingIds.Contains(t.Id));
    }

    public async Task SaveOrUpdateAsync(DataDictionary input) {
        if (input.Name.IsNullOrEmpty()) {
            throw new BusException(false, "请填写字典名称", BusCodeType.NotAcceptable);
        }

        DataDictionary? parentDict = null;
        if (input.ParentId.IsNotNullOrEmpty()) {
            parentDict = await this.GetQueryable().FirstOrDefaultAsync(t => t.Id == input.ParentId);
        }
        
        if (input.Id.IsNullOrEmpty()) {
            // 添加
            input.Id = IdHelper.Get();
            input.CreatedTime = DateTime.Now;
            input.CreatorId = this._currentUser.UserId;
            input.IsDeleted = false;
            input.ParentNum = input.Id;
            if (parentDict != null) {
                input.ParentNum = parentDict.Id + "," + input.Id;
            }

            await this.InsertAsync(input);
            return;
        }

        // 编辑
        var currentDict = await this.FindAsync(t => t.Id == input.Id);
        if (currentDict.IsNullOrEmpty()) {
            throw new BusException(false, "未找到数据", BusCodeType.Forbidden);
        }
        
        if (parentDict?.Id != currentDict.ParentId) {
            // 更换父组织
            var childrenDict = await this.GetListAsync(t => t.ParentNum.Contains(input.Id));
            if (childrenDict.IsNotNullOrEmpty()) {
                currentDict.ParentId = input.ParentId;
                currentDict.ParentNum = parentDict.ParentNum + "," + currentDict.Id;

                foreach (var child in childrenDict) {
                    var arr = child.ParentNum.Split(',');
                    var index = Array.FindIndex(arr, t => t == input.Id);
                    child.ParentNum = currentDict.ParentNum + "," + string.Join(',', arr[(index)..]);
                }

                await this.UpdateAsync(childrenDict);
            }
        }

        currentDict.Name = input.Name;
        currentDict.ParentId = input.ParentId;
        currentDict.Description = input.Description;
        currentDict.Sort = input.Sort;
        currentDict.Code = input.Code;
        await this.UpdateAsync(currentDict);
    }

    public async Task<DataDictionary> GetData([NotNull]string dataId) {
        return await this.FindAsync(t => t.Id == dataId);
    }

    public async Task<IEnumerable<DataDictionaryTreeDto>> GetTreeAsync(string code) {
        var data = await this.FindAsync(t => t.Code == code);
        if (data.IsNullOrEmpty()) {
            return new List<DataDictionaryTreeDto>();
        }

        Expression<Func<DataDictionary, DataDictionaryTreeDto>> selectExpr = d => new DataDictionaryTreeDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var list = await GetQueryable()
            .Where(t => t.ParentNum.Contains(data.Id) && t.Id != data.Id)
            .Select(selectExpr)
            .ToListAsync();
        
        if (list.IsNullOrEmpty()) {
            return new List<DataDictionaryTreeDto>();
        }
        foreach (var v in list.Where(item=> item.ParentId == data.Id)) {
            v.ParentId = null;
        }
        
        return list.Build<string, DataDictionaryTreeDto>();
    }

    public async Task<IEnumerable<DataDictionary>> GetChildrenAsync(string parentCode) {
        var data = await this.FindAsync(t => t.Code == parentCode);
        if (data.IsNullOrEmpty()) {
            return new List<DataDictionary>();
        }
        
        var list = await GetQueryable()
            .Where(t => t.ParentNum.Contains(data.Id) && t.Id != data.Id)
            .OrderBy(t=> t.Sort)
            .ToListAsync();

        return list; 
    }


    public async Task<IEnumerable<DataDictionaryTreeDto>> GetTreeByTagAsync(string tag) {
        if (tag.IsNullOrEmpty()) {
            return new List<DataDictionaryTreeDto>();
        }

        Expression<Func<DataDictionary, DataDictionaryTreeDto>> selectExpr = d => new DataDictionaryTreeDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var list = await GetQueryable()
            .Where(t => t.Tag == tag)
            .Select(selectExpr)
            .ToListAsync();

        if (list.IsNullOrEmpty()) {
            return new List<DataDictionaryTreeDto>();
        }

        return list.Build<string, DataDictionaryTreeDto>();
    }
}