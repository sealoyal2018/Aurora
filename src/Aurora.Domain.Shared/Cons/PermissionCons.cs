namespace Aurora.Domain.Shared.Cons; 

/// <summary>
/// 权限码
/// </summary>
public static class PermissionCons {
    public const string PROJECT_QUERY = "projects.query";

    public static class WeekInvest {
        private const string PREFIX = "weekInvest";
        public const string CREATE = $"{PREFIX}.create";
        public const string UPDATE = $"{PREFIX}.update";
        public const string REMOVE = $"{PREFIX}.remove";

        public static class Construct {
            private const string PREFIX = "weekInvest.construct";
            public const string CREATE = $"{PREFIX}.create";
            public const string UPDATE = $"{PREFIX}.update";
            public const string REMOVE = $"{PREFIX}.remove";
            public const string Audit = $"{PREFIX}.audit";
        }
    }

    public static class Organize {
        private const string PREFIX = "organize";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string BATCH_REMOVE = $"{PREFIX}.batchRemove";
        public const string QUERY = $"{PREFIX}.query";
    }

    public static class User {
        private const string PREFIX = "user";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string POWER = $"{PREFIX}.power";
        public const string QUERY = $"{PREFIX}.query";
        public const string BATCH_REMOVE = $"{PREFIX}.batchRemove";
    }

    public static class Role {
        private const string PREFIX = "role";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string QUERY = $"{PREFIX}.query";
        public const string BATCH_REMOVE = $"{PREFIX}.batchRemove";
        public const string POWER = $"{PREFIX}.power";
        public const string VIEW = $"{PREFIX}.view";
        public const string USER_ENABLE = $"{PREFIX}.user.enable";
    }

    public static class Resource { // resource
        private const string PREFIX = "resource";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string BATCH_REMOVE = $"{PREFIX}.batchRemove";
        public const string QUERY = $"{PREFIX}.query";
    }

    public static class PromoteAudit {
        private const string PREFIX = "promote.audit";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        // public const string QUERY = $"{PREFIX}.query";
    }

    public static class Promote {
        private const string PREFIX = "promote";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string QUERY = $"{PREFIX}.query";
        public const string EXPORT = $"{PREFIX}.export";
        public const string QUERY_ALL = $"{PREFIX}.query.all";
        public const string TRANSFORM_NEGOTIATE = $"{PREFIX}.transform.negotiate";
        public const string TRANSFORM_CONTRACT = $"{PREFIX}.transform.contract";
        public const string TRANSFORM_CONSTRUCT = $"{PREFIX}.transform.construct";
    }

    public static class Connect {
        private const string PREFIX = "connect";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
    }
    
    public static class OperateLog {
        private const string PREFIX = "operateLog";
        public const string QUERY = $"{PREFIX}.query";
    }

    public static class NegotiateLog {
        private const string PREFIX = "negotiate.log";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";

        public static class Contract {
            private const string PREFIX = "contract.negotiate.log";
            public const string ADD = $"{PREFIX}.add";
            public const string EDIT = $"{PREFIX}.edit";
            public const string REMOVE = $"{PREFIX}.remove";
        }

        public static class Construct {
            private const string PREFIX = "construct.negotiate.log";
            public const string ADD = $"{PREFIX}.add";
            public const string EDIT = $"{PREFIX}.edit";
            public const string REMOVE = $"{PREFIX}.remove";
        }
        
    }
    
    public static class Negotiate {
        private const string PREFIX = "negotiate";
        public const string QUERY = $"{PREFIX}.query";
        public const string QUERY_ALL = $"{PREFIX}.query.all";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string IMPORTANT_STATISTICS = $"{PREFIX}.important.statistics.query";
        public const string STATISTICS = $"{PREFIX}.statistics.query";
        public const string IMPORT = $"{PREFIX}.import";
        public const string EXPORT = $"{PREFIX}.export";
        public const string EXPORT_IMPORTANT = $"{PREFIX}.export.important";
        public const string TRANSFORM_CONTRACT = $"{PREFIX}.transform.contract";
        public const string TRANSFORM_CONSTRUCT = $"{PREFIX}.transform.construct";
    }

    public static class Document {
        private const string PREFIX = "document";
        // private const string QUERY = $"{PREFIX}.query";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";

        public static class Contract {
            private const string PREFIX = "document.contract";
            // private const string QUERY = $"{PREFIX}.query";
            public const string ADD = $"{PREFIX}.add";
            public const string EDIT = $"{PREFIX}.edit";
            public const string REMOVE = $"{PREFIX}.remove";
            public const string DOWNLOAD = $"{PREFIX}.download";
        }
        
        public static class Construct {
            private const string PREFIX = "document.construct";
            // private const string QUERY = $"{PREFIX}.query";
            public const string ADD = $"{PREFIX}.add";
            public const string EDIT = $"{PREFIX}.edit";
            public const string REMOVE = $"{PREFIX}.remove";
            public const string DOWNLOAD = $"{PREFIX}.download";
        }
    }
    
    public static class DataDictionary {
        private const string PREFIX = "dataDictionary";
        // private const string QUERY = $"{PREFIX}.query";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string BATCH_REMOVE = $"{PREFIX}.batchRemove";
    }

    public static class ContractLog {
        private const string PREFIX = "contract.log";
        // public const string QUERY = $"{PREFIX}.query";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        
        public static class Construct {
            private const string PREFIX = "contract.log.construct";
            // public const string QUERY = $"{PREFIX}.query";
            public const string ADD = $"{PREFIX}.add";
            public const string EDIT = $"{PREFIX}.edit";
            public const string REMOVE = $"{PREFIX}.remove";
        }
    }

    public static class Contract {
        private const string PREFIX = "contract";
        public const string QUERY = $"{PREFIX}.query";
        public const string QUERY_ALL = $"{PREFIX}.query.all";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        public const string IMPORT = $"{PREFIX}.import";
        public const string EXPORT_IMPORTANT = $"{PREFIX}.export.important";
        public const string IMPORTANT_STATISTICS = $"{PREFIX}.important.statistics.query";
        public const string STATISTICS = $"{PREFIX}.statistics.query";
        public const string TRANSFORM_CONSTRUCT = $"{PREFIX}.transform.construct";
    }
    
    public static class Construct {
        private const string PREFIX = "construct";
        public const string QUERY = $"{PREFIX}.query";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";
        
        public const string IMPORT = $"{PREFIX}.import";
        public const string EXPORT = $"{PREFIX}.export";
        public const string EXPORT_ADDRESS = $"{PREFIX}.export.address";
        public const string EXPORT_INDUSTRY = $"{PREFIX}.export.industry";
        public const string EXPORT_PROFESSION = $"{PREFIX}.export.profession";
        public const string TRANSFORM = $"{PREFIX}.transform";
    }

    public static class AnnualInvestPlan {
        private const string PREFIX = "annualInvestPlan";
        public const string QUERY = $"{PREFIX}.query";
        public const string ADD = $"{PREFIX}.add";
        public const string EDIT = $"{PREFIX}.edit";
        public const string REMOVE = $"{PREFIX}.remove";

        public static class Construct {
            private const string PREFIX = "annualInvestPlan.construct";
            public const string QUERY = $"{PREFIX}.query";
            public const string ADD = $"{PREFIX}.add";
            public const string EDIT = $"{PREFIX}.edit";
            public const string REMOVE = $"{PREFIX}.remove";
        }
    } 
}