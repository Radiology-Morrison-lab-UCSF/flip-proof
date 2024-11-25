namespace FlipProof.Base;

public interface ILogger
{
   /// <summary>
   /// Logs an info message
   /// </summary>
   /// <param name="message"></param>
   void Info(string message);
   
}

public interface ILogger<TLogEvent> : ILogger
{
   void Warn(TLogEvent @event, string message, string prefix="WARNING: ");

#if DEBUG
   void Debug(TLogEvent @event, string message, string prefix = "DEBUG: ") => Warn(@event, message, prefix);
#endif

   void WarnIfTrue(bool test, TLogEvent @event, string message, string prefix = "WARNING: ")
   {
      if (test)
      {
         Warn(@event, message, prefix);
      }
   }
   
   void WarnIfFalse(bool test, TLogEvent @event, string message, string prefix = "WARNING: ") 
      => WarnIfTrue(!test, @event, message, prefix);
}