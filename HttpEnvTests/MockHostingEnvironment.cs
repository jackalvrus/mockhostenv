using System;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;

namespace HttpEnvTests
{
    class MockHostingEnvironment : IDisposable
    {
        private FieldInfo _environmentSingletonField;

        public MockHostingEnvironment(string urlPath, string localPath)
        {
            if (HostingEnvironment.IsHosted) throw new InvalidOperationException("This is only for unit testing");

            new HostingEnvironment();

            _environmentSingletonField = typeof(HostingEnvironment)
                .GetField("_theHostingEnvironment", BindingFlags.Static | BindingFlags.NonPublic);

            var hostingEnvironment = (HostingEnvironment)_environmentSingletonField.GetValue(null);

            var vpType = typeof(HostingEnvironment).Assembly.GetTypes()
                .Where(t => t.Name == "VirtualPath")
                .FirstOrDefault();
            var ctor = vpType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetParameters().Count() == 1)
                .FirstOrDefault();
            var virtualPath = ctor.Invoke(new[] { urlPath });
            SetPrivateField(hostingEnvironment, "_appVirtualPath", virtualPath);
            SetPrivateField(hostingEnvironment, "_appPhysicalPath", localPath);
        }

        private void SetPrivateField<T>(T instance, string fieldName, object value)
        {
            var fi = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(instance, value);
        }

        public void Dispose()
        {
            _environmentSingletonField.SetValue(null, null);
        }
    }
}
