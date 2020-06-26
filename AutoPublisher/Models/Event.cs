using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPublisher
{
    public partial class Event
    {
        public string ObjectKind { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public string Ref { get; set; }
        public string CheckoutSha { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserUsername { get; set; }
        public string UserEmail { get; set; }
        public Uri UserAvatar { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public Repository Repository { get; set; }
        public List<Commit> Commits { get; set; }
        public long TotalCommitsCount { get; set; }
    }

    public partial class Commit
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public Uri Url { get; set; }
        public Author Author { get; set; }
        public List<string> Added { get; set; }
        public List<string> Modified { get; set; }
        public List<object> Removed { get; set; }
    }

    public partial class Author
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public partial class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri WebUrl { get; set; }
        public object AvatarUrl { get; set; }
        public string GitSshUrl { get; set; }
        public Uri GitHttpUrl { get; set; }
        public string Namespace { get; set; }
        public long VisibilityLevel { get; set; }
        public string PathWithNamespace { get; set; }
        public string DefaultBranch { get; set; }
        public Uri Homepage { get; set; }
        public string Url { get; set; }
        public string SshUrl { get; set; }
        public Uri HttpUrl { get; set; }
    }

    public partial class Repository
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Uri Homepage { get; set; }
        public Uri GitHttpUrl { get; set; }
        public string GitSshUrl { get; set; }
        public long VisibilityLevel { get; set; }
    }
}
