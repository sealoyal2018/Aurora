using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System; 

[Table("Sys_DataDictionary"), Comment("数据字典")]
[Index(nameof(ParentId), nameof(Sort))]
public class DataDictionary : EntityBase {
    
    [StringLength(19), Comment("上级数据id")]
    public string? ParentId { get; set; }
    
    [Required, StringLength(100), Comment("名称")]
    public string Name { get; set; }
    
    [Required, StringLength(500), Comment("节点连接字符")]
    public string ParentNum { get; set; }
    
    [Required, StringLength(50), Comment("编码")]
    public string Code { get; set; }
    
    [StringLength(500), Comment("描述")]
    public string? Description { get; set; }

    [Required, Comment("排序"), DefaultValue(0)]
    public int Sort { get; set; }

    [StringLength(50), Comment("附加数据")]
    public string? Tag { get; set; }
}