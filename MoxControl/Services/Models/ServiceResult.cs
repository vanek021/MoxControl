namespace MoxControl.Services.Models
{
    public class ServiceResult<T> where T : class
    {
        public ServiceResult(T result)
        {
            RuleViolations = new List<RuleViolation>();
            Result = result;
        }
        public ServiceResult()
        {
            RuleViolations = new List<RuleViolation>();
        }
        public List<RuleViolation> RuleViolations
        {
            get;
            private set;
        }
        public void AddRuleViolation(string message)
        {
            RuleViolations.Add(new RuleViolation(string.Empty, message));
        }
        public void AddRuleViolation(string name, string message)
        {
            RuleViolations.Add(new RuleViolation(name, message));
        }
        public bool HasViolation
        {
            get { return RuleViolations != null && RuleViolations.Count > 0; }
        }

        public string ErrorMessage
        {
            get
            {
                string msg = string.Empty;
                if (RuleViolations != null && RuleViolations.Count > 0)
                {
                    foreach (RuleViolation item in RuleViolations)
                    {
                        if (msg != string.Empty)
                            msg += "\r\n";
                        msg += item.ErrorMessage;
                    }
                }
                return msg;
            }
        }
        public T Result { get; set; }
    }

    public class RuleViolation
    {
        public RuleViolation(string parameterName, string errorMessage)
        {
            ParameterName = parameterName;
            ErrorMessage = errorMessage;
        }
        public string ParameterName
        {
            get;
            private set;
        }
        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
