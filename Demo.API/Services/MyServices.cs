namespace Demo.API.Services
{
    public class FatherService: IFatherService
    {
        public static int InstanceCount { get; private set; }
        private readonly IChildService _childService;
        public FatherService(IChildService childService)
        {
            _childService = childService;
            InstanceCount++;
        }
    }
    public class MotherService: IMotherService
    {
        public static int InstanceCount { get; private set; }
        private readonly IChildService _childService;
        public MotherService(IChildService childService)
        {
            _childService = childService;
            InstanceCount++;
        }
    }
    public class ChildService: IChildService
    {
        public static int InstanceCount { get; private set; }
        public ChildService()
        {
            InstanceCount++;
        }
    }

    public interface IFatherService
    {
        static int InstanceCount { get; private set; }
    }

    public interface IMotherService
    {
        static int InstanceCount { get; private set; }

    }
    public interface IChildService
    {
        static int InstanceCount { get; private set; }
    }
}
