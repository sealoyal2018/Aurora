using Snowflake;

namespace Aurora.Domain.Shared.Core;

public class IdHelper {
    /// <summary>
    /// id生成器
    /// </summary>
    private static readonly IdWorker _worker;

    static IdHelper() {
        _worker = new IdWorker(1, 1);
    }

    public static string Get() => _worker.NextId().ToString();
}
