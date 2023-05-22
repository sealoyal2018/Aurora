using Aurora.Domain.Shared.Core.Aop;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Aurora.Domain.Shared.DependencyInjections;

/// <summary>
/// 依赖注入扩展
/// </summary>
public static class DependencyInjectionExtensions {
    private static readonly List<string> allAssemblies = new List<string>() {
        "Aurora.Domain",
        "Aurora.Domain.Shared",
        "Aurora.EntityFrameworkCore",
        "Aurora.Application.Contract",
        "Aurora.Application",
        "Aurora.WebApp",
    };
    public static readonly List<Assembly> allFxAssemblies = new List<Assembly>();


    private static readonly ProxyGenerator _generator = new ProxyGenerator();

    /// <summary>
    /// 全局自动注入服务
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static IServiceCollection Autowired(this IServiceCollection self) {
        var lifeTimeMap = new Dictionary<Type, ServiceLifetime>
        {
            { typeof(ITransientDependency), ServiceLifetime.Transient},
            { typeof(IScopedDependency),ServiceLifetime.Scoped},
            { typeof(ISingletonDependency),ServiceLifetime.Singleton}
        };

        foreach(var item in allAssemblies) {
            allFxAssemblies.Add(Assembly.Load(item));
        }
        // using var fs = new StreamWriter(new FileStream("log.txt", FileMode.Create, FileAccess.Write));
        var allTypes = allFxAssemblies.SelectMany(x => x.GetTypes()).ToList();
        List<Type> interfaces = new List<Type>();
        Type theDependency;
        allTypes.ForEach(aType =>
        {
            lifeTimeMap.ToList().ForEach(aMap =>
            {
                theDependency = aMap.Key;
                if (theDependency.IsAssignableFrom(aType) && theDependency != aType && !aType.IsAbstract && aType.IsClass) {
                    //注入实现
                    self.Add(new ServiceDescriptor(aType, aType, aMap.Value));

                    interfaces = allTypes.Where(x => x.IsAssignableFrom(aType) && x.IsInterface && x != theDependency).ToList();
                    //有接口则注入接口
                    if (interfaces.Count > 0) {
                        interfaces.ForEach(aInterface =>
                        {
                            // fs.WriteLine($"type: {aType}, interface: {aInterface}");
                            //注入AOP
                            self.Add(new ServiceDescriptor(aInterface, serviceProvider =>
                            {
                                return _generator.CreateInterfaceProxyWithTarget(aInterface, serviceProvider.GetService(aType), new CastleInterceptor(serviceProvider));
                            }, aMap.Value));
                        });
                    }
                    //无接口则注入自己
                    else {
                        self.Add(new ServiceDescriptor(aType, aType, aMap.Value));
                    }
                }
            });
        });
        return self;
    }
}
